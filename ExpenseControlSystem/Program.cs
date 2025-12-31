using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Services;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<CategoryServices>();
builder.Services.AddScoped<SubCategoryServices>();
builder.Services.AddScoped<ExpenseServices>();
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
