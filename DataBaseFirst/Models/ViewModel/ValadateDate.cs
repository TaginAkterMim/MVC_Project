using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataBaseFirst.Models.ViewModel
{
    public class ValadateDate:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           DateTime CurrentDate = DateTime.Now;
            string message = "";
            if (Convert.ToDateTime(value) < CurrentDate.AddYears(-21))
            {
                return ValidationResult.Success;
            }
            else 
            {
                message = "Age nust be atlest the 21";
                return new ValidationResult(message);
            }
        }
    }
}