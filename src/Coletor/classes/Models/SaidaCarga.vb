Public Class SaidaCarga

    Private _id As Integer
    Public Property Id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property


    Private _idSC As Integer
    Public Property IdSC() As Integer
        Get
            Return _idSC
        End Get
        Set(ByVal value As Integer)
            _idSC = value
        End Set
    End Property

    Private _patioCSId As Integer
    Public Property PatioCSId() As Integer
        Get
            Return _patioCSId
        End Get
        Set(ByVal value As Integer)
            _patioCSId = value
        End Set
    End Property

    Private _saldo As Integer
    Public Property Saldo() As Integer
        Get
            Return _saldo
        End Get
        Set(ByVal value As Integer)
            _saldo = value
        End Set
    End Property

    Private _reserva As String
    Public Property Reserva() As String
        Get
            Return _reserva
        End Get
        Set(ByVal value As String)
            _reserva = value
        End Set
    End Property

    Private _produtoId As Integer
    Public Property ProdutoId() As Integer
        Get
            Return _produtoId
        End Get
        Set(ByVal value As Integer)
            _produtoId = value
        End Set
    End Property

    Private _Produto As String
    Public Property Produto() As String
        Get
            Return _Produto
        End Get
        Set(ByVal value As String)
            _Produto = value
        End Set
    End Property

    Private _codProduto As String
    Public Property CodProduto() As String
        Get
            Return _codProduto
        End Get
        Set(ByVal value As String)
            _codProduto = value
        End Set
    End Property


    Private _NFId As Integer
    Public Property NFId() As Integer
        Get
            Return _NFId
        End Get
        Set(ByVal value As Integer)
            _NFId = value
        End Set
    End Property

    Private _numNF As Integer
    Public Property NumNF() As Integer
        Get
            Return _numNF
        End Get
        Set(ByVal value As Integer)
            _numNF = value
        End Set
    End Property

    Private _nfItemId As Integer
    Public Property NFItemId() As Integer
        Get
            Return _nfItemId
        End Get
        Set(ByVal value As Integer)
            _nfItemId = value
        End Set
    End Property

    Private _embalagemId As Integer
    Public Property EmbalagemId() As Integer
        Get
            Return _embalagemId
        End Get
        Set(ByVal value As Integer)
            _embalagemId = value
        End Set
    End Property

    Private _embalagem As String
    Public Property Embalagem() As String
        Get
            Return _embalagem
        End Get
        Set(ByVal value As String)
            _embalagem = value
        End Set
    End Property

    Private _largura As Decimal
    Public Property Largura() As Decimal
        Get
            Return _largura
        End Get
        Set(ByVal value As Decimal)
            _largura = value
        End Set
    End Property

    Private _altura As Decimal
    Public Property Altura() As Decimal
        Get
            Return _altura
        End Get
        Set(ByVal value As Decimal)
            _altura = value
        End Set
    End Property

    Private _comprimento As Decimal
    Public Property Comprimento() As Decimal
        Get
            Return _comprimento
        End Get
        Set(ByVal value As Decimal)
            _comprimento = value
        End Set
    End Property

    Private _lote As String
    Public Property Lote() As String
        Get
            Return _lote
        End Get
        Set(ByVal value As String)
            _lote = value
        End Set
    End Property

    Private _pesoTotal As Decimal
    Public Property PesoTotal() As Decimal
        Get
            Return _pesoTotal
        End Get
        Set(ByVal value As Decimal)
            _pesoTotal = value
        End Set
    End Property

    Private _bruto As Decimal
    Public Property Bruto() As Decimal
        Get
            Return _bruto
        End Get
        Set(ByVal value As Decimal)
            _bruto = value
        End Set
    End Property

    Private _volumeTotal As Decimal
    Public Property VolumeTotal() As Decimal
        Get
            Return _volumeTotal
        End Get
        Set(ByVal value As Decimal)
            _volumeTotal = value
        End Set
    End Property
    Private _volume As Decimal
    Public Property Volume() As Decimal
        Get
            Return _volume
        End Get
        Set(ByVal value As Decimal)
            _volume = value
        End Set
    End Property

    Private _autonumRCS As Integer
    Public Property AutonumRCS() As Integer
        Get
            Return _autonumRCS
        End Get
        Set(ByVal value As Integer)
            _autonumRCS = value
        End Set
    End Property
End Class
