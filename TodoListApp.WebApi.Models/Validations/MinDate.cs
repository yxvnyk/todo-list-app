using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.CustomValidations;
public class MinDate : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is DateTime dateValue)
        {
            return dateValue.Date >= DateTime.Today;
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} can't be in the past.";
    }
}
