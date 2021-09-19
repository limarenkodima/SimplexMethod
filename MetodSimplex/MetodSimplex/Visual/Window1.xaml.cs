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
using System.Windows.Shapes;

namespace MetodSimplex.Visual
{
    /// <summary>
    /// Логика взаимодействия для AddWindow3.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private bool isFilled;
        public bool IsFilled
        {
            get { return isFilled; }
        }

        
        public int NumberOfVariables
        {
            get
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                return Convert.ToInt32(selectedItem.Content.ToString());
            }
        }

        
        public int NumberOfConstraints
        {
            get
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
                return Convert.ToInt32(selectedItem.Content.ToString());
            }
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            isFilled = true;
            Close();
        }

    }
}
