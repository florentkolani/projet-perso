using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Barcodes.Dmcode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Kolani.Data;
using Projet_Kolani.Models;

namespace Projet_Kolani.Controllers
{
    public class ReglementsController : Controller
    {
        private readonly Projet_KolaniDbContext _context;

        public ReglementsController(Projet_KolaniDbContext context)
        {
            _context = context;
        }

        // GET: Reglements
        public async Task<IActionResult> Index()
        {
            var projet_KolaniDbContext = _context.Reglements.Include(r => r.Facture);
            return View(await projet_KolaniDbContext.ToListAsync());
        }

        // GET: Reglements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reglement = await _context.Reglements
                .Include(r => r.Facture)
                .FirstOrDefaultAsync(m => m.ReglementId == id);
            if (reglement == null)
            {
                return NotFound();
            }

            return View(reglement);
        }

        // GET: Reglements/Create
        public IActionResult Create()
        {
            ViewData["FactureId"] = new SelectList(_context.Factures, "FactureId", "FactureId");
            return View();
        }

        // POST: Reglements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReglementId,Date,Montant,FactureId")] Reglement reglement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reglement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FactureId"] = new SelectList(_context.Factures, "FactureId", "FactureId", reglement.FactureId);
            return View(reglement);
        }

        // GET: Reglements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reglement = await _context.Reglements.FindAsync(id);
            if (reglement == null)
            {
                return NotFound();
            }


            // Détacher l'entité existante du contexte
            _context.Entry(reglement).State = EntityState.Detached;

            ViewData["FactureId"] = new SelectList(_context.Factures, "FactureId", "FactureId", reglement.FactureId);
            return View(reglement);
        }

        // POST: Reglements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReglementId,Date,Montant,FactureId")] Reglement reglement)
        {
            if (id != reglement.ReglementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Réattacher l'entité au contexte et la mettre à jour
                    _context.Attach(reglement).State = EntityState.Modified;
                    //_context.Update(reglement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReglementExists(reglement.ReglementId))
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
            ViewData["FactureId"] = new SelectList(_context.Factures, "FactureId", "FactureId", reglement.FactureId);
            return View(reglement);
        }

        // GET: Reglements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reglement = await _context.Reglements
                .Include(r => r.Facture)
                .FirstOrDefaultAsync(m => m.ReglementId == id);
            if (reglement == null)
            {
                return NotFound();
            }

            return View(reglement);
        }

        // POST: Reglements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reglement = await _context.Reglements.FindAsync(id);
            if (reglement != null)
            {
                _context.Reglements.Remove(reglement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReglementExists(int id)
        {
            return _context.Reglements.Any(e => e.ReglementId == id);
        }
    }
}
