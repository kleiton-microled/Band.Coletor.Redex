Imports System.Web

Public Class windowSize
    Implements IHttpHandler

    ''' <summary>
    '''  You will need to configure this handler in the Web.config file of your 
    '''  web and register it with IIS before being able to use it. For more information
    '''  see the following link: http://go.microsoft.com/?linkid=8101007
    ''' </summary>
#Region "IHttpHandler Members"

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            ' Return false in case your Managed Handler cannot be reused for another request.
            ' Usually this would be false in case you have some state information preserved per request.
            Return True
        End Get
    End Property

    Public Class windowSize
        Implements IHttpHandler
        Implements System.Web.SessionState.IRequiresSessionState

        Public Sub ProcessRequest(context As HttpContext)
            context.Response.ContentType = "application/json"

            Dim json = New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim output = json.Serialize(New With {
            Key .isFirst = context.Session("BrowserWidth") Is Nothing
        })
            context.Response.Write(output)

            context.Session("BrowserWidth") = context.Request.QueryString("Width")
            context.Session("BrowserHeight") = context.Request.QueryString("Height")
        End Sub

        Private Sub IHttpHandler_ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
            Throw New NotImplementedException()
        End Sub

        Public ReadOnly Property IsReusable() As Boolean
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Private ReadOnly Property IHttpHandler_IsReusable As Boolean Implements IHttpHandler.IsReusable
            Get
                Throw New NotImplementedException()
            End Get
        End Property
    End Class

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        ' Write your handler implementation here.

    End Sub

#End Region

End Class
