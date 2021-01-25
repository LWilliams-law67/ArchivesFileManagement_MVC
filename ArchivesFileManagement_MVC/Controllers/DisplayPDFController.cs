using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchivesFileManagement_MVCDB;
using Microsoft.AspNetCore.Mvc;

namespace ArchivesFileManagement_MVC.Controllers
{
    public class DisplayPDFController : Controller
    {
        private readonly BCC_ArchivesContext _context;

        public DisplayPDFController()
        {
            _context = new BCC_ArchivesContext();
        }

        public FileResult Index(int id)
        {
            byte[] caseFile = _context.Hcccmain.FirstOrDefault(c => c.Id == id).Attachment;
            return new FileContentResult(caseFile, "application/pdf");
        }
    }
}
