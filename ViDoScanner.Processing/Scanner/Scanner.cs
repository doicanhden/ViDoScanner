namespace ViDoScanner.Processing.Scanner
{
  using System;
  using System.IO;
  using System.Text;
  using System.Threading.Tasks;
  using System.Xml.Serialization;
  using ViDoScanner.Core;
  using ViDoScanner.Processing.Core;
  using ViDoScanner.Processing.Imaging;

  public class Scanner
  {
    private class ProcessPageData
    {
      public ProcessPageData(Page page, string imagePath, StringBuilder result)
      {
        this.Result = result;
        this.Page = page;
        this.ImagePath = imagePath;
      }
      public StringBuilder Result { get; private set; }
      public Page Page { get; private set; }
      public string ImagePath { get; private set; }
    }

    public Template Template { get; private set; }
    public Configuration Config { get; set; }

    public Scanner()
    {
    }
    public bool LoadTemplate(string pathTemplate)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Template));

      FileStream stream = new FileStream(pathTemplate, FileMode.Open);
      Template = (Template)serializer.Deserialize(stream);
      stream.Close();

      Array.Sort(Template.Pages, (r, l) => r.Index.CompareTo(l.Index));
      foreach (var page in Template.Pages)
      {
        Array.Sort(page.Fields, (r, l) => r.Index.CompareTo(l.Index));
      }

      return (true);
    }

    public bool LoadConfiguration(string pathConfig)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

      FileStream stream = new FileStream(pathConfig, FileMode.Open);
      Config = (Configuration)serializer.Deserialize(stream);
      stream.Close();

      return (true);
    }

    public void SinglePage(Page page, string[] images, Action<string> callBack)
    {
      if (page != null && images != null && images.Length > 0)
      {
        try
        {
          var result = new StringBuilder();

          #region Make CSV header
          {
            StringBuilder sb = new StringBuilder("Tên tệp,");
            foreach (var f in page.Fields)
            {
              sb.AppendFormat("\"{0}\",", f.Name);
            }
            sb.Remove(sb.Length - 1, 1); // Remove last ','.

            result.AppendLine(sb.ToString());
          }
          #endregion

          Task task = new Task(
            () =>
            {
              using (ThreadPoolWait tpw = new ThreadPoolWait())
              {
                foreach (var image in images)
                {
                  tpw.QueueUserWorkItem(ProcessPageProc,
                    new ProcessPageData(page, image, result));
                }

                tpw.WaitOne();
              }
            });

          task.ContinueWith(x => callBack(result.ToString()));
          task.Start();
        }
        catch
        {

        }
        finally
        {

        }
      }
    }
    public void ScanFolder(string folderName, string outputFilename, string extension = "*.jpg")
    {
      if (Template != null && Template.Pages.Length == 1 && Directory.Exists(folderName))
      {
        try
        {
          var imagesPath = Directory.GetFiles(folderName, extension, SearchOption.TopDirectoryOnly);
          if (imagesPath != null && imagesPath.Length > 0)
          {
            var page = Template.Pages[0];
            SinglePage(page, imagesPath, (x) =>
              File.WriteAllText(Config.OutputDirectory + outputFilename, x));
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
    private void ProcessPageProc(object objData)
    {
      try
      {
        var data = objData as ProcessPageData;
        System.Drawing.Bitmap image = Image.FromFile(data.ImagePath);

        if (image != null && data.Page.Width == image.Width && data.Page.Height == image.Height)
        {
          var sb = new StringBuilder();
          sb.AppendFormat("\"{0}\",", Path.GetFileNameWithoutExtension(data.ImagePath));

          foreach (var f in data.Page.Fields)
          {
            var fieldScanner = new FieldScanner(f);

            fieldScanner.CalcRatios(Image.GetGrayscalePixels(image, f.X, f.Y, f.Width, f.Height),
              (byte)Config.GrayscaleThreshold);

            sb.Append(GetStringResult(f.Type, fieldScanner.Checks(
              Config.RatioThreshold, Config.RatioDelta)));

            sb.Append(',');
          }

          sb.Remove(sb.Length - 1, 1); // Remove last ','.

          lock (data.Result)
          {
            data.Result.AppendLine(sb.ToString());
          }
        }
      }
      finally
      {
        // Save log for this page.
      }
    }

    public string GetStringResult(DataTypes type, int[] records)
    {
      var sb = new StringBuilder();

      if (type == DataTypes.Boolean)
      {
        foreach (var i in records)
        {
          sb.Append(i != FieldScanner.BlankSelection);
        }
      }
      else if (type == DataTypes.Alpha)
      {
        foreach (var i in records)
        {
          if (i == FieldScanner.MultiSelection)
          {
            sb.Append(Config.MultiSelection);
          }
          else if (i == FieldScanner.BlankSelection)
          {
            sb.Append(Config.BlankSelection);
          }
          else
          {
            sb.Append((char)('A' + i));
          }
        }
      }
      else if (type == DataTypes.Number1)
      {
        foreach (var i in records)
        {
          if (i == FieldScanner.MultiSelection)
          {
            sb.Append(Config.MultiSelection);
          }
          else if (i == FieldScanner.BlankSelection)
          {
            sb.Append(Config.BlankSelection);
          }
          else
          {
            sb.Append(i);
          }
        }
      }
      else if (type == DataTypes.Number2)
      {
        foreach (var i in records)
        {
          if (i == FieldScanner.MultiSelection)
          {
            sb.Append(Config.MultiSelection);
          }
          else if (i == FieldScanner.BlankSelection)
          {
            sb.Append(Config.BlankSelection);
          }
          else
          {
            if (i < 10)
              sb.Append('0');
            sb.Append(i);
          }
        }
      }
      else
      {
        //??? 
      }

      return (sb.ToString());
    }
  }
}
