using System;
using System.Collections.Generic;

namespace TicketManagementSystemAPI.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? AssignedTo { get; set; }

    public DateTime CreatedDate { get; set; }
}
