using MetodSimplex.Configure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Media;
using Matrix = MetodSimplex.Configure.Matrix;

namespace MetodSimplex.Visual
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Page
    {

        private List<List<Func>> values = new List<List<Func>>();
        internal List<List<Func>> Values { get => values; set => values = value; }
        internal List<Func> Function { get => function; set => function = value; }
        internal List<Func> Basis { get => basis; set => basis = value; }
        public bool Sbs { get => sbs; set => sbs = value; }
        public Frame taskFrame;
        private List<Func> function = new List<Func>();
        private List<Func> basis = new List<Func>();
        private int numberOfVariables;
        private bool sbs = false;
       

        public List<List<TextBox>> v_textBoxes;
        private List<TextBox> f_textBoxes;
        private List<TextBox> basis_TextBoxes;

        public Window3(int numberOfV, int numberOfConstraints, Frame frame)
        {
            InitializeComponent();
            taskFrame = frame;
            numberOfVariables = numberOfV;
            showFunction(0);
            showConstraints(numberOfConstraints);
        }
        public Window3(string text, Frame frame)
        {
            InitializeComponent();
            taskFrame = frame;
            try
            {
                int i = 0;
                List<Func> fr = new List<Func>();
                string[] numbers = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string f in numbers)
                {
                    Func temp = new Func(f);
                    fr.Add(temp);
                }
                if (fr[1].Denominator != 1)
                {
                    throw new Exception();
                }
                numberOfVariables = fr[1].Numerator;
                if (fr[0].Denominator != 1)
                {
                    throw new Exception();
                }
                int numberOfConstraints = fr[0].Numerator;

                int z = 2 + numberOfVariables;
                Func m = fr[2 + numberOfVariables];
                if (fr[2 + numberOfVariables].Denominator != 1)
                {
                    throw new Exception();
                }
                showFunction(fr[2 + numberOfVariables].Numerator);
                showConstraints(numberOfConstraints);
                for (i = 0; i < numberOfVariables; i++)
                {
                    function.Add(fr[(i + 2)]);
                    f_textBoxes[i].Text = function[i].ToString();
                }
                int k = numberOfVariables + 2;
                for (i = 0; i < numberOfConstraints; i++)
                {
                    values.Add(new List<Func>());
                    for (int j = 0; j < numberOfVariables + 1; j++)
                    {
                        k++;
                        values[i].Add(fr[k]);
                        v_textBoxes[i][j].Text = values[i][j].ToString();
                    }
                }
                foreach (WrapPanel wrap_panel in taskPanel.Children)
                {
                    foreach (WrapPanel wp in wrap_panel.Children)
                    {
                        var comboBox = wp.Children.OfType<ComboBox>().First();
                        if (fr[++k] == 0)
                        {
                            comboBox.SelectedIndex = 0;
                        }
                        else if (fr[k] == 1)
                        {
                            comboBox.SelectedIndex = 1;
                        }
                        else if (fr[k] == 2)
                        {
                            comboBox.SelectedIndex = 2;
                        }
                    }
                }
               
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка чтения файла");
                return;
            }
        }
        private void showFunction(int index)
        {
            v_textBoxes = new List<List<TextBox>>();
            f_textBoxes = new List<TextBox>();
            WrapPanel funcPanel = new WrapPanel()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal
            };

            functionPanel.Children.Add(funcPanel);

            TextBlock textBlock = new TextBlock()
            {
                Text = "Функция:  ",
                FontFamily = new FontFamily("Calibri"),
                FontSize = 16
            };

            funcPanel.Children.Add(textBlock);

            #region Переменные
            for (int j = 1; j < numberOfVariables; j++)
            {

                Label label = new Label()
                {
                    Name = "fx" + j,
                    Content = "x" + j + " + ",

                };


                TextBox textBox = new TextBox()
                {
                    Name = "fx" + j,
                    Height = 20,
                    Width = 30
                };

                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                textBox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
                f_textBoxes.Add(textBox);

                funcPanel.Children.Add(textBox);
                funcPanel.Children.Add(label);

            }
            #endregion
            #region Последний элемент

            Label label1 = new Label()
            {
                Name = "fx" + numberOfVariables,
                Content = "x" + numberOfVariables,

            };

            TextBox textBox1 = new TextBox()
            {
                Name = "fx" + numberOfVariables,
                Height = 20,
                Width = 30
            };
            textBox1.KeyDown += new KeyEventHandler(textBox_KeyDown);
            textBox1.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            f_textBoxes.Add(textBox1);
            funcPanel.Children.Add(textBox1);
            funcPanel.Children.Add(label1);

            #endregion
            #region Стрелочка

            Label arrow = new Label()
            {
                Content = "→"
            };
            funcPanel.Children.Add(arrow);

            #endregion
            #region Минимум или максимум

            ObservableCollection<string> list = new ObservableCollection<string>();
            list.Add("min");
           

            ComboBox cmb = new ComboBox()
            {
                Name = "minMax"
            };

            cmb.ItemsSource = list;
            cmb.SelectedIndex = index;
            funcPanel.Children.Add(cmb);
            Label gap = new Label()
            {
                Content = " "
            };
            funcPanel.Children.Add(gap);

            #endregion

        }
        private void showConstraints(int numberOfConstraints)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            list.Add("=");
          


            WrapPanel panel1 = new WrapPanel()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical
            };

            taskPanel.Children.Add(panel1);
            for (int i = 1; i < numberOfConstraints + 1; i++)
            {
                WrapPanel panel = new WrapPanel()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Orientation = Orientation.Horizontal
                };

                List<TextBox> texts = new List<TextBox>();
                panel1.Children.Add(panel);

                for (int j = 1; j < numberOfVariables; j++)
                {

                    Label label = new Label()
                    {
                        Name = "x" + j,
                        Content = "x" + j + " + ",

                    };

                    TextBox textBox = new TextBox()
                    {
                        Name = "x" + i + j,
                        Height = 20,
                        Width = 30
                    };
                    textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                    textBox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
                    texts.Add(textBox);

                    panel.Children.Add(textBox);
                    panel.Children.Add(label);

                }


                //последний элемент без "+"
                #region Последний элемент

                Label label1 = new Label()
                {
                    Name = "x" + numberOfVariables,
                    Content = "x" + numberOfVariables,

                };

                TextBox textBox1 = new TextBox()
                {
                    Name = "x" + i + numberOfVariables,
                    Height = 20,
                    Width = 30
                };
                textBox1.KeyDown += new KeyEventHandler(textBox_KeyDown);
                textBox1.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
                texts.Add(textBox1);
                panel.Children.Add(textBox1);
                panel.Children.Add(label1);

                #endregion
                //знак
                #region Знак

                ComboBox cmb = new ComboBox()
                {
                    Name = "sign" + i.ToString()
                };
                cmb.ItemsSource = list;
                cmb.SelectedItem = "=";
                panel.Children.Add(cmb);
                Label gap = new Label()
                {
                    Content = " "
                };
                panel.Children.Add(gap);

                #endregion
                //свободный член
                #region Свободный член

                TextBox textBox2 = new TextBox()
                {
                    Name = "x" + i + (numberOfVariables + 1),
                    Height = 20,
                    Width = 30
                };
                textBox2.KeyDown += new KeyEventHandler(textBox_KeyDown);
                textBox2.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
                panel.Children.Add(textBox2);
                texts.Add(textBox2);

                #endregion
                v_textBoxes.Add(texts);
            }

        }
        internal bool checkValues()
        {

            basis = new List<Func>();
            function = new List<Func>();

            foreach (TextBox ltb in f_textBoxes)
            {
                try
                {
                    if (ltb.Text == "")
                    {
                        function.Add(new Func(0));
                        ltb.Text = "0";
                    }
                    else function.Add(new Func(ltb.Text));
                }
                catch
                {
                    return false;
                }

            }
            values = new List<List<Func>>();
            foreach (List<TextBox> ltb in v_textBoxes)
            {
                List<Func> row = new List<Func>();
                foreach (TextBox tb in ltb)
                {
                    try
                    {
                        if (tb.Text == "")
                        {
                            row.Add(new Func(0));
                            tb.Text = "0";
                        }
                        else row.Add(new Func(tb.Text));
                    }
                    catch
                    {
                        return false;
                    }
                }
                values.Add(row);
            }
            
            return true;
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\\,-/]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void vector()
        {
            basis_TextBoxes = new List<TextBox>();
            WrapPanel panel = new WrapPanel();
            vectorPanel1.Children.Add(panel);
            #region Поля, кроме последнего
            Label label = new Label()
            {
                Content = "X0 = ("
            };
            panel.Children.Add(label);

            for (int i = 1; i < numberOfVariables; i++)
            {
                TextBox tb = new TextBox()
                {
                    Height = 20,
                    Width = 30
                };
                Label lb = new Label()
                {
                    Content = " , "
                };
                tb.KeyDown += new KeyEventHandler(textBox_KeyDown);
                tb.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
                basis_TextBoxes.Add(tb);
                panel.Children.Add(tb);
                panel.Children.Add(lb);
            }
            #endregion
            #region Последнее поле
            TextBox tb1 = new TextBox()
            {
                Height = 20,
                Width = 30
            };
            Label lb1 = new Label()
            {
                Content = " )"
            };
            tb1.KeyDown += new KeyEventHandler(textBox_KeyDown);
            tb1.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            basis_TextBoxes.Add(tb1);
            panel.Children.Add(tb1);
            panel.Children.Add(lb1);
            #endregion
        }
        #region Пошаговое решение
        private void Window4_Click(object sender, RoutedEventArgs e)
        {
            if (Window4.IsChecked == true)
                sbs = true;
            else
                sbs = false;
        }
        private void sbs_label(object sender, MouseButtonEventArgs e)
        {
            if (Window4.IsChecked == true)
            {
                Window4.IsChecked = false;
                sbs = false;
            }
            else
            {
                Window4.IsChecked = true;
                sbs = true;
            }
        }
        #endregion
        private void ByBasis_Click(object sender, RoutedEventArgs e)
        {
            Table tab = new Table();
            if (!checkValues())
            {
                string message = "Некорректные данные. Проверьте соответствие данных следующим правилам:";
                message += "\r\n - Дроби с нулем в знаменателе недопустимы;";
                message += "\r\n - При оформлении дробей используйте знак '/', не '\\';";
                message += "\r\n - Проверьте отсутствие букв и других символов, не являющихся числами, запятой или знаком деления;";
                message += "\r\n - Число ненулевых переменных в базисе должно совпадать с рангом введенной матрицы";
                MessageBox.Show(message);
                return;
            }
            GoToDefault();

            tab.Old_function = function;
            //формируем часть базисного вектора
            basis = new List<Func>();
            function = new List<Func>();
            for (int i = 0; i < values[0].Count - 1; i++)
            {
                basis.Add(new Func(0));
                function.Add(new Func(0));
            }

            //добавляем новые переменные в матрицу и заканчиваем формирование вектора
            for (int i = 0; i < values.Count; i++)
            {
                basis.Add(new Func(1));
                function.Add(new Func(1));
                Func m = values[i][values[i].Count - 1];
                values[i].RemoveAt(values[i].Count - 1);
                for (int j = 0; j < values.Count; j++)
                    if (i == j)
                        values[i].Add(new Func(1));
                    else values[i].Add(new Func(0));
                values[i].Add(m);
            }
            Matrix matrix = new Matrix(values);
            tab.Function = function;
            tab.Basis = basis;

            if (matrix.GaussMethod(basis) == -1)
            {

                MessageBox.Show("Система ограничений противоречива либо с помощью выбранного вектора невозможно провести решение.");
                return;
            }
           
            tab.Matrix = matrix;
            Table table = new Table(tab, true, true);
            if (Sbs)
            {
                Window4 Window4Visual = new Window4(table, taskFrame);
                taskFrame.Content = Window4Visual;
            }
            else
            {
                table.isWindow2 = false;
                while (table.isWindow2 == false)
                {
                    table = table.StepSimplexMethod(-1, -1);
                    if (table.isArtifical)
                    {
                        int finished = table.IsEndOfArtificialBasis();
                        if (finished > 0)
                        {
                            
                            table.MakeFunction();
                            table.isWindow2 = false;
                         

                            continue;
                        }
                    }
                }
                if (table.isWindow2)
                {
                    Window2 Window2View = new Window2(table);
                    taskFrame.Content = Window2View;
                }

            }

        }
        private void GoToDefault()
        {
            //домножаем функцию на -1, если выбран максимум
            foreach (WrapPanel panel in functionPanel.Children)
            {
                var comboBox = panel.Children.OfType<ComboBox>().First();
                if (comboBox.SelectedIndex == 1)
                {
                    foreach (Func k in function)
                    {
                        k.Sign *= (-1);
                    }
                }
            }
            int i = 0;
            //добавление новых переменных в случае неравенств
            foreach (WrapPanel panel in taskPanel.Children)
            {
                foreach (WrapPanel panel1 in panel.Children)
                {
                    var comboBox = panel1.Children.OfType<ComboBox>().First();
                    if (comboBox.SelectedIndex == 1)
                    {
                        function.Add(new Func(0));
                        for (int j = 0; j < values.Count; j++)
                        {
                            if (i == j)
                            {
                                Func a = values[j][values[j].Count() - 1];
                                values[j].Insert(values[j].Count() - 1, new Func(-1));
                                values[j].Add(a);
                            }
                            else
                            {
                                Func a = values[j][values[j].Count() - 1];
                                values[j].Insert(values[j].Count() - 1, new Func(0));
                                values[j].Add(a);
                            }
                        }
                    }
                    else if (comboBox.SelectedIndex == 2)
                    {
                        function.Add(new Func(0));
                        for (int j = 0; j < values.Count; j++)
                        {
                            if (i == j)
                            {
                                Func a = values[j][values[j].Count() - 1];
                                values[j].Insert(values[j].Count() - 1, new Func(1));
                                values[j].Add(a);
                            }
                            else
                            {
                                Func a = values[j][values[j].Count() - 1];
                                values[j].Insert(values[j].Count() - 1, new Func(0));
                                values[j].Add(a);
                            }
                        }
                    }
                    i++;
                }
            }
            //смена знака при свободном элементе, меньшем нуля
            foreach (List<Func> k in values) //
            {
                if (k[k.Count - 1].Sign == -1)
                {
                    foreach (Func f in k)
                        f.Sign *= (-1);
                }
            }
        }


    }
    
}
