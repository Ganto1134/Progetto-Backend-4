using Microsoft.AspNetCore.Mvc;
using Polizia.DAO;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class TipiViolazioniController : Controller
    {
        private readonly TipoViolazioneDAO _dao;

        public TipiViolazioniController(TipoViolazioneDAO dao)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            var violazioni = _dao.ReadAll();
            return View(violazioni);
        }

        public IActionResult Details(int id)
        {
            var violazione = _dao.Read(id);
            if (violazione == null)
            {
                return NotFound();
            }
            return View(violazione);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoViolazione violazione)
        {
            if (ModelState.IsValid)
            {
                _dao.Create(violazione);
                return RedirectToAction(nameof(Index));
            }
            return View(violazione);
        }

        public IActionResult Edit(int id)
        {
            var violazione = _dao.Read(id);
            if (violazione == null)
            {
                return NotFound();
            }
            return View(violazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TipoViolazione violazione)
        {
            if (id != violazione.IdViolazione)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dao.Update(violazione);
                }
                catch (Exception)
                {
                    if (_dao.Read(violazione.IdViolazione) == null)
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
            return View(violazione);
        }

        public IActionResult Delete(int id)
        {
            var violazione = _dao.Read(id);
            if (violazione == null)
            {
                return NotFound();
            }
            return View(violazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminaViolazione(int id)
        {
            _dao.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
