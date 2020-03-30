DROP TABLE SolAdquisicionDet
DROP TABLE SolAdquisicion
DROP TABLE ProveedorArticulo
DROP TABLE Empleado
DROP TABLE Proveedor
DROP TABLE Articulo
DROP TABLE UnidadMedida
DROP TABLE Estado
DROP TABLE Documento
DROP TABLE Area
GO

create table GD_Area
(
	CodArea int identity primary key
	,DescArea varchar(25)
)

create table GD_Documento
(
	CodDocumento int identity primary key
	,DescDocumento varchar(25)
)

create table GD_Empleado
(
	CodEmpleado int identity primary key
	,CodDocumento int not null
	,NroDocumento varchar(25)
	,ApePaterno varchar(50)
	,ApeMaterno varchar(50)
	,Nombres varchar(50)
	,CodArea int not null
	,constraint fk_Documento foreign key (CodDocumento) references GD_Documento (CodDocumento)
	,constraint fk_Area foreign key (CodArea) references GD_Area (CodArea)
)

create unique index I_Empleado_CodDocumento on GD_Empleado(NroDocumento);

create table GD_UnidadMedida
(
	CodUniMedida char(2) primary key
	,DescUniMedida varchar(25)
)

create table GD_Proveedor
(
	CodProveedor int identity primary key
	,RazonSocial varchar(50)
	,Ruc nvarchar(11)
	,Direccion varchar(100)
)

create table GD_Articulo
(
	CodArticulo int identity primary key
	,DescArticulo varchar(25)
	,CodUniMedida char(2) not null
	,Modelo varchar(50)
	,Marca varchar(50)
	,Observacion varchar(200)
	,constraint fk_UnidadMedida foreign key (CodUniMedida) references GD_UnidadMedida (CodUniMedida)
)

create table GD_ProveedorArticulo
(
	CodProveedor int
	,CodArticulo int
	,constraint fk_Proveedor foreign key (CodProveedor) references GD_Proveedor (CodProveedor)
	,constraint fk_Articulo foreign key (CodArticulo) references GD_Articulo (CodArticulo)
	,primary key(CodProveedor ,CodArticulo)
)

create table GD_Estado
(
	CodEstado Char(1) primary key
	,DescEstado varchar(25)
)

create table GD_SolAdquisicion
(
	CodSolAdquisicion int identity primary key
	,FechaEmision datetime not null
	,CodEstado char(1) not null
	,CodEmpleado int not null
	,CodSolicitante int not null
	,Observacion varchar(200)
	,NroInforme varchar(25)
	,constraint fk_Estado foreign key (CodEstado) references GD_Estado (CodEstado)
	,constraint fk_Empleado foreign key (CodEmpleado) references GD_Empleado (CodEmpleado)
	,constraint fk_Solicitante foreign key (CodSolicitante) references GD_Empleado (CodEmpleado)
)

create table GD_SolAdquisicionDet
(
	CodSolAdquisicionDet int identity
	,CodSolAdquisicion int
	,Cantidad int
	,CodProveedor int not null
	,CodArticulo int not null
	,constraint fk_SolAdquisicion foreign key (CodSolAdquisicion) references GD_SolAdquisicion (CodSolAdquisicion)
	,constraint fk_ProveedorArticulo foreign key (CodProveedor ,CodArticulo) references GD_ProveedorArticulo (CodProveedor ,CodArticulo)
	,primary key (CodSolAdquisicionDet ,CodSolAdquisicion)
)

CREATE TABLE GD_SolCotizacion
(
	CodSolCotizacion int identity(1,1) primary key,
	FechaCotizacion datetime,
	CodSolAdquisicion int,
	CodProveedor int,
	CodEstado char(1)
)
go

ALTER TABLE GD_SolCotizacion
ADD FOREIGN KEY (CodSolAdquisicion)
REFERENCES GD_SolAdquisicion(CodSolAdquisicion);
go

create TABLE GD_SolCotizacionDet
(
	CodSolCotizacion int,
	CodArticulo int,
	Cantidad int
	primary key(CodSolCotizacion,CodArticulo)	
)
go

ALTER TABLE GD_SolCotizacionDet
ADD FOREIGN KEY (CodSolCotizacion)
REFERENCES GD_SolCotizacion(CodSolCotizacion);
go

ALTER TABLE GD_SolCotizacionDet
ADD FOREIGN KEY (CodArticulo)
REFERENCES GD_Articulo(CodArticulo);
go

CREATE PROCEDURE sp_ListaCotizaciones
as
	SELECT CodSolCotizacion, FechaCotizacion,CodSolAdquisicion, 
	A.CodProveedor, P.RazonSocial, A.CodEstado, E.DescEstado
	FROM GD_SolCotizacion as A inner join GD_Proveedor as P on A.CodProveedor = P.CodProveedor
	inner join GD_Estado as E on A.CodEstado = E.CodEstado
	Order BY FechaCotizacion asc;
go

--update SolAdquisicion set CodEstado = 'P';
--go

CREATE PROCEDURE sp_BuscarArticulos_SolAdquisicion
@CodSolAdquisicion int
as
	--Select S.CodArticulo, S.CodProveedor, A.DescArticulo, A.Marca, A.CodUniMedida, S.Cantidad , U.DescUniMedida, P.RazonSocial
	--from 
	--SolAdquisicionDet as S inner join Articulo as A on S.CodArticulo = A.CodArticulo
	--inner join UnidadMedida as U on A.CodUniMedida = U.CodUniMedida
	--inner join Proveedor as P on S.CodProveedor = P.CodProveedor
	Select S.CodArticulo, S.CodProveedor, A.DescArticulo, A.Marca, A.CodUniMedida, S.Cantidad , U.DescUniMedida, P.RazonSocial
	from  GD_SolAdquisicion as SA inner join GD_SolAdquisicionDet as S on SA.CodSolAdquisicion = S.CodSolAdquisicion
	inner join GD_Articulo as A on S.CodArticulo = A.CodArticulo
	inner join GD_UnidadMedida as U on A.CodUniMedida = U.CodUniMedida
	inner join GD_Proveedor as P on S.CodProveedor = P.CodProveedor 	 
	where s.CodSolAdquisicion = @CodSolAdquisicion and SA.CodEstado = 'P'
go

CREATE PROCEDURE sp_GrabarCabSolCotizacion
@CodSolAdquisicion int,
@CodProveedor int
as
	insert into GD_SolCotizacion values(getdate(),@CodSolAdquisicion,@CodProveedor,'P');
	select @@IDENTITY as CodSolCotizacion;
go

--sp_GrabarCabSolCotizacion 1,1
--go


CREATE PROCEDURE sp_GrabarDetSolCotizacion
@CodSolCotizacion int,
@CodSolAdquisicion int,
@CodProveedor int
as
	insert into GD_SolCotizacionDet 
	Select @CodSolCotizacion,S.CodArticulo,S.Cantidad
	from GD_SolAdquisicionDet as S 
	where S.CodSolAdquisicion = @CodSolAdquisicion and S.CodProveedor = @CodProveedor;

	update GD_SolAdquisicion set CodEstado = 'E' where CodSolAdquisicion = @CodSolAdquisicion;

	Select '1' as Resultado;
go

--sp_GrabarDetSolCotizacion 21,1,3
--GO
	
CREATE PROCEDURE sp_ConsultarCotizacion
@CodSolCotizacion int
as
	select SO.CodSolCotizacion, SO.CodSolAdquisicion, P.RazonSocial, SO.CodProveedor,
	SD.CodArticulo, A.DescArticulo, A.Marca, SD.Cantidad, U.DescUniMedida
	from GD_SolCotizacion as SO inner join GD_SolCotizacionDet as SD on SO.CodSolCotizacion = SD.CodSolCotizacion
	inner join GD_Proveedor as P on SO.CodProveedor= P.CodProveedor
	inner join GD_Articulo as A on SD.CodArticulo = A.CodArticulo
	inner join GD_UnidadMedida as U on A.CodUniMedida = U.CodUniMedida
	where SO.CodSolCotizacion = @CodSolCotizacion;
go

--sp_ConsultarCotizacion 49
--go



CREATE PROCEDURE sp_EliminarArticuloSolCotizacion
@CodSolCotizacion int,
@CodArticulo int
as
	delete from GD_SolCotizacionDet  
	where CodSolCotizacion = @CodSolCotizacion and CodArticulo = @CodArticulo;
	select '1' as Respuesta;
go

CREATE PROCEDURE sp_EliminarCotizacion
@CodSolCotizacion int
as
    delete from GD_SolCotizacionDet
	WHERE CodSolCotizacion = @CodSolCotizacion;

	DELETE FROM GD_SolCotizacion
	WHERE CodSolCotizacion = @CodSolCotizacion;
go


CREATE PROCEDURE sp_ActualizarCotizacion
@CodSolCotizacion int
as
    update GD_SolCotizacion set CodEstado = 'E'
	WHERE CodSolCotizacion = @CodSolCotizacion;
go


CREATE PROCEDURE sp_ObetenerEstadoCotizacion
@CodSolCotizacion int
as
    select top 1 CodEstado from  GD_SolCotizacion 
	WHERE CodSolCotizacion = @CodSolCotizacion;
go

insert into GD_Documento (DescDocumento) values ('DNI')

insert into GD_Area (DescArea) values ('MANTENIMIENTO')
insert into GD_Area (DescArea) values ('SISTEMAS')
insert into GD_Area (DescArea) values ('FACTURACION')
insert into GD_Area (DescArea) values ('PUBLICIDAD')
insert into GD_Area (DescArea) values ('PROFESORES')
insert into GD_Area (DescArea) values ('ADMINISTRACION')

insert into GD_Empleado (CodDocumento ,NroDocumento ,ApePaterno ,ApeMaterno ,Nombres ,CodArea)
values (1 ,'54874221' ,'ADMINISTRADOR' ,'ADMINISTRADOR' ,'ADMINISTRADOR' ,6)
insert into GD_Empleado (CodDocumento ,NroDocumento ,ApePaterno ,ApeMaterno ,Nombres ,CodArea)
values (1 ,'40587456' ,'CIPRIANO' ,'TORRES' ,'MARCELO' ,1)
insert into GD_Empleado (CodDocumento ,NroDocumento ,ApePaterno ,ApeMaterno ,Nombres ,CodArea)
values (1 ,'41258745' ,'EZETA' ,'TICONA' ,'YERICO' ,2)
insert into GD_Empleado (CodDocumento ,NroDocumento ,ApePaterno ,ApeMaterno ,Nombres ,CodArea)
values (1 ,'40256981' ,'JIMENEZ' ,'VARGAS' ,'PEDRO' ,3)
insert into GD_Empleado (CodDocumento ,NroDocumento ,ApePaterno ,ApeMaterno ,Nombres ,CodArea)
values (1 ,'43681097' ,'PADILLA' ,'FUERTES' ,'EDGAR' ,4)
insert into GD_Empleado (CodDocumento ,NroDocumento ,ApePaterno ,ApeMaterno ,Nombres ,CodArea)
values (1 ,'46281607' ,'QUIROZ' ,'CISNEROS' ,'OSCAR' ,5)
insert into GD_Empleado (CodDocumento ,NroDocumento ,ApePaterno ,ApeMaterno ,Nombres ,CodArea)
values (1 ,'42951358' ,'TAIPE' ,'QUIROZ' ,'ELSA' ,6)

insert into GD_Estado (CodEstado ,DescEstado) values ('E' ,'EMITIDO')
insert into GD_Estado values('P', 'PENDIENTE');

insert into GD_UnidadMedida (CodUniMedida ,DescUniMedida) values ('UN' ,'UNIDAD')
insert into GD_UnidadMedida (CodUniMedida ,DescUniMedida) values ('MT' ,'METRO')
insert into GD_UnidadMedida (CodUniMedida ,DescUniMedida) values ('LT' ,'LITRO')

insert into GD_Articulo (DescArticulo ,CodUniMedida ,Marca) values ('CANDADO' ,'UN' ,'FORTE')
insert into GD_Articulo (DescArticulo ,CodUniMedida ,Marca) values ('CABLE TW 12 AWG' ,'MT' ,'INDECO')
insert into GD_Articulo (DescArticulo ,CodUniMedida ,Marca) values ('PINTURA BLANCA' ,'LT' ,'CPP')

insert into GD_Proveedor (RazonSocial ,Ruc) values ('FERRETERIA JUANITA SAC' ,'10547845215')
insert into GD_Proveedor (RazonSocial ,Ruc) values ('FERRETERIA OLIVAR SAC' ,'11548214578')
insert into GD_Proveedor (RazonSocial ,Ruc) values ('FERRETERIA CATERPILLAR SAC' ,'14563258741')

insert into GD_ProveedorArticulo (CodProveedor ,CodArticulo) values (1 ,1)
insert into GD_ProveedorArticulo (CodProveedor ,CodArticulo) values (2 ,2)
insert into GD_ProveedorArticulo (CodProveedor ,CodArticulo) values (3 ,3)

insert into GD_SolAdquisicion (FechaEmision, CodEstado, CodEmpleado, CodSolicitante) VALUES ('04/08/2016', 'E', 1, 2)
insert into GD_SolAdquisicion (FechaEmision, CodEstado, CodEmpleado, CodSolicitante) VALUES ('04/08/2016', 'E', 1, 3)
insert into GD_SolAdquisicion (FechaEmision, CodEstado, CodEmpleado, CodSolicitante) VALUES ('04/08/2016', 'E', 1, 4)
insert into GD_SolAdquisicion (FechaEmision, CodEstado, CodEmpleado, CodSolicitante) VALUES ('04/08/2016', 'E', 1, 5)
insert into GD_SolAdquisicion (FechaEmision, CodEstado, CodEmpleado, CodSolicitante) VALUES ('04/08/2016', 'E', 1, 6)
insert into GD_SolAdquisicion (FechaEmision, CodEstado, CodEmpleado, CodSolicitante) VALUES ('04/08/2016', 'E', 1, 7)

insert into GD_SolAdquisicionDet (CodSolAdquisicion ,Cantidad ,CodProveedor ,CodArticulo) values (1 ,3 ,1 ,1)
insert into GD_SolAdquisicionDet (CodSolAdquisicion ,Cantidad ,CodProveedor ,CodArticulo) values (1 ,100 ,2 ,2)
insert into GD_SolAdquisicionDet (CodSolAdquisicion ,Cantidad ,CodProveedor ,CodArticulo) values (1 ,1 ,3 ,3)
insert into GD_SolAdquisicionDet (CodSolAdquisicion ,Cantidad ,CodProveedor ,CodArticulo) values (2 ,1 ,1 ,1)
insert into GD_SolAdquisicionDet (CodSolAdquisicion ,Cantidad ,CodProveedor ,CodArticulo) values (3 ,1 ,1 ,1)
insert into GD_SolAdquisicionDet (CodSolAdquisicion ,Cantidad ,CodProveedor ,CodArticulo) values (3 ,100 ,2 ,2)
insert into GD_SolAdquisicionDet (CodSolAdquisicion ,Cantidad ,CodProveedor ,CodArticulo) values (4 ,2 ,3 ,3)