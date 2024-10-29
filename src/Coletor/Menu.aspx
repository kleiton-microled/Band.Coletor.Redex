<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Menu.aspx.vb" Inherits="Band.Coletor.Redex.Consultas.WebForm2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="lib/jquery-1.7.1.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            //var b = $(window).height(); //gets the window's height, change the selector if you are looking for height relative to some other element
            //$("#table1").css("height", b - 0);
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

    <style type="text/css">
        .style1 {
            width: 94%;
            height: 609px;
        }

        .style2 {
            height: 614px;
            width: 450px;
        }

        .style5 {
            height: 28px;
            font-size: small;
            text-align: center;
            color: #5D7B9D;
            background-color: #FFFFFF;
        }

        input {
            font-family: Tahoma;
            font-size: 9px;
            text-align: center;
        }


        * {
            padding: 0;
            overflow: hidden;
            margin-left: 0;
            margin-right: 0;
            }

        body {
            overflow: hidden;
        }

        .style8 {
            margin-bottom: 0;
            font-size: x-large;
            color: #000000;
            font-weight: bold;
        }

        .style12 {
            height: 45px;
        }

        .style13 {
            height: 44px;
        }

        .style14 {
            height: 39px;
        }

        .style15 {
            height: 37px;
        }

        .style16 {
            height: 111px;
            text-align: justify;
        }

        .style17 {
            font-size: x-large;
            color: #000000;
            font-weight: bold;
        }

        .style18 {
            width: 115px;
            height: 97px;
            float: left;
            margin-left: 167;
        }

        .style19 {
            background-color: #FFFFFF;
        }

        .style20 {
            font-size: x-small;
        }

        .style21 {
            background-color: #FFFFFF;
            font-size: x-large;
        }

        .style22 {
            font-size: x-large;
        }

        #form1 {
            width: 100%;
            height: 100%;
        }

        .style23 {
            height: 34px;
            font-size: large;
            text-align: right;
            color: #FFFFFF;
            width: 37px;
        }

        .style24 {
            height: 34px;
            font-size: xx-small;
            text-align: center;
            width: 352px;
        }

        .auto-style2 {
            height: 5%;
            font-size: xx-small;
            text-align: center;
            width: 25%;
        }

        .auto-style4 {
            width: 100%;
            height: 371px;
        }

        .auto-style5 {
            font-size: x-small;
            color: #000000;
            font-weight: bold;
            margin-left: 1;
        }

        .auto-style7 {
            height: 5%;
            font-size: small;
            text-align: center;
            color: #5D7B9D;
            background-color: #FFFFFF;
            width: 100%;
        }

        .auto-style10 {
            width: 56px;
            height: 47px;
            float: left;
            margin-left: 167;
            font-size: medium;
        }

        .auto-style11 {
            font-size: x-small;
        }

        .auto-style12 {
            background-color: #FFFFFF;
            font-size: xx-small;
        }

        .auto-style14 {
            height: 5%;
            font-size: medium;
            text-align: right;
            color: #FFFFFF;
        }

        .auto-style15 {
            height: 5%;
            font-size: medium;
            text-align: right;
            color: #FFFFFF;
            width: 25%;
        }

        .auto-style16 {
            height: 6%;
        }

        .auto-style17 {
            width: 96%;
            height: 418px;
        }
        .auto-style18 {
            font-size: xx-small;
        }
    </style>
</head>


<body style="width: 240; height: 300">

    <form id="form1" runat="server" class="auto-style17" style="width: 240px; height: 300px">




        <table id="table1" class="auto-style4" align="left" bgcolor="#003366" style="width: 240px; height: 300px">
            <tr>
                <td class="auto-style7"
                    style="font-family: Tahoma; font-weight: bold;"
                    colspan="3" bgcolor="#5D7B9D">
                    <span class="auto-style12">
                        <strong style="font-family: Tahoma; height: 5%;">CHRONOS</strong></span><strong
                            style="font-family: Tahoma;"><span class="style19"> <span class="auto-style18">MOBILE - ARMAZEM </span></span></strong></td>
            </tr>
            <tr>
                <td class="auto-style15" bgcolor="#5D7B9D" style="font-family: TAHoma;">
                    <strong><span class="auto-style11">USUÁRIO</span></strong><span class="auto-style11">&nbsp;&nbsp;
                    </span>
                </td>
                <td class="auto-style14" bgcolor="#5D7B9D" style="font-family: TAHoma;">
                    <strong style="width: 50%; height: 5%;">
                        <asp:TextBox ID="txtUsuario" runat="server" BackColor="#CCCCCC" BorderWidth="1px"
                            Height="100%" Width="100%" CssClass="auto-style5" ForeColor="Black"
                            ReadOnly="True" BorderStyle="Solid"></asp:TextBox>
                    </strong>
                </td>
                <td class="auto-style2" bgcolor="#5D7B9D" style="font-family: Tahoma; font-weight: bold; color: #FFFFFF; font-size: x-small;" width="25%">OPÇÃO :</td>
            </tr>
            <tr>
                <td class="auto-style15" bgcolor="#5D7B9D" style="font-family: TAhoma;">
                    <strong style="width: 50%; height: 5%;"><span class="auto-style11">TERMINAL</span></strong><span class="auto-style11">
                            &nbsp;</span></td>
                <td class="auto-style14" bgcolor="#5D7B9D" style="font-family: TAhoma;">
                    <strong style="height: 5%; width: 50%;">
                        <asp:TextBox ID="txtTerminal" runat="server" BackColor="#CCCCCC" BorderWidth="1px"
                            Height="100%" Width="100%" ForeColor="Black" ReadOnly="True"
                            CssClass="auto-style5" BorderStyle="Solid" Font-Names="Tahoma" Font-Size="6pt"></asp:TextBox>
                    </strong>
                </td>
                <td class="auto-style2" bgcolor="#5D7B9D" style="font-family: TAhoma;">
                    <strong style="height: 5%; width: 25%;">
                        <asp:TextBox ID="txtOpcao" runat="server" BackColor="White" BorderWidth="1px"
                            Height="100%" Width="100%" ForeColor="Black"
                            CssClass="auto-style5" BorderStyle="Solid" AutoPostBack="True"></asp:TextBox>
                    </strong>
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style14" style="height: 15px">
                    <asp:Button ID="btIni7" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="#CCCCCC"
                        Height="100%" Text="[1] - CARREGAMENTO - CARGA SOLTA" Width="100%"
                        Style="font-size: xx-small; margin-top: 0;" Enabled="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style14" style="height: 15px">
                    <asp:Button ID="btIni6" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="White"
                        Height="100%" Text="[2] - DESCARGA ARMAZÉM" Width="100%"
                        Style="font-size: xx-small; margin-top: 0;" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style14" style="height: 15px">
                    <asp:Button ID="btIni" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="#CCCCCC"
                        Height="100%" Text="[3] - DESCARGA CD" Width="100%"
                        Style="font-size: xx-small; margin-top: 0;" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style12" style="height: 15px">
                    <asp:Button ID="btIni5" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="White"
                        Height="100%" Text="[4] - ESTUFAGEM CONTÊINER" Width="100%"
                        Style="font-size: xx-small" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style12" style="height: 15px">
                    <asp:Button ID="btIni3" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="#CCCCCC"
                        Height="100%" Text="[5] - IDENTIFICAÇÃO DO LOTE" Width="100%"
                        Style="font-size: xx-small" Enabled="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="auto-style16" style="height: 15px">
                    <asp:Button ID="btIni8" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="#CCCCCC"
                        Height="100%" Text="[6] - INVENTARIO DE CARGA SOLTA" Width="100%"
                        Style="font-size: xx-small" Enabled="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style12" style="width: 100%; height: 15px"">
                    <asp:Button ID="btIni4" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="#CCCCCC"
                        Height="100%" Text="[7] - MARCANTES - CARGA SOLTA" Width="100%"
                        Style="font-size: xx-small" Enabled="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style12" style="width: 100%; height: 15px">
                    <asp:Button ID="btIni0" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="White"
                        Height="100%" Text="[8] - MOVIMENTAÇAO - CARGA SOLTA" Width="100%"
                        Style="font-size: xx-small" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style14" style="width: 100%; height: 15px">
                    <asp:Button ID="btIni2" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="White"
                        Height="100%" Text="[9] - MOVIMENTAÇÃO - CONTEINER" Width="100%"
                        Style="font-size: xx-small" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style15" style="width: 100%; height: 15px">
                    <asp:Button ID="btIni9" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="#CCCCCC"
                        Height="100%" Text="[10] - DDC - DESOVA DIRETA CAM." Width="100%"
                        Style="font-size: xx-small" Enabled="true" />
                </td>
            </tr>
            <%-- <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style14" style="height: 6%">
                            <asp:Button ID="btIniTalie" runat="server" BackColor="#5D7B9D" 
                                BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234" 
                                Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="White" 
                                Height="100%" Text="[11] - IDENTIFICAÇÃO DO LOTE (TESTE)" Width="100%" 
                                style="font-size: medium; margin-top: 0;" />
                        </td>
            </tr>--%>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style15" style="width: 100%; height: 15px">
                    <asp:Button ID="btIni1" runat="server" BackColor="#5D7B9D"
                        BorderColor="White" BorderStyle="Solid" BorderWidth="1px" CssClass="style234"
                        Font-Bold="True" Font-Names="Tahoma" Font-Size="11px" ForeColor="White"
                        Height="100%" Text="[0] - LOG OFF" Width="100%"
                        Style="font-size: xx-small" />
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#5D7B9D" class="style16" style="width: 100%; height: 15px">
                    <img alt="" class="auto-style10" src="imagens/Capturar.PNG"
                        style="border-style: solid" /></td>
            </tr>
        </table>


    </form>
</body>
</html>
