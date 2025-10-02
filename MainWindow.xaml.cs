using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ToDoList
{
    public partial class MainWindow : Window
    {
        private readonly ToDoManager _manager;
        private const string DataFile = "tasks.json";

        public MainWindow()
        {
            InitializeComponent();

            _manager = new ToDoManager();
            _manager.Load(DataFile);

            DataContext = _manager;

            _manager.SubscribeCategoryChanges(Category_PropertyChanged);

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            _manager.Save(DataFile);
            _manager.UnsubscribeCategoryChanges(Category_PropertyChanged);
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_manager.TryAddLastTask(out var error))
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TasksDataGrid.SelectedItem as TaskItem;
            if (!_manager.TryDeleteTask(selectedTask, out var error))
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) e.Handled = true;
        }

        private void Category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CategoryItem.IsSelected))
            {
                TasksDataGrid.ItemsSource = (IEnumerable<TaskItem>)_manager.GetFilteredTasks();
            }
        }
    }
}
