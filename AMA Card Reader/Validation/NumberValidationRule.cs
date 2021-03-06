﻿using System.Globalization;
using System.Windows.Controls;

namespace AMA_Card_Reader.Validation
{
    public class NumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse((string)value, out int number))
                return new ValidationResult(true, null);
            else
                return new ValidationResult(false, "Value must be a number.");
        }
    }
}
