using ArchivesFileManagement_MVCDB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArchivesFileManagement_MVC.Models.SpecialCollections
{
    public class VMSpecialCollectionSearch
    {
        // results of search query
        public List<ArchivesFileManagement_MVCDB.SpecialCollections> SearchResults { get; internal set; }

        public SelectList TypeOptions { get; set; }

        // Selected search options
        [Required]
        public string SearchType { get; set; }
        [Required]
        public string SearchText { get; set; }
        public string CurrentFilter { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
