namespace ViDoScanner.Utilities
{
  using System;
  using System.IO;
  using System.Xml.Serialization;

  public static class Xml
  {
    public static void Serialize<T>(string fileName, T obj)
    {
      StreamWriter stream = new StreamWriter(fileName);

      XmlSerializer serializer = new XmlSerializer(typeof(T));
      serializer.Serialize(stream, obj);
      stream.Close();
    }

    public static T Deserialize<T>(string fileName)
    {
      FileStream stream = new FileStream(fileName, FileMode.Open);

      XmlSerializer serializer = new XmlSerializer(typeof(T));
      var obj = serializer.Deserialize(stream);
      stream.Close();

      return ((T)obj);
    }
  }
}
