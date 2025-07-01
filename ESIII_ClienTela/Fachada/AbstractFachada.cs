using ESIII_ClienTela.DAO;
using ESIII_ClienTela.Models;
using ESIII_ClienTela.Strategys;

namespace ESIII_ClienTela.Fachada
{
    public class AbstractFachada
    {
        protected readonly Dictionary<string, List<object>> rns = new();
        protected readonly Dictionary<string, object> daos = new();

        protected readonly CartaoDeCreditoDAO cartaoDeCreditoDAO = new();
        protected readonly ClienteDAO clienteDAO = new();
        protected readonly EnderecoDAO enderecoDAO = new();
        protected readonly TelefoneDAO telefoneDAO = new();
        protected readonly CidadeDAO cidadeDAO = new();
        protected readonly EstadoDAO estadoDAO = new();
        protected readonly PaisDAO paisDAO = new();
        protected readonly TipoEnderecoDAO tipoEnderecoDAO = new();
        protected readonly TipoLogradouroDAO tipoLogradouroDAO = new();
        protected readonly TipoResidenciaDAO tipoResidenciaDAO = new();
        protected readonly TipoTelefoneDAO tipoTelefoneDAO = new();
        protected readonly TransacaoDAO transacaoDAO = new();

        protected readonly IStrategy<ClienteModel> criptografarStrategy = new CriptografarStrategy();
        protected readonly IStrategy<ClienteModel> gerarRankingStrategy = new GerarRankingStrategy();
        protected readonly IStrategy<CartaoDeCreditoModel> validarBandeiraStrategy = new ValidarBandeiraStrategy();
        protected readonly IStrategy<EntidadeDominio> validarCamposStrategy = new ValidarCamposStrategy();
        protected readonly IStrategy<ClienteModel> validarDataNascimentoStrategy = new ValidarDataNascimentoStrategy();
        protected readonly IStrategy<ClienteModel> validarEnderecosStrategy = new ValidarEnderecosStrategy();
        protected readonly IStrategy<EnderecoModel> validarExclusaoEndStrategy = new ValidarExclusaoEndStrategy();
        protected readonly IStrategy<ClienteModel> validarExistenciaEmailCpfStrategy = new ValidarExistenciaEmailCpfStrategy();
        protected readonly IStrategy<ClienteModel> validarSenhaStrategy = new ValidarSenhaStrategy();

        public AbstractFachada()
        {
            InicializarDAOs();
            InicializarSalvar();
            InicializarAlterar();
            InicializarExcluir();
            InicializarConsultar();
        }

        protected void InicializarDAOs()
        {
            daos[typeof(ClienteModel).Name] = clienteDAO;
            daos[typeof(CartaoDeCreditoModel).Name] = cartaoDeCreditoDAO;
            daos[typeof(EnderecoModel).Name] = enderecoDAO;
            daos[typeof(TelefoneModel).Name] = telefoneDAO;
            daos[typeof(CidadeModel).Name] = cidadeDAO;
            daos[typeof(EstadoModel).Name] = estadoDAO;
            daos[typeof(PaisModel).Name] = paisDAO;
            daos[typeof(TipoEnderecoModel).Name] = tipoEnderecoDAO;
            daos[typeof(TipoLogradouroModel).Name] = tipoLogradouroDAO;
            daos[typeof(TipoResidenciaModel).Name] = tipoResidenciaDAO;
            daos[typeof(TipoTelefoneModel).Name] = tipoTelefoneDAO;
            daos[typeof(TransacaoModel).Name] = transacaoDAO;
        }

        protected virtual void InicializarSalvar()
        {
            rns[typeof(ClienteModel).Name] = new List<object>
            {
                validarCamposStrategy,
                validarSenhaStrategy,
                criptografarStrategy,
                validarDataNascimentoStrategy,
                validarExistenciaEmailCpfStrategy,
                validarEnderecosStrategy
            };

            rns[typeof(CartaoDeCreditoModel).Name] = new List<object>
            {
                validarCamposStrategy,
                validarBandeiraStrategy
            };

            rns[typeof(EnderecoModel).Name] = new List<object>
            {
                validarCamposStrategy,
            };

            rns[typeof(TelefoneModel).Name] = new List<object>
            {
                validarCamposStrategy,
            };
        }

        protected virtual void InicializarAlterar()
        {
            rns[typeof(ClienteModel).Name] = new List<object>
            {
                validarCamposStrategy,
                validarSenhaStrategy,
                criptografarStrategy,
                validarDataNascimentoStrategy,
                validarExistenciaEmailCpfStrategy,
                validarEnderecosStrategy
            };

            rns[typeof(EnderecoModel).Name] = new List<object>
            {
                validarCamposStrategy
            };

            rns[typeof(CartaoDeCreditoModel).Name] = new List<object>
            {
                validarCamposStrategy
            };

            rns[typeof(TelefoneModel).Name] = new List<object>
            {
                validarCamposStrategy,
            };
        }

        protected virtual void InicializarExcluir()
        {
            rns[typeof(ClienteModel).Name] = new List<object>();

            rns[typeof(EnderecoModel).Name] = new List<object>
            {
                validarExclusaoEndStrategy
            };

            rns[typeof(CartaoDeCreditoModel).Name] = new List<object>();

        }

        protected virtual void InicializarConsultar()
        {
            rns[typeof(ClienteModel).Name] = new List<object>();
            rns[typeof(EnderecoModel).Name] = new List<object>();
            rns[typeof(CartaoDeCreditoModel).Name] = new List<object>();
        }

        protected List<string> ExecutarRegras<T>(T entidade) where T : EntidadeDominio
        {
            var mensagens = new List<string>();

            if (rns.TryGetValue(typeof(T).Name, out var estrategias))
            {
                foreach (var obj in estrategias)
                {
                    if (obj is IStrategy<T> strategy)
                    {
                        var msg = strategy.Processar(entidade);
                        if (!string.IsNullOrWhiteSpace(msg))
                            mensagens.Add(msg);
                    }
                    else if (obj is IStrategy<EntidadeDominio> generica)
                    {
                        var msg = generica.Processar(entidade);
                        if (!string.IsNullOrWhiteSpace(msg))
                            mensagens.Add(msg);
                    }
                }
            }

            return mensagens;
        }
    }
}
