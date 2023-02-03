namespace LuYao.Toolkit.Services;

public interface IOpenFileDialog
{
    string Title { get; set; }
    string Filter { get; set; }
    bool Multiselect { get; set; }
    bool ShowDialog();
    string FileName { get; set; }
    string[] FileNames { get; }
}