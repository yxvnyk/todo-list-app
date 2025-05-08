using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.CustomValidations
{
    /// <summary>
    /// A custom validation attribute that ensures the date is not in the past.
    /// </summary>
    public sealed class MinDate : ValidationAttribute
    {
        /// <summary>
        /// Validates whether the provided date is not in the past.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is either null or a valid date not in the past; otherwise, false.</returns>
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is DateTime dateValue)
            {
                // Ensures the date is not before today's date.
                return dateValue.Date >= DateTime.Today;
            }

            return false;
        }

        /// <summary>
        /// Provides the error message to be used when validation fails.
        /// </summary>
        /// <param name="name">The name of the field being validated.</param>
        /// <returns>The error message indicating the validation failure reason.</returns>
        public override string FormatErrorMessage(string name)
        {
            return $"{name} can't be in the past.";
        }
    }
}
