using Microsoft.AspNetCore.Mvc;
using SecurityIncidentTracker.Models;
using SecurityIncidentTracker.Services;

namespace SecurityIncidentTracker.Controllers
{
    // This controller manages incident-specific actions like creating, viewing details, and editing.
    public class IncidentController : Controller
    {
        // Reference to our data service
        private readonly IncidentService _incidentService;

        // Constructor: inject the IncidentService so we can talk to the database
        public IncidentController(IncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        // GET: Incident/Create
        // Shows the blank form to create a new incident. 
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // 1. Get the list of incident types for the dropdown
            var types = await _incidentService.GetIncidentTypesAsync();

            // 2. Get the list of responders for the dropdown
            var responders = await _incidentService.GetRespondersAsync();

            // 3. Pass these lists to the view using ViewBag
            // ViewBag is a dynamic container for data the view needs but isn't part of the main model.
            ViewBag.IncidentTypes = types;
            ViewBag.Responders = responders;

            // 4. Return the Create view
            return View();
        }

        // POST: Incident/Create
        // Handles the form submission.
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against Cross-Site Request Forgery attacks
        public async Task<IActionResult> Create(IncidentCreateViewModel model)
        {
            // 1. Check if the form data is valid (based on [Required] attributes in the model)
            if (ModelState.IsValid)
            {
                // 2. Call the service to save the incident to the database
                var newId = await _incidentService.CreateIncidentAsync(model);

                if (newId.HasValue)
                {
                    // 3. If successful, go back to the Dashboard
                    // In the future, we could redirect to the "Details" page for this new incident
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // If the database insert failed for some reason, show an error
                    ModelState.AddModelError("", "Failed to create incident in database.");
                }
            }

            // 4. If we got here, something failed (validation error or database error).
            // We need to reload the dropdowns because HTTP is stateless (they don't persist automatically)
            ViewBag.IncidentTypes = await _incidentService.GetIncidentTypesAsync();
            ViewBag.Responders = await _incidentService.GetRespondersAsync();

            // 5. Show the form again with the user's data and error messages
            return View(model);
        }
    }
}