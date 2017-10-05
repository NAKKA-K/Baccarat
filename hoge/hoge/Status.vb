Public Class Status
    Public Const BUTTON_ROW As Integer = 16
    Public Const BUTTON_COLUMN As Integer = 32

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

    End Sub


    ' 2次元配列的に配置されるボタンのインデックスを、1次元配列のインデックスで返す
    ' 引数を省略された場合、フィールドを使用して計算する
    Public Function getIdxOfButton() As Integer
        Return (buttonColumn * BUTTON_ROW) + buttonRow ' (列数 * 最大行数) + 行数
    End Function

    Public Shared Function getIdxOfButton(ByVal row As Integer, ByVal column As Integer) As Integer
        Return (column * BUTTON_ROW) + row ' (列数 * 最大行数) + 行数
    End Function

End Class