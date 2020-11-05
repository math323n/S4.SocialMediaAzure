using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using S4.SocialMediaAzure.DataAccess.Base;
using S4.SocialMediaAzure.Entities.Models;
using S4.SocialMediaAzure.Entities.Models.Context;

namespace S4.SocialMediaAzure.WebApp.Controllers
{
    public class PostController : Controller
    {
        private RepositoryBase<AspNetPost> repo;
        private readonly RepositoryBase<AspNetComment> commentRepo;

        public PostController(RepositoryBase<AspNetPost> repo, RepositoryBase<AspNetComment> commentRepo)
        {
            this.repo = repo;
            this.commentRepo = commentRepo;
        }


        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await repo.GetAllAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            AspNetPost post = await repo.GetByIdAsync(id);
            if(post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            // ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Image,IsEdited,Created,UserId")] AspNetPost post)
        {
            if(ModelState.IsValid)
            {
                post.Created = DateTime.Now;
                post.FkUserId= User.FindFirstValue(ClaimTypes.NameIdentifier);
                await repo.AddAsync(post);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(post.FkUserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            AspNetPost post = await repo.GetByIdAsync(id);

            if(post == null)
            {
                return NotFound();
            }

            if(post.FkUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(post.FkUserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Image,IsEdited,Created,UserId")] AspNetPost post)
        {
            if(id != post.PkId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    AspNetPost originalPost = await repo.GetByIdAsync(id);

                    originalPost.IsEdited = true;

                    originalPost.Title = post.Title;
                    originalPost.Description = post.Description;
                    originalPost.Image = post.Image;

                    await repo.UpdateAsync(originalPost);
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!await PostExists(post.PkId))
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
            ViewData["UserId"] = new SelectList(post.FkUserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            AspNetPost post = await repo.GetByIdAsync(id);
            if(post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AspNetPost post = await repo.GetByIdAsync(id);
            await repo.DeleteAsync(post);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reply()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, [Bind("Description,PostId,Image,IsEdited,Created,UserId")] AspNetComment comment)
        {
            AspNetPost post = await repo.GetByIdAsync(id);
            if(ModelState.IsValid)
            {
                comment.Created = DateTime.Now;
                comment.FkUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                comment.FkPostId = post.PkId;
                await commentRepo.AddAsync(comment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(post.FkUserId);
            return View(post);
        }

        public async Task<IActionResult> Comments(int id)
        {
            AspNetPost post = await repo.GetByIdAsync(id);
            if(post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        private async Task<bool> PostExists(int id)
        {
            return await repo.Exists(id);

        }
    }
}
