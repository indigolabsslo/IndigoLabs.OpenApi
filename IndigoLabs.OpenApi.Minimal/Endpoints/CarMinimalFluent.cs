using IndigoLabs.OpenApi.Shared;

namespace IndigoLabs.OpenApi.Minimal.Endpoints
{
  /// <summary>
  /// This endpoints open api documentation is configured with extension methods.
  /// </summary>
  public class CarMinimalFluent
  {
    private readonly CarRepository _carRepository;

    public CarMinimalFluent(CarRepository carRepository)
    {
      _carRepository = carRepository;
    }

    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
      app.MapGet("MinimalApiFluent/Car", GetAll)
          .WithName("MinimalFluentGetAllCars")
          .WithSummary("Get all cars.")
          .WithDescription("All cars are good.")
          .Produces<List<Car>>()
          .WithOpenApi();

      app.MapGet("MinimalApiFluent/Car/{id}", GetById)
          .WithName("MinimalFluentGetCarById")
          .WithSummary("Get a car by id.")
          .WithDescription("The best car.")
          .Produces<Car>()
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithOpenApi();

      app.MapPost("MinimalApiFluent/Car", Create)
          .WithName("MinimalFluentCreateCar")
          .WithSummary("Create a car.")
          .WithDescription("What a shiny new car.")
          .Produces<int>(StatusCodes.Status201Created)
          .ProducesValidationProblem()
          .WithOpenApi();

      app.MapPut("MinimalApiFluent/Car/{id}", Update)
          .WithName("MinimalFluentUpdateCar")
          .WithSummary("Update a car.")
          .WithDescription("Pimp my ride.")
          .Produces<Car>()
          .ProducesValidationProblem()
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithOpenApi();

      app.MapDelete("MinimalApiFluent/Car/{id}", Delete)
          .WithName("MinimalFluentDeleteCar")
          .WithSummary("Delete a car.")
          .WithDescription("And it's gone.")
          .Produces<Car>()
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithOpenApi();
    }

    private IResult GetAll()
    {
      return TypedResults.Ok(_carRepository.GetAll());
    }

    private IResult GetById(int id)
    {
      var car = _carRepository.GetById(id);

      if (car == null)
        return TypedResults.NotFound();
      else
        return TypedResults.Ok(car);
    }

    private IResult Create(RequestCarDto car)
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

    private IResult Update(int id, Car car)
    {
      if (_carRepository.GetById(id) == null)
        return TypedResults.NotFound();

      return TypedResults.Ok(_carRepository.Update(car));
    }

    private IResult Delete(int id)
    {
      if (_carRepository.GetById(id) == null)
        return TypedResults.NotFound();

      _carRepository.Delete(id);

      return TypedResults.NoContent();
    }
  }
}
