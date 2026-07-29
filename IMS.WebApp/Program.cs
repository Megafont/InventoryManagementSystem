using IMS.Plugins.EFCoreSqlServer;
using IMS.Plugins.InMemory;
using IMS.UseCases.Activities;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.Reports;
using IMS.UseCases.Reports.Interfaces;
using IMS.WebApp.Components;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;

// NOTE:
// This project was built starting with this course as the base:
// https://www.youtube.com/watch?v=yc6obH1DPus


var builder = WebApplication.CreateBuilder(args);

// Since we are using blazor pages (and especially since we're using server interactivity on some pages in this project),
// we cannot or at least should not use the normal builder.AddDbContext() here. The course says due to this, it is not
// clear what a transient lifetime would mean here so Blazor doesn't know when to dispose an instance of the DbContext.
// So we need to use builder.Services.AddDbContextFactory() instead.
// We are doing this to ensure the DbContext always has short-lived instances to avoid thread issues.
builder.Services.AddDbContextFactory<IMS_Db_Context>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryManagement"));

});

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents(); // Enables interactive SSR (server side rendering), which were using on the products page. On the other hand, the inventories page is using static SSR (non-interactive SSR). There is another function like this one below on the app.MapRazorComponents() call.


// If we are in the testing environment, then use the in-memory repositories.
// NOTE: The environments are defined in launchSettings.json.
//       In each profile there, the ASPNETCORE_ENVIRONMENT key sets the environment
//       ASP.NET Core will use when the app is running under that profile.
//		 Which profile runs depends on which one is selected in Visual Studio's run button.
if (builder.Environment.IsEnvironment("Testing"))
{
	// This is needed, as otherwise the static assets in the www folder don't get loaded when running the
	// app in the "Testing" environment.
	StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

	builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
	builder.Services.AddSingleton<IProductRepository, ProductRepository>();
	builder.Services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();
	builder.Services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();
}
else // We are not in the testing environment, so use our Entity Framework Core SQL Server database-backed repositories.
{
	// Unlike the in-memory plugins above, these ones need to use transient lifetime.
	// This is because they don't need to stay in memory for as long as the application is running.
	// The in-memory plugins above do because they are also acting as the database itself,
	// (in other words, each one is not just accessing the data but also storing it).
	// These EFCore plugins don't have to store the data, since the database is doing that.
	builder.Services.AddTransient<IInventoryRepository, InventoryEFCoreRepository>();
	builder.Services.AddTransient<IProductRepository, ProductEFCoreRepository>();
	builder.Services.AddTransient<IInventoryTransactionRepository, InventoryTransactionEFCoreRepository>();
	builder.Services.AddTransient<IProductTransactionRepository, ProductTransactionEFCoreRepository>();
}

builder.Services.AddTransient<IGetInventoriesByNameUseCase, GetInventoriesByNameUseCase>();
builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
builder.Services.AddTransient<IDeleteInventoryByIdUseCase, DeleteInventoryByIdUseCase>();
builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
builder.Services.AddTransient<IGetInventoryByIdUseCase, GetInventoryByIdUseCase>();

builder.Services.AddTransient<IGetProductsByNameUseCase, GetProductsByNameUseCase>();
builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
builder.Services.AddTransient<IDeleteProductByIdUseCase, DeleteProductByIdUseCase>();
builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();
builder.Services.AddTransient<IGetProductByIdUseCase, GetProductByIdUseCase>();

builder.Services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();
builder.Services.AddTransient<IProduceProductUseCase, ProduceProductUseCase>();
builder.Services.AddTransient<ISellProductUseCase, SellProductUseCase>();

builder.Services.AddTransient<ISearchInventoryTransactionsUseCase, SearchInventoryTransactionsUseCase>();
builder.Services.AddTransient<ISearchProductTransactionsUseCase, SearchProductTransactionsUseCase>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	// NOTE: This forces it to use HTTPS (HSTS = HTTP Strict-Transport-Security)
	app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode(); // Enables interactive SSR (server side rendering), which were using on the products page. On the other hand, the inventories page is using static SSR (non-interactive SSR). There is another function like this one above on the builder.Services.AddRazorComponents() call.

app.Run();
