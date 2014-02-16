namespace ViDoScanner.Processing.Scanner
{
  using System.Xml.Serialization;

  [XmlRoot]
  public class Template
  {
    public string Name { get; set; }
    public Page[] Pages { get; set; }
  }
}
