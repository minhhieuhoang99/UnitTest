
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MVC_2.Models;
using MVC_2.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System;

namespace MVC_2.Controllers
{
    public class RookiesController : Controller
    {

        private readonly ILogger<RookiesController> _logger;
        private readonly IService _service;
        public RookiesController(ILogger<RookiesController> logger,IService service ){
            _service = service;
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                var data = _service.GetAll();
                return View(data);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message , ex);
                return StatusCode((int)HttpStatusCode.InternalServerError,ex); 
            }
        }

        public IActionResult Details(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            else return View(b);
        }

        public IActionResult Delete(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            else return View(b);
        }

        [HttpPost]
        public IActionResult Delete(PersonModel person)
        {
            _service.Delete(person.id);
            _service.SaveChanges();
            var a = this.HttpContext.Session.GetString("person");    
            HttpContext.Session.SetString("person", $"{person.id}");  
            TempData["personDelete"] = $"Person {person.id} was removed from the list successfully! ";
            return RedirectToAction("Index"); 
        }

        public IActionResult Edit(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            else return View(b);
        }

        [HttpPost]
        public IActionResult Edit(PersonModel person)
        {

            _service.Update(person);
            _service.SaveChanges();
            return View(person);
        }

        public IActionResult Create() => base.View(_service.Create());

        [HttpPost]
        public IActionResult Create(PersonModel person)
        {
            if (!ModelState.IsValid) 
            {
                ViewBag.Error = "Invalid model !!!";
                return BadRequest(ModelState);
            }
            _service.Add(person);
            _service.SaveChanges();
            // return RedirectToAction("Index"); 
            return View(person);
        }

    }
}