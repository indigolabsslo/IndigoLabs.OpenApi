using IndigoLabs.OpenApi.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IndigoLabs.OpenApi.Minimal.Endpoints
{
  /// <summary>
  /// This endpoints open api documentation is configured by open api operation class.
  /// </summary>
  public class CarMinimalOperation
  {
    private readonly CarRepository _carRepository;

    public CarMinimalOperation(CarRepository carRepository)
    {
      _carRepository = carRepository;
    }


    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
      var group = app.MapGroup("MinimalApiOperation/Car");

      group.MapGet("/", GetAll)
          .WithOpenApi(operation =>
          {
            operation.OperationId = "MinimalOperationGetAllCars";
            operation.Summary = "Get all cars.";
            operation.Description = "All cars are good.";

            return operation;
          });

      group.MapGet("/{id}", GetById)
          .WithOpenApi(operation =>
          {
            operation.OperationId = "MinimalOperationGetCarById";
            operation.Summary = "Get a car by id.";
            operation.Description = "The best car.";

            operation.Parameters.First(x => x.Name == "id").Description = "Id of the car.";

            return operation;
          });

      group.MapPost("/", Create)
          .WithOpenApi(operation =>
          {
            operation.OperationId = "MinimalOperationCreateCar";
            operation.Summary = "Create a car.";
            operation.Description = "What a shiny new car.";

            operation.RequestBody.Description = "A car to create.";

            return operation;
          });

      group.MapPut("/{id}", Update)
          .WithOpenApi(operation =>
          {
            operation.OperationId = "MinimalOperationUpdateCar";
            operation.Summary = "Update a car.";
            operation.Description = "Pimp my ride.";

            operation.Parameters.First(x => x.Name == "id").Description = "Id of the car.";
            operation.RequestBody.Description = "A car to create.";

            return operation;
          });


      group.MapDelete("/{id}", Delete)
          .WithOpenApi(operation =>
          {
            operation.OperationId = "MinimalOperationDeleteCar";
            operation.Summary = "Delete a car.";
            operation.Description = "And it's gone.";

            operation.Parameters.First(x => x.Name == "id").Description = "Id of the car.";

            return operation;
          });

      group.WithOpenApi(operation =>
      {
        foreach (var response in operation.Responses)
        {
          response.Value.Description = $"This is a ver long decription for a response code {response.Key}";
        }
        return operation;
      });

    }

    private Ok<List<Car>> GetAll()
    {
      return TypedResults.Ok(_carRepository.GetAll());
    }

    private Results<Ok<Car>, NotFound> GetById(int id)
    {
      var car = _carRepository.GetById(id);

      if (car == null)
        return TypedResults.NotFound();
      else
        return TypedResults.Ok(car);
    }

    private Results<Created<int>, ValidationProblem> Create(RequestCarDto car)
    {
      var newCar = new Car
      {
        Model = car.Model,
        Color = car.Color,
        Price = car.Price,
      };
      var id = _carRepository.Create(newCar);
      return TypedResults.Created($"MinimalApi/Car/{id}", id);
    }

    private Results<Ok<Car>, NotFound, ValidationProblem> Update(int id, Car car)
    {
      if (_carRepository.GetById(id) == null)
        return TypedResults.NotFound();

      return TypedResults.Ok(_carRepository.Update(car));
    }

    private Results<NotFound, NoContent> Delete(int id)
    {
      if (_carRepository.GetById(id) == null)
        return TypedResults.NotFound();

      _carRepository.Delete(id);

      return TypedResults.NoContent();
    }
  }
}
