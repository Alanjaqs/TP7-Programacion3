using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP7_Prueba
{
    public partial class SeleccionarSucursales : System.Web.UI.Page
    {
        private Conexion conexion = new Conexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                CargarListView();
            }
        }

        public void CargarListView()
        {
            string consultaSucursales = "SELECT [NombreSucursal], [URL_Imagen_Sucursal], [DescripcionSucursal], [Id_Sucursal] FROM [Sucursal]";
            SqlDataAdapter adapter = conexion.ObtenerAdaptador(consultaSucursales);
            DataTable tabla = new DataTable();
            adapter.Fill(tabla);

            lvSucursales.DataSource = tabla;
            lvSucursales.DataBind();
        }

        protected void btnSeleccionar_Command(object sender, CommandEventArgs e)
        {
            if(e.CommandName == "eventoSeleccionar")
            {
                // Guardar en un vector string los datos del CommandArgument divididos por el " - " (division puesta en los Eval en la pagina aspx)         
                string[] datos = e.CommandArgument.ToString().Split('-');

                // Guarda en cada variable los datos correspondientes cargados en el vector (el orden esta dado en los Eval en la pagina aspx)
                string idSucursal = datos[0];
                string nombreSucursal = datos[1];
                string descripcionSucursal = datos[2];

                if (Session["Tabla"] == null)
                {
                    Session["Tabla"] = CrearTabla();
                }

                // Guardar la Session DataTable en un objeto DataTable para verificar si en sus filas tiene algun ID Sucursal igual al seleccionado
                DataTable tabla = (DataTable)Session["Tabla"];
                bool yaExiste = tabla.AsEnumerable().Any(row => row["IdSucursal"].ToString() == idSucursal);

                // Si el ID ya existe
                if (yaExiste)
                {
                    lblMensaje.Text = "Sucursal ya agregada, seleccione otra";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                }
                // Si no existe agrega la sucursal
                else
                {
                    AgregarFila((DataTable)Session["Tabla"], idSucursal, nombreSucursal, descripcionSucursal);
                    Session["Tabla"] = tabla;
                    lblMensaje.Text = "Sucursal agregada: " + nombreSucursal;
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private DataTable CrearTabla()
        {
            DataTable tabla = new DataTable();
            DataColumn column = new DataColumn("IdSucursal", System.Type.GetType("System.String"));

            tabla.Columns.Add(column);
            column = new DataColumn("Nombre", System.Type.GetType("System.String"));
            tabla.Columns.Add(column);
            column = new DataColumn("Descripcion", System.Type.GetType("System.String"));
            tabla.Columns.Add(column);

            return tabla;
        }

        private DataTable AgregarFila(DataTable tabla, string idSucursal, string nombreSucursal, string descripcionSucursal)
        {
            DataRow row = tabla.NewRow();

            row["IdSucursal"] = idSucursal;
            row["Nombre"] = nombreSucursal;
            row["Descripcion"] = descripcionSucursal;

            tabla.Rows.Add(row);

            return tabla;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string sucursal = txtBuscar.Text;
                string consultaSucursalBuscada = "SELECT [NombreSucursal], [URL_Imagen_Sucursal], [DescripcionSucursal], [Id_Sucursal] " +
                "FROM [Sucursal] WHERE [NombreSucursal] LIKE '%" + sucursal + "%'";
                SqlDataAdapter adapter = conexion.ObtenerAdaptador(consultaSucursalBuscada);
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);

                lvSucursales.DataSource = tabla;
                lvSucursales.DataBind();
            }
            else
                CargarListView();        
        }
    }
}