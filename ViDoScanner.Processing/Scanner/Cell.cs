using System.Xml.Serialization;
namespace ViDoScanner.Processing.Scanner
{
  public class Cell
  {
    public Cell()
    {
      X = 0;
      Y = 0;
      Width = 0;
      Height = 0;
    }
    public Cell(int x, int y, int width, int height)
    {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
    }

    [XmlAttribute]
    public int X { get; set; }
    [XmlAttribute]
    public int Y { get; set; }
    [XmlAttribute]
    public int Width { get; set; }
    [XmlAttribute]
    public int Height { get; set; }
    [XmlAttribute]
    public double Ratio { get; set; }
  }
}
