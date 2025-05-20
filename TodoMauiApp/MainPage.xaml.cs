using Microsoft.Maui.Controls;
using Microsoft.Maui.Accessibility;
using TodoMauiApp.Data;
using TodoMauiApp.Models;
using TodoMauiApp.Services;
using TodoMauiApp.ViewModels;

namespace TodoMauiApp;

public partial class MainPage : ContentPage
{
    private readonly TodoListViewModel _viewModel;

    public MainPage(TodoListViewModel viewModel)
    {
        InitializeComponent();
        
        _viewModel = viewModel;
        BindingContext = _viewModel;
        
        // Load initial data
        _viewModel.LoadTodosCommand.Execute(null);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadTodosCommand.Execute(null);
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is TodoItem todoItem)
        {
            System.Diagnostics.Debug.WriteLine($"Toggling todo item: {todoItem.Title}, IsDone: {todoItem.IsDone}");
            _viewModel.ToggleTodoCommand.Execute(todoItem);
        }
    }
}
