using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ArchivesFileManagement_MVCDB;

namespace ArchivesFileManagement_MVC.Models.User
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [Display(Name = "Username"), StringLength(50, MinimumLength = 5), Required]
        public string UserName { get; set; }
        [Display(Name = "Name"), StringLength(50, MinimumLength = 5), Required]
        public string DisplayName { get; set; }
        [Display(Name = "Email"), StringLength(50, MinimumLength = 10), Required]
        public string Email { get; set; }
        [Display(Name = "Creation Date"), DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Created By"), StringLength(50, MinimumLength = 5)]
        public string CreatedBy { get; set; }
        [Display(Name= "Edit Date"), DataType(DataType.Date)]
        public DateTime EditedDate { get; set; }
        [Display(Name = "Edited By"), StringLength(50, MinimumLength = 5)]
        public string EditedBy { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
