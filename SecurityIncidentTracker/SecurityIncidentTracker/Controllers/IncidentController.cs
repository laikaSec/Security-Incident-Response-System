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
    }
}