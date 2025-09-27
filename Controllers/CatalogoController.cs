using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pc2_VargasRenatoSebastian.Data;
using pc2_VargasRenatoSebastian.Models;

namespace pc2_VargasRenatoSebastian.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CatalogoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string ciudad, TipoInmueble? tipo, decimal? precioMin, decimal? precioMax, int? dormitorios, int page = 1)
        {
            const int pageSize = 6;
            var query = _context.Inmuebles.Where(i => i.Activo);

            if (!string.IsNullOrEmpty(ciudad))
                query = query.Where(i => i.Ciudad == ciudad);
            if (tipo.HasValue)
                query = query.Where(i => i.Tipo == tipo);
            if (precioMin.HasValue)
                query = query.Where(i => i.Precio >= precioMin);
            if (precioMax.HasValue)
                query = query.Where(i => i.Precio <= precioMax);
            if (dormitorios.HasValue)
                query = query.Where(i => i.Dormitorios >= dormitorios);

            // Validaciones server-side
            if (precioMin.HasValue && precioMax.HasValue && precioMin > precioMax)
                ModelState.AddModelError("", "El precio mínimo no puede ser mayor al máximo.");
            if ((precioMin ?? 0) < 0 || (precioMax ?? 0) < 0 || (dormitorios ?? 0) < 0)
                ModelState.AddModelError("", "Los valores numéricos no pueden ser negativos.");

            var total = await query.CountAsync();
            var inmuebles = await query
                .OrderBy(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Ciudad = ciudad;
            ViewBag.Tipo = tipo;
            ViewBag.PrecioMin = precioMin;
            ViewBag.PrecioMax = precioMax;
            ViewBag.Dormitorios = dormitorios;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.Ciudades = await _context.Inmuebles.Select(i => i.Ciudad).Distinct().ToListAsync();
            ViewBag.Tipos = Enum.GetValues(typeof(TipoInmueble));

            return View(inmuebles);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var inmueble = await _context.Inmuebles.FindAsync(id);
            if (inmueble == null || !inmueble.Activo)
                return NotFound();
            return View(inmueble);
        }
    }
}
