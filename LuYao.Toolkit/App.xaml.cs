
using FluentValidation;
using LuYao.IO.Updating;
using LuYao.Toolkit.Behaviors;
using LuYao.Toolkit.Controls.AvalonEdit;
using LuYao.Toolkit.Dialogs;
using LuYao.Toolkit.Entities;
using LuYao.Toolkit.Rdm.Dialogs;
using LuYao.Toolkit.Regions;
using LuYao.Toolkit.Services;
using LuYao.Toolkit.Themes;
using LuYao.Toolkit.Update;
using LuYao.Toolkit.Views;
using NewLife.Log;
using NewLife.Reflection;
using OfficeOpenXml;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace LuYao.Toolkit;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    static App()
    {
        AssemblyX.ResolveFilter = ResolveFilter;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public App()
    {
        ProcessManager.GetProcessLock();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var splashScreen = new SplashScreen("Resources/Toolbox.png");
        splashScreen.Show(true);
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Task.Run(ChannelItemSession.FindCount);
        HighlightingHelper.RegisterHighlighting();
        ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
        base.OnStartup(e);
        Task.Run(Clear);
    }
    private static void Clear()
    {
        UpdatePackageHelper.Clear(AppDomain.CurrentDomain.BaseDirectory);
    }

    private static void ThemeManager_ThemeChanged(object sender, ThemeMode e)
    {
        HighlightingHelper.RegisterHighlighting();
    }

    private static bool ResolveFilter(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        if (name.Contains(".resources,")) return false;
        return true;
    }
    private static void ShowError(string msg)
    {
        Services.NotifyService.Warning(msg);
    }
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            XTrace.WriteLine("CurrentDomain_UnhandledException");
            XTrace.WriteException(ex);
            ShowError(ex.Message);
        }
    }

    private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        XTrace.WriteLine("TaskScheduler_UnobservedTaskException");
        XTrace.WriteException(e.Exception);
        ShowError(e.Exception.Message);
        e.SetObserved();
    }

    private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        XTrace.WriteLine("App_DispatcherUnhandledException");
        XTrace.WriteException(e.Exception);
        if (e.Exception is ValidationException v)
        {
            Services.MessageBoxService.Alert(v.Message, "验证失败");
        }
        else
        {
            ShowError(e.Exception.Message);
        }
        e.Handled = true;
    }
    protected override Window CreateShell()
    {
        return this.Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        AppRegisterTypes.RegisterViews(containerRegistry);

        containerRegistry.RegisterDialogWindow<DialogWindow>();
        containerRegistry.RegisterDialogWindow<MultiboxingDialogWindow>(nameof(MultiboxingDialogWindow));
        containerRegistry.RegisterDialog<MultiboxingDialog, MultiboxingDialogViewModel>();

        containerRegistry.RegisterDialog<RdpConnectionDetailDialog, RdpConnectionDetailViewModel>(nameof(RdpConnectionDetailDialog));
        containerRegistry.RegisterDialog<RdmSettingDialog, RdmSettingViewModel>(nameof(RdmSettingDialog));
        containerRegistry.RegisterDialog<RdpGroupDetailDialog, RdpGroupDetailViewModel>(nameof(RdpGroupDetailDialog));
        containerRegistry.RegisterDialog<FindNewVersionDialog, FindNewVersionDialogViewModel>(nameof(FindNewVersionDialog));
        containerRegistry.RegisterDialog<PreviewFileAsStringDialog, PreviewFileAsStringDialogViewModel>(nameof(PreviewFileAsStringDialog));
        containerRegistry.RegisterDialog<ToolkitSettingsDialog, ToolkitSettingsDialogViewModel>(nameof(ToolkitSettingsDialog));
    }

    protected override void Initialize()
    {
        base.Initialize();
        var mgr = this.Container.Resolve<Prism.Regions.IRegionManager>();

        if (ChannelItemSession.HasSessin())
        {
            mgr.RegisterViewWithRegion(RegionNames.MainRegion, ViewNames.Tabs.Session.Index);
        }
        else
        {
            mgr.RegisterViewWithRegion(RegionNames.MainRegion, ViewNames.Tabs.Explorer.Index);
        }


        ServiceProviderContainer.SetProvider(new ServiceProvider());
    }

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
        ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
            static (view) => AppHelper.ViewModelTypeResolver(view, typeof(ViewModelBase).Assembly, view.Assembly)
        );
    }

    protected override void OnExit(ExitEventArgs e)
    {
        ViewStates.ViewStateBag.Flush();
        base.OnExit(e);
    }
}
