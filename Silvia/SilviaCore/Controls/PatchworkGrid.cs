using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SilviaCore.Controls
{
    public class PatchworkGrid : UserControl
    {
        public event Action<PatchworkGridCell> CellClick;

        public int NumColumns { get; set; }
        public int NumRows { get; set; }
        public int CellWidth { get; set; }
        public int CellHeight { get; set; }
        public Style DefaultItemStyle { get; set; }

        private Grid grid = new Grid();
        Dictionary<Vector, PatchworkGridCell> cells = new Dictionary<Vector, PatchworkGridCell>();


        public PatchworkGrid()
        {
            this.IsVisibleChanged += PatchworkGrid_IsVisibleChanged;
            this.Content = grid;
        }

        public PatchworkGrid(int numColumns, int numRows, int cellWidth, int cellHeight) : this()
        {
            this.NumColumns = numColumns;
            this.NumRows = numRows;
            this.CellWidth = cellWidth;
            this.CellHeight = cellHeight;
        }

        private void PatchworkGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ResetGrid();
        }

        public void SetCellContent(PatchworkGridCell cell, object content)
        {
            SetCellContent(cell, content, -1, -1);
        }

        public void SetCellContent(PatchworkGridCell cell, object content, int colSpan, int rowSpan)
        {
            if (colSpan <= 0)
            {
                colSpan = Grid.GetColumnSpan(cell);
            }

            if (rowSpan <= 0)
            {
                rowSpan = Grid.GetRowSpan(cell);
            }

            for (int x = 0; x < colSpan; x++)
            {
                for (int y = 0; y < rowSpan; y++)
                {
                    Vector v = new Vector(cell.Coord.X + x, cell.Coord.Y + y);

                    if (cells.ContainsKey(v))
                    {
                        cells[v].Content = null;
                    }
                }
            }

            cell.Content = content;
        }

        public void ResetCellContent(PatchworkGridCell cell)
        {
            int colSpan = Grid.GetColumnSpan(cell);
            int rowSpan = Grid.GetRowSpan(cell);

            for (int x = 0; x < colSpan; x++)
            {
                for (int y = 0; y < rowSpan; y++)
                {
                    Vector v = new Vector(cell.Coord.X + x, cell.Coord.Y + y);

                    if (cells.ContainsKey(v))
                    {
                        cells[v].Content = cells[v].btn;
                    }
                }
            }

            Grid.SetColumnSpan(cell, 1);
            Grid.SetRowSpan(cell, 1);
        }

        public void ResetGrid()
        {
            this.Width = NumColumns * CellWidth;
            this.Height = NumRows * CellHeight;

            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            for (int c = 0; c < NumColumns; c++)
            {
                ColumnDefinition def = new ColumnDefinition();
                def.Width = new GridLength(CellWidth);

                grid.ColumnDefinitions.Add(def);
            }

            for (int r = 0; r < NumRows; r++)
            {
                RowDefinition def = new RowDefinition();
                def.Height = new GridLength(CellHeight);

                grid.RowDefinitions.Add(def);
            }

            for (int c = 0; c < NumColumns; c++)
            {
                for (int r = 0; r < NumRows; r++)
                {
                    PatchworkGridCell cell = new PatchworkGridCell(DefaultItemStyle);
                    cell.Click += (sender) => CellClick?.Invoke(sender);

                    cell.Coord = new Vector(c, r);
                    Grid.SetRow(cell, r);
                    Grid.SetColumn(cell, c);
                    grid.Children.Add(cell);
                    cells.Add(cell.Coord, cell);
                }
            }
        }
    }
}
