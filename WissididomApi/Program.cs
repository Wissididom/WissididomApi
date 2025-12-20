using WissididomApi.Logic;

namespace WissididomApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotEnv.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddScoped<IVersionsInfo, VersionsInfo>();
            builder.Services.AddScoped<TwitchApi>(sp => new TwitchApi(Environment.GetEnvironmentVariable("TWITCH_CLIENT_ID")!, Environment.GetEnvironmentVariable("TWITCH_CLIENT_SECRET")!));
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}
