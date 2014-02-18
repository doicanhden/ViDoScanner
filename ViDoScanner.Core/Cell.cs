namespace ViDoScanner.Core
{
  public class Cell : Rect
  {
    public Cell()
      :base()
    {
    }

    public Cell(int x, int y, int width, int height)
      :base(x, y, width, height)
    {
    }

    public double Ratio { get; set; }
  }
}
