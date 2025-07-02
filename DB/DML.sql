INSERT INTO Pais (nome) VALUES ('Brasil');

INSERT INTO Estado (paisId, uf, nome)
VALUES
    (1, 'SP', 'São Paulo'),
    (1, 'RJ', 'Rio de Janeiro');

INSERT INTO Cidade (estadoId, nome)
VALUES
    (1, 'São Paulo'),
    (1, 'Mogi das Cruzes'),
    (2, 'Rio de Janeiro'),
    (1, 'Birigui');

INSERT INTO TipoLogradouro (tipo)
VALUES
    ('Rua'),
    ('Avenida'),
    ('Travessa'),
    ('Alameda');

INSERT INTO TipoResidencia (tipo)
VALUES
    ('Casa'),
    ('Apartamento'),
    ('Sítio'),
    ('Condomínio');

INSERT INTO TipoEndereco (tipo)
VALUES
    ('Entrega'),
    ('Cobrança');
    
INSERT INTO Cliente (nome, genero, dataNascimento, cpf, email, senha, status, ranking)
VALUES (
    'João Silva',
    'Masculino',
    '1990-05-20',
    '12345678901',
    'joao.silva@email.com',
    '$2b$10$w9N1hQnL7iG1VnPqR4R1QehX6jO3Sn9Afh8UKdoPvFxVq7y1BXE1W', -- hash simulada de senha
    TRUE,
    10
);

INSERT INTO Endereco (clienteId, cidadeId, tipoLogradouroId, tipoResidenciaId, tipoEnderecoId, apelido, logradouro, numero, bairro, cep, obs)
VALUES
(1, 1, 1, 1, 1, 'Casa', 'Rua das Flores', '123', 'Jardim Primavera', '01234-567', 'Endereço principal'),
(1, 2, 2, 2, 2, 'Apartamento', 'Av. Paulista', '456', 'Centro', '01310-000', 'Endereço para cobrança');

INSERT INTO CartaoDeCredito (clienteId, numero, nomeImpresso, codSeguranca, band, preferencial)
VALUES
(1, '1234567812345678', 'João Silva', '123', 'Visa', TRUE),
(1, '8765432187654321', 'João Silva', '321', 'Mastercard', FALSE);

INSERT INTO TipoTelefone (tipo)
VALUES
    ('Celular'),
    ('Fixo'),
    ('Comercial'),
    ('Recado');

INSERT INTO Telefone (clienteId, ddd, numero, tipoTelefoneId)
VALUES
(1, '11', '900001111', 1),
(1, '11', '900002222', 2),
(1, '12', '988887777', NULL);

select * from Cliente
