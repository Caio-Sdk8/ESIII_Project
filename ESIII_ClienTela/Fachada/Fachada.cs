using ESIII_ClienTela.DAO;
using ESIII_ClienTela.Models;
using ESIII_ClienTela.Models.DTOs;
using ESIII_ClienTela.Strategys;
using MySqlX.XDevAPI;

namespace ESIII_ClienTela.Fachada
{
    public class Fachada : AbstractFachada, IFachada<ClienteModel>
    {
        ClienteDAO CliDao = new();
        CartaoDeCreditoDAO CartaoDao = new();
        EnderecoDAO EnderecoDAO = new();
        TelefoneDAO telefoneDAO = new();

        public ResponseDTO alterar(ClienteModel entidade)
        {
            var response = new ResponseDTO();

            var mensagens = ExecutarRegras(entidade);
            if (mensagens.Any())
            {
                response.Mensagens.AddRange(mensagens);
                return response;
            }

            foreach (CartaoDeCreditoModel cartao in entidade.Cartoes)
            {
                mensagens = ExecutarRegras(cartao);
                if (mensagens.Any())
                {
                    response.Mensagens.AddRange(mensagens);
                    return response;
                }
            }

            foreach (EnderecoModel endereco in entidade.Enderecos)
            {
                mensagens = ExecutarRegras(endereco);
                if (mensagens.Any())
                {
                    response.Mensagens.AddRange(mensagens);
                    return response;
                }
            }

            CliDao.Atualizar(entidade);

            foreach (CartaoDeCreditoModel cartao in entidade.Cartoes)
            {
                CartaoDao.Atualizar(cartao);
            }

            foreach (EnderecoModel endereco in entidade.Enderecos)
            {
                EnderecoDAO.Atualizar(endereco);
            }

            foreach (TelefoneModel telefone in entidade.Telefones)
            {
                telefoneDAO.Atualizar(telefone);
            }

            if (response.Mensagens == null)
                response.Mensagens = new List<string>();

            response.Mensagens.Add("Ok");

            if (response.Entidades == null)
                response.Entidades = new List<EntidadeDominio>();

            response.Entidades.Add(entidade);

            return response;
        }

        public List<ClienteModel> consultar(ClienteModel entidade)
        {
            return CliDao.ListarTodos();
        }

        public string excluir(int id)
        {
            try
            {
                CliDao.Remover(id);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "OK";
        }

        public string AlterarSenha(int id, string senha)
        {
            try
            {
                CliDao.AtualizarSenha(id, BCrypt.Net.BCrypt.HashPassword(senha));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "OK";
        }

        public ResponseDTO salvar(ClienteModel entidade)
        {
            var response = new ResponseDTO();

            var mensagens = ExecutarRegras(entidade);
            if (mensagens.Any())
            {
                response.Mensagens.AddRange(mensagens);
                return response;
            }

            foreach (CartaoDeCreditoModel cartao in entidade.Cartoes)
            {
                mensagens = ExecutarRegras(cartao);
                if (mensagens.Any())
                {
                    response.Mensagens.AddRange(mensagens);
                    return response;
                }
            }

            foreach (EnderecoModel endereco in entidade.Enderecos)
            {
                mensagens = ExecutarRegras(endereco);
                if (mensagens.Any())
                {
                    response.Mensagens.AddRange(mensagens);
                    return response;
                }
            }

            int idCliente = CliDao.Inserir(entidade);
            entidade.Id = idCliente;

            foreach (CartaoDeCreditoModel cartao in entidade.Cartoes)
            {
                cartao.Cliente_id = idCliente;
                CartaoDao.Inserir(cartao);
            }

            foreach (EnderecoModel endereco in entidade.Enderecos)
            {
                endereco.Cliente_id = idCliente;
                EnderecoDAO.Inserir(endereco);
            }

            foreach (TelefoneModel telefone in entidade.Telefones)
            {
                telefone.Cliente_id = idCliente;
                telefoneDAO.Inserir(telefone);
            }

            response.Mensagens.Add("Ok");
            response.Entidades.Add(entidade);

            return response;
        }

        public List<ClienteModel> BuscarClientesComFiltro(string? nome, string? email, string? telefone, string? cpf)
        {
            return CliDao.BuscarClientesParams(nome, email, telefone, cpf);
        }

        public ResponseDTO AtualizarEndereco(EnderecoModel entidade)
        {
            var response = new ResponseDTO();
            var clienteEnd = CliDao.ObterPorId(entidade.Cliente_id);
            if (clienteEnd == null)
            {
                response.Mensagens.Add("Erro: Cliente do endereço não pode ser encontrado para validação");
                return response;
            }

            //tem que validar se tem endereço dos dois tipos, dá pra fazer isso de um jeito, mas ai depende do front

            return response;
        }
        
        public ClienteModel ObterPorId(int id)
        {
            return CliDao.ObterPorId(id);
        }

    }
}
