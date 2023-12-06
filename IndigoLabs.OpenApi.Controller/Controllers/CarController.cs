using IndigoLabs.OpenApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace IndigoLabs.OpenApi.Controller.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CarController : ControllerBase
  {
    private readonly ILogger<CarController> _logger;
    private readonly CarRepository _carRepository;

    public CarController(ILogger<CarController> logger, CarRepository carRepository)
    {
      _logger = logger;
      _carRepository = carRepository;
    }

    /// <summary>
    /// Get all cars.
    /// </summary>
    /// <remarks>All cars are good.</remarks>
    /// <returns>A list of Cars.</returns>
    /// <response code="200">All cars.</response>
    [HttpGet(Name = "GetAllCars")]
    [ProducesResponseType(typeof(List<Car>), StatusCodes.Status200OK)]
    public ActionResult<List<Car>> GetAll()
    {
      return Ok(_carRepository.GetAll());
    }

    /// <summary>
    /// Get a car by Id.
    /// </summary>
    /// <remarks>The best car.</remarks>
    /// <param name="id">Id of the car.</param>
    /// <returns>A car</returns>
    /// <response code="200">Car found.</response>
    /// <response code="404">Car not found.</response>
    [HttpGet("{id}", Name = "GetCarById")]
    [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Car> GetById(int id)
    {
      var car = _carRepository.GetById(id);

      if (car == null)
        return NotFound();

      return Ok(car);
    }

    /// <summary>
    /// Create a car.
    /// </summary>
    /// <remarks>What a shiny new car.</remarks>
    /// <param name="car">A car to create.</param>
    /// <returns>Id of the created car.</returns>
    /// <response code="201">Car created.</response>
    /// <response code="400">Validation error.</response>
    [HttpPost(Name = "CreateCar")]
    [ProducesResponseType(typeof(Car), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<int> Create(RequestCarDto car)
    {
      var newCar = new Car
      {
        Model = car.Model,
        Color = car.Color,
        Price = car.Price,
      };
      var id = _carRepository.Create(newCar);
      return CreatedAtAction(nameof(GetById), new { Id = id }, id);
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
    [HttpPut]
    [Route("{id}", Name = "UpdateCar")]
    [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Car> Update(int id, Car car)
    {
      if (_carRepository.GetById(id) == null)
        return NotFound();

      return Ok(_carRepository.Update(car));
    }

    /// <summary>
    /// Delete a car.
    /// </summary>
    /// <param name="id">Id of the car.</param>
    /// <returns>The updated car.</returns>
    /// <response code="204">Car deleted.</response>
    /// <response code="404">Car to delete not found.</response>
    [HttpDelete]
    [Route("{id}", Name = "DeleteCar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id)
    {
      if (_carRepository.GetById(id) == null)
        return NotFound();

      _carRepository.Delete(id);

      return NoContent();
    }
  }
}