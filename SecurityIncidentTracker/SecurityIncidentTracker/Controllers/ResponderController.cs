using Microsoft.AspNetCore.Mvc;
using SecurityIncidentTracker.Models;
using SecurityIncidentTracker.Services;

namespace SecurityIncidentTracker.Controllers
{
    // This controller manages responders (the people incidents get assigned to).
    public class ResponderController : Controller
    {
        private readonly IncidentService _incidentService;

        // Dependency Injection: we receive the service through the constructor.
        public ResponderController(IncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        // GET: /Responder
        // Shows a list of responders.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Pull responders from the database and send them to the view.
            var responders = await _incidentService.GetRespondersAsync();
            return View(responders);
        }

        // GET: /Responder/Create
        // Shows a blank form for adding a responder.
        [HttpGet]
        public IActionResult Create()
        {
            // Send an empty model to the view.
            return View(new ResponderCreateViewModel());
        }

        // POST: /Responder/Create
        // Handles form submission.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResponderCreateViewModel model)
        {
            // Validate user input using Data Annotations (e.g., [Required], [EmailAddress]).
            if (!ModelState.IsValid)
            {
                // If invalid, show the form again with validation messages.
                return View(model);
            }

            // Save the responder to the database.
            var newResponderId = await _incidentService.CreateResponderAsync(model);

            if (!newResponderId.HasValue)
            {
                // If something went wrong at the DB layer, show an error.
                ModelState.AddModelError("", "Failed to create responder in database.");
                return View(model);
            }

            // PRG pattern: redirect after POST to prevent duplicate form submissions on refresh. [web:256]
            return RedirectToAction(nameof(Index));
        }
    }
}
