using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Dynamic.Framework.Mvc.Attribute
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        private bool _isAllowNull = true;

        public EmailValidationAttribute()
        {
        }

        public EmailValidationAttribute(bool isAllowNull = true)
        {
            this._isAllowNull = isAllowNull;
        }

        public override bool IsValid(object value)
        {
            if (!this._isAllowNull)
                return new Regex("^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$").IsMatch(value.ToString());
            return true;
        }
    }
}
