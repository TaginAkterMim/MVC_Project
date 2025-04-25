using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataBaseFirst.Models.ViewModel
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {
            this.QualificationList = new List<int>();
        }
        public int EmployeeID { get; set; }
        [Required, Display(Name = "First Name"), StringLength(50)]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name"), StringLength(50)]
        public string lastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:yyyy-MM-dd}")]
        [Display(Name ="Date Of Birth")]
        [ValadateDate]
        public System.DateTime DateOfBirth { get; set; }
        [Display(Name ="Salary"),Range(15000,25000)]
        public decimal Salary { get; set; }
        [Display(Name ="Activee ?")]
        public bool IsActive { get; set; }
        [Display(Name ="Profile Picture")]
        public string Picture { get; set; }

        public HttpPostedFileBase PicturePath { get; set; }
        public List<int> QualificationList { get; set; }
    }
}