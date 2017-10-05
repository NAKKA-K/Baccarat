Public Class StatusListMgr
    Dim statusList As ArrayList = ArrayList.Synchronized(New ArrayList)
    Dim statusIdx As Integer = -1 ' 現在のStatus位置

    Public Sub New()
        'statusList.add(New Status)
        ' TODO:クラスを作成した時点で初期値のStatusを追加しておく。そうすることでundoをしたときにlistの0番目をそのまま使用することができる。
    End Sub


    Public Sub addStatus(ByVal status As Status)
        SyncLock statusList.SyncRoot
            statusIdx = statusIdx + 1
            If statusIdx < statusList.Count -1 Then ' 現状指し示す先にListが存在するか(undoした状態か？そうなら上書き
                statusList.RemoveRange(statusIdx, statusList.Count - statusIdx) ' redo先を上書きするため、それ以降をすべて削除
            End If

            statusList.Add(status)
        End SyncLock
    End Sub

    'TODO:デフォルト引数に変数は使えない？
    Public Function getStatus(Optional ByVal idx As Integer = statusIdx) As Status
        Return statusList.Item(idx)
    End Function

    Public Function getStatusIdx() As Integer
        Return statusIdx
    End Function    

    Public Function getMaxIdx() As Integer
        Return statusList.Count
    End Function


    ' undo
    Private Function undoStatus() As Status
        If statusIdx = 0 Then ' undo先がない？
            ' 初期化
            colorSet(DefaultBackColor, 0)
            getButtonOfIdx(0).Text = ""
            statusIdx = -1
            Return New Status
        ElseIf statusIdx < 0 Then
            Return Null
        End If

        statusIdx = statusIdx - 1

        ' 情報の初期化(戻る前の処理)
        Dim buttonIdx As Integer = getIdxOfButton(getStatus(statuIdx).buttonRow, getStatus(statusIdx).buttonColumn)
        ' TODO:Formの内容をここからいじれないのでは？
        colorSet(DefaultBackColor, buttonIdx)
        getButtonOfIdx(buttonIdx).Text = ""

        ' 1つ前の値を取得(戻る処理)
        Return getStatus(statusIdx)

    End Function

    ' redo
    Private Function redoStatus() As Status
        If statusIdx >= statusList.Count - 1 Then ' redo先がない？
            Return Null
        End If

        statusIdx = statusIdx + 1

        ' 1つ後の値を取得
        Dim statusTmp As Status = getStatus(statusIdx)

        ' 情報の初期化
        Dim buttonIdx As Integer = getIdxOfButton(statusTmp.buttonRow, statusTmp.buttonColumn)
        getButtonOfIdx(buttonIdx).Text = statusTmp.gameStatus ' 現状のマスにテキストを書き直して、次
        colorSet(statuTmp.color, buttonIdx) ' 現状のマスを塗り直して、次


    End Function

End Class