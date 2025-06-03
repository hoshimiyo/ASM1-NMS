 using BLL.Utils;
using NMS_API_FE.Services;
using NMS_API_FE.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<IAdminService, AdminService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<ICategoryService, CategoryService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<IGuestService, GuestService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<ILecturerService, LecturerService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<INewsArticlesService, NewsArticleService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<IStaffService, StaffService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<ITagService, TagService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});
builder.Services.AddHttpClient<INewsTagService, NewsTagService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115"); // Adjust the base URL as needed
});

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
    pattern: "{controller=Guest}/{action=All}/{id?}");

app.Run();
