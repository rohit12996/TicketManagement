using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystemAPI.Models;
using TicketReport.Services;

namespace TicketManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly RohitContext _context;
        private readonly PdfReportService _pdfReportService;

        public TicketsController(RohitContext context, PdfReportService pdfReportServices)
        {
            _context = context;
            _pdfReportService = pdfReportServices;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest();
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
                if (ticket == null) return NotFound();
                return ticket;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket updatedTicket)
        {
            try
            {
                if (id != updatedTicket.Id) return BadRequest();
                _context.Entry(updatedTicket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Ticket Updated Successfully");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetAllTickets()
        {
            try
            {
                var data = await _context.Tickets.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
  
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("report/resolved")]
        public async Task<IActionResult> GetResolvedTicketsReport()
        {
            try
            {
                var data = await _context.Tickets.ToListAsync();
                var pdfBytes = await _pdfReportService.GenerateResolvedTicketsReportAsync(data);
                return File(pdfBytes, "application/pdf", "ResolvedTicketsReport.pdf");
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
            
        }
    }
}
