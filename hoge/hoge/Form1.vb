Public Class Form1
    Public Const BUTTON_ROW As Integer = 16
    Public Const BUTTON_COLUMN As Integer = 32

    ReadOnly Property COLOR_GREEN As Color = Color.Green
    ReadOnly Property COLOR_RED As Color = Color.Red
    ReadOnly Property COLOR_BLUE As Color = Color.Blue
    'TODO:constが指定できるのは組み込み型や列挙型だけ(クラスなどは使えない)

    Dim maxLenge As Boolean
    Dim numSymbol As Char = " "

    Dim listMgr As New StatusListMgr
    Dim status As New Status

    ' Hangle系処理---------------------------------------------------------------------------------------------
    ' ロード時にフォーカスを設定する
    Private Sub focusTextBox(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.ActiveControl = Me.TextBox1
        TextBox1.Focus() 'フォーカスをbox1に
    End Sub

    ' テキストボックスを入力してエンターを押したときの処理
    Private Sub pressEnterTextBox(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True 'Enterキーでビープ音が鳴らないようにする
        Else
            Return
        End If

        If TextBox1.Text = "999" Then
            Me.FormBorderStyle = FormBorderStyle.None
            Me.WindowState = FormWindowState.Maximized
        ElseIf TextBox1.Text = "888" Then
            Application.Restart()
        ElseIf TextBox1.Text = "777" Then
            Me.Close()
        End If

        gameProcess() ' そのままクリックしたときの処理を呼べたのでそのまま
        TextBox1.Clear()
    End Sub


    ' 数字の入力を確定された時
    Private Sub gameProcess()
        Dim inputStr As String = TextBox1.Text
        If Not isLogicalInput(inputStr) Then '入力されているか、数字の論理チェックが通っているか
            Return
        End If

        If inputStr(0) > inputStr(1) Then ' プレイヤーwin
            dominate(COLOR_RED)
            status.color = COLOR_RED

        ElseIf inputStr(0) < inputStr(1) Then ' バンカーwin
            dominate(COLOR_BLUE)
            status.color = COLOR_BLUE

        ElseIf inputStr(0) = inputStr(1) Then 'draw
            If dragonGenerate() = True Then
                colorSet(Color.Green, getIdxOfButton(status.buttonRow, status.buttonColumn))
            End If
            status.color = COLOR_GREEN

        End If


        If maxLenge = True Then ' 範囲外
            maxLenge = False
            Return
        End If


        getButtonOfIdx(getIdxOfButton(status.buttonRow, status.buttonColumn)).Text = status.gameStatus


        ' Statusを保存するために、各項目ごとにメソッドで登録
        listMgr.addStatus(status) ' statusListに作成したStatusを追加

    End Sub



    ' ゲームの進捗に関するメソッド-----------------------------------------------------------------------------
    ' undo
    Private Sub undoStatus()
        Dim tmpStatus As Status = listMgr.undo()
        If Not tmpStatus = Null Then
            status = tmpStatus
        End If
    End Sub

    ' redo
    Private Sub redoStatus()
        Dim tmpStatus As Status = listMgr.redo()
        If Not tmpStatus = Null Then
            status = tmpStatus
        End If
    End Sub


    ' textbox内の処理---------------------------------------------------------------------------------------------------------------------
    ' 入力された内容の解釈。数字が2文字、又は０～９の数字、以外の場合false
    Private Function isLogicalInput(ByVal inputNumStr As String) As Boolean
        If inputNumStr = "//" Then ' undo
            undoStatus()
            Return False
        ElseIf inputNumStr = "**" Then ' redo
            redoStatus()
            Return False
        ElseIf inputNumStr.Length = 3 AndAlso inputNumStr(2) Like "[+-.]" Then ' シンボル有、正常値
            numSymbol = inputNumStr(2)
        ElseIf inputNumStr.Length <> 2 Then ' 異常値
            Return False
        Else ' シンボル無、正常値
            numSymbol = " "
        End If


        '入力文字が0~9か調べる
        For i As Integer = 0 To 1
            Select Case inputNumStr(i)
                Case "0" To "9"
                Case Else
                    Return False
            End Select
        Next

        status.gameStatus = getGameStatus(inputNumStr)

        Return True
    End Function


    ' 勝負結果の数字を指定の書式に変換して返す
    Private Function getGameStatus(ByVal inputNumStr As String) As String
        Dim char1 As Char = inputNumStr(0)
        Dim char2 As Char = inputNumStr(1)
        If numSymbol = "+" Then
            char1 = changeCreateCircle(char1)
        ElseIf numSymbol = "-" Then
            char2 = changeCreateCircle(char2)
        ElseIf numSymbol = "." Then
            char1 = changeCreateCircle(char1)
            char2 = changeCreateCircle(char2)
        End If

        Return char1 & "/" & char2
    End Function

    ' 丸付きの数字に変換する
    Private Function changeCreateCircle(ByVal numChar As Char) As Char
        Select Case numChar
            Case "0"
                Return "⓪"
            Case "1"
                Return "①"
            Case "2"
                Return "②"
            Case "3"
                Return "③"
            Case "4"
                Return "④"
            Case "5"
                Return "⑤"
            Case "6"
                Return "⑥"
            Case "7"
                Return "⑦"
            Case "8"
                Return "⑧"
            Case "9"
                Return "⑨"
        End Select
    End Function


    ' 色に関する処理---------------------------------------------------------------------------------------------------------------------
    ' 色の設定をまとめた
    Private Sub colorSet(ByVal color As Color, ByVal buttonIdx As Integer)
        getButtonOfIdx(buttonIdx).BackColor = color
    End Sub

    ' 次に塗る場所が、既に塗られているか？
    Private Function isPaintButton(ByVal button As Button) As Boolean
        If button.BackColor = Color.Green OrElse
        button.BackColor = Color.Red OrElse
        button.BackColor = Color.Blue Then

            Return True
        End If

        Return False
    End Function


    ' ボタン系処理------------------------------------------------------------------------------------------------------------
    ' 指定したボタンのインスタンスを返す
    Public Function getButtonOfIdx(ByVal buttonIdx As Integer) As Button
        Return Me.Controls("Button" & buttonIdx.ToString)
    End Function

    ' 2次元配列的に配置されるボタンのインデックスを、1次元配列のインデックスで返す
    Private Function getIdxOfButton(ByVal row As Integer, ByVal column As Integer) As Integer
        Return (column * BUTTON_ROW) + row ' (列数 * 最大行数) + 行数
    End Function


    ' ドラゴンの発生を司るメソッド
    Private Function dragonGenerate() As Boolean
        ' ドラゴンは発生するか？
        Dim btn As Button = getButtonOfIdx(getIdxOfButton(status.buttonRow + 1, status.buttonColumn))

        If status.isDragon = False AndAlso (isMaxLengeRow(status.buttonRow) OrElse isPaintButton(btn)) Then
            If isMaxLengeColumn(status.buttonColumn) Then ' 範囲外
                maxLenge = True
                Return False
            End If

            status.isDragon = True
            status.dragonBtnColumn = status.buttonColumn ' ドラゴン発生前の列を保存
        End If


        If status.isDragon = False Then
            status.buttonRow = status.buttonRow + 1
        ElseIf isMaxLengeColumn(status.buttonColumn) Then ' 範囲外
            maxLenge = True
            Return False
        Else
            status.buttonColumn = status.buttonColumn + 1
        End If

        Return True
    End Function



    ' 赤と青の処理が同じだったため抽出
    Private Sub dominate(ByVal color As Color)
        If status.dominateColor = COLOR_GREEN Then ' 中立
            If status.isDragon = False Then
                status.buttonRow = status.buttonRow + 1
            ElseIf isMaxLengeColumn(status.buttonColumn) Then ' 範囲外
                maxLenge = True
                Return
            Else
                status.buttonColumn = status.buttonColumn + 1
            End If

        ElseIf status.dominateColor = color Then ' 同じ色の支配
            If dragonGenerate() = False Then
                Return
            End If

        Else ' 違う色の支配
            If status.isDragon = True Then
                status.buttonColumn = status.dragonBtnColumn + 1
                status.isDragon = False
            ElseIf isMaxLengeColumn(status.buttonColumn) Then ' 範囲外
                maxLenge = True
                Return
            Else
                status.buttonColumn = status.buttonColumn + 1
            End If
            status.buttonRow = 0

        End If


        colorSet(status.color, getIdxOfButton(status.buttonRow, status.buttonColumn))
        status.setDominateColor = status.color

    End Sub

    ' 渡された列数が最大列数であるか判定する
    Function isMaxLengeColumn(ByVal column As Integer) As Boolean
        If column >= BUTTON_COLUMN - 1 Then
            Return True
        End If
        Return False
    End Function

    ' 渡された行数が最大行数であるか判定する
    Function isMaxLengeRow(ByVal row As Integer) As Boolean
        If row >= BUTTON_ROW - 1 Then
            Return True
        End If
        Return False
    End Function






    ' テストコード-----------------------------------------------------------------------------------------------------------------------------------
    ' ボタン0をクリックすると自動でランダムな入力をしてバグを誘発させる仕組み
    Private Sub testRandomPlayButton0_Click(sender As Object, e As EventArgs) Handles Button0.Click
        Dim rand As New System.Random()
        Select Case rand.Next(10)
            Case 0 To 1
                TextBox1.Text = "**"
            Case 2 To 3
                TextBox1.Text = "//"
            Case Else
                TextBox1.Text = rand.Next(9).ToString + rand.Next(9).ToString
        End Select

        TextBox1.Focus() 'フォーカスをbox1に
        SendKeys.Send("{ENTER}") ' フォームにEnter入力を送る(フォーカスが有効であればそれに対する)

    End Sub

End Class

