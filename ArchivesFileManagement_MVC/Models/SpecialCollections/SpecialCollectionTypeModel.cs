using ArchivesFileManagement_MVCDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Models.SpecialCollections
{
    public class SpecialCollectionTypeModel
    {
        public int Id { get; set; }
        [Display(Name = "Type")]
        public string TypeName { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public SpecialCollectionTypeModel(ArchivesFileManagement_MVCDB.SpecialCollectionsTypes specialType)
        {
            Id = specialType.Id;
            TypeName = specialType.TypeName;
            IsActive = specialType.IsActive;
        }
        
        public static List<string> GetCollectionTypesForCreatingEditing(BCC_ArchivesContext db)
        {
            return (from t in db.SpecialCollectionsTypes
                         orderby t.TypeName ascending
                         select t.TypeName).ToList();
        }

        public static List<string> GetCollectionTypesForSearching(BCC_ArchivesContext db)
        {
            List<string> typeSelections = new List<string>
            {
                "--SELECT--",
                "ALL"
            };
            var types = (from t in db.SpecialCollectionsTypes
                         orderby t.TypeName ascending
                         select t);
            foreach(var t in types)
            {
                typeSelections.Add(t.TypeName);
            }

            return typeSelections;
        }
    }
}
