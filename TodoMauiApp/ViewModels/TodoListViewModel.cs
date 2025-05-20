using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoMauiApp.Data;
using TodoMauiApp.Models;

namespace TodoMauiApp.ViewModels
{
    public class TodoListViewModel : INotifyPropertyChanged
    {
        private readonly TodoRepository _repository;
        private TodoItem? _selectedTodo;
        private bool _isBusy;
        private string _newTodoTitle = string.Empty;

        public ObservableCollection<TodoItem> Todos { get; } = new ObservableCollection<TodoItem>();

        public TodoItem? SelectedTodo
        {
            get => _selectedTodo;
            set
            {
                if (_selectedTodo != value)
                {
                    _selectedTodo = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewTodoTitle
        {
            get => _newTodoTitle;
            set
            {
                if (_newTodoTitle != value)
                {
                    _newTodoTitle = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"NewTodoTitle changed to: {value}");
                }
            }
        }

        public ICommand LoadTodosCommand { get; }
        public ICommand AddTodoCommand { get; }
        public ICommand ToggleTodoCommand { get; }
        public ICommand DeleteTodoCommand { get; }

        public TodoListViewModel(TodoRepository repository)
        {
            _repository = repository;
            
            LoadTodosCommand = new Command(async () => await LoadTodosAsync());
            AddTodoCommand = new Command(async () => await AddTodoAsync(), () => !string.IsNullOrWhiteSpace(NewTodoTitle));
            ToggleTodoCommand = new Command<TodoItem>(async (item) => await ToggleTodoAsync(item));
            DeleteTodoCommand = new Command<TodoItem>(async (item) => await DeleteTodoAsync(item));

            PropertyChanged += (_, args) => 
            {
                if (args.PropertyName == nameof(NewTodoTitle))
                {
                    ((Command)AddTodoCommand).ChangeCanExecute();
                }
            };

            Debug.WriteLine("TodoListViewModel initialized");
        }

        private async Task LoadTodosAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Debug.WriteLine("Loading todos...");

            try
            {
                Todos.Clear();
                var items = await _repository.GetAllAsync();
                foreach (var item in items)
                {
                    Todos.Add(item);
                }
                Debug.WriteLine($"Loaded {items.Count} todos");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading todos: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load tasks", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddTodoAsync()
        {
            if (string.IsNullOrWhiteSpace(NewTodoTitle) || IsBusy)
                return;

            IsBusy = true;
            Debug.WriteLine($"Adding new todo: {NewTodoTitle}");

            try
            {
                var newTodo = new TodoItem(0, NewTodoTitle, DateTimeOffset.Now, false, 0);
                await _repository.AddAsync(newTodo);
                Debug.WriteLine($"Added todo with ID: {newTodo.Id}");
                
                NewTodoTitle = string.Empty;
                await LoadTodosAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding todo: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to add task", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ToggleTodoAsync(TodoItem item)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Debug.WriteLine($"Toggling todo ID {item.Id}, Title: {item.Title}, from: {item.IsDone} to {!item.IsDone}");

            try
            {
                var updatedItem = item with { IsDone = !item.IsDone };
                await _repository.UpdateAsync(updatedItem);
                Debug.WriteLine($"Toggled todo ID {item.Id}");
                await LoadTodosAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error toggling todo: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update task", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteTodoAsync(TodoItem item)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Debug.WriteLine($"Deleting todo ID {item.Id}, Title: {item.Title}");

            try
            {
                await _repository.DeleteAsync(item.Id);
                Debug.WriteLine($"Deleted todo ID {item.Id}");
                await LoadTodosAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting todo: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete task", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 