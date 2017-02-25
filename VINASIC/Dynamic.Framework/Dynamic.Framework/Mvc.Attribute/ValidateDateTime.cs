using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;

namespace Dynamic.Framework.Mvc.Attribute
{
    public class ValidateDateTime : ValidationAttribute
    {
        private bool _isAllowNull = true;

        public ValidateDateTime()
        {
        }

        public ValidateDateTime(bool isAllowNull = true)
        {
            this._isAllowNull = isAllowNull;
        }

        public override bool IsValid(object value)
        {
            CultureInfo cultureInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm";
            return value == null || !(DateTime.Parse(value.ToString()) < DateTime.Now);
        }
    }
}
