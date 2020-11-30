using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vivero.Data;
using Vivero.Models;

namespace Vivero.Controllers
{
    public class ContactoController : Controller
    {
         private readonly ILogger<ContactoController> _logger;
         private readonly ApplicationDbContext _context;

        public ContactoController(ILogger<ContactoController> logger,
        ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var listContactos=_context.Contacto.ToList();
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contacto);
                _context.SaveChanges();
                contacto.Respuesta="Gracias " + contacto.Nombre + ", lo contactaremos lo más pronto posible.";
                return RedirectToAction("ContactoConfirmacion", contacto);
            }

            return View(contacto);
        }

        //confirmacion
        public IActionResult ContactoConfirmacion(Contacto c)
        {
            return View("ContactoConfirmacion",c);
        }
        // ObtenerContacto
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contacto.FindAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }
            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Apellido,Email,Telefono")] Contacto contacto)
        {
            if (id != contacto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contacto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contacto);
        }
        

   
        public IActionResult Delete(int? id)
        {
            var contacto = _context.Contacto.Find(id);
            _context.Contacto.Remove(contacto);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}