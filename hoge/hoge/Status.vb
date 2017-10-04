Public Class Status
    ' ボタンの情報の格納形式
    Public Structure ButtonStatusStruct
        Dim buttonColumn, buttonRow As Integer
        Dim color As Color
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
    Public Sub setButtonStatus(ByVal buttonColumn As Integer, ByVal buttonRow As Integer, ByVal color As Color, ByVal gameStatus As String)
        Me.buttonStatus.buttonColumn = buttonColumn
        Me.buttonStatus.buttonRow = buttonRow
        Me.buttonStatus.color = color
        Me.buttonStatus.gameStatus = gameStatus
    End Sub


    ' ドラゴン系の情報登録
    Public Sub setDragon(ByVal isDragon As Boolean, ByVal dragonBtnColumn As String)
        Me.isDragon = isDragon
        Me.dragonBtnColumn = dragonBtnColumn
    End Sub


    Public Sub setButtonColumn(buttonColumn As Variant)
        Me.buttonStatus.buttonColumn = buttonColumn
    End Sub
    Public Sub setButtonRow(buttonRow As Variant)
        Me.buttonStatus.buttonRow = buttonRow
    End Sub
    Public Sub setColor(color As Variant)
        Me.buttonStatus.color = color
    End Sub
    Public Sub setGameStatus(gameStatus As Variant)
        Me.buttonStatus.gameStatus = gameStatus
    End Sub

    Public Sub setDominateColor(ByVal dominateColor As Integer) ' 支配情報の登録
        Me.dominateColor = dominateColor
    End Sub

    Public Sub setIsDragon(isDragon As Variant)
        Me.isDragon = isDragon
    End Sub
    Public Sub setDragonBtnColumn(dragonBtnColumn As Variant)
        Me.dragonBtnColumn = dragonBtnColumn
    End Sub

    ' 取得-----------------------------------------------------------------------------------------------------------------------------------
    Public Function getButtonColumn() As Integer
        Return Me.buttonStatus.buttonColumn
    End Function
    Public Function getButtonRow() As Integer
        Return Me.buttonStatus.buttonRow
    End Function
    Public Function getColor() As Color
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