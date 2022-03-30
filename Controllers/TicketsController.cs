using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcExamenLuisEFrias.Filters;
using MvcExamenLuisEFrias.Models;
using MvcExamenLuisEFrias.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcExamenLuisEFrias.Controllers
{
    public class TicketsController : Controller
    {
        private ServiceApiTickets service;
        private ServiceStorageBlobs serviceBlobs;

        public TicketsController(ServiceApiTickets service, ServiceStorageBlobs serviceBlobs)
        {
            this.service = service;
            this.serviceBlobs = serviceBlobs;
        }

        [AuthorizeTickets]
        public async Task<IActionResult> Tickets()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            List<Ticket> tickets = await this.service.GetTicketsUsuarioAsync(token);
            return View(tickets);
        }

        public IActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(string idticket, string idusuario, DateTime fecha, string importe, string producto, IFormFile file)
        {
            //await this.serviceBlobs.UploadBlobAsync(ticket.FileName);

            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync(blobName, stream);
            }
            Ticket ticket = new Ticket();
            ticket.IdUsuario = int.Parse(idusuario);
            ticket.Fecha = fecha;
            ticket.Importe = importe;
            ticket.Producto = producto;
            ticket.FileName = blobName;
            ticket.StoragePath = "https://storagetajamarle.blob.core.windows.net/examen/" + blobName;
            ticket.IdTicket = int.Parse(idticket);

            await this.service.InsertTicketAsync(ticket);
            return RedirectToAction("Tickets");
        }
    }
}
