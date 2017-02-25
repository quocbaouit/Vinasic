using System;
using System.ComponentModel.DataAnnotations;

namespace Dynamic.Framework.Mvc.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredResxAttribute : RequiredAttribute
    {
        public string ErrorMessageResourceNameResx { get; set; }

        public Type VNType { get; set; }

        public Type USType { get; set; }

        public override bool IsValid(object value)
        {
            this.ErrorMessage = string.Empty;
            if (ResxManager.GetResx(ResxManager.CurrentLanguage).UICulture == ResxManager.Vi.UICulture)
                this.ErrorMessageResourceType = this.VNType;
            else
                this.ErrorMessageResourceType = this.USType;
            this.ErrorMessageResourceName = this.ErrorMessageResourceNameResx;
            return base.IsValid(value);
        }
    }
}
