using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Application.Infrastructure
{
    public class FileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly int _maxFileSize;
        public FileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return false;
            }
            var totalSeize = file.ContentLength;
            return totalSeize <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_maxFileSize.ToString());
        }


        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(_maxFileSize.ToString()),
                ValidationType = "filesize"
            };
            rule.ValidationParameters["maxsize"] = _maxFileSize;
            yield return rule;
        }
    }
}