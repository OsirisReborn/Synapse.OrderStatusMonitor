using Synapse.OrderStatus.Infrastructure.Services;
using Synapse.OrderStatus.Infrastructure.AddInfrastructureExtensions;
using Synapse.OrderStatus.Domain.Interfaces;
using Synapse.OrderStatus.Infrastructure.Logging;
using Synapse.OrderStatus.API.Extensions;
using Synapse.OrderStatus.Api.Workers;

var builder = WebApplication.CreateBuilder(args);

//add serilog
builder.Services.AddSerilogExtensions(builder.Configuration);

//add infrastructure servicees
builder.Services.AddInfrastructureServices(builder.Configuration); 
       
//add worker services
builder.Services.AddWorkerServiceExtensions(builder.Configuration);

//add health check endpoint
builder.Services.AddHealthChecks()
    .AddCheck<OrderMonitorService>("OrderMonitorService");
    

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
