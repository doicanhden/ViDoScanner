namespace ViDoScanner.Processing.Models
{
  using System.Xml.Serialization;

  class Template
  {
    [XmlArray]
    public Page[] Pages { get; set; }
  }
}
