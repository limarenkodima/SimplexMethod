using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using MetodSimplex.Configure;

namespace MetodSimplex.Visual
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    /// 
    public partial class Window4 : Page
    {
        Configure.Table table;
        Frame frame;
        int col, row;
        private List<int> basisVariables;
        private List<int> freeVariables;
        public Window4(Configure.Table simplexTable, Frame fr)
        {
            InitializeComponent();
            table = simplexTable;
            basisVariables = simplexTable.BasisVariables;
            freeVariables = simplexTable.FreeVariables;
            frame = fr;
            showStep();
        }

        public void showStep()
        {
            WrapPanel mPanel = new WrapPanel()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical
            };
            matrixPanel.Children.Add(mPanel);

            ToolTip toolTip = new ToolTip()
            {
                Background = Brushes.Gray,
                Foreground = Brushes.White,
                HasDropShadow = false,
                Opacity = 0.9,
                Content = "При нажатии элемент будет опорным"
            };

            Grid grid = new Grid();
            matr.Children.Add(grid);
            grid.ShowGridLines = true;
            for (int i = 0; i < table.BasisVariables.Count + 2; i++)
            {
                RowDefinition c1 = new RowDefinition();
                grid.RowDefinitions.Add(c1);
            }

            for (int i = 0; i < table.FreeVariables.Count + 2; i++)
            {
                ColumnDefinition c1 = new ColumnDefinition();
                grid.ColumnDefinitions.Add(c1);
            }

            for (int j = 0; j < table.FreeVariables.Count; j++)
            {
                TextBlock y = new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = " x" + (table.FreeVariables[j] + 1) + " ",
                    Height = 50,
                    MinWidth = 50,
                    FontSize = 22
                };
                Grid.SetRow(y, 0);
                Grid.SetColumn(y, j + 1);
                grid.Children.Add(y);
            }

            col = table.FindMinCol();
            row = table.FindRow(col);

            for (int i = 0; i < table.BasisVariables.Count; i++)
            {
                TextBlock x = new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = "  x" + (table.BasisVariables[i] + 1) + " ",
                    Height = 50,
                    MinWidth = 50,
                    FontSize = 22
                };
                Grid.SetRow(x, i + 1);
                Grid.SetColumn(x, 0);
                grid.Children.Add(x);


                for (int j = 0; j < table.FreeVariables.Count + 1; j++)
                {
                    int row1 = -1;
                    if (table.Matrix.values[table.Matrix.values.Count - 1][j] < 0)
                        row1 = table.FindRow(j);
                    TextBlock y = new TextBlock()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = "  " + table.Matrix.values[i][j].ToString() + "  ",
                        Height = 50,
                        MinWidth = 50,
                        FontSize = 22
                    };
                    Grid.SetRow(y, i + 1);
                    Grid.SetColumn(y, j + 1);
                    if (i == row && j == col)
                    {
                        //выбор цвета опорного элемента
                        y.Background = new SolidColorBrush(Color.FromRgb(148, 255, 151));
                        y.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(TextBox_Click);
                        y.ToolTip = toolTip;

                    }
                    else if (i == row1 && j != table.FreeVariables.Count && table.FreeVariables.Count != 1 && row != -1 && col != -1)
                    {
                        y.Background = new SolidColorBrush(Color.FromRgb(111, 255, 151));
                        y.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(TextBox_Click);
                        y.ToolTip = toolTip;
                    }
                    grid.Children.Add(y);
                }
                for (int j = 0; j < table.FreeVariables.Count + 1; j++)
                {
                    TextBlock y = new TextBlock()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = "  " + table.Matrix.values[table.BasisVariables.Count][j].ToString() + "  ",
                        Height = 50,
                        MinWidth = 50,
                        FontSize = 22
                    };
                    Grid.SetRow(y, table.BasisVariables.Count + 1);
                    Grid.SetColumn(y, j + 1);
                    grid.Children.Add(y);
                }


            }
        }
        //урезанная таблица
        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            Configure.Table tab = (Configure.Table)table.Clone();
            List<int> bVariables = new List<int>();
            for (int i = 0; i < basisVariables.Count; i++)
                bVariables.Add((int)basisVariables[i]);
            List<int> fVariables = new List<int>();
            for (int i = 0; i < freeVariables.Count; i++)
                fVariables.Add((int)freeVariables[i]);
            tab.BasisVariables = bVariables;
            tab.FreeVariables = fVariables;
            Configure.Table table1 = new Configure.Table(tab, false, tab.isArtifical);

            if (table1.isArtifical)
            {
                int finished = table1.IsEndOfArtificialBasis();
                if (finished > 0)
                {
                    //пересчет
                    table1.MakeFunction();
                    table1.isWindow2 = false;
                    //table1.StepSimplexMethod(row, col);
                    Window4 Window41 = new Window4(table1, frame);
                    frame.Content = Window41;
                    return;
                }
            }
            table1.StepSimplexMethod(row, col);
            if (table1.isWindow2 == true)
            {
                Window2 Window2View = new Window2(table1);
                frame.Content = Window2View;
            }
            else
            {
                Window4 Window4 = new Window4(table1, frame);
                frame.Content = Window4;
            }

        }

        private void TextBox_Click(object sender, RoutedEventArgs e)

        {
            TextBlock current = (TextBlock)sender;
            Func currentValue = new Func(current.Text);
            if (currentValue <= 0)
            {
                MessageBox.Show("Выбрано неположительное значение!");
                return;
            }

            int _row = (int)current.GetValue(Grid.RowProperty);
            int _column = (int)current.GetValue(Grid.ColumnProperty);

            Configure.Table tab = (Configure.Table)table.Clone();
            List<int> bVariables = new List<int>();
            for (int i = 0; i < basisVariables.Count; i++)
                bVariables.Add((int)basisVariables[i]);
            List<int> fVariables = new List<int>();
            for (int i = 0; i < freeVariables.Count; i++)
                fVariables.Add((int)freeVariables[i]);
            tab.BasisVariables = bVariables;
            tab.FreeVariables = fVariables;
            Configure.Table table1 = new Configure.Table(tab, false, tab.isArtifical);

            table1.StepSimplexMethod(_row - 1, _column - 1);
            Window4 Window41 = new Window4(table1, frame);
            frame.Content = Window41;
            return;
        }

    }

}
