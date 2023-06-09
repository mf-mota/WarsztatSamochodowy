using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WarsztatSamochodowy.Data;
using WarsztatSamochodowy.Models;

namespace WarsztatSamochodowy.Controllers
{
    public class CarController : Controller
    {
        //dependency injection to use our getCars method below
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        
        }
        
        [HttpGet]
        public IActionResult CarList()
        {
            AllCars allCars = new AllCars();

            allCars.carList = _carService.GetCars().ToList();
            return View(allCars);
        }

        [HttpGet]
        public IActionResult DisplayCar(int carId)
        {
            Car car = new Car();

            car = _carService.DisplayCar(carId);
            return View(car);
        }

        [HttpDelete]
        public IActionResult DeleteCar(int carId)
        {
            bool isDeleted = _carService.DeleteCar(carId);

            if (isDeleted)
            {
                return Json(new { success = true, message = "Car deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete car." });
            }
        }

        [HttpGet]
        public IActionResult CarsCreate()
        {
            Car newCar = new Car();
            return View(newCar);
        }

        [HttpPut]
        public IActionResult EditCar(int carId, string carMake, string carModel, int ownerId, string registration)
        {
            bool isEdited = _carService.EditCar(carId, carMake, carModel, ownerId, registration);
            if (isEdited)
            {
                return Json(new { success = true, message = "Car edited successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to edit car." });
            }
        }

        [HttpPost]
        public IActionResult CreateCar(string carMake, string carModel, string arrival, string dateDone, int ownerId, string registration)
        {
            bool isCreated = _carService.CreateCar(carMake, carModel, arrival, dateDone, ownerId, registration);
            if (isCreated)
            {
                return Json(new { success = true, message = "Car created successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to create car." });
            }
        }
    }


    }

