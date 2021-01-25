using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArchivesFileManagement_MVC.Models.SpecialCollections;
using ArchivesFileManagement_MVCDB;
using Microsoft.AspNetCore.Http;

namespace ArchivesFileManagement_MVC.Controllers
{
    public class SpecialCollectionsController : Controller
    {
        private readonly BCC_ArchivesContext _context;

        public SpecialCollectionsController()
        {
            _context = new BCC_ArchivesContext();
        }

        // GET: SpecialCollections/
        public IActionResult Index(string searchType, string searchText, string fromDate, string toDate)
        {
            VMSpecialCollectionSearch vm = new VMSpecialCollectionSearch();
            vm.TypeOptions = new SelectList(SpecialCollectionTypeModel.GetCollectionTypesForSearching(_context)); // for search criteria
            ViewBag.TypeOptions = new SelectList(SpecialCollectionTypeModel.GetCollectionTypesForCreatingEditing(_context)); // for uploading new special collection

            IQueryable<SpecialCollections> collections = _context.SpecialCollections.Include(sc => sc.Type);
            if (String.IsNullOrEmpty(searchType))
            {
                vm.SearchResults = (from sc in _context.SpecialCollections
                                orderby sc.UploadDate descending
                                select sc).Include(sc => sc.Type).Take(10).ToList();
                return View(vm);
            }
            else if (!searchType.ToUpper().Equals("--SELECT--"))
            {
                vm.CurrentFilter = searchText;
                vm.SearchResults = GetCollectionsFromContext(collections, searchType, searchText);
                //return View(vm);
            }
            if (DateTime.TryParse(fromDate, out DateTime startDate) && DateTime.TryParse(toDate, out DateTime endDate))
            {
                vm.SearchResults = collections.Where(c => c.UploadDate.Date >= startDate && c.UploadDate.Date <= endDate).ToList();
                vm.FromDate = startDate;
                vm.ToDate = endDate;
            }

            return View(vm);
        }

        // GET: SpecialCollections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialCollections = await _context.SpecialCollections
                .Include(s => s.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialCollections == null)
            {
                return NotFound();
            }

            return PartialView("_Details", specialCollections);
        }

        // GET: SpecialCollections/Create
        public IActionResult Create()
        {
            ViewBag.TypeOptions = new SelectList(SpecialCollectionTypeModel.GetCollectionTypesForCreatingEditing(_context));
            return PartialView("_Create", new VMSpecialCollectionCreate());
        }

        // POST: SpecialCollections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[RequestSizeLimit(26214400)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMSpecialCollectionCreate newSpecialCollectionModel)
        {
            var memoryStream = new MemoryStream();
            newSpecialCollectionModel.Attachment.CopyTo(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            SpecialCollections newSpecColl = new SpecialCollections
            {
                Attachment = fileBytes,
                TypeId = _context.SpecialCollectionsTypes.Where(t => t.TypeName.Equals(newSpecialCollectionModel.RecordType)).Select(t => t.Id).First(),
                Description = newSpecialCollectionModel.Description,
                Location = newSpecialCollectionModel.Location,
                AccessionNo = newSpecialCollectionModel.AccessionNo,
                UploadDate = DateTime.Now,
                UploadedBy = User.Identity.Name,
                EditBy = User.Identity.Name,
            };

            if (ModelState.IsValid)
            {
                _context.Add(newSpecColl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeOptions = new SelectList(SpecialCollectionTypeModel.GetCollectionTypesForCreatingEditing(_context));
            return RedirectToAction(nameof(Index));
        }

        // GET: SpecialCollections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialCollections = await _context.SpecialCollections.FindAsync(id);
            if (specialCollections == null)
            {
                return NotFound();
            }
            ViewBag.TypeOptions = new SelectList(SpecialCollectionTypeModel.GetCollectionTypesForCreatingEditing(_context));
            ViewBag.Id = id;

            VMSpecialCollectionEdit newEditModel = new VMSpecialCollectionEdit
            {
                AccessionNo = specialCollections.AccessionNo,
                Description = specialCollections.Description,
                Location = specialCollections.Location,
                Id = specialCollections.Id,
                RecordType = _context.SpecialCollectionsTypes.Where(t => t.Id == specialCollections.TypeId).Select(t => t.TypeName).FirstOrDefault()
            };

            return PartialView("_Edit", newEditModel);
        }

        // POST: SpecialCollections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMSpecialCollectionEdit newEditModel)
        {
            if (newEditModel.Id == null)
            {
                return NotFound();
            }
            var specialCollection = await _context.SpecialCollections.FindAsync(newEditModel.Id);

            byte[] fileBytes;
            if(newEditModel.Attachment != null)
            {
                var memoryStream = new MemoryStream();
                newEditModel.Attachment.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
                specialCollection.Attachment = fileBytes;
            } 

            specialCollection.Location = newEditModel.Location;
            specialCollection.AccessionNo = newEditModel.AccessionNo;
            specialCollection.Description = newEditModel.Description;
            specialCollection.TypeId = await _context.SpecialCollectionsTypes.Where(t => t.TypeName.Equals(newEditModel.RecordType)).Select(t => t.Id).FirstAsync();
            specialCollection.EditBy = User.Identity.Name;
            specialCollection.EditDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialCollection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialCollectionsExists(specialCollection.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeOptions = new SelectList(SpecialCollectionTypeModel.GetCollectionTypesForCreatingEditing(_context));
            return View(nameof(Index), "Home");
        }

        // POST: SpecialCollections/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialCollections = await _context.SpecialCollections.FindAsync(id);
            _context.SpecialCollections.Remove(specialCollections);
            await _context.SaveChangesAsync();
            ViewBag.TypeOptions = new SelectList(SpecialCollectionTypeModel.GetCollectionTypesForCreatingEditing(_context));
            return RedirectToAction(nameof(Index));
        }

        /********************
         * Helper functions *
         ********************/

        private bool SpecialCollectionsExists(int id)
        {
            return _context.SpecialCollections.Any(e => e.Id == id);
        }

        private List<SpecialCollections> GetCollectionsFromContext(IQueryable<SpecialCollections> db, string searchType, string searchText)
        {
            if(searchType.ToUpper().Equals("ALL"))
            {
                db = db.Where(sc => sc.Description.Contains(searchText))
                       .OrderBy(sc => sc.AccessionNo)
                       .Include(sc => sc.Type);
            }
            else
            {
                db = db.Where(sc => sc.Description.Contains(searchText) && sc.Type.TypeName.Contains(searchType))
                   .OrderBy(sc => sc.AccessionNo)
                   .Include(sc => sc.Type);
            }
            List<SpecialCollections> collections = new List<SpecialCollections>();
            foreach (var sc in db)
            {
                collections.Add(sc);
            }

            return collections;
        }
    }
}
