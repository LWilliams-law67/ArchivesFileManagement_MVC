using ArchivesFileManagement_MVCDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArchivesFileManagement_MVC.Models
{
    public class HcccType
    {
        public int Id { get; set; }
        [Display(Name = "Type"), Required]
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }

        public HcccType(Hccccasetypes typeNavigation)
        {
            Id = typeNavigation.Id;
            Description = typeNavigation.Description;
            Active = typeNavigation.Active;
        }

        public static List<string> GetCaseTypesForSearching(BCC_ArchivesContext db)
        {
            List<string> typeList = new List<string> // Putting default values into available select type options
            {
                "--SELECT",
                "ALL"
            };
            var types = (from t in db.Hccccasetypes
                         orderby t.Description ascending
                         select t);
            foreach (var t in types)
            {
                typeList.Add(t.Description);
            }

            return typeList;
        }

        public static List<string> GetCaseTypesForCreatingEditing(BCC_ArchivesContext db)
        {
            List<string> typeList = (from t in db.Hccccasetypes
                                        orderby t.Description ascending
                                        select t.Description).ToList();
            return typeList;
        }
    }
}