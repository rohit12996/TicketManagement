using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TicketManagementSystemAPI.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Table = iText.Layout.Element.Table;

namespace TicketReport.Services
{
    public class PdfReportService
    {
        public async Task<byte[]> GenerateResolvedTicketsReportAsync(List<Ticket> data)
        {
            var d = JsonConvert.SerializeObject(data);
            var tickets = JsonConvert.DeserializeObject<List<Ticket>>(d);

            // Create a MemoryStream to hold the PDF
            using (var ms = new MemoryStream())
            {
                // Correct PdfWriter initialization with a MemoryStream
                var writer = new PdfWriter(ms);

                // Initialize the PdfDocument with the PdfWriter
                var pdf = new PdfDocument(writer);

                // Create a Document instance to add content
                var document = new Document(pdf);

                // Add Title to the Report
                document.Add(new Paragraph("Resolved Tickets Report")
                            .SetFontSize(18)
                            .SetTextAlignment(TextAlignment.CENTER));

                // Define column widths for the table
                float[] columnWidths = { 1, 3, 3, 3,3,3 }; // Adjust column widths as needed
                var table = new Table(columnWidths);

                // Add table headers
                table.AddCell("Ticket ID");
                table.AddCell("Issue Description");
                table.AddCell("Priority");
                table.AddCell("Status");
                table.AddCell("Assigned to");
                table.AddCell("Title");

                // Add ticket data to the table
                foreach (var ticket in tickets)
                {
                    table.AddCell(ticket.Id.ToString());
                    table.AddCell(ticket.Description);
                    table.AddCell(ticket.Priority);
                    table.AddCell(ticket.Status);
                    table.AddCell(ticket.AssignedTo);
                    table.AddCell(ticket.Title);
                }

                // Add the table to the document
                document.Add(table);

                // Finalize the document and prepare for returning as a response
                document.Close();

                // Return the PDF as a byte array
                return ms.ToArray();
            }
        }
    }
}
