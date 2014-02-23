namespace ViDoScanner.Core
{
  using System.Xml.Serialization;

  public class Field:Rect
  {
    public Field()
    {
      NumberOfRecords = 1;
      NumberOfOptions = 1;
    }

    [XmlAttribute]
    public int Index { get; set; }

    public string Name { get; set; }
    public DataTypes Type { get; set; }
    public Directions Direction { get; set; }
    public int NumberOfRecords { get; set; }
    public int NumberOfOptions { get; set; }
  }
}
