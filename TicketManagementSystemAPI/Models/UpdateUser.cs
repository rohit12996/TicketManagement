using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystemAPI.Models
{
    public class UpdateUser
    {
            [Required]
            public string Username { get; set; }
            public string Role { get; set; } // Optional: Allow role updates
    }
}
