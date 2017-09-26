using System;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index()
        {
            ReferenceDBHandle dBHandle = new ReferenceDBHandle();
            ModelState.Clear();
            return View(dBHandle.GetPersonsWithCityNames());
        }

        //GET: Person Create
        public ActionResult Create()
        {
            ReferenceDBHandle dBHandle = new ReferenceDBHandle();
            ViewBag.CitySelection = dBHandle.GetCities();
            return View();
        }

        //POST Person Create
        [HttpPost]
        public ActionResult Create(PersonModel pmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ReferenceDBHandle dBHandle = new ReferenceDBHandle();
                    ViewBag.CitySelection = dBHandle.GetCities();
                    if (dBHandle.AddPerson(pmodel))
                    {
                        ViewBag.Message = "Контакт добавлен";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }
    }
}