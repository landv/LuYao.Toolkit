using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Interactivity;
using LuYao.Toolkit.Rdm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuYao.Toolkit.Tabs.Rdp.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LuYao.Toolkit.Tabs.Rdo.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LuYao.Toolkit.Tabs.Rdo.Controls;assembly=LuYao.Toolkit.Tabs.Rdo.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:RdpTabItemHeader/>
    ///
    /// </summary>
    public class RdpTabItemHeader : Control
    {
        static RdpTabItemHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RdpTabItemHeader), new FrameworkPropertyMetadata(typeof(RdpTabItemHeader)));
        }
        public RdpTabItemHeader(IRdpConnection connection, RdpSession session)
        {
            this.Connection = connection;
            this.RdpSession = session;
            this.RdpSession.StatusChanged += RdpSession_StatusChanged;
            this.ConnectCommand = new RelayCommand(this.Connect);
            this.DisconnectCommand = new RelayCommand(this.Disconnect);
        }

        private void RdpSession_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.Status = e.Status;
        }
        public IRdpConnection Connection { get; }
        public RdpSession RdpSession { get; }
        public RdpConnectStatus Status
        {
            get { return (RdpConnectStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(RdpConnectStatus), typeof(RdpTabItemHeader), new PropertyMetadata(RdpConnectStatus.Pending));
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        private void Connect()
        {
            this.RdpSession.Connect();
        }
        private void Disconnect()
        {
            this.RdpSession.Disconnect();
        }
    }
}
