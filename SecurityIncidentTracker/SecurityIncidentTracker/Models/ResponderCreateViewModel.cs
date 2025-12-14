using System.ComponentModel.DataAnnotations;

namespace SecurityIncidentTracker.Models
{
    // This ViewModel represents the fields on the "Add Responder" form.
    // We keep it separate from ResponderViewModel so we can control validation rules.
    public class ResponderCreateViewModel
    {
        // Full name of the responder (required).
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
        public string Name { get; set; } = "";

        // Email is optional, but if entered it should look like an email address.
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(255)]
        public string? Email { get; set; }

        // Role is optional (Security Analyst, Incident Manager, etc.)
        [StringLength(100)]
        public string? Role { get; set; }

        // Allows us to deactivate someone without deleting them.
        public bool IsActive { get; set; } = true;
    }
}
