CREATE DATABASE ESIII_Clientela;
USE ESIII_Clientela;

CREATE TABLE Cliente (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    genero VARCHAR(20),
    dataNascimento DATE,
    cpf CHAR(14) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha char(60) NOT NULL,
    status BOOLEAN DEFAULT TRUE,
    ranking INT DEFAULT 0
);

CREATE TABLE TipoEndereco (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(100) NOT NULL
);

CREATE TABLE TipoLogradouro (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(100) NOT NULL
);

CREATE TABLE TipoResidencia (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(100) NOT NULL
);

CREATE TABLE TipoTelefone (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(100) NOT NULL
);

CREATE TABLE Pais (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
);

CREATE TABLE Estado (
    id INT AUTO_INCREMENT PRIMARY KEY,
    paisId INT NOT NULL,
    uf CHAR(2) NOT NULL,
    nome VARCHAR(100) NOT NULL,
    FOREIGN KEY (paisId) REFERENCES Pais(id) ON DELETE CASCADE
);

CREATE TABLE Cidade (
    id INT AUTO_INCREMENT PRIMARY KEY,
    estadoId INT NOT NULL,
    nome VARCHAR(100) NOT NULL,
    FOREIGN KEY (estadoId) REFERENCES Estado(id) ON DELETE CASCADE
);

CREATE TABLE Endereco (
    id INT AUTO_INCREMENT PRIMARY KEY,
    clienteId INT NOT NULL,
    cidadeId INT NOT NULL,
    tipoLogradouroId INT NOT NULL,
    tipoResidenciaId INT NOT NULL,
    tipoEnderecoId INT NOT NULL,
    apelido VARCHAR(100),
    logradouro VARCHAR(200) NOT NULL,
    numero VARCHAR(10) NOT NULL,
    bairro VARCHAR(200) NOT NULL,
    cep CHAR(9) NOT NULL,
    obs VARCHAR(300),
    FOREIGN KEY (clienteId) REFERENCES Cliente(id) ON DELETE CASCADE,
    FOREIGN KEY (cidadeId) REFERENCES Cidade(id) ON DELETE CASCADE,
    FOREIGN KEY (tipoLogradouroId) REFERENCES TipoLogradouro(id) ON DELETE CASCADE,
    FOREIGN KEY (tipoResidenciaId) REFERENCES TipoResidencia(id) ON DELETE CASCADE,
    FOREIGN KEY (tipoEnderecoId) REFERENCES TipoEndereco(id) ON DELETE CASCADE
);

CREATE TABLE Telefone (
    id INT AUTO_INCREMENT PRIMARY KEY,
    clienteId INT NOT NULL,
    ddd CHAR(2) NOT NULL,
    numero VARCHAR(10) NOT NULL,
    tipoTelefoneId INT,
    FOREIGN KEY (clienteId) REFERENCES Cliente(id) ON DELETE CASCADE,
    FOREIGN KEY (tipoTelefoneId) REFERENCES TipoTelefone(id) ON DELETE SET NULL
);

CREATE TABLE Transacao (
    id INT AUTO_INCREMENT PRIMARY KEY,
    clienteId INT NOT NULL,
    `data` DATETIME NOT NULL,
    valor DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (clienteId) REFERENCES Cliente(id) ON DELETE CASCADE
);

CREATE TABLE CartaoDeCredito (
    id INT AUTO_INCREMENT PRIMARY KEY,
    clienteId INT NOT NULL,
    numero VARCHAR(19) NOT NULL,
    nomeImpresso VARCHAR(100) NOT NULL,
    codSeguranca CHAR(3) NOT NULL,
    band VARCHAR(16) NOT NULL,
    preferencial bool,
    FOREIGN KEY (clienteId) REFERENCES Cliente(id) ON DELETE CASCADE
);