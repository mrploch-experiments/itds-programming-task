using System.Text.Json.Serialization;
using CardActionsApp.WebApi;
using CardActionsApp.WebApi.Endpoints;
using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

static async Task<IResult> GetAllTodos2(int id) => TypedResults.Ok();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICardService, CardService>();

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

//var cardActions = app.MapGroup("CardActions").WithTags("CardActions");
app.MapGroup("/cardactions/v1").MapCardActionsApiV1();
//cardActions.MapGet("/", CardActionsGet.Execute).Produces<CardActions>().WithName("GetCardActions").WithOpenApi();
//cardActions.MapGet("/all", CardDetailsGetAll.Execute).Produces<Dictionary<string, Dictionary<string, CardDetails>>>().WithName("GetAllCardDetails").WithOpenApi();

app.Run();


