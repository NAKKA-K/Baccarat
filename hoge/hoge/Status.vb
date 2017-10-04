Public Class Status
    Property buttonColumn As Integer
    Property buttonRow As Integer
    Property color As Color
    Property gameStatus As String

    Property dominateColor As Color
    Property isDragon As Boolean
    Property dragonBtnColumn As Integer

    Public Sub New()
        initStatus()
    End Sub

    Public Sub initStatus()
        buttonColumn = 0
        buttonRow = -1
        color = Control.DefaultBackColor
        gameStatus = ""
        dominateColor = Color.Green
        isDragon = False
        dragonBtnColumn = 0

        ' undoのメソッド自身でやってもらう
        ' colorSet(DefaultBackColor, 0)
        ' getButtonOfIdx(0).Text = ""
        ' statusIdx = -1

    End Sub


    ' 登録-------------------------------------------------------------------------------------------------------------------------------------
    ' ボタン系の情報登録
    Public Sub setButtonStatus(ByVal buttonColumn As Integer, ByVal buttonRow As Integer, ByVal color As Color, ByVal gameStatus As String)
        Me.buttonColumn = buttonColumn
        Me.buttonRow = buttonRow
        Me.color = color
        Me.gameStatus = gameStatus
    End Sub

    ' 支配情報の登録
    Public Sub setDominateColor(ByVal dominateColor As Color)
        Me.dominateColor = dominateColor
    End Sub

    ' ドラゴン系の情報登録
    Public Sub setDragon(ByVal isDragon As Boolean, ByVal dragonBtnColumn As String)
        Me.isDragon = isDragon
        Me.dragonBtnColumn = dragonBtnColumn
    End Sub


End Class