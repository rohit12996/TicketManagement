using TicketManagementSystemAPI.Models;
using TicketReport.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystemAPI.Repository;

namespace TicketManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly PdfReportService _pdfReportService;

        public TicketsController(ITicketRepository ticketRepository, PdfReportService pdfReportService)
        {
            _ticketRepository = ticketRepository;
            _pdfReportService = pdfReportService;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            try
            {
                if (ticket == null)
                {
                    return BadRequest();
                }

                await _ticketRepository.AddAsync(ticket);
                return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(id);
                if (ticket == null) return NotFound();
                return ticket;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket updatedTicket)
        {
            try
            {
                if (id != updatedTicket.Id) return BadRequest();

                await _ticketRepository.UpdateAsync(updatedTicket);
                return Ok("Ticket Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetAllTickets()
        {
            try
            {
                var data = await _ticketRepository.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("report/resolved")]
        public async Task<IActionResult> GetResolvedTicketsReport()
        {
            try
            {
                List<Ticket> data = (List<Ticket>)await _ticketRepository.GetResolvedTicketsAsync();
                var pdfBytes = await _pdfReportService.GenerateResolvedTicketsReportAsync(data);
                return File(pdfBytes, "application/pdf", "ResolvedTicketsReport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
