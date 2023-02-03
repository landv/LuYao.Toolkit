using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using LuYao.IO.Updating;
using LuYao.Toolkit.Behaviors;
using LuYao.Toolkit.Events;
using LuYao.Toolkit.IO;
using LuYao.Toolkit.Rdm.Events;
using LuYao.Toolkit.Themes;
using LuYao.Toolkit.Update;
using LuYao.Toolkit.Views;
using NewLife.Configuration;
using NewLife.Reflection;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuYao.Toolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ViewStates.IViewStateHost
    {
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        private IDialogService _dialogService;
        public MainWindow(IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            this._dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            InitializeComponent();
            _size = new Size(this.MinWidth, this.MinHeight);
            this.ViewSate = new ViewStates.ViewStateBag(this);
            _eventAggregator.GetEvent<Events.OpenTabEvent>().Subscribe(OnOpenTab);
            _eventAggregator.GetEvent<Events.OpenFunctionItemEvent>().Subscribe(OnOpenFunctionItem);
            _eventAggregator.GetEvent<Rdm.Events.OpenRdpConnectionEvent>().Subscribe(this.OnRdmOpenRdpConnection);
            _eventAggregator.GetEvent<Events.PreviewFileAsStringEvent>().Subscribe(OnPreviewFileAsStringEvent);
            _eventAggregator.GetEvent<Events.ThemeChangedEvent>().Subscribe(this.OnThemeChanged);
            this.Width = _size.Width;
            this.Height = _size.Height;
            this.EnsureFormFits();
            this.SizeChanged += MainWindow_SizeChanged;
            this.Closed += MainWindow_Closed;
            this.Loaded += MainWindow_Loaded;

            var cfg = ToolkitConfig.Current;
            var themeManager = ThemeManager.Current;
            if (cfg.Theme != themeManager.Theme) themeManager.Theme = cfg.Theme;
            if (cfg.CheckForUpdatesOnStartup) this.CheckUpdate();
        }

        private void OnThemeChanged(ThemeMode theme)
        {
            ThemeManager.Current.Theme = theme;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var ass = AssemblyX.Create(Assembly.GetExecutingAssembly());
            this.Title = $"路遥工具箱 v{ass.Version}";
            TempHelper.ClearTemp();
        }

        private void OnPreviewFileAsStringEvent(PreviewFileAsStringEventPayload payload)
        {
            _dialogService.Show(nameof(Dialogs.PreviewFileAsStringDialog), new DialogParameters { { "Payload", payload } }, result => { });
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void EnsureFormFits()
        {
            var workingArea = Screen.FromHandle(this.GetHandle()).WorkingArea;
            if (base.Width > workingArea.Width * 0.8)
            {
                base.Width = workingArea.Width * 0.8;
            }
            if (base.Height > workingArea.Height * 0.8)
            {
                base.Height = workingArea.Height * 0.8;
            }

            if (this.Left < 0) this.Left = 0;
            if (this.Top < 0) this.Top = 0;
        }

        [ViewStates.WatchViewState(nameof(WindowSize))]
        private Size _size;
        public Size WindowSize
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowSize)));
                }
            }
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.WindowSize = e.NewSize;
        }

        public ViewStates.ViewStateBag ViewSate { get; }
        public int InstanceId => 1;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnOpenFunctionItem(OpenFunctionItemEventPayload payload)
        {
            var item = payload.Item;
            Services.TongjiService.Tongji(item.View);
            if (payload.IsNewSession)
            {
                _regionManager.RequestNavigate(Regions.RegionNames.MainRegion, ViewNames.Tabs.Session.Index, (r) =>
                {
                    if (r.Result == true) _regionManager.RequestNavigate(Regions.RegionNames.WorkingRegion, item.View);
                });
            }
        }

        private void OnOpenTab(Tabs.Tab tab)
        {
            _regionManager.RequestNavigate(Regions.RegionNames.MainRegion, tab.View);
        }
        private void OnRdmOpenRdpConnection(OpenRdpConnectionEventPayload args)
        {
            var id = args.Id;
            _regionManager.RequestNavigate(Regions.RegionNames.MainRegion, ViewNames.Tabs.Rdp.Index, (r) =>
            {
                if (r.Result == true) Tabs.Rdp.Index.Open(id);
            });
        }
        private async void CheckUpdate()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            var config = UpdateConfig.Current;
            if (config.NextCheckUpdate <= DateTime.Now)
            {
                var package = await config.GetLastVersion();
                var root = AppDomain.CurrentDomain.BaseDirectory;
                if (!await package.HasUpdate(Assembly.GetExecutingAssembly(), root))
                {
                    return;
                }
                _dialogService.Show(nameof(Update.FindNewVersionDialog), new DialogParameters { { nameof(UpdatePackage), package } }, result =>
                {
                    if (result.Result == ButtonResult.Ignore)
                    {
                        config.ResetNextCheckTime();
                        config.Save();
                    }
                    else if (result.Result == ButtonResult.Yes)
                    {
                        Notification.Show(new Update.Update(package), ShowAnimation.None, true);
                    }
                });
            }
        }

        private void Settings_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            _dialogService.ShowDialog(nameof(Dialogs.ToolkitSettingsDialog));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

            e.Handled = true;
            _dialogService.ShowDialog(nameof(Dialogs.ToolkitSettingsDialog));
        }
    }
}
