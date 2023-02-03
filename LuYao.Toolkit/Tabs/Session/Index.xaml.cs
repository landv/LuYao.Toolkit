using LuYao.Toolkit.Events;
using LuYao.Toolkit.Views;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Windows.Controls;

namespace LuYao.Toolkit.Tabs.Session
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    [ViewName(ViewNames.Tabs.Session.Index)]
    public partial class Index : UserControl
    {
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        private IDialogService _dialogService;
        public Index(IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _regionManager = regionManager;
            _dialogService = dialogService;
            InitializeComponent();
            _eventAggregator.GetEvent<OpenFunctionItemEvent>().Subscribe(this.OnOpenFunctionItem);
        }

        private void OnOpenFunctionItem(OpenFunctionItemEventPayload item)
        {
            if (item.IsMultiboxing)
            {
                //不支持多开
                if (item.Item.Multiboxing == false) return;
                var p = new DialogParameters { };
                p.Add("Function", item.Item);
                _dialogService.Show(nameof(Dialogs.MultiboxingDialog), p, (result) => { }, nameof(Dialogs.MultiboxingDialogWindow));
                return;
            }
            this.txtTitle.Text = item.Item.Title;
            if (item.IsNewSession == false)
            {
                if (_regionManager.Regions.ContainsRegionWithName(Regions.RegionNames.WorkingRegion))
                {
                    _regionManager.RequestNavigate(Regions.RegionNames.WorkingRegion, item.Item.View);
                }
                else
                {
                    _regionManager.RegisterViewWithRegion(Regions.RegionNames.WorkingRegion, item.Item.View);
                }
            }
        }

        private void txtKeyword_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    Move(-1);
                    break;
                case System.Windows.Input.Key.Down:
                    Move(1);
                    break;
                default:
                    if (this.lbSuggestions.Items.Count > 0 && this.lbSuggestions.SelectedIndex < 0) this.lbSuggestions.SelectedIndex = 0;
                    break;
            }
        }
        private void Move(int i)
        {
            if (this.lbSuggestions.Visibility != System.Windows.Visibility.Visible) return;
            var count = this.lbSuggestions.Items.Count;
            var idx = this.lbSuggestions.SelectedIndex;
            switch (i)
            {
                case 1:
                    idx++;
                    break;
                case -1:
                    idx--;
                    break;
            }
            if (idx >= 0 && idx < count) this.lbSuggestions.SelectedIndex = idx;
        }
    }
}
