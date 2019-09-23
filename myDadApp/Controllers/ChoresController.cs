using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myDadApp.Models;

namespace myDadApp.Controllers
{
    public class ChoresController : Controller
    {
        private readonly myDataContext _context;

        public ChoresController(myDataContext context)
        {
            _context = context;
        }

        // GET: Chores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chore.ToListAsync());
        }

        // GET: Chores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // GET: Chores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Owner,IsDone,CreateDt,CompleteDt")] Chore chore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chore);
        }

        // GET: Chores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chore.FindAsync(id);
            if (chore == null)
            {
                return NotFound();
            }
            return View(chore);
        }

        // POST: Chores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description,Owner,IsDone,CreateDt,CompleteDt")] Chore chore)
        {
            if (id != chore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (chore.IsDone == true)
                    {
                        chore.CompleteDt = DateTime.UtcNow;
                    } else
                    {
                        chore.CompleteDt = null;
                    }
                    _context.Update(chore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoreExists(chore.Id))
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
            return View(chore);
        }

        // GET: Chores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // POST: Chores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var chore = await _context.Chore.FindAsync(id);
            _context.Chore.Remove(chore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoreExists(string id)
        {
            return _context.Chore.Any(e => e.Id == id);
        }
    }
}
