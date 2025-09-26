using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoList
{

    public partial class MainWindow : Window
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskItem>();
            DataContext = this;
        }
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var currentTask = Tasks.LastOrDefault();

            if (currentTask != null && !string.IsNullOrWhiteSpace(currentTask.Title) &&
                !string.IsNullOrWhiteSpace(currentTask.Category) && currentTask.Priority > 0)
            {
                //Tasks.Add(new TaskItem());
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: Название, Категория и Приоритет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }


    }
}
