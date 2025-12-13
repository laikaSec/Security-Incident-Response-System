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

        // Placeholder for the Create page (GET request)
        // This will eventually show the blank form.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // TODO: Load dropdown data (types, responders) and show the view
            return View();
        }
    }
}