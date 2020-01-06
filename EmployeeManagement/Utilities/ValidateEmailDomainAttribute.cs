using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Utilities
{
    public class ValidateEmailDomainAttribute:ValidationAttribute
    {
        private readonly string _allowedDomain;
        public ValidateEmailDomainAttribute(string allowedDomain)
        {
            _allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == _allowedDomain.ToUpper();
            
        }
    }
}
