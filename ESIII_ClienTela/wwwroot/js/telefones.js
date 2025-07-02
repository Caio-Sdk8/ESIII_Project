let telefoneCount = 0;

// Adiciona um telefone na lista dinâmica (cadastro)
function adicionarTelefone() {
    // // Backend: Se desejar salvar telefone imediatamente, faça uma chamada AJAX aqui
    // // $.post('/api/telefones', { tipo, ddd, numero, ... }).done(function(resp) { ... });

    const idx = telefoneCount++;
    const html = `
    <div class="accordion-item" id="telefone-item-${idx}">
        <h2 class="accordion-header" id="headingTelefone${idx}">
            <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTelefone${idx}" aria-expanded="false" aria-controls="collapseTelefone${idx}">
                Telefone ${idx + 1}
            </button>
        </h2>
        <div id="collapseTelefone${idx}" class="accordion-collapse collapse show" aria-labelledby="headingTelefone${idx}" data-bs-parent="#accordionTelefones">
            <div class="accordion-body">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group mb-2">
                            <label>Tipo de Telefone<span class="text-danger">*</span></label>
                            <select class="form-control" name="TipoTelefone[]">
                                <option value="">Selecione</option>
                                <option value="Celular">Celular</option>
                                <option value="Residencial">Residencial</option>
                                <option value="Comercial">Comercial</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group mb-2">
                            <label>DDD<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="DDD[]" maxlength="2" />
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form-group mb-2">
                            <label>Número do Telefone<span class="text-danger">*</span></label>
                            <input type="text" class="form-control" name="Phone[]" />
                        </div>
                    </div>
                    <div class="col-md-1 d-flex align-items-end">
                        <button type="button" class="btn btn-link text-danger p-0" onclick="removerTelefone('telefone-item-${idx}')" title="Remover Telefone">
                            <img src="/images/icon-trash.png" alt="Remover" width="22" height="10" />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>`;
    document.querySelector("#accordionTelefones").insertAdjacentHTML('beforeend', html);
    $('input[name="Phone[]"]').mask('00000-0000');
    $('input[name="DDD[]"]').mask('00');
}

// Remove um telefone da lista dinâmica (cadastro ou edição)
function removerTelefone(id) {
    // // Backend: Se desejar remover telefone imediatamente, faça uma chamada AJAX aqui
    // // $.ajax({ url: '/api/telefones/' + id, type: 'DELETE' }).done(function(resp) { ... });

    const el = document.getElementById(id);
    if (el) el.remove();
}

// Adiciona um telefone na lista dinâmica do modal de edição
function adicionarTelefoneEdicao(tipoId = '', ddd = '', numero = '') {
    if (typeof adicionarTelefoneEdicao.count === 'undefined') {
        adicionarTelefoneEdicao.count = 0;
    }
    const idx = adicionarTelefoneEdicao.count++;
    const html = `
        <div class="accordion-item" id="edit-telefone-item-${idx}">
            <h2 class="accordion-header" id="edit-headingTelefone${idx}">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#edit-collapseTelefone${idx}" aria-expanded="false" aria-controls="edit-collapseTelefone${idx}">
                    Telefone ${idx + 1}
                </button>
            </h2>
            <div id="edit-collapseTelefone${idx}" class="accordion-collapse collapse show" aria-labelledby="edit-headingTelefone${idx}" data-bs-parent="#edit-accordionTelefones">
                <div class="accordion-body">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group mb-2">
                                <label>Tipo de Telefone<span class="text-danger">*</span></label>
                                <select class="form-control" name="EditTipoTelefone[]"></select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group mb-2">
                                <label>DDD<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditDDD[]" maxlength="2" value="${ddd || ''}" />
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group mb-2">
                                <label>Número do Telefone<span class="text-danger">*</span></label>
                                <input type="text" class="form-control" name="EditPhone[]" value="${numero || ''}" />
                            </div>
                        </div>
                        <div class="col-md-1 d-flex align-items-end">
                            <button type="button" class="btn btn-link text-danger p-0" onclick="removerTelefone('edit-telefone-item-${idx}')" title="Remover Telefone">
                                <img src="/images/icon-trash.png" alt="Remover" width="22" height="22" />
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;
    document.querySelector("#edit-accordionTelefones").insertAdjacentHTML('beforeend', html);

    // Preencher o select dinamicamente
    $.get('/Home/ListarTiposTelefone', function (tipos) {
        let $select = $(`#edit-telefone-item-${idx} select[name="EditTipoTelefone[]"]`);
        $select.append('<option value="">Selecione</option>');
        tipos.forEach(function (tipo) {
            let selected = tipo.id == tipoId ? 'selected' : '';
            $select.append(`<option value="${tipo.id}" ${selected}>${tipo.tipo}</option>`);
        });
    });

    // Máscaras corretas
    $(`#edit-telefone-item-${idx} input[name="EditPhone[]"]`).mask('00000-0000');
    $(`#edit-telefone-item-${idx} input[name="EditDDD[]"]`).mask('00');
}