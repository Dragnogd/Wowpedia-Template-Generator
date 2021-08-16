Public Class ItemRow
    Implements IComparable

    Public Property Name
    Public Property ID
    Public Property Category
    Public Property Materials As New List(Of ItemMaterial)
    Public Property SkillPoints
    Public Property OrangeSkill
    Public Property YellowSkill
    Public Property GreenSkill
    Public Property GraySkill
    Public Property Source

    Private Function IComparable_CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim Item = CType(obj, ItemRow)

        If Not Item Is Nothing Then
            Return Me.Name.CompareTo(Item.Name)
        Else
            Throw New ArgumentException("Object is not a BomItem")
        End If
    End Function
End Class

Public Class ItemMaterial
    Public Material
    Public Quantity
End Class