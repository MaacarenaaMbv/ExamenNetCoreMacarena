using ExamenNetCoreMacarena.Models;
using ExamenNetCoreMacarena.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenNetCoreMacarena.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatilla repo;

        public ZapatillasController(RepositoryZapatilla repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        public async Task<IActionResult> DetailsPartial(int? posicion, int idproducto)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            ModelPaginacionZapatillas model = await this.repo.GetDatosPaginacionZapatillasAsync(posicion.Value, idproducto);
            int numeroRegistros = model.NumeroRegistros;
            int siguiente = posicion.Value + 1;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }

            ViewData["ULTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["ZAPATILLA"] = model.Zapatilla;
            ViewData["POSICION"] = posicion;

            return View();
        }

        public async Task<IActionResult> _Imagenes(int? posicion, int idproducto)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ModelPaginacionZapatillas model = await this.repo.GetDatosPaginacionZapatillasAsync(posicion.Value, idproducto);
            int numeroRegistros = model.NumeroRegistros;
            int siguiente = posicion.Value + 1;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }

            ViewData["ULTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["ZAPATILLA"] = model.Zapatilla;
            ViewData["POSICION"] = posicion;

            return PartialView("_Imagenes", model.ImagenesZapatilla);
        }


        public async Task<IActionResult> Create()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<string> imagenes, int idZapatilla)
        {
            await this.repo.InsertarImagenes(imagenes, idZapatilla);
            /*ModelPaginacionZapatillas model = await this.repo.GetDatosPaginacionZapatillasAsync(1, idZapatilla);
            ViewData["ZAPATILLA"] = model.Zapatilla;
            return RedirectToAction("DetailsPartial", new {idproducto = idZapatilla});*/
            return View("Index");
        }
    }
}
