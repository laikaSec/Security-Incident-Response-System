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
    }
}
