using Microsoft.EntityFrameworkCore;
using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(RohitContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Ticket>> GetResolvedTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }
    }

}
