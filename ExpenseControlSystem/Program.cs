using ExpenseControlSystem.Configurations;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Interfaces;
using ExpenseControlSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder
    .Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(
            option => {
                option.SuppressModelStateInvalidFilter = true;
            });

builder
    .Services
        .AddSwaggerGen()
        .AddEndpointsApiExplorer();

builder.Services.Configure<EmailConfigurations>(
    builder.Configuration.GetSection("SendEmail")
);

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<CategoryServices>();
builder.Services.AddScoped<SubCategoryServices>();
builder.Services.AddScoped<ExpenseServices>();
builder.Services.AddScoped<GenerateXlsx>();
builder.Services.AddHttpClient<IEmailServices, BrevoEmailServices>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ExpenseControlSystem.Data.ExpenseControlSystemDataContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
