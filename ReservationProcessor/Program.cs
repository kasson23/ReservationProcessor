using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMqUtils;


namespace ReservationProcessor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					var rabittConfig = hostContext.Configuration.GetSection("rabbit");
					services.Configure<RabbitOptions>(rabittConfig);
					services.AddHttpClient<ReservationHttpService>(); // need nuget package
					services.AddHostedService<ReservationListener>(); // this is our program
				});
	}
}
