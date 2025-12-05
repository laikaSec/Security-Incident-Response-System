using Microsoft.AspNetCore.Mvc;
using SecurityIncidentTracker.Models;
using SecurityIncidentTracker.Services;

namespace SecurityIncidentTracker.Controllers
{
    // This controller handles requests to the home/dashboard page.
    // When someone visits the webiste root ("/"), this controller responds.
    public class HomeController : Controller
    {
        // This variable holds a reference to our IncidentService.
        // We use it to get incident data from the database. 
        private readonly IncidentService _incidentService;

        // Constructor: ASP.NET automatically provides ("injects") the Incident Service
        // because we registered it in Program.cs with AddScoped<IncidentService>(). 
        // This pattern is called "Dependency Injection" - it makes code easier to test and manage. 
        public HomeController(IncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        // This method handles GET requests to "/" or "/Home/Index".
        // It fetches incident data from the database and sends it to the view. 
        // "async" means this method can wait for the database without freezing the app. 
        public async Task<IActionResult> Index()
        {
            // Ask the IncidentService to get all active (non-closed) incidents. 
            // "await" mean "wait for this to finish before continuing".
            var incidents = await _incidentService.GetActiveIncidentsAsync();

            // Ask the IncidentService to get summary statistics (counts, averages).
            var metrics = await _incidentService.GetDashboardMetricsAsync();

            // Store the metrics in ViewBag so the view can access them.
            // ViewBag is a simple way to pass extra data to the view.
            ViewBag.Metrics = metrics;

            // Return the Index view with the list of incidents as its "model".
            // The view will loop through this list to display each incident.
            return View(incidents);
        }

        // This method handles the Privacy page (default template page).
        // We keep it so the app doesn't break if someone clicks the Privacy link.
        public IActionResult Privacy()
        {
            return View();
        }

        // This method handles errors.
        // If something goes wrong, ASP.NET shows the Error view.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new IncidentViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}