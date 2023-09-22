using Edc.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddDbContext();
builder.AddAuthentication();

builder.AddAccountContext();

builder.AddMediatR();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapAccountRoutes();

app.Run();
