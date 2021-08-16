Imports System.IO
Imports System.Text.RegularExpressions
Imports HtmlAgilityPack
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Form1
    Dim Achievements As New List(Of Achievement)
    Dim MapList As New List(Of Maps)
    Dim CriteriaTreeList As New List(Of WoWCriteriaTree)
    Dim CriteriaList As New List(Of WoWCriteria)
    Dim NPCList As New List(Of WoWNPC)
    Dim ExtraInfo = Nothing
    Dim CurrentAchievement As String
    Dim Blacklist As New List(Of String)

    Public CurrentPatch = "9.1.0"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WebWowpedia.Source = New Uri("https://wowpedia.fandom.com/")
        Me.WindowState = FormWindowState.Maximized

        Using reader As StreamReader = New StreamReader("blacklist.txt")
            Dim line = reader.ReadLine

            While Not line Is Nothing
                Blacklist.Add(line)

                line = reader.ReadLine
            End While
        End Using

        Dim afile As FileIO.TextFieldParser = New FileIO.TextFieldParser("map.csv")
        afile.TextFieldType = FileIO.FieldType.Delimited
        afile.Delimiters = New String() {","}
        afile.HasFieldsEnclosedInQuotes = True

        ' parse the actual file
        Do While Not afile.EndOfData
            Dim StrArr As String()
            Try
                StrArr = afile.ReadFields
                Try
                    Dim newMap As Maps = New Maps
                    newMap.ID = StrArr(0)
                    newMap.Name = StrArr(2)
                    MapList.Add(newMap)
                Catch ex As Exception

                End Try
            Catch ex As FileIO.MalformedLineException
                Stop
            End Try
        Loop

        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser("achievement.csv")
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim StrArr As String()
            While Not MyReader.EndOfData
                Try
                    StrArr = MyReader.ReadFields()
                    Dim NewAchievement As Achievement = New Achievement
                    NewAchievement.Description_lang = StrArr(0)
                    NewAchievement.Title_lang = StrArr(1)
                    NewAchievement.Reward_lang = StrArr(2)
                    NewAchievement.ID = StrArr(3)
                    NewAchievement.Instance_ID = StrArr(4)
                    NewAchievement.Faction = StrArr(5)
                    NewAchievement.Supercedes = StrArr(6)
                    NewAchievement.Category = StrArr(7)
                    NewAchievement.Minimum_criteria = StrArr(8)
                    NewAchievement.Points = StrArr(9)
                    NewAchievement.Flags = StrArr(10)
                    Dim UIOrder = 0
                    If Integer.TryParse(StrArr(11), UIOrder) Then
                        NewAchievement.Ui_order = UIOrder
                    Else
                        NewAchievement.Ui_order = 999999
                    End If
                    NewAchievement.IconFileID = StrArr(12)
                    NewAchievement.RewardItemID = StrArr(13)
                    NewAchievement.Criteria_tree = StrArr(14)
                    NewAchievement.Shares_criteria = StrArr(15)

                    If Startup.chkLoadInstanceIDS.Checked Then
                        Try
                            If NewAchievement.Instance_ID <= 0 Then
                                For Each map In MapList
                                    If NewAchievement.Description_lang.contains(map.Name) Or NewAchievement.Title_lang.contains(map.Name) Then
                                        If IsNumeric(map.ID) Then
                                            NewAchievement.Instance_ID = map.ID
                                        End If
                                    End If
                                Next
                            End If
                        Catch ex As Exception

                        End Try

                        'Manually set achievements which can't be matched automatically
                        Select Case NewAchievement.Title_lang
                            Case "Call of the Grand Crusade (10 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Call of the Grand Crusade (25 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Upper Back Pain (10 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Upper Back Pain (25 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Not One, But Two Jormungars (10 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Not One, But Two Jormungars (25 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Three Sixty Pain Spike (10 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Three Sixty Pain Spike (25 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Resilience Will Fix It (10 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Salt and Pepper (10 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Salt and Pepper (25 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "The Traitor King (10 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "The Traitor King (25 player)"
                                NewAchievement.Instance_ID = "649"
                            Case "Earth, Wind & Fire (10 player)"
                                NewAchievement.Instance_ID = "624"
                            Case "Earth, Wind & Fire (25 player)"
                                NewAchievement.Instance_ID = "624"
                            Case "Wrap God"
                                NewAchievement.Instance_ID = "1762"
                        End Select
                    End If

                    Achievements.Add(NewAchievement)
                    ComboBox1.Items.Add(NewAchievement.Title_lang & "," & NewAchievement.ID)

                    'Add Category if missing from ComboBox2
                    Dim Found = False
                    For Each item In ComboBox2.Items
                        If item = NewAchievement.Category Then
                            Found = True
                        End If
                    Next

                    If Not Found Then
                        ComboBox2.Items.Add(NewAchievement.Category)
                    End If


                    'Using writer As StreamWriter = New StreamWriter("achievementlist.txt", True)
                    '    writer.WriteLine("*[[" & NewAchievement.Title_lang & "]]")
                    'End Using
                Catch ex As Microsoft.VisualBasic.
                  FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                End Try
            End While
        End Using

        ComboBox2.Items.Add("15076")

        Achievements.Sort()
        ComboBox2.Sorted = True

        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser("criteria.csv")
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim StrArr As String()
            While Not MyReader.EndOfData
                Try
                    StrArr = MyReader.ReadFields()
                    Dim NewCriteria As WoWCriteria = New WoWCriteria
                    NewCriteria.ID = StrArr(0)
                    NewCriteria.Type = StrArr(1)
                    NewCriteria.Asset = StrArr(2)
                    NewCriteria.Modifier_tree_ID = StrArr(3)
                    NewCriteria.Start_event = StrArr(4)
                    NewCriteria.Start_asset = StrArr(5)
                    NewCriteria.Start_timer = StrArr(6)
                    NewCriteria.Fail_event = StrArr(7)
                    NewCriteria.Fail_asset = StrArr(8)
                    NewCriteria.Flags = StrArr(9)
                    NewCriteria.Flags = StrArr(10)
                    NewCriteria.Eligibility_world_state_ID = StrArr(11)

                    CriteriaList.Add(NewCriteria)
                Catch ex As Microsoft.VisualBasic.
                  FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                End Try
            End While
        End Using

        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser("npcdump.lua")
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim StrArr As String()
            While Not MyReader.EndOfData
                Try
                    StrArr = MyReader.ReadFields()
                    Dim NewCriteria As WoWNPC = New WoWNPC
                    NewCriteria.id = StrArr(0)
                    NewCriteria.name = StrArr(1)

                    NPCList.Add(NewCriteria)
                Catch ex As Microsoft.VisualBasic.
                  FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                End Try
            End While
        End Using

        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser("criteriatree.csv")
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim StrArr As String()
            While Not MyReader.EndOfData
                Try
                    StrArr = MyReader.ReadFields()
                    Dim NewCriteria As WoWCriteriaTree = New WoWCriteriaTree
                    NewCriteria.ID = StrArr(0)
                    NewCriteria.Description_lang = StrArr(1)
                    NewCriteria.Parent = StrArr(2)
                    NewCriteria.Amount = StrArr(3)
                    NewCriteria.Operator1 = StrArr(4)
                    NewCriteria.CriteriaID = StrArr(5)
                    NewCriteria.OrderIndex = StrArr(6)
                    NewCriteria.Flags = StrArr(7)

                    CriteriaTreeList.Add(NewCriteria)
                Catch ex As Microsoft.VisualBasic.
                  FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                End Try
            End While
        End Using

        ComboBox1.Sorted = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim template As String = ""

        'Get occurances of comma
        Dim lastIndex = ComboBox1.SelectedItem.ToString.LastIndexOf(",")

        Dim SelectedItem As String = ComboBox1.SelectedItem.ToString.Substring(0, lastIndex)
        Dim SelectedItemID As String = ComboBox1.SelectedItem.ToString.Substring(lastIndex + 1)

        Dim CategoryID = 0

        'Get Category of selected achievement
        For Each achievement In Achievements
            If achievement.ID = SelectedItemID Then
                CategoryID = achievement.Category
            End If
        Next

        'Check if this achievment has a horde and alliance version
        Dim AchievementCount = 0
        For Each achievement2 In Achievements
            If achievement2.Title_lang = SelectedItem Then
                AchievementCount += 1
            End If
        Next

        Dim AllianceTemplate = ""
        Dim HordeTemplate = ""
        Dim PageContents = ""
        Dim PageName = ""
        Dim ElinkAlliance = 0
        Dim ELinkHorde = 0

        For Each achievement In Achievements
            If Not IsNumeric(achievement.Category) Then
                achievement.Category = -1
            End If

            'Alliance Tooltip
            If achievement.Title_lang = SelectedItem And achievement.Faction <> "0" And achievement.Category = CategoryID Then
                Dim sourceString As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/achievement=" & achievement.ID)
                PageName = achievement.Title_lang

                'Achievement Stub
                If CheckBox3.Checked Then
                    'AllianceTemplate &= "{{Stub/Achievement}}" & vbNewLine
                    AllianceTemplate &= "{{stub/PTR|9.1.0}}" & vbNewLine
                End If

                Dim CategoryObject As Object = GetCategory(achievement.Category)

                'Fetch ICON
                Dim Icon As String
                Using reader2 As StreamReader = New StreamReader("listfile.csv")
                    Dim line2 = reader2.ReadLine
                    While Not line2 Is Nothing
                        Dim StrArr2 As String() = line2.Split(";")
                        If StrArr2(0) = achievement.IconFileID Then
                            Icon = StrArr2(1).Split("/")(2)
                            Icon = Icon.Split(".")(0)
                            Exit While
                        End If
                        line2 = reader2.ReadLine
                    End While
                End Using

                'Wowhead Achievement Criteria
                Dim url = "https://www.wowhead.com/achievement=" & achievement.ID
                Dim web = New HtmlWeb()
                Dim doc = web.Load(url)

                'Fetch Achievement Criteria
                TextBox4.Text = ""
                Dim CriteriaCounter = 0
                Dim Criteras As New List(Of Criteria)
                Dim Reward = achievement.Reward_lang

                'Criteria Tree Builder
                sourceString = New System.Net.WebClient().DownloadString("https://www.wowhead.com/achievement=" & achievement.ID)
                For Each criteriaTree In CriteriaTreeList
                    If criteriaTree.Parent = achievement.Criteria_tree And achievement.Criteria_tree <> "0" And criteriaTree.Flags <> "2" Then
                        Dim FoundCriteria = False
                        Dim CriteriaType = "?"
                        If criteriaTree.Description_lang.length > 0 Then
                            'CriteriaTree with description
                            'Check wowhead for additional confirmation
                            FoundCriteria = True
                            If sourceString.ToLower.Contains(criteriaTree.Description_lang.ToLower) Then
                                TextBox4.AppendText(criteriaTree.Description_lang & vbNewLine)
                                CriteriaCounter += 1
                                Dim NewCriteria = New Criteria
                                NewCriteria.Title = criteriaTree.Description_lang
                                NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                Criteras.Add(NewCriteria)
                            End If
                        Else
                            'CriteriaTree without description
                            For Each criteria In CriteriaList
                                If criteriaTree.CriteriaID = criteria.ID Then
                                    If criteria.Type = "8" Then
                                        'Achievement Criteria
                                        'Search achievements for ID found in Critiera Asset field
                                        For Each achievement2 In Achievements
                                            If criteria.Asset = achievement2.ID Then
                                                'Found achievement. Add to table if not done already
                                                FoundCriteria = True
                                                TextBox4.AppendText(achievement2.Title_lang & vbNewLine)
                                                CriteriaCounter += 1
                                                Dim NewCriteria = New Criteria
                                                NewCriteria.Title = achievement2.Title_lang
                                                NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                                Criteras.Add(NewCriteria)
                                            End If
                                        Next
                                    ElseIf criteria.Type = "0" Then
                                        'Kill Creature Criteria
                                        For Each npc In NPCList
                                            If criteria.Asset = npc.id Then
                                                FoundCriteria = True
                                                If Not npc.name.Contains("[DNT]") And Not npc.name.Contains("Credit -") And Not npc.name.Contains("Kill Credit:") Then
                                                    TextBox4.AppendText(npc.name & " slain" & vbNewLine)
                                                    CriteriaCounter += 1
                                                    Dim NewCriteria = New Criteria
                                                    NewCriteria.Title = npc.name & " slain"
                                                    NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                                    Criteras.Add(NewCriteria)
                                                End If
                                            End If
                                        Next
                                    ElseIf criteria.Type = "27" Then
                                        'Complete Quest Criteria
                                        Dim sourceString2 As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/quest=" & criteria.Asset)

                                        Dim QuestTitle As String() = sourceString2.Substring(sourceString2.LastIndexOf("<h1 class=""heading-size-1"">"), 200).Split("</h1>")
                                        Dim Quest = QuestTitle(1)
                                        Quest = Quest.Split(">")(1)
                                        Quest = Quest.Replace("""", "")

                                        TextBox4.AppendText(Quest & vbNewLine)
                                        Application.DoEvents()
                                        FoundCriteria = True
                                        If Quest <> "Quests" Then
                                            CriteriaCounter += 1
                                            Dim NewCriteria = New Criteria
                                            NewCriteria.Title = Quest
                                            NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                            Criteras.Add(NewCriteria)
                                        End If
                                        ResponsiveSleep(200)
                                    End If
                                End If
                            Next
                        End If
                        If FoundCriteria = False Then
                            TextBox4.AppendText("Unmatched criteria error: " & achievement.Title_lang & ", " & CriteriaType & vbNewLine)
                        End If
                    End If
                Next

                'Stubs
                If CheckBox1.Checked Then
                    AllianceTemplate &= "{{Stub/Sl}}" & vbNewLine
                End If

                'Achievement Tooltip
                AllianceTemplate &= "{{#data:Achievementtip" & vbNewLine
                AllianceTemplate &= "|name=" & achievement.Title_lang & vbNewLine
                AllianceTemplate &= "|icon=" & Icon & vbNewLine
                AllianceTemplate &= "|description=" & achievement.Description_lang & vbNewLine

                'Points
                If achievement.Points <> "0" Then
                    AllianceTemplate &= "|points=" & achievement.Points & vbNewLine
                End If

                'Faction
                If achievement.Faction = "0" Then
                    AllianceTemplate &= "|faction=Horde" & vbNewLine
                ElseIf achievement.Faction = "1" Then
                    AllianceTemplate &= "|faction=Alliance" & vbNewLine
                End If

                'Category
                AllianceTemplate &= "|category=" & CategoryObject(0) & vbNewLine

                'Reward
                If achievement.Reward_lang.length > 0 Then
                    achievement.Reward_lang = achievement.Reward_lang.replace("Toy Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Mount Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Pet Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Title Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Reward: ", "")
                    AllianceTemplate &= "|reward=[[" & achievement.Reward_lang & "]]" & vbNewLine
                End If

                'Account Wide Progress
                If sourceString.Contains("Account-wide progress") Then
                    AllianceTemplate &= "|account=" & vbNewLine
                End If

                'Meta Achievement
                If sourceString.Contains("meta-achievement") Then
                    AllianceTemplate &= "|meta=" & vbNewLine
                End If

                'Achievement Criteria if it has one
                If CriteriaCounter > 0 Then
                    Dim Remainder = CriteriaCounter Mod 2
                    Dim Division = Math.Truncate(CriteriaCounter / 2)
                    Dim Header1Added = False
                    Dim Header2Added = False

                    Dim CurrentColumn = 1
                    Dim Column1 As New List(Of String)
                    Dim Column2 As New List(Of String)
                    Criteras.Sort()
                    For Each Criteria In Criteras
                        If CurrentColumn = 1 Then
                            CurrentColumn = 2
                            If CheckBox2.Checked Then
                                Column1.Add("*[[" & Criteria.Title & "]]" & vbNewLine)
                            Else
                                Column1.Add("*" & Criteria.Title & vbNewLine)
                            End If
                        ElseIf CurrentColumn = 2 Then
                            CurrentColumn = 1
                            If CheckBox2.Checked Then
                                Column2.Add("*[[" & Criteria.Title & "]]" & vbNewLine)
                            Else
                                Column2.Add("*" & Criteria.Title & vbNewLine)
                            End If
                        End If
                    Next

                    'Generate AllianceTemplate
                    AllianceTemplate &= "|criteria=" & vbNewLine
                    For Each achievement2 In Column1
                        AllianceTemplate &= achievement2
                    Next
                    AllianceTemplate &= "|criteria2=" & vbNewLine
                    For Each achievement2 In Column2
                        AllianceTemplate &= achievement2
                    Next
                End If

                If AchievementCount <> 2 Then
                    AllianceTemplate &= "}}" & vbNewLine

                Else
                    AllianceTemplate &= "}}"
                End If

                If achievement.Faction = "0" Then
                    'Horde
                    ELinkHorde = achievement.ID
                ElseIf achievement.Faction = "1" Then
                    'Alliance
                    ElinkAlliance = achievement.ID
                End If
            End If

            'Horde Tooltip
            If achievement.Title_lang = SelectedItem And achievement.Faction = "0" And achievement.Category = CategoryID Then
                Dim sourceString As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/achievement=" & achievement.ID)

                Dim CategoryObject As Object = GetCategory(achievement.Category)

                'Fetch ICON
                Dim Icon As String
                Using reader2 As StreamReader = New StreamReader("listfile.csv")
                    Dim line2 = reader2.ReadLine
                    While Not line2 Is Nothing
                        Dim StrArr2 As String() = line2.Split(";")
                        If StrArr2(0) = achievement.IconFileID Then
                            Icon = StrArr2(1).Split("/")(2)
                            Icon = Icon.Split(".")(0)
                            Exit While
                        End If
                        line2 = reader2.ReadLine
                    End While
                End Using

                'Wowhead Achievement Criteria
                Dim url = "https://www.wowhead.com/achievement=" & achievement.ID
                Dim web = New HtmlWeb()
                Dim doc = web.Load(url)

                'Fetch Achievement Criteria
                TextBox4.Text = ""
                Dim CriteriaCounter = 0
                Dim Criteras As New List(Of Criteria)
                Dim Reward = achievement.Reward_lang

                'Criteria Tree Builder
                sourceString = New System.Net.WebClient().DownloadString("https://www.wowhead.com/achievement=" & achievement.ID)
                For Each criteriaTree In CriteriaTreeList
                    If criteriaTree.Parent = achievement.Criteria_tree And achievement.Criteria_tree <> "0" And criteriaTree.Flags <> "2" Then
                        Dim FoundCriteria = False
                        Dim CriteriaType = "?"
                        If criteriaTree.Description_lang.length > 0 Then
                            'CriteriaTree with description
                            'Check wowhead for additional confirmation
                            FoundCriteria = True
                            If sourceString.ToLower.Contains(criteriaTree.Description_lang.ToLower) Then
                                TextBox4.AppendText(criteriaTree.Description_lang & vbNewLine)
                                CriteriaCounter += 1
                                Dim NewCriteria = New Criteria
                                NewCriteria.Title = criteriaTree.Description_lang
                                NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                Criteras.Add(NewCriteria)
                            End If
                        Else
                            'CriteriaTree without description
                            For Each criteria In CriteriaList
                                If criteriaTree.CriteriaID = criteria.ID Then
                                    If criteria.Type = "8" Then
                                        'Achievement Criteria
                                        'Search achievements for ID found in Critiera Asset field
                                        For Each achievement2 In Achievements
                                            If criteria.Asset = achievement2.ID Then
                                                'Found achievement. Add to table if not done already
                                                FoundCriteria = True
                                                TextBox4.AppendText(achievement2.Title_lang & vbNewLine)
                                                CriteriaCounter += 1
                                                Dim NewCriteria = New Criteria
                                                NewCriteria.Title = achievement2.Title_lang
                                                NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                                Criteras.Add(NewCriteria)
                                            End If
                                        Next
                                    ElseIf criteria.Type = "0" Then
                                        'Kill Creature Criteria
                                        For Each npc In NPCList
                                            If criteria.Asset = npc.id Then
                                                FoundCriteria = True
                                                If Not npc.name.Contains("[DNT]") And Not npc.name.Contains("Credit -") And Not npc.name.Contains("Kill Credit:") Then
                                                    TextBox4.AppendText(npc.name & " slain" & vbNewLine)
                                                    CriteriaCounter += 1
                                                    Dim NewCriteria = New Criteria
                                                    NewCriteria.Title = npc.name & " slain"
                                                    NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                                    Criteras.Add(NewCriteria)
                                                End If
                                            End If
                                        Next
                                    ElseIf criteria.Type = "27" Then
                                        'Complete Quest Criteria
                                        Dim sourceString2 As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/quest=" & criteria.Asset)

                                        Dim QuestTitle As String() = sourceString2.Substring(sourceString2.LastIndexOf("<h1 class=""heading-size-1"">"), 200).Split("</h1>")
                                        Dim Quest = QuestTitle(1)
                                        Quest = Quest.Split(">")(1)
                                        Quest = Quest.Replace("""", "")

                                        TextBox4.AppendText(Quest & vbNewLine)
                                        Application.DoEvents()
                                        FoundCriteria = True
                                        If Quest <> "Quests" Then
                                            CriteriaCounter += 1
                                            Dim NewCriteria = New Criteria
                                            NewCriteria.Title = Quest
                                            NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
                                            Criteras.Add(NewCriteria)
                                        End If
                                        ResponsiveSleep(200)
                                    End If
                                End If
                            Next
                        End If
                        If FoundCriteria = False Then
                            TextBox4.AppendText("Unmatched criteria error: " & achievement.Title_lang & ", " & CriteriaType & vbNewLine)
                        End If
                    End If
                Next

                'Achievement Tooltip
                HordeTemplate &= "{{#data:#Horde" & vbNewLine
                HordeTemplate &= "|name=" & achievement.Title_lang & vbNewLine
                HordeTemplate &= "|icon=" & Icon & vbNewLine
                HordeTemplate &= "|description=" & achievement.Description_lang & vbNewLine

                'Points
                If achievement.Points <> "0" Then
                    HordeTemplate &= "|points=" & achievement.Points & vbNewLine
                End If

                'Faction
                If achievement.Faction = "0" Then
                    HordeTemplate &= "|faction=Horde" & vbNewLine
                ElseIf achievement.Faction = "1" Then
                    HordeTemplate &= "|faction=Alliance" & vbNewLine
                End If

                'Category
                HordeTemplate &= "|category=" & CategoryObject(0) & vbNewLine

                'Reward
                If achievement.Reward_lang.length > 0 Then
                    achievement.Reward_lang = achievement.Reward_lang.replace("Toy Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Mount Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Pet Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Title Reward: ", "")
                    achievement.Reward_lang = achievement.Reward_lang.replace("Reward: ", "")
                    HordeTemplate &= "|reward=[[" & achievement.Reward_lang & "]]" & vbNewLine
                End If

                'Account Wide Progress
                If sourceString.Contains("Account-wide progress") Then
                    HordeTemplate &= "|account=" & vbNewLine
                End If

                'Meta Achievement
                If sourceString.Contains("meta-achievement") Then
                    HordeTemplate &= "|meta=" & vbNewLine
                End If

                'Achievement Criteria if it has one
                If CriteriaCounter > 0 Then
                    Dim Remainder = CriteriaCounter Mod 2
                    Dim Division = Math.Truncate(CriteriaCounter / 2)
                    Dim Header1Added = False
                    Dim Header2Added = False

                    Dim CurrentColumn = 1
                    Dim Column1 As New List(Of String)
                    Dim Column2 As New List(Of String)
                    Criteras.Sort()
                    For Each Criteria In Criteras
                        If CurrentColumn = 1 Then
                            CurrentColumn = 2
                            If CheckBox2.Checked Then
                                Column1.Add("*[[" & Criteria.Title & "]]" & vbNewLine)
                            Else
                                Column1.Add("*" & Criteria.Title & vbNewLine)
                            End If
                        ElseIf CurrentColumn = 2 Then
                            CurrentColumn = 1
                            If CheckBox2.Checked Then
                                Column2.Add("*[[" & Criteria.Title & "]]" & vbNewLine)
                            Else
                                Column2.Add("*" & Criteria.Title & vbNewLine)
                            End If
                        End If
                    Next

                    'Generate HordeTemplate
                    HordeTemplate &= "|criteria=" & vbNewLine
                    For Each achievement2 In Column1
                        HordeTemplate &= achievement2
                    Next
                    HordeTemplate &= "|criteria2=" & vbNewLine
                    For Each achievement2 In Column2
                        HordeTemplate &= achievement2
                    Next
                End If

                HordeTemplate &= "}}" & vbNewLine

                If achievement.Faction = "0" Then
                    'Horde
                    ELinkHorde = achievement.ID
                ElseIf achievement.Faction = "1" Then
                    'Alliance
                    ElinkAlliance = achievement.ID
                End If
            End If

            'Page Description
            If achievement.Title_lang = SelectedItem And Not PageContents.Length > 1 And achievement.Category = CategoryID Then
                Dim CategoryObject As Object = GetCategory(achievement.Category)
                Dim sourceString = New System.Net.WebClient().DownloadString("https://www.wowhead.com/achievement=" & achievement.ID)

                PageContents &= vbNewLine

                'Stubs
                If achievement.Category = "161" Then
                    PageContents &= "{{WorldEvent/Midsummer Fire Festival|doc=}}" & vbNewLine
                    PageContents &= vbNewLine
                End If

                'Achievement Description
                Dim NewDescription = achievement.Description_lang
                NewDescription = NewDescription.replace("Defeat", "defeating")
                NewDescription = NewDescription.replace("Complete", "completing")
                NewDescription = NewDescription.replace("Slay", "slaying")
                NewDescription = NewDescription.replace("Collect", "collecting")
                NewDescription = NewDescription.replace("Kill", "killing")
                NewDescription = NewDescription.replace("Reach", "reaching")
                NewDescription = NewDescription.replace("Win", "winning")
                NewDescription = NewDescription.replace("Achieve", "achieving")
                NewDescription = NewDescription.replace("Fish", "fishing")
                NewDescription = NewDescription.replace("Destroy", "destroying")
                NewDescription = NewDescription.replace("Catch", "catching")
                NewDescription = NewDescription.replace("Obtain", "obtaining")
                NewDescription = NewDescription.replace("Rescue", "rescuing")

                If achievement.Category = "15301" Or CategoryObject(2) = "15301" Or achievement.Category = "97" Or CategoryObject(2) = "97" Then
                    PageContents &= "'''" & achievement.Title_lang & "''' is an " & CategoryObject(1) & " earned by " & NewDescription & vbNewLine
                Else
                    PageContents &= "'''" & achievement.Title_lang & "''' is a " & CategoryObject(1) & " earned by " & NewDescription & vbNewLine
                End If
                PageContents &= vbNewLine

                'Get Patch the achievement was added
                Dim PatchAdded As String()
                Dim Patch As String
                Try
                    PatchAdded = sourceString.Substring(sourceString.LastIndexOf("Added in patch"), 30).Split(".")
                    Patch = PatchAdded(0).Split(" ")(3) & "." & PatchAdded(1) & "." & PatchAdded(2)

                    If Patch = "9.0.1" Then
                        Patch = "9.0.2"
                    End If
                Catch ex As Exception
                    Patch = "ERROR"
                End Try

                'Criteria of
                Dim Parent = Nothing
                If sourceString.Contains("criteria-of") Then
                    For Each criteria In CriteriaList
                        If criteria.Asset = achievement.ID Then
                            For Each criteriatree In CriteriaTreeList
                                If criteria.ID = criteriatree.CriteriaID Then
                                    For Each criteriatree2 In CriteriaTreeList
                                        If criteriatree.Parent = criteriatree2.ID Then
                                            For Each achievement2 In Achievements
                                                If criteriatree2.ID = achievement2.Criteria_tree Then
                                                    PageContents &= "==Criteria of==" & vbNewLine
                                                    PageContents &= "*[[" & achievement2.Title_lang & "]]" & vbNewLine
                                                    PageContents &= vbNewLine
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If

                'Series Builder
                Dim SupercedeLister As New List(Of SupercedeList)
                If sourceString.Contains("infobox-series") Then
                    PageContents &= "==Series==" & vbNewLine
                    Dim NewSupercede As SupercedeList = New SupercedeList
                    NewSupercede.Title = achievement.Title_lang
                    NewSupercede.Order = Integer.Parse(achievement.Ui_order)
                    SupercedeLister.Add(NewSupercede)

                    'Go Up the supercedence
                    Dim AchievementID = achievement.ID
                    While True
                        Dim Found = False
                        For Each achievement2 In Achievements
                            If AchievementID = achievement2.Supercedes Then
                                NewSupercede = New SupercedeList
                                NewSupercede.Title = achievement2.Title_lang
                                NewSupercede.Order = Integer.Parse(achievement2.Ui_order)
                                SupercedeLister.Add(NewSupercede)
                                AchievementID = achievement2.ID
                                Found = True
                            End If
                        Next
                        If Found = False Then
                            Exit While
                        End If
                    End While

                    If achievement.Supercedes <> "0" Then
                        'Go down the supercedence
                        Dim AchievementSupercedes = achievement.Supercedes
                        While True
                            Dim Found = False
                            For Each achievement2 In Achievements
                                If AchievementSupercedes = achievement2.ID Then
                                    NewSupercede = New SupercedeList
                                    NewSupercede.Title = achievement2.Title_lang
                                    NewSupercede.Order = Integer.Parse(achievement2.Ui_order)
                                    SupercedeLister.Add(NewSupercede)
                                    AchievementSupercedes = achievement2.Supercedes
                                    Found = True
                                End If
                            Next
                            If Found = False Then
                                Exit While
                            End If
                        End While
                    End If

                    SupercedeLister.Sort()
                    For Each item In SupercedeLister
                        PageContents &= "*[[" & item.Title & "]]" & vbNewLine
                    Next
                    PageContents &= vbNewLine
                End If

                'Patch
                PageContents &= "==Patch changes==" & vbNewLine
                PageContents &= "*{{Patch " & Patch & "|note=Added.}}" & vbNewLine
                PageContents &= vbNewLine

                If AchievementCount < 2 Then
                    'External Links
                    PageContents &= "==External links==" & vbNewLine
                    PageContents &= "<!-- Please read https://wow.gamepedia.com/Wowpedia:External_links_policy before adding new links. -->" & vbNewLine
                    PageContents &= "{{Elinks-achievement|" & achievement.ID & "}}"
                    PageContents &= vbNewLine
                End If


                PageContents = PageContents.Replace("Torghast, Tower of the Damned", "[[Torghast, Tower of the Damned]]")

                'Replace NPC names with links
                'Using reader As StreamReader = New StreamReader("NPCDB.csv")
                '    Dim line = reader.ReadLine

                '    While Not line Is Nothing
                '        Dim strArr() As String = line.Split(";")
                '        If PageContents.Contains(strArr(1)) Then
                '            PageContents = PageContents.Replace(strArr(1), "[[" & strArr(1) & "]]")
                '        End If
                '        line = reader.ReadLine
                '    End While
                'End Using
            End If
        Next

        If AchievementCount >= 2 Then
            'External Links
            Dim ELINK = ""
            ELINK &= "==External links==" & vbNewLine
            ELINK &= "<!-- Please read https://wow.gamepedia.com/Wowpedia:External_links_policy before adding new links. -->" & vbNewLine
            If ElinkAlliance <> ELinkHorde Then
                ELINK &= "{{Elinks-achievement|h=" & ELinkHorde & "|a=" & ElinkAlliance & "}}"
            Else
                ELINK &= "{{Elinks-achievement|" & ElinkAlliance & "}}"
            End If

            TextBox1.Text = "{{FactionSwitch}}" & AllianceTemplate & HordeTemplate & PageContents & ELINK & vbNewLine & "{{versions/Achievement}}"
        Else
            TextBox1.Text = AllianceTemplate & PageContents
        End If

        Dim Address = New Uri("https://wow.gamepedia.com/" & PageName & "?action=edit")
        CurrentAchievement = PageName
        WebWowpedia.Source = Address
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For Each achievement In Achievements
            If achievement.Category = "15285" Then
                'Fetch Instance
                Dim Instance As String
                Using reader2 As StreamReader = New StreamReader("map.csv")
                    Dim line2 = reader2.ReadLine
                    While Not line2 Is Nothing
                        Dim StrArr2 As String() = line2.Split(",")
                        If StrArr2(0) = achievement.Instance_ID Then
                            Instance = StrArr2(2)
                            Exit While
                        End If
                        line2 = reader2.ReadLine
                    End While
                End Using

                TextBox4.AppendText(Instance & "," & achievement.Title_lang & "," & achievement.Description_lang & ",10" & vbNewLine)
            End If
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each achievement In Achievements
            'Get Parent Category
            Dim ParentID = ""
            Using reader As StreamReader = New StreamReader("achievement_category.csv")
                Dim line = reader.ReadLine
                While Not line Is Nothing
                    Dim StrArr As String() = line.Split(",")
                    If StrArr(1) = achievement.Category Then
                        ParentID = StrArr(2)
                        Using writer As StreamWriter = New StreamWriter("achievementdump.txt", True)
                            writer.WriteLine(achievement.Title_lang)
                            TextBox4.AppendText(achievement.Title_lang & vbNewLine)
                            Application.DoEvents()
                        End Using
                    End If
                    line = reader.ReadLine
                End While
            End Using
        Next

        Process.Start("achievementdump.txt")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        GenerateListOfAchievements(ComboBox2.SelectedItem)
    End Sub

    Public Sub ResponsiveSleep(ByRef iMilliSeconds As Integer)
        Dim i As Integer, iHalfSeconds As Integer = iMilliSeconds / 500
        For i = 1 To iHalfSeconds
            Threading.Thread.Sleep(500) : Application.DoEvents()
        Next i
    End Sub

    Private Sub btnInsertToWowpedia_Click(sender As Object, e As EventArgs) Handles btnInsertToWowpedia.Click
        Dim script = "document.getElementById('wpTextbox1').value = " & "`" & TextBox1.Text & "`"
        WebWowpedia.ExecuteScriptAsync(script)
        script = "document.getElementById('wpSummary').value = " & "`Auto generated for Patch " & CurrentPatch & " " & DateTime.Now & "`"
        WebWowpedia.ExecuteScriptAsync(script)
        script = "document.querySelectorAll('[name=""wpSave""]')[0].click();"
        WebWowpedia.ExecuteScriptAsync(script)
    End Sub

    Private Sub btnAchievement_Click(sender As Object, e As EventArgs) Handles btnAchievement.Click
        Dim Address = New Uri("https://wow.gamepedia.com/" & CurrentAchievement & " (achievement)?action=edit")
        WebWowpedia.Source = Address
    End Sub

    Function OldCode()
        'For Each criteriaTree In CriteriaTreeList
        '    If criteriaTree.Parent = achievement.Criteria_tree And achievement.Criteria_tree <> "0" And criteriaTree.Flags <> "2" Then
        '        Dim FoundCriteria = False
        '        Dim CriteriaType = "?"
        '        If criteriaTree.Description_lang.length > 0 Then
        '            'CriteriaTree with description
        '            'Check wowhead for additional confirmation
        '            FoundCriteria = True
        '            If sourceString.ToLower.Contains(criteriaTree.Description_lang.ToLower) Then
        '                TextBox4.AppendText(criteriaTree.Description_lang & vbNewLine)
        '                CriteriaCounter += 1
        '                Dim NewCriteria = New Criteria
        '                NewCriteria.Title = criteriaTree.Description_lang
        '                NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
        '                Criteras.Add(NewCriteria)
        '            End If
        '        Else
        '            'CriteriaTree without description
        '            For Each criteria In CriteriaList
        '                If criteriaTree.CriteriaID = criteria.ID Then
        '                    If criteria.Type = "8" Then
        '                        'Achievement Criteria
        '                        'Search achievements for ID found in Critiera Asset field
        '                        For Each achievement2 In Achievements
        '                            If criteria.Asset = achievement2.ID Then
        '                                'Found achievement. Add to table if not done already
        '                                FoundCriteria = True
        '                                TextBox4.AppendText(achievement2.Title_lang & vbNewLine)
        '                                CriteriaCounter += 1
        '                                Dim NewCriteria = New Criteria
        '                                NewCriteria.Title = achievement2.Title_lang
        '                                NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
        '                                Criteras.Add(NewCriteria)
        '                            End If
        '                        Next
        '                    ElseIf criteria.Type = "0" Then
        '                        'Kill Creature Criteria
        '                        For Each npc In NPCList
        '                            If criteria.Asset = npc.id Then
        '                                FoundCriteria = True
        '                                If Not npc.name.Contains("[DNT]") And Not npc.name.Contains("Credit -") And Not npc.name.Contains("Kill Credit:") Then
        '                                    TextBox4.AppendText(npc.name & " slain" & vbNewLine)
        '                                    CriteriaCounter += 1
        '                                    Dim NewCriteria = New Criteria
        '                                    NewCriteria.Title = npc.name & " slain"
        '                                    NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
        '                                    Criteras.Add(NewCriteria)
        '                                End If
        '                            End If
        '                        Next
        '                    ElseIf criteria.Type = "27" Then
        '                        'Complete Quest Criteria
        '                        Dim sourceString2 As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/quest=" & criteria.Asset)

        '                        Dim QuestTitle As String() = sourceString2.Substring(sourceString2.LastIndexOf("<h1 class=""heading-size-1"">"), 200).Split("</h1>")
        '                        Dim Quest = QuestTitle(1)
        '                        Quest = Quest.Split(">")(1)
        '                        Quest = Quest.Replace("""", "")

        '                        TextBox4.AppendText(Quest & vbNewLine)
        '                        Application.DoEvents()
        '                        FoundCriteria = True
        '                        If Quest <> "Quests" Then
        '                            CriteriaCounter += 1
        '                            Dim NewCriteria = New Criteria
        '                            NewCriteria.Title = Quest
        '                            NewCriteria.Order = Integer.Parse(criteriaTree.OrderIndex)
        '                            Criteras.Add(NewCriteria)
        '                            ResponsiveSleep(200)
        '                        End If
        '                    End If
        '                End If
        '            Next
        '        End If
        '        If FoundCriteria = False Then
        '            TextBox4.AppendText("Unmatched criteria error: " & achievement.Title_lang & ", " & CriteriaType & vbNewLine)
        '        End If
        '    End If
        'Next

        'Achievement Criteria if it has one

        'DROPDOWN MENU (DOES NOT WORK WITH AJAXSUB)
        'If CriteriaCounter > 0 Then
        '    Dim CurrentColumn = 1
        '    Dim Column1 As New List(Of String)
        '    Dim Column2 As New List(Of String)
        '    Dim Column3 As New List(Of String)
        '    Dim Template2 = "|" & vbNewLine
        '    Template2 &= "{| class=""mw-collapsible mw-collapsed"" width=""100%""" & vbNewLine
        '    Template2 &= "|colspan=""3""| " & Description & vbNewLine
        '    Criteras.Sort()
        '    For Each Criteria In Criteras
        '        If CurrentColumn = 1 Then
        '            Template2 &= "|-" & vbNewLine
        '            Template2 &= "|" & Criteria.Title & vbNewLine
        '            CurrentColumn = 2
        '        ElseIf CurrentColumn = 2 Then
        '            CurrentColumn = 3
        '            Template2 &= "|" & Criteria.Title & vbNewLine
        '        ElseIf CurrentColumn = 3 Then
        '            CurrentColumn = 1
        '            Template2 &= "|" & Criteria.Title & vbNewLine
        '        End If
        '    Next
        '    Template2 &= "|}"
        '    Description = Template2
        'End If

        'Dim hw As HtmlWeb = New HtmlWeb()
        'Dim doc As HtmlAgilityPack.HtmlDocument = New HtmlAgilityPack.HtmlDocument()
        'doc = hw.Load("https://wow.gamepedia.com/" & WebsiteString)
        '
        'Dim ListofStrings = New List(Of StringSorter)
        'For Each link As HtmlNode In doc.DocumentNode.SelectNodes("//a[@href]")
        '    'Get the value of the HREF attribute
        '    Dim hrefValue As String = link.GetAttributeValue("href", String.Empty)
        '    If hrefValue(0) = "/" And Not hrefValue.Contains("index.php") Then
        '        If link.InnerText.Length > 0 And Not hrefValue.Contains("redlink=1") Then
        '            'Url Cleanup
        '            Dim URL = hrefValue.Replace("/", "")
        '            URL = URL.Replace("_", " ")
        '            URL = URL.Replace("%21", "!")
        '            URL = URL.Replace("%23", "#")
        '            URL = URL.Replace("%27", "'")
        '            URL = URL.Replace("%22", """")
        '            URL = URL.Replace("/", "")
        '            URL = URL.Replace("%3F", "?")

        '            'InnerText Cleanup
        '            Dim InnerText = link.InnerText
        '            InnerText = InnerText.Replace("_", " ")
        '            InnerText = InnerText.Replace("%21", "!")
        '            InnerText = InnerText.Replace("%23", "#")
        '            InnerText = InnerText.Replace("%27", "'")
        '            InnerText = InnerText.Replace("%22", """")
        '            InnerText = InnerText.Replace("/", "")
        '            InnerText = InnerText.Replace("&#91;", "")
        '            InnerText = InnerText.Replace("&#93;", "")
        '            InnerText = InnerText.Replace("%3F", "?")

        '            Dim StringSort = New StringSorter
        '            StringSort.URL = URL
        '            StringSort.Text = InnerText
        '            StringSort.Length = InnerText.Length

        '            Dim Found = False
        '            For Each item In ListofStrings
        '                If item.Text.Trim().ToLower() = StringSort.Text.Trim().ToLower() Then
        '                    Found = True
        '                End If
        '            Next

        '            If Found = False Then
        '                ListofStrings.Add(StringSort)
        '            End If

        '            'Replace string found in reward
        '            If Reward.length > 0 Then
        '                If InnerText = URL Then
        '                    Reward = Reward.replace(InnerText, "[[" & InnerText & "]]")
        '                Else
        '                    InnerText = InnerText.Replace(InnerText, "[[" & URL & "|" & InnerText & "]]")
        '                End If
        '            End If
        '        End If
        '    End If
        'Next

        'ListofStrings.Sort()
        'For Each SString In ListofStrings
        '    If SString.Length > 1 Then
        '        Dim DEBUG = False
        '        'Look through string to make sure there are no smaller links inside bigger links
        '        Dim LinkCount = 0
        '        Dim LinkEndCounter = 0
        '        Dim NewString = ""
        '        Dim DetectedAlternateName = False
        '        Dim StartPosition = 0
        '        Dim CurrentPosition = 0 'Position in original string
        '        Dim ActualPosition = 0 'Position in description
        '        Dim OriginalString = Description

        '        If SString.Text.Contains("Mechagon") Then
        '            Dim tmpStr = ""
        '            For Each SString2 In ListofStrings
        '                tmpStr &= SString2.Text & " : " & SString2.URL & vbNewLine
        '            Next
        '            'MsgBox(tmpStr)
        '            DEBUG = True
        '        End If

        '        'If Not TmpReplace.Contains("[[[[") And Not TmpReplace.Contains("]]]]") Then
        '        If SString.Text = SString.URL Or SString.URL.Contains("(item)") Or SString.URL.Contains("(achievement)") Or SString.URL.Contains("(quest)") Then
        '            'Description = Description.Replace(SString.Text, "[[" & SString.URL & "]]")
        '            Description = Replace(Description, SString.Text, "[[" & SString.URL & "]]", Compare:=CompareMethod.Text)
        '        Else
        '            'Description = Description.Replace(SString.Text, "[[" & SString.URL & "|" & SString.Text & "]]")
        '            Description = Replace(Description, SString.Text, "[[" & SString.URL & "|" & SString.Text & "]]", Compare:=CompareMethod.Text)
        '        End If


        '        For Each character As Char In Description
        '            If CurrentPosition = 200 And DEBUG Then
        '                Label4.Text = ""
        '            End If

        '            If character = "[" Then
        '                LinkCount += 1

        '                If LinkCount = 3 Then
        '                    StartPosition = CurrentPosition
        '                End If
        '            ElseIf character = "]" Then
        '                LinkCount -= 1
        '                DetectedAlternateName = False


        '                If LinkCount = 1 Then
        '                    'Find endinging position in original string
        '                    If DEBUG Then
        '                        Label4.Text = ""
        '                    End If
        '                    Dim CurrentPosition2 = 0
        '                    Dim NewString2 = ""
        '                    For Each character2 As Char In OriginalString
        '                        If CurrentPosition2 >= StartPosition And character2 <> "]" And StartPosition > 0 Then
        '                            NewString2 &= character2
        '                            CurrentPosition += 1
        '                        End If
        '                        CurrentPosition2 += 1

        '                        If character2 = "]" Then
        '                            NewString &= NewString2
        '                            StartPosition = 0
        '                            Exit For
        '                        End If
        '                    Next
        '                End If
        '            ElseIf character = "|" And LinkCount > 2 Then
        '                DetectedAlternateName = True
        '            End If

        '            If (LinkCount > 2 Or LinkEndCounter <> 0) Then
        '                'Link inside link detected. Do not add character and decrement counter

        '                If character = "[" Then
        '                    LinkEndCounter = 2
        '                ElseIf character = "]" Then
        '                    LinkEndCounter -= 1
        '                End If
        '            ElseIf StartPosition = 0 Then
        '                NewString &= character
        '            End If

        '            'Do not increase current position for newly added strings
        '            If Description(ActualPosition) = OriginalString(CurrentPosition) And DetectedAlternateName = False And LinkCount < 3 Then
        '                If CurrentPosition + 1 <= OriginalString.Length - 1 Then
        '                    CurrentPosition += 1
        '                End If
        '            End If
        '            If ActualPosition + 1 <= Description.Length - 1 Then
        '                ActualPosition += 1
        '            End If
        '        Next
        '        Description = NewString
        '    End If
        'Next

        'ResponsiveSleep(300)
    End Function

    Public Sub GenerateCategory(CategoryID As String, Optional categoryFilename As String = "")
        Dim AchievementsAdded As New List(Of String)
        Dim LastInstanceID = 0
        Dim InstanceCount = 0
        Dim template As String = "{{i-note|This list is up to date as of [[Patch 9.1.0]]. Last generated on " & DateTime.Today & ".}}" & vbNewLine
        template &= "{| class=""darktable sortable zebra plainlinks"" align=center" & vbNewLine
        template &= "|-" & vbNewLine
        If chkDungeonRaidsTable.Checked Then
            template &= "!Dungeon !! Title !! class=unsortable | Description !! Reward" & vbNewLine
        ElseIf chkFoS.Checked Then
            template &= "!Title !! class=unsortable | Description !! Reward !! Still Attainable?" & vbNewLine
        Else
            template &= "! Title !! class=unsortable | Description !! Reward" & vbNewLine
        End If
        template &= "|-" & vbNewLine

        'If we are in a dungeon/raid category, sort by InstanceID
        If chkDungeonRaidsTable.Checked Then
            Achievements.Sort()
        End If

        For Each achievement In Achievements
            If achievement.Category = CategoryID Then
                If Not achievement.Title_lang.contains("Hidden Character Achievement") And Not achievement.Title_lang.contains("Hidden Tracking") And Not achievement.Title_lang.Contains("<Hidden>") And Not achievement.Title_lang.Contains("<DND>") And Not achievement.Title_lang.Contains("[DNT]") And Not AchievementsAdded.Contains(achievement.Title_lang) Then
                    'Check if there a seperate Horde/Alliance version of the achievement
                    Dim achievementCount = 0
                    For Each achievement2 In Achievements
                        If achievement2.Title_lang = achievement.Title_lang Then
                            achievementCount += 1
                        End If
                    Next

                    Dim Description As String = achievement.Description_lang

                    'Fetch Achievement Criteria
                    TextBox4.Text = ""
                    Dim CriteriaCounter = 0
                    Dim Criteras As New List(Of Criteria)
                    Dim Reward = achievement.Reward_lang

                    'Criteria Tree Builder
                    Dim sourceString As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/achievement=" & achievement.ID)

                    'Determine correct link for page
                    Dim Webpage = "None"
                    Dim WebpageName = ""
                    Dim WebpageStatus As Integer = 0
                    Dim WebsiteString = achievement.Title_lang
                    WebsiteString = WebsiteString.replace("?", "%3F")

                    'Check if there is a page with (achievement) at end
                    Dim Faction = "None"
                    If achievement.Faction = "0" Then
                        Faction = "Horde"
                    ElseIf achievement.Faction = "1" Then
                        Faction = "Alliance"
                    End If

                    Try
                        sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement)")
                        Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (achievement)"
                        WebpageName = Webpage
                        WebpageStatus = 3
                    Catch ex As Exception
                        Try
                            sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
                            Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)

                            If achievement.ID = "1176" Then
                                ExtraInfo = "100_gold"
                            ElseIf achievement.ID = "1177" Then
                                ExtraInfo = "1000_gold"
                            ElseIf achievement.ID = "1178" Then
                                ExtraInfo = "5000_gold"
                            ElseIf achievement.ID = "1180" Then
                                ExtraInfo = "10000_gold"
                            ElseIf achievement.ID = "1181" Then
                                ExtraInfo = "25000_gold"
                            ElseIf achievement.ID = "5455" Then
                                ExtraInfo = "50000_gold"
                            ElseIf achievement.ID = "5456" Then
                                ExtraInfo = "100000_gold"
                            ElseIf achievement.ID = "6753" Then
                                ExtraInfo = "200000_gold"
                            End If

                            If ExtraInfo <> Nothing Then
                                sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")")
                                Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")")
                                WebpageName = "https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")"
                                WebpageStatus = 10
                            Else
                                sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
                                Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
                                WebpageName = "https://wow.gamepedia.com/" & WebsiteString
                            End If
                        Catch ex2 As Exception

                        End Try
                    End Try

                    If Not Webpage Is Nothing Then
                        'Extract links from respective page
                        Dim ListOfStrings = New List(Of String)

                        If WebpageName.Length < 2 Then
                            WebpageName = "https://wow.gamepedia.com/" & WebsiteString
                        End If

                        Dim newURL = New Uri(WebpageName & "?action=edit")
                        WebWowpedia.Source = newURL
                        ResponsiveSleep(4000)

                        Dim script = "document.getElementById('wpTextbox1').value"
                        Dim scriptExecute = WebWowpedia.ExecuteScriptAsync(script)
                        ResponsiveSleep(1000)
                        Dim result = scriptExecute.Result

                        'Get Links just in tooltip
                        Dim ResultsInTooltip = result.Split("}}")(0)

                        'Get Reward
                        Dim regexString2 = New Regex("\|reward=(.*?)\\n")
                        Dim matchedLink As Match = regexString2.Match(result)

                        If matchedLink.Value <> Nothing Then
                            Reward = matchedLink.Value.Replace("|reward=", "").Replace("\n", "")
                        End If

                        'Main text links
                        Dim regexString = New Regex("\[(.*?)\]")
                        Dim matchedLinks As MatchCollection = regexString.Matches(result)
                        Dim Title = achievement.Title_lang

                        'Populate links
                        For Each link As Match In matchedLinks
                            Dim newLink = link.Value.Replace("[", "").Replace("]", "")
                            If Not ListOfStrings.Contains(newLink) Then
                                If Not newLink.Contains("|category=") Then
                                    ListOfStrings.Add(newLink)
                                End If
                            End If
                        Next

                        'Check for embedded links
                        Dim counter = 0
                        For Each link In ListOfStrings
                            For Each link2 In ListOfStrings
                                If link2.Contains(link) And link2 <> link Then
                                    ListOfStrings.Remove(counter)
                                End If
                            Next
                            counter += 1
                        Next

                        'Replace links
                        For Each link In ListOfStrings
                            Description = Description.Replace(link, "[[" & link & "]]")
                        Next

                        txtWebpageSource.Text = result
                        Application.DoEvents()

                        Dim TitleFinal = ""
                        Dim TitleTemplate = ""
                        If WebpageStatus = 1 Then
                            TitleTemplate = "|[[" & Title & " (achievement) (Horde)]]"
                            TitleFinal = Title & " (achievement) (Horde)"
                        ElseIf WebpageStatus = 2 Then
                            TitleTemplate = "|[[" & Title & " (Horde)]]"
                            TitleFinal = Title & " (Horde)"
                        ElseIf WebpageStatus = 3 Then
                            TitleTemplate = "|[[" & Title & " (achievement)]]"
                            TitleFinal = Title & " (achievement)"
                        ElseIf WebpageStatus = 4 Then
                            TitleTemplate = "|[[" & Title & " (achievement) (Alliance)]]"
                            TitleFinal = Title & " (achievement) (Alliance)"
                        ElseIf WebpageStatus = 5 Then
                            TitleTemplate = "|[[" & Title & " (Alliance)]]"
                            TitleFinal = Title & " (Alliance)"
                        ElseIf WebpageStatus = 10 Then
                            TitleTemplate = "|[[" & Title & " (" & ExtraInfo & ")]]"
                            TitleFinal = Title & " (" & ExtraInfo & ")"
                        Else
                            TitleTemplate = "|[[" & Title & "]]"
                            TitleFinal = Title
                        End If

                        If Not AchievementsAdded.Contains(TitleFinal) And Description.Length > 1 Then
                            AchievementsAdded.Add(TitleFinal)

                            If chkDungeonRaidsTable.Checked Then
                                If LastInstanceID <> achievement.Instance_ID Then
                                    'Update RowSpan if needed
                                    If LastInstanceID <> 0 And InstanceCount > 0 Then
                                        template = template.Replace("%ROWSPAN%", InstanceCount)
                                        InstanceCount = 0
                                    End If

                                    'Get InstanceName
                                    For Each map In MapList
                                        If map.ID = achievement.Instance_ID Then
                                            template &= "! rowspan=""%ROWSPAN%"" |[[" & map.Name & "]]" & vbNewLine
                                        End If
                                    Next
                                End If
                                InstanceCount += 1
                                LastInstanceID = achievement.Instance_ID
                            End If

                            'template &= TitleTemplate

                            If result.ToLower.Contains("unobtainable|") Then
                                template &= TitleTemplate & "<br><span style=""color:#ff0000"">Unobtainable</span>" & vbNewLine
                            Else
                                template &= TitleTemplate & vbNewLine
                            End If

                            If achievementCount = 2 Or result.Contains("#data:#Horde") Then
                                template &= "|{{center|'''Alliance'''}}" & "{{#usingarg:" & TitleFinal & "|description|error}}" & vbNewLine
                            Else
                                template &= "|" & "{{#usingarg:" & TitleFinal & "|description|error}}" & vbNewLine
                            End If

                            If result.Contains("|criteria=") And result.Contains("|criteria2=") Then
                                'Two Criterias
                                template &= "{{col-begin}}" & vbNewLine
                                template &= "{{col|2}}" & vbNewLine
                                template &= "{{#usingarg:" & TitleFinal & "|criteria|error}}" & vbNewLine
                                template &= "{{col|2}}" & vbNewLine
                                template &= "{{#usingarg:" & TitleFinal & "|criteria2|error}}" & vbNewLine
                                template &= "{{col-end}}" & vbNewLine
                            ElseIf result.Contains("|criteria") Then
                                'Single Criteria
                                template &= "{{col-begin}}" & vbNewLine
                                template &= "{{col|1}}" & vbNewLine
                                template &= "{{#usingarg:" & TitleFinal & "|criteria|error}}" & vbNewLine
                                template &= "{{col-end}}" & vbNewLine
                            End If

                            If achievementCount = 2 Or result.Contains("#data:#Horde") Then
                                template &= "{{center|'''Horde'''}}" & "{{#usingarg:" & TitleFinal & "#Horde|description|error}}" & vbNewLine

                                If result.Contains("|criteria=") And result.Contains("|criteria2=") Then
                                    'Two Criterias
                                    template &= "{{col-begin}}" & vbNewLine
                                    template &= "{{col|2}}" & vbNewLine
                                    template &= "{{#usingarg:" & TitleFinal & "#Horde|criteria|error}}" & vbNewLine
                                    template &= "{{col|2}}" & vbNewLine
                                    template &= "{{#usingarg:" & TitleFinal & "#Horde|criteria2|error}}" & vbNewLine
                                    template &= "{{col-end}}" & vbNewLine
                                ElseIf result.Contains("|criteria") Then
                                    'Single Criteria
                                    template &= "{{col-begin}}" & vbNewLine
                                    template &= "{{col|1}}" & vbNewLine
                                    template &= "{{#usingarg:" & TitleFinal & "#Horde|criteria|error}}" & vbNewLine
                                    template &= "{{col-end}}" & vbNewLine
                                End If
                            End If

                            'If Reward.Length > 0 Then
                            '    'Reward = Reward.replace("Mount Reward:", "")
                            '    'Reward = Reward.replace("Reward: ", "")
                            '    'Reward = Reward.replace("You will unlock:<BR>", "")
                            '    'Reward = Reward.replace("You will unlock:<br />", "")
                            '    'Reward = Reward.replace(", ", " and ")

                            '    If achievement.Points > 0 Then
                            '        template &= "|{{cost|ach=" & achievement.Points & "}}" & vbNewLine & Reward & vbNewLine
                            '    Else
                            '        template &= "|" & "{{#usingarg:" & TitleFinal & "|points|error}}" & vbNewLine
                            '    End If
                            'Else
                            '    If achievement.Points > 0 Then
                            '        template &= "|" & "{{#usingarg:" & TitleFinal & "|points|error}}" & vbNewLine
                            '    Else
                            '        template &= "|" & Reward & vbNewLine
                            '    End If
                            'End If

                            If achievement.Points > 0 Then
                                template &= "|{{cost|ach=" & "{{#usingarg:" & TitleFinal & "|points|10}}" & "}}" & vbNewLine
                            Else
                                template &= "|{{cost|ach=" & "{{#usingarg:" & TitleFinal & "|points|0}}" & "}}" & vbNewLine
                            End If

                            If result.ToLower.Contains("|mount=") Then
                                template &= "'''Mount Reward:'''<br>" & vbNewLine
                                template &= "*[[{{#usingarg:" & TitleFinal & "|mount|error}}]]" & vbNewLine
                            End If

                            If result.ToLower.Contains("|pet=") Then
                                template &= "'''Pet Reward:'''<br>" & vbNewLine
                                template &= "*[[{{#usingarg:" & TitleFinal & "|pet|error}}]]" & vbNewLine
                            End If

                            If result.ToLower.Contains("|toy=") Then
                                template &= "'''Toy Reward:'''<br>" & vbNewLine
                                template &= "*[[{{#usingarg:" & TitleFinal & "|toy|error}}]]" & vbNewLine
                            End If

                            If result.ToLower.Contains("|title=") Then
                                template &= "'''Title Reward:'''<br>" & vbNewLine
                                template &= "*{{#usingarg:" & TitleFinal & "|title|error}}" & vbNewLine
                            End If

                            If result.ToLower.Contains("|reward=") Then
                                template &= "'''Reward:'''<br>" & vbNewLine
                                template &= "*{{#usingarg:" & TitleFinal & "|reward|error}}" & vbNewLine
                            End If

                            If chkFoS.Checked Then
                                'WebWowpedia.Source = New Uri("https://wowhead.com/achievement=" & achievement.ID & "#comments")

                                Dim attainable As String
                                'Dim attainable = InputBox("Is this achievement still attainable: " & vbNewLine & Title & vbNewLine & Description)

                                If result.ToLower.Contains("{{unobtainable") Or result.ToLower.Contains("{{classic only") Then
                                    attainable = "No"
                                Else
                                    attainable = "Yes"
                                End If

                                template &= "|" & attainable & vbNewLine
                            End If

                            template &= "|-" & vbNewLine

                            template = template.Replace("\u003C", "<")

                            TextBox1.Text = ""
                            TextBox1.AppendText(template)
                        End If
                    End If
                End If
            End If
        Next

        If chkDungeonRaidsTable.Checked Then
            If LastInstanceID <> 0 And InstanceCount > 0 Then
                TextBox1.Text = TextBox1.Text.Replace("%ROWSPAN%", InstanceCount)
                InstanceCount = 0
            End If
        End If

        TextBox1.AppendText("|}")

        If categoryFilename.Length > 1 Then
            Using writer As StreamWriter = New StreamWriter("C:\Users\ryanc\Desktop\Wowpedia\" & categoryFilename & ".txt")
                writer.WriteLine(TextBox1.Text)
            End Using
        End If

        'TextBox1.Text = template
    End Sub

    Public Sub GenerateListOfAchievements(CategoryID As String, Optional categoryFilename As String = "")

        Dim LastInstanceID = 0
        Dim InstanceCount = 0
        Dim template As String = ""

        Dim URLlist As New List(Of String)

        'If we are in a dungeon/raid category, sort by InstanceID
        If chkDungeonRaidsTable.Checked Then
            Achievements.Sort()
        End If

        'Get list of achievements currently in wowhead category
        Dim CategoryObject As Object = GetCategory(CategoryID)
        Dim CategoryName = CategoryObject(0)
        Dim CategoryURL = "https://wowpedia.fandom.com/wiki/Category:" & CategoryName & " achievements"

        Dim AchievementsAdded As New List(Of String)

        'Dim hw As HtmlWeb = New HtmlWeb()
        'Dim doc As HtmlAgilityPack.HtmlDocument = New HtmlAgilityPack.HtmlDocument()
        'doc = hw.Load(CategoryURL)

        Dim ListofStrings = GetPagesInCategory(CategoryName & " achievements")

        Dim InstanceIDInsertedLast = Nothing
        Dim TextToAppend = ""
        Dim RequireClose = False

        For Each achievement In Achievements
            TextBox2.Text = achievement.Title_lang
            If achievement.Category = CategoryID Then
                If Not Blacklist.Contains(achievement.Title_lang) And Not achievement.Title_lang.contains("Hidden Character Achievement") And Not achievement.Title_lang.contains("Hidden Tracking") And Not achievement.Title_lang.Contains("<Hidden>") And Not achievement.Title_lang.Contains("<DND>") And Not achievement.Title_lang.Contains("[DNT]") And Not AchievementsAdded.Contains(achievement.Title_lang) Then
                    'Check if there a seperate Horde/Alliance version of the achievement
                    Dim achievementCount = 0
                    For Each achievement2 In Achievements
                        If achievement2.Title_lang = achievement.Title_lang Then
                            achievementCount += 1
                        End If
                    Next

                    Dim Description As String = achievement.Description_lang

                    'Fetch Achievement Criteria
                    TextBox4.Text = ""
                    Dim CriteriaCounter = 0
                    Dim Criteras As New List(Of Criteria)
                    Dim Reward = achievement.Reward_lang

                    'Criteria Tree Builder
                    'Dim sourceString As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/achievement=" & achievement.ID)

                    'Determine correct link for page
                    Dim Webpage = "None"
                    Dim WebpageName = ""
                    Dim WebpageStatus As Integer = 0
                    Dim WebsiteString = achievement.Title_lang
                    WebsiteString = WebsiteString.replace("?", "%3F")

                    'Check if there is a page with (achievement) at end
                    Dim Faction = "None"
                    If achievement.Faction = "0" Then
                        Faction = "Horde"
                    ElseIf achievement.Faction = "1" Then
                        Faction = "Alliance"
                    End If

                    If achievement.Instance_ID = "15438" Then
                        Dim stop2 = "1"
                    End If

                    WebsiteString = WebsiteString.Replace("_", " ")
                    WebsiteString = WebsiteString.Replace("%21", "!")
                    WebsiteString = WebsiteString.Replace("%23", "#")
                    WebsiteString = WebsiteString.Replace("%27", "'")
                    WebsiteString = WebsiteString.Replace("%22", """")
                    WebsiteString = WebsiteString.Replace("/", "")
                    WebsiteString = WebsiteString.Replace("&#91;", "")
                    WebsiteString = WebsiteString.Replace("&#93;", "")
                    WebsiteString = WebsiteString.Replace("%3F", "?")

                    For Each link In ListofStrings
                        If link = WebsiteString & " (legacy)" Then
                            Webpage = "https://wowpedia.fandom.com/wiki/" & WebsiteString & " (legacy)"
                            WebpageName = Webpage
                            WebpageStatus = 11
                        ElseIf link = WebsiteString & " (Legacy)" Then
                            Webpage = "https://wowpedia.fandom.com/wiki/" & WebsiteString & " (Legacy)"
                            WebpageName = Webpage
                            WebpageStatus = 13
                        ElseIf link = WebsiteString & " (achievement) (legacy)" Then
                            Webpage = "https://wowpedia.fandom.com/wiki/" & WebsiteString & " (achievement) (legacy)"
                            WebpageName = Webpage
                            WebpageStatus = 12
                        ElseIf link = WebsiteString & " (achievement)" Then
                            Webpage = "https://wowpedia.fandom.com/wiki/" & WebsiteString & " (achievement)"
                            WebpageName = Webpage
                            WebpageStatus = 3
                        End If
                    Next

                    If Webpage = "None" Then
                        ExtraInfo = Nothing
                        If achievement.ID = "1176" Then
                            ExtraInfo = "100_gold"
                        ElseIf achievement.ID = "1177" Then
                            ExtraInfo = "1000_gold"
                        ElseIf achievement.ID = "1178" Then
                            ExtraInfo = "5000_gold"
                        ElseIf achievement.ID = "1180" Then
                            ExtraInfo = "10000_gold"
                        ElseIf achievement.ID = "1181" Then
                            ExtraInfo = "25000_gold"
                        ElseIf achievement.ID = "5455" Then
                            ExtraInfo = "50000_gold"
                        ElseIf achievement.ID = "5456" Then
                            ExtraInfo = "100000_gold"
                        ElseIf achievement.ID = "6753" Then
                            ExtraInfo = "200000_gold"
                        End If

                        If ExtraInfo <> Nothing Then
                            Webpage = "https://wowpedia.fandom.com/wiki/" & WebsiteString & " (" & ExtraInfo & ")"
                            WebpageName = "https://wowpedia.fandom.com/wiki/" & WebsiteString & " (" & ExtraInfo & ")"
                            WebpageStatus = 10
                        Else
                            Webpage = "https://wowpedia.fandom.com/wiki/" & WebsiteString
                            WebpageName = "https://wowpedia.fandom.com/wiki/" & WebsiteString
                        End If
                    End If

                    Dim Title = achievement.Title_lang

                    If Not Webpage Is Nothing Then
                        Dim TitleFinal = ""
                        Dim TitleTemplate = ""
                        If WebpageStatus = 1 Then
                            TitleTemplate = "|[[" & Title & " (achievement) (Horde)]]"
                            TitleFinal = Title & " (achievement) (Horde)"
                        ElseIf WebpageStatus = 2 Then
                            TitleTemplate = "|[[" & Title & " (Horde)]]"
                            TitleFinal = Title & " (Horde)"
                        ElseIf WebpageStatus = 3 Then
                            TitleTemplate = "|[[" & Title & " (achievement)]]"
                            TitleFinal = Title & " (achievement)"
                        ElseIf WebpageStatus = 4 Then
                            TitleTemplate = "|[[" & Title & " (achievement) (Alliance)]]"
                            TitleFinal = Title & " (achievement) (Alliance)"
                        ElseIf WebpageStatus = 5 Then
                            TitleTemplate = "|[[" & Title & " (Alliance)]]"
                            TitleFinal = Title & " (Alliance)"
                        ElseIf WebpageStatus = 10 Then
                            TitleTemplate = "|[[" & Title & " (" & ExtraInfo & ")]]"
                            TitleFinal = Title & " (" & ExtraInfo & ")"
                        ElseIf WebpageStatus = 11 Then
                            TitleTemplate = "|[[" & Title & " (legacy)]]"
                            TitleFinal = Title & " (legacy)"
                        ElseIf WebpageStatus = 12 Then
                            TitleTemplate = "|[[" & Title & " (achievement) (legacy)]]"
                            TitleFinal = Title & " (achievement) (legacy)"
                        ElseIf WebpageStatus = 13 Then
                            TitleTemplate = "|[[" & Title & " (Legacy)]]"
                            TitleFinal = Title & " (Legacy)"
                        Else
                            TitleTemplate = "|[[" & Title & "]]"
                            TitleFinal = Title
                        End If

                        If Not AchievementsAdded.Contains(TitleFinal) And Description.Length > 1 Then
                            AchievementsAdded.Add(TitleFinal)

                            'Escape double quotes. Do not include # symbol as mediawiki do not allow this in titles.
                            TitleFinal = TitleFinal.Replace("""", "\""").Replace("#", "")

                            If chkDungeonRaidsTable.Checked Then
                                Dim instanceName = ""
                                For Each map In MapList
                                    If map.ID = achievement.Instance_ID Then
                                        instanceName = map.Name
                                    End If
                                Next

                                If InstanceIDInsertedLast <> achievement.Instance_ID Then
                                    'Create new item in table with instance name
                                    If Not InstanceIDInsertedLast Is Nothing And RequireClose = True Then
                                        TextBox1.AppendText(vbTab & vbTab & "}," & vbNewLine)
                                        RequireClose = False
                                    End If

                                    TextToAppend = vbTab & vbTab & "[""" & instanceName & """] = {" & vbNewLine & vbTab & vbTab & vbTab & """" & TitleFinal & """," & vbNewLine
                                    InstanceIDInsertedLast = achievement.Instance_ID
                                Else
                                    If TextToAppend.Length > 1 Then
                                        TextBox1.AppendText(TextToAppend)
                                        RequireClose = True
                                        TextToAppend = ""
                                    End If

                                    TextBox1.AppendText(vbTab & vbTab & vbTab & """" & TitleFinal & """," & vbNewLine)
                                    InstanceIDInsertedLast = achievement.Instance_ID
                                End If
                            Else
                                TextBox1.AppendText(vbTab & vbTab & """" & TitleFinal & """," & vbNewLine)
                            End If
                        End If
                    End If
                End If
            End If
        Next

        If chkDungeonRaidsTable.Checked Then
            TextBox1.AppendText(vbTab & vbTab & "}," & vbNewLine)
        End If
    End Sub

    Private Sub btnGenerateAll_Click(sender As Object, e As EventArgs) Handles btnGenerateAll.Click
        Dim StartAt = 168

        Using reader As StreamReader = New StreamReader("AchievementNamedCategories.csv")
            Dim line = reader.ReadLine

            While Not line Is Nothing
                Dim strArr() = line.Split(",")
                txtProcessing.Text = strArr(1) & "_" & strArr(2)

                If strArr(1).Contains("DungeonRaids") And strArr(0) <> "168" Then
                    chkDungeonRaidsTable.Checked = True
                Else
                    chkDungeonRaidsTable.Checked = False
                End If

                If (StartAt > 0 And StartAt = Integer.Parse(strArr(0))) Or StartAt = 0 Then
                    StartAt = 0
                    GenerateCategory(strArr(0), strArr(1) & "_" & strArr(2))

                    WebWowpedia.Source = New Uri("https://wowpedia.fandom.com/" & strArr(3) & "/" & strArr(2) & "?action=edit")

                    ResponsiveSleep(3000)

                    Dim FinalString = "<div class=""ajaxHide"">{{Transclude|" & strArr(3) & "}}</div>" & vbNewLine
                    FinalString &= "<onlyinclude>" & TextBox1.Text & "</onlyinclude>" & vbNewLine
                    FinalString &= "<div class=""ajaxHide"">{{achievements|article}}</div>"

                    Dim script = "document.getElementById('wpTextbox1').value = " & "`" & FinalString & "`"
                    WebWowpedia.ExecuteScriptAsync(script)
                    script = "document.getElementById('wpSummary').value = " & "`Auto generated for Patch " & CurrentPatch & " " & DateTime.Now & "`"
                    WebWowpedia.ExecuteScriptAsync(script)
                    script = "document.querySelectorAll('[name=""wpSave""]')[0].click();"
                    WebWowpedia.ExecuteScriptAsync(script)

                    ResponsiveSleep(10000)
                End If
                line = reader.ReadLine
            End While
        End Using
    End Sub

    Public Sub OldCode2()
        'For Each link As HtmlNode In doc.DocumentNode.SelectNodes("//a[@href]")
        '    'Get the value of the HREF attribute
        '    Dim hrefValue As String = link.GetAttributeValue("href", String.Empty)
        '    If hrefValue(0) = "/" And Not hrefValue.Contains("index.php") Then
        '        If link.InnerText.Length > 0 And Not hrefValue.Contains("redlink=1") Then
        '            'Url Cleanup
        '            Dim URL = hrefValue.Replace("/", "")
        '            URL = URL.Replace("_", " ")
        '            URL = URL.Replace("%21", "!")
        '            URL = URL.Replace("%23", "#")
        '            URL = URL.Replace("%27", "'")
        '            URL = URL.Replace("%22", """")
        '            URL = URL.Replace("/", "")
        '            URL = URL.Replace("%3F", "?")

        '            'InnerText Cleanup
        '            Dim InnerText = link.InnerText
        '            InnerText = InnerText.Replace("_", " ")
        '            InnerText = InnerText.Replace("%21", "!")
        '            InnerText = InnerText.Replace("%23", "#")
        '            InnerText = InnerText.Replace("%27", "'")
        '            InnerText = InnerText.Replace("%22", """")
        '            InnerText = InnerText.Replace("/", "")
        '            InnerText = InnerText.Replace("&#91;", "")
        '            InnerText = InnerText.Replace("&#93;", "")
        '            InnerText = InnerText.Replace("%3F", "?")

        '            Dim StringSort = New StringSorter
        '            StringSort.URL = URL
        '            StringSort.Text = InnerText
        '            StringSort.Length = InnerText.Length

        '            Dim Found = False
        '            For Each item In ListofStrings
        '                If item.Text.Trim().ToLower() = StringSort.Text.Trim().ToLower() Then
        '                    Found = True
        '                End If
        '            Next

        '            If Found = False Then
        '                ListofStrings.Add(StringSort)
        '                URLlist.Add(URL)
        '            End If
        '        End If
        '    End If
        'Next

        'If Faction = "None" Then
        '    Try
        '        sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement)")
        '        Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (achievement)"
        '        WebpageName = Webpage
        '        WebpageStatus = 3
        '    Catch ex As Exception
        '        Try
        '            sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
        '            Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)

        '            If Achievement.ID = "1176" Then
        '                ExtraInfo = "100_gold"
        '            ElseIf Achievement.ID = "1177" Then
        '                ExtraInfo = "1000_gold"
        '            ElseIf Achievement.ID = "1178" Then
        '                ExtraInfo = "5000_gold"
        '            ElseIf Achievement.ID = "1180" Then
        '                ExtraInfo = "10000_gold"
        '            ElseIf Achievement.ID = "1181" Then
        '                ExtraInfo = "25000_gold"
        '            ElseIf Achievement.ID = "5455" Then
        '                ExtraInfo = "50000_gold"
        '            ElseIf Achievement.ID = "5456" Then
        '                ExtraInfo = "100000_gold"
        '            ElseIf Achievement.ID = "6753" Then
        '                ExtraInfo = "200000_gold"
        '            End If

        '            If ExtraInfo <> Nothing Then
        '                sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")")
        '                Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")")
        '                WebpageName = "https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")"
        '                WebpageStatus = 10
        '            Else
        '                sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
        '                Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
        '                WebpageName = "https://wow.gamepedia.com/" & WebsiteString
        '            End If
        '        Catch ex2 As Exception

        '        End Try
        '    End Try
        'ElseIf Faction = "Horde" Then
        '    Try
        '        sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement) (Horde)")
        '        Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (achievement) (Horde)"
        '        WebpageName = Webpage
        '        WebpageStatus = 1
        '    Catch ex As Exception
        '        Try
        '            sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (Horde)")
        '            Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (Horde)"
        '            WebpageName = Webpage
        '            WebpageStatus = 2
        '        Catch ex2 As Exception
        '            Try
        '                sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement)")
        '                Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement)")
        '                WebpageName = "https://wow.gamepedia.com/" & WebsiteString & " (achievement)"
        '                WebpageStatus = 3
        '            Catch ex3 As Exception
        '                Try
        '                    sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
        '                    Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
        '                    WebpageName = "https://wow.gamepedia.com/" & WebsiteString
        '                Catch ex4 As Exception

        '                End Try
        '            End Try
        '        End Try
        '    End Try
        'ElseIf Faction = "Alliance" Then
        '    Try
        '        sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement) (Alliance)")
        '        Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (achievement) (Alliance)"
        '        WebpageName = Webpage
        '        WebpageStatus = 4
        '    Catch ex As Exception
        '        Try
        '            sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (Alliance)")
        '            Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (Alliance)"
        '            WebpageName = Webpage
        '            WebpageStatus = 5
        '        Catch ex2 As Exception
        '            Try
        '                sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement)")
        '                Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement)")
        '                WebpageName = "https://wow.gamepedia.com/" & WebsiteString & " (achievement)"
        '                WebpageStatus = 3
        '            Catch ex3 As Exception
        '                Try
        '                    sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
        '                    Webpage = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
        '                    WebpageName = "https://wow.gamepedia.com/" & WebsiteString
        '                Catch ex4 As Exception

        '                End Try
        '            End Try
        '        End Try
        '    End Try
        'End If
    End Sub

    Private Sub btnGenerateList_Click(sender As Object, e As EventArgs) Handles btnGenerateList.Click
        Dim StartAt = 0

        TextBox1.Text = ""
        TextBox1.AppendText("return {" & vbNewLine)
        TextBox1.AppendText(vbTab & "lastUpdate = ""This list is up to date as of [[Patch" & CurrentPatch & "]]," & vbNewLine)

        Using reader As StreamReader = New StreamReader("AchievementNamedCategories.csv")
            Dim line = reader.ReadLine

            While Not line Is Nothing
                Dim strArr() = line.Split(",")
                txtProcessing.Text = strArr(1) & "_" & strArr(2)

                If strArr(3) = "Dungeons & Raids achievements" And strArr(0) <> "168" And strArr(0) <> "14808" And strArr(0) <> "14805" Then
                    chkDungeonRaidsTable.Checked = True
                Else
                    chkDungeonRaidsTable.Checked = False
                End If

                If (StartAt > 0 And StartAt = Integer.Parse(strArr(0))) Or StartAt = 0 Then
                    StartAt = 0

                    If strArr(2).Length > 1 Then
                        TextBox1.AppendText(vbTab & "[""" & strArr(3).Replace("&", "&#38;").Replace("'", "&#39;").Replace("""", "&#34;") & "/" & strArr(2).Replace("&", "&#38;").Replace("'", "&#39;").Replace("""", "&#34;") & """] = {" & vbNewLine)
                    Else
                        TextBox1.AppendText(vbTab & "[""" & strArr(3).Replace("&", "&#38;").Replace("'", "&#39;").Replace("""", "&#34;") & """] = {" & vbNewLine)
                    End If

                    GenerateListOfAchievements(strArr(0), strArr(1) & "_" & strArr(2))

                    ResponsiveSleep(1000)
                    Application.DoEvents()
                End If

                TextBox1.AppendText(vbTab & "}," & vbNewLine)

                line = reader.ReadLine
            End While
        End Using
    End Sub

    Function GetCategory(categoryID) As Object
        'Fetch Achievement Category
        Dim Category As String
        Using reader2 As StreamReader = New StreamReader("achievement_category.csv")
            Dim line2 = reader2.ReadLine
            While Not line2 Is Nothing
                Dim StrArr2 As String() = line2.Split(",")
                If StrArr2(1) = categoryID Then
                    Category = StrArr2(0)
                    Exit While
                End If
                line2 = reader2.ReadLine
            End While
        End Using

        'Automatic Category Detection
        Dim CategoryText = ""

        'Get Parent Category
        Dim ParentID = ""
        Using reader As StreamReader = New StreamReader("achievement_category.csv")
            Dim line = reader.ReadLine
            While Not line Is Nothing
                Dim StrArr As String() = line.Split(",")
                If StrArr(1) = categoryID Then
                    ParentID = StrArr(2)
                    TextBox5.Text = categoryID
                End If
                line = reader.ReadLine
            End While
        End Using

        If categoryID = "92" Then
            'Character
            CategoryText = "[[Character achievements|character achievement]]"
        ElseIf categoryID = "96" Or ParentID = "96" Then
            'Quests
            CategoryText = "[[Quest achievements|quest achievement]]"
            If Category = "Quests" Then
                Category = "Quest"
            End If

            If ParentID = "96" Then
                Category &= " quest"
            End If
        ElseIf categoryID = "97" Or ParentID = "97" Then
            'Exploration
            CategoryText = "[[Exploration achievements|exploration achievement]]"

            If ParentID = "97" Then
                Category &= " exploration"
            End If
        ElseIf categoryID = "95" Or ParentID = "95" Then
            'Player vs. Player
            CategoryText = "[[Player vs. Player achievements|player vs. player achievement]]"

            If categoryID = "15283" Then
                Category = "Player vs. Player World"
            End If
        ElseIf categoryID = "168" Or ParentID = "168" Then
            'Dungeons & Raids
            CategoryText = "[[Dungeons & Raids achievements|dungeon & raid achievement]]"
        ElseIf categoryID = "169" Or ParentID = "169" Then
            'Dungeons & Raids
            CategoryText = "[[Professions achievements|profession achievement]]"
        ElseIf categoryID = "201" Or ParentID = "201" Then
            'Dungeons & Raids
            CategoryText = "[[Reputation achievements|reputation achievement]]"

            If ParentID = "201" Then
                Category &= " reputation"
            End If
        ElseIf categoryID = "155" Or ParentID = "155" Then
            'World Events
            CategoryText = "[[World Events achievements|world event achievement]]"

            If categoryID = "161" Then
                Category = "Midsummer Fire Festival"
            End If
        ElseIf categoryID = "15117" Or ParentID = "15117" Then
            'Pet Battles
            CategoryText = "[[Pet Battles achievements|pet battle achievement]]"

            If categoryID = "15119" Then
                Category = "Pet battles/Battle"
            ElseIf categoryID = "15118" Then
                Category = "Pet battles/Collect"
            ElseIf categoryID = "15120" Then
                Category = "Pet battles/Level"
            End If
        ElseIf categoryID = "15301" Or ParentID = "15301" Then
            'Expansion Features
            CategoryText = "[[Expansion Features achievements|expansion feature achievement]]"

            If categoryID = "15307" Then
                Category = "Island Expedition"
            End If
        ElseIf categoryID = "15246" Or ParentID = "15246" Then
            'Collections
            CategoryText = "[[Collections achievements|collection achievement]]"

            If categoryID = "15248" Then
                Category = "Collections/Mounts"
            ElseIf categoryID = "15259" Then
                Category = "Collections/Appearances"
            ElseIf categoryID = "15247" Then
                Category = "Collections/Toy Box"
            End If
        ElseIf categoryID = "15234" Or ParentID = "15234" Then
            'Legacy
            CategoryText = "[[Legacy achievements|legacy achievement]]"

            If ParentID = "15234" Then
                Category = "Legacy " & Category
            End If
        ElseIf categoryID = "81" Or ParentID = "81" Then
            'Feats of Strengths
            CategoryText = "[[Feats of Strength achievements|feat of strength achievement]]"

            If Category <> "Feats of Strength" Then
                Category = "Feats of Strength " & Category
            End If
        ElseIf categoryID = "15088" Then
            'Guild General
            CategoryText = "[[Guild achievements#General|guild general achievement]]"
            Category = "General guild"
        ElseIf categoryID = "15077" Then
            'Guild Quests
            CategoryText = "[[Guild achievements#Quests|guild quest achievement]]"
            Category = "Quest guild"
        ElseIf categoryID = "15078" Or ParentID = "15078" Then
            'Guild Player vs. Player
            CategoryText = "[[Guild achievements#Player_vs._Player|guild player vs. player achievement]]"
            Category = "Player vs. Player guild"
        ElseIf categoryID = "15079" Or ParentID = "15079" Then
            'Guild Dungeons & Raids
            CategoryText = "[[Guild achievements#Dungeons_.26_Raids|guild dungeons & raid achievement]]"
            Category = "Dungeons & Raids guild"
        ElseIf categoryID = "15080" Or ParentID = "15080" Then
            'Guild Professions
            CategoryText = "[[Guild achievements#Professions|guild profession achievement]]"
            Category = "Guild professions"
        ElseIf categoryID = "15089" Or ParentID = "15089" Then
            'Guild Reputation
            CategoryText = "[[Guild achievements#Reputation|guild reputation achievement]]"
            Category = "Guild reputation"
        ElseIf categoryID = "15093" Or ParentID = "15093" Then
            'Guild Guild Feats of Strength
            CategoryText = "[[Guild achievements#Guild_Feats_of_Strength|guild feat of strength achievement]]"
            Category = "Guild Feats of Strength"
        End If

        Return {Category, CategoryText, ParentID}
    End Function

    Private Sub btnFixCategories_Click(sender As Object, e As EventArgs) Handles btnFixCategories.Click
        Dim startAt = 15279
        Dim startAtAchievement = ""

        Using reader As StreamReader = New StreamReader("AchievementNamedCategories.csv")
            Dim line = reader.ReadLine

            While Not line Is Nothing
                Dim strArr() = line.Split(",")
                txtProcessing.Text = strArr(1) & "_" & strArr(2)

                If (startAt > 0 And startAt = Integer.Parse(strArr(0))) Or startAt = 0 Then
                    startAt = 0
                    'Get list of achievements currently in wowhead category
                    For Each achievement In Achievements
                        If achievement.Category = strArr(0) And (startAtAchievement = achievement.ID Or startAtAchievement = "") Then
                            startAtAchievement = ""
                            Dim WebsiteString = achievement.Title_lang
                            Dim sourceString
                            Dim Webpage = ""

                            Try
                                sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (legacy)")
                                Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (legacy)"
                            Catch ex2 As Exception
                                Try
                                    sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (Legacy)")
                                    Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (Legacy)"
                                Catch ex3 As Exception
                                    Try
                                        sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (achievement)")
                                        Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (achievement)"
                                    Catch ex4 As Exception
                                        sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
                                        Webpage = "https://wow.gamepedia.com/" & WebsiteString

                                        If achievement.ID = "1176" Then
                                            ExtraInfo = "100_gold"
                                        ElseIf achievement.ID = "1177" Then
                                            ExtraInfo = "1000_gold"
                                        ElseIf achievement.ID = "1178" Then
                                            ExtraInfo = "5000_gold"
                                        ElseIf achievement.ID = "1180" Then
                                            ExtraInfo = "10000_gold"
                                        ElseIf achievement.ID = "1181" Then
                                            ExtraInfo = "25000_gold"
                                        ElseIf achievement.ID = "5455" Then
                                            ExtraInfo = "50000_gold"
                                        ElseIf achievement.ID = "5456" Then
                                            ExtraInfo = "100000_gold"
                                        ElseIf achievement.ID = "6753" Then
                                            ExtraInfo = "200000_gold"
                                        End If

                                        If ExtraInfo <> Nothing Then
                                            sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")")
                                            Webpage = "https://wow.gamepedia.com/" & WebsiteString & " (" & ExtraInfo & ")"
                                        Else
                                            sourceString = New System.Net.WebClient().DownloadString("https://wow.gamepedia.com/" & WebsiteString)
                                            Webpage = "https://wow.gamepedia.com/" & WebsiteString
                                        End If
                                    End Try
                                End Try
                            End Try

                            If Webpage.Length > 1 Then
                                WebWowpedia.Source = New Uri(Webpage & "?action=edit")

                                ResponsiveSleep(4000)

                                Dim script = "document.getElementById('wpTextbox1').value"
                                Dim scriptExecute = WebWowpedia.ExecuteScriptAsync(script)
                                ResponsiveSleep(1000)
                                Dim result = scriptExecute.Result

                                If result.Contains("|category") And result.ToLower.Contains("achievementtip") Then
                                    Dim strArr2() As String = result.Split("\n")

                                    Dim CategoryObject As Object = GetCategory(strArr(0))
                                    Dim CategoryName = CategoryObject(0)

                                    For i = 0 To strArr2.Count - 1
                                        strArr2(i) = strArr2(i).Replace("n|", "|")
                                    Next

                                    Dim Match = False
                                    Dim MatchString = ""
                                    For Each line In strArr2
                                        If line.Contains("|category") Then
                                            MatchString = line
                                        End If

                                        If line = "|category=" & CategoryName Then
                                            Match = True
                                        End If
                                    Next

                                    If Match = True Then
                                        TextBox1.AppendText(achievement.Title_lang & " Category is correct" & vbNewLine)
                                    Else
                                        If MatchString.Contains("}}") Then
                                            result = result.Replace(MatchString, "|category=" & CategoryName & vbNewLine & "}}")
                                        Else
                                            result = result.Replace(MatchString, "|category=" & CategoryName)
                                        End If


                                        result = result.Replace("\n", vbNewLine).Replace("\u003C", "<").Trim("""")

                                        Dim script2 = "document.getElementById('wpTextbox1').value = " & "`" & result & "`"
                                        WebWowpedia.ExecuteScriptAsync(script2)
                                        script2 = "document.querySelectorAll('[name=""wpSummary""]')[0].value = " & "`Changed Category from " & MatchString.Replace("|category=", "") & " to " & CategoryName & "`"
                                        WebWowpedia.ExecuteScriptAsync(script2)


                                        ResponsiveSleep(2000)
                                        Dim wait = 123
                                        script2 = "document.querySelectorAll('[name=""wpSave""]')[0].click();"
                                        WebWowpedia.ExecuteScriptAsync(script2)

                                        TextBox1.AppendText(achievement.Title_lang & " Changed Category from" & line & " to " & "|category=" & CategoryName & vbNewLine)

                                        ResponsiveSleep(5000)
                                    End If
                                End If
                            Else
                                Using writer As StreamWriter = New StreamWriter("CategoryErrors.txt", True)
                                    writer.WriteLine(achievement.Title_lang)
                                End Using
                            End If
                        End If
                    Next
                    Application.DoEvents()
                End If

                line = reader.ReadLine
            End While
        End Using
    End Sub

    Function GetPagesInCategory(categoryName)
        Dim jsonString As String = New System.Net.WebClient().DownloadString("https://wowpedia.fandom.com//api.php?action=query&format=json&list=categorymembers&cmtitle=Category:" & categoryName & "&cmlimit=500")

        Dim JsonPost As CategoryList.Rootobject = JsonConvert.DeserializeObject(Of CategoryList.Rootobject)(jsonString)

        Dim AchievementsList As New List(Of String)

        For Each item As CategoryList.Categorymember In JsonPost.query.categorymembers
            If Not item.title Is Nothing Then
                AchievementsList.Add(item.title)
            End If
        Next

        Return AchievementsList
    End Function
End Class
