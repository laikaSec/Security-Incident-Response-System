using Microsoft.Data.SqlClient;
using SecurityIncidentTracker.Models;
using System.Data;

namespace SecurityIncidentTracker.Services
{
    // This class is responsible for all database operations related to incidents
    // It handles reading from and writing to the SQL database
    // By putting all database code in one place, it's easier to manage and test
    public class IncidentService
    {
        // This variable holds the connection string (the address and login info for the database)
        private readonly string _connectionString;

        // Constructor: when you create an IncidentService, pass in the connection string
        public IncidentService(IConfiguration configuration)
        {
            // Get the connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        // This method gets all active incidents (not closed) from the database
        // It returns a list of IncidentViewModel objects
        public async Task<List<IncidentViewModel>> GetActiveIncidentsAsync()
        {
            // Create an empty list to hold all the incidents we'll read from the database
            var incidents = new List<IncidentViewModel>();

            // "using" ensures the database connection closes automatically when done
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Write the SQL query that will get the data we need
                // LEFT JOIN means "include information from these tables even if there's no match"
                // This prevents errors if an incident has no assigned responder
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        i.IncidentID,
                        i.Title,
                        i.Description,
                        i.Severity,
                        i.Status,
                        it.TypeName,
                        r.Name AS AssignedResponder,
                        i.DetectedDate,
                        i.SourceIP,
                        i.AffectedSystem,
                        DATEDIFF(HOUR, i.DetectedDate, GETDATE()) AS HoursSinceDetection
                    FROM Incidents i
                    LEFT JOIN IncidentTypes it ON i.IncidentTypeID = it.TypeID
                    LEFT JOIN Responders r ON i.AssignedTo = r.ResponderID
                    WHERE i.Status NOT IN ('Closed')
                    ORDER BY 
                        CASE i.Severity
                            WHEN 'Critical' THEN 1
                            WHEN 'High' THEN 2
                            WHEN 'Medium' THEN 3
                            WHEN 'Low' THEN 4
                        END,
                        i.DetectedDate DESC", conn);
                // Open the database connection asynchronously (don't block the application)
                await conn.OpenAsync();

                // Execute the SQL query and get back a reader object
                // The reader lets us step through each row of results
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                // Loop through each row returned by the query
                while (await reader.ReadAsync())
                {
                    // Create a new IncidentViewModel and fill it with data from this row
                    incidents.Add(new IncidentViewModel
                    {
                        IncidentID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Severity = reader.GetString(3),
                        Status = reader.GetString(4),
                        IncidentType = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        AssignedResponder = reader.IsDBNull(6) ? "" : reader.GetString(6),
                        DetectedDate = reader.GetDateTime(7),
                        SourceIP = reader.IsDBNull(8) ? null : reader.GetString(8),
                        AffectedSystem = reader.IsDBNull(9) ? null : reader.GetString(9),
                        HoursSinceDetection = reader.GetInt32(10)
                    });
                }
            }

            // Return the filled list of incidents
            return incidents;
        }

        // This method gets dashboard metrics (statistics about all incidents)
        public async Task<DashboardMetrics> GetDashboardMetricsAsync()
        {
            // Create an empty metrics object
            var metrics = new DashboardMetrics();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // This query counts incidents by severity and status
                // SUM(CASE WHEN...) is a way to count only rows that match a condition
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        COUNT(*) AS TotalActive,
                        SUM(CASE WHEN Severity = 'Critical' THEN 1 ELSE 0 END) AS CriticalCount,
                        SUM(CASE WHEN Severity = 'High' THEN 1 ELSE 0 END) AS HighCount,
                        SUM(CASE WHEN Severity = 'Medium' THEN 1 ELSE 0 END) AS MediumCount,
                        SUM(CASE WHEN Severity = 'Low' THEN 1 ELSE 0 END) AS LowCount,
                        SUM(CASE WHEN Status = 'New' THEN 1 ELSE 0 END) AS NewCount,
                        SUM(CASE WHEN Status = 'In Progress' THEN 1 ELSE 0 END) AS InProgressCount,
                        AVG(DATEDIFF(MINUTE, DetectedDate, 
                            CASE WHEN Status IN ('Resolved', 'Closed') THEN ResolvedDate ELSE GETDATE() END)) AS AvgResponseTimeMinutes
                    FROM Incidents
                    WHERE Status NOT IN ('Closed')", conn);

                await conn.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    // Fill in all the metrics from the row we got back
                    metrics.TotalActive = reader.GetInt32(0);
                    metrics.CriticalCount = reader.GetInt32(1);
                    metrics.HighCount = reader.GetInt32(2);
                    metrics.MediumCount = reader.GetInt32(3);
                    metrics.LowCount = reader.GetInt32(4);
                    metrics.NewCount = reader.GetInt32(5);
                    metrics.InProgressCount = reader.GetInt32(6);
                    metrics.AvgResponseTimeMinutes = reader.IsDBNull(7) ? 0 : Convert.ToDouble(reader.GetInt32(7));
                }
            }

            return metrics;
        }

        // This method gets all responders (team members) from the database
        public async Task<List<ResponderViewModel>> GetRespondersAsync()
        {
            var responders = new List<ResponderViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ResponderID, Name, Email, Role, IsActive FROM Responders ORDER BY Name", conn);

                await conn.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    responders.Add(new ResponderViewModel
                    {
                        ResponderID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Role = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        IsActive = reader.GetBoolean(4)
                    });
                }
            }

            return responders;
        }

        // This method gets all incident types (Phishing, Malware, etc.)
        public async Task<List<IncidentTypeViewModel>> GetIncidentTypesAsync()
        {
            var types = new List<IncidentTypeViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TypeID, TypeName, Description FROM IncidentTypes ORDER BY TypeName", conn);

                await conn.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    types.Add(new IncidentTypeViewModel
                    {
                        TypeID = reader.GetInt32(0),
                        TypeName = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2)
                    });
                }
            }

            return types;
        }
    }
}