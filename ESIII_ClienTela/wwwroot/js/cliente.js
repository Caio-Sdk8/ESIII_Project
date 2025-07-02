document.addEventListener('DOMContentLoaded', function () {
    // Habilita o botão Salvar do cadastro
    const btnSalvar = document.querySelector('#modalCadastro button[type="submit"]');
    if (btnSalvar) btnSalvar.disabled = false;

    // Exclusão de cliente com confirmação
    document.querySelectorAll('button[title="Excluir"]').forEach(btn => {
        btn.addEventListener('click', function () {
            const idCliente = btn.getAttribute('data-id');
            const tr = btn.closest('tr');
            if (!tr) return;

            mostrarModalConfirmacaoExclusao(() => {
                fetch(`/Home/deletarCliente/${idCliente}`, {
                    method: 'DELETE'
                })
                    .then(resp => {
                        if (resp.ok) {
                            tr.remove();
                            mostrarModalSucesso(); // opcional
                        } else {
                            mostrarModalErro("Erro ao excluir cliente.");
                        }
                    })
                    .catch(() => {
                        mostrarModalErro("Erro ao tentar excluir cliente.");
                    });
            });
        });
    });


    // Sucesso ao editar cliente
    const formEditar = document.querySelector('#modalEditarCliente form');
    if (formEditar) {
        formEditar.addEventListener('submit', function (e) {
            e.preventDefault();
            // // Backend: Chamada para atualizar cliente
            // // $.ajax({ url: '/clientes/' + id, type: 'PUT', data: { ...dados... } }).done(function(resp) { ... });
            if (typeof mostrarModalSucessoEdicao === 'function') {
                mostrarModalSucessoEdicao();
            }
            // Fechar o modal após salvar
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalEditarCliente'));
            if (modal) modal.hide();
        });
    }

    // Sucesso ao cadastrar cliente + validação completa
    const formCadastro = document.querySelector('#modalCadastro form');
    if (formCadastro) {
        formCadastro.addEventListener('submit', function (e) {
            e.preventDefault();

            // DADOS PESSOAIS
            const nome = document.getElementById('Name').value.trim();
            const genero = document.getElementById('Genero').value;
            const nascimento = document.getElementById('Nascimento').value;
            const cpf = document.getElementById('CPF').value.trim();

            if (!nome || !genero || !nascimento || !cpf) {
                mostrarModalErro('Preencha todos os campos obrigatórios em Dados Pessoais.');
                return;
            }

            // DADOS CADASTRAIS
            const email = document.getElementById('Email').value.trim();
            const senha = document.getElementById('Senha').value;
            const confirmarSenha = document.getElementById('ConfirmarSenha').value;
            if (!email || !senha || !confirmarSenha) {
                mostrarModalErro('Preencha todos os campos obrigatórios em Dados Cadastrais.');
                return;
            }
            if (senha !== confirmarSenha) {
                mostrarModalErro('A senha e a confirmação de senha devem ser iguais.');
                return;
            }

            // Validação do padrão da senha
            const senhaValida = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$/.test(senha);
            if (!senhaValida) {
                mostrarModalErro('A senha deve conter no mínimo 8 caracteres, uma letra maiúscula, uma letra minúscula e um caractere especial.');
                return;
            }

            // TELEFONES
            const telefones = document.querySelectorAll('#accordionTelefones .accordion-item');
            if (telefones.length === 0) {
                mostrarModalErro('Adicione pelo menos um telefone.');
                return;
            }
            for (let tel of telefones) {
                const tipo = tel.querySelector('select[name="TipoTelefone[]"]').value;
                const ddd = tel.querySelector('input[name="DDD[]"]').value.trim();
                const numero = tel.querySelector('input[name="Phone[]"]').value.trim();
                if (!tipo || !ddd || !numero) {
                    mostrarModalErro('Preencha todos os campos obrigatórios em Telefone.');
                    return;
                }
            }

            // ENDEREÇOS
            const enderecos = document.querySelectorAll('#accordionEnderecos .accordion-item');
            let temCobranca = false, temEntrega = false;
            if (enderecos.length === 0) {
                mostrarModalErro('Adicione pelo menos um endereço de cobrança e um de entrega.');
                return;
            }
            for (let end of enderecos) {
                const tipo = end.querySelector('input[name="TipoEndereco[]"]').value;
                const tipoResidencia = end.querySelector('input[name="TipoResidencia[]"]').value.trim();
                const tipoLogradouro = end.querySelector('input[name="TipoLogradouro[]"]').value.trim();
                const logradouro = end.querySelector('input[name="Logradouro[]"]').value.trim();
                const numero = end.querySelector('input[name="Numero[]"]').value.trim();
                const bairro = end.querySelector('input[name="Bairro[]"]').value.trim();
                const cep = end.querySelector('input[name="CEP[]"]').value.trim();
                const cidade = end.querySelector('input[name="Cidade[]"]').value.trim();
                const estado = end.querySelector('input[name="Estado[]"]').value.trim();
                const pais = end.querySelector('input[name="Pais[]"]').value.trim();

                if (!tipoResidencia || !tipoLogradouro || !logradouro || !numero || !bairro || !cep || !cidade || !estado || !pais) {
                    mostrarModalErro('Preencha todos os campos obrigatórios em Endereço.');
                    return;
                }
                if (tipo === 'Cobranca') temCobranca = true;
                if (tipo === 'Entrega') temEntrega = true;
            }
            if (!temCobranca || !temEntrega) {
                mostrarModalErro('Adicione pelo menos um endereço de cobrança e um de entrega.');
                return;
            }

            // CARTÕES
            const cartoesEls = document.querySelectorAll('#accordionCartoes .accordion-item');
            if (cartoesEls.length === 0) {
                mostrarModalErro('Adicione pelo menos um cartão.');
                return;
            }
            for (let cartao of cartoesEls) {
                const numero = cartao.querySelector('input[name="NumeroCartao[]"]').value.trim();
                const nomeCartao = cartao.querySelector('input[name="NomeCartao[]"]').value.trim();
                const bandeira = cartao.querySelector('select[name="Bandeira[]"]').value.trim();
                const cvv = cartao.querySelector('input[name="CVV[]"]').value.trim();
                if (!numero || !nomeCartao || !bandeira || !cvv) {
                    mostrarModalErro('Preencha todos os campos obrigatórios em Cartão.');
                    return;
                }
            }

            // Se chegou aqui, está tudo OK!
            // // Backend: Chamada para cadastrar novo cliente
            // // $.post('/clientes', { ...dados... }).done(function(resp) { ... });

            if (typeof mostrarModalSucesso === 'function') {
                mostrarModalSucesso();
            }
            // Fechar o modal após salvar
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalCadastro'));
            if (modal) modal.hide();
        });
    }

    // Preencher dados do cliente no modal de edição
    var modalEditar = document.getElementById('modalEditarCliente');
    if (modalEditar) {
        modalEditar.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;
            var nome = button.getAttribute('data-nome');
            var email = button.getAttribute('data-email');
            var telefone = button.getAttribute('data-telefone');
            var status = button.getAttribute('data-status');

            // // Backend: Chamada para buscar dados do cliente pelo id
            // // $.get('/clientes/' + id, function(data) { ...preencher campos... });

            // Preencher campos do modal
            if (modalEditar.querySelector('#EditName')) modalEditar.querySelector('#EditName').value = nome;
            if (modalEditar.querySelector('#EditEmail')) modalEditar.querySelector('#EditEmail').value = email;
            if (modalEditar.querySelector('#EditTelefone')) modalEditar.querySelector('#EditTelefone').value = telefone;
            if (modalEditar.querySelector('#EditStatus')) modalEditar.querySelector('#EditStatus').checked = status === '1';

            // Limpar telefones e endereços existentes
            var accordionTelefones = modalEditar.querySelector("#edit-accordionTelefones");
            var accordionEnderecos = modalEditar.querySelector("#edit-accordionEnderecos");
            if (accordionTelefones) accordionTelefones.innerHTML = '';
            if (accordionEnderecos) accordionEnderecos.innerHTML = '';

            // // Backend: Preencher telefones e endereços do cliente buscados
            // // Exemplo: data.telefones.forEach(t => adicionarTelefoneEdicao(t.tipo, t.ddd, t.numero));
            // // Exemplo: data.enderecos.forEach(e => adicionarEnderecoEdicao(e.tipo, ...));

            // Adicionar telefones e endereços do cliente (simulação)
            if (typeof adicionarTelefoneEdicao === 'function') {
                adicionarTelefoneEdicao();
                adicionarTelefoneEdicao();
            }
            if (typeof adicionarEnderecoEdicao === 'function') {
                adicionarEnderecoEdicao();
                adicionarEnderecoEdicao();
            }
        });
    }

    // Modal de sucesso para edição de endereços
    const formEditarEnderecos = document.getElementById('formEditarEnderecos');
    if (formEditarEnderecos) {
        formEditarEnderecos.addEventListener('submit', function (e) {
            e.preventDefault();
            // // Backend: Chamada para atualizar endereços do cliente
            // // $.ajax({ url: '/clientes/' + id + '/enderecos', type: 'PUT', data: { ...dados... } }).done(function(resp) { ... });
            if (typeof mostrarModalSucessoEnderecos === 'function') {
                mostrarModalSucessoEnderecos();
            }
            // Fechar o modal após salvar
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalEditarEnderecos'));
            if (modal) modal.hide();
        });
    }

    // Abrir modal de edição de endereços ao clicar no botão "Alterar Endereços"
    document.querySelectorAll('button[title="Alterar Endereços"]').forEach(btn => {
        btn.addEventListener('click', function () {
            // // Backend: Buscar endereços do cliente pelo id
            // // $.get('/clientes/' + id + '/enderecos', function(data) { ...preencher campos... });

            // Limpa a lista de endereços do modal de edição
            const accordion = document.querySelector('#accordionEnderecosEdicao');
            if (accordion) accordion.innerHTML = '';
            // // Backend: Preencher endereços buscados
            // // Exemplo: data.forEach(e => adicionarEnderecoEdicao(e.tipo, ...));

            // Adiciona um endereço vazio para edição (mock)
            if (typeof adicionarEnderecoEdicao === 'function') {
                adicionarEnderecoEdicao();
            }
            // Abre o modal
            var modal = new bootstrap.Modal(document.getElementById('modalEditarEnderecos'));
            modal.show();
        });
    });

    // Abrir modal de senha com o id do cliente
    document.querySelectorAll('button[title="Alterar Senha"]').forEach(btn => {
        btn.addEventListener('click', function () {
            const id = btn.getAttribute('data-id');
            document.getElementById('SenhaClienteId').value = id;
            // Limpa campos ao abrir
            document.getElementById('NovaSenha').value = '';
            document.getElementById('ConfirmarNovaSenha').value = '';
        });
    });

    // Validação e submissão do formulário de senha
    formEditarSenha.addEventListener('submit', function (e) {
        const novaSenha = document.getElementById('NovaSenha').value;
        const confirmarNovaSenha = document.getElementById('ConfirmarNovaSenha').value;

        const senhaValida = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$/.test(novaSenha);
        if (!senhaValida) {
            e.preventDefault();
            mostrarModalErro('A nova senha deve conter no mínimo 8 caracteres, uma letra maiúscula, uma letra minúscula e um caractere especial.');
            return;
        }
        if (novaSenha !== confirmarNovaSenha) {
            e.preventDefault();
            mostrarModalErro('A nova senha e a confirmação devem ser iguais.');
            return;
        }
    });
});

