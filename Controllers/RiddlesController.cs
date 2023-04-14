using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasterEggHunt.Data;
using EasterEggHunt.Models;
using Microsoft.AspNetCore.Authorization;

namespace EasterEggHunt.Controllers
{
    public class RiddlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RiddlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Riddles
        public async Task<IActionResult> Index()
        {
              return _context.Riddle != null ? 
                          View(await _context.Riddle.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Riddle'  is null.");
        }


        // GET: Riddles/FindRiddleForm
        public async Task<IActionResult> FindRiddleForm()
        {
            return _context.Riddle != null ?
                   View() :
                   Problem("Entity set 'ApplicationDbContext.Riddle'  is null.");
        }

        // POST: Riddles/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchForClue)
        {
            return _context.Riddle != null ?  //where - sort the list by filtering by the searched clue
               View("Index", await _context.Riddle.Where(j => j.RiddleAnswer.Contains(SearchForClue)).ToListAsync()) :
               Problem("Entity set 'ApplicationDbContext.Riddle'  is null.");
        }

        // GET: Riddles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Riddle == null)
            {
                return NotFound();
            }

            var riddle = await _context.Riddle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riddle == null)
            {
                return NotFound();
            }

            return View(riddle);
        }

        [Authorize]
        // GET: Riddles/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        // POST: Riddles/Create - post to DB
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RiddleQuestion,RiddleAnswer")] Riddle riddle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(riddle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(riddle);
        }

        [Authorize]
        // GET: Riddles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Riddle == null)
            {
                return NotFound();
            }

            var riddle = await _context.Riddle.FindAsync(id);
            if (riddle == null)
            {
                return NotFound();
            }
            return View(riddle);
        }

        [Authorize]
        // POST: Riddles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RiddleQuestion,RiddleAnswer")] Riddle riddle)
        {
            if (id != riddle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(riddle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiddleExists(riddle.Id))
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
            return View(riddle);
        }

        [Authorize]
        // GET: Riddles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Riddle == null)
            {
                return NotFound();
            }

            var riddle = await _context.Riddle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riddle == null)
            {
                return NotFound();
            }

            return View(riddle);
        }

        [Authorize]
        // POST: Riddles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Riddle == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Riddle'  is null.");
            }
            var riddle = await _context.Riddle.FindAsync(id);
            if (riddle != null)
            {
                _context.Riddle.Remove(riddle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiddleExists(int id)
        {
          return (_context.Riddle?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
