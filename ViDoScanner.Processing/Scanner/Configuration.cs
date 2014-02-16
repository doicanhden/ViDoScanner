namespace ViDoScanner.Processing.Scanner
{
  public class Configuration
  {
    public Configuration()
    {
      BlankSelection = "-";
      MultiSelection = "*";
      RatioThreshold = 25;
      RatioTheta = 125;
    }

    public string ImagesDirectory { get; set; }
    public string OutputDirectory { get; set; }
    public string BlankSelection { get; set; }
    public string MultiSelection { get; set; }
    public double RatioThreshold { get; set; }
    public double RatioTheta { get; set; }
  }
}
