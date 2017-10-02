
' undo,redo用、必要なすべての情報の格納形式
Public Class Status
    ' ボタンの情報の格納形式
    Public Structure ButtonStatusStruct
        Public buttonColumn, buttonRow, color As Integer
        Public gameStatus As String
    End Structure

    Public buttonStatus As ButtonStatusStruct
    Public dominateColor As Integer
    Public isDragon As Boolean
    Public dragonBtnColumn As Integer

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