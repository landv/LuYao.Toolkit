using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using LuYao.Toolkit.Rdm;
using LuYao.Toolkit.Rdm.Events;
using LuYao.Toolkit.Services;
using NewLife.Configuration;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XCode;

namespace LuYao.Toolkit.Channels.Networks;

public partial class RemoteDesktopManagerViewModel : ViewModelBase
{
    [INotifyPropertyChanged]
    public partial class SearchInput
    {
        public SearchInput(RemoteDesktopManagerViewModel vm)
        {
            this.vm = vm ?? throw new ArgumentNullException(nameof(vm));
        }
        private RemoteDesktopManagerViewModel vm;
        [ObservableProperty]
        private RdpGroup _group = RdpGroup.All;

        [ObservableProperty]
        private string _keyword;
        partial void OnGroupChanged(RdpGroup value)
        {
            vm.Search();
        }
    }
    private readonly IEventAggregator eventAggregator;
    public RemoteDesktopManagerViewModel(IEventAggregator eventAggregator)
    {
        this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        this._input = new SearchInput(this);
        this.eventAggregator.GetEvent<RdpConnectionChangedEvent>().Subscribe(OnRdoConnectionChanged);
        this.eventAggregator.GetEvent<AfterRdmSettingEvent>().Subscribe(OnAfterRdmSetting);
        this.Reload();
    }

    private void OnAfterRdmSetting()
    {
        this.Reload();
    }

    private void OnRdoConnectionChanged()
    {
        this.Reload();
    }

    [ObservableProperty]
    private List<RdpGroup> _groups = new List<RdpGroup> { RdpGroup.All };

    [ObservableProperty]
    private SearchInput _input;

    [ObservableProperty]
    private List<RdpConnectionBase> _connections = new List<RdpConnectionBase>();

    [ObservableProperty]
    private IList _selectedConnections;

    [RelayCommand]
    private void Reload()
    {
        var groups = Entities.RdpGroup.FindAll(null, Entities.RdpGroup._.Rank, null, 0, 0);
        var list = new List<RdpGroup> { RdpGroup.All };
        foreach (var g in groups)
        {
            list.Add(new RdpGroup(g));
        }
        this.Groups = list;
        this.Search();
    }

    [RelayCommand]
    private void Search()
    {
        WhereExpression where = new WhereExpression();
        var order = Entities.RdpConnection._.GroupId.Asc() & Entities.RdpConnection._.Name.Asc();
        if (Input.Group != null && Input.Group.Id != Guid.Empty)
        {
            where &= Entities.RdpConnection._.GroupId == Input.Group.Id;
        }
        if (!string.IsNullOrWhiteSpace(Input.Keyword))
        {
            where &= (
                Entities.RdpConnection._.Name.Contains(Input.Keyword) |
                Entities.RdpConnection._.Host.Contains(Input.Keyword) |
                Entities.RdpConnection._.Username.Contains(Input.Keyword) |
                Entities.RdpConnection._.Remark.Contains(Input.Keyword)
                );
        }

        var list = Entities.RdpConnection.FindAll(where, order, null, 0, 0);
        var connections = new List<RdpConnectionBase>(list.Count);
        var groups = new SortedSet<string>();
        foreach (var item in list)
        {
            connections.Add(new RdpConnectionBase(item));
            if (!string.IsNullOrWhiteSpace(item.GroupName)) { groups.Add(item.GroupName); }
        }
        this.Connections = connections;
        if (groups.Count > 1)
        {
            foreach (var item in this.Connections)
            {
                if (string.IsNullOrWhiteSpace(item.GroupName)) item.GroupName = "默认";
            }
            var cvs = CollectionViewSource.GetDefaultView(this.Connections);
            cvs.GroupDescriptions.Add(new PropertyGroupDescription(nameof(RdpConnectionBase.GroupName)));
        }
    }

    [RelayCommand]
    private void Add()
    {
        this.eventAggregator.GetEvent<AddRdpConnectionEvent>().Publish();
    }

    [RelayCommand]
    private void OpenSetting()
    {
        this.eventAggregator.GetEvent<OpenRdmSettingEvent>().Publish();
    }

    [RelayCommand]
    private void Edit(RdpConnectionBase connection)
    {
        this.eventAggregator.GetEvent<EditRdpConnectionEvent>().Publish(new EditRdpConnectionEventPayload(connection.Id));
    }

    [RelayCommand]
    private void Open(RdpConnectionBase connection)
    {
        if (connection == null) return;
        this.eventAggregator.GetEvent<OpenRdpConnectionEvent>().Publish(new OpenRdpConnectionEventPayload(connection.Id));
    }

    [RelayCommand]
    private void Delete(IList connections)
    {
        if (connections.Count == 0)
        {
            NotifyService.Warning("请先选择要删除的数据");
            return;
        }
        var ids = connections.Cast<RdpConnectionBase>().Select(i => i.Id).ToArray();
        if (MessageBoxService.Confirm($"确定要删除 {ids.Length} 个链接吗？") == false) return;

        var count = Entities.RdpConnection.Delete(Entities.RdpConnection._.Id.In(ids));
        NotifyService.Success($"成功删除链接 {count} 个。");
        this.Reload();
    }

    [RelayCommand]
    private void Import()
    {
        var dialog = FileDialogService.CreateOpenFileDialog();
        dialog.Title = "导入远程桌面链接";
        dialog.Filter = "远程桌面链接备份|*.rdcsv";
        if (dialog.ShowDialog() == false) return;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null,
        };
        using (var reader = new StreamReader(dialog.FileName))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<RdpConnectionCsvMap>();
            var records = csv.GetRecords<RdpConnection>().ToList();
            if (records.Count <= 0)
            {
                NotifyService.Warning("要导入的数据为空");
                return;
            }

            var itemsForAdd = new List<Entities.RdpConnection>();
            var itemsForUpdate = new List<Entities.RdpConnection>();
            var itemsForSkip = new List<RdpConnection>();

            var groups = new Dictionary<string, Entities.RdpGroup>();

            var validator = RdpConnectionValidator.Instance;
            for (int i = 0; i < records.Count; i++)
            {
                RdpConnection item = records[i];
                var result = validator.Validate(item);
                if (result.IsValid == false)
                {
                    NotifyService.Warning($"第 {i + 1} 行数据有误。{result}");
                    return;
                }
                if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
                if (!string.IsNullOrWhiteSpace(item.GroupName))
                {
                    if (groups.ContainsKey(item.GroupName)) continue;
                    groups.Add(item.GroupName, null);
                }
            }

            if (groups.Count > 0)
            {
                var find = Entities.RdpGroup.FindAll(Entities.RdpGroup._.Name.In(groups.Keys));
                foreach (var name in groups.Keys.ToArray())
                {
                    var e = find.FirstOrDefault(i => i.Name == name);
                    if (e == null)
                    {
                        e = new Entities.RdpGroup
                        {
                            CreatedAt = DateTime.Now,
                            Name = name,
                            Rank = 0,
                            UpdatedAt = DateTime.Now,
                            Id = Guid.NewGuid()
                        };
                        e.Insert();
                    }
                    groups[name] = e;
                }
            }

            foreach (var item in records)
            {
                if (!string.IsNullOrWhiteSpace(item.GroupName) && groups.TryGetValue(item.GroupName, out var g)) item.GroupId = g.Id;
                else item.GroupId = Guid.Empty;

                var e = Entities.RdpConnection.FindById(item.Id);
                if (e != null)
                {
                    if (e.UpdatedAt >= item.UpdatedAt)
                    {
                        itemsForSkip.Add(item);
                    }
                    else
                    {
                        item.Write(e);
                        itemsForUpdate.Add(e);
                    }
                }
                else
                {
                    e = new Entities.RdpConnection { Id = Guid.NewGuid(), CreatedAt = DateTime.Now };
                    item.Write(e);
                    itemsForAdd.Add(e);
                }
            }

            itemsForAdd.Insert();
            itemsForUpdate.Update();

            NotifyService.Success($"导入成功！新增：{itemsForAdd.Count} 更新：{itemsForUpdate.Count} 跳过：{itemsForSkip.Count}");
            this.Reload();
        }
    }

    [RelayCommand]
    private void Export(IList connections)
    {

        if (connections.Count == 0)
        {
            NotifyService.Warning("请先选择要导出的数据");
            return;
        }
        Guid[] ids = (from RdpConnectionBase i in connections select i.Id).ToArray();
        IReadOnlyList<Entities.RdpConnection> list = Entities.RdpConnection.FindAllByIds(ids);
        if (connections.Count == 0)
        {
            NotifyService.Warning("要导出的数据为空");
            return;
        }
        ISaveFileDialog dialog = FileDialogService.CreateSaveFileDialog();
        dialog.Title = "导出远程桌面链接";
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dialog.Filter = "远程桌面链接备份|*.rdcsv";
        dialog.AddExtension = true;
        dialog.FileName = $"远程桌面_{DateTime.Now:yyyyMMdd_HHmmss}.rdcsv";
        if (!dialog.ShowDialog()) return;
        string fn = dialog.FileName;
        if (!fn.EndsWith(".rdcsv", StringComparison.InvariantCultureIgnoreCase)) fn += ".rdcsv";
        List<RdpConnection> exports = new List<RdpConnection>(list.Count);
        foreach (Entities.RdpConnection item in list) exports.Add(new RdpConnection(item));
        using (StreamWriter writer = new StreamWriter(fn, append: false, Encoding.UTF8))
        {
            using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<RdpConnectionCsvMap>();
            csv.WriteHeader<RdpConnection>();
            csv.NextRecord();
            foreach (RdpConnection record in exports)
            {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }
        NotifyService.Success($"数据导出成功,共 {exports.Count} 条.");
    }
}