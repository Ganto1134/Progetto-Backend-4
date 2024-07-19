using Microsoft.AspNetCore.Mvc;
using Polizia.DAO;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class AnagraficheController : Controller
    {
        private readonly AnagraficaDAO _dao;

        public AnagraficheController(AnagraficaDAO dao)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            var anagrafiche = _dao.ReadAll();
            return View(anagrafiche);
        }

        public IActionResult Details(int id)
        {
            var anagrafica = _dao.Read(id);
            if (anagrafica == null)
            {
                return NotFound();
            }
            return View(anagrafica);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Anagrafica anagrafica)
        {
            if (ModelState.IsValid)
            {
                _dao.Create(anagrafica);
                return RedirectToAction(nameof(Index));
            }
            return View(anagrafica);
        }

        public IActionResult Edit(int id)
        {
            var anagrafica = _dao.Read(id);
            if (anagrafica == null)
            {
                return NotFound();
            }
            return View(anagrafica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Anagrafica anagrafica)
        {
            if (id != anagrafica.IdAnagrafica)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dao.Update(anagrafica);
                }
                catch (Exception)
                {
                    if (_dao.Read(anagrafica.IdAnagrafica) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(anagrafica);
        }

        public IActionResult Delete(int id)
        {
            var anagrafica = _dao.Read(id);
            if (anagrafica == null)
            {
                return NotFound();
            }
            return View(anagrafica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminaInformazione(int id)
        {
            _dao.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}