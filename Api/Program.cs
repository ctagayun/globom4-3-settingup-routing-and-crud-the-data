using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

//add the HouseDbContext to the dependency injection container
//I will be registering it with a scope of "Scope" which means a new instance
//will be created for each request that the API will receive.
//Because of that I am turning off a feature of database context that
//tracks each entity instance for property changes. It is more performant
//this way
builder.Services.AddDbContext<HouseDbContext>(o => 
    o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddScoped<IHouseRepository, HouseRepository>();
//builder.Services.AddScoped<IBidRepository, BidRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//allow everything from localhost:3000
app.UseCors(p => p.WithOrigins("http://localhost:3000")
    .AllowAnyHeader().AllowAnyMethod().AllowCredentials());

app.UseHttpsRedirection();

//*Now we will access the HouseDBContext. The DI container 
//*will provide an instance for me. We use the Interface instead of the 
//concrete HouseRepository because our repo will not be dependent 
//on a specific database. No need to "await the task". That will be 
//handled by the framework
app.MapGet("/houses", (IHouseRepository repo) => repo.GetAll())
          .Produces<HouseDto[]>(StatusCodes.Status200OK);
                      //*this means access the Houses property
                      //*of the HouseDbContext which contains a collection 
                      //*HousesEntity. It will be automatically serialized to JSON

//*This means use only this EP when the second ppart of the EP is an integer.
//*now in the lambda we can determine that houseID is the FIRST parameter. ASP.Netcore
//*is smart enough to see that: {houseId:int} == int houseId.
//*The second parameter is IHouseRepository coming from the DEPENDENCY INJECTOR CONTAINER
app.MapGet("/house/{houseId:int}", async(int houseId,IHouseRepository repo) =>
   {
     var house = await repo.Get(houseId); //*now get the house
     if (house == null)
       //*to determine the problem the standard way of doing this is to use the "Results"
       //*object to determine the problem
       return Results.Problem($"House with ID {houseId} not found.",
              statusCode: 404);

     //*Use the result object to determine to the status code to return.
     return Results.Ok(house);
     //*This is the metadata forSwagger has to be added again so that it knows 
     //*EP producess a 404 and 200OK
   }).ProducesProblem(404).Produces<HouseDetailDto>(StatusCodes.Status200OK);

  //*Add house
  //*Tell the api to look for the dto in the body. this is the first parameter,
  //*second parameter is the IHouseRepository
  app.MapPost("/houses", async([FromBody] HouseDetailDto dto, IHouseRepository repo) =>
   {
     var newHouse = await repo.Add(dto); //*now get the house

     //*Use the result object to determine to the status code to return.
     //*The first param by REST convention we return the URL and ID where the newly 
     //*created house can be found. The second param is the house as it was added to the 
     //*database
     return Results.Created($"/houses/{newHouse.id}", newHouse);
     //*This is the metadata forSwagger has to be added again so that it knows 
     //*EP producess a 201
   }).ProducesProblem(404).Produces<HouseDetailDto>(StatusCodes.Status200OK);          

  //*Update house
  //*Tell the api to look for the dto in the body. this is the first parameter,
  //*second parameter is the IHouseRepository
  app.MapPut("/houses", async([FromBody] HouseDetailDto dto, IHouseRepository repo) =>
   {
     //*We need to make sure that the house in the request body actually exist 
     if(await repo.Get(dto.Id) == null) 
       return Results.Problem($"House with ID {dto.Id} not found.",
              statusCode: 404);

     var updatedHouse = await repo.Update(dto); //*now update the house
    
     return Results.Ok(updatedHouse);

     //*This is the metadata forSwagger has to be added again so that it knows 
     //*EP producess a 201
   }).ProducesProblem(404).Produces<HouseDetailDto>(StatusCodes.Status200OK);        

    //* Delete House
    app.MapDelete("/houses/{houseId:int}", async(int houseId, IHouseRepository repo) =>
   {
     if(await repo.Get(houseId) == null) 
       return Results.Problem($"House with ID {houseId} not found.",
              statusCode: 404);

     //*Note no date to return
     await repo.Delete(houseId); //*now delete the house
    
     return Results.Ok();

     //*This is the metadata forSwagger has to be added again so that it knows 
     //*EP producess a 201
   }).ProducesProblem(404).Produces<HouseDetailDto>(StatusCodes.Status200OK);      

app.Run();

