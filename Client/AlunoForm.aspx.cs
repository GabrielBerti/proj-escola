using ModelEF;
using System;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class AlunoForm : System.Web.UI.Page
    {
        projEscolaEntities context = new projEscolaEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNome.Focus();
                carregaPagina();
                carregaGrid();
                carregaLstCursos();
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
                        aluno alunoResult = context.aluno.First(x => x.id == id);
                        alunoResult.nome = txtNome.Text;
                        alunoResult.cpf = txtCpf.Text;
                        alunoResult.telefone = txtTelefone.Text;
                        alunoResult.email = txtEmail.Text;
                        alunoResult.ra = txtRa.Text;
                        alunoResult.id_curso = int.Parse(CboCurso.SelectedValue.ToString());

                        lblMensagem.Text = "Registro alterado com sucesso !";
                        lblMensagem.ForeColor = Color.Green;
                        lblMensagem.Font.Bold = true;
                        ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                        carregaGrid();
                    }
                    else
                    {
                        aluno aluno = new aluno()
                        {
                            nome = txtNome.Text,
                            cpf = txtCpf.Text,
                            telefone = txtTelefone.Text,
                            email = txtEmail.Text,
                            ra = txtRa.Text,
                            id_curso = int.Parse(CboCurso.SelectedValue.ToString()),
                        };
                        context.aluno.Add(aluno);

                        lblMensagem.Text = "Registro inserido com sucesso !";
                        lblMensagem.ForeColor = Color.Green;
                        lblMensagem.Font.Bold = true;
                        ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                    }
                    // context.SaveChanges();

                    try
                    {
                        // seu código...
                        context.SaveChanges();
                        LimparForm();
                        carregaGrid();
                        //Response.Redirect("AlunoForm.aspx");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var eve in ex.EntityValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Entidade do tipo \"{0}\" com estado \"{1}\" tem os seguintes erros de validação:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("- Propriedade: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }

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
                   string.IsNullOrWhiteSpace(CboCurso.Text);
        }
        private void LimparForm()
        {

            txtId.Text = String.Empty;
            txtNome.Text = String.Empty;
            txtCpf.Text = String.Empty;
            txtTelefone.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtRa.Text = String.Empty;
            CboCurso.SelectedValue = null;
            txtNome.Focus();
        }
        private void carregaPagina()
        {
            string id = Request.QueryString["id"];
            if (!String.IsNullOrEmpty(id))
            {
                int newId = Convert.ToInt32(id);
                var alunoResult = context.aluno.First(x => x.id == newId);
                txtId.Text = alunoResult.id.ToString();
                txtNome.Text = alunoResult.nome;
                txtCpf.Text = alunoResult.cpf.ToString();
                txtTelefone.Text = alunoResult.telefone.ToString();
                txtEmail.Text = alunoResult.email.ToString();
                CboCurso.SelectedValue = alunoResult.id.ToString();
            }
        }

        protected void GVResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int line = int.Parse(e.CommandArgument.ToString());
            String idAlt = gvResult2.Rows[line].Cells[2].Text;

            if (e.CommandName == "A")
            {
                //Response.Redirect("AlunoForm.aspx?id=" + int.Parse(lblSelectedId.Text));
                alterarPagina(idAlt);
            }
            else if (e.CommandName == "E")
            {
                deleteRegistro(idAlt);
            }
        }
        protected void deleteRegistro(String idDel)
        {
            try
            {
                int id = Convert.ToInt32(idDel);
                context.aluno.Remove(context.aluno.First(x => x.id == id));
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
            gvResult2.DataSource = context.aluno.ToList();
            gvResult2.DataBind();
        }

        private void carregaLstCursos()
        {
            CboCurso.DataSource = context.curso.ToList();
            CboCurso.DataValueField = "Id";
            CboCurso.DataTextField = "Nome";
            CboCurso.DataBind();
        }

        private void alterarPagina(String id)
        {
            int newId = Convert.ToInt32(id);
            var alunoResult = context.aluno.First(x => x.id == newId);
            txtId.Text = alunoResult.id.ToString();
            txtNome.Text = alunoResult.nome;
            txtCpf.Text = alunoResult.cpf.ToString();
            txtTelefone.Text = alunoResult.telefone.ToString();
            txtEmail.Text = alunoResult.email.ToString();
            txtRa.Text = alunoResult.ra.ToString();
            CboCurso.SelectedValue = alunoResult.id_curso.ToString();
            txtNome.Focus();
        }
    }
}