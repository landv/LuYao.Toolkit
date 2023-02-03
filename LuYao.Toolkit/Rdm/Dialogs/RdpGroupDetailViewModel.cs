using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Rdm.Dialogs;

public partial class RdpGroupDetailViewModel : ViewModelBase, IDialogAware
{
    [ObservableProperty]
    private string _action = "添加";

    [ObservableProperty]
    private RdpGroup _group = new RdpGroup { };
    public string Title => $"{this.Action}分组";

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
        if (parameters.TryGetValue<Guid>("id", out var id))
        {
            var e = Entities.RdpGroup.FindById(id);
            if (e == null)
            {
                Close();
                return;
            }
            this.Group = new RdpGroup(e);
            this.Action = "编辑";
        }
        else
        {
            this.Action = "添加";
            this.Group = new RdpGroup { };
        }
    }

    [RelayCommand]
    private void Close()
    {
        this.RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    [RelayCommand]
    private void Save()
    {
        RdpGroupValidator.Instance.ValidateAndThrow(this.Group);
        Entities.RdpGroup group;
        if (this.Group.Id != Guid.Empty)
        {
            group = Entities.RdpGroup.FindById(this.Group.Id);
        }
        else
        {
            group = new Entities.RdpGroup { CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        }
        group.Name = this.Group.Name;
        group.UpdatedAt = DateTime.Now;
        if (group.Id == Guid.Empty)
        {
            group.Id = Guid.NewGuid();
            group.Insert();
        }
        else
        {
            group.Update();
        }
        this.RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }
}
