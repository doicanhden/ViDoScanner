using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace ViDoScanner.Processing.Imaging
{
  /// <summary>
  ///  Core image relatad methods.
  /// </summary>
  /// 
  /// <remarks>All methods of this class are static and represent general routines
  /// used by different image processing classes.</remarks>
  /// 
  public static class Image
  {
    /// <summary>
    /// Check if specified 8 bpp image is grayscale.
    /// </summary>
    /// 
    /// <param name="image">Image to check.</param>
    /// 
    /// <returns>Returns <b>true</b> if the image is grayscale or <b>false</b> otherwise.</returns>
    /// 
    /// <remarks>The methods checks if the image is a grayscale image of 256 gradients.
    /// The method first examines if the image's pixel format is
    /// <see cref="System.Drawing.Imaging.PixelFormat">Format8bppIndexed</see>
    /// and then it examines its palette to check if the image is grayscale or not.</remarks>
    /// 
    public static bool IsGrayscale(Bitmap image)
    {
      bool ret = false;

      if (image.PixelFormat == PixelFormat.Format8bppIndexed)
      {
        ret = true;
        ColorPalette cp = image.Palette;
        Color c;
        for (int i = 0; i < 256; i++)
        {
          c = cp.Entries[i];
          if ( ( c.R != i ) || ( c.G != i ) || ( c.B != i ) )
          {
            ret = false;
            break;
          }
        }
      }

      return (ret);
    }

    /// <summary>
    /// Load bitmap from file.
    /// </summary>
    /// <param name="fileName">File name to load bitmap from.</param>
    /// <returns>Returns loaded bitmap.</returns>
    public static Bitmap FromFile(string fileName)
    {
      Bitmap loadedImage = null;
      FileStream stream = null;

      try
      {
        // read image to temporary memory stream
        stream = File.OpenRead(fileName);
        MemoryStream memoryStream = new MemoryStream();

        byte[] buffer = new byte[10000];
        while (true)
        {
          int read = stream.Read( buffer, 0, 10000 );

          if (read == 0)
            break;

          memoryStream.Write(buffer, 0, read);
        }

        loadedImage = (Bitmap)Bitmap.FromStream(memoryStream);
      }
      finally
      {
        if (stream != null)
        {
          stream.Close();
          stream.Dispose();
        }
      }

      return (loadedImage);
    }

    /// <summary>
    /// Get grayscale pixels for region
    /// </summary>
    /// <param name="bitmap">Source bitmap</param>
    /// <param name="x">X location</param>
    /// <param name="y">Y location</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    /// <returns></returns>
    public static byte[,] GetGrayscalePixels(Bitmap bitmap, int x, int y, int width, int height)
    {
      BitmapData bmpData = bitmap.LockBits(new Rectangle(x, y, width, height),
        ImageLockMode.ReadOnly, bitmap.PixelFormat);

      try
      {
        unsafe
        {
          byte[,] pixel = new byte[width, height];
          byte* ptr = (byte*)bmpData.Scan0.ToPointer();

          int depth = System.Drawing.Image.GetPixelFormatSize(bmpData.PixelFormat);

          if (depth == 24 || depth == 32)
          {
            int off = depth / 8;

            for (int yy = 0; yy < height; ++yy)
            {
              byte* row = ptr + (yy * bmpData.Stride);

              for (int xx = 0; xx < width; xx += off)
              {
                // Convert to grayscale
                pixel[xx, yy] = (byte)(
                  row[xx + 2] * 0.299 + // Red 
                  row[xx + 1] * 0.587 + // Green
                  row[xx    ] * 0.114); // Blue
              }
            }
          }
          else if (depth == 8)
          {
            for (int yy = 0; yy < height; ++yy)
            {
              byte* row = ptr + (yy * bmpData.Stride);

              for (int xx = 0; xx < width; ++xx)
              {
                pixel[xx, yy] = row[xx];
              }
            }
          }
          else
          {
            throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
          }

          return (pixel);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        bitmap.UnlockBits(bmpData);
      }
    }
  }

}
