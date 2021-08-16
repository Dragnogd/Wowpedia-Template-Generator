Public Class SupercedeList
    Implements IComparable

    Public Property Title
    Public Property Order

    Private Function IComparable_CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim SupercedeList = CType(obj, SupercedeList)

        If Not SupercedeList Is Nothing Then
            Return Me.Order.CompareTo(SupercedeList.Order)
        Else
            Throw New ArgumentException("Object is not a BomItem")
        End If
    End Function
End Class
