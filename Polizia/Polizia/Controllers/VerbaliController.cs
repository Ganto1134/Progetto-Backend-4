using Microsoft.AspNetCore.Mvc;
using Polizia.DAO;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class VerbaliController : Controller
    {
        private readonly VerbaleDAO _dao;

        public VerbaliController(VerbaleDAO dao)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            var verbali = _dao.ReadAll();
            return View(verbali);
        }

        public IActionResult Details(int id)
        {
            var verbale = _dao.Read(id);
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                _dao.Create(verbale);
                return RedirectToAction(nameof(Index));
            }
            return View(verbale);
        }

        public IActionResult Edit(int id)
        {
            var verbale = _dao.Read(id);
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Verbale verbale)
        {
            if (id != verbale.IdVerbale)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dao.Update(verbale);
                }
                catch (Exception)
                {
                    if (_dao.Read(verbale.IdVerbale) == null)
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
            return View(verbale);
        }

        public IActionResult Delete(int id)
        {
            var verbale = _dao.Read(id);
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminaVerbale(int id)
        {
            _dao.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

