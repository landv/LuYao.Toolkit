using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using UtfUnknown;

namespace LuYao.Toolkit.Channels.Files;

public partial class DetectFileEncodeingViewModel : ViewModelBase, IFileDragDropTarget
{
    [INotifyPropertyChanged]
    public partial class FileItem
    {
        public FileItem(FileInfo file)
        {
            this.Path = file.FullName;
            this.Name = file.Name;
            if (!file.Exists) return;
            this.Length = file.Length;
            this.ReadEncoding();
        }
        [ObservableProperty]
        private string _path;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        public string _encoding;
        [ObservableProperty]
        private long _length;
        private void ReadEncoding()
        {
            try
            {
                using FileStream stream = new FileStream(this.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var r = CharsetDetector.DetectFromStream(stream, 4096);
                if (r.Detected != null) this.Encoding = r.Detected.EncodingName;
            }
            catch (Exception)
            {
                Encoding = string.Empty;
            }
        }
    }

    private IEventAggregator _eventAggregator;

    public DetectFileEncodeingViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
    }

    [ObservableProperty]
    private ObservableCollection<FileItem> _files = new ObservableCollection<FileItem>();

    [RelayCommand]
    private void OpenFiles()
    {
        var dialog = Services.FileDialogService.CreateOpenFileDialog();
        dialog.Title = "编码识别";
        dialog.Multiselect = true;
        if (dialog.ShowDialog() != true) return;
        AddFiles(dialog.FileNames);

    }

    [RelayCommand]
    private void Clear()
    {
        this.Files.Clear();
    }
    private void AddFiles(string[] files)
    {
        if (files == null || files.Length == 0) return;
        Array.Reverse(files);
        foreach (var file in files)
        {
            var info = new FileInfo(file);
            var item = new FileItem(info);
            Files.Insert(0, item);
        }
    }

    public void OnFilesDropped(string group, string[] filepaths)
    {
        AddFiles(filepaths);
    }

    [RelayCommand]
    private void Preview(FileItem file)
    {
        var payload = new PreviewFileAsStringEventPayload(file.Path, Encoding.GetEncoding(file.Encoding));
        this._eventAggregator.GetEvent<PreviewFileAsStringEvent>().Publish(payload);
    }
}
