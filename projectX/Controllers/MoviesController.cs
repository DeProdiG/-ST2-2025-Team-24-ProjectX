using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectX.Models;

namespace projectX.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieActors!)
                .ThenInclude(ma => ma.Actor)
                .ToListAsync();

            return View(movies);
        }

        private void PopulateActorsDropdown(IEnumerable<int>? selectedActorIds = null)
        {
            var actors = _context.Actors?.ToList() ?? new List<Actor>();
            var selectList = new SelectList(actors, "Id", "Name");

            if (selectedActorIds != null)
            {
                foreach (var item in selectList)
                {
                    item.Selected = selectedActorIds.Contains(int.Parse(item.Value));
                }
            }

            ViewData["ActorId"] = selectList;
        }

        public IActionResult Create()
        {
            PopulateActorsDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Duration,ReleaseDate,Rating")] Movie movie, int[] selectedActors)
        {
            if (ModelState.IsValid)
            {
                // Save movie first to get the ID
                _context.Add(movie);
                await _context.SaveChangesAsync(); // This generates the movie.Id

                // Now handle many-to-many relationship with actors
                if (selectedActors != null)
                {
                    foreach (var actorId in selectedActors)
                    {
                        movie.MovieActors!.Add(new MovieActors
                        {
                            MovieId = movie.Id, // Now this has the actual ID
                            ActorId = actorId
                        });
                    }
                    // Save changes again to persist the relationships
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateActorsDropdown();
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Pass current actor IDs for pre-selection
            var currentActorIds = movie.MovieActors?.Select(ma => ma.ActorId).ToList();
            PopulateActorsDropdown(currentActorIds);

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Duration,ReleaseDate,Rating")] Movie movie, int[] selectedActors)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing movie with its actors
                    var existingMovie = await _context.Movies
                        .Include(m => m.MovieActors)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingMovie == null)
                    {
                        return NotFound();
                    }

                    // Update scalar properties
                    existingMovie.Title = movie.Title;
                    existingMovie.Duration = movie.Duration;
                    existingMovie.ReleaseDate = movie.ReleaseDate;
                    existingMovie.Rating = movie.Rating;

                    // Update actors relationship
                    existingMovie.MovieActors?.Clear();

                    if (selectedActors != null)
                    {
                        foreach (var actorId in selectedActors)
                        {
                            existingMovie.MovieActors!.Add(new MovieActors
                            {
                                MovieId = id,
                                ActorId = actorId
                            });
                        }
                    }

                    _context.Update(existingMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            PopulateActorsDropdown();
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}