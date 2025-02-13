using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Repository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetResolvedTicketsAsync();
    }

}
