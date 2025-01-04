
using System.Text.Json.Serialization;
using LibraryDb.Model.LibraryContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            var connectionString = string.Empty;
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
            {
	            connectionString = builder.Configuration.GetConnectionString("SqlBooksDb");
            }
            else if (environment == "Azure")
            {
	            connectionString = builder.Configuration.GetConnectionString("BooksDb");
            }
            else
            {
	            throw new InvalidOperationException("Environment is not set/valid");
            }

            builder.Services.AddDbContext<LibraryContext>(opt =>
	            opt.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			var app = builder.Build();
			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
				db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
			}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
				app.UseSwagger();
				app.UseSwaggerUI();
			}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
