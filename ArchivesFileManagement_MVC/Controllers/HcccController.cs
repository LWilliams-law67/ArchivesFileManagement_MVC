using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArchivesFileManagement_MVCDB;
using ArchivesFileManagement_MVC.Models;
using ArchivesFileManagement_MVC.Models.Hccc;


namespace ArchivesFileManagement_MVC.Controllers
{
    public class HcccController : Controller
    {
        private readonly BCC_ArchivesContext _context;

        public HcccController()
        {
            _context = new BCC_ArchivesContext();
        }

        // GET: Hccc
        public IActionResult Index(string searchType, string searchCriteria, string searchText, string fromDate, string toDate)
        {
            VMArchiveSearch vm = new VMArchiveSearch();
            vm.TypeOptions = new SelectList(HcccType.GetCaseTypesForSearching(_context)); // for search type
            ViewBag.TypeOptions = HcccType.GetCaseTypesForCreatingEditing(_context); // for uploading new case, just db types available
            ViewBag.CriteriaOptions = new SelectList(new List<string>
            {
                "--SELECT--",
                "ALL",
                "CASE #",
                "DESCRIPTION"
            });

            IQueryable<Hcccmain> hcccQuery = _context.Hcccmain.Include(c => c.TypeNavigation);
            if (String.IsNullOrEmpty(searchType) || String.IsNullOrEmpty(searchCriteria) /*|| String.IsNullOrEmpty(searchText)*/)
            {
                vm.SearchResults = (from h in hcccQuery
                                 orderby h.UploadedDate descending
                                 select h).Take(10).ToList();

                return View(vm);
            }
            else if( !searchType.ToUpper().Equals("--SELECT--") && !searchCriteria.ToUpper().Equals("--SELECT--"))
            {
                vm.CurrentFilter = searchText;
                vm.SearchResults = GetCasesFromContext(_context, searchType, searchCriteria, searchText);
                //return View(vm);
            }
            //else
            //{
            //     hcccQuery = hcccQuery.
            //}

            if (DateTime.TryParse(fromDate, out DateTime startDate) && DateTime.TryParse(toDate, out DateTime endDate))
            {
                vm.SearchResults = hcccQuery.Where(c => c.UploadedDate.Date >= startDate && c.UploadedDate.Date <= endDate).ToList();
                vm.FromDate = fromDate;
                vm.ToDate = toDate;
            }

            return View(vm);
        }

        // GET: Hccc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hcccmain = await _context.Hcccmain
                .Include(h => h.TypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hcccmain == null)
            {
                return NotFound();
            }

            return PartialView("_Details", hcccmain);
        }

        // GET: Hccc/Create
        public IActionResult Create()
        {
            ViewBag.TypeOptions = new SelectList(HcccType.GetCaseTypesForCreatingEditing(_context));
            return PartialView("_Create", new VMHcccCreate());
        }

        // POST: Hccc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMHcccCreate newUploadModel)
        {
            var memoryStream = new MemoryStream();
            newUploadModel.Attachment.CopyTo(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            Hcccmain newHccc = new Hcccmain
            {
                Attachment = fileBytes,
                Type = _context.Hccccasetypes.Where(t => t.Description.Equals(newUploadModel.Type)).Select(t => t.Id).FirstOrDefault(),
                CaseNo = newUploadModel.CaseNo,
                Description = newUploadModel.Description,
                Location = newUploadModel.Location,
                SessionNo = newUploadModel.SessionNo,
                UploadedDate = DateTime.Now,
                UploadedBy = User.Identity.Name,
                EditedBy = User.Identity.Name,
                DestructionDate = newUploadModel.DestructionDate
            };

            if (ModelState.IsValid)
            {
                _context.Add(newHccc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeOptions = new SelectList(HcccType.GetCaseTypesForCreatingEditing(_context));
            return PartialView("_Create", newUploadModel);
        }

        // GET: Hccc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hcccmain = await _context.Hcccmain.FindAsync(id);
            if (hcccmain == null)
            {
                return NotFound();
            }
            ViewBag.TypeOptions = new SelectList(HcccType.GetCaseTypesForCreatingEditing(_context));
            ViewBag.Id = id;

            VMHcccEdit newEditModel = new VMHcccEdit 
            {
                Id = hcccmain.Id,
                Type = _context.Hccccasetypes.Where(t => t.Id == hcccmain.Type).Select(t => t.Description).FirstOrDefault(),
                CaseNo = hcccmain.CaseNo,
                Description = hcccmain.Description,
                Location = hcccmain.Location,
                SessionNo = hcccmain.SessionNo,
                DestructionDate = hcccmain.DestructionDate
            };

            return PartialView("_Edit", newEditModel);
        }

        // POST: Hccc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMHcccEdit newEditModel)
        {
            if (newEditModel.Id == null)
            {
                return NotFound();
            }
            var hcccRecord = await _context.Hcccmain.FindAsync(newEditModel.Id);

            byte[] fileBytes;
            if (newEditModel.Attachment != null)
            {
                var memoryStream = new MemoryStream();
                newEditModel.Attachment.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
                hcccRecord.Attachment = fileBytes;
            }

            hcccRecord.Description = newEditModel.Description;
            hcccRecord.CaseNo = newEditModel.CaseNo;
            hcccRecord.DestructionDate = newEditModel.DestructionDate;
            hcccRecord.Type = await _context.Hccccasetypes.Where(t => t.Description.Equals(newEditModel.Type)).Select(t => t.Id).FirstOrDefaultAsync();
            hcccRecord.Location = newEditModel.Location;
            hcccRecord.SessionNo = newEditModel.SessionNo;
            hcccRecord.EditedBy = User.Identity.Name;
            hcccRecord.EditedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hcccRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HcccmainExists(hcccRecord.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            ViewBag.TypeOptions = new SelectList(HcccType.GetCaseTypesForCreatingEditing(_context));
            return View(nameof(Index), "Home");
        }

        // GET: Hccc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hcccmain = await _context.Hcccmain
                .Include(h => h.TypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hcccmain == null)
            {
                return NotFound();
            }

            return View(hcccmain);
        }

        // POST: Hccc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hcccmain = await _context.Hcccmain.FindAsync(id);
            _context.Hcccmain.Remove(hcccmain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HcccmainExists(int id)
        {
            return _context.Hcccmain.Any(e => e.Id == id);
        }

        /********************
         * Helper functions *
         ********************/
        private List<Hcccmain> GetCasesFromContext(BCC_ArchivesContext db, string searchType, string searchCriteria, string searchText)
        {
            bool isAllTypes = searchType.ToUpper().Equals("ALL");
            bool isAllCriteria = searchCriteria.ToUpper().Equals("ALL");
            var hcccQuery = db.Hcccmain.Include(c => c.TypeNavigation);
            // If search criteria is 'description' limit query of cases by description
            if (isAllCriteria)
            {
                if (isAllTypes)
                {
                    hcccQuery = db.Hcccmain.Where(c => c.Description.Contains(searchText) || c.CaseNo.Contains(searchText))
                                           .OrderBy(c => c.CaseNo)
                                           .Include(c => c.TypeNavigation);
                }
                else
                {
                    hcccQuery = db.Hcccmain.Where(c => (c.Description.Contains(searchText) || c.CaseNo.Contains(searchText)) && c.TypeNavigation.Description.Contains(searchType))
                                           .OrderBy(c => c.CaseNo)
                                           .Include(c => c.TypeNavigation);
                }
            }
            else if(searchCriteria.ToUpper().Equals("DESCRIPTION"))// limit query cases by case number
            {
                if (isAllTypes)
                {
                    hcccQuery = db.Hcccmain.Where(c => c.Description.Contains(searchText))
                                           .OrderBy(c => c.CaseNo)
                                           .Include(c => c.TypeNavigation);
                }
                else
                {
                    hcccQuery = db.Hcccmain.Where(c => c.Description.Contains(searchText) && c.TypeNavigation.Description.Contains(searchType))
                                           .OrderBy(c => c.CaseNo)
                                           .Include(c => c.TypeNavigation);
                }
            }
            else
            {
                if (isAllTypes)
                {
                    hcccQuery = db.Hcccmain.Where(c => c.CaseNo.Equals(searchText))
                                           .OrderBy(c => c.CaseNo)
                                           .Include(c => c.TypeNavigation);
                }
                else
                {
                    hcccQuery = db.Hcccmain.Where(c => c.CaseNo.Equals(searchText) && c.TypeNavigation.Description.Contains(searchType))
                                           .OrderBy(c => c.CaseNo)
                                           .Include(c => c.TypeNavigation);
                }
            }

            List<Hcccmain> cases = new List<Hcccmain>();
            foreach (var c in hcccQuery)
            {
                cases.Add(c);
            }

            return cases;
        }
    }
}
