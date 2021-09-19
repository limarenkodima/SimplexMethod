using MetodSimplex.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MetodSimplex.Visual
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Page
    {
        Configure.Table table;
        public Window2(Configure.Table simplexTable)
        {
            InitializeComponent();
            table = simplexTable;
            showWindow2();
        }
        public void showWindow2()
        {
            WrapPanel mPanel = new WrapPanel()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical
            };
            matrixPanel.Children.Add(mPanel);

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
                    Height = 30,
                    MinWidth = 30,
                    FontSize = 18
                };
                Grid.SetRow(y, 0);
                Grid.SetColumn(y, j + 1);
                grid.Children.Add(y);
            }

            for (int i = 0; i < table.BasisVariables.Count; i++)
            {
                TextBlock x = new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = "  x" + (table.BasisVariables[i] + 1) + " ",
                    Height = 30,
                    MinWidth = 30,
                    FontSize = 18
                };
                Grid.SetRow(x, i + 1);
                Grid.SetColumn(x, 0);
                grid.Children.Add(x);

                for (int j = 0; j < table.FreeVariables.Count + 1; j++)
                {
                    TextBlock y = new TextBlock()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = "  " + table.Matrix.values[i][j].ToString() + "  ",
                        Height = 30,
                        MinWidth = 30,
                        FontSize = 18
                    };
                    Grid.SetRow(y, i + 1);
                    Grid.SetColumn(y, j + 1);
                    grid.Children.Add(y);
                }
                for (int j = 0; j < table.FreeVariables.Count + 1; j++)
                {
                    TextBlock y = new TextBlock()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = "  " + table.Matrix.values[table.BasisVariables.Count][j].ToString() + "  ",
                        Height = 30,
                        MinWidth = 30,
                        FontSize = 18
                    };
                    Grid.SetRow(y, table.BasisVariables.Count + 1);
                    Grid.SetColumn(y, j + 1);
                    grid.Children.Add(y);
                }
            }

            string text = "Ответ: \r\n";
            if (table.KindOfWindow2 == 0)
            {
                text += "Решений нет. ";
            }
            else
            {
                List<Func> f = new List<Func>();
                for (int i = 0; i < table.BasisVariables.Count + table.FreeVariables.Count; i++)
                {
                    f.Add(new Func());
                }
                text += "x* = (";
                for (int i = 0; i < table.BasisVariables.Count; i++)
                {
                    int k = table.BasisVariables[i];
                    Func answ = table.Matrix.values[i][table.Matrix.values[i].Count - 1];
                    f[k] = answ;
                }
                for (int i = 0; i < table.FreeVariables.Count; i++)
                {
                    int k = table.FreeVariables[i];
                    f[k] = new Func(0);
                }
                for (int i = 0; i < table.BasisVariables.Count + table.FreeVariables.Count; i++)
                {
                    text += f[i] + ", ";
                }
                text += ")";
                text += "f(x*) = " + table.Matrix.values[table.Matrix.values.Count - 1][table.Matrix.values[0].Count - 1] * -1;
            }
            Window2Label.FontSize = 18;
            Window2Label.Content = text;
        }
    }
}
