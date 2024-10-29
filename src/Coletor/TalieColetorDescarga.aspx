cbRegistro<%@ Page Language="vb" AutoEventWireup="false" EnableEventValidation="false" Inherits="Band.Coletor.Redex.Consultas.TalieColetorDescarga" CodeBehind="TalieColetorDescarga.aspx.vb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="icon" href="favicon.ico" />
    <title></title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/fontawesome-all.css" rel="stylesheet" />
    <link href="Content/toastr.css" rel="stylesheet" />
    <link href="Content/css/estilos.css" rel="stylesheet" />
    <link href="Content/site.css" rel="stylesheet" />
    <link href="Content/css/select2.css" rel="stylesheet" />
    <link href="Content/css/jquery.dataTables.min.css" rel="stylesheet" />



    <style type="text/css">
        .separador {
            border-top: 3px double #8c8b8b;
            margin: 3px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server" class="auto-style10">
        <header>
            <nav class="navbar navbar-expand-lg navbar-light bg-light">
                <asp:LinkButton ID="btnHome" runat="server" class="navbar-brand">
                <img src="Content/img/logo-coletor.png" class="d-inline-block align-top" alt=""/>
                </asp:LinkButton>
                <asp:LinkButton ID="btnLogout" runat="server" class="nav-link"><i class="fa fa-power-off mr-3"></i>Sair</asp:LinkButton>
            </nav>
        </header>

        <asp:HiddenField runat="server" ID="hddnTalieId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnTalieItensSelIdx" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnNFItensSelIdx" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnConteinerId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnGateId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnReserva" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnNFId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnCrossDocking" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnNFItemId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnNovoNFItem" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnEmbalagemId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnProdutoId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnClienteId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnReservaId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnTalieItemId" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnTipoDescarga" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnRowIndex" Value="" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hddnOrigem" Value="" ClientIDMode="Static" />

        <br />
        <div class="row mt-3">
            <div class="col-sm-12 col-lg-8 offset-lg-2">
                <div class="card">
                    <div class="card-header">
                        <i class="fas fa-truck fa-lg"></i>Descarga para Armazém
                        <div class=" float-right">
                            <div class="btn-group">
                                <asp:LinkButton ID="btSair" OnClick="btSair_Click" runat="server" class="btn btn-link"><i class="fa fa-home"></i>&nbsp;&nbsp;Menu</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="pull-left">
                            <ul class="nav nav-tabs" role="tablist">
                                <li class="nav-item">
                                    <a href="#descarga" class="nav-link active" data-toggle="tab"><i class="fa fa-edit" style="padding-right: 8px;"></i>Descarga</a></li>
                                <li class="nav-item">
                                    <a href="#talies" class="nav-link" data-toggle="tab">
                                        <i class="fa fa-search" style="padding-right: 8px;"></i>Talies
                                    </a>
                                </li>
                            </ul>
                        </div>


                        <div class="tab-content">
                            <div class="tab-pane fade show active" id="descarga">
                                <br />
                                 <div class="row">

                        <div class="col-sm-12">
                            <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                            <% if (ViewState("Sucesso") IsNot Nothing) Then

                                    If (ViewState("Sucesso") = True) Then
                            %>
                            <div class="alert alert-success">
                                <asp:label runat="server" id="lblMensagem"></asp:label>
                            </div>

                            <%      End If%>
                            <% End If%>
                        </div>
                    </div>

                                <div class="row">

                                    <div class="form-group col-sm-3">
                                        <label for="cbRegistro">Registro:</label>
                                        <asp:DropDownList runat="server" ID="cbRegistro" TabIndex="3" AutoPostBack="true" CssClass="form-control selecionar" data-placeholder="Insira o Registro" />
                                    </div>
                                    <div class="form-group col-sm-3">

                                        <label for="lblTalieNumero">Talie:</label>
                                        <asp:TextBox ID="lblTalieNumero" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label class="control-label">&nbsp;</label>
                                            <div class="checkbox text-left">
                                                <asp:CheckBox ID="chkTalieFechado" runat="server" CssClass="disabled" Enabled="false" AutoPostBack="True" Text="Talie Fechado?" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel runat="server" ID="pnlCrossDocking" />

                                    <div class="col-sm-3 <% IIf(pnlCrossDocking.Visible, "", "invisivel")  %>  ">
                                        <div class="form-group">
                                            <label class="control-label">&nbsp;</label>
                                            <div class="checkbox text-left">
                                                <asp:CheckBox ID="chkCrossdocking" runat="server" AutoPostBack="True" Text="Crossdocking?" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-sm-3">

                                        <label for="txtInicio">Início:</label>
                                        <asp:TextBox ID="txtInicio" ReadOnly="True" runat="server" CssClass="form-control" AutoPostBack="True" MaxLength="12"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-sm-3">

                                        <label for="txtFIM">Término:</label>
                                        <asp:TextBox ID="txtFIM" ReadOnly="True" runat="server" CssClass="form-control" AutoPostBack="True" MaxLength="12"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <label for="cbCliente">Cliente:</label>
                                        <asp:TextBox ID="txtCliente" runat="server" ReadOnly="true" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-sm-3">

                                        <label for="txtPlaca">Placa:</label>
                                        <asp:TextBox ID="txtPlaca" ReadOnly="True" runat="server" CssClass="form-control" AutoPostBack="True" MaxLength="12"></asp:TextBox>
                                    </div>
                                    <asp:Panel runat="server" ID="pnlConteiner" />
                                    <% if pnlConteiner.Visible Then %>
                                    <div class="form-group col-sm-3 ">
                                        <label for="txtConteiner">Contêiner:</label>
                                        <asp:TextBox ID="txtConteiner" runat="server" CssClass="form-control" AutoPostBack="True" MaxLength="12"></asp:TextBox>
                                    </div>
                                    <% end If %>

                                    <div class="form-group col-sm-3">
                                        <label for="cbReserva">Reserva:</label>
                                        <asp:TextBox ID="txtReserva" runat="server" ReadOnly="true" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-sm-3">
                                        <label for="cbConferente">Conferente:</label>
                                        <asp:DropDownList runat="server" ID="cbConferente" TabIndex="3" AutoPostBack="true" CssClass="form-control" />
                                    </div>
                                    <div class="form-group col-sm-3">
                                        <label for="cbConferente">Equipe:</label>
                                        <asp:DropDownList runat="server" ID="cbEquipe" TabIndex="3" AutoPostBack="true" CssClass="form-control" />
                                    </div>
                                    <div class="form-group col-sm-3">
                                        <label for="cbModo">Modo:</label>
                                        <asp:DropDownList runat="server" ID="cbModo" TabIndex="3" AutoPostBack="true" CssClass="form-control" />
                                    </div>
                                    <div class="form-group col-sm-3">

                                        <label class="control-label">&nbsp;</label>
                                        <div class="checkbox text-left">
                                            <asp:CheckBox ID="chkEstufagemCompleta" runat="server" CssClass="chk-r" Text="ESTUFAGEM COMPLETA" />
                                        </div>

                                    </div>

                                </div>

                                <div class="row mt-5">
                                    <asp:Panel ID="pnlLimpar" runat="server">
                                            <asp:LinkButton ID="btnTalieCancelar" Width="100%" runat="server" CssClass="btn btn-warning btn-block" Enable="false"><i class="fa fa-eraser"></i>&nbsp;&nbsp;Limpar</asp:LinkButton>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlGravar" runat="server" CssClass="form-group col-xs-12 col-md-2 col-lg-2">
                                            <asp:LinkButton ID="btnTalieGravar" Width="100%" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Gravar</asp:LinkButton>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlFinalizar" Visible="false" runat="server" CssClass="form-group col-xs-12 col-md-2 col-lg-2 " style="left: 0px; top: 0px; height: 35px">
                                            <button id="btnTalieFinalizar" type="button" onclick="FinalizarTalie()" style="width: 100%" class="btn btn-success"><span aria-hidden="true" class="fa fa-truck"></span>&nbsp;&nbsp;Finalizar</button>
                                    </asp:Panel>
                                </div>

                                <asp:Panel ID="pnlEdicaoItem" runat="server" Visible="false">
                                    <br />
                                    <hr class="separador" />
                                    <div class="col-sm-12 text-center">
                                        <h4>
                                            <label class="control-label">ITEM</label></h4>
                                    </div>
                                    <hr class="separador mb-5" />
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="cbNF">NF:</label>
                                                <asp:DropDownList runat="server" ID="cbNF" TabIndex="3" AutoPostBack="true" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtEmbalagem">Embalagem:</label>
                                                <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="cbProduto">Produto:</label>
                                                <asp:TextBox ID="txtProduto" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtQtdeNF">Qtde. NF</label>
                                                <asp:TextBox ID="txtQtdeNF" runat="server" Enabled="false" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtQtdeDescarga">Qtde. Descarga</label>
                                                <asp:TextBox ID="txtQtdeDescarga" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtComprimento">Comprimento</label>
                                                <asp:TextBox ID="txtComprimento" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtLargura">Largura</label>
                                                <asp:TextBox ID="txtLargura" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtAltura">Altura</label>
                                                <asp:TextBox ID="txtAltura" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
<%--                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtComprimento">Comprimento</label>
                                                <asp:TextBox ID="txtComprimento" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>--%>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txtPeso">Peso</label>
                                                <asp:TextBox ID="txtPeso" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
<%--                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="cbLocal">Local:</label>
                                                <asp:DropDownList runat="server" ID="cbLocal" TabIndex="3" AutoPostBack="true" CssClass="form-control" />
                                            </div>
                                        </div>--%>
                                        <asp:Panel runat="server" ID="itemLocalArmazem" />
                                        <div class="col-sm-3 <% IIf(itemLocalArmazem.Visible, "", "invisivel") %> ">
                                            <div class="form-group">
                                                <label for="txtLocal">Ocupação:</label>
                                                <asp:TextBox ID="txtLocal" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-3 col-sm-offset-6">
                                            <div class="form-group">
                                                <asp:LinkButton ID="btnItemCancelar" OnClick="btnDescargaCancelar_Click1" Width="100%" runat="server" CssClass="btn btn-warning"><span aria-hidden="true" class="fa fa-eraser"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <asp:LinkButton ID="btnItemGravar" OnClick="btnItemGravar_Click" Width="100%" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-save"></span>&nbsp;&nbsp;Gravar</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <hr class="separador" />
                                    <br />
                                </asp:Panel>
                                <br />
                                <div class="row ">
                                    <div class="col-sm-12 mt-1">
                                        <div class="table-responsive">
                                            <asp:GridView ID="grdItens" ClientIDMode="Static" runat="server" DataKeyNames="AUTONUM_TI" CssClass="table table-hover table-sm grdViewTable" GridLines="None" AutoGenerateColumns="False" Font-Size="12px" CellSpacing="-1" OnRowCommand="grdItens_RowCommand" OnRowDataBound="grdItens_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="NUM_NF" HeaderText="NF" />
                                                    <asp:BoundField DataField="SERIE_NF" HeaderText="Série" />
                                                    <asp:BoundField DataField="ITEM" HeaderText="Item" />
                                                    <asp:BoundField DataField="DESCRICAO_EMB" HeaderText="Embalagem" />
                                                    <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Produto" />
                                                    <asp:BoundField DataField="QTDE" HeaderText="Qtde. NF" />
                                                    <asp:BoundField DataField="QTDE_DESCARGA" HeaderText="Qtde. Descarga" />
                                                    <asp:BoundField DataField="LARGURA" HeaderText="Largura" />
                                                    <asp:BoundField DataField="ALTURA" HeaderText="Altura" />
                                                    <asp:BoundField DataField="COMPRIMENTO" HeaderText="Comprimento" />
                                                    <asp:BoundField DataField="PESO" HeaderText="Peso" />
                                                    <asp:BoundField DataField="ARMAZEM" HeaderText="Local" />
                                                    <asp:BoundField DataField="YARD" HeaderText="Yard" />
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="cmdEditar" CssClass="btn btn-sm btn-info btn-block" CommandArgument="<%# Container.DataItemIndex %>" CommandName="EDITAR">
                                                                    <i class='fa fa-edit'></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="campo-acao" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <a href="#" class="btn btn-danger btn-sm" onclick="excluirItem('<%# Eval("AUTONUM_TI") %>',this)" data-toggle="modal" role="button" data-target="#modal-exclusao" title="Excluir"><i class="fa fa-trash"></i></a>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="campo-acao" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="headerStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>

                            </div>



                            <div class="tab-pane fade" id="talies">
                                <div class="row">

                                        <div id="FiltrarTalie" class="invisivel" runat="server" >
                                         
                                            <div class="col-xs-12 col-md-3 col-lg-3">
                                                <div class="form-group">
                                                    <label for="txtFiltroRegistro">Registro</label>
                                                    <asp:TextBox ID="txtFiltroRegistro" runat="server" CssClass="form-control" ></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-xs-12 col-md-3 col-lg-3">
                                                <div class="form-group">
                                                    <label for="txtFiltroTalie">Talie</label>
                                                    <asp:TextBox ID="txtFiltroTalie" runat="server" CssClass="form-control" ></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-xs-12 col-md-3 col-lg-3 "  >
                                                <div class="form-group">
                                                    <label>&nbsp</label>
                                                    <asp:LinkButton ID="btnPesquisarTalie" ClientIDMode="Static" OnClick="btnPesquisarTalie_Click"  Width="100%" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="fa fa-search"></span>&nbsp;&nbsp;Pesquisar</asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="col-xs-12 col-md-3 col-lg-3 ">
                                             <div class="form-group">
                                                   <label>&nbsp</label>
                                                    
                                                 <a href="#" onclick="CancelarFiltroTalie()" style="width:100%" class="btn btn-warning" id="btnCancelarFiltro">
                                                     <span aria-hidden="true" class="fas fa-times"></span>&nbsp;&nbsp;Cancelar Filtro
                                                 </a>
                                                </div>
                                                </div>
                                        </div>
                                    </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="grdTalie" DataKeyNames="Id,AUTONUM_REG" CssClass="table table-hover table-sm grdViewTable data-table-grid" GridLines="None" CellSpacing="-1" runat="server" OnRowCommand="grdTalie_RowCommand" OnRowDataBound="grdTalie_RowDataBound" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="autonum_reg" HeaderText="Lote" />
                                            <asp:BoundField DataField="Id" HeaderText="Talie" />
                                            <asp:BoundField DataField="INICIO" HeaderText="Início" />
                                            <asp:BoundField DataField="PLACA" HeaderText="Placa" />
                                            <asp:BoundField DataField="ID_CONTEINER" HeaderText="Contêiner" />
                                            <asp:BoundField DataField="REFERENCE" HeaderText="Reserva" />
                                            <asp:BoundField DataField="instrucao" HeaderText="Instrução" />
                                            <asp:BoundField DataField="Fantasia" HeaderText="Cliente" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="cmdEditarTalie" CssClass="btn btn-sm btn-info btn-block" CommandArgument="<%# Container.DataItemIndex %>" CommandName="EDITAR">
                                                                    <i class='fa fa-edit'></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="campo-acao" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <a href="#" class="btn btn-danger btn-sm" onclick="excluirTalie('<%# Eval("Id") %>')" data-toggle="modal" role="button" data-target="#modal-exclusao" title="Excluir"><i class="fa fa-trash"></i></a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="campo-acao" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="headerStyle" />
                                    </asp:GridView>
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
                        <strong>Atenção:</strong> Confirma a exclusão do <span id="origem"></span>?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                        <button type="button" class="btn btn-danger" onclick="confirmarExclusao()">Confirmar Exclusão</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-fechamento" tabindex="-1" role="dialog" aria-labelledby="modal-fechamento-label" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <strong>Atenção:</strong> Confirma o fechamento deste talie e transferência da carga para o estoque?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                        <button id="btnConfirmatFechamento" type="button" class="btn btn-danger" onclick="ConfirmarFechamento()">Confirmar Fechamento</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-verifica-etiquetas" tabindex="-1" role="dialog" aria-labelledby="modal-verifica-etiquetas-label" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <strong>Atenção:</strong> Não Consta geração de etiquetas deste registro. <br/>
                        Deseja continuar assim mesmo?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" onclick="MensagemOperacaoCancelada()" data-dismiss="modal">Não</button>
                        <button id="btnAtualizarAlertaEtiqueta1" type="button" class="btn btn-danger" onclick="AtualizarAlertaEtiqueta1()">Sim</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-verifica-pendencias" tabindex="-1" role="dialog" aria-labelledby="modal-verifica-pendencias-label" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <strong>Atenção:</strong> Consta pendencia de emissão de etiquetas deste registro.<br/>
                        Deseja continuar assim mesmo?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" onclick="MensagemOperacaoCancelada()" >Não</button>
                        <button id="btnVerificaPendenciaEmisssoEtiquetas" type="button" class="btn btn-danger" onclick="VerificaPendenciaEmisssoEtiquetas()">Sim</button>
                    </div>
                </div>
            </div>
        </div>




        <asp:Button ID="btnConsultarTalies" runat="server" OnClick="btnConsultarTalies_Click" style="display:none" />
        <asp:HiddenField ID="TabName" runat="server" />
    </form>

    <script src="lib/jquery-1.12.4.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/toastr.js"></script>
    <script src="Scripts/select2.min.js"></script>

    <script src="Content/plugins/datatables/js/jquery.dataTables.min.js"></script>
<%--    <script src="Content/plugins/datatables/js/dataTables.bootstrap4.min.js"></script>
    <script src="Content/plugins/datatables/js/datetime-moment.js"></script>--%>

    <script>
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "personal";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
        $(document).ready(function () {

            <%--$('#<%= grdTalie.ClientID %>').DataTable({
                "drawCallback": function (settings) {
                    AddBotaoFiltrar();
                },
                "dom": '<"botao-filtrar">frtip',
                "bLengthChange": false,
                "bFilter": true,
                "language": {
                    "url": "Scripts/pt-br.json"
                },
                "order": [],
            });--%>

            $('#<%= grdTalie.ClientID %>').DataTable({
                "dom": '<"botao-filtrar">frtip',
                "bLengthChange": false,
                "bFilter": true,
                "language": {
                    "url": "Scripts/pt-br.json"
                },
                "order": [],
            });

            $('#<%= grdItens.ClientID %>').DataTable({
                "bLengthChange": false,
                "bFilter": true,
                "language": {
                    "url": "Scripts/pt-br.json"
                }
            });

            $('.selecionar').select2({
                language: 'pt-BR',
                width: 'resolve'
            });

            
        });



        function AddBotaoFiltrar() {
            var r = $('<button id="btnFiltrar"  onclick="FiltrarTalie();return false;" class="btn btn-primary"><span aria-hidden="true" class="fa fa-search"></span>&nbsp;&nbsp;Filtrar</button>');

            var filtrar = $('<div class="col-xs-12 col-md-3 col-lg-3 "><div class= "form-group"><button id="btnFiltrar"  onclick="FiltrarTalie();return false;" style="width:100%" class="btn btn-primary"><span aria-hidden="true" class="fas fa-filter"></span>&nbsp;&nbsp;Filtrar</button></div></div>')
            var limparFiltro = $('<div class="col-xs-12 col-md-3 col-lg-3 "><div class= "form-group"><button id="btnFiltrar"  onclick="LimparFiltroTalie();return false;" style="width:100%" class="btn btn-warning"><span aria-hidden="true" class="fa fa-eraser"></span>&nbsp;&nbsp;Limpar Filtro</button></div></div>')
            
            if ($("#txtFiltroRegistro").val() !== '' || $("#txtFiltroTalie").val() !== '') {
                $(".botao-filtrar").append(limparFiltro)
            } else {
                $(".botao-filtrar").append(filtrar)
            }

            $(".botao-filtrar").css("display", "inline");
        }

        function excluirItem(id, row) {
            var rowData = row.parentNode.parentNode;
            $('#modal-exclusao').data('id', id);
            $('#modal-exclusao').data('data-origem', 'item');
            $('#hddnOrigem').val('item');
            $('#origem').text('item');

            $('#hddnRowIndex').val(rowData.rowIndex);
        }

        function excluirTalie(id) {
            //var id = $('#lblTalieNumero').text()
            $('#modal-exclusao').data('id', id);
            $('#modal-exclusao').data('data-origem', 'talie');
            $('#hddnOrigem').val('talie');
            $('#origem').text('talie');

        }

        function excluirTalieOLD(id, row) {
            var rowData = row.parentNode.parentNode;
            $('#modal-exclusao').data('id', id);
            $('#modal-exclusao').data('data-origem', 'talie');
            $('#hddnOrigem').val('talie');
            $('#origem').text('talie');
            $('#hddnRowIndex').val(rowData.rowIndex);
        }

        function confirmarExclusao() {

            var _origem = $('#modal-exclusao').data('data-origem');

            if (_origem === 'item') {
                confirmarExclusaoItem()
            }

            if (_origem === 'talie') {
                confirmarExclusaoTalie()
            }
        }

        function confirmarExclusaoItem() {
            var _id = $('#modal-exclusao').data('id');
            $.ajax({
                type: "POST",
                url: "TalieColetorDescarga.aspx/ExcluirItem",
                data: '{ id: "' + _id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {

                    toastr.success('Item excluído com sucesso!', 'Coletor');

                    $('#modal-exclusao')
                        .data('id', '0')
                        .modal('hide');

                    var $rows = $("#grdItens tr");
                    linha = $('#hddnRowIndex').val();
                    $rows.eq(linha).hide();

                },
                error: function (response) {
                    toastr.error('O item não pode ser excluído', 'Coletor');
                }
            });
        }

        function confirmarExclusaoTalie() {
            var _id = $('#modal-exclusao').data('id');
            $.ajax({
                type: "POST",
                url: "TalieColetorDescarga.aspx/ExcluirTalie",
                data: '{ id: "' + _id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    toastr.success(data.d, 'Coletor');

                    $('#modal-exclusao')
                        .data('id', '0')
                        .modal('hide');

                    //$('#linha-' + _id).remove();

                    setTimeout(function () {
                        location.reload();
                    }, 1000);

                },
                error: function (response) {
                    toastr.error(response.d, 'Coletor');
                }
            });
        }

        function exibirFiltro() {
            $('#panelFiltro').removeClass('invisivel');
        }

        function FinalizarTalie() {
            console.log('Finalizar');

            if ($('#lblTalieNumero').val() === '') {
                //toastr.error('Selecione um talie existente para fechamento', 'Coletor');
                toastr.error('Finalize somente após o lançamento do talie', 'Coletor');
                return;
            }

            if (ObterTotalItensPorTalie() === 0) {
                toastr.error('Não foi encontrado lancamento de carga para o talie selecionado');
                return;
            }

            $('#modal-fechamento').data('id', $('#lblTalieNumero').val());
            $('#modal-fechamento').modal('show');

        }

        function ObterTotalItensPorTalie() {
            var _id = $('#lblTalieNumero').val();
            
            $.ajax({
                type: "POST",
                url: "TalieColetorDescarga.aspx/ObterTotalItensPorTalie",
                data: '{ id: "' + _id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    return data;

                },
                error: function (response) {
                    toastr.error('Erro ao obter dados dos itens do talie ' + _id + ' ' + response.d, 'Coletor');
                }
            });
        }

        var etiquetas = 0;
        var pendencias = 0;
        var totalItens = 0;
        function ConfirmarFechamento() {
            var _id = $('#modal-fechamento').data('id');
            console.log('Fechamento');

            if ($("#chkTalieFechado").is(':checked')) {
                toastr.error('Talie já está fechado - Transferência de carga para estoque cancelada');
            }

            var estufagemCompleta = 0;
            if ($("#chkEstufagemCompleta").is(':checked')) {
                estufagemCompleta = 1;
            }


            var _usuarioId = '<%=Session("AUTONUMUSUARIO")%>';

            $('#btnConfirmatFechamento')
                .html('<i class="fa fa-spinner fa-spin"></i>')
                .addClass('disabled');

            $.ajax({
                type: "POST",
                url: "TalieColetorDescarga.aspx/ValidarFecharTalie",
                data: '{ idTalie: "' + _id + '", tipoDescarga: "' + $('#hddnTipoDescarga').val() + '", conteinerId: "' + $('#hddnConteinerId').val() + '", conteiner: "' + $('#txtConteiner').val() + '", usuarioId:"'+_usuarioId+'" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    etiquetas = 0;
                    pendencias = 0;
                    totalItens = 0;

                    if (response != null && response.d != null) {
                        var data = response.d;
                        data = $.parseJSON(data);
                        etiquetas = data.Etiquetas;
                        pendencias = data.Pendencias;
                        totalItens = data.TotalItens;

                        console.log(etiquetas);
                        console.log(pendencias);
                        console.log(totalItens);
                     
                        if ('<%=hddnTipoDescarga.Value%>' === '<%=Band.Coletor.Redex.Consultas.OpcoesDescarga.DA.ToString()%>') {

                            if (etiquetas === 0) {
                                $('#modal-fechamento').modal('hide');
                                $('#modal-verifica-etiquetas').modal('show');
                            } else if (pendencias !== 0) {
                                $('#modal-fechamento').modal('hide');
                                $('#modal-verifica-pendencias').modal('show');
                            } else {
                                FinalizarTalieAposValidacoes();
                            }
                            
                        } else {
                            FinalizarTalieAposValidacoes();
                        }

                    }
                },
                error: function (jqXHR, status, err) {
                    if (jqXHR.statusText === 'Internal Server Error') {
                        if (jqXHR.responseJSON) {
                            toastr.error(jqXHR.responseJSON.Message, 'Coletor');
                            return;
                        }
                    }
                    
                    console.log(jqXHR);
                    toastr.error(jqXHR.responseText, 'Coletor');
                }
            }).always(function () {
                $('#btnConfirmatFechamento')
                    .html('<i class="fa fa-spinner fa-spin"></i>')
                    .removeClass('disabled');
            });
        }

        var atualizarAlertaEtiqueta1 = 0;
        var atualizarAlertaEtiqueta2 = 0;

        function AtualizarAlertaEtiqueta1() {
            atualizarAlertaEtiqueta1 = 1;

            $('#btnAtualizarAlertaEtiqueta1')
                .html('<i class="fa fa-spinner fa-spin"></i>')
                .addClass('disabled');

            if (pendencias !== 0) {
                $('#modal-verifica-pendencias').modal('show');
            } else {
                FinalizarTalieAposValidacoes();
            }
        }
        function VerificaPendenciaEmisssoEtiquetas() {
            $('#btnVerificaPendenciaEmisssoEtiquetas')
                .html('<i class="fa fa-spinner fa-spin"></i>')
                .addClass('disabled');
            atualizarAlertaEtiqueta2 = 1;
            FinalizarTalieAposValidacoes();
        }

        function MensagemOperacaoCancelada() {
            toastr.error('Operação Cancelada', 'Coletor');
            $('#modal-verifica-etiquetas').modal('hide');
            $('#modal-verifica-pendencias').modal('hide')
            FinalizarTalieAposValidacoes();
        }

        function FinalizarTalieAposValidacoes() {
            var _id = $('#modal-fechamento').data('id');
            console.log('Fechamento');

            if ($("#chkTalieFechado").is(':checked')) {
                toastr.error('Talie já está fechado - Transferência de carga para estoque cancelada');
            }

            var estufagemCompleta = 0;
            if ($("#chkEstufagemCompleta").is(':checked')) {
                estufagemCompleta = 1;
            }

            var _usuarioId = '<%=Session("AUTONUMUSUARIO")%>';
            //data: '{ idTalie: "' + _id + '", tipoDescarga: "' + $('#hddnTipoDescarga').val() + '", conteinerId: "' + $('#hddnConteinerId').val() + '", conteiner: "' + $('#txtConteiner').val() + '", usuarioId:"' + _usuarioId + '" }',

            $.ajax({
                type: "POST",
                url: "TalieColetorDescarga.aspx/FinalizarTalie",
                data: '{ idTalie: "' + _id + '", inicio: "' + $('#txtInicio').val() + '", estufagemCompleta: "' + estufagemCompleta + '", tipoDescarga: "' + $('#hddnTipoDescarga').val() + '", conteinerId: "' + $('#hddnConteinerId').val() + '", conteiner: "' + $('#txtConteiner').val() + '", usuarioId:"' + _usuarioId + '", atualizarAlertaEtiqueta1:' + atualizarAlertaEtiqueta1 + ', atualizarAlertaEtiqueta2:' + atualizarAlertaEtiqueta2 + ', etiquetas: ' + etiquetas + ', pendencias: ' + pendencias + ', totalItens: ' + totalItens+' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    toastr.success(response.d, 'Coletor');
                    $('#modal-fechamento')
                        .data('id', '0')
                        .modal('hide');

                    setTimeout(function () {
                        //location.reload();
                        var registro = $('#cbRegistro').val();
                        if ('<%=hddnTipoDescarga.Value%>' === '<%=Band.Coletor.Redex.Consultas.OpcoesDescarga.DA.ToString()%>') {
                            window.location.href = 'TalieColetorDescarga.aspx?dd=1&cd=0&registro=' + registro;
                        } else {
                            window.location.href = 'TalieColetorDescarga.aspx?dd=0&cd=1&registro=' + registro;
                        }
                        
                    }, 1500);

                },
                error: function (jqXHR, status, err) {
                    toastr.error(jqXHR.responseText, 'Coletor');
                }
                //,
                
                
            })
            .always(function () {
                $('#modal-verifica-etiquetas').modal('hide');
                $('#modal-verifica-pendencias').modal('hide');

                $('#btnVerificaPendenciaEmisssoEtiquetas')
                    .html('<i class="fa fa-spinner fa-spin"></i>')
                    .removeClass('disabled');

                $('#btnAtualizarAlertaEtiqueta1')
                    .html('<i class="fa fa-spinner fa-spin"></i>')
                    .removeClass('disabled');
            });
        }

        function exibirFiltro() {
            $('#panelFiltro').removeClass('invisivel');
        }

        function FiltrarTalie() {
            $('#FiltrarTalie').removeClass("invisivel");
            $('#btnFiltrar').addClass("invisivel");
            $('#grdTalie_filter').addClass("invisivel");
        }

        function CancelarFiltroTalie() {
            $('#FiltrarTalie').addClass("invisivel");
            $('#btnFiltrar').removeClass("invisivel");
            $('#grdTalie_filter').removeClass("invisivel");
            return false;
        }

        function LimparFiltroTalie() {
            document.getElementById('<%= btnConsultarTalies.ClientID %>').click();
        }

        function PesquisarTalieResgistro() {
            $('.nav-tabs a[href="#talies"]').tab('show');
        }

        $('#hddnPesquisarTalieResgistro').change(function () {
            PesquisarTalieResgistro()
        })

    </script>

</body>
</html>
