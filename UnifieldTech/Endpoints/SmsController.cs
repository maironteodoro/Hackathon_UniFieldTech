using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using UnifieldTech.Models;
using UnifieldTech.Data;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {

        [HttpPost]
        public ActionResult Post(Cliente cliente, UnifieldTechContext db)
        {
            // Lógica para enviar SMS utilizando a biblioteca Twilio
            TwilioClient.Init("AC2c556c8c4bbc592728c527807458b033", "15b5ff25237e8b7b2a8e1c4f205cacdb");
            var messageOptions = MessageResource.Create(
                body: cliente.Codigo,
                from: new PhoneNumber("whatsapp:+14155238886"),
                to: new PhoneNumber("whatsapp:+553591529241")
            );

            return Ok(messageOptions.Sid); // Retorna uma resposta HTTP 200 OK com o SID do Twilio
        }
    }
}