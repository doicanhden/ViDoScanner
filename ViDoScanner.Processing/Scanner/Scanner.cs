using System;
using System.IO;
using System.Xml.Serialization;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ViDoScanner.Processing.Enums;


namespace ViDoScanner.Processing.Scanner
{
  public class Scanner
  {
    private Template template;
    private Configuration config;

    public bool LoadTemplate(string pathTemplate)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Template));

      FileStream stream = new FileStream(pathTemplate, FileMode.Open);
      template = (Template)serializer.Deserialize(stream);
      stream.Close();

      return (true);
    }
    public bool LoadConfig(string pathConfig)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

      FileStream stream = new FileStream(pathConfig, FileMode.Open);
      config = (Configuration)serializer.Deserialize(stream);
      stream.Close();

      return (true);
    }
    public void SaveLog(string pathLog)
    {
      StreamWriter stream = new StreamWriter(pathLog);

      XmlSerializer serializer = new XmlSerializer(typeof(Template));
      serializer.Serialize(stream, template);

      stream.Close();
    }
    public string Scan(string imagePath)
    {
      if (template != null)
      {
        foreach (var page in template.Pages)
        {
          Bitmap image = Imaging.Image.FromFile(imagePath);
          if (image != null && page.Width == image.Width && page.Height == image.Height && Imaging.Image.IsGrayscale(image))
          {
            string pageResult = string.Empty;
            Imaging.LockBitmap lockedImage = new Imaging.LockBitmap(image);
            lockedImage.LockBits();

            foreach (var field in page.Fields)
            {
              field.BuildCells();
              field.CalcRatios(lockedImage, 194);
              field.Checks(15);

              pageResult += field.GetResult();
            }
            lockedImage.UnlockBits();

            Console.WriteLine(pageResult);

            return (pageResult);
          }
        }

      }
      return (string.Empty);
    }

  }
}
