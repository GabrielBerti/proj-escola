<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="CursoForm.aspx.cs" Inherits="Client.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js" integrity="sha384-9/reFTGAW83EW2RDu2S0VKaIzap3H66lZH81PoYlFhbGU+6BZp6G7niu735Sk7lN" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js" integrity="sha384-B4gt1jrGC7Jh4AgTPSdUtOBvfO8shuf57BaghqFfPlYxofvL8/KUEfYiJOMMV+rV" crossorigin="anonymous"></script>
    <title>Curso</title>
    <script type="text/javascript">
        function showMessage() {
            $('.toast').toast('show');
        }
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function formata(campo, mask, evt) {

            if (document.all) { // Internet Explorer
                key = evt.keyCode;
            }
            else { // Nestcape
                key = evt.which;
            }

            string = campo.value;
            i = string.length;

            if (i < mask.length) {
                if (mask.charAt(i) == '§') {
                    return (key > 47 && key < 58);
                } else {
                    if (mask.charAt(i) == '!') { return true; }
                    for (c = i; c < mask.length; c++) {
                        if (mask.charAt(c) != '§' && mask.charAt(c) != '!')
                            campo.value = campo.value + mask.charAt(c);
                        else if (mask.charAt(c) == '!') {
                            return true;
                        } else {
                            return (key > 47 && key < 58);
                        }
                    }
                }
            } else return false;
        }

    </script>

    <div class="toast ml-auto" role="alert" data-delay="5000" data-autohide="true" style="margin-right: 15px; margin-top: 15px;">
        <div class="toast-header">
            <strong class="mr-auto text-primary">Mensagem</strong>
            <button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">
                <span aria-hidden="true">×</span>
            </button>
        </div>
        <div class="toast-body">
            <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
        </div>
    </div>

     <form id="form" runat="server">
            <div class="row">
                <div class="col-2">
                    <div class="form-group">
                        <asp:Label ID="lblId" runat="server" Text="ID"></asp:Label>
                        <asp:TextBox class="form-control" ReadOnly="true" ID="txtId" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-5">
                    <div class="form-group">
                        <asp:Label ID="lblNome" runat="server" Text="*Nome"></asp:Label>
                        <asp:TextBox class="form-control" MaxLength="100" ID="txtNome" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-5">
                    <div class="form-group">
                        <asp:Label ID="lblCargaHoraria" runat="server" Text="*Carga Horária"></asp:Label>
                        <asp:TextBox class="form-control" MaxLength="100" ID="txtCargaHoraria" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-5">
                    <div class="form-group">
                        <asp:Label ID="lblHorarioInicio" runat="server" Text="*Horário Início"></asp:Label>
                        <asp:TextBox onKeyPress="return formata(this, '§§:§§', event)" class="form-control" MaxLength="100" ID="txtHorarioInicio" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-5">
                    <div class="form-group">
                        <asp:Label ID="lblHorarioFim" runat="server" Text="*Horario Fim"></asp:Label>
                        <asp:TextBox onKeyPress="return formata(this, '§§:§§', event)" class="form-control" MaxLength="100" ID="txtHorarioFim" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-5">
                    <div class="form-group">
                        <asp:Label ID="lblNumeroSala" runat="server" Text="Número da Sala"></asp:Label>
                        <asp:TextBox onkeypress="return allowOnlyNumber(event);" class="form-control" MaxLength="10" ID="txtNumeroSala" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="form-group">
                        <asp:Button class="btn btn-primary" ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                        <asp:Button class="btn btn-primary" ID="btnLimpar" runat="server" Text="Limpar" OnClick="btnLimpar_Click" />
                    </div>
                </div>
            </div>

             <div class="container">
                <asp:Label ID="lblSelectedId" runat="server" Visible="false" Text=""></asp:Label>

                <div class="table-responsive">
                    <asp:GridView ID="gvResult2" runat="server" AutoGenerateColumns="False" OnRowCommand="GVResult_RowCommand">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" HeaderText="Alterar" CommandName="A" ControlStyle-Width="18" ImageUrl="~/img/alterar.png"></asp:ButtonField>
                            <asp:ButtonField ButtonType="Image" HeaderText="Excluir" CommandName="E" ControlStyle-Width="18" ImageUrl="~/img/excluir.png"></asp:ButtonField>
                            <asp:BoundField DataField="id" HeaderText="ID" />
                            <asp:BoundField DataField="nome" HeaderText="Nome" />
                            <asp:BoundField DataField="carga_horaria" HeaderText="Carga Horária" />
                            <asp:BoundField DataField="horario_inicio" HeaderText="Horario Início" />
                            <asp:BoundField DataField="horario_fim" HeaderText="Horário Fim" />
                            <asp:BoundField DataField="numero_sala" HeaderText="Num. Sala" />
                        </Columns>
                    </asp:GridView>
                </div>
             </div>
            <script>
                var table = document.getElementById("gvResult2");
                table.classList.add("table");
                table.classList.add("table-hover");
            </script>
        
        </form>
</asp:Content>

