using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Models.Hccc
{
    public class VMHcccEdit
    {
        public int? Id { get; set; }
        [Display(Name = "Type"), Required]
        public string Type { get; set; }
        [Display(Name = "#"), Required]
        public string CaseNo { get; set; }
        [Display(Name = "Description"), Required]
        public string Description { get; set; }
        [Display(Name = "Location"), Required]
        public string Location { get; set; }
        [Required]
        public string SessionNo { get; set; }
        public string DestructionDate { get; set; }
        [Display(Name = "File")]
        public IFormFile Attachment { get; set; }
    }
}
