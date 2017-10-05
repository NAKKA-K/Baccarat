Public Class StatusListMgr
    Dim statusList As ArrayList = ArrayList.Synchronized(New ArrayList)
    Dim statusIdx As Integer = -1 ' 現在のStatus位置

    Public Sub New()
        statusList.Add(New Status)
        statusIdx = statusIdx + 1

        ' TODO:クラスを作成した時点で初期値のStatusを追加しておく。そうすることでundoをしたときにlistの0番目をそのまま使用することができる。
    End Sub


    Public Sub addStatus(ByVal status As Status)
        SyncLock statusList.SyncRoot
            If statusIdx < getListEndIndex() Then ' 現状指し示す先にListが存在するか(undoした状態か？そうなら上書き
                statusList.RemoveRange(statusIdx + 1, getListEndIndex() - statusIdx) ' redo先を上書きするため、それ以降をすべて削除
            End If

            statusList.Add(status)
            statusIdx = statusIdx + 1
        End SyncLock
    End Sub

    'TODO:デフォルト引数に変数は使えない？
    Public Function getStatus(ByVal idx As Integer) As Status
        Return statusList.Item(idx)
    End Function

    Public Function getStatusIdx() As Integer
        Return statusIdx
    End Function

    Public Function getCount() As Integer
        Return statusList.Count
    End Function

    Public Function getListEndIndex() As Integer
        Return statusList.Count - 1
    End Function

    ' undo
    Public Function undoStatus() As Status
        If statusIdx <= 0 Then ' undo先がない？
            Return Nothing
        End If

        ' 情報の初期化(戻る前の処理)
        Dim buttonIdx As Integer = getStatus(statusIdx).getIdxOfButton()
        getFormButton(buttonIdx).BackColor = Control.DefaultBackColor
        getFormButton(buttonIdx).Text = ""

        statusIdx = statusIdx - 1
        Return getStatus(statusIdx)
    End Function

    ' redo
    Public Function redoStatus() As Status
        If statusIdx >= getListEndIndex() Then ' redo先がない？
            Return Nothing
        End If

        ' 1つ後の値を取得
        statusIdx = statusIdx + 1
        Dim statusTmp As Status = getStatus(statusIdx)

        ' 情報の初期化
        Dim buttonIdx As Integer = statusTmp.getIdxOfButton()
        getFormButton(buttonIdx).Text = statusTmp.gameStatus ' 現状のマスにテキストを書き直して、次
        getFormButton(buttonIdx).BackColor = statusTmp.color ' 現状のマスを塗り直して、次

        Return statusTmp
    End Function


    ' Form1のボタン取得するメソッド
    Private Shared Function getFormButton(ByVal buttonIdx As Integer) As Button
        Return Form1.Controls("Button" & buttonIdx.ToString)
    End Function

End Class