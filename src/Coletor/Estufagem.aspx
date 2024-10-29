<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Estufagem.aspx.vb" Inherits="Band.Coletor.Redex.Consultas.Estufagem" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>

    <meta charset="utf-8"/>
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

    <style type="text/css">
      
        .auto-style1 {
            font-weight: bold;
            font-size: 0.9em;
        }
      
    </style>
</head>
<body>
   
    <form id="form1" runat="server">
         <header>
            <nav class="navbar navbar-expand-lg navbar-light bg-light">
                <asp:LinkButton ID="btnHome" runat="server" class="navbar-brand">
                <img src="Content/img/logo-coletor.png" class="d-inline-block align-top" alt=""/>
                </asp:LinkButton>
                <asp:LinkButton ID="btnLogout" runat="server" class="nav-link"><i class="fa fa-power-off mr-3"></i>Sair</asp:LinkButton>
            </nav>
        </header>

        <asp:ScriptManager runat="server"></asp:ScriptManager>
        
        <asp:HiddenField runat="server" ID="hddnMensagemConfirmacao" Value="" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnAutonumNFI" Value="" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnSC" Value="" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnPatio" Value="" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnQtdeSaida" Value="" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnCodProduto" Value="" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnRowIndex" Value="" ClientIDMode="Static" />


        <asp:HiddenField runat="server" ID="hddnRegistro" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnAutonumBOO" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnAutonumPatio" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnAutonumRomaneio" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnAutonumCliente" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnAutonumTalie" Value="0" ClientIDMode="Static" />

        <asp:CheckBox runat="server" ID="chkDocking" Visible="false" />
          <%--      <asp:Panel ID="pnConfirma" align="center" CssClass="modalPopup" runat="server" Visible="true" Style="background: #FFF68F; font-family: Tahoma; font-size: 13px;" BackColor="#CCCCCC">
            <div style="background: #FFF68F; padding: 10px; padding: 10px;">
                <asp:Label ID="lblAlerta" runat="server" Text="Atenção: Existem lotes ainda não desovados. Deseja Continuar ? "></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnConfirmar" runat="server" Text="Sim" UseSubmitBehavior="false" />
                &nbsp;<asp:Button ID="btnFecharConfirm" runat="server" Text="Não" UseSubmitBehavior="false" />
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
            CancelControlID="btnConfirmar" DropShadow="true" PopupControlID="pnConfirma" BackgroundCssClass="modalBackground"
            PopupDragHandleControlID="pnConfirma" TargetControlID="btnFecharConfirm" OkControlID="btnConfirmar">
        </cc1:ModalPopupExtender>--%>

        <div class="row mt-3">
            <div class="col-sm-12 col-lg-8 offset-lg-2">

                <div class="card">
                    <div class="card-header">
                        <i class="fas fa-truck fa-lg"></i>
                        &nbsp&nbsp Estufagem
    
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

                        <% End If %>                        <% End If%>

                        <div class="row">
                            <div class="form-group col-xs-12 col-md-2 col-lg-2 ">
                                <label for="txtConteiner">Contêiner:</label>
                                <asp:TextBox ID="txtConteiner" ClientIDMode="Static" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtConteiner_TextChanged"></asp:TextBox>
                            </div>
                            <div class="form-group col-xs-12 col-md-2 col-lg-2 ">
                                <label for="txtReserva">Reserva:</label>
                                <asp:TextBox ID="txtReserva" runat="server" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group col-xs-12 col-md-4 col-lg-4 ">
                                <label for="cbCliente">Cliente:</label>
                                <asp:TextBox ID="txtCliente" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </div>
<%--                        </div>
                        <div class="row">--%>
                            <div class="form-group col-xs-6 col-md-2 col-lg-2 ">

                                <label for="txtInicio">Início:</label>
                                <asp:TextBox ID="txtInicio" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group col-xs-6 col-md-2 col-lg-2 ">

                                <label for="txtFim">Término:</label>
                                <asp:TextBox ID="txtFIM" runat="server" ReadOnly="true" CssClass="form-control" />
                            </div>
                            <div class="form-group col-xs-12 col-md-4 col-lg-4 ">

                                <label for="cbConferente">Conferente:</label>
                                <asp:DropDownList runat="server" ID="cbConferente" CssClass="form-control" />
                            </div>
                        
                            <div class="form-group col-xs-6 col-md-4 col-lg-4 ">

                                <label for="cbEquipe">Equipe:</label>
                                <asp:DropDownList runat="server" ID="cbEquipe" CssClass="form-control" />
                            </div>
                            <div class="form-group col-xs-6 col-md-4 col-lg-4 ">

                                <label for="cbModo">Modo:</label>
                                <asp:DropDownList runat="server" ID="cbModo" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row  mt-3">
                            <asp:Panel ID="pnlLimpar" runat="server">
                                <asp:LinkButton ID="btnTalieCancelar" Width="100%" runat="server" CssClass="btn btn-warning btn-block" Enable="false"><i class="fa fa-eraser"></i>&nbsp;&nbsp;Limpar</asp:LinkButton>
                            </asp:Panel>

                            <asp:Panel ID="pnlGravar" runat="server" CssClass="form-group col-xs-12 col-md-2 col-lg-2">
                                <asp:LinkButton ID="btnGravar" Width="100%" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Gravar</asp:LinkButton>
                            </asp:Panel>

                            <asp:Panel ID="pnlFinalizar" Visible="false" runat="server" CssClass="form-group col-xs-12 col-md-2 col-lg-2 " style="left: 0px; top: 0px; height: 35px">
                                <button id="btnFinalizar" type="button" onclick="FinalizarTalie()" style="width: 100%" class="btn btn-success"><span aria-hidden="true" class="fa fa-truck"></span>&nbsp;&nbsp;Finalizar
                                </button>
                            </asp:Panel>
                        </div>
                        <br />
                        <div class="row">
                            <div class="form-group col-xs-12 col-md-2 col-lg-2 ">
                                <label for="txtProduto">
                                <span class="auto-style1">NF </span>:</label>
                                <asp:TextBox ID="txtOS" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtOS_TextChanged"  MaxLength="12" />
                            </div>
                            <div class="form-group col-xs-12 col-md-2 col-lg-2 ">
                                <label for="cbNFs">Item NF :</label>
                                <asp:DropDownList runat="server" ID="cbNFs" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cbNFs_SelectedIndexChanged" />
                                 
                            </div>
                            <div class="form-group col-xs-12 col-md-2 col-lg-2 ">
                                <label for="txtQuantidade">Quantidade:</label>
                                <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control"  />
                            </div>
                            <div class="form-group col-xs-12 col-md-2 col-lg-2 ">
                                <label for="txtReservaCarga">Reserva:</label>
                                <asp:TextBox ID="txtReservaCarga" runat="server" CssClass="form-control" AutoPostBack="True" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group col-xs-12 col-md-2 col-lg-2 offset-md-2 offset-lg-2">
                                <label>&nbsp;</label>
                                <asp:LinkButton ID="btnCarga" runat="server" CssClass="btn btn-primary btn-block"><span aria-hidden="true" class="fa fa-charging-station"></span>&nbsp;&nbsp;Gravar Estufagem</asp:LinkButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 mt-3">
                                <div class="table-responsive">
                                    <asp:GridView ID="grdCargaEstufada" DataKeyNames="autonum_sc" CssClass="table table-hover table-sm grdViewTable" GridLines="None" CellSpacing="-1" Font-Size="12px" runat="server" AutoGenerateColumns="false"  OnRowDataBound="grdCargaEstufada_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="num_nf" HeaderText="NF" />
                                            <asp:BoundField DataField="QTDE_SAIDA" HeaderText="Qtde." />
                                            <asp:BoundField DataField="reference" HeaderText="Reserva" />
                                            <asp:BoundField DataField="ID_CONTEINER" HeaderText="Id Contêiner" />
                                            <asp:BoundField DataField="DESCRICAO_EMB" HeaderText="Embalagem" />
                                            <asp:BoundField DataField="MERCADORIA" HeaderText="Mercadoria" />
                                            <asp:BoundField DataField="PESO_BRUTO" HeaderText="Peso" />
                                            <asp:BoundField DataField="VOLUME" HeaderText="Volume" />
                                            <asp:BoundField DataField="RAZAO" HeaderText="Cliente" />
                                            <asp:BoundField DataField="NAVIO_VIAGEM" HeaderText="Navio / Viagem" />
                                            <asp:BoundField DataField="COD_PRODUTO" HeaderText="Cód. Barra" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <a href="#" class="btn btn-danger btn-sm" onclick="descarga('<%# Eval("AUTONUM_NFI") %>', '<%# Eval("AUTONUM_SC") %>','<%# Eval("AUTONUM_PATIO") %>','<%# Eval("QTDE_SAIDA") %>','<%# Eval("COD_PRODUTO") %>',this)" data-toggle="modal" role="button" data-target="#modal-exclusao" title="Descarga"><i class="fa fa-undo"></i>&nbsp Descarga</a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="campo-acao" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="headerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row invisivel">
                            <div class="form-group col-sm-4 col-sm-offset-8">
                                <div class="form-group">
                                    <asp:LinkButton ID="btSairOLD" Width="100%" runat="server" CssClass="btn btn-success"><span aria-hidden="true" class="fa fa-home"></span>&nbsp;&nbsp;Sair</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-exclusao" tabindex="-1" role="dialog" aria-labelledby="modal-exclusao-label" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <strong>Atenção:</strong> Confirma a Descarga do Registro?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                        <button type="button" class="btn btn-danger" onclick="confirmarExclusao()">Confirmar Descarga</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-talie-finalizado" tabindex="-1" role="dialog" aria-labelledby="modal-talie-finalizado-label" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <strong>Atenção:</strong> Estufagem já finalizada <br/>
                        Deseja Cancelar?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary"  data-dismiss="modal">Não</button>
                        <button type="button" class="btn btn-danger" onclick="GerarCancelamento()">Sim</button>
                    </div>
                </div>
            </div>
        </div>
         <div class="modal fade" id="modal-confirma-fechamento" tabindex="-1" role="dialog" aria-labelledby="modal-confirma-fechamento-label" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <strong>Confirma o fechamento?</strong><br/>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary"  data-dismiss="modal">Não</button>
                        <button type="button" class="btn btn-danger" onclick="ConfirmarFechamento()">Sim</button>
                    </div>
                </div>
            </div>
        </div>
        <table>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 45%;">&nbsp;</td>
            </tr>
        </table>
        <p>
            &nbsp;
        </p>

    </form>

<%--    <script src="lib/jquery-1.12.4.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/toastr.js"></script>
    <script src="Scripts/select2.min.js"></script>
    <script src="Content/plugins/datatables/js/jquery.dataTables.min.js"></script>
    <script src="Content/plugins/datatables/js/dataTables.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables/js/datetime-moment.js"></script>--%>

        <script src="lib/jquery-1.12.4.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/toastr.js"></script>
    <script src="Scripts/select2.min.js"></script>

    <script src="Content/plugins/datatables/js/jquery.dataTables.min.js"></script>

    <script>
        $(document).ready(function () {
            $('#<%= grdCargaEstufada.ClientID %>').DataTable({
                "bLengthChange": false,
                "bFilter": true,
                "language": {
                    "url": "Scripts/pt-br.json"
                }
            });
        });

        function descarga(autonumNFI, sc, patio, qtdeSaida, codProduto, row) {
            var rowData = row.parentNode.parentNode;
            $('#modal-exclusao').data('id', autonumNFI);
            $('#hddnAutonumNFI').val(autonumNFI);
            $('#hddnSC').val(sc);
            $('#hddnPatio').val(patio);
            $('#hddnQtdeSaida').val(qtdeSaida);
            $('#hddnCodProduto').val(codProduto);
            $('#hddnRowIndex').val(rowData.rowIndex);
        }

        function confirmarExclusao() {
            var _autonumNFI = $('#hddnAutonumNFI').val();
            var _sc = $('#hddnSC').val();
            var _patio = $('#hddnPatio').val();
            var _qtdeSaida = $('#hddnQtdeSaida').val();
            var _codProduto = $('#hddnCodProduto').val();
            var _conteiner = $('#txtConteiner').val();

            $.ajax({
                type: "POST",
                url: "Estufagem.aspx/Descarga",
                data: '{ autonumNFI: "' + _autonumNFI + '", sc:"' + _sc + '", patio:"' + _patio + '", qtdeSaida: "' + _qtdeSaida + '", codProduto: "' + _codProduto + '"  }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {

                    toastr.success('Registro descarregado com sucesso!', 'Coletor');

                    $('#modal-exclusao')
                        .data('id', '0')
                        .modal('hide');

                    var $rows = $("#grdCargaEstufada tr");
                    linha = $('#hddnRowIndex').val();
                    $rows.eq(linha).remove();

                    setTimeout(function () {
                        //location.reload();
                       
                        window.location.href = 'Estufagem.aspx?cntr=' + _conteiner;

                    }, 1000);

                },
                error: function (jqXHR, status, err) {
                    $('#modal-exclusao').modal('hide');
                    if (jqXHR.statusText === 'Internal Server Error') {
                        if (jqXHR.responseJSON) {
                            toastr.error(jqXHR.responseJSON.Message, 'Coletor');
                            return;
                        }
                    }
                    toastr.error(jqXHR.responseText, 'Coletor');
                }
            });
        }

        function FinalizarTalie() {
            console.log('Finalizar');

            if ($('#lblTalieNumero').val() === '') {
                toastr.error('Finalize somente após o lançamento do talie', 'Coletor');
                return;
            }

            if (ValidarFechamento() === 0) {
                toastr.error('Não foi encontrado lancamento de carga para o talie selecionado');
                return;
            }

            $('#modal-fechamento').data('id', $('#lblTalieNumero').val());
            $('#modal-fechamento').modal('show');

        }

        function ValidarFechamento() {
            var _talie = $('#hddnAutonumTalie').val();
            var _autonumRomaneio = $('#hddnAutonumRomaneio').val();
            var _autonumPatio = $('#hddnAutonumPatio').val();
            var _autonumBOO = $('#hddnAutonumBOO').val();

            $.ajax({
                type: "POST",
                url: "Estufagem.aspx/ValidarFechamento",
                data: '{ talie: ' + _talie + ', autonumRomaneio:'+_autonumRomaneio+', autonumPatio: '+ _autonumPatio+', autonumBOO:'+_autonumBOO+' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d === 0) {
                        ValidarFechamentoParte2()
                    } else {
                        $('#modal-talie-finalizado').data('id', _talie);
                        $('#modal-talie-finalizado').modal('show');


                    }
                    
                },
                error: function (jqXHR, status, err) {
                    if (jqXHR.statusText === 'Internal Server Error') {
                        if (jqXHR.responseJSON) {
                            toastr.error(jqXHR.responseJSON.Message, 'Coletor');
                            return;
                        }
                    }
                    toastr.error(jqXHR.responseText, 'Coletor');
                }
            });
        }

        function ValidarFechamentoParte2() {
            var _talie = $('#hddnAutonumTalie').val();
            var _autonumRomaneio = $('#hddnAutonumRomaneio').val();
            var _autonumPatio = $('#hddnAutonumPatio').val();
            var _autonumBOO = $('#hddnAutonumBOO').val();

            $.ajax({
                type: "POST",
                url: "Estufagem.aspx/ValidarFechamentoParte2",
                data: '{ talie: ' + _talie + ', autonumRomaneio:' + _autonumRomaneio + ', autonumPatio: ' + _autonumPatio + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#modal-confirma-fechamento').data('id', _talie);
                    $('#modal-confirma-fechamento').modal('show');

                },
                error: function (jqXHR, status, err) {
                    if (jqXHR.statusText === 'Internal Server Error') {
                        if (jqXHR.responseJSON) {
                            toastr.error(jqXHR.responseJSON.Message, 'Coletor');
                            return;
                        }
                    }
                    toastr.error(jqXHR.responseText, 'Coletor');
                }
            });
        }

        function ConfirmarFechamento() {
            var _talie = $('#hddnAutonumTalie').val();
            var _autonumRomaneio = $('#hddnAutonumRomaneio').val();
            var _autonumPatio = $('#hddnAutonumPatio').val();
            var _autonumBOO = $('#hddnAutonumBOO').val();
            var _conteiner = $('#txtConteiner').val();

            $.ajax({
                type: "POST",
                url: "Estufagem.aspx/ConfirmaFechamento",
                data: '{ talie: ' + _talie + ', autonumRomaneio:' + _autonumRomaneio + ', autonumPatio: ' + _autonumPatio + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                   
                    toastr.success(response.d, 'Coletor');
                    $('#modal-fechamento')
                        .data('id', '0')
                        .modal('hide');

                    setTimeout(function () {
                        //location.reload();
                        window.location.href = 'Estufagem.aspx?cntr=' + _conteiner;

                    }, 1500);
                },
                error: function (jqXHR, status, err) {
                    if (jqXHR.statusText === 'Internal Server Error') {
                        if (jqXHR.responseJSON) {
                            toastr.error(jqXHR.responseJSON.Message, 'Coletor');
                            return;
                        }
                    }
                    toastr.error(jqXHR.responseText, 'Coletor');
                }
            });
        }

        function GerarCancelamento() {
            var _talie = $('#hddnAutonumTalie').val();
            var _autonumRomaneio = $('#hddnAutonumRomaneio').val();
            var _autonumPatio = $('#hddnAutonumPatio').val();
            var _autonumBOO = $('#hddnAutonumBOO').val();
            var _conteiner = $('#txtConteiner').val();

            $.ajax({
                type: "POST",
                url: "Estufagem.aspx/GerarCancelamento",
                data: '{ talie: ' + _talie + ', autonumRomaneio:' + _autonumRomaneio + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    toastr.success(data.d, 'Coletor');

                    $('#modal-talie-finalizado')
                        .data('id', '0')
                        .modal('hide');

                    setTimeout(function () {
                        //location.reload();
                        window.location.href = 'Estufagem.aspx?cntr=' + _conteiner;

                    }, 1500);

                },
                error: function (jqXHR, status, err) {
                    if (jqXHR.statusText === 'Internal Server Error') {
                        if (jqXHR.responseJSON) {
                            toastr.error(jqXHR.responseJSON.Message, 'Coletor');
                            return;
                        }
                    }
                    toastr.error(jqXHR.responseText, 'Coletor');
                }
            });
        }
        
    </script>
</body>
</html>
