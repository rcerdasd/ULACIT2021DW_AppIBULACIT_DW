using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/Error")]
    public class ErrorController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Error error = new Error();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, FechaHora, Fuente, Numero, Descripcion, Vista, Accion FROM Error WHERE Codigo = @Codigo", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        error.Codigo = dataReader.GetInt32(0);
                        error.CodigoUsuario = dataReader.GetInt32(1);
                        error.FechaHora = dataReader.GetDateTime(2);
                        error.Fuente = dataReader.GetString(3);
                        error.Numero = dataReader.GetString(4);
                        error.Descripcion = dataReader.GetString(5);
                        error.Vista = dataReader.GetString(6);
                        error.Accion = dataReader.GetString(7);
                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
            return Ok(error);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Error> estadisticas = new List<Error>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, FechaHora, Fuente, Numero, Descripcion, Vista, Accion FROM Error", sqlConnection);
                    sqlConnection.Open();

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Error error = new Error();

                        error.Codigo = dataReader.GetInt32(0);
                        error.CodigoUsuario = dataReader.GetInt32(1);
                        error.FechaHora = dataReader.GetDateTime(2);
                        error.Fuente = dataReader.GetString(3);
                        error.Numero = dataReader.GetString(4);
                        error.Descripcion = dataReader.GetString(5);
                        error.Vista = dataReader.GetString(6);
                        error.Accion = dataReader.GetString(7);

                        estadisticas.Add(error);
                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
            return Ok(estadisticas);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Error error)
        {
            if (error == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Error (CodigoUsuario, FechaHora, Fuente, Numero, Descripcion, Vista, Accion) VALUES (@CodigoUsuario, @FechaHora, @Fuente, @Numero, @Descripcion, @Vista, @Accion)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", error.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@FechaHora", error.FechaHora);
                    sqlCommand.Parameters.AddWithValue("@Fuente", error.Fuente);
                    sqlCommand.Parameters.AddWithValue("@Numero", error.Numero);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", error.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Vista", error.Vista);
                    sqlCommand.Parameters.AddWithValue("@Accion", error.Accion);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }

            return Ok(error);
        }

        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            if (id < 1)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@"DELETE Error WHERE Codigo = @Codigo",
                                         sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(id);
        }
    }
}
