Public Class Achievement
    Implements IComparable

    Public Property Description_lang
    Public Property Title_lang
    Public Property Reward_lang
    Public Property ID
    Public Property Instance_ID
    Public Property Faction
    Public Property Supercedes
    Public Property Category
    Public Property Minimum_criteria
    Public Property Points
    Public Property Flags
    Public Property Ui_order As Integer
    Public Property IconFileID
    Public Property RewardItemID
    Public Property Criteria_tree
    Public Property Shares_criteria
    Public Property TextureFilename

    Private Function IComparable_CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim Achievement = CType(obj, Achievement)

        If Not Achievement Is Nothing Then
            If Form1.chkDungeonRaidsTable.Checked Then
                Dim Sort1 = Me.Instance_ID.CompareTo(Achievement.Instance_ID)
                If Sort1 = 0 Then Sort1 = Me.Ui_order.CompareTo(Achievement.Ui_order)
                Return Sort1
            Else
                Return Me.Ui_order.CompareTo(Achievement.Ui_order)
            End If
        Else
            Throw New ArgumentException("Object is not a BomItem")
        End If
    End Function
End Class
