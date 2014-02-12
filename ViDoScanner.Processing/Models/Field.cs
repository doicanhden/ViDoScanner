namespace ViDoScanner.Processing.Models
{
  using System.Xml.Serialization;

  [XmlType]
  public class Field
  {
    [XmlAttribute]
    public int Index { get; set; }

    [XmlAttribute]
    public int X { get; set; }

    [XmlAttribute]
    public int Y { get; set; }

    [XmlAttribute]
    public int Width { get; set; }

    [XmlAttribute]
    public int Height { get; set; }

    [XmlElement]
    public string Name { get; set; }

    [XmlElement]
    public int NumberOfBlanks { get; set; }

    [XmlElement]
    public int NumberOfRows { get; set; }

    [XmlElement]
    public int NumberOfCols { get; set; }
  }
}
