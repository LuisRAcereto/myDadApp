using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using myDadApp.Models;

namespace myDadApp.Controllers
{
    [Authorize]
    public class ChoresController : Controller
    {
        private readonly myDataContext _context;
        private readonly Container _cosmos;

        public ChoresController(myDataContext context, Container cosmos)
        {
            _cosmos = cosmos;
            _context = context;
        }

        // GET: Chores
        public async Task<IActionResult> Index()
        {
            // From Cosmos
            return View(await GetCosmosItems());


            return View(await _context.Chore
                .Where(c => c.Owner == User.Identity.Name)
                .OrderByDescending(c => c.CreateDt)
                .ToListAsync());
        }

        private async Task<List<Chore>> GetCosmosItems()
        {
            List<Chore> myData = new List<Chore>();

            // Read a single query page from Azure cosmos DB as stream
            var myQueryDef = new Microsoft.Azure.Cosmos.QueryDefinition($"Select * from f where f.IsDone != true");

            var feedIterator = _cosmos.GetItemQueryIterator<Chore>(myQueryDef);

            while (feedIterator.HasMoreResults)
            {
                var set = await feedIterator.ReadNextAsync();
                myData.AddRange(set);
            }
            return myData.OrderBy(c=>c.CreateDt).ToList();
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
                chore.Owner = User.Identity.Name;

                // MB: Add cosmos item
                await _cosmos.CreateItemAsync(chore);

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
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description,IsDone,CreateDt,CompleteDt")] Chore chore)
        {
            if (id != chore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (chore.IsDone == true && chore.CompleteDt == null)
                    {
                        chore.CompleteDt = DateTime.UtcNow;
                    } 
                    else
                    {
                        chore.CompleteDt = null;
                    }

                    // MB: Update Cosmos
                    await _cosmos.UpsertItemAsync(chore);

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
