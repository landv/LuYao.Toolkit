using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using LuYao.Toolkit.Rdm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace LuYao.Toolkit.Rdm.Dialogs;

public partial class RdpConnectionDetailViewModel : ViewModelBase, IDialogAware
{

    public RdpConnectionDetailViewModel()
    {
    }
    public string Title { get; private set; }

    [ObservableProperty]
    private IList<RdpGroup> _groups;

    private RdpConnection _connection;

    public RdpConnection Connection
    {
        get => _connection;
        set => SetProperty(ref _connection, value);
    }

    public event Action<IDialogResult> RequestClose;

    [RelayCommand]
    void Save()
    {
        RdpConnectionValidator.Instance.ValidateAndThrow(this.Connection);
        if (this.Connection.Id == Guid.Empty)
        {
            var e = new Entities.RdpConnection
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
            };
            this.Connection.Write(e);
            e.Save();
        }
        else
        {
            var e = Entities.RdpConnection.FindById(this.Connection.Id);
            if (e == null) throw new Exception("数据未找到");
            e.UpdatedAt = DateTime.Now;

            this.Connection.Write(e);

            e.Save();
        }
        this.RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }
    [RelayCommand]
    void Cancel()
    {
        this.RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        //act=add : 不去数据库查询数据，标题设置成 新建
        //act=edit : 去数据库查一查数据，标题设置成 编辑
        if (parameters.TryGetValue<string>("act", out var act))
        {
            switch (act)
            {
                case "add":
                    this.Title = "添加远程桌面";
                    this.Connection = new RdpConnection
                    {
                        Port = 3389,
                        DesktopSize = DesktopSize.X1024Y768,
                        AutoExpand = true,
                        ColorDepth = ColorDepth.Depth16Bit,
                        AudioSetting = AudioRedirection.PlayNoSound,
                        KeyboardSetting = KeyboardRedirection.OnLocal,
                        BitmapCaching = true,
                        AuthenticationLevel = AuthenticationLevel.Connect,
                        EnableCredSspSupport = true,
                        Username = "administrator"
                    };
                    break;
                case "edit":
                    this.Title = "编辑远程桌面";
                    if (!parameters.TryGetValue<Guid>("id", out var id)) throw new Exception("id 未找到");
                    var e = Entities.RdpConnection.FindById(id);
                    if (e == null) throw new Exception("数据未找到");
                    this.Connection = new RdpConnection(e);
                    var g = Entities.RdpGroup.FindById(e.GroupId);
                    if (g == null) this.Connection.GroupId = Guid.Empty;
                    break;
                default:
                    throw new Exception($"未知的 action:{act}");
            }
        }
        var items = new List<RdpGroup> { RdpGroup.None };
        foreach (var item in Entities.RdpGroup.GetAllGroups())
        {
            items.Add(new RdpGroup(item));
        }
        this.Groups = items;
    }
}
