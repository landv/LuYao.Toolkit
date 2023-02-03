using AxMSTSCLib;
using System;
using System.Windows.Forms;

namespace LuYao.Toolkit.Rdm;
public static class MsRdpClientFactory
{
    private class MsRdpClient5 : AxMsRdpClient5, IMsRdpClient
    {
        public MsRdpClient5()
        {
            Name = nameof(MsRdpClient5);
        }
        public Control Control => this;

        public void Attach(IMsRdpClientHandler handler)
        {
            OnConnected += handler.OnConnected;
            OnDisconnected += handler.OnDisconnected;
            OnLoginComplete += handler.OnLoginComplete;
            OnConnecting += handler.OnConnecting;
        }

        public void Detach(IMsRdpClientHandler handler)
        {
            OnConnected -= handler.OnConnected;
            OnDisconnected -= handler.OnDisconnected;
            OnLoginComplete -= handler.OnLoginComplete;
            OnConnecting -= handler.OnConnecting;
        }

        public void Update(IRdpConnection desktop)
        {
            AdvancedSettings6.BitmapPersistence = Convert.ToInt32(desktop.BitmapCaching);
            ColorDepth = (int)desktop.ColorDepth;
            FullScreenTitle = desktop.Name + " - " + desktop.Host;
            AdvancedSettings6.ConnectToServerConsole = desktop.ConnectToConsole;
            Server = desktop.Host.Trim();

            if (!string.IsNullOrWhiteSpace(desktop.Username))
            {
                UserName = desktop.Username.Trim();
                if (!string.IsNullOrWhiteSpace(desktop.Password))
                {
                    AdvancedSettings6.ClearTextPassword = desktop.Password;
                }
            }
            if (!string.IsNullOrWhiteSpace(desktop.Domain)) Domain = desktop.Domain.Trim();

            AdvancedSettings6.RDPPort = desktop.Port;
            SecuredSettings2.AudioRedirectionMode = Convert.ToInt32(desktop.AudioSetting);
            SecuredSettings2.KeyboardHookMode = Convert.ToInt32(desktop.KeyboardSetting);
            AdvancedSettings6.RedirectDrives = desktop.RedirectDisks;
            AdvancedSettings6.RedirectPorts = desktop.RedirectPorts;
            AdvancedSettings6.RedirectPrinters = desktop.RedirectPrinters;
            AdvancedSettings6.RedirectSmartCards = desktop.RedirectSmartCards;
            AdvancedSettings6.SmartSizing = desktop.SmartSizing;
            AdvancedSettings6.allowBackgroundInput = 1;
            AdvancedSettings6.PinConnectionBar = true;
            AdvancedSettings6.DisplayConnectionBar = true;
            AdvancedSettings6.ContainerHandledFullScreen = 0;
            ConnectingText = "Connecting to " + desktop.Name + "...";
            DisconnectedText = "Disconnected from " + desktop.Name;
            int num = 0;
            if (!desktop.AllowWallpaper)
            {
                num++;
            }
            if (!desktop.AllowContents)
            {
                num += 2;
            }
            if (!desktop.AllowAnimation)
            {
                num += 4;
            }
            if (!desktop.AllowThemes)
            {
                num += 8;
            }
            AdvancedSettings6.PerformanceFlags = num;
            AdvancedSettings6.AuthenticationLevel = (uint)desktop.AuthenticationLevel;
            if (!desktop.AutoExpand)
            {
                DesktopHeight = desktop.DisplayHeight;
                DesktopWidth = desktop.DisplayWidth;
            }
            else
            {
                DesktopHeight = Height;
                DesktopWidth = Width;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Fix for the missing focus issue on the rdp client component
            if (m.Msg == 0x0021) // WM_MOUSEACTIVATE
            {
                if (!ContainsFocus)
                {
                    Focus();
                }
            }

            base.WndProc(ref m);
        }
    }
    private class MsRdpClient6 : AxMsRdpClient6, IMsRdpClient
    {
        public Control Control => this;
        public void Attach(IMsRdpClientHandler handler)
        {
            OnConnected += handler.OnConnected;
            OnDisconnected += handler.OnDisconnected;
            OnLoginComplete += handler.OnLoginComplete;
            OnConnecting += handler.OnConnecting;
        }

        public void Detach(IMsRdpClientHandler handler)
        {
            OnConnected -= handler.OnConnected;
            OnDisconnected -= handler.OnDisconnected;
            OnLoginComplete -= handler.OnLoginComplete;
            OnConnecting -= handler.OnConnecting;
        }

        public void Update(IRdpConnection connection)
        {

            AdvancedSettings7.BitmapPersistence = Convert.ToInt32(connection.BitmapCaching);
            ColorDepth = (int)connection.ColorDepth;
            FullScreenTitle = connection.Name + " - " + connection.Host;
            AdvancedSettings7.ConnectToServerConsole = connection.ConnectToConsole;
            AdvancedSettings7.ConnectToAdministerServer = connection.ConnectToConsole;
            Server = connection.Host.Trim();

            if (!string.IsNullOrWhiteSpace(connection.Username))
            {
                UserName = connection.Username.Trim();
                if (!string.IsNullOrWhiteSpace(connection.Password))
                {
                    AdvancedSettings6.ClearTextPassword = connection.Password;
                }
            }
            if (!string.IsNullOrWhiteSpace(connection.Domain)) Domain = connection.Domain.Trim();

            AdvancedSettings7.RDPPort = connection.Port;
            SecuredSettings2.AudioRedirectionMode = Convert.ToInt32(connection.AudioSetting);
            SecuredSettings2.KeyboardHookMode = Convert.ToInt32(connection.KeyboardSetting);
            AdvancedSettings7.RedirectDrives = connection.RedirectDisks;
            AdvancedSettings7.RedirectPorts = connection.RedirectPorts;
            AdvancedSettings7.RedirectPrinters = connection.RedirectPrinters;
            AdvancedSettings7.RedirectSmartCards = connection.RedirectSmartCards;
            AdvancedSettings7.SmartSizing = connection.SmartSizing;
            AdvancedSettings7.allowBackgroundInput = 1;
            AdvancedSettings7.PinConnectionBar = true;
            AdvancedSettings7.DisplayConnectionBar = true;
            AdvancedSettings7.ContainerHandledFullScreen = 0;
            ConnectingText = "Connecting to " + connection.Name + "...";
            DisconnectedText = "Disconnected from " + connection.Name;
            int num = 0;
            if (!connection.AllowWallpaper)
            {
                num++;
            }
            if (!connection.AllowContents)
            {
                num += 2;
            }
            if (!connection.AllowAnimation)
            {
                num += 4;
            }
            if (!connection.AllowThemes)
            {
                num += 8;
            }
            AdvancedSettings7.PerformanceFlags = num;
            AdvancedSettings7.AuthenticationLevel = (uint)connection.AuthenticationLevel;
            AdvancedSettings7.EnableCredSspSupport = connection.EnableCredSspSupport;
            if (!connection.AutoExpand)
            {
                DesktopHeight = connection.DisplayHeight;
                DesktopWidth = connection.DisplayWidth;
            }
            else
            {
                DesktopHeight = Height;
                DesktopWidth = Width;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Fix for the missing focus issue on the rdp client component
            if (m.Msg == 0x0021) // WM_MOUSEACTIVATE
            {
                if (!ContainsFocus)
                {
                    Focus();
                }
            }

            base.WndProc(ref m);
        }
    }
    private class MsRdpClient7 : AxMsRdpClient7, IMsRdpClient
    {
        public Control Control => this;
        public void Attach(IMsRdpClientHandler handler)
        {
            OnConnected += handler.OnConnected;
            OnDisconnected += handler.OnDisconnected;
            OnLoginComplete += handler.OnLoginComplete;
            OnConnecting += handler.OnConnecting;
        }

        public void Detach(IMsRdpClientHandler handler)
        {
            OnConnected -= handler.OnConnected;
            OnDisconnected -= handler.OnDisconnected;
            OnLoginComplete -= handler.OnLoginComplete;
            OnConnecting -= handler.OnConnecting;
        }

        public void Update(IRdpConnection desktop)
        {
            AdvancedSettings8.BitmapPersistence = Convert.ToInt32(desktop.BitmapCaching);
            ColorDepth = (int)desktop.ColorDepth;
            FullScreenTitle = desktop.Name + " - " + desktop.Host;
            AdvancedSettings8.ConnectToServerConsole = desktop.ConnectToConsole;
            AdvancedSettings8.ConnectToAdministerServer = desktop.ConnectToConsole;
            Server = desktop.Host.Trim();

            if (!string.IsNullOrWhiteSpace(desktop.Username))
            {
                UserName = desktop.Username.Trim();
                if (!string.IsNullOrWhiteSpace(desktop.Password))
                {
                    AdvancedSettings6.ClearTextPassword = desktop.Password;
                }
            }
            if (!string.IsNullOrWhiteSpace(desktop.Domain)) Domain = desktop.Domain.Trim();

            AdvancedSettings8.RDPPort = desktop.Port;
            SecuredSettings3.AudioRedirectionMode = Convert.ToInt32(desktop.AudioSetting);
            SecuredSettings3.KeyboardHookMode = Convert.ToInt32(desktop.KeyboardSetting);
            AdvancedSettings8.RedirectDrives = desktop.RedirectDisks;
            AdvancedSettings8.RedirectPorts = desktop.RedirectPorts;
            AdvancedSettings8.RedirectPrinters = desktop.RedirectPrinters;
            AdvancedSettings8.RedirectSmartCards = desktop.RedirectSmartCards;
            AdvancedSettings8.SmartSizing = desktop.SmartSizing;
            AdvancedSettings8.allowBackgroundInput = 1;
            AdvancedSettings8.PinConnectionBar = true;
            AdvancedSettings8.DisplayConnectionBar = true;
            AdvancedSettings8.ContainerHandledFullScreen = 1;
            ConnectingText = "Connecting to " + desktop.Name + "...";
            DisconnectedText = "Disconnected from " + desktop.Name;
            int num = 0;
            if (!desktop.AllowWallpaper)
            {
                num++;
            }
            if (!desktop.AllowContents)
            {
                num += 2;
            }
            if (!desktop.AllowAnimation)
            {
                num += 4;
            }
            if (!desktop.AllowThemes)
            {
                num += 8;
            }
            AdvancedSettings8.PerformanceFlags = num;
            AdvancedSettings8.AuthenticationLevel = (uint)desktop.AuthenticationLevel;
            AdvancedSettings8.EnableCredSspSupport = desktop.EnableCredSspSupport;

            if (!desktop.AutoExpand)
            {
                DesktopHeight = desktop.DisplayHeight;
                DesktopWidth = desktop.DisplayWidth;
            }
            else
            {
                DesktopHeight = Height;
                DesktopWidth = Width;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Fix for the missing focus issue on the rdp client component
            if (m.Msg == 0x0021) // WM_MOUSEACTIVATE
            {
                if (!ContainsFocus)
                {
                    Focus();
                }
            }

            base.WndProc(ref m);
        }
    }
    private class MsRdpClient8 : AxMsRdpClient8NotSafeForScripting, IMsRdpClient
    {
        public MsRdpClient8()
        {
        }
        public Control Control => this;
        public void Attach(IMsRdpClientHandler handler)
        {
            OnConnected += handler.OnConnected;
            OnDisconnected += handler.OnDisconnected;
            OnLoginComplete += handler.OnLoginComplete;
            OnConnecting += handler.OnConnecting;
        }
        public void Detach(IMsRdpClientHandler handler)
        {
            OnConnected -= handler.OnConnected;
            OnDisconnected -= handler.OnDisconnected;
            OnLoginComplete -= handler.OnLoginComplete;
            OnConnecting -= handler.OnConnecting;
        }
        public void Update(IRdpConnection desktop)
        {
            AdvancedSettings9.BitmapPersistence = Convert.ToInt32(desktop.BitmapCaching);
            ColorDepth = (int)desktop.ColorDepth;
            FullScreenTitle = desktop.Name + " - " + desktop.Host;
            AdvancedSettings9.ConnectToServerConsole = desktop.ConnectToConsole;
            AdvancedSettings9.ConnectToAdministerServer = desktop.ConnectToConsole;
            Server = desktop.Host.Trim();

            if (!string.IsNullOrWhiteSpace(desktop.Username))
            {
                UserName = desktop.Username.Trim();
                if (!string.IsNullOrWhiteSpace(desktop.Password))
                {
                    AdvancedSettings6.ClearTextPassword = desktop.Password;
                }
            }
            if (!string.IsNullOrWhiteSpace(desktop.Domain)) Domain = desktop.Domain.Trim();

            AdvancedSettings9.RDPPort = desktop.Port;
            SecuredSettings3.AudioRedirectionMode = Convert.ToInt32(desktop.AudioSetting);
            SecuredSettings3.KeyboardHookMode = Convert.ToInt32(desktop.KeyboardSetting);
            AdvancedSettings9.RedirectDrives = desktop.RedirectDisks;
            AdvancedSettings9.RedirectPorts = desktop.RedirectPorts;
            AdvancedSettings9.RedirectPrinters = desktop.RedirectPrinters;
            AdvancedSettings9.RedirectSmartCards = desktop.RedirectSmartCards;
            AdvancedSettings9.SmartSizing = desktop.SmartSizing;
            AdvancedSettings9.allowBackgroundInput = 1;
            AdvancedSettings9.PinConnectionBar = true;
            AdvancedSettings9.DisplayConnectionBar = true;
            AdvancedSettings9.ContainerHandledFullScreen = 1;
            ConnectingText = "Connecting to " + desktop.Name + "...";
            DisconnectedText = "Disconnected from " + desktop.Name;
            int num = 0;
            if (!desktop.AllowWallpaper)
            {
                num++;
            }
            if (!desktop.AllowContents)
            {
                num += 2;
            }
            if (!desktop.AllowAnimation)
            {
                num += 4;
            }
            if (!desktop.AllowThemes)
            {
                num += 8;
            }
            AdvancedSettings8.PerformanceFlags = num;
            AdvancedSettings8.AuthenticationLevel = (uint)desktop.AuthenticationLevel;
            AdvancedSettings8.EnableCredSspSupport = desktop.EnableCredSspSupport;

            if (!desktop.AutoExpand)
            {
                DesktopHeight = desktop.DisplayHeight;
                DesktopWidth = desktop.DisplayWidth;
            }
            else
            {
                DesktopHeight = Height;
                DesktopWidth = Width;
            }
        }
        protected override void WndProc(ref Message m)
        {
            // Fix for the missing focus issue on the rdp client component
            if (m.Msg == 0x0021) // WM_MOUSEACTIVATE
            {
                if (!ContainsFocus)
                {
                    Focus();
                }
            }

            base.WndProc(ref m);
        }
    }
    public static IMsRdpClient Create(int? version)
    {
        switch (version)
        {
            case 5: return new MsRdpClient5();
            case 6: return new MsRdpClient6();
            case 7: return new MsRdpClient7();
            case 8: return new MsRdpClient8();
        }
        return null;
    }
}

