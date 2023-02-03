using LuYao.Toolkit.Helpers;
using LuYao.Toolkit.Rdm;
using NewLife;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LuYao.Toolkit.Rdm.Dialogs
{
    /// <summary>
    /// RdoConnectionDetail.xaml 的交互逻辑
    /// </summary>
    public partial class RdpConnectionDetailDialog : UserControl
    {
        public RdpConnectionDetailDialog()
        {
            InitializeComponent();
            ComboBoxHelper.BindEnum<DesktopSize>(DesktopSizeComboBox);
            ComboBoxHelper.BindEnum<ColorDepth>(ColorDepthComboBox);
            ComboBoxHelper.BindEnum<AudioRedirection>(AudioRedirectionComboBox);
            ComboBoxHelper.BindEnum<KeyboardRedirection>(KeyboardRedirectionComboBox);
            ComboBoxHelper.BindEnum<AuthenticationLevel>(AuthenticationLevelComboBox);
            DesktopSizeComboBox.SelectionChanged += DesktopSizeComboBox_SelectionChanged;
            NameTextBox.LostFocus += NameTextBox_OnLostFocus;
        }

        private void DesktopSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var current = (DesktopSize)(e.AddedItems[0] as dynamic).Value;
                if (current == DesktopSize.Custom)
                {
                    WidthNumericUpDown.IsEnabled = true;
                    HeightNumericUpDown.IsEnabled = true;
                }
                else
                {
                    WidthNumericUpDown.IsEnabled = false;
                    HeightNumericUpDown.IsEnabled = false;
                    var arr = current.GetDescription()
                        .Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
                    WidthNumericUpDown.Value = Convert.ToInt32(arr[0]);
                    HeightNumericUpDown.Value = Convert.ToInt32(arr[1]);
                }
            }
        }
        private void NameTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.NameTextBox.Text)) return;
            if (!string.IsNullOrWhiteSpace(this.HostTextBox.Text)) return;
            var host = this.NameTextBox.Text.Replace('：', ':').Trim();
            if (host.Contains(":"))
            {
                var arr = host.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2 && int.TryParse(arr[1], out var port) && port > 0 && port < 65535)
                {
                    host = arr[0];
                    this.PortNumericUpDown.Value = port;
                }
            }

            this.HostTextBox.Text = host;
        }
    }
}
