using ModelEF;
using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class ProfessorForm : System.Web.UI.Page
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
            else if (!txtEmail.Text.Contains('@'))
            {
                lblMensagem.Text = "Email inválido";
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Font.Bold = true;
                ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
            }
            else
            {
                try
                {

                    if (!string.IsNullOrWhiteSpace(txtId.Text))
                    {
                        int id = int.Parse(txtId.Text);
                        professor professorResult = context.professor.First(x => x.id == id);
                        professorResult.nome = txtNome.Text;
                        professorResult.cpf = txtCpf.Text;
                        professorResult.telefone = txtTelefone.Text;
                        professorResult.email = txtEmail.Text;
                        professorResult.salario = decimal.Parse(txtSalario.Text);

                        lblMensagem.Text = "Registro alterado com sucesso !";
                        lblMensagem.ForeColor = Color.Green;
                        lblMensagem.Font.Bold = true;
                        ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                        carregaGrid();
                    }
                    else
                    {

                        professor p = new professor()
                        {
                            nome = txtNome.Text,
                            cpf = txtCpf.Text,
                            telefone = txtTelefone.Text,
                            email = txtEmail.Text,
                            salario = decimal.Parse(txtSalario.Text),
                        };
                        context.professor.Add(p);

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
                   string.IsNullOrWhiteSpace(txtCpf.Text) ||
                   string.IsNullOrWhiteSpace(txtTelefone.Text) ||
                   string.IsNullOrWhiteSpace(txtEmail.Text) ||
                   string.IsNullOrWhiteSpace(txtSalario.Text);
        }
        private void LimparForm()
        {

            txtId.Text = String.Empty;
            txtNome.Text = String.Empty;
            txtCpf.Text = String.Empty;
            txtTelefone.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtSalario.Text = String.Empty;
            txtNome.Focus();
        }
        private void carregaPagina()
        {
            string id = Request.QueryString["id"];
            if (!String.IsNullOrEmpty(id))
            {
                int newId = Convert.ToInt32(id);
                var professorResult = context.professor.First(x => x.id == newId);
                txtId.Text = professorResult.id.ToString();
                txtNome.Text = professorResult.nome;
                txtCpf.Text = professorResult.cpf.ToString();
                txtTelefone.Text = professorResult.telefone;
                txtEmail.Text = professorResult.email;
                txtSalario.Text = professorResult.salario.ToString();
            }
        }

        protected void GVResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int line = int.Parse(e.CommandArgument.ToString());
            lblSelectedId.Text = gvResult.Rows[line].Cells[2].Text;
            if (e.CommandName == "A")
            {
                Response.Redirect("ProfessorForm.aspx?id=" + int.Parse(lblSelectedId.Text));
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
                context.professor.Remove(context.professor.First(x => x.id == id));
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
            gvResult.DataSource = context.professor.ToList();
            gvResult.DataBind();
        }
    }
}