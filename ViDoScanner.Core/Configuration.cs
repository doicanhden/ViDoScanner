namespace ViDoScanner.Core
{
  public class Configuration
  {
    public Configuration()
    {
      BlankSelection = "-";
      MultiSelection = "*";
      RatioThreshold = 25;
      RatioDelta = 125;
      GrayscaleThreshold = 144;
    }

    public string ImagesDirectory { get; set; }
    public string OutputDirectory { get; set; }
    public string BlankSelection { get; set; }
    public string MultiSelection { get; set; }
    public double RatioThreshold { get; set; }
    public double RatioDelta { get; set; }
    public int GrayscaleThreshold { get; set; }
  }
}
