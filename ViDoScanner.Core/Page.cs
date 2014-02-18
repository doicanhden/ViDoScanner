using System.Xml.Serialization;
namespace ViDoScanner.Core
{
  public class Page
  {
    [XmlAttribute]
    public int Index { get; set; }
    [XmlAttribute]
    public int Width { get; set; }
    [XmlAttribute]
    public int Height { get; set; }

    public string ImagePath { get; set; }
    public Resolution Resolution { get; set; }
    public string Name { get; set; }
    public Field[] Fields { get; set; }
    public Anchor[] Anchor { get; set; }
  }
}
