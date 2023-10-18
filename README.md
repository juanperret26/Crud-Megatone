# Crud-Megatone
 El controlador ProductosController será responsable de exponer los métodos Create, Delete, Edit y Listado. El controlador FamiliaController será responsable de exponer los métodos Create, Delete y Edit. El controlador MarcasController será responsable de exponer el método Create, Delete y Edit. Siguiendo el enunciado que posteriormente esta detallado:

Alta Producto: Permita dar de alta un registro en la tabla producto, si se crea un producto ya eliminado, dar de alta un registro nuevo, no reactivar el borrado, tener en cuenta la fecha modificación. 
Baja Producto: Marque baja lógica en la tabla productos, con la fecha de baja. 
Modificación producto: Debe permitir editar Descripción del producto, precio costo, precio venta, idMarca y idFamilia, tener en cuenta la fecha modificación. 
Alta Familia y Marca: Permita dar de alta Familia y Marca, tener en cuenta la fecha modificación. 
Baja Familia y Marca: Marca Baja Logica, con la fecha de baja correspondiente. No debe permitir borrar si tiene producto asociado activo (no borrado). 
Modificación Familia y Marca: solo se debe permitir modificar descripción, tener en cuenta la fecha modificación. 
Listado: Devolver un listado de productos, se debe permitir filtrar por código producto, idMarca o IdFamilia. Debe traer todo ordenado por fecha de modificación y solo los registros activos. Al momento de filtrar no son obligatorios todos los filtros, pero al menos uno de los 3 tiene que pasarse. 
