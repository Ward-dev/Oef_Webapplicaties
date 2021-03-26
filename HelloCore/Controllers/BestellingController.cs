using HelloCore.Data;
using HelloCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloCore.Controllers
{
    public class BestellingController : Controller
    {
        private readonly HelloCoreContext _context;
        public BestellingController(HelloCoreContext context)
        {
            this._context = context;
        }

        //GET: Bestelling
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bestellingen.ToListAsync());
        }

        //GET: Bestelling/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Testtitel";
            ViewBag.KlantenLijst = new SelectList(await _context.Klanten.ToListAsync(), "KlantID", "Naam");
            return View();
        }

        //POST: Bestelling/CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BestellingID,Artikel,Prijs,KlantID")] Bestelling bestelling)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bestelling);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bestelling);
        }

        //GET : Bestelling/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            ViewBag.KlantenLijst = new SelectList(await _context.Klanten.ToListAsync(), "KlantID", "Naam");
            var bestelling = await _context.Bestellingen.FindAsync(id);
            if (bestelling == null)
            {
                return NotFound();
            }

            return View(bestelling);
        }

        //POST: Bestelling/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("BestellingID,Artikel,Prijs,KlantID,Klant")]Bestelling bestelling)
        {
            if (id != bestelling.BestellingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestelling);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellingExists(bestelling.BestellingID))
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
            return View(bestelling);
        }

        //GET:Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = await _context.Bestellingen.Include(x=>x.Klant)
                .FirstOrDefaultAsync(m => m.BestellingID == id);
            if (bestelling == null)
            {
                return NotFound();
            }

            return View(bestelling);
        }
        //POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestelling = await _context.Bestellingen.FindAsync(id);
            _context.Bestellingen.Remove(bestelling);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellingExists(int id)
        {
            return _context.Bestellingen.Any(e => e.BestellingID == id);
        }


    }
}
