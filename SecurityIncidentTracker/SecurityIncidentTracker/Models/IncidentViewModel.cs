using System;

namespace SecurityIncidentTracker.Models
{
    // This class holds the data we want to display on the dashboard page
    // It combines information from multiple database tables into one object
    // so the view (webpage) can easily show all the incident information
    public class IncidentViewModel
    {
        // Unique identifier for the incident (comes from the Incidents table)
        public int IncidentID { get; set; }

        // The title or brief description of what happened
        public string Title { get; set; } = "";

        // A longer description explaining what happened.
        public string Description { get; set; } = "";

        // How serious the incident is: Critical, High, Medium, or Low
        public string Severity { get; set; } = "";

        // Where are we in handling this incident? New, In Progress, Resolved, or Closed
        public string Status { get; set; } = "";

        // What type of incident is it? (Phishing, Malware, etc. - from IncidentTypes table)
        public string IncidentType { get; set; } = "";

        // Which team member is handling this incident? (from Responders table)
        public string AssignedResponder { get; set; } = "";

        // When was this incident first detected?
        public DateTime DetectedDate { get; set; }

        // The IP address where the incident originated from
        public string? SourceIP { get; set; }

        // Which system or computer was affected?
        public string? AffectedSystem { get; set; }

        // How many hours have passed since the incident was detected
        // (we calculate this in the query, not stored in database)
        public int HoursSinceDetection { get; set; }

        // Used by the default Error view; safe to leave as-is.
        public string? RequestId { get; set; }

        // Convenience property: true if RequestId has a value.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    // This class holds summary statistics about all incidents for the dashboard
    // It shows things like "5 critical incidents", "average response time 2 hours", etc.
    public class DashboardMetrics
    {
        // Total number of incidents that are not yet closed
        public int TotalActive { get; set; }

        // Count of Critical severity incidents
        public int CriticalCount { get; set; }

        // Count of High severity incidents
        public int HighCount { get; set; }

        // Count of Medium severity incidents
        public int MediumCount { get; set; }

        // Count of Low severity incidents
        public int LowCount { get; set; }

        // Count of incidents with "New" status (not yet started)
        public int NewCount { get; set; }

        // Count of incidents currently being worked on
        public int InProgressCount { get; set; }

        // Average time in minutes it takes to respond to an incident
        public double AvgResponseTimeMinutes { get; set; }
    }

    // This class represents a responder (team member) in our system
    public class ResponderViewModel
    {
        // Unique ID of the responder
        public int ResponderID { get; set; }

        // Full name of the team member
        public string Name { get; set; } = "";

        // Email address
        public string Email { get; set; } = "";

        // Their role on the team (e.g., "Security Analyst", "Incident Manager")
        public string Role { get; set; } = "";

        // Is this person still active? (true = active, false = inactive/left team)
        public bool IsActive { get; set; } = true;
    }

    // This class holds the incident type information
    public class IncidentTypeViewModel
    {
        // Unique ID for the type
        public int TypeID { get; set; }

        // Name of the type (e.g., "Phishing", "Malware")
        public string TypeName { get; set; } = "";

        // Brief description of what this type of incident is
        public string Description { get; set; } = "";
    }
}

   
