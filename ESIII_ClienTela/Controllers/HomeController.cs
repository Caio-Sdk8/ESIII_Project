using ESIII_ClienTela.DAO;
using ESIII_ClienTela.Fachada;
using ESIII_ClienTela.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ESIII_ClienTela.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        ClienteFachada fachada = new();
        ClienteDAO CliDao = new();
        // GET: ClientesController

        public ActionResult Index(int page = 1, string? nome = null, string? email = null, string? telefone = null)
        {
            const int itensPorPagina = 20;

            var clientesFiltrados = CliDao.BuscarClientesParams(nome, email, telefone, null);

            int totalClientes = clientesFiltrados.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalClientes / itensPorPagina);

            var clientesPaginados = clientesFiltrados
                .Skip((page - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToList();

            ViewBag.PaginaAtual = page;
            ViewBag.TotalPaginas = totalPaginas;

            ViewBag.FiltroNome = nome ?? "";
            ViewBag.FiltroEmail = email ?? "";
            ViewBag.FiltroTelefone = telefone ?? "";

            return View(clientesPaginados);
        }

        [HttpPost]
        public ActionResult AlterarSenha(int id, string senha, string confirmarSenha)
        {
            var resultado = fachada.AlterarSenha(id, senha, confirmarSenha);

            if (resultado == "ok")
                return RedirectToAction("Index");

            ModelState.AddModelError("", resultado);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
