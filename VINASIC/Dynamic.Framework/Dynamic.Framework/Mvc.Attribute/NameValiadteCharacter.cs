using System;
using System.ComponentModel.DataAnnotations;

namespace Dynamic.Framework.Mvc.Attribute
{
    public class NameValiadteCharacter : ValidationAttribute
    {
        private bool _isAllowNull = true;

        public NameValiadteCharacter()
        {
        }

        public NameValiadteCharacter(bool isAllowNull = true)
        {
            this._isAllowNull = isAllowNull;
        }

        public override bool IsValid(object value)
        {
            char[] chArray = "!@#$%^&*()+=-[]\\';,./{}|\":<>?".ToCharArray();
            if (value != null)
            {
                for (int index = 0; index < chArray.Length; ++index)
                {
                    string str = chArray[index].ToString();
                    if (value.ToString().IndexOf(str, StringComparison.Ordinal) != -1)
                        return false;
                }
            }
            return true;
        }
    }
}
