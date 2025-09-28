using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<CategoryItem> Categories { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskItem>();
            Categories = new ObservableCollection<CategoryItem>
            {
                new CategoryItem { Name = "All", IsSelected = true }
            };
            DataContext = this;

            foreach (var category in Categories)
            {
                category.PropertyChanged += Category_PropertyChanged;
            }
        }
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var currentTask = Tasks.LastOrDefault();

            if (currentTask != null && !string.IsNullOrWhiteSpace(currentTask.Title) &&
                !string.IsNullOrWhiteSpace(currentTask.Category) && currentTask.Priority > 0)
            {    
                if (!Categories.Any(c => c.Name.Equals(currentTask.Category, StringComparison.OrdinalIgnoreCase)))
                {

                    Categories.Add(new CategoryItem { Name = currentTask.Category, IsSelected = true });
                    SortTasks();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: Название, Категория и Приоритет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {

            var selectedTask = TasksDataGrid.SelectedItem as TaskItem;

            if (selectedTask != null && Tasks.Count>1)
            {
                Tasks.Remove(selectedTask);
                SortTasks();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }

        private void FilterTasks()
        {
            var selectedCategories = Categories.Where(c => c.IsSelected).Select(c => c.Name).ToList();

            if (selectedCategories.Contains("All"))
            {
                TasksDataGrid.ItemsSource = Tasks;
            }
            else
            {
                var filteredTasks = Tasks.Where(t => selectedCategories.Contains(t.Category)).ToList();
                TasksDataGrid.ItemsSource = filteredTasks;
            }
        }

        private void Category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CategoryItem.IsSelected))
            {
                FilterTasks();
            }
        }

        private void SortTasks()
        {
            var sortedTasks = Tasks
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ThenBy(t => t.Title)
                .ToList();
            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }
    }
}
