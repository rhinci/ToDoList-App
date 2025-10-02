using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ToDoList
{
    public class ToDoManager
    {
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<CategoryItem> Categories { get; } = new ObservableCollection<CategoryItem>();

        public ToDoManager()
        {
            Categories.Add(new CategoryItem { Name = "All", IsSelected = true });
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var tasks = JsonSerializer.Deserialize<ObservableCollection<TaskItem>>(json);
                if (tasks != null)
                {
                    Tasks.Clear();
                    foreach (var t in tasks) Tasks.Add(t);

   
                    foreach (var catName in Tasks.Select(t => t.Category).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct(StringComparer.OrdinalIgnoreCase))
                    {
                        if (!Categories.Any(c => c.Name.Equals(catName, StringComparison.OrdinalIgnoreCase)))
                            Categories.Add(new CategoryItem { Name = catName, IsSelected = false });
                    }
                }
            }
        }

        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(Tasks);
            File.WriteAllText(filePath, json);
        }

        public bool TryAddLastTask(out string errorMessage)
        {
            errorMessage = null;
            var currentTask = Tasks.LastOrDefault();
            if (currentTask == null)
            {
                errorMessage = "Нет задачи для добавления.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(currentTask.Title) ||
                string.IsNullOrWhiteSpace(currentTask.Category) ||
                currentTask.Priority <= 0)
            {
                errorMessage = "Пожалуйста, заполните обязательные поля: Название, Категория и Приоритет.";
                return false;
            }

            if (!Categories.Any(c => c.Name.Equals(currentTask.Category, StringComparison.OrdinalIgnoreCase)))
            {
                Categories.Add(new CategoryItem { Name = currentTask.Category, IsSelected = true });
            }

            return true;
        }

        public bool TryDeleteTask(TaskItem task, out string errorMessage)
        {
            errorMessage = null;
            if (task == null)
            {
                errorMessage = "Пожалуйста, выберите задачу для удаления.";
                return false;
            }

            if (Tasks.Count <= 1)
            {
                errorMessage = "Нельзя удалить последнюю задачу.";
                return false;
            }

            return Tasks.Remove(task);
        }

        public object GetFilteredTasks()
        {
            var selected = Categories.Where(c => c.IsSelected).Select(c => c.Name).ToList();
            if (selected.Contains("All"))
                return Tasks;
            return Tasks.Where(t => selected.Contains(t.Category)).ToList();
        }

        public void SubscribeCategoryChanges(PropertyChangedEventHandler handler)
        {
            foreach (var c in Categories)
                c.PropertyChanged += handler;
        }

        public void UnsubscribeCategoryChanges(PropertyChangedEventHandler handler)
        {
            foreach (var c in Categories)
                c.PropertyChanged -= handler;
        }
    }
}
