using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectX.Models;
using projectX.Services.Patterns;
using System.Net.Sockets;

namespace projectX.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILoggerService _logger;
        private readonly PricingContext _pricingContext;
        private readonly Services.NotificationFactory _notificationFactory;

        public TicketController(AppDbContext context)
        {
            _context = context;
            _logger = LoggerService.Instance;
            _pricingContext = new PricingContext(new StandardPricingStrategy());
            _notificationFactory = new Services.NotificationFactory();
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Ticket index page accessed");
            var tickets = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Screening);
            return View(await tickets.ToListAsync());
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Ticket creation page accessed");
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,ScreeningId,UserId,Quantity,UserType")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // Apply pricing strategy based on user type
                var basePrice = ticket.Price; // This is double
                var userType = ticket.UserType ?? "Standard";

                // Set appropriate pricing strategy
                switch (userType.ToLower())
                {
                    case "student":
                        _pricingContext.SetStrategy(new StudentPricingStrategy());
                        break;
                    case "senior":
                        _pricingContext.SetStrategy(new SeniorPricingStrategy());
                        break;
                    case "premium":
                        _pricingContext.SetStrategy(new PremiumPricingStrategy());
                        break;
                    default:
                        _pricingContext.SetStrategy(new StandardPricingStrategy());
                        break;
                }

                // Calculate final price using strategy pattern
                ticket.Price = _pricingContext.ExecuteStrategy(basePrice, ticket.Quantity, userType);

                var user = _context.Users.FirstOrDefault(x => x.Id == ticket.UserId);
                var screening = _context.Screenings.FirstOrDefault(x => x.Id == ticket.ScreeningId);

                ticket.User = user;
                ticket.Screening = screening;

                _context.Add(ticket);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Ticket created with ID: {ticket.Id} for user: {user?.Username}");

                // Send notification using factory pattern
                if (user != null)
                {
                    var notification = _notificationFactory.CreateNotification(Services.NotificationType.Email);
                    var message = $"Your ticket for {screening?.Name} has been booked. Total: {ticket.Price:C}";
                    notification.Send(user.Email ?? user.Username, message);
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(ticket.UserId, ticket.ScreeningId);
            return View(ticket);
        }

        // Add new action to demonstrate patterns
        public IActionResult CalculatePrice(double basePrice, int quantity, string userType, string notificationType)
        {
            // Use strategy pattern for pricing
            switch (userType.ToLower())
            {
                case "student":
                    _pricingContext.SetStrategy(new StudentPricingStrategy());
                    break;
                case "senior":
                    _pricingContext.SetStrategy(new SeniorPricingStrategy());
                    break;
                case "premium":
                    _pricingContext.SetStrategy(new PremiumPricingStrategy());
                    break;
                default:
                    _pricingContext.SetStrategy(new StandardPricingStrategy());
                    break;
            }

            var finalPrice = _pricingContext.ExecuteStrategy(basePrice, quantity, userType);

            // Use factory pattern for notification
            if (Enum.TryParse<Services.NotificationType>(notificationType, true, out var notifType))
            {
                var notification = _notificationFactory.CreateNotification(notifType);
                notification.Send("test@example.com",
                    $"Price calculated: {basePrice:C} x {quantity} = {finalPrice:C} for {userType}");
            }

            return Json(new { basePrice, quantity, userType, finalPrice });
        }

        // View logs (demonstrating singleton)
        public IActionResult ViewLogs()
        {
            var logs = _logger.GetLogs();
            return View(logs);
        }

        private void PopulateDropdowns(object selectedUserId = null, object selectedScreeningId = null)
        {
            var users = _context.Users?.ToList() ?? new List<User>();
            var screenings = _context.Screenings?.ToList() ?? new List<Screening>();

            ViewData["ScreeningId"] = new SelectList(screenings, "Id", "Name", selectedScreeningId);
            ViewData["UserId"] = new SelectList(users, "Id", "Username", selectedUserId);

            // Add user types for pricing strategy
            ViewData["UserTypes"] = new List<SelectListItem>
            {
                new SelectListItem { Value = "standard", Text = "Standard" },
                new SelectListItem { Value = "student", Text = "Student" },
                new SelectListItem { Value = "senior", Text = "Senior" },
                new SelectListItem { Value = "premium", Text = "Premium" }
            };
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            PopulateDropdowns(ticket.UserId, ticket.ScreeningId);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,ScreeningId,UserId,Quantity,UserType")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Ticket updated with ID: {ticket.Id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            PopulateDropdowns(ticket.UserId, ticket.ScreeningId);
            return View(ticket);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Screening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Ticket deleted with ID: {id}");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}