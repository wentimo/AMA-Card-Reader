using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AMA_Card_Reader.Validator
{
    public class OneThroughThreeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value.ToString(), out int i) && i <= 3)
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Value must be a number between 1 and 3.");
        }
    }
}
