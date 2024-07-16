using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Kolani.Data;
using Projet_Kolani.Models;

namespace Projet_Kolani.Controllers
{
    public class EnginsController : Controller
    {
        private readonly Projet_KolaniDbContext _context;

        public EnginsController(Projet_KolaniDbContext context)
        {
            _context = context;
        }

        // GET: Engins
        public async Task<IActionResult> Index()
        {
            var projet_KolaniDbContext = _context.Engins.Include(e => e.Proprietaire);
            return View(await projet_KolaniDbContext.ToListAsync());
        }

        // GET: Engins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engin = await _context.Engins
                .Include(e => e.Proprietaire)
                .FirstOrDefaultAsync(m => m.EnginId == id);
            if (engin == null)
            {
                return NotFound();
            }

            return View(engin);
        }

        // GET: Engins/Create
        public IActionResult Create()
        {
            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId");
            return View();
        }

        // POST: Engins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnginId,Immatriculation,Categorie,CotationAssurance,MajorationEconomat,ProprietaireId")] Engin engin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(engin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId", engin.ProprietaireId);
            return View(engin);
        }

        // GET: Engins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engin = await _context.Engins.FindAsync(id);
            if (engin == null)
            {
                return NotFound();
            }
            _context.Entry(engin).State = EntityState.Detached;

            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId", engin.ProprietaireId);
            return View(engin);
        }

        // POST: Engins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnginId,Immatriculation,Categorie,CotationAssurance,MajorationEconomat,ProprietaireId")] Engin engin)
        {
            if (id != engin.EnginId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Attach(engin).State = EntityState.Modified;
                   // _context.Update(engin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnginExists(engin.EnginId))
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
            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId", engin.ProprietaireId);
            return View(engin);
        }

        // GET: Engins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engin = await _context.Engins
                .Include(e => e.Proprietaire)
                .FirstOrDefaultAsync(m => m.EnginId == id);
            if (engin == null)
            {
                return NotFound();
            }

            return View(engin);
        }

        // POST: Engins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var engin = await _context.Engins.FindAsync(id);
            if (engin != null)
            {
                _context.Engins.Remove(engin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnginExists(int id)
        {
            return _context.Engins.Any(e => e.EnginId == id);
        }
    }
}
