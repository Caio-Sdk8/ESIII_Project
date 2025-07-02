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

            const dataObj = document.getElementById('EditNascimento').value;
            const dataFormatada = dataObj.length > 10 ? dataObj.substring(0, 10) : dataObj;

            const cliente = {
                Nome: document.getElementById('EditName').value,
                Genero: document.getElementById('EditGenero').value,
                DataNascimento: dataFormatada,
                Cpf: document.getElementById('EditCPF').value,
                Email: document.getElementById('EditEmail').value,
                Senha: document.getElementById('EditSenha').value,
                Status: true,
                Ranking: 0,
                Telefones: [],
                Enderecos: [],
                Cartoes: []
            };

            document.querySelectorAll('#edit-accordionTelefones .accordion-item').forEach(function (item) {
                cliente.Telefones.push({
                    TipoTelefone_id: Number(item.querySelector('select[name="EditTipoTelefone[]"]').value),
                    Ddd: item.querySelector('input[name="EditDDD[]"]').value,
                    Numero: item.querySelector('input[name="EditPhone[]"]').value
                });
            });

            document.querySelectorAll('#edit-accordionEnderecos .accordion-item').forEach(function (item) {
                cliente.Enderecos.push({
                    TipoEndereco_id: Number(item.querySelector('select[name="EditTipoEndereco[]"]').value),
                    TipoResidencia_id: Number(item.querySelector('select[name="EditTipoResidencia[]"]').value),
                    TipoLogradouro_id: Number(item.querySelector('select[name="EditTipoLogradouro[]"]').value),
                    Logradouro: item.querySelector('input[name="EditLogradouro[]"]').value,
                    Numero: item.querySelector('input[name="EditNumero[]"]').value,
                    Bairro: item.querySelector('input[name="EditBairro[]"]').value,
                    Cep: item.querySelector('input[name="EditCEP[]"]').value,
                    Cidade_id: item.querySelector('select[name="EditCidade[]"]').value,
                    Obs: item.querySelector('textarea[name="EditObservacoes[]"]').value,
                    Apelido: cliente.Nome + " Endereco" 
                });
            });

            document.querySelectorAll('#edit-accordionCartoes .accordion-item').forEach(function (item) {
                cliente.Cartoes.push({
                    Numero: item.querySelector('input[name="EditNumeroCartao[]"]').value,
                    NomeImpresso: item.querySelector('input[name="EditNomeImpresso[]"]').value,
                    CodSeguranca: item.querySelector('input[name="EditCodSeguranca[]"]').value,
                    Band: item.querySelector('select[name="EditBandeira[]"]').value,
                    Preferencial: item.querySelector('input[name="EditPreferencial[]"]').value === 'true'
                });
            });

            console.log(JSON.stringify(cliente, null, 2));

            fetch('/Home/Alterar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(cliente)
            })
                .then(async response => {
                    if (response.ok) {
                        if (typeof mostrarModalSucesso === 'function') mostrarModalSucesso();
                        const modal = bootstrap.Modal.getInstance(document.getElementById('modalEditarCliente'));
                        if (modal) modal.hide();
                    } else {
                        const data = await response.json();
                        if (data?.Mensagens) {
                            mostrarModalErro(data.Mensagens.join('<br>'));
                        } else {
                            mostrarModalErro('Erro ao salvar alterações.');
                        }
                    }
                })
                .catch(error => {
                    console.error('Erro na requisição:', error);
                    mostrarModalErro('Erro ao salvar alterações.');
                });
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
            var id = button.getAttribute('data-id');

            // Limpar telefones existentes
            var accordionTelefones = modalEditar.querySelector("#edit-accordionTelefones");
            if (accordionTelefones) accordionTelefones.innerHTML = '';

            // Limpar listas e resetar contadores
            if (typeof adicionarTelefoneEdicao !== 'undefined') adicionarTelefoneEdicao.count = 0;
            if (typeof adicionarEnderecoEdicaoCliente !== 'undefined') adicionarEnderecoEdicaoCliente.count = 0;
            if (typeof adicionarCartaoEdicao !== 'undefined') adicionarCartaoEdicao.count = 0;

            var accordionEnderecos = modalEditar.querySelector("#edit-accordionEnderecos");
            if (accordionEnderecos) accordionEnderecos.innerHTML = '';
            var accordionCartoes = modalEditar.querySelector("#edit-accordionCartoes");
            if (accordionCartoes) accordionCartoes.innerHTML = '';

            // Buscar dados do cliente via AJAX
            $.get('/Home/ObterCliente', { id: id }, function (data) {
                // Preencher campos principais se desejar
                if (modalEditar.querySelector('#EditName')) modalEditar.querySelector('#EditName').value = data.nome;
                if (modalEditar.querySelector('#EditEmail')) modalEditar.querySelector('#EditEmail').value = data.email;

                // Preencher telefones
                if (data.telefones && Array.isArray(data.telefones)) {
                    data.telefones.forEach(function (tel) {
                        if (typeof adicionarTelefoneEdicao === 'function') {
                            adicionarTelefoneEdicao(tel.tipoTelefone_id || tel.tipoTelefoneId, tel.ddd, tel.numero);
                        }
                    });
                }

                // Preencher endereços
                if (data.enderecos && Array.isArray(data.enderecos)) {
                    data.enderecos.forEach(function (end) {
                        if (typeof adicionarEnderecoEdicaoCliente === 'function') {
                            adicionarEnderecoEdicaoCliente(
                                end.tipoEndereco_id || end.tipoEnderecoId,
                                end.tipoResidencia_id || end.tipoResidenciaId,
                                end.tipoLogradouro_id || end.tipoLogradouroId,
                                end.logradouro,
                                end.numero,
                                end.bairro,
                                end.cep,
                                end.cidade_id || end.cidadeId,
                                end.estado,
                                end.pais,
                                end.obs
                            );
                        }
                    });
                }

                // Preencher cartões
                if (data.cartoes && Array.isArray(data.cartoes)) {
                    data.cartoes.forEach(function (cartao) {
                        if (typeof adicionarCartaoEdicao === 'function') {
                            adicionarCartaoEdicao(
                                cartao.numero,
                                cartao.nomeImpresso,
                                cartao.codSeguranca,
                                cartao.band,
                                cartao.preferencial
                            );
                        }
                    });
                }
            });
        });
    }

    // Abrir modal de edição de endereços ao clicar no botão "Alterar Endereços"
    document.querySelectorAll('button[title="Alterar Endereços"]').forEach(btn => {
        btn.addEventListener('click', function () {
            const idCliente = btn.getAttribute('data-id');
            // Limpa a lista de endereços do modal de edição
            const accordion = document.querySelector('#accordionEnderecosEdicao');
            if (accordion) accordion.innerHTML = '';

            // Busca endereços do cliente pelo id
            $.get('/Home/ObterCliente', { id: idCliente }, function (data) {
                if (data.enderecos && Array.isArray(data.enderecos)) {
                    data.enderecos.forEach(function (end) {
                        // Chama a função já existente para inserir o endereço no modal de edição
                        adicionarEnderecoEdicao(
                            end.tipoEndereco_id || end.tipoEnderecoId || '', // id do tipo de endereço
                            end.tipoResidencia_id || end.tipoResidenciaId || '',
                            end.tipoLogradouro_id || end.tipoLogradouroId || '',
                            end.logradouro || '',
                            end.numero || '',
                            end.bairro || '',
                            end.cep || '',
                            end.cidade_id || end.cidadeId || '',
                            end.estado || '',
                            end.pais || '',
                            end.obs || ''
                        );
                    });
                }
            });

            // Abre o modal
            var modal = new bootstrap.Modal(document.getElementById('modalEditarEnderecos'));
            modal.show();
        });
    });

    // Modal de sucesso para edição de endereços
    const formEditarEnderecos = document.getElementById('formEditarEnderecos');
    if (formEditarEnderecos) {
        formEditarEnderecos.addEventListener('submit', function (e) {
            e.preventDefault();

            // Monta array de endereços
            const enderecos = [];
            document.querySelectorAll('#accordionEnderecosEdicao .accordion-item').forEach(function (item) {
                enderecos.push({
                    Id: parseInt(item.getAttribute('data-id'), 10), // se tiver o id
                    TipoEndereco_id: parseInt(item.querySelector('select[name="EditTipoEndereco[]"]').value, 10),
                    TipoResidencia_id: parseInt(item.querySelector('select[name="EditTipoResidencia[]"]').value, 10),
                    TipoLogradouro_id: parseInt(item.querySelector('select[name="EditTipoLogradouro[]"]').value, 10),
                    Logradouro: item.querySelector('input[name="EditLogradouro[]"]').value,
                    Numero: item.querySelector('input[name="EditNumero[]"]').value,
                    Bairro: item.querySelector('input[name="EditBairro[]"]').value,
                    Cep: item.querySelector('input[name="EditCEP[]"]').value,
                    Cidade_id: parseInt(item.querySelector('select[name="EditCidade[]"]').value, 10),
                    Obs: item.querySelector('textarea[name="EditObservacoes[]"]').value,
                    Cliente_id: idCliente 
                });
            });

            // Envia para o backend
            $.ajax({
                url: '/Home/AtualizarEnderecos',
                type: 'POST',
                data: JSON.stringify(enderecos),
                contentType: 'application/json',
                success: function () {
                    mostrarModalSucessoEnderecos();
                    const modal = bootstrap.Modal.getInstance(document.getElementById('modalEditarEnderecos'));
                    if (modal) modal.hide();
                },
                error: function () {
                    mostrarModalErro('Erro ao atualizar endereços.');
                }
            });
        });
    }

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

    // Preencher endereços no modalEditarEnderecos
    var modalEditarEnderecos = document.getElementById('modalEditarEnderecos');
    if (modalEditarEnderecos) {
        modalEditarEnderecos.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;
            var id = button.getAttribute('data-id');

            // Limpar endereços existentes
            var accordionEnderecos = modalEditarEnderecos.querySelector("#accordionEnderecosEdicao");
            if (accordionEnderecos) accordionEnderecos.innerHTML = '';

            // Buscar dados do cliente via AJAX
            $.get('/Home/ObterCliente', { id: id }, function (data) {
                // Preencher endereços
                if (data.enderecos && Array.isArray(data.enderecos)) {
                    data.enderecos.forEach(function (end) {
                        if (typeof adicionarEnderecoEdicao === 'function') {
                            adicionarEnderecoEdicao(
                                end.tipoEndereco_id || end.tipoEnderecoId,
                                end.tipoResidencia_id || end.tipoResidenciaId,
                                end.tipoLogradouro_id || end.tipoLogradouroId,
                                end.logradouro,
                                end.numero,
                                end.bairro,
                                end.cep,
                                end.cidade_id || end.cidadeId,
                                end.estado,
                                end.pais,
                                end.obs
                            );
                        }
                    });
                }
            });
        });
    }

    document.querySelectorAll('.status-switch').forEach(function(switchEl) {
        switchEl.addEventListener('change', function() {
            const id = this.getAttribute('data-id');
            const novoStatus = this.checked;

            fetch(`/Home/AlterarStatus/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ status: novoStatus })
            })
            .then(resp => {
                if (!resp.ok) {
                    mostrarModalErro('Erro ao alterar status.');
                    // Reverte o switch visualmente se falhar
                    this.checked = !novoStatus;
                }
            })
            .catch(() => {
                mostrarModalErro('Erro ao alterar status.');
                this.checked = !novoStatus;
            });
        });
    });
});

