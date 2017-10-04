' undo,redo用、必要なすべての情報の格納形式
Public Class Status
    ' ボタンの情報の格納形式
    Public Structure ButtonStatusStruct
        Dim buttonColumn, buttonRow, color As Integer
        Dim gameStatus As String
    End Structure

    Dim buttonStatus As ButtonStatusStruct
    Dim dominateColor As Integer
    Dim isDragon As Boolean
    Dim dragonBtnColumn As Integer

    Public Sub New()
        initStatus()
    End Sub

    Public Sub initStatus()
        buttonStatus.buttonColumn = 0
        buttonStatus.buttonRow = -1
        buttonStatus.gameStatus = ""
        dominateColor = 0
        isDragon = False
        dragonBtnColumn = 0

        ' undoのメソッド自身でやってもらう
        ' colorSet(DefaultBackColor, 0)
        ' getButtonOfIdx(0).Text = ""
        ' statusIdx = -1

    End Sub


    ' 登録-------------------------------------------------------------------------------------------------------------------------------------
    ' ボタン系の情報登録
    Public Sub setButtonStatus(ByVal buttonColumn As Integer, ByVal buttonRow As Integer, ByVal color As Integer, ByVal gameStatus As String)
        Me.buttonStatus.buttonColumn = buttonColumn
        Me.buttonStatus.buttonRow = buttonRow
        Me.buttonStatus.color = color
        Me.buttonStatus.gameStatus = gameStatus
    End Sub

    ' 支配情報の登録
    Public Sub setDominateColor(ByVal dominateColor As Integer)
        Me.dominateColor = dominateColor
    End Sub

    ' ドラゴン系の情報登録
    Public Sub setDragon(ByVal isDragon As Boolean, ByVal dragonBtnColumn As String)
        Me.isDragon = isDragon
        Me.dragonBtnColumn = dragonBtnColumn
    End Sub


    ' 取得-----------------------------------------------------------------------------------------------------------------------------------
    Public Function getButtonColumn() As Integer
        Return Me.buttonStatus.buttonColumn
    End Function
    Public Function getButtonRow() As Integer
        Return Me.buttonStatus.buttonRow
    End Function
    Public Function getColor() As Integer
        Return Me.buttonStatus.color
    End Function
    Public Function getGameStatus() As String
        Return Me.buttonStatus.gameStatus
    End Function


    Public Function getDominateColor() As Integer
        Return Me.dominateColor
    End Function

    Public Function getIsDragon() As Boolean
        Return Me.isDragon
    End Function
    Public Function getDragonBtnColumn() As Integer
        Return Me.dragonBtnColumn
    End Function

End Class