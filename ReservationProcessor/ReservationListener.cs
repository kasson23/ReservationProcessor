using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMqUtils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationProcessor
{
	public class ReservationListener : RabbitListener
	{
		private readonly ILogger<ReservationListener> Logger;
		private readonly ReservationHttpService Service;

		public ReservationListener(ILogger<ReservationListener> logger, 
			IOptionsMonitor<RabbitOptions> options, 
			ReservationHttpService service) : base(options)
		{
			Logger = logger;
			Service = service;
			base.QueueName = "reservations";
			base.ExchangeName = "";
		}

		public async override Task<bool> Process(string message)
		{
			// using (var client = new HttpClient()) { } // this is desposing the client -- DO not do this

			// 1. deseralize the message (above) into an object (so we'll need a model)
			var request = JsonSerializer.Deserialize<Reservation>(message);
			// 2. Maybe we'll log it for fun
			Logger.LogInformation($"Got a reservation!!!!    {Environment.NewLine} \t {request.For}");
			// 3. Do our "business logic" (reservations with <= 3 books, it gets cancelled)
			var op = request.Books.Count <= 3 ? "cancelled" : "accepted";
			// 4. do the appropriate http call to our api
			if(request.Books.Count <= 3)
			{
				// 5. If the post works, then remove it from the queue
				return await Service.MarkReservationAsAccepted(request);
			}
			else
			{
				// 6. if it fails, it stays on the queue.
				return await Service.MarkReservationAsCancelled(request);
			}
		}
	}
}
