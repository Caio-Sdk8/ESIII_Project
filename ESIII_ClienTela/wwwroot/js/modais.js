// Modal de sucesso genérico
function mostrarModalSucesso() {
    // // Backend: Pode ser chamado após resposta de sucesso do backend em cadastro
    removerModalExistente('modalSucesso');
    const modalHtml = `
        <div class="modal fade" id="modalSucesso" tabindex="-1" aria-labelledby="modalSucessoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title" id="modalSucessoLabel">Sucesso</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    </div>
                    <div class="modal-body">
                        Operação realizada com sucesso!
                    </div>
                </div>
            </div>
        </div>`;
    document.body.insertAdjacentHTML('beforeend', modalHtml);
    var modal = new bootstrap.Modal(document.getElementById('modalSucesso'));
    modal.show();
    modal._element.addEventListener('hidden.bs.modal', function () {
        removerModalExistente('modalSucesso');
    });
}

// Modal de sucesso para edição de cliente
function mostrarModalSucessoEdicao() {
    // // Backend: Pode ser chamado após resposta de sucesso do backend em edição de cliente
    removerModalExistente('modalSucessoEdicao');
    const modalHtml = `
        <div class="modal fade" id="modalSucessoEdicao" tabindex="-1" aria-labelledby="modalSucessoEdicaoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title" id="modalSucessoEdicaoLabel">Sucesso</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    </div>
                    <div class="modal-body">
                        Alterações salvas com sucesso!
                    </div>
                </div>
            </div>
        </div>`;
    document.body.insertAdjacentHTML('beforeend', modalHtml);
    var modal = new bootstrap.Modal(document.getElementById('modalSucessoEdicao'));
    modal.show();
    var modalEditar = bootstrap.Modal.getInstance(document.getElementById('modalEditarCliente'));
    if (modalEditar) modalEditar.hide();
    modal._element.addEventListener('hidden.bs.modal', function () {
        removerModalExistente('modalSucessoEdicao');
    });
}

// Modal de sucesso para edição de endereços
function mostrarModalSucessoEnderecos() {
    // // Backend: Pode ser chamado após resposta de sucesso do backend em edição de endereços
    removerModalExistente('modalSucessoEnderecos');
    const modalHtml = `
        <div class="modal fade" id="modalSucessoEnderecos" tabindex="-1" aria-labelledby="modalSucessoEnderecosLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title" id="modalSucessoEnderecosLabel">Sucesso</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    </div>
                    <div class="modal-body">
                        Endereços salvos com sucesso!
                    </div>
                </div>
            </div>
        </div>`;
    document.body.insertAdjacentHTML('beforeend', modalHtml);
    var modal = new bootstrap.Modal(document.getElementById('modalSucessoEnderecos'));
    modal.show();
    var modalEditarEnderecos = bootstrap.Modal.getInstance(document.getElementById('modalEditarEnderecos'));
    if (modalEditarEnderecos) modalEditarEnderecos.hide();
    modal._element.addEventListener('hidden.bs.modal', function () {
        removerModalExistente('modalSucessoEnderecos');
    });
}

// Modal de erro genérico
function mostrarModalErro(msg) {
    // // Backend: Pode ser chamado após resposta de erro do backend
    removerModalExistente('modalErro');
    const modalHtml = `
        <div class="modal fade" id="modalErro" tabindex="-1" aria-labelledby="modalErroLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h5 class="modal-title" id="modalErroLabel">Atenção</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    </div>
                    <div class="modal-body">
                        ${msg}
                    </div>
                </div>
            </div>
        </div>`;
    document.body.insertAdjacentHTML('beforeend', modalHtml);
    var modal = new bootstrap.Modal(document.getElementById('modalErro'));
    modal.show();
    modal._element.addEventListener('hidden.bs.modal', function () {
        removerModalExistente('modalErro');
    });
}

// Modal de confirmação de exclusão
function mostrarModalConfirmacaoExclusao(onConfirm) {
    removerModalExistente('modalConfirmarExclusao');
    const modalHtml = `
        <div class="modal fade" id="modalConfirmarExclusao" tabindex="-1" aria-labelledby="modalConfirmarExclusaoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-warning">
                        <h5 class="modal-title">Confirmar Exclusão</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    </div>
                    <div class="modal-body">
                        Tem certeza que deseja excluir este cliente?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-danger" id="btnConfirmarExclusao">Excluir</button>
                    </div>
                </div>
            </div>
        </div>`;

    document.body.insertAdjacentHTML('beforeend', modalHtml);
    const modal = new bootstrap.Modal(document.getElementById('modalConfirmarExclusao'));
    modal.show();

    document.getElementById('btnConfirmarExclusao').onclick = function () {
        modal.hide();
        document.getElementById('modalConfirmarExclusao').addEventListener('hidden.bs.modal', function handler() {
            removerModalExistente('modalConfirmarExclusao');
            if (typeof onConfirm === 'function') onConfirm();
            document.getElementById('modalConfirmarExclusao').removeEventListener('hidden.bs.modal', handler);
        });
    };

    document.getElementById('modalConfirmarExclusao').addEventListener('hidden.bs.modal', function () {
        removerModalExistente('modalConfirmarExclusao');
    });
}

// Utilitário para remover modais duplicados
function removerModalExistente(id) {
    const modal = document.getElementById(id);
    if (modal) modal.remove();
}

