using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationProcessor
{
	public class Reservation
	{
		public int Id { get; set; }
		public string For { get; set; }
		public string Status { get; set; }
		public DateTime ReservationCreated { get; set; }
		public List<string> Books { get; set; }
	}
}
