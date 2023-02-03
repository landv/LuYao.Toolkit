namespace LuYao.Toolkit.Services
{
    public interface ISaveFileDialog
    {
        string Title { get; set; }

        string InitialDirectory { get; set; }

        string Filter { get; set; }

        bool AddExtension { get; set; }

        string FileName { get; set; }
        string DefaultExt { get; set; }


        bool ShowDialog();
    }
}
