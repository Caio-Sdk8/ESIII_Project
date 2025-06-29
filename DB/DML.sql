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
