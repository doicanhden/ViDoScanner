namespace ViDoScanner.Core
{
  public class Configuration
  {
    public Configuration()
    {
      BlankSelection = "-";
      MultiSelection = "*";
      RatioThreshold = 15;
      RatioDelta = 125;
      GrayscaleThreshold = 144;
    }

    public string BlankSelection { get; set; }
    public string MultiSelection { get; set; }
    public double RatioThreshold { get; set; }
    public double RatioDelta { get; set; }
    public int GrayscaleThreshold { get; set; }
  }
}
