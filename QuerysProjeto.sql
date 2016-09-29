CREATES

create table Usuario (
IdUsuario int not null primary key identity (1,1),
Nome varchar (50) not null,
Email varchar (50) not null,
Telefone varchar (13) not null,
IdPerfil int not null
)

create table AvaliacaoPergunta (
IdAvaliacaoPergunta int not null primary key identity (1,1),
IdAvaliacao int not null,
IdPergunta int not null
)

create table PerguntaTipoResposta (
IdPerguntaTipoResposta int not null primary key identity (1,1),
IdPergunta int not null,
IdTipoResposta int not null
)

create table Perfil (
IdPerfil int not null primary key identity (1,1),
DescricaoPerfil varchar (50) not null
)

create table TipoResposta (
IdTipoReposta int not null primary key identity (1,1),
DescricaoTipoResposta varchar (50) not null
)

create table Pergunta (
IdPergunta int not null primary key identity (1,1),
DescricaoPergunta varchar (50) not null
)

create table Avaliacao (
IdAvaliacao int not null primary key identity (1,1),
DescricaoAvaliacao varchar (50) not null,
Expiracao date,
Titulo varchar (50) not null,
IdUsuario int not null,
IdPerfil int not null
)


create table AvaliacaoResposta (
IdAvaliacaoResposta int not null primary key identity (1,1),
IdAvaliacao int not null,
IdUsuario int not null,
IdPergunta int not null,
IdTipoReposta int not null
)

****************************************************************************************


ADICIONA CONSTRAINTS

alter table AvaliacaoResposta
add constraint fk_AvaliacaoResposta_Avaliacao foreign key (IdAvaliacao) 
references Avaliacao (IdAvaliacao)

alter table AvaliacaoResposta
add constraint fk_AvaliacaoResposta_Usuario foreign key (IdUsuario) 
references Usuario (IdUsuario)

alter table AvaliacaoResposta
add constraint fk_AvaliacaoResposta_Pergunta foreign key (IdPergunta) 
references Pergunta (IdPergunta)

alter table AvaliacaoResposta
add constraint fk_AvaliacaoResposta_TipoResposta foreign key (IdTipoResposta) 
references TipoResposta (IdTipoResposta)

alter table Avaliacao
add constraint fk_Avaliacao_Usuario foreign key (IdUsuario) 
references Usuario (IdUsuario)

alter table Avaliacao
add constraint fk_Avaliacao_Perfil foreign key (IdPerfil) 
references Perfil (IdPerfil)

alter table Avaliacao
add constraint fk_Avaliacao_Perfil foreign key (IdPerfil) 
references Perfil (IdPerfil)

alter table PerguntaTipoResposta
add constraint fk_PerguntaTipoResposta_Pergunta foreign key (IdPergunta) 
references Pergunta (IdPergunta)

alter table PerguntaTipoResposta
add constraint fk_PerguntaTipoResposta_TipoResposta foreign key (IdTipoResposta) 
references TipoResposta (IdTipoResposta)

alter table AvaliacaoPergunta
add constraint fk_AvaliacaoPergunta_Avaliacao foreign key (IdAvaliacao) 
references Avaliacao (IdAvaliacao)

alter table AvaliacaoPergunta
add constraint fk_AvaliacaoPergunta_Pergunta foreign key (IdPergunta) 
references Pergunta (IdPergunta)

alter table Usuario
add constraint fk_Usuario_Perfil foreign key (IdPerfil) 
references Perfil (IdPerfil)

***********************************************

Adiciona nova coluna
ALTER TABLE pergunta ADD PerguntaStatus bit NULL

Apaga coluna
ALTER TABLE Perfil DROP COLUMN UsuarioStatus


select * from Usuario