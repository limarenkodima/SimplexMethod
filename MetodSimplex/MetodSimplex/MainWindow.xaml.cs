using Microsoft.Win32;
using MetodSimplex.Configure;
using MetodSimplex.Visual;
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
using System.IO;

namespace MetodSimplex
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window3 task;
        public MainWindow()
        {
            InitializeComponent();
        }

        public Frame getFrame()
        {
            return taskFrame;
        }

       //кнопка справка
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            new Spravka().ShowDialog();
        }

        private void OpenTask_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt";
            openFileDialog.ShowDialog();
            string filename = openFileDialog.FileName;
            try
            {
                string text = System.IO.File.ReadAllText(filename);
                task = new Window3(text, taskFrame);
                taskFrame.Content = task;
            }
            catch (Exception)
            {
                return;
            }

        }
        // кнопка сейв
        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            //if (task == null)
            //{
            //    MessageBox.Show("Сначала нужно создать задачу!");
            //    return;
            //}
            //if (!task.checkValues())
            //{
            //    string message = "Некорректные данные. Проверьте соответствие данных следующим правилам:";
            //    message += "\r\n - Дроби с нулем в знаменателе недопустимы;";
            //    message += "\r\n - При оформлении дробей используйте знак '/', не '\\';";
            //    message += "\r\n - Проверьте отсутствие букв и других символов, не являющихся числами, запятой или знаком деления;";
            //    MessageBox.Show(message);
            //    return;
            //}
            //SaveFileDialog saveFile = new SaveFileDialog();
            //saveFile.Filter = "Text files(*.txt)|*.txt";
            //saveFile.ShowDialog();
            //saveFile.ToString();


            //// получаем выбранный файл

            //string filename = saveFile.FileName;
            //StreamWriter writer = new StreamWriter(filename);
            //string text = task.Values.Count.ToString();
            //writer.WriteLine(text);
            //text = task.Values.Count.ToString();
            //writer.WriteLine(text);
            //int cl = int.Parse(task.Values.Count.ToString());
            //if (saveFile.ShowDialog() == DialogResult)
            //{
            //    return;
            //}
            ////if (sfd.ShowDialog() == DialogResult.Cancel)
            ////{
            ////    return;
            ////}
            ////FileStream File.OpenRead(string file);
            ////FileStream File.OpenWrite(string file);


            ////SaveFileDialog sfd = new SaveFileDialog();
            ////sfd.Filter = "Text files(*.txt)|*.txt";
            ////sfd.RestoreDirectory = true;
            ////sfd.Title = "Сохранить";
            ////sfd.DefaultExt = "txt";
            ////sfd.CreatePrompt = true;
            ////sfd.OverwritePrompt = true;
            ////sfd.ShowDialog();
            ////string fname = sfd.FileName;
            ////StreamWriter writer = new StreamWriter(fname);
            ////string col = columnBox.SelectedItem.ToString();
            ////writer.WriteLine(col);
            ////col = rowBox.SelectedItem.ToString();
            ////writer.WriteLine(col);
            ////int cl = int.Parse(columnBox.SelectedItem.ToString());
            ////if (sfd.ShowDialog() == DialogResult.Cancel)
            ////{
            ////    return;
            ////}


            //// сохраняем текст в файл


            ////foreach (Func f in task.Function)
            ////{
            ////    text = " ";
            ////    for (int i = 0; i < cl + 2; i++)
            ////    {

            ////        text += f.ToString() + " ";
            ////        if (i != cl + 1)
            ////        {
            ////            text += " ";
            ////        }
            ////    }
            ////    writer.WriteLine(text);

            ////    writer.Close();
            ////    MessageBox.Show("Задача сохранена в файл.");
            ////}






            //text += task.Values.Count + " " + (task.Values[0].Count - 1) + " ";
            //foreach (Func f in task.Function)
            //{
            //    text += f.ToString() + " ";
            //}
            //text += " ";
            //foreach (WrapPanel wp in task.functionPanel.Children)
            //{
            //    var comboBox = wp.Children.OfType<ComboBox>().First();
            //    if (comboBox.SelectedIndex == 0)
            //    {
            //        text += "0 ";
            //    }
            //    else
            //    {
            //        text += "1 ";
            //    }
            //}
            //for (int i = 0; i < task.Values.Count; i++)
            //{
            //    for (int j = 0; j < task.Values[0].Count; j++)
            //    {
            //        text += task.Values[i][j].ToString() + " ";
            //    }

            //    text += " ";
            //}
            //foreach (WrapPanel panel in task.taskPanel.Children)
            //{
            //    foreach (WrapPanel wp in panel.Children)
            //    {
            //        var comboBox = wp.Children.OfType<ComboBox>().First();
            //        if (comboBox.SelectedIndex == 0)
            //        {
            //            text += "0 ";
            //        }
            //        else if (comboBox.SelectedIndex == 1)
            //        {
            //            text += "1 ";
            //        }
            //        else
            //        {
            //            text += "2 ";
            //        }
            //    }
            //    writer.WriteLine(text);

            //      writer.Close();
            //      MessageBox.Show("Задача сохранена в файл.");


            //    //SaveFileDialog sfd = new SaveFileDialog();
            //    //sfd.Filter = "Text files(*.txt)|*.txt";
            //    //sfd.RestoreDirectory = true;
            //    //sfd.Title = "Сохранить";
            //    //sfd.DefaultExt = "txt";
            //    //sfd.CreatePrompt = true;
            //    //sfd.OverwritePrompt = true;
            //    //if (sfd.ShowDialog() == DialogResult.Cancel)
            //    //{
            //    //    return;
            //    //}
            //    //string fname = sfd.FileName;
            //    //StreamWriter writer = new StreamWriter(fname);
            //    //string col = columnBox.SelectedItem.ToString();
            //    //writer.WriteLine(col);
            //    //col = rowBox.SelectedItem.ToString();
            //    //writer.WriteLine(col);
            //    //int cl = int.Parse(columnBox.SelectedItem.ToString());
            //    //foreach (DataGridViewRow row in dataGridView1.Rows)
            //    //{
            //    //    col = "";
            //    //    for (int i = 0; i < cl + 2; i++)
            //    //    {
            //    //        col += row.Cells[i].Value.ToString();
            //    //        if (i != cl + 1)
            //    //        {
            //    //            col += " ";
            //    //        }
            //    //    }
            //    //    writer.WriteLine(col);
            //    //}
            //    //writer.Close();
            //    //MessageBox.Show("Задача сохранена в файл.");
            //}





            if (task == null)
            {
                MessageBox.Show("Сначала нужно создать задачу!");
                return;
            }
            if (!task.checkValues())
            {
                string message = "Некорректные данные. Проверьте соответствие данных следующим правилам:";
                message += "\r\n - Дроби с нулем в знаменателе недопустимы;";
                message += "\r\n - При оформлении дробей используйте знак '/', не '\\';";
                message += "\r\n - Проверьте отсутствие букв и других символов, не являющихся числами, запятой или знаком деления;";
                MessageBox.Show(message);
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text files(*.txt)|*.txt";
            saveFile.ShowDialog();
            // получаем выбранный файл
            string filename = saveFile.FileName;
            // сохраняем текст в файл
            string text = "";
            text += task.Values.Count + " " + (task.Values[0].Count - 1) + " ";
            foreach (Func f in task.Function)
            {
                text += f.ToString() + " ";
            }
            text += " ";
            foreach (WrapPanel wp in task.functionPanel.Children)
            {
                var comboBox = wp.Children.OfType<ComboBox>().First();
                if (comboBox.SelectedIndex == 0)
                {
                    text += "0 ";
                }
                else
                {
                    text += "1 ";
                }
            }
            for (int i = 0; i < task.Values.Count; i++)
            {
                for (int j = 0; j < task.Values[0].Count; j++)
                {
                    text += task.Values[i][j].ToString() + " ";
                }

                text += " ";
            }
            foreach (WrapPanel panel in task.taskPanel.Children)
            {
                foreach (WrapPanel wp in panel.Children)
                {
                    var comboBox = wp.Children.OfType<ComboBox>().First();
                    if (comboBox.SelectedIndex == 0)
                    {
                        text += "0 ";
                    }
                    else if (comboBox.SelectedIndex == 1)
                    {
                        text += "1 ";
                    }
                    else
                    {
                        text += "2 ";
                    }
                }
            }
            //if (task.isVectorNeeded.IsChecked == true)

            text += "1 ";
            foreach (Func f in task.Basis)
            {
                text += f.ToString() + " ";
            }
                text += "0 ";
                text += " ";
                try
                {
                    System.IO.File.WriteAllText(filename, text);
                }
                catch (Exception)
                {
                    return;
                }
                MessageBox.Show("Файл сохранен");
            

        }

       
        // создать задачу
        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            Window1 newTask = new Window1();
            newTask.ShowDialog();
            if (newTask.IsFilled == true)
            {
                task = new Window3(newTask.NumberOfVariables, newTask.NumberOfConstraints, taskFrame);
                taskFrame.Content = task;
            }
        }
    }
}
