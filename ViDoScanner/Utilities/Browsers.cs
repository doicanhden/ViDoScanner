using System.Windows.Forms;
using ViDoScanner.ViewModels;
namespace ViDoScanner.Utilities
{
  public static class Browsers
  {
    public static string ShowBrowserFolder(string description = "", bool showNewFolderButton = true)
    {
      var folderBrowser = new FolderBrowserDialog();
      folderBrowser.ShowNewFolderButton = showNewFolderButton;
      folderBrowser.Description = description;

      if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        return (folderBrowser.SelectedPath);
      }

      return (string.Empty);
    }

    public static string ShowOpenFile(string title = "Open", string filter = "All Files (*.*)|*.*")
    {
      OpenFileDialog openFileDlg = new OpenFileDialog();
      openFileDlg.Title = title;
      openFileDlg.Filter = filter;

      if (openFileDlg.ShowDialog() == DialogResult.OK)
      {
        return (openFileDlg.FileName);
      }

      return (string.Empty);
    }
    public static string ShowSaveFile(string title = "Save", string filter = "All Files (*.*)|*.*")
    {
      SaveFileDialog saveFileDlg = new SaveFileDialog();
      saveFileDlg.Title = title;
      saveFileDlg.Filter = filter;

      if (saveFileDlg.ShowDialog() == DialogResult.OK)
      {
        return (saveFileDlg.FileName);
      }

      return (string.Empty);
    }

    public static string ShowPromptBox(string title = "", string detail = "Nhập vào một chuỗi")
    {
      var prompt = new PromptViewModel() { Title = title, Detail = detail};
      var promptBox = new Windows.PromptBox(prompt);

      if (promptBox.ShowDialog() == true)
      {
        return (prompt.Text);
      }

      return (string.Empty);
    }
  }
}
