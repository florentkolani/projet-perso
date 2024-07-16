using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Kolani.Data;
using Projet_Kolani.Models;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Properties;


namespace Projet_Kolani.Controllers
{
    public class FacturesController : Controller
    {
        private readonly Projet_KolaniDbContext _context;

        public FacturesController(Projet_KolaniDbContext context)
        {
            _context = context;
        }

        // GET: Factures
        public async Task<IActionResult> Index()
        {
            var projet_KolaniDbContext = _context.Factures.Include(f => f.Proprietaire);
            return View(await projet_KolaniDbContext.ToListAsync());
        }

        // GET: Factures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facture = await _context.Factures
                .Include(f => f.Proprietaire)
                .FirstOrDefaultAsync(m => m.FactureId == id);
            if (facture == null)
            {
                return NotFound();
            }

            return View(facture);
        }

        // GET: Factures/Create
        public IActionResult Create()
        {
            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId");
            return View();
        }

        // POST: Factures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FactureId,Date,MontantTotal,ProprietaireId")] Facture facture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId", facture.ProprietaireId);
            return View(facture);
        }

        // GET: Factures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facture = await _context.Factures.FindAsync(id);
            if (facture == null)
            {
                return NotFound();
            }

            // Détacher l'entité existante du contexte
            _context.Entry(facture).State = EntityState.Detached;

            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId", facture.ProprietaireId);
            return View(facture);
        }

        // POST: Factures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FactureId,Date,MontantTotal,ProprietaireId")] Facture facture)
        {
            if (id != facture.FactureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Réattacher l'entité au contexte et la mettre à jour
                    _context.Attach(facture).State = EntityState.Modified;
                    //_context.Update(facture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactureExists(facture.FactureId))
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
            ViewData["ProprietaireId"] = new SelectList(_context.Proprietaires, "ProprietaireId", "ProprietaireId", facture.ProprietaireId);
            return View(facture);
        }

        // GET: Factures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facture = await _context.Factures
                .Include(f => f.Proprietaire)
                .FirstOrDefaultAsync(m => m.FactureId == id);
            if (facture == null)
            {
                return NotFound();
            }

            return View(facture);
        }

        // POST: Factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facture = await _context.Factures.FindAsync(id);
            if (facture != null)
            {
                _context.Factures.Remove(facture);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactureExists(int id)
        {
            return _context.Factures.Any(e => e.FactureId == id);
        }


        public async Task<IActionResult> GeneratePdf(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facture = await _context.Factures
                .Include(f => f.Proprietaire)
                .FirstOrDefaultAsync(m => m.FactureId == id);

            if (facture == null)
            {
                return NotFound();
            }

            // Créer un nouveau document PDF
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter pdfWriter = new PdfWriter(memoryStream);
            PdfDocument pdfDocument = new PdfDocument(pdfWriter);
            Document document = new Document(pdfDocument);

            document.Add(new Paragraph("Reçu Du Paiement")
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            .SetFontSize(20) // Taille de police
            .SetFontColor(ColorConstants.BLUE) // Couleur du texte
            .SetBold()); // Texte en gras

            // Créer un tableau pour afficher les informations de la facture avec un style
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            // Style du tableau
            table.SetBorder(Border.NO_BORDER); // Pas de bordure
            table.SetTextAlignment(TextAlignment.CENTER); // Alignement central

            // Style des cellules d'en-tête
            Style thStyle = new Style()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY) // Couleur de fond
                .SetTextAlignment(TextAlignment.CENTER) // Alignement central
                .SetBold(); // Texte en gras

            // Ajouter les en-têtes du tableau avec le style
            Cell cellIdHeader = new Cell().Add(new Paragraph("Propriété")).AddStyle(thStyle);
            Cell cellValueHeader = new Cell().Add(new Paragraph("Valeur")).AddStyle(thStyle);
            table.AddHeaderCell(cellIdHeader);
            table.AddHeaderCell(cellValueHeader);

            // Style des cellules de données
            Style tdStyle = new Style()
                .SetTextAlignment(TextAlignment.CENTER); // Alignement central

            // Ajouter les informations de la facture dans le tableau avec le style
            table.AddCell(new Cell().Add(new Paragraph("ID de la facture")).AddStyle(tdStyle));
            table.AddCell(new Cell().Add(new Paragraph(facture.FactureId.ToString())).AddStyle(tdStyle));

            table.AddCell(new Cell().Add(new Paragraph("Date")).AddStyle(tdStyle));
            table.AddCell(new Cell().Add(new Paragraph(facture.Date.ToShortDateString())).AddStyle(tdStyle));

            table.AddCell(new Cell().Add(new Paragraph("Montant Total")).AddStyle(tdStyle));
            table.AddCell(new Cell().Add(new Paragraph(facture.MontantTotal.ToString())).AddStyle(tdStyle));

            if (facture.Proprietaire != null)
            {
                table.AddCell(new Cell().Add(new Paragraph("Propriétaire")).AddStyle(tdStyle));
                table.AddCell(new Cell().Add(new Paragraph($"{facture.Proprietaire.Prenom} {facture.Proprietaire.Nom}")).AddStyle(tdStyle));
            }
            else
            {
                table.AddCell(new Cell().Add(new Paragraph("Propriétaire")).AddStyle(tdStyle));
                table.AddCell(new Cell().Add(new Paragraph("Inconnu")).AddStyle(tdStyle));
            }

            // Ajouter le tableau au document PDF
            document.Add(table);

            // Fermer le document PDF
            document.Close();

            // Retourner le fichier PDF en tant que réponse
            return File(memoryStream.ToArray(), "application/pdf", "Facture.pdf");
        }

    }
}
