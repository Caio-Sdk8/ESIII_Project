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

        ClienteDAO CliDao = new();
        public Fachada.Fachada fachada = new();

        public ActionResult Index(int page = 1, string? nome = null, string? email = null, string? telefone = null)
        {
            const int itensPorPagina = 20;

            var clientesFiltrados = fachada.BuscarClientesComFiltro(nome, email, telefone, null);

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
        public ActionResult AlterarSenha(int id, string senha)
        {
            var resultado = fachada.AlterarSenha(id, senha);

            if (resultado == "ok")
                return RedirectToAction("Index");

            ModelState.AddModelError("", resultado);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Salvar(ClienteModel cliente)
        {
            var response = fachada.salvar(cliente);

            if (response.Mensagens.Any(m => m != "Ok"))
            {
                return BadRequest(new
                {
                    Sucesso = false,
                    Mensagens = response.Mensagens
                });
            }

            // Sucesso
            return Ok(new
            {
                Sucesso = true,
                Mensagens = response.Mensagens,
                Entidade = response.Entidades.FirstOrDefault()
            });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult deletarCliente(int id)
        {
            var resultado = fachada.excluir(id);


            if (resultado == "OK")
                return RedirectToAction("Index");

            ModelState.AddModelError("", resultado);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult ObterCliente(int id)
        {
            // Use a Fachada para buscar o cliente
            var cliente = fachada.ObterPorId(id);
            if (cliente == null)
                return NotFound();

            return Json(cliente);
        }

        [HttpGet]
        public IActionResult ListarTiposTelefone()
        {
            var tipos = new TipoTelefoneDAO().ListarTodos();
            return Json(tipos);
        }

        [HttpGet]
        public IActionResult ListarTiposEndereco()
        {
            var tipos = new TipoEnderecoDAO().ListarTodos();
            return Json(tipos);
        }

        [HttpGet]
        public IActionResult ListarTiposResidencia()
        {
            var tipos = new TipoResidenciaDAO().ListarTodos();
            return Json(tipos);
        }

        [HttpGet]
        public IActionResult ListarTiposLogradouro()
        {
            var tipos = new TipoLogradouroDAO().ListarTodos();
            return Json(tipos);
        }

        [HttpGet]
        public IActionResult ListarCidades()
        {
            var cidades = new CidadeDAO().ListarTodos();
            return Json(cidades);
        }

        [HttpGet]
        public IActionResult ListarBandeirasCartao()
        {
            var bandeiras = System.Enum.GetValues(typeof(ESIII_ClienTela.Enums.BandeiraCartaoEnum))
                .Cast<ESIII_ClienTela.Enums.BandeiraCartaoEnum>()
                .Select(b => new { id = (int)b, nome = b.ToString() })
                .ToList();
            return Json(bandeiras);
        }

        [HttpPost]
        public IActionResult Alterar([FromBody] ClienteModel cliente)
        {
            var response = fachada.alterar(cliente);

            if (response.Mensagens.Any(m => m != "Ok"))
            {
                return BadRequest(new
                {
                    Sucesso = false,
                    Mensagens = response.Mensagens
                });
            }

            return Ok(new
            {
                Sucesso = true,
                Mensagens = response.Mensagens,
                Entidade = response.Entidades.FirstOrDefault()
            });
        }

        [HttpPost]
        public IActionResult Teste([FromBody] ClienteTeste cliente)
        {
            Console.WriteLine(cliente == null ? "Cliente NULL" : "Cliente OK");
            Console.WriteLine(cliente?.Nome);
            Console.WriteLine(cliente?.Genero);
            Console.WriteLine(cliente?.DataNascimento);
            Console.WriteLine(cliente?.Cpf);
            Console.WriteLine(cliente?.Email);
            Console.WriteLine(cliente?.Senha);
            Console.WriteLine(cliente?.Status);
            Console.WriteLine(cliente?.Ranking);
            /*if (cliente?.Telefones != null)
            {
                for (int i = 0; i < cliente.Telefones.Count; i++)
                {
                    Console.WriteLine($"Telefone[{i}]: {System.Text.Json.JsonSerializer.Serialize(cliente.Telefones[i])}");
                }
            }*/

            /*if (cliente?.Enderecos != null)
            {
                for (int i = 0; i < cliente.Enderecos.Count; i++)
                {
                    Console.WriteLine($"Endereco[{i}]: {System.Text.Json.JsonSerializer.Serialize(cliente.Enderecos[i])}");
                }
            }*/

            /*if (cliente?.Cartoes != null)
            {
                for (int i = 0; i < cliente.Cartoes.Count; i++)
                {
                    Console.WriteLine($"Cartao[{i}]: {System.Text.Json.JsonSerializer.Serialize(cliente.Cartoes[i])}");
                }
            }*/

            return Ok();
        }

        [HttpPost]
        public IActionResult AtualizarEnderecos([FromBody] List<EnderecoModel> enderecos)
        {
            foreach (var endereco in enderecos)
            {
                // Atualize cada endereço no banco
                new EnderecoDAO().Atualizar(endereco);
            }
            return Ok();
        }
    }
}
