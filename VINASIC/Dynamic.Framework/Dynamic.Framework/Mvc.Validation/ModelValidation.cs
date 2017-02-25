using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Dynamic.Framework.Mvc.Validation
{
    public class ModelValidation
    {
        public static IEnumerable<Dynamic.Framework.Mvc.Error> Validate(object objValidate, ControllerContext controllerContext)
        {
            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>)(() => objValidate), objValidate.GetType());
            foreach (ModelMetadata modelMetadata in metadata.Properties)
            {
                foreach (ModelValidator modelValidator in modelMetadata.GetValidators(controllerContext))
                {
                    foreach (ModelValidationResult validationResult in modelValidator.Validate(metadata.Model))
                        yield return new Dynamic.Framework.Mvc.Error()
                        {
                            MemberName = modelMetadata.PropertyName,
                            Message = validationResult.Message
                        };
                }
            }
        }
    }
}
