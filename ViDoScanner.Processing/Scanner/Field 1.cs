namespace ViDoScanner.Processing.Scanner
{
  using System;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.Runtime.InteropServices;

  public class Field
  {
    private byte[] pixels;
    public Field()
    {
    }

    public void Preprocessing(Bitmap image, Rectangle rect)
    {
      try
      {
        BitmapData bitmapData = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);

        pixels = new byte[rect.Width * rect.Height];
        Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);

        image.UnlockBits(bitmapData);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

    }
  }
}
