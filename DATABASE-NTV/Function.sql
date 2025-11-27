-- =============================
-- Function: f_get_cortometrajes()
-- =============================
select
    c.idcortometraje, 
    c.titulo, 
    c.autor,  
    c.descripcion, 
    c.urlvideo,
    c.urlportada, 
    c.duracionminutos,
    c.duracionsegundos, 
    c.aniolanzamiento, 
    c.estado,
    array_agg(cat.nombre) as categorias
from cortometraje as c
left join cortometraje_categoria as cc
    on cc.fk_idcortometraje = c.idcortometraje
left join categoria as cat
    on cat.idcategoria = cc.fk_idcategoria
group by 
    c.idcortometraje, c.titulo, c.autor, c.descripcion, 
    c.urlvideo, c.urlportada, c.duracionminutos,
    c.duracionsegundos, c.aniolanzamiento, c.estado
order by c.idcortometraje;

create or replace function f_get_cortometrajes(
    p_titulo varchar default null,
    p_duracion_minutos int default null,
    p_duracion_segundos int default null,
    p_anio int default null,
    p_estado boolean default null,
    p_categoria varchar default null,
    p_skip int default 0,
    p_take int default 10
)
returns table (
    idCortometraje int,
    titulo varchar,
    autor varchar,
    descripcion varchar,
    urlVideo varchar,
    urlPortada varchar,
    duracionMinutos int,
    duracionSegundos int,
    anioLanzamiento int,
    estado boolean,
    categorias varchar[]
)
language plpgsql
as $$
begin
    return query
        select
            c.idcortometraje, 
            c.titulo, 
            c.autor,  
            c.descripcion, 
            c.urlvideo,
            c.urlportada, 
            c.duracionminutos,
            c.duracionsegundos, 
            c.aniolanzamiento, 
            c.estado,
            array_agg(cat.nombre) as categorias
        from cortometraje as c
        left join cortometraje_categoria as cc
            on cc.fk_idcortometraje = c.idcortometraje
        left join categoria as cat
            on cat.idcategoria = cc.fk_idcategoria
        where (p_titulo is null or c.titulo ilike '%' || p_titulo || '%')
          and (p_duracion_minutos is null or c.duracionminutos >= p_duracion_minutos)
          and (p_duracion_segundos is null or c.duracionsegundos >= p_duracion_segundos)
          and (p_anio is null or c.aniolanzamiento = p_anio)
          and (p_estado is null or c.estado = p_estado)
          and (p_categoria is null or cat.nombre ilike '%' || p_categoria || '%')
        group by 
            c.idcortometraje, c.titulo, c.autor, c.descripcion, 
            c.urlvideo, c.urlportada, c.duracionminutos,
            c.duracionsegundos, c.aniolanzamiento, c.estado
        order by c.idcortometraje
        limit p_take
        offset p_skip;
end;
$$;

-- =============================
-- Function: f_get_cortometraje_by_id()
-- =============================
select
    c.idcortometraje, 
    c.titulo, 
    c.autor,  
    c.descripcion, 
    c.urlvideo,
    c.urlportada, 
    c.duracionminutos,
    c.duracionsegundos, 
    c.aniolanzamiento, 
    c.estado,
    array_agg(cat.nombre) as categorias
from cortometraje as c
left join cortometraje_categoria as cc
    on cc.fk_idcortometraje = c.idcortometraje
left join categoria as cat
    on cat.idcategoria = cc.fk_idcategoria
where c.idcortometraje = 1
group by 
    c.idcortometraje, c.titulo, c.autor, c.descripcion, 
    c.urlvideo, c.urlportada, c.duracionminutos,
    c.duracionsegundos, c.aniolanzamiento, c.estado
order by c.idcortometraje;

create or replace function f_get_cortometraje_by_id(
    p_idcortometraje int
)
returns table (
    idCortometraje int,
    titulo varchar,
    autor varchar,
    descripcion varchar,
    urlVideo varchar,
    urlPortada varchar,
    duracionMinutos int,
    duracionSegundos int,
    anioLanzamiento int,
    estado boolean,
    categorias varchar[]
)
language plpgsql
as $$
begin
    return query
        select
            c.idcortometraje, 
            c.titulo, 
            c.autor,  
            c.descripcion, 
            c.urlvideo,
            c.urlportada, 
            c.duracionminutos,
            c.duracionsegundos, 
            c.aniolanzamiento, 
            c.estado,
            array_agg(cat.nombre) as categorias
        from cortometraje as c
        left join cortometraje_categoria as cc
            on cc.fk_idcortometraje = c.idcortometraje
        left join categoria as cat
            on cat.idcategoria = cc.fk_idcategoria
        where c.idcortometraje = p_idcortometraje
        group by 
            c.idcortometraje, c.titulo, c.autor, c.descripcion, 
            c.urlvideo, c.urlportada, c.duracionminutos,
            c.duracionsegundos, c.aniolanzamiento, c.estado
        order by c.idcortometraje;
end;
$$;

-- =============================
-- Function: f_post_cortometraje()
-- =============================
create or replace function f_post_cortometraje(
    p_titulo varchar,
    p_autor varchar,
    p_descripcion text,
    p_urlvideo varchar,
    p_urlportada varchar,
    p_duracionminutos int,
    p_duracionsegundos int,
    p_aniolanzamiento int,
    p_estado boolean,
    p_categorias int[]
)
returns int
language plpgsql
as $$
declare
    v_idcortometraje int;
    v_idcategoria int;
begin 
    insert into cortometraje 
        (titulo, autor, descripcion, urlVideo, urlPortada, duracionMinutos, duracionSegundos, anioLanzamiento, estado)
    values
        (p_titulo, p_autor, p_descripcion, p_urlvideo, p_urlportada, p_duracionminutos, p_duracionsegundos, p_aniolanzamiento, p_estado)
    returning idCortometraje into v_idcortometraje;

    foreach v_idcategoria in array p_categorias
    loop
        insert into cortometraje_categoria (fk_idcortometraje, fk_idcategoria)
        values (v_idcortometraje, v_idcategoria);
    end loop;

    return v_idcortometraje;
end;
$$;

-- =============================
-- Function: f_put_cortometraje()
-- =============================
create or replace function f_put_cortometraje(
    p_idcortometraje int,
    p_titulo varchar default null,
    p_autor varchar default null,
    p_descripcion text default null,
    p_urlvideo varchar default null,
    p_urlportada varchar default null,
    p_duracionminutos int default null,
    p_duracionsegundos int default null,
    p_aniolanzamiento int default null,
    p_estado boolean default null
)
returns int
language plpgsql
as $$
begin
    update cortometraje
    set
        titulo = coalesce(p_titulo, titulo),
        autor = coalesce(p_autor, autor),
        descripcion = coalesce(p_descripcion, descripcion),
        urlVideo = coalesce(p_urlvideo, urlVideo),
        urlPortada = coalesce(p_urlportada, urlPortada),
        duracionMinutos = coalesce(p_duracionminutos, duracionMinutos),
        duracionSegundos = coalesce(p_duracionsegundos, duracionSegundos),
        anioLanzamiento = coalesce(p_aniolanzamiento, anioLanzamiento),
        estado = coalesce(p_estado, estado)
    where idCortometraje = p_idcortometraje;

    return p_idcortometraje;
end;
$$;

-- =============================
-- Function: f_put_categorias_cortometraje()
-- =============================
create or replace function f_put_categorias_cortometraje(
    p_idcortometraje int,
    p_categorias int[]
)
returns void
language plpgsql
as $$
begin
    if p_categorias is null then
        return;
    end if;

    delete from cortometraje_categoria
    where fk_idcortometraje = p_idcortometraje;

    insert into cortometraje_categoria(fk_idcortometraje, fk_idcategoria)
    select p_idcortometraje, unnest(p_categorias);
end;
$$;

-- =============================
-- Function: f_delete_cortometraje()
-- =============================
create or replace function f_delete_cortometraje(
    p_idcortometraje int
)
returns int
language plpgsql
as $$
begin
    delete from cortometraje_categoria
    where fk_idcortometraje = p_idcortometraje;

    delete from cortometraje
    where idcortometraje = p_idcortometraje;

    return 1;
end;
$$;

-- =============================
-- Function: f_put_continuar_viendo()
-- =============================
create or replace function f_put_continuar_viendo(
    p_idusuario int,
    p_idcortometraje int,
    p_minutoVisto int,
    p_segundoVisto int
)
returns void
language plpgsql
as $$
begin 
    if exists (
        select 1
        from continuar_viendo
        where fk_idusuario = p_idusuario
          and fk_idcortometraje = p_idcortometraje
    ) then 
        update continuar_viendo
        set minutoVisto = p_minutoVisto,
            segundoVisto = p_segundoVisto,
            fechaUltimaVez = now()
        where fk_idusuario = p_idusuario
          and fk_idcortometraje = p_idcortometraje;
    else
        insert into continuar_viendo 
            (fk_idusuario, fk_idcortometraje, minutoVisto, segundoVisto, fechaUltimaVez)
        values 
            (p_idusuario, p_idcortometraje, p_minutoVisto, p_segundoVisto, now());
    end if;
end;
$$;
