using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViDoScanner.Core;
namespace ViDoScanner.Processing
{
  public class Scanner
  {
    private object lockObj1 = new object();
    private object lockObj2 = new object();
    private bool isScanning = false;
    private bool isPausing = false;
    private int scannedImages = 0;
    private int maximumImages = 0;

    public int ScannedImages
    {
      get { lock (lockObj2) return (scannedImages); }
      set { lock (lockObj2) scannedImages = value; }
    }
    public int MaximumImages
    {
      get { lock (lockObj2) return (maximumImages); }
      set { lock (lockObj2) maximumImages = value; }
    }
    public bool IsScanning
    {
      get { lock (lockObj1) return (isScanning); }
      set { lock (lockObj1) isScanning = value; }
    }
    public bool IsPausing
    {
      get { lock (lockObj1) return (isPausing); }
      set { lock (lockObj1) isPausing = value; }
    }

    public Configuration Config { get; set; }

    public Scanner()
    {
      ScannedImages = 0;
      MaximumImages = 0;
      Config = new Configuration();
    }
    public void Reset()
    {
      IsScanning = true;
    }
    public void ScanAllDirectories(string directoryName, string imageExtension, string outputDirectoryName, Page page)
    {
      if (page == null)
        throw new System.ArgumentNullException("page");

      if (Directory.Exists(directoryName) && Directory.Exists(outputDirectoryName))
      {
//      try
        {
          var task = new Task(() =>
          {
            var subDirs = Directory.GetDirectories(directoryName, "*", SearchOption.AllDirectories);
            outputDirectoryName += Path.DirectorySeparatorChar;

            foreach (var dir in subDirs)
            {
              var outputFileName = outputDirectoryName + dir.Substring(directoryName.Length + 1).Replace(Path.DirectorySeparatorChar, '_') + ".txt";
              ScanDirectory(dir, imageExtension, outputFileName, page);

              if (!IsScanning)
                break;
            }
          });

          task.Start();
        }
//      catch
        {

        }
      }
    }
    public void ScanDirectory(string directoryName, string imageExtension, string outputFileName, Page page)
    {
      if (page == null)
        throw new System.ArgumentNullException("page");

//    try
      {
        var task = new Task(() =>
        {
          var imagePaths = Directory.GetFiles(directoryName, imageExtension, SearchOption.TopDirectoryOnly);
          Images(imagePaths, page, (x) => File.WriteAllText(outputFileName, x));
        });

        task.Start();
      }
//    catch
      {

      }
    }
    public void Images(string[] imagePaths, Page page, System.Action<string> callBack)
    {
      if (page == null)
        throw new System.ArgumentNullException("page");

      if (imagePaths != null && imagePaths.Length > 0)
      {
//      try
        {
          var task = new Task(
            () =>
            {
              MaximumImages += imagePaths.Length;

              StringBuilder result = new StringBuilder();

              result.AppendLine(GetPageCSVHeader(page));
              foreach (var imagePath in imagePaths)
              {
                result.AppendLine(Image(imagePath, page, Config));

                ++ScannedImages;

                while (true)
                {
                  lock(lockObj1)
                  {
                    if (!IsPausing)
                      break;
                  }

                  Thread.Sleep(100);
                }

                lock(lockObj1)
                {
                  if (!IsScanning)
                    break;
                }
              }

              callBack(result.ToString());
            });

          task.Start();
        }
//      catch
        {

        }
      }
    }

    public static string Image(string imagePath, Page page, Configuration config)
    {
      try
      {
        var result = new StringBuilder();
        Bitmap image = Imaging.Image.FromFile(imagePath);

        if (image != null && page.Width == image.Width && page.Height == image.Height)
        {
          foreach (var f in page.Fields)
          {
            var fieldScanner = new FieldScanner(f);

            fieldScanner.CalcRatios(Imaging.Image.GetGrayscalePixels(image, f.X, f.Y, f.Width, f.Height),
              (byte)config.GrayscaleThreshold);

            int[] records = fieldScanner.Checks(config.RatioThreshold, config.RatioDelta);

            result.Append(GetStringResult(f.Type, records, config.BlankSelection, config.MultiSelection));
            result.Append(',');
          }

          result.AppendFormat("\"{0}\"", imagePath);
        }

        return (result.ToString());
      }
      finally
      {

      }
    }
    public static string GetPageCSVHeader(Page page)
    {
      var sb = new StringBuilder();
      foreach (var f in page.Fields)
      {
        sb.AppendFormat("\"{0}\",", f.Name);
      }
      sb.Append("\"Tên tệp\"");

      return (sb.ToString());
    }
    private static string GetStringResult(DataTypes type, int[] records, string blankSelection, string multiSelection)
    {
      var sb = new StringBuilder();
      switch (type)
      {
        case DataTypes.Alpha:
          foreach (var i in records)
          {
            if (i == FieldScanner.MultiSelection)
            {
              sb.Append(multiSelection);
            }
            else if (i == FieldScanner.BlankSelection)
            {
              sb.Append(blankSelection);
            }
            else
            {
              sb.Append((char)('A' + i));
            }
          }
          break;
        case DataTypes.Number1:
          foreach (var i in records)
          {
            if (i == FieldScanner.MultiSelection)
            {
              sb.Append(multiSelection);
            }
            else if (i == FieldScanner.BlankSelection)
            {
              sb.Append(blankSelection);
            }
            else
            {
              sb.Append(i);
            }
          }
          break;
        case DataTypes.Number2:
          foreach (var i in records)
          {
            if (i == FieldScanner.MultiSelection)
            {
              sb.Append(multiSelection);
            }
            else if (i == FieldScanner.BlankSelection)
            {
              sb.Append(blankSelection);
            }
            else
            {
              if (i < 10)
                sb.Append('0');
              sb.Append(i);
            }
          }
          break;
        case DataTypes.Boolean:
          foreach (var i in records)
          {
            sb.Append(i != FieldScanner.BlankSelection);
          }
          break;
        default:
          break;
      }

      return (sb.ToString());
    }
  }
}
