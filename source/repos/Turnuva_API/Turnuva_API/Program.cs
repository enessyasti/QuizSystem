using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data;
using TournamentAPI.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tournament.db"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections() 
    .AddFiltering()   
    .AddSorting();    

var app = builder.Build();

app.MapGraphQL();

app.Run();