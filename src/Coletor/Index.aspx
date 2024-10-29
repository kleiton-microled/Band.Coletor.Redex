<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Index.aspx.vb" Inherits="Band.Coletor.Redex.Consultas.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=5" />
    <title>CHRONOS - DESOVA COLETOR</title>

    <script src="lib/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="Scripts/BrowserWindowSize.js" type="text/javascript"></script>

    
    <script type="text/javascript" src="../lib/dispose.js"></script>
     <script type = "text/javascript" >
         $(document).ready(function () {
             var b = $(window).height(); //gets the window's height, change the selector if you are looking for height relative to some other element
             //$("#table1").css("height", b - 150);
            
            // if (document.getElementById('HiddenField1') != null) {
            //     document.getElementById('HiddenField1').value = b;
              
             //}
         });
       


        
    </script>

    <style type="text/css">
        .style1
        {
            font-family: tahoma;
            font-size: large;
            text-align: right;
            color: #FFFFFF;
            height: 86px;
            width: 34px;
            font-weight: bold;
        }
        .style5
        {
            width: 297px;
            height: 86px;
        }
        .style6
        {
            font-family: Tahoma;
            font-size: small;
            width: 441px;
          
        }
        .style8
        {
            width: 100%;
            height: 46px;
        }
        .style9
        {
            height: 44px;
            text-align: right;
            width: 34px;
        }
        .style10
        {
            width: 297px;
            height: 44px;
            text-align: center;
        }
        .style11
        {
            font-size: large;
            font-weight: bold;
        }
        .style13
        {
            height: 33px;
        }
        .style14
        {
            font-size: x-small;
        }
        .style15
        {
            width: 297px;
            text-align: center;
            height: 55px;
        }
      
        .style17
        {
            font-size: large;
            font-family: tahoma;
            font-style: normal;
            font-weight: bold;
            color: #FFFFFF;
            height: 65px;
            width: 34px;
        }
        .style18
        {
            height: 55px;
            width: 34px;
        }
        .style20
        {
            text-align: right;
            font-size: large;
            font-family: tahoma;
            font-style: normal;
            font-weight: bold;
            color: #FFFFFF;
            height: 75px;
            width: 34px;
        }
        .style21
        {
            width: 297px;
            height: 75px;
        }
        .style22
        {
            text-align: right;
            font-size: large;
            font-family: tahoma;
            font-style: normal;
            font-weight: bold;
            color: #FFFFFF;
            height: 63px;
            width: 34px;
        }
        .style23
        {
            width: 297px;
            height: 63px;
        }
        .style24
        {
            width: 297px;
            height: 65px;
        }
        .style25
        {
            height: 70px;
        }
        .auto-style1 {
            height: 10%;
            text-align: center;
            width: 20%;
        }
        .auto-style4 {
            font-size: medium;
            font-weight: normal;
        }
        .auto-style7 {
            
            text-align: center;
        }
        .auto-style16 {
            font-family: tahoma;
            font-size: large;
            color: #FFFFFF;
            
            width: 20%;
            font-weight: bold;
            text-align: center;
        }
        .auto-style18 {
            font-size: large;
            font-family: tahoma;
            font-style: normal;
            font-weight: bold;
            color: #FFFFFF;
            
            width: 20%;
            text-align: center;
        }
        .auto-style19 {
            width: 70%;
            
            text-align: center;
        }
        .auto-style24 {
            font-size: small;
        }
        .auto-style25 {
            font-size: x-small;
            font-weight: bold;
        }
        .auto-style30 {
            width: 100%;
            height: 828px;
        }
       
        .auto-style31 {
            height: 70px;
            text-align: left;
        }
        .auto-style32 {
            font-family: tahoma;
            font-size: large;
            text-align: right;
            color: #FFFFFF;
            height: 20px;
            width: 108px;
            font-weight: bold;
            position: relative;
            left: 0px;
            top: -12px;
        }
       
    </style>

</head>

<body  style="width: 220px; height: 300px" >

    <form id="form1" runat="server" style="width: 220px; height: 300px; font-size: medium;">

            <table id="table1" cellpadding="2" cellspacing="0" bgcolor="#0066CC" 
                style="border: 1px solid #000000; width: 220px; height: 300px;"; align="left" 
               class="auto-style30" >
                <tr>
                    <td 
                        bgcolor="#5D7B9D" class="auto-style1">
                        <strong style="width: 20%; height: 20%">
                        <asp:Image ID="Image1" runat="server" Height="25px" 
                            ImageUrl="~/imagens/cadeado.png" Width="25px" />
                    </td>
                    <td bgcolor="#5D7B9D" 
                        class="auto-style19" style="width: 80%; height: 12,5%; font-size: small;">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" 
                            Text="CHRONOS MOBILE - ARMAZEM" Font-Names="Tahoma" Width="100%" 
                            CssClass="auto-style24" ForeColor="White" Height="25px" style="font-size: xx-small"></asp:Label>
                        <br />
                        <br style="width: 80%; height: 20%" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#5D7B9D" class="auto-style18" style="width: 20%; height: 10%">
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="X-Large" 
                            Text="USUARIO:" Font-Names="Tahoma" Width="90%" 
                            CssClass="auto-style4" ForeColor="White" Height="80%" style="font-size: x-small"></asp:Label>
                        </td>
                    <td 
                        bgcolor="#5D7B9D" class="auto-style19" style="width: 80%; height: 12,5%">
                        <strong style="width: 80%; height: 10%">
                        <asp:TextBox ID="txtUsuario" runat="server" Font-Size="Medium" Width="90%" 
                            AutoPostBack="True" Font-Names="Tahoma" Height="25px" TabIndex="1" 
                            BorderStyle="Solid" CssClass="auto-style25" style="font-size: x-small"></asp:TextBox>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#5D7B9D" class="auto-style18" style="width: 20%; height: 10%">
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="X-Large" 
                            Text="SENHA:" Font-Names="Tahoma" Width="90%" 
                            CssClass="auto-style4" ForeColor="White" Height="80%" style="font-size: x-small"></asp:Label>
                        </td>
                    <td bgcolor="#5D7B9D" class="auto-style19" style="width: 80%; height: 12,5%">
                        <strong style="width: 80%; height: 10%">
                        <asp:TextBox ID="txtSenha" runat="server" Font-Size="X-Small" TextMode="Password" 
                            Width="90%" Font-Names="Tahoma" height="25px" TabIndex="2" 
                            BorderStyle="Solid" CssClass="auto-style25" style="font-size: x-small"></asp:TextBox>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style16" bgcolor="#5D7B9D" style="width: 20%; height: 10%">
                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="X-Large" 
                            Text="TERMINAL:" Font-Names="Tahoma" Width="90%" 
                            CssClass="auto-style4" ForeColor="White" Height="80%" style="font-size: x-small"></asp:Label>
                        </td>
                    <td bgcolor="#5D7B9D" class="auto-style19" style="width: 80%; height: 12,5%">
                        <strong style="width: 80%; height: 10%">
                        <asp:TextBox ID="txtPatio" runat="server" Font-Size="X-Small" Width="90%" 
                            Enabled="False" Font-Names="Tahoma" height="25px" TabIndex="3" 
                            BorderStyle="Solid" CssClass="auto-style25" style="font-size: x-small"></asp:TextBox>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#5D7B9D" class="auto-style18" style="width: 20%; height: 10%">
                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="X-Large" 
                            Text="EMPRESA:" Font-Names="Tahoma" Width="90%" 
                            CssClass="auto-style4" ForeColor="White" Height="80%" style="font-size: x-small"></asp:Label>
                        </td>
                    <td bgcolor="#5D7B9D" class="auto-style19">
                        <strong style="width: 80%; height: 10%">
                        <asp:TextBox ID="txtEmpresa" runat="server" Font-Size="X-Small" Width="90%" 
                            Enabled="False" Font-Names="Tahoma" height="25px" TabIndex="4" 
                            BorderStyle="Solid" CssClass="auto-style25" style="font-size: x-small"></asp:TextBox>
                        </strong>
                    </td>
                </tr>
                <tr style="width: 100%; ">
                    <td 
                        bgcolor="#5D7B9D" class="auto-style7" colspan="2" style="width: 100%; height: 10%">
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <asp:ImageButton ID="btLogin" runat="server" 
                            ImageUrl="~/imagens/red_login_button_lg.png" Enabled="False" Width="50px" 
                                        Height="25px" />
                        </td>
                </tr>
                <tr style="width: 100%; ">
                    <td 
                        bgcolor="#5D7B9D" class="auto-style7" colspan="2" style="width: 100%; height: 10%">
                        <asp:Label ID="lblERRO" runat="server" Font-Bold="True" Font-Size="X-Large" Font-Names="Tahoma" Width="100%" 
                            CssClass="style14" ForeColor="Red" Height="100%" style="font-size: x-small"></asp:Label>
                        </td>
                </tr>
                </strong>
                <tr>
                    <td 
                        bgcolor="#5D7B9D" class="auto-style7" colspan="2" style="width: 100%; height: 10%;">
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" 
                            Text="VERSAO 18.08.2016" Font-Names="Tahoma" Width="100%" 
                            CssClass="style14" ForeColor="#99FFCC" Height="100%" style="font-size: x-small"></asp:Label>
                    </td>
                </tr>
                <tr style="width: 100%; height: 100%">
                    <td colspan="2" 
                        bgcolor="#5D7B9D" class="auto-style31" style="width: 100%; height: 10%">
                        <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="X-Large" 
                            Text="NOVA SENHA:" Font-Names="Tahoma" Width="40%" 
                            CssClass="auto-style4" ForeColor="White" Height="80%" Visible="False" style="font-size: x-small"></asp:Label>
                        <strong>
                        <asp:TextBox ID="txtNovaSenha" runat="server" Font-Size="X-Small" TextMode="Password" 
                            Width="16%" Font-Names="Tahoma" height="25px" TabIndex="2" 
                            BorderStyle="Solid" CssClass="auto-style25" Visible="False" style="font-size: x-small"></asp:TextBox>
                            <asp:Button ID="btSalvar" runat="server" BackColor="#5D7B9D" 
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CssClass="auto-style32" 
                                Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" ForeColor="White" 
                                Text="GRAVAR" 
                                
                                
                                
                                
                                
                                
                                style="font-size: x-small; width: 64px;" UseSubmitBehavior="False" Visible="False" />
                        </strong>
                    </td>
                </tr>
                </table>

 


    </form>


</body>
</html>
