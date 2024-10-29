<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventarioCNTR.aspx.vb" Inherits="Band.Coletor.Redex.Consultas.WebFormICNTR" %>

<%@ register assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta  charset="utf-8 />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>
    <link rel="icon" href="favicon.ico"/>
    <title></title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/estilos.css" rel="stylesheet" />
    <link href="Content/site.css" rel="stylesheet" />
    <link href="Content/fontawesome-all.css" rel="stylesheet" />
    <link href="Content/toastr.css" rel="stylesheet" />
    <link href="Content/css/select2.css" rel="stylesheet" />
    <link href="Content/plugins/datatables/css/dataTables.bootstrap4.min.css" rel="stylesheet" />

</head>
<body>
  
    <form id="form1" runat="server" defaultbutton="btFiltra" >
         <header>
            <nav class="navbar navbar-expand-lg navbar-light bg-light">
                <asp:LinkButton ID="btnHome" runat="server" class="navbar-brand">
                <img src="Content/img/logo-coletor.png" class="d-inline-block align-top" alt=""/>
                </asp:LinkButton>
                <asp:LinkButton ID="btnLogout" runat="server" class="nav-link"><i class="fa fa-power-off mr-3"></i>Sair</asp:LinkButton>
            </nav>
        </header>

  <div class="row mt-3">
    <div class="col-sm-12 col-lg-8 offset-lg-2">

        <div class="card">
            <div class="card-header">
                <i class="fas fa-truck fa-lg"></i>
                Movimentação de Contêiner
    
                    <div class=" float-right">
                        <div class="btn-group">
                            <asp:LinkButton ID="btSair" OnClick="btSair_Click" runat="server" class="btn btn-link"><i class="fa fa-home"></i>&nbsp;&nbsp;Menu</asp:LinkButton>
                        </div>
                    </div>
            </div>

                  
                    <div class="card-body">
                         <div class="row">
                             <div class="col-sm-3">
                                 <div class="form-group">
                                     <label for="txtCNTR4">Contêiner:</label>
                                     <asp:TextBox ID="txtCNTR4" runat="server" CssClass="form-control" ></asp:TextBox>
                                     
                                 </div>
                             </div>
                             <div class="col-sm-6">
                                  <div class="form-group">
                                 <label class="control-label">&nbsp;</label>
                                 <asp:TextBox ID="txtCNTR"  runat="server" CssClass="form-control" MaxLength="12" ></asp:TextBox>
                             </div>
                             </div>
                             <div class="col-sm-3 " style="left: 0px; top: 0px; height: 35px">
                                 <div class="form-group">
                                     <label class="control-label">&nbsp;</label>
                                     <asp:LinkButton ID="btFiltra" Width="100%" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save" UseSubmitBehavior="False"></span>&nbsp;&nbsp;Filtra</asp:LinkButton>

                                 </div>
                             </div>
                         </div>
                        <div class="row">
                            <div class="form-group col-sm-3">
                                <label for="">Tamanho:</label>
                                <asp:TextBox ID="txtTAM" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Tipo:</label>
                                <asp:TextBox ID="txtTiPO" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">EF:</label>
                                <asp:TextBox ID="txtEF" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">GWT:</label>
                                <asp:TextBox ID="txtGWT" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>

                            <div class="form-group col-sm-3">
                                <label for="">Temper.:</label>
                                <asp:TextBox ID="txtTemp" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Escala:</label>
                                <asp:TextBox ID="txtEscala" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>

                            <div class="form-group col-sm-3">
                                <label for="">IMO:</label>
                                <asp:TextBox ID="txtIMO" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>

                            <div class="form-group col-sm-3">
                                <label for="">Entrada:</label>
                                <asp:TextBox ID="txtEntrada" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Categoria:</label>
                                <asp:TextBox ID="txtCATEG" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Navio:</label>
                                <asp:TextBox ID="txtNavio" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Posicionar:</label>
                                <asp:TextBox ID="txtPosicionamento" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Local:</label>
                                <span>&nbsp;AT.</span>
                                <asp:TextBox ID="txtYardAtual" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Regime:</label>
                                <asp:TextBox ID="txtRegime" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3 invisivel">
                                <asp:TextBox ID="txtSistema" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="txtAutonum" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="">Local/Mot.:</label>
                                <asp:TextBox ID="txtYard" AutoPostBack="True" runat="server" CssClass="form-control">XXXXXXXXXX</asp:TextBox>
                            </div>
                            <div class="form-group col-sm-6">
                                <label for="">Motivo Posicionamento:</label>
                                <asp:DropDownList runat="server" ID="cbMotivoPos" CssClass="form-control" />
                            </div>

                            <div class="form-group col-sm-6">
                                <label for="">Empilhadeira:</label>
                                <asp:DropDownList runat="server" ID="cbEmpilhadeira" CssClass="form-control" />
                            </div>
                            <div class="form-group col-sm-6">
                                <label for="">Veículo:</label>
                                <asp:DropDownList runat="server" ID="cbVeiculo" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="row mt-5">
                            <div class="form-groupcol-xs-12 col-md-6 col-lg-3">
                                    <asp:LinkButton ID="btSalvar" Width="100%"  UseSubmitBehavior="False" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Gravar</asp:LinkButton>
                            </div>
                             <div class="form-group col-xs-12 col-md-6 col-lg-3">
                                    <asp:LinkButton ID="btSalvar0" Width="100%"  UseSubmitBehavior="False" runat="server" CssClass="btn btn-warning"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Limpar</asp:LinkButton>
                            </div>
                                  <div class="form-group col-xs-12 col-md-6 col-lg-3">
                                    <asp:LinkButton ID="btSairOLD" Width="100%" UseSubmitBehavior="False" runat="server" CssClass="btn btn-success"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Sair</asp:LinkButton>
                            </div>
                        </div>
                         </div>

                    </div>
                    </div>

            </div>
    </form>
     <p>
        &nbsp;
    </p>

    <script src="lib/jquery-1.12.4.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/toastr.js"></script>
    <script src="Scripts/select2.min.js"></script>
    <script src="Content/plugins/datatables/js/jquery.dataTables.min.js"></script>
    <script src="Content/plugins/datatables/js/dataTables.bootstrap4.min.js"></script>
    <%--<script src="Content/plugins/datatables/js/datetime-moment.js"></script>--%>
    <script src="Content/plugins/moment/moment-with-locales.js"></script>

     <script type = "text/javascript" >
         
    </script>
</body>
</html>
