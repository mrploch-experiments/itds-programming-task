using System.Text.Json.Serialization;
using CardActionsApp.Business;
using CardActionsApp.WebApi;
using CardActionsApp.WebApi.Endpoints;
using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICardService, CardService>()
       .AddSingleton<ICardActionsService, CardActionsService>()
       .AddSingleton<GetAllowedCardActions>();

builder.Services.ConfigureHttpJsonOptions(options =>
                                          {
                                              options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                          });
builder.Services.Configure<JsonOptions>(options =>
                                        {
                                            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("/v1/cardactions").MapCardActionsApiV1();

app.Run();

public partial class Program
{ }