using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectX.Models;
using System.Net.Sockets;

namespace projectX.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = _context.Reviews
                .Include(t => t.User)
                .Include(t => t.Movie);
            return View(await reviews.ToListAsync());
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comment,Title,Username")] Review review)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == review.UserId);
            var movie = _context.Movies.FirstOrDefault(x => x.Id == review.MovieId);

            review.User = user;
            review.Movie = movie;

            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(review.UserId, review.MovieId);
            return View(review);
        }

        private void PopulateDropdowns(object? selectedUserId = null, object? selectedScreeningId = null)
        {
            var users = _context.Users?.ToList() ?? new List<User>();
            var movies = _context.Movies?.ToList() ?? new List<Movie>();

            ViewData["MovieId"] = new SelectList(movies, "Id", "Title", selectedScreeningId);
            ViewData["UserId"] = new SelectList(users, "Id", "Username", selectedUserId);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            PopulateDropdowns(review.UserId, review.MovieId);
            return View(review);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,ScreeningId,UserId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(review.Id))
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
            PopulateDropdowns(review.UserId, review.MovieId);
            return View(review);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(t => t.User)
                .Include(t => t.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
