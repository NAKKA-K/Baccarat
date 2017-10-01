Public Class Form1
    Dim num(255), Val, Debug As Integer
    Dim slash As Char
    Dim CmdButtons(9) As System.Windows.Forms.Button

    ' これより上は使っていない(何に使っていたのか分からないため、一応置いている。いらなければ消す)
    Dim maxLenge As Boolean

    Dim gameStatus As String
    Dim numSymbol As Char = " "
    Dim dominateColor As Integer = 0 ' その行を支配する色(赤、青)
    Dim isDragon As Boolean ' ドラゴン状態か？
    Dim buttonRow As Integer = -1
    Dim buttonColumn As Integer = 0
    Dim dragonBtnColumn As Integer

    Const COLOR_GREEN As Integer = 0
    Const COLOR_RED As Integer = 1
    Const COLOR_BLUE As Integer = 2

    Public Const BUTTON_ROW As Integer = 16
    Public Const BUTTON_COLUMN As Integer = 32

    Public Shared statusIdx As Integer = -1 ' 現在のStatus位置
    'Public Shared statusList As New ArrayList ' undo,redoの情報を保存する変数
    Public Shared statusList As ArrayList = ArrayList.Synchronized(New ArrayList)


    ' undo
    Private Sub undoStatus()
        Label4.Text = "add = " & statusIdx
        If statusIdx <= 0 Then ' undo先がない？
            ' 初期化
            buttonColumn = 0
            buttonRow = -1
            dominateColor = 0
            isDragon = False
            dragonBtnColumn = 0
            colorSet(DefaultBackColor, 0)
            getButtonOfIdx(0).Text = ""
            statusIdx = -1
            Return
        End If

        statusIdx = statusIdx - 1

        ' 情報の初期化
        Dim buttonIdx As Integer = getIdxOfButton(buttonRow, buttonColumn)
        colorSet(DefaultBackColor, buttonIdx)
        getButtonOfIdx(buttonIdx).Text = ""

        ' 1つ前の値を取得
        Dim statusTmp As Status = getStatus(statusIdx)
        buttonColumn = statusTmp.getButtonColumn()
        buttonRow = statusTmp.getButtonRow()
        dominateColor = statusTmp.getDominateColor()
        isDragon = statusTmp.getIsDragon()
        dragonBtnColumn = statusTmp.getDragonBtnColumn()
        gameStatus = statusTmp.getGameStatus()

    End Sub
    ' redo
    Private Sub redoStatus()
        Label4.Text = "add = " & statusIdx

        If statusIdx >= statusList.Count - 1 Then ' redo先がない？
            Return
        End If

        statusIdx = statusIdx + 1

        ' 1つ後の値を取得
        Dim statusTmp As Status = getStatus(statusIdx)
        buttonColumn = statusTmp.getButtonColumn()
        buttonRow = statusTmp.getButtonRow()
        dominateColor = statusTmp.getDominateColor()
        isDragon = statusTmp.getIsDragon()
        dragonBtnColumn = statusTmp.getDragonBtnColumn()

        ' 情報の初期化
        Dim buttonIdx As Integer = getIdxOfButton(buttonRow, buttonColumn)
        getButtonOfIdx(buttonIdx).Text = statusTmp.getGameStatus() ' 現状のマスにテキストを書き直して、次
        colorSet(getChangedColor(statusTmp.getColor()), buttonIdx) ' 現状のマスを塗り直して、次


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

        gameStatus = getGameStatus(inputNumStr)

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
    ' 色数値に対応したColorを返す。
    Public Function getChangedColor(ByVal colorNum As Integer) As Color
        Select Case colorNum
            Case COLOR_GREEN
                Return Color.Green
            Case COLOR_RED
                Return Color.Red
            Case COLOR_BLUE
                Return Color.Blue
            Case Else
                Return DefaultBackColor
        End Select
    End Function


    ' 色の設定をまとめた
    Private Sub colorSet(ByVal color As Color, ByVal buttonIdx As Integer)
        getButtonOfIdx(buttonIdx).BackColor = color
    End Sub

    ' 次に塗る場所が、既に塗られているか？
    Private Function isPaintButton(ByVal buttonIdx As Integer) As Boolean
        If getButtonOfIdx(buttonIdx).BackColor = Color.Green OrElse
        getButtonOfIdx(buttonIdx).BackColor = Color.Red OrElse
        getButtonOfIdx(buttonIdx).BackColor = Color.Blue Then

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
        Return column * BUTTON_ROW + row ' 00-0,10-1,20-2, 01-3,11-4,21-5, 02-6,12-7,22-8
    End Function


    ' ドラゴンの発生を司るメソッド
    Private Function dragonGenerate() As Boolean
        ' ドラゴンは発生するか？
        If isDragon = False AndAlso (buttonRow >= BUTTON_ROW - 1 OrElse isPaintButton(getIdxOfButton(buttonRow + 1, buttonColumn)) = True) Then
            If buttonColumn >= BUTTON_COLUMN - 1 Then ' 範囲外
                maxLenge = True
                Return False
            End If

            isDragon = True
            dragonBtnColumn = buttonColumn
        End If


        If isDragon = False Then
            buttonRow = buttonRow + 1
        ElseIf buttonColumn >= BUTTON_COLUMN - 1 Then ' 範囲外
            maxLenge = True
            Return False
        Else
            buttonColumn = buttonColumn + 1
        End If

        Return True
    End Function




    ' テキストボックスを入力してエンターを押したときの処理
    Private Sub pressEnterTextBox(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True 'Enterキーでビープ音が鳴らないようにする
        Else
            Return
        End If


        ButtonEn_Click() ' そのままクリックしたときの処理を呼べたのでそのまま

        If TextBox1.Text = "999" Then
            Me.FormBorderStyle = FormBorderStyle.None
            Me.WindowState = FormWindowState.Maximized
        ElseIf TextBox1.Text = "888" Then
            Application.Restart()
        ElseIf TextBox1.Text = "777" Then
            Me.Close()
        End If
        TextBox1.Clear()
    End Sub


    ' 数字の入力を確定された時
    Private Sub ButtonEn_Click()
        Dim inputStr As String = TextBox1.Text
        If Not isLogicalInput(inputStr) Then '入力されているか、数字の論理チェックが通っているか
            Return
        End If

        Dim colorNum As Integer
        If inputStr(0) > inputStr(1) Then ' プレイヤーwin
            dominate(COLOR_RED)
            colorNum = COLOR_RED

        ElseIf inputStr(0) < inputStr(1) Then ' バンカーwin
            dominate(COLOR_BLUE)
            colorNum = COLOR_BLUE

        ElseIf inputStr(0) = inputStr(1) Then 'draw
            If dragonGenerate() = True Then
                colorSet(Color.Green, getIdxOfButton(buttonRow, buttonColumn))
            End If
            colorNum = COLOR_GREEN

        End If


        'TextBox1.Clear() ' テキストボックスを初期化

        If maxLenge = True Then ' 範囲外
            maxLenge = False
            Return
        End If


        getButtonOfIdx(getIdxOfButton(buttonRow, buttonColumn)).Text = gameStatus


        ' Statusを保存するために、各項目ごとにメソッドで登録
        Dim saveStatus As Status = New Status
        saveStatus.setButtonStatus(buttonColumn, buttonRow, colorNum, gameStatus)
        saveStatus.setDominateColor(dominateColor)
        saveStatus.setDragon(isDragon, dragonBtnColumn)
        addStatus(saveStatus) ' statusListに作成したStatusを追加

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' ロード時にフォーカスを設定する
        Me.ActiveControl = Me.TextBox1
    End Sub

    ' 赤と青の処理が同じだったため抽出
    Private Sub dominate(ByVal colorNum As Integer)
        If dominateColor = COLOR_GREEN Then ' 中立
            If isDragon = False Then
                buttonRow = buttonRow + 1
            ElseIf buttonColumn >= BUTTON_COLUMN - 1 Then ' 範囲外
                maxLenge = True
                Return
            Else
                buttonColumn = buttonColumn + 1
            End If

        ElseIf dominateColor = colorNum Then ' 同じ色の支配
            If dragonGenerate() = False Then
                Return
            End If

        Else ' 違う色の支配
            If isDragon = True Then
                buttonColumn = dragonBtnColumn + 1
                isDragon = False
            ElseIf buttonColumn >= BUTTON_COLUMN - 1 Then ' 範囲外
                maxLenge = True
                Return
            Else
                buttonColumn = buttonColumn + 1
            End If
            buttonRow = 0

        End If


        colorSet(getChangedColor(colorNum), getIdxOfButton(buttonRow, buttonColumn))
        dominateColor = colorNum

    End Sub



    ' statusList系の操作------------------------------------------------------------------------------------------------------------------
    ' 指定したインデックスのStatusを返す
    Public Shared Function getStatus(ByVal statusIdx As Integer) As Status
        Return statusList.Item(statusIdx)
    End Function

    ' 引数のStatusをリストに登録
    Public Shared Sub addStatus(ByVal status As Status)
        SyncLock statusList.SyncRoot
            statusIdx = statusIdx + 1
            If statusIdx < statusList.Count - 1 Then ' undoした状態か？そうなら上書き
                statusList.RemoveRange(statusIdx, statusList.Count - statusIdx) ' redo先を上書きするため、それ以降をすべて削除
            End If

            statusList.Add(status)

        End SyncLock

    End Sub



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