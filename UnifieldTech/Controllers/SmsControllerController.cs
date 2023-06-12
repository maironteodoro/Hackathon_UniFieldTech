using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class TwilioController : ApiController
{
	[Microsoft.AspNetCore.Mvc.Route("sendmessage")]
	[Microsoft.AspNetCore.Mvc.HttpPost]
	public IHttpActionResult SendMessage()
	{
		string? accountSid = Environment.GetEnvironmentVariable("AC2c556c8c4bbc592728c527807458b033");
		string? authToken = Environment.GetEnvironmentVariable("15b5ff25237e8b7b2a8e1c4f205cacdb");

		TwilioClient.Init(accountSid, authToken);

		var message = MessageResource.Create(
			body: "This is the ship that made the Kessel Run in fourteen parsecs?",
			from: new Twilio.Types.PhoneNumber("+13613011261"), 
			to: new Twilio.Types.PhoneNumber("+5535998653759")
		);

		return Ok(message.Sid);
	}
}
