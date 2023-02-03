using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace LuYao.Toolkit.Channels.Texts
{
    /// <summary>
    /// RegexEvaluator.xaml 的交互逻辑
    /// </summary>
    [Views.ViewName(Views.ViewNames.Channels.Texts.RegexEvaluator)]
    public partial class RegexEvaluator : UserControl
    {
        public RegexEvaluator()
        {
            InitializeComponent();
        }
        private void MatchesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is Capture c)
                {
                    //InputTextBox.Select(c.Index, c.Length);
                    break;
                }
            }
        }

        private void GroupDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is Capture c)
                {
                    //InputTextBox.Select(c.Index, c.Length);
                    break;
                }
            }
        }

        private void CaptureDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is Capture c)
                {
                    //InputTextBox.Select(c.Index, c.Length);
                    break;
                }
            }
        }
    }
}
