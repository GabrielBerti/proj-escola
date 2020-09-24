using ModelEF;
using System;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class CursoProfessorForm : System.Web.UI.Page
    {
        projEscolaEntities context = new projEscolaEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                carregaPagina();
                carregaGrid();
                carregaLstCursos();
                carregaLstProfessores();
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

                    if (!string.IsNullOrWhiteSpace(txtId.Text))
                    {
                        int id = int.Parse(txtId.Text);
                        curso_professor cursoProfessorResult = context.curso_professor.First(x => x.id == id);
                        cursoProfessorResult.id_curso = int.Parse(CboCurso.SelectedValue.ToString());
                        cursoProfessorResult.id_professor = int.Parse(cboProfessor.SelectedValue.ToString());

                        lblMensagem.Text = "Registro alterado com sucesso !";
                        lblMensagem.ForeColor = Color.Green;
                        lblMensagem.Font.Bold = true;
                        ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "showMessage();", true);
                        carregaGrid();
                    }
                    else
                    {
                        curso_professor curso_professor = new curso_professor()
                        {
                            id_curso = int.Parse(CboCurso.SelectedValue.ToString()),
                            id_professor = int.Parse(cboProfessor.SelectedValue.ToString()),
                        };
                        context.curso_professor.Add(curso_professor);

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
                        //Response.Redirect("CursoProfessorForm.aspx");
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
            return string.IsNullOrWhiteSpace(cboProfessor.Text) ||
                   string.IsNullOrWhiteSpace(CboCurso.Text);
        }
        private void LimparForm()
        {
            txtId.Text = String.Empty;
            CboCurso.SelectedValue = null;
            cboProfessor.SelectedValue = null;
        }
        private void carregaPagina()
        {
            string id = Request.QueryString["id"];
            if (!String.IsNullOrEmpty(id))
            {
                int newId = Convert.ToInt32(id);
                var cursoProfessorResult = context.curso_professor.First(x => x.id == newId);

                CboCurso.SelectedValue = cursoProfessorResult.id_curso.ToString();
                cboProfessor.SelectedValue = cursoProfessorResult.id_professor.ToString();
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
                context.curso_professor.Remove(context.curso_professor.First(x => x.id == id));
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
            gvResult2.DataSource = context.curso_professor.ToList();
            gvResult2.DataBind();
        }

        private void carregaLstCursos()
        {
            CboCurso.DataSource = context.curso.ToList();
            CboCurso.DataValueField = "Id";
            CboCurso.DataTextField = "Nome";
            CboCurso.DataBind();
        }

        private void carregaLstProfessores()
        {
            cboProfessor.DataSource = context.professor.ToList();
            cboProfessor.DataValueField = "Id";
            cboProfessor.DataTextField = "Nome";
            cboProfessor.DataBind();
        }

        private void alterarPagina(String id)
        {
            int newId = Convert.ToInt32(id);
            var cursoProfessorResult = context.curso_professor.First(x => x.id == newId);

            txtId.Text = cursoProfessorResult.id.ToString();
            CboCurso.SelectedValue = cursoProfessorResult.id_curso.ToString();
            cboProfessor.SelectedValue = cursoProfessorResult.id_professor.ToString();
        }
    }
}