using ArchivesFileManagement_MVCDB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Controllers
{
    public class DisplayCollectionController : Controller
    {
        private readonly BCC_ArchivesContext _context;

        public DisplayCollectionController()
        {
            _context = new BCC_ArchivesContext();
        }

        public FileResult Index(int id)
        {
            byte[] collectionFile = _context.SpecialCollections.FirstOrDefault(c => c.Id == id).Attachment;
            return new FileContentResult(collectionFile, "application/pdf");
        }
    }
}
