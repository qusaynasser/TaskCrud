
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskCrud.Data;
using TaskCrud.DTO_s.Product;
using TaskCrud.Error_s;

namespace TaskCrud
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IValidator<ProductDto>, CreateProductValidationDto>();
            builder.Services.AddExceptionHandler<GlobalExcptionHandler>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Host.UseSerilog((cotext, configuration) =>
            {
                configuration.ReadFrom.Configuration(cotext.Configuration);
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
