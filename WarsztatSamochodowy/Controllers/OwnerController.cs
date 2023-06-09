using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WarsztatSamochodowy.Data;
using WarsztatSamochodowy.Models;

namespace WarsztatSamochodowy.Controllers
{
    public class OwnerController : Controller
    {
        //dependency injection to use our getCars method below
        private readonly IOwnerService _ownerService;
        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;

        }

        [HttpGet]
        public IActionResult AllOwners()
        {
            AllOwners allOwners = new AllOwners();

            allOwners.ownerList = _ownerService.GetOwners().ToList();
            return View(allOwners);
        }

        [HttpGet]
        public IActionResult CreateOwner()
        {
            CarOwner newOwner = new CarOwner();
            return View(newOwner);
        }

        [HttpPost]
        public IActionResult OwnerCreate(string firstName, string lastName, int phoneNumber)
        {
            bool isCreated = _ownerService.OwnerCreate(firstName, lastName, phoneNumber);
            if (isCreated)
            {
                return Json(new { success = true, message = "Client created successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to create client." });
            }
        }

    }
}
