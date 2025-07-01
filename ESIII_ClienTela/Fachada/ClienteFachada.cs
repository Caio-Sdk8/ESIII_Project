using ESIII_ClienTela.DAO;
using ESIII_ClienTela.Models;
using ESIII_ClienTela.Strategys;

namespace ESIII_ClienTela.Fachada
{
    public class ClienteFachada : IFachada<ClienteModel>
    {
        ValidarCamposStrategy ValidarCampos = new();
        ValidarExistenciaEmailCpfStrategy ValidarExistenciaEmailCpf = new();
        ValidarSenhaStrategy ValidarSenha = new();
        ValidarDataNascimentoStrategy ValidarDataNascimento = new();
        GerarRankingStrategy GerarRanking = new();
        CriptografarStrategy Criptografar = new();

        ValidarEnderecosStrategy ValidarEnderecos = new();

        ClienteDAO CliDao = new();

        public ClienteModel alterar(ClienteModel entidade)
        {
            throw new NotImplementedException();
        }

        public List<ClienteModel> consultar(ClienteModel entidade)
        {
            return CliDao.ListarTodos();
        }

        public string excluir(ClienteModel entidade)
        {
            throw new NotImplementedException();
        }

        public string AlterarSenha(ClienteModel cliente)
        {
            var strategy = new CriptografarStrategy();
            var resultado = strategy.Processar(cliente);

            if (resultado != "ok")
                return resultado;

            bool sucesso = CliDao.AtualizarSenha(cliente.Id, cliente.Senha);
            if (!sucesso)
                return "Erro ao atualizar senha no banco.";

            return "ok";
        }


        public string salvar(ClienteModel entidade)
        {
            string validacaoCampos = ValidarCampos.Processar(entidade);
            if(validacaoCampos != "OK")
            {
                return validacaoCampos;
            }

            string validacaoEmailCpf = ValidarExistenciaEmailCpf.Processar(entidade);
            if(validacaoEmailCpf != "Ok")
            {
                return validacaoEmailCpf;
            }

            string validacaoSenha = ValidarSenha.Processar(entidade);
            if (validacaoSenha != string.Empty)
            {
                return validacaoSenha;
            }

            string validacaoDataNasc = ValidarDataNascimento.Processar(entidade);
            if(validacaoDataNasc != "Ok")
            {
                return validacaoDataNasc;
            }

            string ranking = GerarRanking.Processar(entidade);
            if (ranking != "Erro: Não foi possível gerar um ranking único.")
            {
                entidade.Ranking = int.Parse(ranking);
            }
            else
            {
                return ranking;
            }

            Criptografar.Processar(entidade);

            try
            {
                return CliDao.Inserir(entidade).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string salvarCompleto(ClienteCompletoViewModel Entidade)
        {
            string validacaoCampos;
            foreach (var cartao in Entidade.Cartoes)
            {
                validacaoCampos = ValidarCampos.Processar(cartao);
                if (validacaoCampos != "OK")
                {
                    return validacaoCampos;
                }
            }
            foreach (var Telefone in Entidade.Telefone)
            {
                validacaoCampos = ValidarCampos.Processar(Telefone);
                if (validacaoCampos != "OK")
                {
                    return validacaoCampos;
                }
            }
            foreach (var Endereco in Entidade.Enderecos)
            {
                validacaoCampos = ValidarCampos.Processar(Endereco);
                if (validacaoCampos != "OK")
                {
                    return validacaoCampos;
                }
                ValidarEnderecos.Enderecos.Add(Endereco);
            }

            string validacaoEnd = ValidarEnderecos.Processar(Entidade.Cliente);
            if (validacaoEnd != "OK")
            {
                return validacaoEnd;
            }

            int idCli;
            try
            {
                idCli = int.Parse(salvar(Entidade.Cliente));
            } catch (Exception ex) {
                return ex.Message;
            }

            return "ok";
        }

        public List<ClienteModel> BuscarClientesComFiltro(string? nome, string? email, string? telefone, string? cpf)
        {
            return CliDao.BuscarClientesParams(nome, email, telefone, cpf);
        }
    }
}
