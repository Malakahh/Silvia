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
        public event Action<PatchworkGridItem> ItemClick;

        public int NumColumns { get; set; }
        public int NumRows { get; set; }
        public int CellWidth { get; set; }
        public int CellHeight { get; set; }
        public Style DefaultItemStyle { get; set; }

        private Grid grid = new Grid();
        List<Image> imgs = new List<Image>();

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
            UpdateGrid();
        }

        public void UpdateGrid()
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
                    PatchworkGridItem item = new PatchworkGridItem(DefaultItemStyle);
                    item.Click += (sender) => ItemClick?.Invoke(sender);

                    item.coord = new Vector(c, r);
                    Grid.SetRow(item, r);
                    Grid.SetColumn(item, c);
                    grid.Children.Add(item);
                }
            }
        }
    }
}
