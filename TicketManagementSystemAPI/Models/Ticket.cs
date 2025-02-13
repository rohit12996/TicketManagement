using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystemAPI.Models;

public partial class Ticket
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
    public string Title { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
    public string? Description { get; set; }

    [Required]
    [RegularExpression(@"^(Low|Medium|High)$", ErrorMessage = "Priority must be Low, Medium, or High.")]
    public string Priority { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(Open|Resolved)$", ErrorMessage = "Status must be Open or Resolved.")]
    public string Status { get; set; } = null!;

    [StringLength(100, ErrorMessage = "AssignedTo length can't be more than 100 characters.")]
    public string? AssignedTo { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
}
