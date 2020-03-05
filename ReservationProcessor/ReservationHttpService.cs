using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationProcessor
{
	public class ReservationHttpService
	{
		private HttpClient Client;

		public ReservationHttpService(HttpClient client, IConfiguration config)
		{
			client.BaseAddress = new Uri(config.GetValue<string>("apiUrl"));
			client.DefaultRequestHeaders.Add("accept", "application/json");
			client.DefaultRequestHeaders.Add("User-Agent", "reservation-processor");
			Client = client;
		}

		public async Task<bool> MarkReservationAsAccepted(Reservation reservation)
		{
			return await JustDoIt(reservation, "approved");
		}

		public async Task<bool> MarkReservationAsCancelled(Reservation reservation)
		{
			return await JustDoIt(reservation, "cancelled");
		}

		private async Task<bool> JustDoIt(Reservation reservation, string status)
		{
			var reservationJson = JsonSerializer.Serialize(reservation);
			var content = new StringContent(reservationJson);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = await Client.PostAsync($"/reservations/{status}", content);
			return response.IsSuccessStatusCode;
		}
	}
}
