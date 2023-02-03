using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// DirectorySelector.xaml 的交互逻辑
    /// </summary>
    public partial class DirectorySelector : UserControl
    {
        public DirectorySelector()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register(
                "Path",
                typeof(string),
                typeof(DirectorySelector),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }



        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DirectorySelector), new PropertyMetadata(default(ICommand)));



        public string DialogTitle { get; set; }

        private void OpenDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDialog();
        }
        private void OpenDialog()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog { SelectedPath = Path, Multiselect = false };
            if (!string.IsNullOrWhiteSpace(DialogTitle))
            {
                dialog.Description = DialogTitle;
                dialog.UseDescriptionForTitle = true;
            }
            if (dialog.ShowDialog() == true)
            {
                Path = dialog.SelectedPath;
                var cmd = this.Command;
                if (cmd != null && cmd.CanExecute(this.Path))
                {
                    cmd.Execute(this.Path);
                }
            }
        }

        private void PathTextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (
                e.Data.GetDataPresent(DataFormats.FileDrop)
                && e.Data.GetData(DataFormats.FileDrop) is string[] dirs
                && dirs.Length == 1
            )
            {
                var path = dirs[0];
                var match = Directory.Exists(path);
                if (match)
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }

            e.Handled = true;
        }

        private void PathTextBox_PreviewDrop(object sender, DragEventArgs e)
        {

            e.Handled = true;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dirs && dirs.Length > 0)
            {
                Path = dirs[0];
            }
        }

        private void OpenDirectory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Path)) return;
            Process.Start("explorer", $"\"{Path}\"");
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Path)) return;
            Services.ClipboardService.CopyText(Path);
        }
    }
}
