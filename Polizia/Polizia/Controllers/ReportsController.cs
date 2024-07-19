using Microsoft.AspNetCore.Mvc;
using Polizia.DAO;
using Polizia.Models;
using System.Linq;

namespace Polizia.Controllers
{
    public class ReportsController : Controller
    {
        private readonly VerbaleDAO _verbaleDao;

        public ReportsController(VerbaleDAO verbaleDao)
        {
            _verbaleDao = verbaleDao;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerbaliPerTrasgressore()
        {
            var report = _verbaleDao.GetVerbaliPerTrasgressore();
            return View(report);
        }

        public IActionResult PuntiDecurtatiPerTrasgressore()
        {
            var report = _verbaleDao.GetPuntiDecurtatiPerTrasgressore();
            return View(report);
        }

        public IActionResult ViolazioniConPiuDiDieciPunti()
        {
            var report = _verbaleDao.GetViolazioniConPiuDiDieciPunti();
            return View(report);
        }

        public IActionResult ViolazioniConImportoMaggioreDiQuattrocento()
        {
            var report = _verbaleDao.GetViolazioniConImportoMaggioreDiQuattrocento();
            return View(report);
        }
    }
}
