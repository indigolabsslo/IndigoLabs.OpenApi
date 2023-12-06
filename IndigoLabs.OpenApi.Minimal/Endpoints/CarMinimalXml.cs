using IndigoLabs.OpenApi.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IndigoLabs.OpenApi.Minimal.Endpoints
{
  /// <summary>
  /// This endpoints open api documentation is configured by xml documentation.
  /// </summary>
  public class CarMinimalXml
  {
    private readonly CarRepository _carRepository;

    public CarMinimalXml(CarRepository carRepository)
    {
      _carRepository = carRepository;
    }


    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
      app.MapGet("MinimalApiXml/Car", GetAll);
      app.MapGet("MinimalApiXml/Car/{id}", GetById);
      app.MapPost("MinimalApiXml/Car", Create);
      app.MapPut("MinimalApiXml/Car/{id}", Update);
      app.MapDelete("MinimalApiXml/Car/{id}", Delete);
    }

    /// <summary>
    /// Get all cars.
    /// </summary>
    /// <remarks>All cars are good.</remarks>
    /// <returns>A list of Cars.</returns>
    /// <response code="200">All cars.</response>
    private Ok<List<Car>> GetAll()
    {
      return TypedResults.Ok(_carRepository.GetAll());
    }

    /// <summary>
    /// Get a car by Id.
    /// </summary>
    /// <remarks>The best car.</remarks>
    /// <param name="id">Id of the car.</param>
    /// <returns>A car</returns>
    /// <response code="200">Car found.</response>
    /// <response code="404">Car not found.</response>
    public Results<Ok<Car>, NotFound> GetById(int id)
    {
      var car = _carRepository.GetById(id);

      if (car == null)
        return TypedResults.NotFound();
      else
        return TypedResults.Ok(car);
    }

    /// <summary>
    /// Create a car.
    /// </summary>
    /// <remarks>What a shiny new car.</remarks>
    /// <param name="car">A car to create.</param>
    /// <returns>Id of the created car.</returns>
    /// <response code="201">Car created.</response>
    /// <response code="400">Validation error.</response>
    public Results<Created<int>, ValidationProblem> Create(RequestCarDto car)
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

    /// <summary>
    /// Update a car.
    /// </summary>
    /// <remarks>Pimp my ride.</remarks>
    /// <param name="id">Id of the car.</param>
    /// <param name="car">A car to update.</param>
    /// <returns>The updated car.</returns>
    /// <response code="200">Car found.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="404">Car to update not found.</response>
    public Results<Ok<Car>, NotFound, ValidationProblem> Update(int id, Car car)
    {
      if (_carRepository.GetById(id) == null)
        return TypedResults.NotFound();

      return TypedResults.Ok(_carRepository.Update(car));
    }

    /// <summary>
    /// Delete a car.
    /// </summary>
    /// <param name="id">Id of the car.</param>
    /// <returns>The updated car.</returns>
    /// <response code="204">Car deleted.</response>
    /// <response code="404">Car to delete not found.</response>
    public Results<NotFound, NoContent> Delete(int id)
    {
      if (_carRepository.GetById(id) == null)
        return TypedResults.NotFound();

      _carRepository.Delete(id);

      return TypedResults.NoContent();
    }
  }
}
