using Bogus;

namespace IndigoLabs.OpenApi.Shared
{
  public class CarRepository
  {

    private List<Car> _cars;
    private int IdCounter = 1;

    public CarRepository()
    {

      var faker = new Faker("en");

      _cars = Enumerable.Range(1, 5).Select(i =>
      new Car
      {
        Id = i,
        Model = faker.Vehicle.Model(),
        Price = faker.Random.Number(1000, 100000),
        Color = faker.Commerce.Color()
      }).ToList();

      IdCounter += _cars.Count;
    }
    public List<Car> GetAll() => _cars;
    public Car GetById(int id) => _cars.FirstOrDefault(x => x.Id == id);
    public int Create(Car car)
    {
      IdCounter += 1;
      car.Id = IdCounter;
      _cars.Add(car);
      return car.Id;
    }
    public Car Update(Car car)
    {
      var carToUpdate = _cars.First(x => x.Id == car.Id);
      carToUpdate.Model = car.Model;
      carToUpdate.Price = car.Price;
      carToUpdate.Color = car.Color;
      return carToUpdate;
    }

    public void Delete(int id) => _cars.RemoveAll(x => x.Id == id);
  }
}
