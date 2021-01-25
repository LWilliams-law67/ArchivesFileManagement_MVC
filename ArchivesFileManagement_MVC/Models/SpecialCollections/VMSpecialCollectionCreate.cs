using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ArchivesFileManagement_MVC.Models.SpecialCollections
{
    public class VMSpecialCollectionCreate
    {
        [Display(Name = "Type"), Required]
        public string RecordType { get; set; }
        [Display(Name = "Location"), Required]
        public string Location { get; set; }
        [Display(Name = "Accession #"), Required]
        public string AccessionNo { get; set; }
        [Display(Name = "Description"), Required]
        public string Description { get; set; }
        [Display(Name = "File"), Required]
        public IFormFile Attachment { get; set; }
    }
}