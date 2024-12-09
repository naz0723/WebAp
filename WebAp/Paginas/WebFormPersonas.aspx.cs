using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace WebAp.Paginas
{
    public partial class WebFormPersonas : System.Web.UI.Page
    {
        private static readonly string apiUrl = "https://localhost:7224/api/personas"; // Cambia por la URL de tu API
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar personas al cargar la página
                CargarPersonasAsync();
            }
        }
        // Método para cargar la lista de personas
        protected async void btnCargar_Click(object sender, EventArgs e)
        {
            await CargarPersonasAsync();
        }

        // Método para guardar una persona (crear o actualizar)
        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            var nuevaPersona = new Personas
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text),
                Telefono = txtTelefono.Text,
                AdicionadoPor = "WebFormüser"
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string json = JsonSerializer.Serialize(nuevaPersona); // Serializamos el objeto a JSON
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json"); // Configuramos el contenido

                    HttpResponseMessage response = await client.PostAsync(apiurl, content); // Enviamos el POST

                    if (response.IsSuccessStatusCode)
                    {
                        Response.Write("<script>alert('Persona guardada con éxito');</script>");
                        await CargarPersonasAsync(); // Recargar la lista
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        Response.Write($"<script>alert('Error: (response. StatusCode) [error]');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error al guardar datos: [ex.Message}'); </script>");
                }
            }
        }
    }
}