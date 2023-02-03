using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Texts;

public partial class CsvReaderViewModel : ViewModelBase
{
    private const int PAGE_SIZE = 100;

    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Delimiter))]
    private string _delimiter = ",";
    public int MaxPageCount
    {
        get
        {
            if (this._total > 0)
            {
                var count = this._total / PAGE_SIZE;
                if (this._total % PAGE_SIZE > 0) count++;
                return count;
            }
            return 0;
        }
    }
    [ObservableProperty]
    private int _pageIndex = 1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaxPageCount))]
    private int _total = 0;

    [ObservableProperty]
    private DataTable _rows;

    private DataTable _table = new DataTable();
    partial void OnPageIndexChanged(int value)
    {
        DoPage(value);
    }

    private void DoPage(int value)
    {
        if (this.Total <= 0) return;
        var skip = (Math.Min(value, this.MaxPageCount) - 1) * PAGE_SIZE;

        var dt = this.Rows;
        dt.Clear();
        dt.BeginLoadData();
        for (int i = skip; i < this._table.Rows.Count && dt.Rows.Count < PAGE_SIZE; i++)
        {
            var row = this._table.Rows[i];
            dt.ImportRow(row);
        }
        dt.EndLoadData();
        //this.OnPropertyChanged(nameof(this.Rows));
    }

    [RelayCommand]
    private void Open()
    {
        var dialog = Services.FileDialogService.CreateOpenFileDialog();
        dialog.Title = "打开 CSV 文件";
        if (dialog.ShowDialog())
        {
            this.FileName = dialog.FileName;
            Read(dialog.FileName);
        }
    }
    [RelayCommand]
    private void Reload()
    {
        if (String.IsNullOrWhiteSpace(this.FileName)) return;
        Read(this.FileName);
    }

    private void Read(string fn)
    {
        try
        {
            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                BadDataFound = this.OnBadDataFound,
            };
            if (!string.IsNullOrEmpty(this.Delimiter)) config.Delimiter = this.Delimiter;
            var encoding = Services.FileService.GetEncoding(fn);
            using (var fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fs, encoding))
            using (var csv = new CsvReader(reader, config))
            {
                using (var dr = new CsvDataReader(csv))
                {

                    var ss = dr.GetSchemaTable();
                    this._table = new DataTable();
                    _table.Load(dr);
                    this.Rows = _table.Clone();
                    this.Total = _table.Rows.Count;
                    if (this.PageIndex != 1)
                    {
                        this.PageIndex = 1;
                    }
                    else
                    {
                        DoPage(1);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Services.NotifyService.Warning(e.Message);
        }
    }

    private void OnBadDataFound(BadDataFoundArgs args)
    {
    }
}
