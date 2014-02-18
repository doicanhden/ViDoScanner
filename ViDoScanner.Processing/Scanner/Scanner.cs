using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using ViDoScanner.Core;
using ViDoScanner.Processing.Imaging;

namespace ViDoScanner.Processing.Scanner
{
  public class Scanner
  {
    private class ProcessPageData
    {
      public Page Page { get; set; }
      public string ImagePath { get; set; }
    }

    private Template template;
    private Configuration config;
    private List<string> results = new List<string>();

    public List<string> Results
    {
      get { return (results); }
    }
    public Scanner()
    {
      config = new Configuration();
      config.GrayscaleThreshold = 144;
      config.RatioThreshold = 15;
    }
    public bool LoadTemplate(string pathTemplate)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Template));

      FileStream stream = new FileStream(pathTemplate, FileMode.Open);
      template = (Template)serializer.Deserialize(stream);
      stream.Close();

      Array.Sort(template.Pages, (r, l) => r.Index.CompareTo(l.Index));
      foreach (var page in template.Pages)
      {
        Array.Sort(page.Fields, (r, l) => r.Index.CompareTo(l.Index));
      }

      return (true);
    }

    public bool LoadConfiguration(string pathConfig)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

      FileStream stream = new FileStream(pathConfig, FileMode.Open);
      config = (Configuration)serializer.Deserialize(stream);
      stream.Close();

      return (true);
    }

    public void ScanFolder(string folderName, string extension = "*.jpg", bool subDirectories = true)
    {
      if (template != null || !Directory.Exists(folderName))
      {
        try
        {
          var images = new Queue<string>(Directory.GetFiles(folderName, extension,
            subDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));

          while (images.Count != 0)
          {
            foreach (var page in template.Pages)
            {
              ThreadPool.QueueUserWorkItem(ProcessPageProc, new ProcessPageData()
              {
                Page = page,
                ImagePath = images.Dequeue()
              });
            }
          }
        }
        catch
        {

        }
        finally
        {

        }
      }
    }

    private void AddResult(string result)
    {
      lock (Results)
      {
        Results.Add(result);
      Console.WriteLine(result);
      }
    }
    private void ProcessPageProc(object objData)
    {
      List<CalcCellsRatio> logs = new List<CalcCellsRatio>();
      try
      {
        var data = objData as ProcessPageData;
        var result = string.Empty;

        System.Drawing.Bitmap image = Image.FromFile(data.ImagePath);
        if (image != null && data.Page.Width == image.Width && data.Page.Height == image.Height)
        {
          foreach (var f in data.Page.Fields)
          {
            var grayscalePixels = Image.GetGrayscalePixels(image, f.X, f.Y, f.Width, f.Height);

            CalcCellsRatio fieldScan = new CalcCellsRatio(f.NumberOfRecords, f.NumberOfSelection);

            fieldScan.BuildCells(f.Width, f.Height, f.Direction == Directions.Vertical);

            fieldScan.CalcRatios(grayscalePixels, (byte)config.GrayscaleThreshold);

            var records = fieldScan.Checks(config.RatioThreshold, config.RatioDelta);

            logs.Add(fieldScan);

            result += GetResult(records, f.Type) + new string(' ', f.NumberOfBlanks);
          }
        }

        AddResult(result + data.ImagePath);
      }
      finally
      {
        // Save log for this page.
      }
    }

    public string GetResult(int[] records, DataTypes type)
    {
      string result = string.Empty;

      switch (type)
      {
        case DataTypes.Alpha:
          foreach (var i in records)
          {
            if (i == CalcCellsRatio.MultiSelection)
            {
              result += config.MultiSelection;
            }
            else if (i == CalcCellsRatio.BlankSelection)
            {
              result += config.BlankSelection;
            }
            else
            {
              result += ((char)('A' + i)).ToString();
            }
          }
          break;
        case DataTypes.Number1:
          foreach (var i in records)
          {
            if (i == CalcCellsRatio.MultiSelection)
            {
              result += config.MultiSelection;
            }
            else if (i == CalcCellsRatio.BlankSelection)
            {
              result += config.BlankSelection;
            }
            else
            {
              result += i.ToString();
            }
          }
          break;
        case DataTypes.Number2:
          foreach (var i in records)
          {
            if (i == CalcCellsRatio.MultiSelection)
            {
              result += config.MultiSelection;
            }
            else if (i == CalcCellsRatio.BlankSelection)
            {
              result += config.BlankSelection;
            }
            else
            {
              if (i < 10)
                result += '0';
              result += i.ToString();
            }
          }
          break;
        case DataTypes.Boolean:
          foreach (var i in records)
          {
            result += (i == CalcCellsRatio.BlankSelection) ? "FALSE" : "TRUE";
          }
          break;
        default:
          break;
      }

      return (result);
    }
  }
}
