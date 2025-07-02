let cartaoCount = 0;
let cartoes = [];

// Adiciona um cartão na lista dinâmica (cadastro)
function adicionarCartao() {
    // // Backend: Se desejar salvar cartão imediatamente, faça uma chamada AJAX aqui
    // // $.post('/api/cartoes', { ...dados... }).done(function(resp) { ... });

    const idx = cartaoCount++;
    // Se for o primeiro cartão, ele será o principal
    const isPrincipal = cartoes.length === 0;
    cartoes.push({ id: idx, principal: isPrincipal });

    const html = `
<div class="accordion-item" id="cartao-item-${idx}">
    <h2 class="accordion-header" id="headingCartao${idx}">
        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseCartao${idx}" aria-expanded="false" aria-controls="collapseCartao${idx}">
            Cartão ${idx + 1}
            <span class="ms-2" id="icon-principal-${idx}" style="cursor:pointer;" onclick="definirPrincipalCartao(${idx})" title="Definir como principal">
                <img src="/images/${isPrincipal ? 'icon-star-full' : 'icon-star'}.png" alt="Principal" width="20" height="20" style="vertical-align:middle;" />
            </span>
        </button>
    </h2>
    <div id="collapseCartao${idx}" class="accordion-collapse collapse show" aria-labelledby="headingCartao${idx}" data-bs-parent="#accordionCartoes">
        <div class="accordion-body">
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group mb-2">
                        <label>Número do Cartão<span class="text-danger">*</span></label>
                        <input type="text" class="form-control" name="NumeroCartao[]" maxlength="19" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group mb-2">
                        <label>Nome Impresso no Cartão<span class="text-danger">*</span></label>
                        <input type="text" class="form-control" name="NomeCartao[]" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group mb-2">
                        <label>Bandeira<span class="text-danger">*</span></label>
                        <select class="form-control" name="Bandeira[]">
                            <option value="">Selecione</option>
                            <option value="MasterCard">MasterCard</option>
                            <option value="Visa">Visa</option>
                            <option value="Hipercard">Hipercard</option>
                            <option value="American Express">American Express</option>
                            <option value="Elo">Elo</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group mb-2">
                        <label>CVV<span class="text-danger">*</span></label>
                        <input type="text" class="form-control" name="CVV[]" maxlength="4" />
                    </div>
                </div>
                <div class="col-md-1 d-flex align-items-end">
                    <button type="button" class="btn btn-link text-danger p-0" onclick="removerCartao('cartao-item-${idx}', ${idx})" title="Remover Cartão">
                        <img src="/images/icon-trash.png" alt="Remover" width="22" height="22" />
                    </button>
                </div>
            </div>
        </div>
    </div>
</div>`;
    document.querySelector("#accordionCartoes").insertAdjacentHTML('beforeend', html);
    atualizarIconesCartaoPrincipal();
}

// Remove um cartão da lista dinâmica (cadastro ou edição)
function removerCartao(id, idx) {
    // // Backend: Se desejar remover cartão imediatamente, faça uma chamada AJAX aqui
    // // $.ajax({ url: '/api/cartoes/' + id, type: 'DELETE' }).done(function(resp) { ... });

    const el = document.getElementById(id);
    if (el) el.remove();
    // Remove do array de cartões
    const eraPrincipal = cartoes.find(c => c.id === idx && c.principal);
    cartoes = cartoes.filter(c => c.id !== idx);

    // Se o removido era o principal, define o próximo mais antigo como principal
    if (eraPrincipal && cartoes.length > 0) {
        cartoes.forEach(c => c.principal = false);
        cartoes[0].principal = true;
    }
    atualizarIconesCartaoPrincipal();
}

// Define um cartão como principal
function definirPrincipalCartao(idx) {
    // // Backend: Se desejar atualizar o cartão principal no backend, faça uma chamada AJAX aqui
    // // $.ajax({ url: '/api/cartoes/' + idx + '/principal', type: 'PUT' }).done(function(resp) { ... });

    cartoes.forEach(c => c.principal = false);
    const cartao = cartoes.find(c => c.id === idx);
    if (cartao) cartao.principal = true;
    atualizarIconesCartaoPrincipal();
}

// Atualiza os ícones de cartão principal
function atualizarIconesCartaoPrincipal() {
    cartoes.forEach(c => {
        const icon = document.getElementById(`icon-principal-${c.id}`);
        if (icon) {
            icon.innerHTML = `<img src="/images/${c.principal ? 'icon-star-full' : 'icon-star'}.png" alt="Principal" width="20" height="20" style="vertical-align:middle;" />`;
        }
    });
}

// Adiciona um cartão na lista dinâmica do modal de edição
function adicionarCartaoEdicao(numero = '', nomeImpresso = '', codSeguranca = '', bandeira = '', preferencial = false) {
    if (typeof adicionarCartaoEdicao.count === 'undefined') {
        adicionarCartaoEdicao.count = 0;
    }
    const idx = adicionarCartaoEdicao.count++;
    const isPrincipal = preferencial;
    const estrelaImg = isPrincipal ? 'icon-star-full' : 'icon-star';

    const html = `
<div class="accordion-item" id="edit-cartao-item-${idx}">
    <h2 class="accordion-header" id="edit-headingCartao${idx}">
        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#edit-collapseCartao${idx}" aria-expanded="false" aria-controls="edit-collapseCartao${idx}">
            Cartão ${idx + 1}
            <span class="ms-2" id="edit-icon-principal-${idx}" style="cursor:pointer;" onclick="definirPrincipalCartaoEdicao(${idx})" title="Definir como principal">
                <img src="/images/${estrelaImg}.png" alt="Principal" width="20" height="20" style="vertical-align:middle;" />
            </span>
        </button>
    </h2>
    <div id="edit-collapseCartao${idx}" class="accordion-collapse collapse show" aria-labelledby="edit-headingCartao${idx}" data-bs-parent="#edit-accordionCartoes">
        <div class="accordion-body">
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group mb-2">
                        <label>Número do Cartão<span class="text-danger">*</span></label>
                        <input type="text" class="form-control" name="EditNumeroCartao[]" maxlength="19" value="${numero || ''}" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group mb-2">
                        <label>Nome Impresso<span class="text-danger">*</span></label>
                        <input type="text" class="form-control" name="EditNomeImpresso[]" value="${nomeImpresso || ''}" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group mb-2">
                        <label>Bandeira<span class="text-danger">*</span></label>
                        <select class="form-control" name="EditBandeira[]"></select>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group mb-2">
                        <label>Cód. Segurança<span class="text-danger">*</span></label>
                        <input type="text" class="form-control" name="EditCodSeguranca[]" maxlength="4" value="${codSeguranca || ''}" />
                    </div>
                </div>
                <input type="hidden" name="EditPreferencial[]" value="${isPrincipal ? 'true' : 'false'}" />
                <div class="col-md-1 d-flex align-items-end">
                    <button type="button" class="btn btn-link text-danger p-0" onclick="removerCartaoEdicao('edit-cartao-item-${idx}')" title="Remover Cartão">
                        <img src="/images/icon-trash.png" alt="Remover" width="22" height="22" />
                    </button>
                </div>
            </div>
        </div>
    </div>
</div>`;
    document.querySelector("#edit-accordionCartoes").insertAdjacentHTML('beforeend', html);

    // Preencher o select de bandeira via AJAX
    $.get('/Home/ListarBandeirasCartao', function (bandeiras) {
        let $select = $(`#edit-cartao-item-${idx} select[name="EditBandeira[]"]`);
        $select.append('<option value="">Selecione</option>');
        bandeiras.forEach(function (b) {
            let selected = (b.nome === bandeira || b.id == bandeira) ? 'selected' : '';
            $select.append(`<option value="${b.nome}" ${selected}>${b.nome}</option>`);
        });
    });

    // Máscaras
    $(`#edit-cartao-item-${idx} input[name="EditNumeroCartao[]"]`).mask('0000 0000 0000 0000');
    $(`#edit-cartao-item-${idx} input[name="EditCodSeguranca[]"]`).mask('0000');
}

// Define um cartão como principal no modal de edição
function definirPrincipalCartaoEdicao(idx) {
    // Remove o destaque de todos
    document.querySelectorAll('[id^="edit-icon-principal-"]').forEach(icon => {
        icon.innerHTML = `<img src="/images/icon-star.png" alt="Principal" width="20" height="20" style="vertical-align:middle;" />`;
    });
    // Destaca o selecionado
    const icon = document.getElementById(`edit-icon-principal-${idx}`);
    if (icon) {
        icon.innerHTML = `<img src="/images/icon-star-full.png" alt="Principal" width="20" height="20" style="vertical-align:middle;" />`;
    }
    // Atualiza os campos hidden
    document.querySelectorAll('input[name="EditPreferencial[]"]').forEach(inp => inp.value = 'false');
    const input = document.querySelector(`#edit-cartao-item-${idx} input[name="EditPreferencial[]"]`);
    if (input) input.value = 'true';
}

// Função para alternar estrela preferencial (edição)
function togglePreferencialEdicao(el) {
    const input = el.querySelector('input[name="EditPreferencial[]"]');
    const icon = el.querySelector('i');
    if (input.value === 'true') {
        input.value = 'false';
        icon.classList.remove('text-warning');
        icon.classList.add('text-secondary');
    } else {
        // Desmarcar todas as outras estrelas
        document.querySelectorAll('.preferencial-estrela input[name="EditPreferencial[]"]').forEach(inp => {
            inp.value = 'false';
            inp.parentElement.querySelector('i').classList.remove('text-warning');
            inp.parentElement.querySelector('i').classList.add('text-secondary');
        });
        input.value = 'true';
        icon.classList.remove('text-secondary');
        icon.classList.add('text-warning');
    }
}

// Remove um cartão do modal de edição
function removerCartaoEdicao(id, idx) {
    // // Backend: Se desejar remover cartão de edição imediatamente, faça uma chamada AJAX aqui
    // // $.ajax({ url: '/api/cartoes/' + id, type: 'DELETE' }).done(function(resp) { ... });

    const el = document.getElementById(id);
    if (el) el.remove();
    // Não manipula o array global de cartões, pois é edição local
    atualizarIconesCartaoPrincipalEdicao();
}

// Atualiza os ícones de cartão principal no modal de edição
function atualizarIconesCartaoPrincipalEdicao() {
    // Apenas visual, não há controle de array global para edição
    // Pode ser expandido conforme necessidade
}

