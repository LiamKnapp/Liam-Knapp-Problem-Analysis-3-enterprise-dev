using Microsoft.EntityFrameworkCore;
using QuoteAppServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add CORS support:
builder.Services.AddCors(options => {
    options.AddPolicy("AllowTaskClients", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Add DbContext
builder.Services.AddDbContext<QuotesDB>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("QuoteDB")
    )
);

// add our quote manager svc:
builder.Services.AddScoped<IQuoteManager, QuoteManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use cors:
app.UseCors("AllowTaskClients");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
