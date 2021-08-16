Public Class StringSorter
    Implements IComparable

    Public Property URL
    Public Property Text
    Public Property Length

    Private Function IComparable_CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim StringSorter = CType(obj, StringSorter)

        If Not StringSorter Is Nothing Then
            Return StringSorter.Length.CompareTo(Me.Length)
        Else
            Throw New ArgumentException("Object is not a BomItem")
        End If
    End Function
End Class
