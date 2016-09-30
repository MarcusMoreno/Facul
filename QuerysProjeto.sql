Create DataBase SAS-Estacio
go

 /***************CREATES*************************/

create table Usuario (
IdUsuario int not null primary key identity (1,1),
Nome varchar (50) not null,
Email varchar (50) not null,
Telefone varchar (13) not null,
IdPerfil int not null,
UsuarioStatus bit not null,
Senha varbinary not null
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
DescricaoPerfil varchar (50) not null,
PerfilStatus bit not null,
)

create table TipoResposta (
IdTipoResposta int not null primary key identity (1,1),
DescricaoTipoResposta varchar (50) not null, 
TipoRespostaStatus bit not null
)

create table Pergunta (
IdPergunta int not null primary key identity (1,1),
DescricaoPergunta varchar (50) not null, 
PerguntaStatus bit not null
)

create table Avaliacao (
IdAvaliacao int not null primary key identity (1,1),
DescricaoAvaliacao varchar (50) not null,
Expiracao dateTime,
Titulo varchar (50) not null,
IdUsuario int not null,
IdPerfil int not null,
AvaliacaoStatus bit not null
)

/***************CONSTRAINTS*************************/

alter table Avaliacao
add constraint fk_Avaliacao_Usuario foreign key (IdUsuario) 
references Usuario (IdUsuario)

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