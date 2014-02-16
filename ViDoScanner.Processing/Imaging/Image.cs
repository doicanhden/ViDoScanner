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
    /// Create and initialize new 8 bpp grayscale image.
    /// </summary>
    /// 
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    /// 
    /// <returns>Returns the created grayscale image.</returns>
    /// 
    /// <remarks>The method creates new 8 bpp grayscale image and initializes its palette.
    /// Grayscale image is represented as
    /// <see cref="System.Drawing.Imaging.PixelFormat">Format8bppIndexed</see>
    /// image with palette initialized to 256 gradients of gray color.</remarks>
    /// 
    public static Bitmap CreateGrayscaleImage(int width, int height)
    {
      Bitmap image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

      SetGrayscalePalette(image);

      return (image);
    }

    /// <summary>
    /// Set pallete of the 8 bpp indexed image to grayscale.
    /// </summary>
    /// 
    /// <param name="image">Image to initialize.</param>
    /// 
    /// <remarks>The method initializes palette of
    /// <see cref="System.Drawing.Imaging.PixelFormat">Format8bppIndexed</see>
    /// image with 256 gradients of gray color.</remarks>
    /// 
    /// <exception cref="UnsupportedImageFormatException">Provided image is not 8 bpp indexed image.</exception>
    /// 
    public static void SetGrayscalePalette(Bitmap image)
    {
      // check pixel format
      if (image.PixelFormat != PixelFormat.Format8bppIndexed)
        throw new Exception("Source image is not 8 bpp image.");

      ColorPalette cp = image.Palette;

      for ( int i = 0; i < 256; ++i)
      {
        cp.Entries[i] = Color.FromArgb(i, i, i);
      }
      // set palette back
      image.Palette = cp;
    }
    
    public static Bitmap ConvertToGrayscale(Bitmap source)
    {
      Bitmap image = CreateGrayscaleImage(source.Width, source.Height);

      LockBitmap bm1 = new LockBitmap(source);
      LockBitmap bm2 = new LockBitmap(image);
      bm1.LockBits();
      bm2.LockBits();
      for (int y = 0; y < bm1.Height; ++y)
      {
        for (int x = 0; x < bm1.Width; ++x)
        {
          Color c = bm1.GetPixel(x, y);
          byte g = (byte)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
          bm2.SetPixel(x, y, Color.FromArgb(g, g, g));
        }
      }
      bm2.UnlockBits();
      bm1.UnlockBits();

      return (image);
    }

    public static byte[,] ConvertToBW(Bitmap source, int boxSize = 5, int c = -5)
    {
      if (boxSize < 5)
        boxSize = 5;

      int w = source.Width;
      int h = source.Height;
      int s = (boxSize * 2 + 1) * (boxSize * 2 + 1);

      byte[,] grayscale = new byte[source.Width, source.Height];
      int[,] integral = new int[source.Width, source.Height];

      { // Convert image source to grayscale
        LockBitmap bm = new LockBitmap(source);

        bm.LockBits();
        for (int y = 0; y < bm.Height; ++y)
        {
          for (int x = 0; x < bm.Width; ++x)
          {
            var clr = bm.GetPixel(x, y);
            integral[x, y] = grayscale[x, y] = (byte)(clr.R * 0.299 + clr.G * 0.587 + clr.B * 0.114);
          }
        }
        bm.UnlockBits();
      }

      { // Sum integral image.
        for (int i = 1; i < w; ++i)
          integral[i, 0] += integral[i - 1, 0];

        for (int j = 1; j < h; ++j)
        {
          integral[0, j] += integral[0, j - 1];
          for (int i = 1; i < w; ++i)
            integral[i, j] += integral[i - 1, j] + integral[i, j - 1] - integral[i - 1, j - 1];
        }
      }

      byte[,] binary = new byte[w - 2 * boxSize - 1, h - 2 * boxSize - 1];

      { // Convert image to black & white
        byte mean;
        for (int i = 1 + boxSize; i < w - boxSize; ++i)
        {
          for (int j = 1 + boxSize; j < h - boxSize; ++j)
          {
            mean = (byte)(Math.Max(0, c + (
              integral[i + boxSize, j + boxSize] -
              integral[i + boxSize, j - boxSize - 1] -
              integral[i - boxSize - 1, j + boxSize] +
              integral[i - boxSize - 1, j - boxSize - 1]) / s));

            binary[i - boxSize - 1, j - boxSize - 1] = (byte)(grayscale[i, j] < mean ? 0 : 255);
          }
        }
      }

      return (binary);
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
  }

}
