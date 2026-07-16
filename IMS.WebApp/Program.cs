using IMS.Plugins.InMemory;
using IMS.UseCases.Activities;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Products.Interfaces;
using IMS.WebApp.Components;

// NOTE:
// This project was built starting with this course as the base:
// https://www.youtube.com/watch?v=yc6obH1DPus


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents(); // Enables interactive SSR (server side rendering), which were using on the products page. On the other hand, the inventories page is using static SSR (non-interactive SSR). There is another function like this one below on the app.MapRazorComponents() call.


builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();
builder.Services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();

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
