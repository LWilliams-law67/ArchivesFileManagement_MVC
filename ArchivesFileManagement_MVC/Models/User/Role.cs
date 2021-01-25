using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Models.User
{
    public class Role
    {
        [Display(Name = "Role Id")]
        public int RoleId { get; set; }
        [Display(Name = "Role Name"), StringLength(50, MinimumLength = 5), Required]
        public string RoleName { get; set; }
        [Display(Name = "Role Description"), StringLength(50, MinimumLength = 10)]
        public string RoleDescription { get; set; }
        [Display(Name = "Is Active"), Required]
        public bool IsActive { get; set; }
        [Display(Name = "Creation Date"), DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Created By"), StringLength(50, MinimumLength = 5)]
        public string CreatedBy { get; set; }
        [Display(Name = "Edit Date"), DataType(DataType.Date)]
        public DateTime EditDate { get; set; }
        [Display(Name = "Edited By"), StringLength(50, MinimumLength = 5)]
        public string EditBy { get; set; }

        public User[] Users { get; set; }
    }
}
