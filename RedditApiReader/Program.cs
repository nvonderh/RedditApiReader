using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RedditApiReader.Data;
using RedditApiReader.HostedServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RedditApiReaderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RedditApiReaderContext") ?? throw new InvalidOperationException("Connection string 'RedditApiReaderContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<GetNewPostsHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RedditInfoItems}/{action=Index}/{id?}");

app.Run();
