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

select * from f_get_cortometrajes(null, null, null, null, null, null, null, null);
