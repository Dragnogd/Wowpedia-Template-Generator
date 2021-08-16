Imports System.IO
Imports System.Text.RegularExpressions
Imports HtmlAgilityPack
Imports LsonLib
Imports Newtonsoft.Json
Imports NLua
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome

Public Class Professions
    Dim Links As New List(Of String)
    Dim CurrentPage
    Dim PageLoaded = False
    Dim PageCheck = False
    Dim ItemQueue As New List(Of String)
    Dim driver As IWebDriver = New ChromeDriver

    Dim ItemList As New Dictionary(Of Integer, String)

    Private Sub Professions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WebView21.Source = New Uri("https://www.wowhead.com/blacksmithing")
        Me.WindowState = WindowState.Maximized

        driver.Manage().Timeouts().PageLoad() = New TimeSpan(0, 0, 10)
    End Sub

    Public Function GetOuterHTML() As Task(Of String)
        Dim htmlReturn = WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
        Return htmlReturn
    End Function

    Private Sub btnFetchLinks_Click(sender As Object, e As EventArgs) Handles btnFetchLinks.Click
        TextBox1.Text = ""
        Dim doc As HtmlDocument = New HtmlDocument()
        Dim InititalPage As String = InputBox("Enter Wowhead Page to scan", "Enter Page", "https://www.wowhead.com/blacksmithing#shadowlands-plans")
        driver.Navigate().GoToUrl(InititalPage)
        doc.LoadHtml(driver.PageSource)
        Dim AllPagesScanned = False
        Dim itemRows As New List(Of ItemRow)

        Dim TempStop = 0

        While AllPagesScanned = False
            Dim Nodes = doc.DocumentNode.SelectNodes("//table[@class='listview-mode-default']//tr[@class='listview-row']")
            For Each node As HtmlNode In Nodes
                Dim newRow As ItemRow = New ItemRow
                TempStop += 1

                If TempStop > 5 Then
                    'Exit While
                End If

                'Get Item Link
                Dim Node2 = node.SelectSingleNode(".//td[1]//a")
                'Dim itemName = Node2.GetAttributeValue("href", "").Split("/")(2)
                Dim itemLink = Node2.GetAttributeValue("href", "").Split("/")(1).Replace("item=", "")
                newRow.ID = itemLink

                'Get Item Name
                'Dim itemName = WoWpediaGenerator.GetInnerTextCustom(doc, "//a[contains(@href,'spell=')]")

                'Get Spell Link
                Dim Node3 = node.SelectSingleNode(".//td[2]//a")
                Dim spellLink = Node3.GetAttributeValue("href", "").Split("/")(1).Replace("spell=", "")

                'WebView21.Source = New Uri("https://www.wowhead.com/spell=" & spellLink)
                'Dim pageContents2 = GetOuterHTML()
                'ResponsiveSleep(10000)
                Dim sourceString As String = New System.Net.WebClient().DownloadString("https://www.wowhead.com/spell=" & spellLink)

                ResponsiveSleep(500)
                'Dim htmldecoded2 = JsonConvert.DeserializeObject(sour)

                Dim doc2 As HtmlDocument = New HtmlDocument()
                driver.Navigate().GoToUrl("https://www.wowhead.com/spell=" & spellLink & "#taught-by-npc")
                doc2.LoadHtml(driver.PageSource)

                Dim SpellNode1 = doc2.DocumentNode.SelectSingleNode("//h1")
                Dim itemNameFormatted = SpellNode1.GetDirectInnerText()
                newRow.Name = itemNameFormatted
                TextBox1.AppendText("Processing " & newRow.Name & "..." & vbNewLine)


                'Get Skill Points
                Dim OrangeNode = doc2.DocumentNode.SelectSingleNode("//span[@class='r1']")
                If Not OrangeNode Is Nothing Then
                    newRow.OrangeSkill = OrangeNode.GetDirectInnerText()
                Else
                    newRow.OrangeSkill = """N/A"""
                End If

                Dim YellowNode = doc2.DocumentNode.SelectSingleNode("//span[@class='r2']")
                If Not YellowNode Is Nothing Then
                    newRow.YellowSkill = YellowNode.GetDirectInnerText()
                Else
                    newRow.YellowSkill = """N/A"""
                End If

                Dim GreenNode = doc2.DocumentNode.SelectSingleNode("//span[@class='r3']")
                If Not GreenNode Is Nothing Then
                    newRow.GreenSkill = GreenNode.GetDirectInnerText()
                Else
                    newRow.GreenSkill = """N/A"""
                End If

                Dim GreyNode = doc2.DocumentNode.SelectSingleNode("//span[@class='r4']")
                If Not GreyNode Is Nothing Then
                    newRow.GraySkill = GreyNode.GetDirectInnerText()
                Else
                    newRow.GraySkill = """N/A"""
                End If

                'Get Skill Points
                Dim regexString2 = New Regex("(?<=Rewards)(.*)(?=skill points)")
                Dim matchedLink = regexString2.Match(sourceString)
                Dim RewardsSkill = GetIntegerValues(matchedLink.Value.Trim())
                If RewardsSkill.length Then
                    newRow.SkillPoints = RewardsSkill
                Else
                    newRow.SkillPoints = 1
                End If

                'Taught By
                Dim TaughtByString = ""
                Dim TaughtByNPC = WowheadQueries.GetInnerTextCustom(doc2, "//div[@id='tab-taught-by-npc']//a[contains(@href,'npc=')]")
                If TaughtByNPC.length > 1 Then
                    TaughtByString &= "[[" & TaughtByNPC & "]]<br>"
                End If

                driver.Navigate().GoToUrl("https://www.wowhead.com/spell=" & spellLink & "#taught-by-item")
                doc2.LoadHtml(driver.PageSource)
                Dim TaughtByItem = WowheadQueries.GetInnerTextCustom(doc2, "//div[@id='tab-taught-by-item']//a[contains(@href,'item=') and contains(@class,'cleartext')]")
                If TaughtByItem.length > 1 Then
                    TaughtByString &= "[[" & TaughtByItem & "]]"
                End If

                newRow.Source = TaughtByString

                'Get Category
                Dim Category = WowheadQueries.GetInnerTextByClass(doc2, "div", "q0")
                If Category.length > 1 Then
                    newRow.Category = Category
                End If

                'Get Mats
                Dim Node4 = node.SelectNodes(".//td[3]/div")

                For Each mats In Node4
                    Dim newMats = New ItemMaterial
                    Try
                        Dim Node5 = mats.SelectSingleNode(".//a")
                        Dim MatName = Node5.GetAttributeValue("href", "").Split("/")(2).Replace("-", " ")

                        MatName = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(MatName).Replace("Of", "of").Replace("Shaldorei", "Shal'dorei")

                        Dim Node6 = mats.SelectSingleNode(".//span//div[1]")
                        If Not Node6 Is Nothing Then
                            Dim MatQuantity = Node6.GetDirectInnerText()
                            newMats.Material = MatName
                            newMats.Quantity = MatQuantity
                            newRow.Materials.Add(newMats)
                        Else
                            newMats.Material = MatName
                            newMats.Quantity = 1
                            newRow.Materials.Add(newMats)
                        End If
                    Catch ex As Exception

                    End Try
                Next
                itemRows.Add(newRow)
            Next

            Dim NodeNext = doc.DocumentNode.SelectNodes("//div[@class='listview-nav']//a")
            Dim NodeCurrent = doc.DocumentNode.SelectSingleNode("//div[@class='listview-nav']//span")
            If Not NodeCurrent Is Nothing Then
                Dim NewNodeCurrent As Integer = GetIntegerValues(NodeCurrent.InnerText.Split("of")(0).Split("-")(1))
                AllPagesScanned = True
                For Each node In NodeNext
                    If node.InnerText.Contains("Next") Then
                        Dim Active = node.Attributes("data-active").Value
                        If Active = "yes" Then
                            AllPagesScanned = False
                            driver.Navigate().GoToUrl(InititalPage & ";" & NewNodeCurrent)
                            driver.Navigate().Refresh()
                            doc.LoadHtml(driver.PageSource)
                            Exit For
                        End If
                    End If
                Next
            Else
                Exit While
            End If
        End While

        'Sort and display the data gathered
        itemRows.Sort()

        TextBox1.Text = ""

        TextBox1.AppendText("return {" & vbNewLine)
        TextBox1.AppendText(vbTab & "lastUpdate = ""This list is up to date as of [[Patch " & Form1.CurrentPatch & "]]""," & vbNewLine)
        TextBox1.AppendText(vbTab & "[""Shadowlands blacksmithing plans""] = {" & vbNewLine)

        Dim LastItemInserted = ""
        Dim Counter = 1
        For Each item As ItemRow In itemRows
            LastItemInserted = item.Name

            Dim MaterialString = "{"
            For Each material As ItemMaterial In item.Materials
                MaterialString &= "{""" & material.Material & """," & material.Quantity & "},"
                'TextBox1.AppendText(vbTab & vbTab & vbTab & vbTab & "[""" & material.Material & """] = " & material.Quantity & "," & vbNewLine)
            Next
            MaterialString = MaterialString.Trim(",")
            MaterialString &= "}"

            'Name (Index)
            TextBox1.AppendText(vbTab & vbTab & "[" & Counter & "] = {""" & item.Name & """,""" & item.Category & """," & MaterialString & "," & item.SkillPoints & "," & item.OrangeSkill & "," & item.YellowSkill & "," & item.GreenSkill & "," & item.GraySkill & ",""" & item.Source & """}," & vbNewLine)

            ''Name
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "Name = """ & item.Name & """," & vbNewLine)
            ''Category
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "Category = """ & item.Category & """," & vbNewLine)
            ''Materials
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "Materials = {" & vbNewLine)

            'TextBox1.AppendText(vbTab & vbTab & vbTab & "}," & vbNewLine)
            ''Skill Points
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "SkillPoints = " & item.SkillPoints & "," & vbNewLine)
            ''Orange Skill
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "OrangeSkill = """ & item.OrangeSkill & """," & vbNewLine)
            ''Yellow Skill
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "YellowSkill = """ & item.YellowSkill & """," & vbNewLine)
            ''Green Skill
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "GreenSkill = """ & item.GreenSkill & """," & vbNewLine)
            ''Grey Skill
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "GreySkill = """ & item.GraySkill & """," & vbNewLine)
            ''Source
            'TextBox1.AppendText(vbTab & vbTab & vbTab & "Source = """ & item.Source & """," & vbNewLine)
            ''Close Table
            'TextBox1.AppendText(vbTab & vbTab & "}," & vbNewLine)
            Counter += 1
        Next
        TextBox1.AppendText(vbTab & "}" & vbNewLine)
        TextBox1.AppendText("}")
    End Sub
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadLinks()
    End Sub

    Public Async Sub LoadLinks()
        For Each link In Links
            WebView21.Source = New Uri("https://www.wowhead.com" & link)
            Dim doc As HtmlDocument = New HtmlDocument()
            Dim html As String = Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
            Dim htmldecoded = JsonConvert.DeserializeObject(html)
            Try
                doc.LoadHtml(htmldecoded)
                ResponsiveSleep(5000)
                Application.DoEvents()
                Dim Name = Await GetInnerHTML(doc, "h1", "")
                TextBox1.Text &= Name.Trim("""") & vbNewLine
            Catch ex As Exception
                TextBox1.Text &= "Could not load " & link
            End Try
        Next
    End Sub

    Public Async Function GetInnerHTML(doc, element, className) As Task(Of String)
        Dim Node As HtmlNode
        If className.length > 1 Then
            Node = doc.DocumentNode.SelectSingleNode("//" & element & "[@class='" & className & "']")
        Else
            Node = doc.DocumentNode.SelectSingleNode("//" & element)
        End If
        If Not Node Is Nothing Then
            Return Node.InnerText
        Else
            Return ""
        End If
    End Function

    Public Sub ResponsiveSleep(ByRef iMilliSeconds As Integer)
        Dim i As Integer, iHalfSeconds As Integer = iMilliSeconds / 500
        For i = 1 To iHalfSeconds
            Threading.Thread.Sleep(500) : Application.DoEvents()
        Next i
    End Sub

    Private Sub btnGeneratePage_Click(sender As Object, e As EventArgs) Handles btnGeneratePage.Click
        'Check Item Queue
        If ItemQueue.Count > 0 Then
            txtItemID.Text = ItemQueue(0)
            ItemQueue.RemoveAt(0)
            txtItemQueueLength.Text = ItemQueue.Count
        End If

        'Check if Page Name field is not blank
        If txtItemName.Text.Length > 1 Then
            WebView21.Source = New Uri("https://www.wowhead.com/?search=" & txtItemName.Text)
            ResponsiveSleep(4000)
            Dim URLName = WebView21.Source.ToString
            Dim URLComponents = URLName.Split("/")
            For Each component In URLComponents
                If component.Contains("item=") Then
                    txtItemID.Text = component.Replace("item=", "")
                End If
            Next
        End If
        txtItemName.Text = ""

        If cboNewPagesOnly.Checked Then
            GenerateItem(txtItemID.Text, True)
        Else
            GenerateItem(txtItemID.Text)
        End If
    End Sub

    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        Dim script = "document.getElementById('wpTextbox1').value = " & "`" & TextBox1.Text & "`"
        WebView21.ExecuteScriptAsync(script)
        script = "document.getElementById('wpSummary').value = " & "`Auto generated for Patch " & Form1.CurrentPatch & " " & DateTime.Now & "`"
        WebView21.ExecuteScriptAsync(script)
        script = "document.querySelectorAll('[name=""wpSave""]')[0].click();"
        WebView21.ExecuteScriptAsync(script)
    End Sub

    Private Sub btnItemPage_Click(sender As Object, e As EventArgs) Handles btnItemPage.Click
        Dim Address = New Uri("https://wow.gamepedia.com/" & CurrentPage & " (item)?action=edit")
        WebView21.Source = Address
    End Sub

    Private Sub WebView21_NavigationCompleted(sender As Object, e As Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        If PageCheck = True Then
            PageLoaded = True
        End If
    End Sub

    Private Async Sub btnOutputLinks_Click(sender As Object, e As EventArgs) Handles btnOutputLinks.Click
        Using reader As StreamReader = New StreamReader("ProfessionDB.csv")
            Dim line = reader.ReadLine

            While Not line Is Nothing
                Dim strArr() = line.Split(",")

                If strArr(2) = txtProfession.Text And strArr(3) = txtExpansion.Text Then
                    '1.) Navigate to wowhead page
                    WebView21.Source = New Uri("https://www.wowhead.com/item=" & strArr(1))
                    ResponsiveSleep(4000)
                    Dim doc As HtmlDocument = New HtmlDocument()
                    Dim html As String = Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
                    Dim htmldecoded = JsonConvert.DeserializeObject(html)

                    'Get Name of Item
                    Dim Name = GetInnerTextByClass(doc, "h1", "heading-size-1")

                    'Get Skill
                    'Dim Orange =
                    'Dim Yellow = GetInnerTextByClass(doc, "span", "r2")
                    'Dim Green = GetInnerTextByClass(doc, "span", "r2")
                    'Dim Grey = GetInnerTextByClass(doc, "span", "r2")

                    WebView21.Source = New Uri("https://www.wowhead.com/item=" & strArr(1))

                End If

                line = reader.ReadLine
            End While
        End Using
    End Sub

    Private Async Sub btnQueueItems_Click(sender As Object, e As EventArgs) Handles btnQueueItems.Click
        Dim doc As HtmlDocument = New HtmlDocument()
        Dim InitialPage = InputBox("Enter Page")
        driver.Navigate().GoToUrl(InitialPage)
        doc.LoadHtml(driver.PageSource)
        Dim AllpagesScanned = False

        While AllPagesScanned = False
            Dim Nodes = doc.DocumentNode.SelectNodes("//tr[@class='listview-row']//td//a")
            For Each node As HtmlNode In Nodes
                Dim value = node.GetAttributeValue("href", "")
                Dim Found = False
                Try
                    Dim strArr() = value.Split("/")
                    Dim ItemID = strArr(1).Replace("item=", "")

                    If value.Contains("/item=") And Not ItemQueue.Contains(ItemID) Then
                        If ItemID.Length > 0 Then
                            ItemQueue.Add(ItemID)
                        End If
                    End If
                Catch ex As Exception

                End Try
            Next

            Dim NodeNext = doc.DocumentNode.SelectNodes("//div[@class='listview-nav']//a")
            Dim NodeCurrent = doc.DocumentNode.SelectSingleNode("//div[@class='listview-nav']//span")
            If Not NodeCurrent Is Nothing Then
                Dim NewNodeCurrent As Integer = GetIntegerValues(NodeCurrent.InnerText.Split("of")(0).Split("-")(1))
                AllpagesScanned = True
                For Each node In NodeNext
                    If node.InnerText.Contains("Next") Then
                        Dim Active = node.Attributes("data-active").Value
                        If Active = "yes" Then
                            AllpagesScanned = False
                            driver.Navigate().GoToUrl(InitialPage & ";" & NewNodeCurrent)
                            driver.Navigate().Refresh()
                            doc.LoadHtml(driver.PageSource)
                            Exit For
                        End If
                    End If
                Next
            Else
                Exit While
            End If
        End While
        txtItemQueueLength.Text = ItemQueue.Count
    End Sub

    Private Sub btnClearQueue_Click(sender As Object, e As EventArgs) Handles btnClearQueue.Click
        ItemQueue.Clear()
        txtItemQueueLength.Text = 0
    End Sub

    Private Function cboItems_SelectedIndexChangedAsync(sender As Object, e As EventArgs) As Task Handles cboItems.SelectedIndexChanged
        Dim ItemID = cboItems.SelectedItem.split("|")(1)
        GenerateItem(ItemID)
    End Function

    Public Sub GenerateItem(ItemID, Optional NewPagesOnly = False)
        Try
            driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#created-by-spell;0-10+20")
        Catch ex As Exception
            GenerateItem(ItemID, True)
            Exit Sub
        End Try

        WowheadQueries.SetupDoc(driver.PageSource)

        If NewPagesOnly = True Then
            Dim wowheadPage As HtmlDocument = New HtmlDocument
            Try
                driver.Navigate().GoToUrl("https://wowpedia.fandom.com/wiki/" & WowheadQueries.GetItemName(True).ToString & "?action=edit")
            Catch ex As Exception
                GenerateItem(ItemID, True)
                Exit Sub
            End Try

            wowheadPage.LoadHtml(driver.PageSource)
            Dim PageContents = wowheadPage.DocumentNode.SelectSingleNode("//div//textarea[@id='wpTextbox1']")
            If Not PageContents Is Nothing Then
                Dim content = PageContents.InnerText
                If content.Length > 1 Then
                    'Page is populated. Move onto next page
                    If ItemQueue.Count > 0 Then
                        txtItemID.Text = ItemQueue(0)
                        ItemQueue.RemoveAt(0)
                        txtItemQueueLength.Text = ItemQueue.Count
                        GenerateItem(txtItemID.Text, True)
                    End If
                    Exit Sub
                End If
            End If
        End If

        TextBox1.Text = "{{Stub/Item}}" & vbNewLine & "{{#data:Itemtip" & vbNewLine

        Dim classID = Nothing
        Dim subClassID = Nothing
        Dim itemSubType = Nothing
        Dim itemType = Nothing
        Dim useFound = False
        Dim equipFound = False

        Using test As New Lua
            Dim Res = test.GetTableDict(test.DoFile("ItemDump.lua")(0))

            For Each Item In Res
                If Item.Key = ItemID Then
                    For Each item2 In Item.Value.Keys
                        Dim ItemValue = Res(Item.Key)(item2)

                        Select Case item2
                            Case "name"
                                '|name=	Item name as it appears in-game.
                                TextBox1.AppendText("|name=" & ItemValue & vbNewLine)

                            Case "classID"
                                classID = ItemValue
                            Case "subclassID"
                                subClassID = ItemValue
                            Case "itemType"
                                itemType = ItemValue
                            Case "itemSubType"
                                itemSubType = ItemValue
                            Case "sellPrice"
                                '|sellprice=	Amount of copper vendors are willing to give for this item.
                                If ItemValue > 0 Then
                                    TextBox1.AppendText("|sellprice=" & ItemValue & vbNewLine)
                                End If
                            Case "itemLevel"
                                '|ilvl=	Item level. Only displayed for items you can equip. Uses {{DE}} to show disenchant info. Force the display with |geartoken=
                                TextBox1.AppendText("|ilvl=" & ItemValue & vbNewLine)
                            Case "itemQuality"
                                '|quality=	Item quality; one of: poor, common, uncommon, rare, epic, legendary, heirloom, token.
                                Select Case ItemValue
                                    Case "0"
                                        TextBox1.AppendText("|quality=poor" & vbNewLine)
                                    Case "1"
                                        TextBox1.AppendText("|quality=common" & vbNewLine)
                                    Case "2"
                                        TextBox1.AppendText("|quality=uncommon" & vbNewLine)
                                    Case "3"
                                        TextBox1.AppendText("|quality=rare" & vbNewLine)
                                    Case "4"
                                        TextBox1.AppendText("|quality=epic" & vbNewLine)
                                    Case "5"
                                        TextBox1.AppendText("|quality=legendary" & vbNewLine)
                                    Case "6"
                                        TextBox1.AppendText("|quality=artifact" & vbNewLine)
                                    Case "7"
                                        TextBox1.AppendText("|quality=heirloom" & vbNewLine)
                                    Case "8"
                                        TextBox1.AppendText("|quality=token" & vbNewLine)
                                End Select
                            Case "itemType"
                                '|type=	Item type (if not mount, glyph) e.g. "Staff", "Sword", "Leather"
                            Case "bindType"
                                '|bind=	Item binding type, one of: bop, boe, bou, bta, quest.
                                Select Case ItemValue
                                    Case "1"
                                        TextBox1.AppendText("|bind=bop" & vbNewLine)
                                    Case "2"
                                        TextBox1.AppendText("|bind=boe" & vbNewLine)
                                    Case "3"
                                        TextBox1.AppendText("|bind=bou" & vbNewLine)
                                    Case "4"
                                        TextBox1.AppendText("|bind=quest" & vbNewLine)
                                End Select
                            Case "itemEquipLoc"
                                '|slot=	Inventory slot the item can be equipped in, e.g. "Trinket"
                                Select Case ItemValue
                                    Case "INVTYPE_HEAD"
                                        TextBox1.AppendText("|slot=Head" & vbNewLine)
                                    Case "INVTYPE_NECK"
                                        TextBox1.AppendText("|slot=Neck" & vbNewLine)
                                    Case "	INVTYPE_SHOULDER"
                                        TextBox1.AppendText("|slot=Shoulder" & vbNewLine)
                                    Case "INVTYPE_BODY"
                                        TextBox1.AppendText("|slot=Shirt" & vbNewLine)
                                    Case "INVTYPE_CHEST"
                                        TextBox1.AppendText("|slot=Chest" & vbNewLine)
                                    Case "INVTYPE_WAIST"
                                        TextBox1.AppendText("|slot=Waist" & vbNewLine)
                                    Case "INVTYPE_LEGS"
                                        TextBox1.AppendText("|slot=Legs" & vbNewLine)
                                    Case "INVTYPE_FEET"
                                        TextBox1.AppendText("|slot=Feet" & vbNewLine)
                                    Case "INVTYPE_WRIST"
                                        TextBox1.AppendText("|slot=Wrist" & vbNewLine)
                                    Case "INVTYPE_HAND"
                                        TextBox1.AppendText("|slot=Hands" & vbNewLine)
                                    Case "INVTYPE_FINGER"
                                        TextBox1.AppendText("|slot=Finger" & vbNewLine)
                                    Case "INVTYPE_TRINKET"
                                        TextBox1.AppendText("|slot=Trinket" & vbNewLine)
                                    Case "INVTYPE_WEAPON"
                                        TextBox1.AppendText("|slot=One-Hand" & vbNewLine)
                                    Case "INVTYPE_SHIELD"
                                        TextBox1.AppendText("|slot=Off Hand" & vbNewLine)
                                    Case "INVTYPE_RANGED"
                                        TextBox1.AppendText("|slot=Ranged" & vbNewLine)
                                    Case "INVTYPE_CLOAK"
                                        TextBox1.AppendText("|slot=Back" & vbNewLine)
                                    Case "INVTYPE_2HWEAPON"
                                        TextBox1.AppendText("|slot=Two-Hand" & vbNewLine)
                                    Case "INVTYPE_BAG"
                                        TextBox1.AppendText("|slot=Bag" & vbNewLine)
                                    Case "INVTYPE_TABARD"
                                        TextBox1.AppendText("|slot=Tabard" & vbNewLine)
                                    Case "INVTYPE_ROBE"
                                        TextBox1.AppendText("|slot=Chest" & vbNewLine)
                                    Case "INVTYPE_WEAPONMAINHAND"
                                        TextBox1.AppendText("|slot=Main Hand" & vbNewLine)
                                    Case "INVTYPE_WEAPONOFFHAND"
                                        TextBox1.AppendText("|slot=Off Hand" & vbNewLine)
                                    Case "INVTYPE_HOLDABLE"
                                        TextBox1.AppendText("|slot=Held In Off-hand" & vbNewLine)
                                    Case "INVTYPE_AMMO"
                                        TextBox1.AppendText("|slot=Ammo" & vbNewLine)
                                    Case "INVTYPE_THROWN"
                                        TextBox1.AppendText("|slot=Thrown" & vbNewLine)
                                    Case "INVTYPE_RANGEDRIGHT"
                                        TextBox1.AppendText("|slot=Ranged" & vbNewLine)
                                    Case "INVTYPE_QUIVER"
                                        TextBox1.AppendText("|slot=Quiver" & vbNewLine)
                                    Case "	INVTYPE_RELIC"
                                        TextBox1.AppendText("|slot=Relic" & vbNewLine)
                                End Select
                            Case "ITEM_MOD_STAMINA_SHORT"
                                '|stamina=	Stamina bonus.
                                'TextBox1.AppendText("|stamina=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_INTELLECT_SHORT"
                                '|intellect=	Intellect bonus.
                                'TextBox1.AppendText("|intellect=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_CRIT_RATING_SHORT"
                                '|crit=	Crit rating bonus.
                                'TextBox1.AppendText("|crit=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_MASTERY_RATING_SHORT"
                                '|mastery=	Mastery rating bonus.
                                'TextBox1.AppendText("|mastery=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_VERSATILITY"
                                '|versatility=	Versatility rating bonus
                                'TextBox1.AppendText("|versatility=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_HASTE_RATING_SHORT"
                                '|haste=	Haste rating bonus.
                                'TextBox1.AppendText("|haste=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_AGILITY_SHORT"
                                '|agility=	Agility bonus.
                                'TextBox1.AppendText("|agility=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_STRENGTH_SHORT"
                                '|strength=	Strength bonus.
                                TextBox1.AppendText("|strength=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_CR_LIFESTEAL_SHORT"
                                '|leech=	Leech rating bonus.
                                TextBox1.AppendText("|leech=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_CR_AVOIDANCE_SHORT"
                                '|avoidance=	Avoidance rating bonus.
                                TextBox1.AppendText("|avoidance=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_SHADOW_RESISTANCE_SHORT"
                                '|shadow=	Shadow resistance bonus.
                                'TextBox1.AppendText("|shadow=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_ARCANE_RESISTANCE_SHORT"
                                '|arcane=	Arcane resistance bonus.
                                'TextBox1.AppendText("|arcane=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_FROST_RESISTANCE_SHORT"
                                '|frost=	Frost resistance bonus.
                                'TextBox1.AppendText("|frost=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_FIRE_RESISTANCE_SHORT"
                                '|fire=	Fire resistance bonus.
                                'TextBox1.AppendText("|fire=" & ItemValue & vbNewLine)
                            Case "ITEM_MOD_NATURE_RESISTANCE_SHORT"
                                '|nature=	Nature resistance bonus.
                                'TextBox1.AppendText("|nature=" & ItemValue & vbNewLine)
                            Case "itemMinLevel"
                                '|level=	Level required to use this item.
                                If ItemValue > 1 Then
                                    TextBox1.AppendText("|level=" & ItemValue & vbNewLine)
                                End If
                            Case "itemStackCount"
                                '|stack=	Number; maximum stack size of items that can be stacked.
                                If ItemValue > 1 Then
                                    TextBox1.AppendText("|stack=" & ItemValue & vbNewLine)
                                End If
                            Case "EMPTY_SOCKET_PRISMATIC"
                                '|prismatic-sockets=	Number of prismatic sockets this item has.
                                TextBox1.AppendText("|prismatic-sockets=" & ItemValue & vbNewLine)
                            Case "EMPTY_SOCKET_META"
                                '|meta-sockets=	Number of meta sockets this item has.
                                TextBox1.AppendText("|meta-sockets=" & ItemValue & vbNewLine)
                            Case "EMPTY_SOCKET_DOMINATION"
                                '|domination-sockets=	Number of meta sockets this item has.
                                TextBox1.AppendText("|domination-sockets=" & ItemValue & vbNewLine)
                        End Select

                        'Tooltip Dump
                        If item2.Contains("Tooltip_") Then
                            '|unique-eq=	Maximum number of this item the player may have equipped.
                            If ItemValue = "Unique-Equipped" Then
                                TextBox1.AppendText("|unique-eq=1" & vbNewLine)
                            ElseIf ItemValue.Contains("Unique-Equipped:") Then
                                Dim strArr() As String = ItemValue.split(":")

                                If ItemValue.Contains("(") And ItemValue.Contains(")") Then
                                    Dim strArr2() As String = strArr(1).Split("(")
                                    TextBox1.AppendText("|unique-type=" & strArr2(0) & vbNewLine)
                                    TextBox1.AppendText("|unique-eq=" & strArr2(1).Replace(")", "") & vbNewLine)
                                Else
                                    TextBox1.AppendText("|unique-type=" & strArr(1) & vbNewLine)
                                End If
                            End If

                            '|unique=	Maximum number of this item the player may have in his bags.
                            If ItemValue = "Unique" Then
                                TextBox1.AppendText("|unique=1" & vbNewLine)
                            ElseIf ItemValue.Contains("Unique (") Then
                                Dim strArr() As String = ItemValue.Split("(")
                                TextBox1.AppendText("|unique=" & strArr(1).Replace(")", "") & vbNewLine)
                            End If

                            '|equip=	Misc "On Equip" effects.
                            If ItemValue.Contains("Equip:") Then
                                TextBox1.AppendText("|equip=" & ItemValue.replace("Equip:", "").trim() & vbNewLine)
                                equipFound = True
                            End If

                            '|use=	Use effect.
                            If ItemValue.Contains("Use:") Then
                                TextBox1.AppendText("|use=" & ItemValue.replace("Use:", "").trim() & vbNewLine)
                                useFound = True
                            End If

                            '|sockbonus=	Socket bonus description.
                            If ItemValue.Contains("Socket Bonus:") Then
                                TextBox1.AppendText("|sockbonus=" & ItemValue.replace("Socket Bonus:", "").trim() & vbNewLine)
                            End If

                            '|armor=	Amount of armor on this item.
                            If ItemValue.Contains("Armor") Then
                                TextBox1.AppendText("|armor=" & ItemValue.replace(" Armor", "").trim() & vbNewLine)
                            End If

                            '|reagent=	Number; provide "1" if the item is tagged as a "Crafting Reagent", "2" if the item is tagged as an "Optional Crafting Reagent".
                            If ItemValue = "Crafting Reagent" Then
                                TextBox1.AppendText("|reagant=1" & ItemValue & vbNewLine)
                            ElseIf ItemValue = "Optional Crafting Reagent" Then
                                TextBox1.AppendText("|reagant=2" & ItemValue & vbNewLine)
                            End If

                            '|flavor=	Flavor text.
                            If ItemValue.Contains("""") Then
                                TextBox1.AppendText("|flavor=" & ItemValue.replace("""", "") & vbNewLine)
                            End If

                            '|classes=	List of classes this item is limited to, e.g. "Mage, Warlock, Priest".
                            If ItemValue.Contains("Classes:") Then
                                TextBox1.AppendText("|classes=" & ItemValue.replace("Classes:", "").trim() & vbNewLine)
                            End If

                            '|races=	List of races this item is limited to, e.g. "Blood Elf"
                            If ItemValue.Contains("Races:") Then
                                TextBox1.AppendText("|races=" & ItemValue.replace("Races:", "").trim() & vbNewLine)
                            End If

                            '|indestructible=	flag; Indestructible.
                            If ItemValue = "Indestructible" Then
                                TextBox1.AppendText("|indestructible" & vbNewLine)
                            End If

                            '|read=	Flag; provide if the item can be read.
                            If ItemValue = "<This item can be read>" Then
                                TextBox1.AppendText("|read" & vbNewLine)
                            End If

                            '|toy=	Flag; provide if the item is tagged as a "Toy".
                            If ItemValue = "Toy" Then
                                TextBox1.AppendText("|toy" & vbNewLine)
                            End If

                            '|mount=	Riding skill required to learn this mount.
                            If ItemValue = "Requires Apprentice Riding" Then
                                TextBox1.AppendText("|mount=75" & vbNewLine)
                            End If

                            '|prof=	Profession required to use this item.
                            '|profskill=	Profession skill required to use this item.
                            Dim Professions = {"Leatherworking", "Tailoring", "Engineering", "Blacksmithing", "Cooking", "Alchemy", "First Aid", "Enchanting", "Fishing", "Jewelcrafting", "Inscription"}
                            If ItemValue.Contains("Requires") Then
                                For Each prof In Professions
                                    If ItemValue.contains(prof) Then
                                        If ItemValue.contains("(") And ItemValue.contains(")") Then
                                            Dim strArr() As String = ItemValue.split("(")
                                            TextBox1.AppendText("|prof=" & strArr(0).Replace("Requires", "").Trim() & vbNewLine)
                                            TextBox1.AppendText("|profskill=" & strArr(1).Replace(")", "").Trim() & vbNewLine)
                                        Else
                                            TextBox1.AppendText("|prof=" & ItemValue.replace("Requires", "").trim() & vbNewLine)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    'ItemType (classID, subClassID)
                    If Not classID Is Nothing And Not subClassID Is Nothing Then
                        Select Case classID
                            Case "0"
                                'Consumable
                                Select Case subClassID
                                    Case "0"
                                        TextBox1.AppendText("|type=Explosives and Devices" & vbNewLine)
                                    Case "1"
                                        'TextBox1.AppendText("|type=Potion" & vbNewLine)
                                    Case "2"
                                        TextBox1.AppendText("|type=Elixir" & vbNewLine)
                                    Case "3"
                                        'TextBox1.AppendText("|type=Flask" & vbNewLine)
                                    Case "4"
                                        TextBox1.AppendText("|type=Scroll " & vbNewLine)
                                    Case "5"
                                        'TextBox1.AppendText("|type=Food & Drink" & vbNewLine)
                                    Case "6"
                                        TextBox1.AppendText("|type=Item Enhancement " & vbNewLine)
                                    Case "7"
                                        TextBox1.AppendText("|type=Bandage" & vbNewLine)
                                    Case "8"
                                        'TextBox1.AppendText("|type=Other" & vbNewLine)
                                    Case "9"
                                        TextBox1.AppendText("|type=Vantus Runes" & vbNewLine)
                                End Select
                            Case "1"
                                'Container
                                Select Case subClassID
                                    Case "0"
                                        TextBox1.AppendText("|type=Bag" & vbNewLine)
                                    Case "1"
                                        TextBox1.AppendText("|type=Soul Bag" & vbNewLine)
                                    Case "2"
                                        TextBox1.AppendText("|type=Herb Bag" & vbNewLine)
                                    Case "3"
                                        TextBox1.AppendText("|type=Enchanting Bag" & vbNewLine)
                                    Case "4"
                                        TextBox1.AppendText("|type=Engineering Bag" & vbNewLine)
                                    Case "5"
                                        TextBox1.AppendText("|type=Gem Bag" & vbNewLine)
                                    Case "6"
                                        TextBox1.AppendText("|type=Mining Bag" & vbNewLine)
                                    Case "7"
                                        TextBox1.AppendText("|type=Leatherworking Bag" & vbNewLine)
                                    Case "8"
                                        TextBox1.AppendText("|type=Inscription Bag" & vbNewLine)
                                    Case "9"
                                        TextBox1.AppendText("|type=Tackle Bag" & vbNewLine)
                                    Case "10"
                                        TextBox1.AppendText("|type=Cooking Bag" & vbNewLine)
                                End Select
                            Case "2"
                                'Weapon
                                Select Case subClassID
                                    Case "0"
                                        TextBox1.AppendText("|type=Axe" & vbNewLine)
                                    Case "1"
                                        TextBox1.AppendText("|type=Axe" & vbNewLine)
                                    Case "2"
                                        TextBox1.AppendText("|type=Bow" & vbNewLine)
                                    Case "3"
                                        TextBox1.AppendText("|type=Gun" & vbNewLine)
                                    Case "4"
                                        TextBox1.AppendText("|type=Mace" & vbNewLine)
                                    Case "5"
                                        TextBox1.AppendText("|type=Mace" & vbNewLine)
                                    Case "6"
                                        TextBox1.AppendText("|type=Polearm" & vbNewLine)
                                    Case "7"
                                        TextBox1.AppendText("|type=Sword" & vbNewLine)
                                    Case "8"
                                        TextBox1.AppendText("|type=Sword" & vbNewLine)
                                    Case "9"
                                        TextBox1.AppendText("|type=Warglaives" & vbNewLine)
                                    Case "10"
                                        TextBox1.AppendText("|type=Staff" & vbNewLine)
                                    Case "11"
                                        TextBox1.AppendText("|type=Bear Claw" & vbNewLine)
                                    Case "12"
                                        TextBox1.AppendText("|type=Cat Claw" & vbNewLine)
                                    Case "13"
                                        TextBox1.AppendText("|type=Fist Weapon" & vbNewLine)
                                    Case "14"
                                        'TextBox1.AppendText("|type=Miscellaneous" & vbNewLine)
                                    Case "15"
                                        TextBox1.AppendText("|type=Dagger" & vbNewLine)
                                    Case "16"
                                        TextBox1.AppendText("|type=Thrown" & vbNewLine)
                                    Case "17"
                                        TextBox1.AppendText("|type=Spear" & vbNewLine)
                                    Case "18"
                                        TextBox1.AppendText("|type=Crossbow" & vbNewLine)
                                    Case "19"
                                        TextBox1.AppendText("|type=Wand" & vbNewLine)
                                    Case "20"
                                        TextBox1.AppendText("|type=Fishing Pole" & vbNewLine)
                                End Select
                            Case "3"
                                'Gem
                            Case "4"
                                'Armor
                                Select Case subClassID
                                    Case "0"
                                        'TextBox1.AppendText("|type=Miscellaneous" & vbNewLine)
                                    Case "1"
                                        TextBox1.AppendText("|type=Cloth" & vbNewLine)
                                    Case "2"
                                        TextBox1.AppendText("|type=Leather" & vbNewLine)
                                    Case "3"
                                        TextBox1.AppendText("|type=Mail" & vbNewLine)
                                    Case "4"
                                        TextBox1.AppendText("|type=Plate" & vbNewLine)
                                    Case "5"
                                        TextBox1.AppendText("|type=Cosmetic" & vbNewLine)
                                    Case "6"
                                        TextBox1.AppendText("|type=Shield" & vbNewLine)
                                    Case "7"
                                        TextBox1.AppendText("|type=Libram" & vbNewLine)
                                    Case "8"
                                        TextBox1.AppendText("|type=Idol" & vbNewLine)
                                    Case "9"
                                        TextBox1.AppendText("|type=Totem" & vbNewLine)
                                    Case "10"
                                        TextBox1.AppendText("|type=Sigil" & vbNewLine)
                                    Case "11"
                                        TextBox1.AppendText("|type=relic" & vbNewLine)
                                End Select
                            Case "5"
                                'Reagent
                            Case "6"
                                'Projectile
                            Case "7"
                                'Tradeskill
                            Case "8"
                                'Item Enhancement
                            Case "9"
                                'Recipe
                            Case "10"
                                'Money (OBSOLETE)
                            Case "11"
                                'Quiver
                            Case "12"
                                'Quest
                            Case "13"
                                'Key
                            Case "14"
                                'Permanent (OBSOLETE)
                            Case "15"
                                'Miscellaneous
                            Case "16"
                                'Glpyh
                            Case "17"
                                'Battle Pets
                            Case "18"
                                'Wow Token
                        End Select
                    End If

                    'Wowhead Scrape
                    If useFound = False Then
                        TextBox1.AppendText(GetUseText())
                    End If
                    If equipFound = False Then
                        TextBox1.AppendText(GetEquipText())
                    End If

                    TextBox1.AppendText(GetIconName())
                    TextBox1.AppendText(GetStats())
                    TextBox1.AppendText(GetAdditionalStats())
                    TextBox1.AppendText(GetDurability())
                    TextBox1.AppendText(GetWeaponDamage())

                    'Other Fields
                    TextBox1.AppendText("|itemid=" & Item.Key & vbNewLine)

                    TextBox1.AppendText("}}" & vbNewLine)

                    '-------------------------------------------------------
                    '------------------------Body---------------------------
                    '-------------------------------------------------------
                    Dim TabContent As HtmlDocument = New HtmlDocument

                    If chkDropBoss.Checked = False Then
                        If classID = "1555" Then
                            TextBox1.AppendText(vbNewLine & "'''" & GetItemName(True) & "''' is a " & itemType.tolower() & " " & itemSubType.tolower() & " item" & vbNewLine)
                        Else

                            Dim bodyString = vbNewLine & "'''" & GetItemName(True) & "'''"

                            If GetItemName(True).ToString.EndsWith("s") Then
                                bodyString &= " are created with "
                            Else
                                bodyString &= " is created with "
                            End If

                            'Profession Expansion + Skill
                            driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#created-by-spell")
                            TabContent.LoadHtml(driver.PageSource)
                            Dim CreatedBySpell = GetWowheadTabContents(TabContent, "tab-created-by-spell", 4, False, "div", True)
                            If CreatedBySpell.length > 1 Then
                                If CreatedBySpell.contains("(") Then
                                    Dim ProfessionExpansion = CreatedBySpell.split("(")(0).trim()
                                    Dim strArr() As String = ProfessionExpansion.split(" ")
                                    txtExpansion.Text = ProfessionExpansion.split(" ")(strArr.Count - 2)
                                    txtProfession.Text = ProfessionExpansion.split(" ")(strArr.Count - 1)
                                    Dim SkillLevel = GetIntegerValues(CreatedBySpell.split("(")(1).trim())
                                    bodyString &= "[[" & ProfessionExpansion & "]] (" & SkillLevel & ")"
                                Else
                                    bodyString &= "[[" & txtExpansion.Text & " " & txtProfession.Text & "]]"
                                End If
                            End If

                            'Taught By (NPC)
                            driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#taught-by-npc")
                            TabContent.LoadHtml(driver.PageSource)
                            Dim TaughtByNPC = GetWowheadTabContents(TabContent, "tab-taught-by-npc", 1, False)
                            If TaughtByNPC.length > 1 Then
                                bodyString &= "; taught by " & TaughtByNPC
                            End If

                            'Taught By (Item)
                            driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#taught-by-item")
                            TabContent.LoadHtml(driver.PageSource)
                            Dim TaughtByItem = GetWowheadTabContents(TabContent, "tab-taught-by-item", 3, False)
                            If TaughtByItem.length > 1 Then
                                If TaughtByNPC.length > 1 Then
                                    bodyString &= " and " & TaughtByItem
                                Else
                                    bodyString &= "; taught by " & TaughtByItem
                                End If
                            End If
                            bodyString &= "." & vbNewLine
                            TextBox1.AppendText(bodyString)
                        End If
                    Else
                        If GetItemName(True).ToString.EndsWith("s") Then
                            TextBox1.AppendText(vbNewLine & "'''" & GetItemName(True) & "''' are dropped by [[" & GetDropBy() & "]] in [[" & GetDropLocation() & "]]." & vbNewLine)
                        Else
                            TextBox1.AppendText(vbNewLine & "'''" & GetItemName(True) & "''' is dropped by [[" & GetDropBy() & "]] in [[" & GetDropLocation() & "]]." & vbNewLine)
                        End If
                    End If

                    TextBox1.AppendText(GetItemMats())



                    'Dropped By
                    driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#dropped-by")
                    TabContent.LoadHtml(driver.PageSource)
                    Dim DroppedBy = GetWowheadTabContents(TabContent, "tab-dropped-by", 1)
                    If DroppedBy.length > 1 Then
                        TextBox1.AppendText(vbNewLine & "==Dropped by==" & vbNewLine)
                        TextBox1.AppendText(DroppedBy)
                    End If

                    'Quest Rewards
                    driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#reward-from-q")
                    TabContent.LoadHtml(driver.PageSource)
                    Dim QuestReward = GetWowheadTabContents(TabContent, "tab-reward-from-q", 1)
                    If QuestReward.length > 1 Then
                        TextBox1.AppendText(vbNewLine & "==As a quest reward==" & vbNewLine)
                        TextBox1.AppendText(QuestReward)
                    End If

                    'Mission Rewards
                    driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#missions")
                    TabContent.LoadHtml(driver.PageSource)
                    Dim Missions = GetWowheadTabContents(TabContent, "tab-missions", 1)
                    If Missions.length > 1 Then
                        TextBox1.AppendText(vbNewLine & "==As a mission reward==" & vbNewLine)
                        TextBox1.AppendText(Missions)
                    End If

                    'Reagent For
                    driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#reagent-for")
                    TabContent.LoadHtml(driver.PageSource)
                    Dim ReagentFor = GetWowheadTabContents(TabContent, "tab-reagent-for", 2)
                    If ReagentFor.length > 1 Then
                        TextBox1.AppendText(vbNewLine & "==As an ingredient==" & vbNewLine)
                        TextBox1.AppendText(ReagentFor)
                    End If

                    'Objective of
                    driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#objective-of")
                    TabContent.LoadHtml(driver.PageSource)
                    Dim ObjectiveOf = GetWowheadTabContents(TabContent, "tab-objective-of", 1)
                    If ObjectiveOf.length > 1 Then
                        TextBox1.AppendText(vbNewLine & "==As an objective==" & vbNewLine)
                        TextBox1.AppendText(ObjectiveOf)
                    End If

                    'Objective of
                    driver.Navigate().GoToUrl("https://www.wowhead.com/item=" & Integer.Parse(ItemID) & "#disenchanting")
                    TabContent.LoadHtml(driver.PageSource)
                    Dim Discenchanted = GetWowheadTabContents(TabContent, "tab-disenchanting", 3)
                    If Discenchanted.length > 1 Then
                        TextBox1.AppendText(vbNewLine & "==When disenchanted==" & vbNewLine)
                        TextBox1.AppendText(Discenchanted)
                    End If

                    TextBox1.AppendText(GetPatch())

                    TextBox1.AppendText(vbNewLine & GetExternalLinks())

                    If chkDropBoss.Checked = False Then
                        TextBox1.AppendText(vbNewLine & "[[Category:" & txtProfession.Text & " crafted items]]")
                    Else
                        If classID = "15" Then
                        Else
                            TextBox1.AppendText(vbNewLine & "[[Category:" & GetDropLocation() & " items]]")
                        End If
                    End If


                    WebView21.Source = New Uri("https://wowpedia.fandom.com/wiki/" & GetItemName(True) & "?action=edit")

                    PageLoaded = True
                End If
            Next
        End Using
    End Sub

    Public Function GetWowheadTabContents(doc, tabID, column, Optional list = True, Optional tag = "a", Optional returnFirstInnerText = False)
        Dim Nodes = doc.DocumentNode.SelectNodes("//div[@id='" & tabID & "']//tr[@class='listview-row']//td[" & column & "]//" & tag)
        If Not Nodes Is Nothing Then
            Dim ValueString = ""
            Dim lastRowValue = ""
            For Each node As HtmlNode In Nodes
                If returnFirstInnerText = False Then
                    Dim RowValue = node.GetDirectInnerText()
                    If RowValue <> lastRowValue Then
                        If list = False Then
                            ValueString &= "[[" & RowValue & "]]"
                        Else
                            ValueString &= "*[[" & RowValue & "]]" & vbNewLine
                        End If
                    End If
                    lastRowValue = RowValue
                Else
                    Return node.InnerText
                End If
            Next
            Return ValueString
        End If
        Return ""
    End Function

    Private Sub Professions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        driver.Quit()
        driver = Nothing
    End Sub
End Class