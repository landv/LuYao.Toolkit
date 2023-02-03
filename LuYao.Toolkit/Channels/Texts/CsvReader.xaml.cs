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

namespace LuYao.Toolkit.Channels.Texts
{
    /// <summary>
    /// CsvReader.xaml 的交互逻辑
    /// </summary>
    [Views.ViewName(Views.ViewNames.Channels.Texts.CsvReader)]
    public partial class CsvReader : UserControl
    {
        public CsvReader()
        {
            InitializeComponent();
        }

        private void MainDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column is DataGridBoundColumn dataGridBoundColumn)
            {
                if (e.PropertyName.Contains('.'))
                {
                    dataGridBoundColumn.Binding = new Binding("[" + e.PropertyName + "]");
                }
                dataGridBoundColumn.MaxWidth = 200;
            }
        }
    }
}
