namespace ViDoScanner.Core
{
  public class CalcCellsRatio
  {
    private int nRecords = 1;
    private int nSelection = 1;

    public CalcCellsRatio(int nRecords, int nSelection)
    {
      this.nRecords = nRecords;
      this.nSelection = nSelection;
    }

    public static int BlankSelection { get { return (-1); } }
    public static int MultiSelection { get { return (-2); } }
    public Cell[,] Cells { get; set; }

    public void BuildCells(int width, int height, bool isVertical)
    {
      Cells = new Cell[nRecords, nSelection];

      if (isVertical)
      {
        int cellWidth = width / nSelection;
        int cellHeight = height / nRecords;

        for (int i = 0; i < nRecords; ++i)
        {
          for (int j = 0; j < nSelection; ++j)
          {
            Cells[i, j] = new Cell(
              j * cellWidth, i * cellHeight, cellWidth, cellHeight);
          }
        }
      }
      else
      {
        int cellWidth = width / nRecords;
        int cellHeight = height / nSelection;

        for (int i = 0; i < nRecords; ++i)
        {
          for (int j = 0; j < nSelection; ++j)
          {
            Cells[i, j] = new Cell(
              i * cellWidth, j * cellHeight, cellWidth, cellHeight);
          }
        }
      }
    }

    public void CalcRatios(byte[,] grayscalePixels, byte grayscaleThreshold = 144)
    {
      int cX, cY, count = 0;
      foreach (var cell in Cells)
      {
        cX = cell.X + cell.Width;
        cY = cell.Y + cell.Height;
        count = 0;

        for (int y = cell.Y; y < cY; ++y)
        {
          for (int x = cell.X; x < cX; ++x)
          {
            if (grayscalePixels[x, y] < grayscaleThreshold)
            {
              ++count;
            }
          }
        }

        cell.Ratio = ((double)count / (cell.Width * cell.Height)) * 100;
      }
    }

    public int[] Checks(double ratioThreshold = 15, double ratioDelta = 125)
    {
      int[] results = new int[nRecords];

      int maxRatio1, maxRatio2;
      for (int i = 0; i < nRecords; ++i)
      {
        maxRatio1 = maxRatio2 = 0;
        for (int j = 1; j < nSelection; ++j)
        {
          if (Cells[i, j].Ratio > Cells[i, maxRatio1].Ratio)
          {
            maxRatio2 = maxRatio1;
            maxRatio1 = j;
          }
        }

        results[i] = (Cells[i, maxRatio1].Ratio < ratioThreshold) ? BlankSelection :
          (Cells[i, maxRatio1].Ratio * ratioDelta <= Cells[i, maxRatio2].Ratio) ? MultiSelection : maxRatio1;
      }

      return (results);
    }
  }
}
