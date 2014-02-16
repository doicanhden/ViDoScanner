namespace ViDoScanner.Processing.Scanner
{
  using System.Drawing;
  using System.Xml.Serialization;
  using ViDoScanner.Processing.Enums;

  public class Field
  {
    [XmlAttribute]
    public int Index { get; set; }

    [XmlAttribute]
    public int X { get; set; }

    [XmlAttribute]
    public int Y { get; set; }

    [XmlAttribute]
    public int Width { get; set; }

    [XmlAttribute]
    public int Height { get; set; }

    public string Name { get; set; }
    public DataTypes Type { get; set; }
    public Directions Direction { get; set; }
    public int NumberOfRecords { get; set; }
    public int NumberOfSelection { get; set; }
    public int NumberOfBlanks { get; set; }
    public string DefaultValue { get; set; }

    public int[] Records { get; set; }
    public Cell[]  Cells { get; set; }

    public void BuildCells()
    {
      Cells = new Cell[NumberOfRecords * NumberOfSelection];

      if (Direction == Directions.Horizontal)
      {
        int cellWidth = Width / NumberOfRecords;
        int cellHeight = Height / NumberOfSelection;

        for (int i = 0; i < NumberOfRecords; ++i)
        {
          for (int j = 0; j < NumberOfSelection; ++j)
          {
            Cells[i * NumberOfSelection + j] = new Cell(
              X + i * cellWidth, Y + j * cellHeight, cellWidth, cellHeight);
          }
        }
      }
      else
      {
        int cellWidth = Width / NumberOfSelection;
        int cellHeight = Height / NumberOfRecords;

        for (int i = 0; i < NumberOfRecords; ++i)
        {
          for (int j = 0; j < NumberOfSelection; ++j)
          {
            Cells[i * NumberOfSelection + j] = new Cell(
              X + j * cellWidth, Y + i * cellHeight, cellWidth, cellHeight);
          }
        }
      }
    }

    public void CalcRatios(Imaging.LockBitmap lockImage, byte grayscaleThreshold = 194, int step = 1)
    {
      int cX, cY, count = 0;
      foreach (var cell in Cells)
      {
        cX = cell.X + cell.Width;
        cY = cell.Y + cell.Height;
        count = 0;

        for (int y = cell.Y; y < cY; y += step)
        {
          for (int x = cell.X; x < cX; x += step)
          {
            Color c = lockImage.GetPixel(x, y);
            byte g = (byte)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);

            if (g < grayscaleThreshold)
            {
              ++count;
            }
          }
        }

        cell.Ratio = ((double)count / (cell.Width * cell.Height)) * 100;
      }
    }

    public void Checks(double ratioThreshold = 25, double ratioTheta = 125)
    {
      Records = new int[NumberOfRecords];

      int maxRatio1, maxRatio2, idx;

      for (int i = 0; i < NumberOfRecords; ++i)
      {
        maxRatio1 = maxRatio2 = i * NumberOfSelection;
        for (int j = 1; j < NumberOfSelection; ++j)
        {
          idx = i * NumberOfSelection + j;
          if (Cells[idx].Ratio > Cells[maxRatio1].Ratio)
          {
            maxRatio2 = maxRatio1;
            maxRatio1 = idx;
          }
        }

        if (Cells[maxRatio1].Ratio >= ratioThreshold)
        {
          if (Cells[maxRatio1].Ratio * ratioTheta > Cells[maxRatio2].Ratio)
            Records[i] = maxRatio1 % NumberOfSelection;
          else
            Records[i] = -2; // Multi-Selection.
        }
        else
        {
          Records[i] = -1; // Blank-Selection.
        }
      }
    }


    static readonly string[] alpha = {
      "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
      "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                                     };
    public string GetResult(string blankSelection = "-", string multiSelection = "*")
    {
      string result = string.Empty;

      foreach (var i in Records)
      {
        if (i == -1)
        {
          return (blankSelection);
        }
        else if (i == -2)
        {
          return (multiSelection);
        }
        else
        {
          switch (Type)
          {
            case DataTypes.Alpha:
              result += alpha[i];
              break;
            case DataTypes.Number1:
              result += i.ToString();
              break;
            case DataTypes.Number2:
              if (i < 10)
                result += '0';
              result += i.ToString();
              break;
            case DataTypes.Boolean:
              result += (i == 0) ? "TRUE" : "FALSE";
              break;
            default:
              break;
          }
        }
      }
      result += new string(' ', NumberOfBlanks);

      return (result);
    }

  }
}
