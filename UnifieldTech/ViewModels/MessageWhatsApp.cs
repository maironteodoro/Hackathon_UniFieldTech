using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace UnifieldTech.ViewModels
{
    public class MessageWhatsApp
    {
        public void EnviarCodigoValidacao(string celular, string codigo)
        {
            // Inicializar a configuração do Twilio
            TwilioClient.Init("AC2c556c8c4bbc592728c527807458b033", "");

            // Enviar mensagem pelo Twilio
            var messageOptions = new CreateMessageOptions(
                new PhoneNumber($"whatsapp:+55{celular}"))
            {
                From = new PhoneNumber("whatsapp:+14155238886"),
                Body = "Esse é seu código de validação: " + codigo
            };

            MessageResource.Create(messageOptions);
        }
    }
}
