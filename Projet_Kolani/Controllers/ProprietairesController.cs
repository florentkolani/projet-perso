﻿using System;
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
    public class ProprietairesController : Controller
    {
        private readonly Projet_KolaniDbContext _context;

        public ProprietairesController(Projet_KolaniDbContext context)
        {
            _context = context;
        }

        // GET: Proprietaires
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proprietaires.ToListAsync());
        }

        // GET: Proprietaires/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietaire = await _context.Proprietaires
                .FirstOrDefaultAsync(m => m.ProprietaireId == id);
            if (proprietaire == null)
            {
                return NotFound();
            }

            return View(proprietaire);
        }

        // GET: Proprietaires/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proprietaires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProprietaireId,Nom,Prenom")] Proprietaire proprietaire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proprietaire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proprietaire);
        }

        // GET: Proprietaires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietaire = await _context.Proprietaires.FindAsync(id);
            if (proprietaire == null)
            {
                return NotFound();
            }
            // Détacher l'entité existante du contexte
            _context.Entry(proprietaire).State = EntityState.Detached;

            return View(proprietaire);
        }

        // POST: Proprietaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProprietaireId,Nom,Prenom")] Proprietaire proprietaire)
        {
            if (id != proprietaire.ProprietaireId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Réattacher l'entité au contexte et la mettre à jour
                    _context.Attach(proprietaire).State = EntityState.Modified;

                   // _context.Update(proprietaire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProprietaireExists(proprietaire.ProprietaireId))
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
            return View(proprietaire);
        }

        // GET: Proprietaires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietaire = await _context.Proprietaires
                .FirstOrDefaultAsync(m => m.ProprietaireId == id);
            if (proprietaire == null)
            {
                return NotFound();
            }

            return View(proprietaire);
        }

        // POST: Proprietaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proprietaire = await _context.Proprietaires.FindAsync(id);
            if (proprietaire != null)
            {
                _context.Proprietaires.Remove(proprietaire);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProprietaireExists(int id)
        {
            return _context.Proprietaires.Any(e => e.ProprietaireId == id);
        }
    }
}
