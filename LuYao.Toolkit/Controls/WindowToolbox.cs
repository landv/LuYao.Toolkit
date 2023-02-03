using CommunityToolkit.Mvvm.Input;
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

namespace LuYao.Toolkit.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LuYao.Toolkit.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LuYao.Toolkit.Controls;assembly=LuYao.Toolkit.Controls"
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
    ///     <MyNamespace:WindowToolbox/>
    ///
    /// </summary>
    public class WindowToolbox : Control
    {
        static WindowToolbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowToolbox), new FrameworkPropertyMetadata(typeof(WindowToolbox)));
        }

        public WindowToolbox()
        {
            this.TopmostCommand = new RelayCommand(this.OnTopmostClick);
            this.MinimizeCommand = new RelayCommand(this.OnMinimizeClick);
            this.MaximizeCommand = new RelayCommand(this.OnMaximizeClick);
            this.CloseCommand = new RelayCommand(this.OnCloseClick);
        }
        public bool MaximizeBox
        {
            get { return (bool)GetValue(MaximizeBoxProperty); }
            set { SetValue(MaximizeBoxProperty, value); }
        }

        public static readonly DependencyProperty MaximizeBoxProperty = DependencyProperty.Register(nameof(MaximizeBox), typeof(bool), typeof(WindowToolbox), new PropertyMetadata(true));


        public bool MinimizeBox
        {
            get { return (bool)GetValue(MinimizeBoxProperty); }
            set { SetValue(MinimizeBoxProperty, value); }
        }

        public static readonly DependencyProperty MinimizeBoxProperty =
            DependencyProperty.Register("MinimizeBox", typeof(bool), typeof(WindowToolbox), new PropertyMetadata(true));



        public bool TopmostBox
        {
            get { return (bool)GetValue(TopmostBoxProperty); }
            set { SetValue(TopmostBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopmostBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopmostBoxProperty =
            DependencyProperty.Register("TopmostBox", typeof(bool), typeof(WindowToolbox), new PropertyMetadata(true));

        private void OnTopmostClick()
        {
            var win = Window.GetWindow(this);
            win.Topmost = !win.Topmost;
        }
        private void OnMinimizeClick()
        {
            var win = Window.GetWindow(this);
            win.WindowState = WindowState.Minimized;
        }
        private void OnMaximizeClick()
        {
            var win = Window.GetWindow(this);
            if(win.WindowState == WindowState.Maximized)
            {
                win.WindowState = WindowState.Normal;
            }
            else
            {
                win.WindowState = WindowState.Maximized;
            }
        }
        private void OnCloseClick()
        {
            var win = Window.GetWindow(this);
            win.Close();
        }


        public ICommand TopmostCommand
        {
            get { return (ICommand)GetValue(TopmostCommandProperty); }
            set { SetValue(TopmostCommandProperty, value); }
        }

        public static readonly DependencyProperty TopmostCommandProperty = DependencyProperty.Register("TopmostCommand", typeof(ICommand), typeof(WindowToolbox), new PropertyMetadata(null));



        public ICommand MinimizeCommand
        {
            get { return (ICommand)GetValue(MinimizeCommandProperty); }
            set { SetValue(MinimizeCommandProperty, value); }
        }

        public static readonly DependencyProperty MinimizeCommandProperty =
            DependencyProperty.Register("MinimizeCommand", typeof(ICommand), typeof(WindowToolbox), new PropertyMetadata(null));



        public ICommand MaximizeCommand
        {
            get { return (ICommand)GetValue(MaximizeCommandProperty); }
            set { SetValue(MaximizeCommandProperty, value); }
        }

        public static readonly DependencyProperty MaximizeCommandProperty =
            DependencyProperty.Register("MaximizeCommand", typeof(ICommand), typeof(WindowToolbox), new PropertyMetadata(null));



        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(WindowToolbox), new PropertyMetadata(null));



    }
}
