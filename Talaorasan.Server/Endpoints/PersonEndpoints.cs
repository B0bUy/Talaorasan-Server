using Microsoft.AspNetCore.Mvc;
using Talaorasan.Server.Logic;
using Talaorasan.Shared.Requests;
using Talaorasan.Shared.Transaction;

namespace Talaorasan.Server.Endpoints
{
    public static class PersonEndpoints
    {
        public static IEndpointRouteBuilder MapPersonEndpoints(this IEndpointRouteBuilder builder)
        {
            var mapPerson = builder.MapGroup("/api/persons")
                                    .WithTags("Persons");
            mapPerson.MapPost("/manage", async 
                ([FromBody] PersonDto person, 
                 [FromServices] IPersonService personService, 
                 CancellationToken token) =>
            {
                var result = await personService.Manage(person, token);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });
            mapPerson.MapPost("/get-all", async 
                ([FromBody] CollectionRequest request, 
                 [FromServices] IPersonService personService, 
                 CancellationToken token) =>
            {
                var result = await personService.GetAll(request, token);
                return Results.Ok(result);
            });
            return mapPerson;
        }
    }
}
