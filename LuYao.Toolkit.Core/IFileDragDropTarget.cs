namespace LuYao.Toolkit;

public interface IFileDragDropTarget
{
    void OnFilesDropped(string group, string[] filepaths);
}
