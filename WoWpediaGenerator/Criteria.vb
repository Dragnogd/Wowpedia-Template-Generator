Public Class Criteria
    Implements IComparable

    Public Property Title
    Public Property Order

    Public Property ID

    Private Function IComparable_CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim Criteria = CType(obj, Criteria)

        If Not Criteria Is Nothing Then
            Return Me.Order.CompareTo(Criteria.Order)
        Else
            Throw New ArgumentException("Object is not a BomItem")
        End If
    End Function
End Class
