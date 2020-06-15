using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuevoSoftware.ApplicationMonitoring.Data;

namespace WebApplicationUI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly NSAppMonDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ResourceController(NSAppMonDBContext context)
        {
            _context = context;
        }
        // GET: Resource
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.NsresourcesT.ToListAsync());
        }

        // GET: Resource/Details/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsresourcesT = await _context.NsresourcesT
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nsresourcesT == null)
            {
                return NotFound();
            }

            return View(nsresourcesT);
        }

        // GET: Resource/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }
        // POST: Resource/Create
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nsresourcesT"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Language,ResourceKey,ResourceText")] NsresourcesT nsresourcesT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nsresourcesT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nsresourcesT);
        }
        // GET: Resource/Edit/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsresourcesT = await _context.NsresourcesT.FindAsync(id);
            if (nsresourcesT == null)
            {
                return NotFound();
            }
            return View(nsresourcesT);
        }
        // POST: Resource/Edit/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nsresourcesT"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Language,ResourceKey,ResourceText")] NsresourcesT nsresourcesT)
        {
            if (id != nsresourcesT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nsresourcesT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NsresourcesTExists(nsresourcesT.Id))
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
            return View(nsresourcesT);
        }
        // GET: Resource/Delete/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsresourcesT = await _context.NsresourcesT
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nsresourcesT == null)
            {
                return NotFound();
            }

            return View(nsresourcesT);
        }
        // POST: Resource/Delete/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nsresourcesT = await _context.NsresourcesT.FindAsync(id);
            _context.NsresourcesT.Remove(nsresourcesT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool NsresourcesTExists(int id)
        {
            return _context.NsresourcesT.Any(e => e.Id == id);
        }
    }
}
