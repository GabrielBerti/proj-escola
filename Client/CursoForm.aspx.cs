using ModelEF;
using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        projEscolaEntities context = new projEscolaEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNome.Focus();
                carregaPagina();
                carregaGrid();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (validaCamposObrigatorios())
            {
                lblMensagem.Text = "Existem campos obrigatórios que não foram preenchidos";
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Font.Bold = true;
                ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
            }
            else
            {
                try
                {
                    int numSala;
                    if (!string.IsNullOrWhiteSpace(txtNumeroSala.Text))
                    {
                        numSala = int.Parse(txtNumeroSala.Text);
                    }
                    else
                    {
                        numSala = 0;
                    }

                    if (!string.IsNullOrWhiteSpace(txtId.Text))
                    {
                        int id = int.Parse(txtId.Text);
                        curso cursoResult = context.curso.First(x => x.id == id);
                        cursoResult.nome = txtNome.Text;
                        cursoResult.carga_horaria = txtCargaHoraria.Text;
                        cursoResult.horario_inicio = txtHorarioInicio.Text;
                        cursoResult.horario_fim = txtHorarioFim.Text;
                        cursoResult.numero_sala = numSala;

                        lblMensagem.Text = "Registro alterado com sucesso !";
                        lblMensagem.ForeColor = Color.Green;
                        lblMensagem.Font.Bold = true;
                        ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                        carregaGrid();
                    }
                    else
                    {
                        curso curso = new curso()
                        {
                            nome = txtNome.Text,
                            carga_horaria = txtCargaHoraria.Text,
                            horario_inicio = txtHorarioInicio.Text,
                            horario_fim = txtHorarioFim.Text,
                            numero_sala = numSala,
                        };
                        context.curso.Add(curso);

                        lblMensagem.Text = "Registro inserido com sucesso !";
                        lblMensagem.ForeColor = Color.Green;
                        lblMensagem.Font.Bold = true;
                        ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                    }
                    context.SaveChanges();
                    LimparForm();
                    carregaGrid();
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = $"{"Ocorreu um erro ao efetuar a operação."} {ex.Message}";
                    lblMensagem.ForeColor = Color.Red;
                    lblMensagem.Font.Bold = true;
                    ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                }
            }
        }
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparForm();
        }
        private bool validaCamposObrigatorios()
        {
            return string.IsNullOrWhiteSpace(txtNome.Text) ||
                   string.IsNullOrWhiteSpace(txtCargaHoraria.Text) ||
                   string.IsNullOrWhiteSpace(txtHorarioInicio.Text) ||
                   string.IsNullOrWhiteSpace(txtHorarioFim.Text);
        }
        private void LimparForm()
        {

            txtId.Text = String.Empty;
            txtNome.Text = String.Empty;
            txtCargaHoraria.Text = String.Empty;
            txtHorarioInicio.Text = String.Empty;
            txtHorarioFim.Text = String.Empty;
            txtNumeroSala.Text = String.Empty;
            txtNome.Focus();
        }
        private void carregaPagina()
        {
            string id = Request.QueryString["id"];
            if (!String.IsNullOrEmpty(id))
            {
                int newId = Convert.ToInt32(id);
                var cursoResult = context.curso.First(x => x.id == newId);
                txtId.Text = cursoResult.id.ToString();
                txtNome.Text = cursoResult.nome;
                txtCargaHoraria.Text = cursoResult.carga_horaria.ToString();
                txtHorarioInicio.Text = cursoResult.horario_inicio;
                txtHorarioFim.Text = cursoResult.horario_fim;
                txtNumeroSala.Text = cursoResult.numero_sala.ToString();
            }
        }

        protected void GVResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int line = int.Parse(e.CommandArgument.ToString());
            lblSelectedId.Text = gvResult2.Rows[line].Cells[2].Text;
            if (e.CommandName == "A")
            {
                Response.Redirect("CursoForm.aspx?id=" + int.Parse(lblSelectedId.Text));
            }
            else if (e.CommandName == "E")
            {
                deleteRegistro();
            }
        }
        protected void deleteRegistro()
        {
            try
            {
                int id = Convert.ToInt32(lblSelectedId.Text);
                context.curso.Remove(context.curso.First(x => x.id == id));
                context.SaveChanges();

                lblMensagem.Text = "Registro removido com sucesso";
                lblMensagem.ForeColor = Color.Green;
                lblMensagem.Font.Bold = true;
                ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                carregaGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = $"{"Ocorreu um erro ao efetuar a operação."} {ex.Message}";
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Font.Bold = true;
                ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
            }
        }
        private void carregaGrid()
        {
            gvResult2.DataSource = context.curso.ToList();
            gvResult2.DataBind();
        }
    }
}