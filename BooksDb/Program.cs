using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.Json.Serialization;
using BooksDb.Models;
using BooksDb.Services;


namespace BooksDb
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers().AddJsonOptions(options => 
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

			builder.Services.AddScoped<AuthorService>();
			builder.Services.AddScoped<BookService>();
			builder.Services.AddScoped<BorrowerService>();
			builder.Services.AddScoped<CopyService>();
			builder.Services.AddScoped<BookLoanService>();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Configuration.AddUserSecrets<Program>();
			var conString = builder.Configuration.GetConnectionString("BooksDb");

			if (builder.Environment.IsEnvironment("Azure"))
			{
				var password = builder.Configuration["DbPassword"];

				var sqlConnectionBuilder = new SqlConnectionStringBuilder(conString)
				{
					Password = password
				};

				conString = sqlConnectionBuilder.ConnectionString;
			}

			builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(conString);
			});
			

			var app = builder.Build();

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
