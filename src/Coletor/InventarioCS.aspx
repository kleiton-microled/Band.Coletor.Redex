<%@ page language="vb" autoeventwireup="false" codebehind="InventarioCS.aspx.vb" inherits="Band.Coletor.Redex.Consultas.WebForm3" %>

<%@ register assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
   
    <form id="form1" runat="server" submitdisabledcontrols="False">
         <header>
            <nav class="navbar navbar-expand-lg navbar-light bg-light">
                <asp:LinkButton ID="btnHome" runat="server" class="navbar-brand">
                <img src="Content/img/logo-coletor.png" class="d-inline-block align-top" alt=""/>
                </asp:LinkButton>
                <asp:LinkButton ID="btnLogout" runat="server" class="nav-link"><i class="fa fa-power-off mr-3"></i>Sair</asp:LinkButton>
            </nav>
        </header>

        <asp:scriptmanager runat="server"></asp:scriptmanager>

<div class="row mt-3">
    <div class="col-sm-12 col-lg-8 offset-lg-2">

                <div class="card">
                    <div class="card-header">
                        <i class="fas fa-truck fa-lg"></i>
                        Movimentação de Carga Solta
    
                    <div class=" float-right">
                        <div class="btn-group">
                            <asp:LinkButton ID="btSair" OnClick="btSair_Click" runat="server" class="btn btn-link"><i class="fa fa-home"></i>&nbsp;&nbsp;Menu</asp:LinkButton>
                        </div>
                    </div>
                    </div>

                    <div class="card-body">
                        <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                        <% if ViewState("Sucesso") IsNot Nothing Then
                                If Convert.ToBoolean(ViewState("Sucesso")) = True Then %>
                        <div class="alert alert-success">
                            Registro cadastrado/atualizado com sucesso!
                             <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                 <span aria-hidden="true">&times;</span>
                             </button>
                        </div>

                        <%      End If
                           End If%>
                         <div class="row">
                              <div class="form-group col-sm-3">
                                     <label for="">Lote:</label>
                                     <asp:TextBox ID="txtLote" AutoPostBack="True" runat="server" CssClass="form-control" ></asp:TextBox>
                                     
                                 </div>
                              <div class="form-group col-sm-3">
                                     <label for="">BL:</label>
                                     <asp:TextBox ID="txtBL" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXX</asp:TextBox>
                                     
                             </div>
                               <div class="form-group col-sm-6">
                                                <label for="">Item:</label>
                                                <asp:DropDownList runat="server" ID="cbItem" autopostback="True" CssClass="form-control" />
                                        </div>
                             <div class="form-group col-sm-3">
                                     <label for="">Quantidade:</label>
                                     <asp:TextBox ID="txtQtde" ReadOnly="true" runat="server" CssClass="form-control" >X</asp:TextBox>
                             </div>
                             <div class="form-group col-sm-3">
                                     <label for="">Embalagem:</label>
                                     <asp:TextBox ID="txtEmbalagem" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXX</asp:TextBox>
                             </div>
                             <div class="form-group col-sm-3">
                                     <label for="">Volume:</label>
                                     <asp:TextBox ID="txtVolume" readonly="True" runat="server" CssClass="form-control" >X</asp:TextBox>
                             </div>
                             <div class="form-group col-sm-3">
                                     <label for="">Local:</label>
                                     <asp:TextBox ID="txtLocal" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXXXXXXX</asp:TextBox>
                             </div>
                              <div class="form-group col-sm-3">
                                     <label for="">Mercadoria:</label>
                                     <asp:TextBox ID="txtMercadoria" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXX</asp:TextBox>
                             </div>
                             <asp:Panel ID="pnEsconde1" runat="server" Visible="false">
                                  <div class="form-group col-sm-3">
                                     <label for="">Marca:</label>
                                     <asp:TextBox ID="txtMarca" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXXX</asp:TextBox>
                             </div>
                             </asp:Panel>
                               <div class="form-group col-sm-3">
                                     <label for="">Importadora/Exportadora:</label>
                                     <asp:TextBox ID="txtCliente" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXXXXXXX</asp:TextBox>
                             </div>
                             <div class="form-group col-sm-3">
                                     <label for="">NVOCC:</label>
                                     <asp:TextBox ID="txtNVOCC" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXXXXXXX</asp:TextBox>
                             </div>
                             <div class="form-group col-sm-3">
                                     <label for="">Contêiner:</label>
                                     <asp:TextBox ID="txtConteiner" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXXXXXXX</asp:TextBox>
                             </div>
                             <div class="form-group col-sm-3">
                                     <label for="">Entrada:</label>
                                     <asp:TextBox ID="txtEntrada" readonly="True" runat="server" CssClass="form-control" >XXXXXXXXXXXXXXX</asp:TextBox>
                             </div>
                             <asp:Panel ID="pnEsconde2" runat="server" Visible="false">
                                     <div class="form-group">
                                         <label for="">Doc.:</label>
                                         <asp:TextBox ID="txtDOC" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXXXXXXX</asp:TextBox>
                                 </div>
                                 <div class="form-group col-sm-3">
                                         <label for="">Canal:</label>
                                         <asp:TextBox ID="txtCanal" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXXXXXXX</asp:TextBox>
                                 </div>
                                </asp:panel>

                             <asp:Panel ID="pnEsconde3" runat="server" Visible="false">
                                 <div class="form-group col-sm-3">
                                         <label for="">Mov. Agenda:</label>
                                         <asp:TextBox ID="txtMOV" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXXXX</asp:TextBox>
                                 </div>
                            </asp:Panel>

                             <div class="form-group col-sm-3">
                                         <label for="">Anvisa:</label>
                                         <asp:TextBox ID="txtANVISA" ReadOnly="True" runat="server" CssClass="form-control"></asp:TextBox>
                                 </div>
                              <div class="form-group col-sm-3">
                                         <label for="">IMO:</label>
                                         <asp:TextBox ID="txtIMO" ReadOnly="True" runat="server" CssClass="form-control">XXXXXXXXXXXXXXX</asp:TextBox>
                                 </div>
                             <div class="form-group col-sm-3">
                                         <label for="">Quantidade:</label>
                                         <asp:TextBox ID="txtQtdePos" AutoPostBack="True" runat="server" CssClass="form-control">XXXXXX</asp:TextBox>
                                 </div>
                             <div class="form-group col-sm-6">
                                                <label for="">Armazém:</label>
                                                <asp:DropDownList runat="server" ID="cbArm"  autopostback="True" CssClass="form-control" />
                                        </div>
                             <div class="form-group col-sm-3">
                                         <label for="">Local Posição:</label>
                                         <asp:TextBox ID="txtlocalpos" AutoPostBack="True" runat="server" CssClass="form-control">XXXXXX</asp:TextBox>
                                 </div>
                             <asp:Panel runat="server" ID="PanelOcupacao">
                                <div class="form-group col-sm-6">
                                                <label for="">Ocupação</label>
                                                <asp:DropDownList runat="server" ID="cbOcupacao_CT" AutoPostBack="True" CssClass="form-control">
                                                    <asp:ListItem>0%</asp:ListItem>
                                                    <asp:ListItem>25%</asp:ListItem>
                                                    <asp:ListItem>50%</asp:ListItem>
                                                    <asp:ListItem>75%</asp:ListItem>
                                                    <asp:ListItem>100%</asp:ListItem>
                                                </asp:DropDownList>
                                        </div>
                             </asp:Panel>
                         
                        <asp:Panel runat="server" ID="PanelLocalPosicionamento" visible="False">

                        <div class="form-group col-sm-6">
                                                <label for="">Local Posicionamento:</label>
                                                <asp:DropDownList runat="server" ID="cbLocalPOS" autopostback="True"  CssClass="form-control" />
                                                <asp:ListSearchExtender ID="cbLocalPOS_ListSearchExtender" runat="server"
                                                    Enabled="True" IsSorted="True" TargetControlID="cbLocalPOS">
                                                </asp:ListSearchExtender>
                                        </div>
                        </asp:Panel>

                        <asp:Panel ID="pnEsconde4" runat="server" Visible="false">
                            <div class="form-group col-sm-3">
                                     <label for="">NO&nbsp; LOCAL</label>
                                     <asp:TextBox ID="txtCargaNoLocal"  runat="server" CssClass="form-control" ></asp:TextBox>
                             </div>
                        </asp:Panel>

                         <div class="form-group col-sm-3">
                                                <label for="">Motivo:</label>
                                                <asp:DropDownList runat="server" ID="cbMotivoPos" CssClass="form-control" />
                                        </div>
                        
                             <asp:Panel runat="server" ID="PanelSistema" Visible="false">

                             <div class="form-group col-sm-3">
                                     <label for="">&nbsp</label>
                                     <asp:TextBox ID="txtSISTEMA" backcolor="#5D7B9D" borderstyle="None" forecolor="#FFCC66"  runat="server" CssClass="form-control" ></asp:TextBox>
                             </div>
                             </asp:Panel>

                        <div class="form-group col-sm-3">
                                     <label for="">&nbsp</label>
                                     <asp:TextBox ID="txtYard" Font-Size="Medium" BackColor="#FFFF66" AutoPostBack="True" Visible="False" runat="server" CssClass="form-control" ></asp:TextBox>
                             </div>
                         </div>

                    <div class="row">
                        <div class="form-group col-xs-12 col-md-6 col-lg-3">
                                    <asp:LinkButton ID="btSalvar" Width="100%"  UseSubmitBehavior="False" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Gravar</asp:LinkButton>
                            </div>
                         <div class="form-group col-xs-12 col-md-6 col-lg-3">
                                    <asp:LinkButton ID="btSalvar0" Width="100%"  UseSubmitBehavior="False" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Gravar / Repetir</asp:LinkButton>
                            </div>
                        <div class="form-group col-xs-12 col-md-6 col-lg-3">
                                    <asp:LinkButton ID="btSairOLD" Width="100%"  UseSubmitBehavior="False" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Sair</asp:LinkButton>
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
    <script src="Content/plugins/datatables/js/datetime-moment.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            var b = $(window).height(); //gets the window's height, change the selector if you are looking for height relative to some other element
            $("#table1").css("height", b - 0);
        });


        function AlturaTela() {

            var H = document.documentElement.clientHeight;
            var W = window.screen.availWidth;
            //   alert(H);

            // moveTo(-4, -4);
            //resizeTo(screen.availWidth + 8, screen.availHeight + 8);

            //document.getElementById('form1').style.height = H + 'px';
            //alert("form1");
            //    document.getElementById('login-box').style.height = H + 'px';
            //   alert(H);
            //    document.getElementById('table1').style.height = H + 'px';
            //    document.getElementById('login-box').style.width = W + 'px';


        }
    </script>
</body>
</html>
