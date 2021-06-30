﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppIBULACIT.Controllers;
using AppIBULACIT.Models;

namespace AppIBULACIT.Views
{
    public partial class frmServicio : System.Web.UI.Page
    {
        IEnumerable<Servicio> servicios = new ObservableCollection<Servicio>();
        ServicioManager servicioManager = new ServicioManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarControles();
        }

        private async void InicializarControles()
        {
            try
            {
                servicios = await servicioManager.ObtenerServicios(Session["Token"].ToString());
                gvServicios.DataSource = servicios.ToList();
                gvServicios.DataBind();
            }
            catch (Exception ex)
            {

                lblStatus.Text = "Hubo un error al cargar la lista de servicios";
            }
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            string resultado = string.Empty;
            resultado = await servicioManager.Eliminar(Session["Token"].ToString(), lblCodigoEliminar.Text);
            if (!string.IsNullOrEmpty(resultado))
            {
                lblCodigoEliminar.Text = string.Empty;
                ltrModalMensaje.Text = "Servicio eliminado";
                btnAceptarModal.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModal();})", true);
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo servicio";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;
            ddlEstadoMant.Visible = true;
            txtCodigoMant.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento();});", true);

        }

        protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvServicios.Rows[index];
            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Mantenimiento servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtDescripcion.Text = row.Cells[1].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento();});", true);
                    break;
                case "Eliminar":
                    lblCodigoEliminar.Text = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el servicio " + lblCodigoEliminar.Text + "?";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModal(); });", true);
                    break;
                default:
                    break;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text))
            {
                Servicio servicio = new Servicio()
                {
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue

                };

                Servicio servicioIngresado = await servicioManager.Ingresar(servicio, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(servicioIngresado.Descripcion))
                {
                    lblResultado.Text = "Servicio ingresado con exito";
                    lblResultado.ForeColor = Color.Green;
                    lblResultado.Visible = true;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                }
                else
                {
                    lblResultado.Text = "Hubo un error al ingresar el servicio";
                    lblResultado.ForeColor = Color.Maroon;
                    lblResultado.Visible = true;
                }
            }
            else
            {
                Servicio servicio = new Servicio()
                {
                    Codigo = Convert.ToInt32(txtCodigoMant.Text),
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue

                };

                Servicio servicioModificado = await servicioManager.Actualizar(servicio, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(servicioModificado.Descripcion))
                {
                    lblResultado.Text = "Servicio ingresado con exito";
                    lblResultado.ForeColor = Color.Green;
                    lblResultado.Visible = true;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                }
                else
                {
                    lblResultado.Text = "Hubo un error al ingresar el servicio";
                    lblResultado.ForeColor = Color.Maroon;
                    lblResultado.Visible = true;
                }
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {​​​ CloseMantenimiento(); }​​​);", true);

        }

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }
    }
}