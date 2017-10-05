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
    End Sub

End Class