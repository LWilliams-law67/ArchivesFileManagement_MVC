using ArchivesFileManagement_MVCDB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Models
{
    public class VMArchiveSearch
    {
        // results of search query
        public List<Hcccmain> SearchResults { get; internal set; }

        public SelectList TypeOptions { get; set; }

        // Selected search options
        [Required]
        public string SearchType { get; set; }
        [Required]
        public string SearchCriteria { get; set; }
        [Required]
        public string SearchText { get; set; }
        public string CurrentFilter { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
