using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using S4.SocialMediaAzure.Entities.Models;
using S4.SocialMediaAzure.Entities.Models.Context;

namespace S4.SocialMediaAzure.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly SocialMediaContext _context;

        public PostController(SocialMediaContext context)
        {
            _context = context;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var socialMediaContext = _context.AspNetPosts.Include(a => a.FkUser);
            return View(await socialMediaContext.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetPost = await _context.AspNetPosts
                .Include(a => a.FkUser)
                .FirstOrDefaultAsync(m => m.PkId == id);
            if (aspNetPost == null)
            {
                return NotFound();
            }

            return View(aspNetPost);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            ViewData["FkUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PkId,Title,Description,Image,IsEdited,Created,Updated,FkUserId")] AspNetPost aspNetPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetPost.FkUserId);
            return View(aspNetPost);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetPost = await _context.AspNetPosts.FindAsync(id);
            if (aspNetPost == null)
            {
                return NotFound();
            }
            ViewData["FkUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetPost.FkUserId);
            return View(aspNetPost);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PkId,Title,Description,Image,IsEdited,Created,Updated,FkUserId")] AspNetPost aspNetPost)
        {
            if (id != aspNetPost.PkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetPostExists(aspNetPost.PkId))
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
            ViewData["FkUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetPost.FkUserId);
            return View(aspNetPost);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetPost = await _context.AspNetPosts
                .Include(a => a.FkUser)
                .FirstOrDefaultAsync(m => m.PkId == id);
            if (aspNetPost == null)
            {
                return NotFound();
            }

            return View(aspNetPost);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aspNetPost = await _context.AspNetPosts.FindAsync(id);
            _context.AspNetPosts.Remove(aspNetPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetPostExists(int id)
        {
            return _context.AspNetPosts.Any(e => e.PkId == id);
        }
    }
}
