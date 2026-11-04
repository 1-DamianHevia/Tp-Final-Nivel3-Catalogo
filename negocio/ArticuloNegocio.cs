using dominio;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion DescCat, M.Descripcion DescMarca, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where M.Id = A.IdMarca And C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Precio =Math.Round((decimal)datos.Lector["Precio"],2);

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int) datos.Lector["IdMarca"];
                    aux.Marca.DescripcionMarca = (string)datos.Lector["DescMarca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.DescripcionCat = (string)datos.Lector["DescCat"];
                    

                    
                    


                    lista.Add(aux);

                }
                return lista;
            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void agregar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.setearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, ImagenUrl, Precio, IdMarca, IdCategoria)values('" + articulo.CodArticulo + "', '" + articulo.Nombre + "', '" + articulo.Descripcion + "','" + articulo.UrlImagen + "' , '" + articulo.Precio + "', @idMarca, @idCategoria)");

                datos.setearParametro("@IdMarca", articulo.Marca.Id);
                datos.setearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @imagenUrl, Precio = @Precio where id = @id");

                datos.setearParametro("@codigo", articulo.CodArticulo);
                datos.setearParametro("@nombre", articulo.Nombre);
                datos.setearParametro("@descripcion", articulo.Descripcion);
                datos.setearParametro("@IdMarca", articulo.Marca.Id);
                datos.setearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametro("@imagenUrl", articulo.UrlImagen);
                datos.setearParametro("@precio", articulo.Precio);
                datos.setearParametro("@id", articulo.Id);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally 
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<Articulo>filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {

                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion DescCat, M.Descripcion DescMarca, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where M.Id = A.IdMarca And C.Id = A.IdCategoria And ";

                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a: ":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a: ":
                            consulta += "Precio < " + filtro;
                            break;
                        case "Igual a: ":
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con: ":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con: ":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        case "Contiene: ":
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {

                    switch (criterio)
                    {
                        case "Comienza con: ":
                            consulta += "C.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con: ":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        case "Contiene: ":
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }


                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 2);

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.DescripcionMarca = (string)datos.Lector["DescMarca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.DescripcionCat = (string)datos.Lector["DescCat"];






                    lista.Add(aux);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }





        }
    }

  
}
