using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuYao.Toolkit.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.CrossBorder;

public partial class MercadoToWorldFirstViewModel : ViewModelBase
{
    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Input))]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    private string input;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(Output))]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    private string output;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(OverwriteExistsFiles))]
    private bool overwriteExistsFiles;

    [ObservableProperty]
    [ViewStates.WatchViewState(nameof(AppendFileName))]
    private bool appendFileName;

    [ObservableProperty]
    private string log = "准备就绪";

    private bool CanConvert()
    {
        if (string.IsNullOrWhiteSpace(this.Input)) return false;
        if (string.IsNullOrWhiteSpace(this.Output)) return false;
        return true;
    }

    private void WriteLog(StringBuilder log, string message, params object[] args)
    {
        if (args != null && args.Length > 0)
        {
            log.AppendLine(string.Format(message, args));
        }
        else
        {
            log.AppendLine(message);
        }
        this.Log = log.ToString();
    }
    public class OutputLine
    {
        public string 订单编号;
        public DateTime? 支付时间;
        public decimal? 订单金额;
        public string 商品标题;
        public string 买家名称;
        public string 收货地址;

        public string 币种 => "USD";
        public int 商品数量 => 1;
        public bool IsEmpty()
        {
            if (string.IsNullOrWhiteSpace(this.订单编号) == false) return false;
            if (this.支付时间 != null) return false;
            if (this.订单金额 != null) return false;
            if (string.IsNullOrWhiteSpace(this.商品标题) == false) return false;
            if (string.IsNullOrWhiteSpace(this.买家名称) == false) return false;
            if (string.IsNullOrWhiteSpace(this.收货地址) == false) return false;
            return true;
        }
    }

    private void Write(string file, ExcelWorksheet sheet, List<OutputLine> lines)
    {
        var fn = Path.GetFileNameWithoutExtension(file);
        var ext = Path.GetExtension(file);
        if (this.AppendFileName && !fn.StartsWith("【万里汇上传】"))
        {
            fn = $"【万里汇上传】{fn}";
        }
        var target = Path.Combine(this.Output, $"{fn}{ext}");
        if (File.Exists(target))
        {
            if (!this.OverwriteExistsFiles) throw new Exception($"目标文件已存在，请先删除或者选中【覆盖同名文件】。{Environment.NewLine}路径：{target}");
            File.Delete(target);
        }
        using (var fs = File.OpenWrite(target))
        using (var pkg = new ExcelPackage(fs))
        {
            var sheets = pkg.Workbook.Worksheets;
            var dist = sheets.Add(sheet.Name);
            //write header
            //write body
            dist.SetValue("A1", "Order ID");
            dist.SetValue("B1", "Paid Date");
            dist.SetValue("C1", "Order Total");
            dist.SetValue("D1", "Currency Code");
            dist.SetValue("E1", "Product Title");
            dist.SetValue("F1", "Product Quantity");
            dist.SetValue("G1", "Buyer Name/Buyer ID");
            dist.SetValue("H1", "Shipping Address");

            for (int i = 1; i <= 8; i++)
            {
                var style = dist.Cells[1, i].Style;
                style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            //TODO:杂项信息

            var line2 = new string[] {
                @"本列填写内容：订单编号

填写具体要求：
最长可接受64个字符，超过将不会被计算到交易申报数据中",
                @"本列填写内容：支付时间

填写具体要求：
1. 针对不同平台的时间格式要求不同，请参照以下平台的时间格式示例：
Tophatter：MM/dd/yyyy，06/21/2019

Cdiscount：dd/MM/yyyy HH:mm: ss，04 / 02 / 2020 12:37:50

Rakuten：dd / MM / yyyy - HH:mm，26 / 12 / 2019 - 14:50

                或 MM/ dd / yyyy hh: mm: ss aa，02 / 13 / 2020 12:19:20 PM

FNAC：dd / MM / yyyy，14 / 12 / 2019

Etsy：MM / dd / yyyy，11 / 27 / 2019

Newegg：MM / dd / yyyy HH: mm: ss，12 / 26 / 2019 23:30:18

2.不在以上平台范围内的，时间格式要求： yyyy / mm / dd 或者mm / dd / yyyy，2022 / 01 / 31 或者 01 / 31 / 2022",
                @"本列填写内容：订单总金额（非商品单价，请填写每笔交易的总金额）

填写具体要求：
1.必须是数字
2.可以有小数点，最多只支持小数点后两位，超过小数点后两位的数字将被截断且不计入额度统计",
                @"本列填写内容：币种

填写具体要求：
1.仅支持大写
2.且必须为三位字母代码
例如: USD,EUR,GBP",
                @"本列填写内容：商品标题

填写具体要求：
1.最长可接受512个字符，超过将不会被计算到交易申报数据中",
                @"本列填写内容：商品数量

填写具体要求：
1.仅支持整数",
                @"本列填写内容：买家名称 / 买家ID

填写具体要求：
1.最长可接受128字符，超过将不会被计算到交易申报数据中",
                @"本列填写内容：收货地址

填写具体要求：
1.最长可接受512个字符，超过将不会被计算到交易申报数据中"
            };

            var line3 = new string[]
            {
                "填写示例：2002021154repeat2020526",
                @"填写示例：详见上表示例",
                @"填写示例：15.21",
                @"填写示例：EUR",
                @"填写示例：Gilet pour chien/Harnais Chien- Noir taille XL --",
                @"填写示例：1",
                @"填写示例：M. Bouscailloux Loric 12 COURS MERCURE",
                @"填写示例：FR"
            };

            for (int i = 1; i <= line2.Length; i++)
            {
                dist.SetValue(2, i, line2[i - 1]);
                var style = dist.Cells[2, i].Style;
                style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                style.WrapText = true;
            }
            dist.Row(2).CustomHeight = true;

            for (int i = 1; i <= line3.Length; i++)
            {
                dist.SetValue(3, i, line3[i - 1]);
                var style = dist.Cells[2, i].Style;
                style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                style.WrapText = true;
            }
            dist.Row(3).CustomHeight = true;

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var row = i + 4;
                dist.SetValue($"A{row}", line.订单编号);
                dist.SetValue($"B{row}", line.支付时间);
                dist.SetValue($"C{row}", line.订单金额);
                dist.SetValue($"D{row}", line.币种);
                dist.SetValue($"E{row}", line.商品标题);
                dist.SetValue($"F{row}", line.商品数量);
                dist.SetValue($"G{row}", line.买家名称);
                dist.SetValue($"H{row}", line.收货地址);

                dist.Cells[$"B{row}"].Style.Numberformat.Format = "dd/MM/yy";
            }

            for (int i = 1; i <= 8; i++)
            {
                dist.Column(i).AutoFit(40, 58);
            }
            pkg.Save();
        }
    }
    private void Convert(string[] files, StringBuilder log)
    {
        var regex = new Regex("((\\w+)(\\s)(\\d+),(\\s)(\\d+))", RegexOptions.None);//表达式对象
        var output = new List<OutputLine>();
        foreach (var file in files)
        {
            WriteLog(log, "处理文件：{0}", Path.GetFileName(file));
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var pkg = new ExcelPackage(fs))
            {
                foreach (var sheet in pkg.Workbook.Worksheets)
                {
                    WriteLog(log, "读取 Sheet : {0},行：{1}，列：{2}。", sheet.Name, sheet.Dimension.Rows, sheet.Dimension.Columns);
                    output.Clear();
                    for (int row = 4; row <= sheet.Dimension.Rows; row++)
                    {
                        var line = new OutputLine();
                        for (int col = 1; col <= sheet.Dimension.Columns; col++)
                        {
                            var cell = sheet.Cells[row, col];
                            switch (col)
                            {
                                case 1://Order #
                                    line.订单编号 = cell.GetCellValue<string>();
                                    break;
                                case 2://Order date
                                    {
                                        var str = cell.GetCellValue<string>();
                                        if (!string.IsNullOrWhiteSpace(str) && regex.IsMatch(str))
                                        {
                                            var dt = regex.Match(str).Groups[1].Value;
                                            line.支付时间 = DateTime.Parse(dt).Date;
                                        }
                                    }
                                    break;
                                case 17://Listing title
                                    line.商品标题 = cell.GetCellValue<string>();
                                    break;
                                case 19://Listing sale unit price (USD)
                                    {
                                        var str = cell.GetCellValue<string>();
                                        if (!string.IsNullOrWhiteSpace(str) && decimal.TryParse(str, out var m))
                                        {
                                            line.订单金额 = m;
                                        }
                                    }
                                    break;
                                case 25://Buyer
                                    line.买家名称 = cell.GetCellValue<string>();
                                    break;
                                case 31://Country
                                    line.收货地址 = cell.GetCellValue<string>();
                                    break;
                            }
                        }
                        if (!line.IsEmpty()) output.Add(line);
                    }

                    if (output.Count <= 0)
                    {
                        WriteLog(log, "未读取到有效数据，跳过。");
                        continue;
                    }
                    else
                    {
                        WriteLog(log, "转换数据");
                        Write(file, sheet, output);
                        WriteLog(log, "处理成功");
                        break;
                    }
                }
            }
        }
        WriteLog(log, "处理完成");
    }

    [RelayCommand(CanExecute = nameof(CanConvert))]
    private async Task Convert()
    {
        var log = new StringBuilder();
        try
        {
            if (!Directory.Exists(this.Input)) throw new Exception("美客多报表目录不存在！");
            if (!Directory.Exists(this.Output)) Directory.CreateDirectory(this.Output);
            if (this.Input == this.Output) throw new Exception("两个目录不能相同");
            var files = Directory.GetFiles(this.Input, "*.xlsx");
            if (files.Length <= 0) throw new Exception("没有找到美客多报表文件，仅支持 xlsx 格式。");
            log.AppendLine($"找到 {files.Length} 个文件");
            await Task.Run(() => Convert(files, log));
            NotifyService.Success("转换成功！");
        }
        catch (Exception e)
        {
            log.AppendLine(e.ToString());
            throw;
        }
        finally
        {
            this.Log = log.ToString();
        }
    }
}
