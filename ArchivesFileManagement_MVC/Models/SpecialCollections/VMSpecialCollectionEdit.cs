using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Models.SpecialCollections
{
    public class VMSpecialCollectionEdit
    {
        public int? Id { get; set; }
        [Display(Name = "Type"), Required]
        public string RecordType { get; set; }
        [Display(Name = "Location"), Required]
        public string Location { get; set; }
        [Display(Name = "Accession #"), Required]
        public string AccessionNo { get; set; }
        [Display(Name = "Description"), Required]
        public string Description { get; set; }
        [Display(Name = "File")]
        public IFormFile Attachment { get; set; }
    }
}
