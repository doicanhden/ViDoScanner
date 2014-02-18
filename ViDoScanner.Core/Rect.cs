namespace ViDoScanner.Core
{
  using System.Xml.Serialization;

  public class Rect
  {
    public Rect()
    {
      X = 0;
      Y = 0;
      Width = 0;
      Height = 0;
    }

    public Rect(int x, int y, int width, int height)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
    }

    [XmlAttribute]
    public int X { get; set; }
    [XmlAttribute]
    public int Y { get; set; }
    [XmlAttribute]
    public int Width { get; set; }
    [XmlAttribute]
    public int Height { get; set; }
  }
}
