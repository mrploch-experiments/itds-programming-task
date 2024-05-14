using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Itds.CardActionsMicroservice.Business.Services;
using Itds.CardActionsMicroservice.Business.UseCases;
using Itds.CardActionsMicroservice.WebApi.Endpoints;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICardService, CardService>().AddSingleton<ICardActionsService, CardActionsService>().AddSingleton<GetAllowedCardActions>();

builder.Services.ConfigureHttpJsonOptions(static options =>
                                          {
                                              options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                          });
builder.Services.Configure<JsonOptions>(static options =>
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

await app.RunAsync();

[SuppressMessage("ReSharper", "RedundantTypeDeclarationBody", Justification = "Required for the integration tests")]
public partial class Program
{ }