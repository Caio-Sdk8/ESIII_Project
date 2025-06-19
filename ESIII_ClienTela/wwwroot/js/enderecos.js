let enderecoCount = 0;
let existeCobranca = false;

// Cadastro - abre modal para escolha do tipo de endereço
function abrirEscolhaTipoEndereco() {
    document.getElementById('btnAddCobranca').disabled = existeCobranca;
    var modal = new bootstrap.Modal(document.getElementById('modalTipoEndereco'));
    modal.show();
}

function adicionarEndereco(tipo) {
    // // Backend: Se desejar salvar endereço imediatamente, faça uma chamada AJAX aqui
    // // $.post('/api/enderecos', { tipo, ...dados... }).done(function(resp) { ... });

    if (tipo === 'Cobranca' && existeCobranca) return;
    if (tipo === 'Cobranca') existeCobranca = true;

    const idx = enderecoCount++;
    const tipoLabel = tipo === 'Cobranca' ? 'Cobrança' : 'Entrega';
    const html = `
    <div class="accordion-item" id="endereco-item-${idx}">
        <h2 class="accordion-header" id="headingEndereco${idx}">
            <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseEndereco${idx}" aria-expanded="false" aria-controls="collapseEndereco${idx}">
                Endereço ${tipoLabel}
            </button>
        </h2>
        <div id="collapseEndereco${idx}" class="accordion-collapse collapse show" aria-labelledby="headingEndereco${idx}" data-bs-parent="#accordionEnderecos">
            <div class="accordion-body">
                <input type="hidden" name="TipoEndereco[]" value="${tipo}" />
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group mb-2">
                            <label>Tipo de Residência<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="TipoResidencia[]" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-2">
                            <label>Tipo de Logradouro<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="TipoLogradouro[]" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-2">
                            <label>Logradouro<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="Logradouro[]" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group mb-2">
                            <label>Número<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="Numero[]" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-2">
                            <label>Bairro<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="Bairro[]" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group mb-2">
                            <label>CEP<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="CEP[]" maxlength="9" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group mb-2">
                            <label>Cidade<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="Cidade[]" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group mb-2">
                            <label>Estado<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="Estado[]" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group mb-2">
                            <label>País<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="Pais[]" />
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group mb-2">
                            <label>Observações</label>
                            <textarea class="form-control" name="Observacoes[]"></textarea>
                        </div>
                    </div>
                    <div class="col-md-1 d-flex align-items-end">
                        <button type="button" class="btn btn-link text-danger p-0" onclick="removerEndereco('endereco-item-${idx}', '${tipo}')" title="Remover Endereço">
                            <img src="/images/icon-trash.png" alt="Remover" width="22" height="22" />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>`;
    document.querySelector("#accordionEnderecos").insertAdjacentHTML('beforeend', html);
}

function removerEndereco(id, tipo) {
    // // Backend: Se desejar remover endereço imediatamente, faça uma chamada AJAX aqui
    // // $.ajax({ url: '/api/enderecos/' + id, type: 'DELETE' }).done(function(resp) { ... });

    const el = document.getElementById(id);
    if (el) el.remove();
    if (tipo === 'Cobranca') existeCobranca = false;
}

// ----------- EDIÇÃO -----------

// Abre modal para escolha do tipo de endereço na edição
function abrirEscolhaTipoEnderecoEdicao() {
    // // Backend: Se desejar buscar tipos de endereço do backend, faça uma chamada AJAX aqui
    // // $.get('/api/enderecos/tipos', function(data) { ... });

    // Cria modal temporário para edição se não existir
    if (!document.getElementById('modalTipoEnderecoEdicao')) {
        const modalHtml = `
        <div class="modal fade" id="modalTipoEnderecoEdicao" tabindex="-1" aria-labelledby="modalTipoEnderecoEdicaoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalTipoEnderecoEdicaoLabel">Tipo de Endereço</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    </div>
                    <div class="modal-body">
                        <button type="button" class="btn btn-primary w-100 mb-2" id="btnAddCobrancaEdicao" data-tipo="Cobranca">Cobrança</button>
                        <button type="button" class="btn btn-secondary w-100" id="btnAddEntregaEdicao" data-tipo="Entrega">Entrega</button>
                    </div>
                </div>
            </div>
        </div>`;
        document.body.insertAdjacentHTML('beforeend', modalHtml);

        // Adiciona listeners apenas uma vez
        document.getElementById('btnAddCobrancaEdicao').addEventListener('click', function () {
            $('#modalTipoEnderecoEdicao').modal('hide');
            setTimeout(() => adicionarEnderecoEdicao('Cobranca'), 300);
        });
        document.getElementById('btnAddEntregaEdicao').addEventListener('click', function () {
            $('#modalTipoEnderecoEdicao').modal('hide');
            setTimeout(() => adicionarEnderecoEdicao('Entrega'), 300);
        });
    }

    // Atualiza o estado dos botões antes de mostrar o modal
    const existeCobrancaEdicao = !!document.querySelector('#accordionEnderecosEdicao input[name="EditTipoEndereco[]"][value="Cobranca"]');
    document.getElementById('btnAddCobrancaEdicao').disabled = existeCobrancaEdicao;

    var modal = new bootstrap.Modal(document.getElementById('modalTipoEnderecoEdicao'));
    modal.show();
}

// Adiciona um endereço na lista dinâmica do modal de edição
function adicionarEnderecoEdicao(
    tipo = '',
    tipoResidencia = '',
    tipoLogradouro = '',
    logradouro = '',
    numero = '',
    bairro = '',
    cep = '',
    cidade = '',
    estado = '',
    pais = '',
    observacoes = ''
) {
    // // Backend: Se desejar salvar endereço de edição imediatamente, faça uma chamada AJAX aqui
    // // $.post('/api/enderecos', { tipo, ...dados... }).done(function(resp) { ... });

    if (!tipo) return; // Só adiciona se o tipo foi escolhido

    if (tipo === 'Cobranca') {
        // Garante que só exista um endereço de cobrança
        const existe = !!document.querySelector('#edit-accordionEnderecos input[name="EditTipoEndereco[]"][value="Cobranca"]');
        if (existe) return;
    }

    if (typeof adicionarEnderecoEdicao.count === 'undefined') {
        adicionarEnderecoEdicao.count = 0;
    }
    const idx = adicionarEnderecoEdicao.count++;
    const tipoLabel = tipo === 'Cobranca' ? 'Cobrança' : (tipo === 'Entrega' ? 'Entrega' : '');
    const html = `
        <div class="accordion-item" id="edit-endereco-item-${idx}">
            <h2 class="accordion-header" id="edit-headingEndereco${idx}">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#edit-collapseEndereco${idx}" aria-expanded="false" aria-controls="edit-collapseEndereco${idx}">
                    Endereço ${tipoLabel}
                </button>
            </h2>
            <div id="edit-collapseEndereco${idx}" class="accordion-collapse collapse show" aria-labelledby="edit-headingEndereco${idx}" data-bs-parent="#edit-accordionEnderecos">
                <div class="accordion-body">
                    <input type="hidden" name="EditTipoEndereco[]" value="${tipo}" />
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group mb-2">
                                <label>Tipo de Residência<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditTipoResidencia[]" value="${tipoResidencia}" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-2">
                                <label>Tipo de Logradouro<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditTipoLogradouro[]" value="${tipoLogradouro}" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-2">
                                <label>Logradouro<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditLogradouro[]" value="${logradouro}" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group mb-2">
                                <label>Número<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditNumero[]" value="${numero}" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-2">
                                <label>Bairro<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditBairro[]" value="${bairro}" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-2">
                                <label>CEP<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditCEP[]" maxlength="9" value="${cep}" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-2">
                                <label>Cidade<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditCidade[]" value="${cidade}" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group mb-2">
                                <label>Estado<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditEstado[]" value="${estado}" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-2">
                                <label>País<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditPais[]" value="${pais}" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="form-group mb-2">
                                <label>Observações</label>
                                <textarea class="form-control" name="EditObservacoes[]">${observacoes}</textarea>
                            </div>
                        </div>
                        <div class="col-md-1 d-flex align-items-end">
                            <button type="button" class="btn btn-link text-danger p-0" onclick="removerEnderecoEdicao('edit-endereco-item-${idx}')" title="Remover Endereço">
                                <img src="/images/icon-trash.png" alt="Remover" width="22" height="22" />
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;
    document.querySelector("#edit-accordionEnderecos").insertAdjacentHTML('beforeend', html);
}

// Remove um endereço do modal de edição
function removerEnderecoEdicao(id) {
    // // Backend: Se desejar remover endereço de edição imediatamente, faça uma chamada AJAX aqui
    // // $.ajax({ url: '/api/enderecos/' + id, type: 'DELETE' }).done(function(resp) { ... });

    const el = document.getElementById(id);
    if (el) el.remove();
}