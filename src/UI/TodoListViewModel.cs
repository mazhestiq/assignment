using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using Assignment.Application.TodoLists.Commands.CreateTodoList;
using Caliburn.Micro;
using MediatR;

namespace Assignment.UI;
public class TodoListViewModel : Screen, INotifyDataErrorInfo
{
    private readonly ISender _sender;
    private readonly Dictionary<string, ICollection<string>> _validationErrors = new();
    private readonly string[] _existingTitles;

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            ValidateTitleName();
            NotifyOfPropertyChange(() => Title);
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand CloseCommand { get; }

    public TodoListViewModel(ISender sender, string[] existingTitles)
    {
        _sender = sender;
        _existingTitles = existingTitles;
        Title = null;
        SaveCommand = new RelayCommand(SaveExecute);
        CloseCommand = new RelayCommand(CloseExecute);
    }

    private async void SaveExecute(object parameter)
    {
        await _sender.Send(new CreateTodoListCommand(Title));
        await TryCloseAsync(true);
    }

    private async void CloseExecute(object parameter)
    {
        await TryCloseAsync(false);
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return _validationErrors.ContainsKey(propertyName) ?
            _validationErrors[propertyName] : null;
    }

    public bool HasErrors => _validationErrors.Any();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    private void ValidateTitleName()
    {
        ClearErrors(nameof(Title));
        if (string.IsNullOrWhiteSpace(Title))
            AddError(nameof(Title), "Title cannot be empty.");

        if (Title?.Length >= 200)
            AddError(nameof(Title), "The title of a ToDoList cannot exceed 200 characters.");

        if (_existingTitles.Contains(Title))
            AddError(nameof(Title), "The title already in use.");
    }

    private void AddError(string propertyName, string error)
    {
        if (!_validationErrors.ContainsKey(propertyName))
            _validationErrors[propertyName] = new List<string>();

        if (!_validationErrors[propertyName].Contains(error))
        {
            _validationErrors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (_validationErrors.ContainsKey(propertyName))
        {
            _validationErrors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }
}
