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
    /// Логика взаимодействия для HelpView.xaml
    /// </summary>
    public partial class Spravka : Window
    {
        public Spravka()
        {

            InitializeComponent();
            string text = "Приложение предназначено для решения задач линейного программирования "
                + "методом искусственного базиса. \r\n "
                + "\r\n"
                + "Чтобы создать новую задачу, выполните следующие действия:\r\n"
                + "1. Нажмите кнопку 'Создать задачу' на верхней панели; \r\n"
                + "2. Во всплывающем окне введите количество переменных и количество ограничений задачи;\r\n"
                + "3. Введите данные задачи в появившуюся форму.\r\n \r\n"
                + "Чтобы сохранить задачу:\r\n"
                + "1. Создайте новую задачу или внесите правки в существующую;\r\n"
                + "2. На верхней панели выберите пункт 'Файл', в выпавшем меню выберите пункт 'Сохранить задачу в файл';\r\n"
                + "3. Введите имя файла и нажмите кнопку 'Сохранить'."
                + "\r\n\r\n Чтобы увидеть пошаговое решение, поставьте галочку на пункте 'Действие для режими шагами' при заполнении значений задачи."
                + "При пошаговом решении опорные элементы можно выбрать одинарным кликом по элементу, доступному для выбора (такие ячейки выделены цветом, указанным в правой панели).";
            textBlock.Text = text;

        }
    }
}
