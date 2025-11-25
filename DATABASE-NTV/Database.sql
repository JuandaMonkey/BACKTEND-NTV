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
-- Insert: Roles
-- =============================
insert into rol (nombre) values 
('admin'),
('usuario');

select * from public.rol;

-- =============================
-- Insert: Usuarios
-- =============================
insert into usuario (nombre, correo, contrasena, fk_idRol)
values (
    'admin', 
    'admin@correo.com', 
    'contrasena1234',
    1
);

select u.idUsuario,
       u.nombre, 
       u.correo,   
       r.nombre as rol
from usuario as u
join rol as r
on u.fk_idRol = r.idRol;

select *
from usuario as u
