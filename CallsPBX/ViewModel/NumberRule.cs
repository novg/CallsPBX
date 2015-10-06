using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CallsPBX.ViewModel
{
    public class NumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string pattern = @"^(\d+|\d+[ ,])*$";
            Regex regex = new Regex(pattern);

            if (regex.IsMatch(value.ToString()))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Invalid input");
            }
        }
    }
}
