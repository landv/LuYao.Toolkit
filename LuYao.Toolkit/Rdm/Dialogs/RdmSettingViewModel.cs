using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace LuYao.Toolkit.Rdm.Dialogs;

public partial class RdmSettingViewModel : ViewModelBase, IDialogAware
{
    private readonly IDialogService _dialogService;

    public RdmSettingViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    public string Title => "远程桌面设置";


    public event Action<IDialogResult> RequestClose;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        LoadGroups();
    }

    [ObservableProperty]
    private List<RdpGroup> _groups = new List<RdpGroup>();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditGroupCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteGroupCommand))]
    [NotifyCanExecuteChangedFor(nameof(UpGroupCommand))]
    [NotifyCanExecuteChangedFor(nameof(DownGroupCommand))]
    private RdpGroup _currentGroup = null;

    private void LoadGroups()
    {
        var groups = Entities.RdpGroup.GetAllGroups();
        var list = new List<RdpGroup>(groups.Count);
        foreach (var g in groups)
        {
            list.Add(new RdpGroup(g));
        }
        this.Groups = list;
    }
    [RelayCommand]
    private void AddGroup()
    {
        _dialogService.ShowDialog(nameof(RdpGroupDetailDialog), (r) =>
        {
            if (r.Result == ButtonResult.OK)
            {
                this.LoadGroups();
            }
        });
    }

    private bool CanEdit() => this.CurrentGroup != null;
    [RelayCommand(CanExecute = nameof(CanEdit))]
    private void EditGroup()
    {
        var p = new DialogParameters();
        p.Add("id", this.CurrentGroup.Id);
        _dialogService.ShowDialog(nameof(RdpGroupDetailDialog), p, (r) =>
        {
            if (r.Result == ButtonResult.OK)
            {
                this.LoadGroups();
            }
        });
    }

    private bool CanDelete() => this.CurrentGroup != null;
    [RelayCommand(CanExecute = nameof(CanDelete))]
    private void DeleteGroup()
    {
        if (this.CurrentGroup == null) return;
        if (Services.MessageBoxService.Confirm("是否确认删除此分组？") == false) return;
        var e = Entities.RdpGroup.FindById(this.CurrentGroup.Id);
        if (e != null) e.Delete();
        this.LoadGroups();
    }

    private bool CanUp()
    {
        if (this.CurrentGroup == null) return false;
        var id = this.CurrentGroup.Id;
        var all = Entities.RdpGroup.GetAllGroups();
        if (all.Count > 0 && all[0].Id != id) return true;
        return false;
    }
    [RelayCommand(CanExecute = nameof(CanUp))]
    private void UpGroup()
    {
        var all = Entities.RdpGroup.GetAllGroups();
        for (int i = 0; i < all.Count; i++)
        {
            all[i].Rank = i * 2;
            if (all[i].Id == this.CurrentGroup.Id)
            {
                all[i].Rank -= 3;
            }
        }
        all.Save();
        LoadGroups();
    }

    private bool CanDown()
    {
        if (this.CurrentGroup == null) return false;
        var id = this.CurrentGroup.Id;
        var all = Entities.RdpGroup.GetAllGroups();
        if (all.Count > 0 && all[all.Count - 1].Id != id) return true;
        return false;
    }
    [RelayCommand(CanExecute = nameof(CanDown))]
    private void DownGroup()
    {
        var all = Entities.RdpGroup.GetAllGroups();
        for (int i = 0; i < all.Count; i++)
        {
            all[i].Rank = i * 2;
            if (all[i].Id == this.CurrentGroup.Id)
            {
                all[i].Rank += 3;
            }
        }
        all.Save();
        LoadGroups();
    }
}
