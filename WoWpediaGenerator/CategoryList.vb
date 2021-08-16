Public Class CategoryList
    Public Class Categorymember
        Public Property pageid As Integer
        Public Property ns As Integer
        Public Property title As String
    End Class

    Public Class Query
        Public Property categorymembers As List(Of Categorymember)
    End Class

    Public Class Rootobject
        Public Property batchcomplete As String
        Public Property query As Query
    End Class

End Class

