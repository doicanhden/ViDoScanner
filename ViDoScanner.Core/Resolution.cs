namespace ViDoScanner.Core
{
  public class Resolution
  {
    public Resolution()
    {
      X = 0;
      Y = 0;
    }
    public Resolution(double x = 0, double y = 0)
    {
      X = x;
      Y = y;
    }

    public double X { get; set; }
    public double Y { get; set; }
  }
}
