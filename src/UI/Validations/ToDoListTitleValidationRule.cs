using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Assignment.Application.TodoLists.Queries.GetTodos;

namespace Assignment.UI.Validations;

public class ToDoListTitleValidationRule : ValidationRule
{
    
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        string title = value as string;
        if (title?.Length > 200)
        {
            return new ValidationResult(false, "The title of a ToDoList cannot exceed 200 characters.");
        }

        return new ValidationResult(true, null);
    }
}
