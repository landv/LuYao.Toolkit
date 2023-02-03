using System.Windows.Input;

namespace LuYao.Toolkit.Controls;

public partial class CodeEditor
{
    public static RoutedCommand OpenSearchCommand { get; } = new RoutedCommand(nameof(OpenSearchCommand), typeof(CodeEditor));
    public static RoutedCommand OpenAllFoldedCommand { get; } = new RoutedCommand(nameof(OpenAllFoldedCommand), typeof(CodeEditor));
    public static RoutedCommand CloseAllFoldedCommand { get; } = new RoutedCommand(nameof(CloseAllFoldedCommand), typeof(CodeEditor));

    private void InitCommands()
    {
        this.CommandBindings.Add(new CommandBinding(OpenSearchCommand, OnOpenSearch));
        this.CommandBindings.Add(new CommandBinding(OpenAllFoldedCommand, OnOpenAllFolded, CanFolded));
        this.CommandBindings.Add(new CommandBinding(CloseAllFoldedCommand, OnCloseAllFolded, CanFolded));
    }

    private void CanFolded(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = !string.IsNullOrWhiteSpace(this.Text) && this._editorContext != null && this._editorContext.FoldingStrategy != null;
    }

    private void OnCloseAllFolded(object sender, ExecutedRoutedEventArgs e)
    {
        if (this._foldingManager == null) return;
        var isFrist = true;
        foreach (var item in this._foldingManager.AllFoldings)
        {
            if (isFrist)
            {
                isFrist = false;
                continue;
            }
            item.IsFolded = true;
        }
    }


    private void OnOpenAllFolded(object sender, ExecutedRoutedEventArgs e)
    {
        if (this._foldingManager == null) return;
        foreach (var item in this._foldingManager.AllFoldings) item.IsFolded = false;
    }

    private void OnOpenSearch(object sender, ExecutedRoutedEventArgs e)
    {
        this._searchPanel.Open();
    }
}
