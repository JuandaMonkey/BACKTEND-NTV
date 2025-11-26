-- =============================
-- Tabla: rol
-- =============================
create table rol (
    idRol serial primary key,
    nombre varchar(20) unique not null
);

-- =============================
-- Tabla: usuario
-- =============================
create table usuario (
    idUsuario serial primary key,
    nombre varchar(20) unique not null,
    correo varchar(150) unique not null,
    contrasena varchar(200) not null,
    fk_idRol int,

    constraint fk_usuario_rol
        foreign key (fk_idRol)
        references rol(idRol)
        on update cascade
        on delete set null
);

-- =============================
-- Tabla: categoria
-- =============================
create table categoria (
    idCategoria serial primary key,
    nombre varchar(100) unique not null
);

-- =============================
-- Tabla: cortometraje
-- =============================
create table cortometraje (
    idCortometraje serial primary key,
    titulo varchar(300) not null,
    autor varchar(100) not null,
    descripcion varchar(500),
    urlVideo varchar(500) not null,
    urlPortada varchar(500) not null,
    duracionMinutos int not null,
    duracionSegundos int not null,
    anioLanzamiento int not null,
    estado boolean default true
);

-- =============================
-- Tabla: cortometraje_categoria
-- =============================
create table cortometraje_categoria (
    id serial primary key,
    fk_idCortometraje int not null,
    fk_idCategoria int,

    constraint fk_cc_cortometraje
        foreign key (fk_idCortometraje)
        references cortometraje(idCortometraje)
        on delete cascade,

    constraint fk_cc_categoria
        foreign key (fk_idCategoria)
        references categoria(idCategoria)
        on delete set null
);
-- =============================

-- =============================
-- Tabla: continuar_viendo
-- =============================
create table continuar_viedno (
    idContinuar serial primary key,
    fk_idUsuario int not null,
    fk_idCortometraje int not null,
    minutoVisto int,
    segundoVisto int,
    fechaUltimaVez timestamp,

    constraint fk_cc_usuario
        foreign key (fk_idUsuario)
        references usuario(idUsuario)
        on delete cascade,

    constraint fk_cc_cortometraje
        foreign key (fk_idCortometraje)
        references cortometraje(idCortometraje)
        on delete set null
);