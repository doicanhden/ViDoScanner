namespace ViDoScanner.Windows
{
  using System.Windows;
  using ViDoScanner.ViewModels;

  /// <summary>
  /// Interaction logic for PromptBox.xaml
  /// </summary>
  public partial class PromptBox : Window
  {
    public PromptBox(PromptViewModel prompt)
    {
      InitializeComponent();
      this.DataContext = prompt;
    }
  }
}
