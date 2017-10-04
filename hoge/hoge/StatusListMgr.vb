Public Class StatusListMgr
    Dim statusList As ArrayList = ArrayList.Synchronized(New ArrayList)
    Dim statusIdx As Integer = -1 ' 現在のStatus位置

    Public Sub addStatus(ByVal status As Status)
        SyncLock statusList.SyncRoot
            statusIdx = statusIdx + 1
            If statusIdx < getMaxIdx() Then ' 現状指し示す先にListが存在するか(undoした状態か？そうなら上書き
                statusList.RemoveRange(statusIdx, statusList.Count - statusIdx) ' redo先を上書きするため、それ以降をすべて削除
            End If

            statusList.Add(status)
        End SyncLock
    End Sub


    Public Function getStatus(ByVal idx As Integer = statusIdx) As Status
        Return statusList.Item(idx)
    End Function

    Public Function getStatusIdx() As Integer
        Return statusIdx
    End Function    

    Public Function getMaxIdx() As Integer
        Return statusList.Count
    End Function
End Class