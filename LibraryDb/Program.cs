
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
            
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
	        var connectionString = builder.Configuration.GetConnectionString("BooksDb");
	        
            builder.Services.AddDbContext<LibraryContext>(opt =>
	            opt.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
	            options.AddPolicy("AllowAll", builder =>
		            builder.AllowAnyOrigin()
			            .AllowAnyMethod()
			            .AllowAnyHeader());
            });

			var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
				app.UseSwagger();
				app.UseSwaggerUI();
			}

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

			app.UseAuthorization();

            
			app.MapControllers();

            app.Run();

           
		}
    }
}
