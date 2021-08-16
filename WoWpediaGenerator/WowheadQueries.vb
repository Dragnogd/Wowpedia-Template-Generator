Imports System.IO
Imports System.Net.Sockets
Imports System.Text.RegularExpressions
Imports HtmlAgilityPack
Imports Newtonsoft.Json

Module WowheadQueries
    Private WholePageHTML As HtmlDocument = New HtmlDocument()
    Dim TooltipHTML As New HtmlDocument
    Dim InfoBoxHTML As New HtmlDocument

    Public Sub SetupDoc(html)
        WholePageHTML.LoadHtml(html)
        TooltipHTML.LoadHtml(GetHTMLInsideTag(WholePageHTML, "div", "wowhead-tooltip "))
        InfoBoxHTML.LoadHtml(GetHTMLInsideTag(WholePageHTML, "table", "infobox"))
    End Sub

    Public Function GetItemName(Optional clean As Boolean = False)
        '|name=	Item name as it appears in-game.
        Dim Name = GetInnerTextAfterComment(TooltipHTML, "nstart")
        If Name.length > 0 Then
            If clean Then
                Return Name
            Else
                Return "|name=" & Name & vbNewLine
            End If
        End If
        Return ""
    End Function

    Public Function GetGearSlot()
        '|slot=	Inventory slot the item can be equipped in, e.g. "Trinket"
        'Armour Types
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Wrist") Then
            Return "|slot=Wrist" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Head") Then
            Return "|slot=Head" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Neck") Then
            Return "|slot=Neck" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Shoulder") Then
            Return "|slot=Shoulder" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Back") Then
            Return "|slot=Back" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Chest") Then
            Return "|slot=Chest" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Waist") Then
            Return "|slot=Waist" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Hands") Then
            Return "|slot=Hands" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Legs") Then
            Return "|slot=Legs" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Feet") Then
            Return "|slot=Feet" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Trinket") Then
            Return "|slot=Trinket" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Finger") Then
            Return "|slot=Finger" & vbNewLine
        End If
        'Weapon Types
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "One-Hand") Then
            Return "|slot=One-Hand" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Two-Hand") Then
            Return "|slot=Two-Hand" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Two-Hand") Then
            Return "|slot=Two-Hand" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Held In Off-hand") Then
            Return "|slot=Held In Off-hand" & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetItemType()
        'Armor Types
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Plate") Then
            Return "|type=Plate" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Leather") Then
            Return "|type=Leather" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Mail") Then
            Return "|type=Mail" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Cloth") Then
            Return "|type=Cloth" & vbNewLine
        End If

        'Weapon Types
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Fist Weapon") Then
            Return "|type=Fist Weapon" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Dagger") Then
            Return "|type=Dagger" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Bow") Then
            Return "|type=Bow" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Staff") Then
            Return "|type=Staff" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Mace") Then
            Return "|type=Mace" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Polearm") Then
            Return "|type=Polearm" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Sword") Then
            Return "|type=Sword" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Axe") Then
            Return "|type=Axe" & vbNewLine
        End If

        'Bags
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Enchanting Bag") Then
            Return "|type=Enchanting Bag" & vbNewLine
        ElseIf HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Herb Bag") Then
            Return "|type=Herb Bag" & vbNewLine
        ElseIf HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Bag") Then
            Return "|type=Bag" & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetUniqueEquipped()
        '|unique=	Maximum number of this item the player may have in his bags.
        '|unique-eq=	Maximum number of this item the player may have equipped.
        '|unique-type=	Uniqueness category, e.g. "Jeweler's Gems"
        Dim ReturnString = ""
        Dim UniqueFound = False
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Unique-Equipped") Then
            ReturnString = "|unique-eq=1" & vbNewLine
            UniqueFound = True
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Shadowlands Crafted Legendary") Then
            ReturnString &= "|unique-type=Shadowlands Crafted Legendary" & vbNewLine
            UniqueFound = True
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Unique-Equipped: Warlords Crafted (3)") Then
            ReturnString &= "|unique-type=Unique-Equipped: Warlords Crafted" & vbNewLine
            ReturnString = ReturnString.Replace("|unique-eq=1", "|unique-eq=3")
            UniqueFound = True
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Unique") And UniqueFound = False Then
            ReturnString = "|unique=1" & vbNewLine
        End If
        Return ReturnString
    End Function

    Public Function GetBind()
        '|bind=	Item binding type, one of: bop, boe, bou, bta, quest.
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Binds when equipped") Then
            Return "|bind=boe" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Binds when picked up") Then
            Return "|bind=bop" & vbNewLine
        End If
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Quest Item") Then
            Return "|bind=quest" & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetReagent()
        '|reagent=	Number; provide "1" if the item is tagged as a "Crafting Reagent", "2" if the item is tagged as an "Optional Crafting Reagent".
        If HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Optional Crafting Reagent") Then
            Return "|reagent=2" & vbNewLine
        ElseIf HTMLContains(TooltipHTML.DocumentNode.OuterHtml, "Optional Crafting Reagent") Then
            Return "|reagent=1" & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetWeaponDamage()
        '<!--dmg--> (e.g. 30 - 44 Damage)
        Dim DamageFetch = GetInnerTextByComment(TooltipHTML, "span", "<!--dmg-->")
        Dim DamageString = ""
        If DamageFetch <> "" Then
            If Not DamageFetch Is Nothing Then
                If DamageFetch.contains("-") Then
                    DamageString &= "|dmghigh=" & GetIntegerValues(DamageFetch.split("-")(1)) & vbNewLine
                    DamageString &= "|dmglow=" & GetIntegerValues(DamageFetch.split("-")(0)) & vbNewLine
                End If
            End If
            '<!--spd--> (E.g. Speed 2.60)
            Dim WeaponSpeed = GetInnerTextByComment(TooltipHTML, "th", "<!--spd-->")
            If WeaponSpeed.length > 1 Then

                Dim rgx As New Regex("\bSpeed \b")
                Dim result() As String = rgx.Split(WeaponSpeed)
                DamageString &= "|speed=" & GetIntegerValues(result(1)) & vbNewLine
            End If
        End If
        Return DamageString
    End Function

    Public Function GetStats()
        Dim statsString = ""
        '<!--stat3--> (Agility)
        Dim Agility = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat3-->"))
        If Agility.length > 0 Then
            statsString &= "|agility=" & Agility & vbNewLine
        End If

        '<!--stat4--> (Strength)
        Dim Strength = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat4-->"))
        If Strength.length > 0 Then
            statsString &= "|strength=" & Strength & vbNewLine
        End If

        '<!--stat5--> (Intellect)
        Dim Intellect = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat5-->"))
        If Intellect.length > 0 Then
            statsString &= "|intellect=" & Intellect & vbNewLine
        End If

        '<!--stat7--> (Stamina)
        Dim Stamina = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat7-->"))
        If Stamina.length > 0 Then
            statsString &= "|stamina=" & Stamina & vbNewLine
        End If

        '<!--stat71--> (Agility or Strength or Intellect)
        Dim AgilityStrengthIntellect = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat71-->"))
        If AgilityStrengthIntellect.length > 0 Then
            statsString &= "|agistrint=" & AgilityStrengthIntellect & vbNewLine
        End If

        '<!--stat72--> (Agility or Strength or Intellect)
        Dim AgilityStrength = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat72-->"))
        If AgilityStrength.length > 0 Then
            statsString &= "|agistr=" & AgilityStrength & vbNewLine
        End If

        '<!--stat73--> (Agility or Intellect)
        Dim AgilityIntellect = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat73-->"))
        If AgilityIntellect.length > 0 Then
            statsString &= "|agiint=" & AgilityIntellect & vbNewLine
        End If

        '<!--stat74--> (Strength or Intellect)
        Dim StrengthIntellect = GetIntegerValues(GetInnerTextByComment(TooltipHTML, "span", "<!--stat74-->"))
        If StrengthIntellect.length > 0 Then
            statsString &= "|strint=" & StrengthIntellect & vbNewLine
        End If

        '<!--gem1-->
        Dim GemStats = GetInnerTextByComment(TooltipHTML, "span", "<!--gem1-->")
        If GemStats.length > 0 Then
            Dim GemStatsValue = GetIntegerValues(GemStats)
            If GemStats.contains("Agility") Then
                statsString &= "|agility=" & GemStatsValue & vbNewLine
            ElseIf gemstats.contains("Strength") Then
                statsString &= "|strength=" & GemStatsValue & vbNewLine
            ElseIf GemStats.Contains("Stamina") Then
                statsString &= "|stamina=" & GemStatsValue & vbNewLine
            ElseIf GemStats.contains("Intellect") Then
                statsString &= "|intellect=" & GemStatsValue & vbNewLine
            ElseIf GemStats.contains("Critical Strike") Then
                statsString &= "|crit=" & GemStatsValue & vbNewLine
            ElseIf GemStats.contains("Mastery") Then
                statsString &= "|mastery=" & GemStatsValue & vbNewLine
            ElseIf GemStats.contains("Haste") Then
                statsString &= "|haste=" & GemStatsValue & vbNewLine
            ElseIf GemStats.contains("Movement Speed") Then
                statsString &= "|movementspeed=" & GemStatsValue & "%" & vbNewLine
            ElseIf GemStats.contains("Versatility") Then
                statsString &= "|versatility=" & GemStatsValue & "%" & vbNewLine
            End If
        End If

        Return statsString
    End Function

    Public Function GetSet()
        '|set=	If the item is part of the set, Wowpedia set page name.
        Dim SetItem = GetInnerHTMLCustom(TooltipHTML, "//a[contains(@href,'item-set')]")
        If SetItem.length > 1 Then
            Return "|set=" & SetItem.trim() & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetAdditionalStats()
        Dim AdditionalStatsStr = ""
        Dim Dodge = GetInnerTextByComment(TooltipHTML, "span", "<!--rtg13-->")
        If Dodge.length > 0 Then
            Dodge = GetIntegerValues(Dodge.split("Dodge")(0))
            AdditionalStatsStr &= "|dodge=" & Dodge & vbNewLine
        End If

        Dim Parry = GetInnerTextByComment(TooltipHTML, "span", "<!--rtg14-->")
        If Parry.length > 0 Then
            Parry = GetIntegerValues(Parry.split("Parry")(0))
            AdditionalStatsStr &= "|parry=" & Parry & vbNewLine
        End If

        Dim Crit = GetInnerTextByComment(TooltipHTML, "span", "<!--rtg32-->")
        If Crit.length > 0 Then
            Crit = GetIntegerValues(Crit.split("Critical")(0))
            AdditionalStatsStr &= "|crit=" & Crit & vbNewLine
        End If

        Dim Haste = GetInnerTextByComment(TooltipHTML, "span", "<!--rtg36-->")
        If Haste.length > 0 Then
            Haste = GetIntegerValues(Haste.split("Haste")(0))
            AdditionalStatsStr &= "|haste=" & Haste & vbNewLine
        End If

        Dim Versatility = GetInnerTextByComment(TooltipHTML, "span", "<!--rtg40-->")
        If Versatility.length > 0 Then
            Versatility = GetIntegerValues(Versatility.split("Versatility")(0))
            AdditionalStatsStr &= "|versatility=" & Versatility & vbNewLine
        End If

        Dim Mastery = GetInnerTextByComment(TooltipHTML, "span", "<!--rtg49-->")
        If Mastery.length > 0 Then
            Mastery = GetIntegerValues(Mastery.split("Mastery")(0))
            AdditionalStatsStr &= "|mastery=" & Mastery & vbNewLine
        End If

        If AdditionalStatsStr.Length > 1 Then
            Return AdditionalStatsStr
        Else
            Return ""
        End If
    End Function

    Public Function GetArmor()
        '|armor=	Amount of armor on this item.
        Dim ArmorText = GetInnerTextByComment(TooltipHTML, "span", "<!--amr-->")
        If ArmorText.length > 0 Then
            Return "|armor=" & GetIntegerValues(ArmorText) & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetDurability()
        '|durability=	Maximum durability.
        Dim Durability = GetInnerHTMLByComment(TooltipHTML, "td", "<!--ps-->")

        If Durability.length > 0 Then
            If Durability.contains("Durability") Then
                Dim rgx As New Regex("\bDurability \b")
                Dim result() As String = rgx.Split(Durability)
                Return "|durability=" & GetIntegerValues(result(1).Split("/")(0)) & vbNewLine
            End If
        End If
        Return ""
    End Function

    Public Function GetLevel()
        '|level=	Level required to use this item.
        Dim Level = GetInnerHTMLByComment(TooltipHTML, "td", "<!--rlvl-->")
        If Level.length > 0 And Level.contains("rlvl") Then
            Dim rgx As New Regex("\brlvl\b")
            Dim result = rgx.Split(Level)(1)
            If result.Contains("<br>") Then
                Dim rgx2 As New Regex("\b\<br\b")
                Return "|level=" & GetIntegerValues(rgx2.Split(result)(0)) & vbNewLine
            ElseIf result.Contains("sellprice") Then
                Dim rgx2 As New Regex("\bsellprice\b")
                Return "|level=" & GetIntegerValues(rgx2.Split(result)(0)) & vbNewLine
            End If
        End If
        Return ""
    End Function

    Public Function GetIlvl()
        '|ilvl=	Item level. Only displayed for items you can equip. Uses {{DE}} to show disenchant info. Force the display with |geartoken=
        Dim Ilvl = GetInnerTextByComment(TooltipHTML, "span", "<!--ilvl-->")
        If Ilvl.length > 0 Then
            Return "|ilvl=" & GetIntegerValues(Ilvl) & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetStack()
        '|ilvl=	Item level. Only displayed for items you can equip. Uses {{DE}} to show disenchant info. Force the display with |geartoken=
        Dim Stack = GetInnerTextByClass(TooltipHTML, "div", "whtt-extra whtt-maxstack")
        If Stack.length > 0 Then
            Return "|stack=" & GetIntegerValues(Stack) & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetSellPrice()
        '|sellprice=	Amount of copper vendors are willing to give for this item.
        Dim TotalGold As String = GetInnerTextByClass(TooltipHTML, "span", "moneygold")
        Dim TotalSilver As String = GetInnerTextByClass(TooltipHTML, "span", "moneysilver")
        Dim TotalCopper As String = GetInnerTextByClass(TooltipHTML, "span", "moneycopper")
        If TotalGold.Length = 1 Then
            TotalGold = "0" & TotalGold
        End If
        If TotalSilver.Length = 1 Then
            TotalSilver = "0" & TotalSilver
        End If
        If TotalCopper.Length = 1 Then
            TotalCopper = "0" & TotalCopper
        End If
        If TotalGold.Length > 1 Or TotalSilver.Length > 1 Or TotalCopper.Length > 1 Then
            If TotalGold.Length = 0 Then
                TotalGold = "00"
            End If
            If TotalSilver.Length = 0 Then
                TotalSilver = "00"
            End If
            If TotalCopper.Length = 0 Then
                TotalCopper = "00"
            End If
            Return "|sellprice=" & TotalGold & TotalSilver & TotalCopper & vbNewLine
        Else
            Return ""
        End If
    End Function

    Public Function GetItemID()
        '|itemid=	Item ID.
        Return "|itemid=" & Professions.txtItemID.Text & vbNewLine
    End Function

    Public Function GetFlavorText()
        '|flavor=	Flavor text.
        'Dim FlavorText = GetInnerTextCustom(TooltipHTML, "//comment()[contains(.,""rlvl"")]/following-sibling::span")
        'If FlavorText.length > 1 Then
        '    Return "|flavor=" & FlavorText.trim("""") & vbNewLine
        'End If
        Return ""
    End Function

    Public Function GetItemQuality()
        '|quality=	Item quality; one of: poor, common, uncommon, rare, epic, legendary, heirloom, token.
        Dim Name As String = GetInnerTextAfterComment(TooltipHTML, "nstart")
        Dim QualityClass = GetClassName(TooltipHTML, "b", Name)
        If QualityClass.length > 0 Then
            Dim Quality = ""
            Select Case QualityClass
                Case "q1"
                    Quality = "common"
                Case "q2"
                    Quality = "uncommon"
                Case "q3"
                    Quality = "rare"
                Case "q4"
                    Quality = "epic"
                Case "q5"
                    Quality = "legendary"
                Case "q6"
                Case "q7"
                Case "q8"
            End Select
            If Quality.Length > 1 Then
                Return "|quality=" & Quality & vbNewLine
            End If
        End If
        Return ""
    End Function

    Public Function GetIconName()
        '|icon=	Item icon; all lower case, omit File: and extension.
        Dim Icon = GetInnerTextCustom(InfoBoxHTML, "//li[@class='icon-db-link']//a[1]")
        If Icon.length > 0 Then
            Return "|icon=" & Icon.trim() & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetItemMats()
        Dim Nodes = WholePageHTML.DocumentNode.SelectNodes("//td[@style='padding: 0px;']")
        Dim Counter = 1
        Dim Itembox As String = ""
        If Not Nodes Is Nothing Then
            For Each node As HtmlNode In Nodes
                Itembox &= vbNewLine & "{{Itembox|Materials required: Rank " & Counter & vbNewLine
                Dim Nodes2 = node.SelectNodes(".//child::div")
                Dim NodeCount = 1
                For Each node2 As HtmlNode In Nodes2
                    Dim Material = node2.SelectSingleNode(".//a")
                    Dim Amount = node2.SelectSingleNode(".//span[@style='visibility: hidden;']")
                    If Not Material Is Nothing Then
                        Dim newAmount As String
                        If Amount Is Nothing Then
                            newAmount = "1"
                        Else
                            newAmount = Amount.InnerText
                        End If
                        Dim Material2 = Material.GetAttributeValue("href", "")
                        If Material2.Contains("/") And Not Material2.Contains("?filter=") Then
                            Material2 = Material2.Split("/")(2).Replace("-", " ")
                            Material2 = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Material2).Replace("Of", "of").Replace("Shaldorei", "Shal'dorei")
                            Itembox &= "|q" & NodeCount & "=" & newAmount & "|" & Material2 & vbNewLine
                            NodeCount += 1
                        End If
                    End If
                Next
                Itembox &= "}}" & vbNewLine
                Counter += 1
            Next
            If Counter = 2 Then
                Itembox = Itembox.Replace("Materials required: Rank 1", "Materials required:")
            End If
            If Itembox.Length > 0 Then
                Return Itembox
            Else
                Return ""
            End If
        End If
        Return ""
    End Function

    Public Function GetTaughtBy()
        If HTMLContains(WholePageHTML.DocumentNode.OuterHtml, "#taught-by-item") Then
            Dim Nodes = WholePageHTML.DocumentNode.SelectNodes("//tr[@class='listview-row']//td")
            Dim Counter = 1
            Dim Itembox As String = ""
            If Not Nodes Is Nothing Then
                For Each node As HtmlNode In Nodes
                    Itembox &= vbNewLine & "{{Itembox|Materials required: Rank " & Counter & vbNewLine
                    Dim Nodes2 = node.SelectNodes(".//child::div")
                    Dim NodeCount = 1
                    For Each node2 As HtmlNode In Nodes2
                        Dim Material = node2.SelectSingleNode(".//a")
                        Dim Amount = node2.SelectSingleNode(".//span[@style='visibility: hidden;']")
                        If Not Material Is Nothing And Not Amount Is Nothing Then
                            Dim Material2 = Material.GetAttributeValue("href", "").Split("/")(2).Replace("-", " ")
                            Material2 = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Material2).Replace("Of", "of")
                            Itembox &= "|q" & NodeCount & "=" & Amount.InnerText & "|" & Material2 & vbNewLine
                            NodeCount += 1
                        End If
                    Next
                    Itembox &= "}}" & vbNewLine
                    Counter += 1
                Next
                If Itembox.Length > 0 Then
                    Return Itembox
                Else
                    Return ""
                End If
            End If
        End If
        Return ""
    End Function

    Public Function GetClasses()
        '|classes=	List of classes this item Is limited to, e.g. "Mage, Warlock, Priest".
        Dim Classes = GetInnerTextByClass(TooltipHTML, "div", "wowhead-tooltip-item-classes")
        If Classes.length > 1 Then
            Classes = Classes.replace("Classes: ", "")
            Return "|classes=" & Classes & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetUseText()
        '|use=	Use effect.
        Dim UseEffect = GetInnerTextByComment(TooltipHTML, "span", "Use:")
        If UseEffect.length > 1 Then
            UseEffect = UseEffect.replace("Use: ", "")
            Return "|use=" & UseEffect & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetEquipText()
        '|equip=	Misc "On Equip" effects.
        Dim EquipEffect = GetInnerTextByComment(TooltipHTML, "span", "Equip:")
        If EquipEffect.length > 1 Then
            EquipEffect = EquipEffect.replace("Equip: ", "")
            Return "|equip=" & EquipEffect & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetPrismaticSockets()
        '|prismatic-sockets=	Number of prismatic sockets this item has.
        Dim PrismaticSocketCount = Regex.Split(TooltipHTML.DocumentNode.OuterHtml, "Prismatic Socket").Length - 1
        If PrismaticSocketCount > 0 Then
            Return "|prismatic-sockets=" & PrismaticSocketCount & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetMetaSockets()
        '|meta-sockets=	Number of meta sockets this item has.
        Dim MetaSocketCount = Regex.Split(TooltipHTML.DocumentNode.OuterHtml, "Meta Socket").Length - 1
        If MetaSocketCount > 0 Then
            Return "|meta-sockets=" & MetaSocketCount & vbNewLine
        End If
        Return ""
    End Function
    Public Function GetDominationSockets()
        '|domination=	Number of meta sockets this item has.
        Dim DominationSocketCount = Regex.Split(TooltipHTML.DocumentNode.OuterHtml, "Domination Socket").Length - 1
        If DominationSocketCount > 0 Then
            Return "|domination-sockets=" & DominationSocketCount & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetSocketBonus()
        '|sockbonus=	Socket bonus description.
        Dim SocketBonus = GetInnerTextCustom(TooltipHTML, "//span[contains(string(), ""Socket"")]")
        If SocketBonus.length > 0 Then
            Return "|sockbonus=" & SocketBonus.replace("Socket Bonus: +", "") & vbNewLine
        End If
        Return ""
    End Function

    Public Function GetDropBy()
        'Return boss the item is dropped by
        Dim Boss = GetInnerTextCustom(WholePageHTML, "//a[contains(@href,'npc=')]")
        If Boss.length > 1 Then
            Return Boss
        End If
        Return ""
    End Function

    Public Function GetDropLocation()
        'Get Location the item is drop in
        Dim Zone = GetInnerTextCustom(WholePageHTML, "//a[contains(@href,'zone=')]")
        If Zone.length > 1 Then
            Return Zone
        End If
        Return ""
    End Function

    Public Function GetPatch()
        Dim Patch = GetInnerHTMLCustom(InfoBoxHTML, "//div[@id=""infobox-contents-0""]//div[contains(string(),""Added in patch"")]")
        If Patch.length > 1 Then
            Patch = Patch.replace("Added in patch ", "").trim()
            Dim PatchComponents = Patch.Split(".")
            Patch = PatchComponents(0) & "." & PatchComponents(1) & "." & PatchComponents(2)

            If Patch = "9.0.1" Then
                Patch = "9.0.2"
            ElseIf Patch = "6.0.1" Then
                Patch = "6.0.2"
            ElseIf Patch = "5.0.1" Then
                Patch = "5.0.4"
            End If
            Return vbNewLine & "==Patch changes==" & vbNewLine & "*{{Patch " & Patch & "|note=Added.}}"
        Else
            Return vbNewLine & "==Patch changes==" & vbNewLine & "*{{Patch 3.0.2|note=Added.}}"
        End If
    End Function

    Public Function GetExternalLinks()
        Dim linkStr = ""
        linkStr &= vbNewLine & "==External links==" & vbNewLine
        linkStr &= "<!-- Please read https://wow.gamepedia.com/Wowpedia:External_links_policy before adding new links. -->" & vbNewLine
        linkStr &= ("{{Elinks-item|" & Professions.txtItemID.Text & "}}" & vbNewLine)
        Return linkStr
    End Function

    '--------------------------------------------------
    '--------------Wowhead Scraping Functions----------
    '--------------------------------------------------
    Public Function GetIntegerValues(str)
        If Not str Is Nothing Then
            Dim Res As String
            For Each c As Char In str
                If IsNumeric(c) Then
                    Res = Res & c
                End If
            Next
            If Res Is Nothing Then
                Return ""
            Else
                Return Res
            End If
        End If
        Return ""
    End Function

    Public Function GetInnerTextAfterComment(doc, htmlComment)
        Dim Node As HtmlNode = doc.DocumentNode.SelectSingleNode("//comment()[contains(.,'" & htmlComment & "')]/following-sibling::*[1]")
        If Not Node Is Nothing Then
            If Not Node.InnerText Is Nothing Then
                Return Node.InnerText
            End If
        End If
        Return ""
    End Function

    Public Function GetInnerTextByClass(doc, tagName, ClassName)
        'Gets the InnerText of an element by specifying the tagname and all classes
        Dim Node As HtmlNode = doc.DocumentNode.SelectSingleNode("//" & tagName & "[@class='" & ClassName & "']")
        If Not Node Is Nothing Then
            If Not Node.InnerText Is Nothing Then
                Return Node.InnerText
            End If
        End If
        Return ""
    End Function

    Public Function GetInnerTextByComment(doc, tagname, comment)
        Dim Nodes = doc.DocumentNode.SelectNodes("//" & tagname)
        If Not Nodes Is Nothing Then
            For Each node As HtmlNode In Nodes
                If node.InnerHtml.Contains(comment) Then
                    Return node.InnerText
                End If
            Next
        End If
        Return ""
    End Function

    Public Function GetInnerHTMLByComment(doc, tagname, comment)
        Dim Nodes = doc.DocumentNode.SelectNodes("//" & tagname)
        If Not Nodes Is Nothing Then
            For Each node As HtmlNode In Nodes
                If node.InnerHtml.Contains(comment) Then
                    Return node.InnerHtml
                End If
            Next
        End If
        Return ""
    End Function

    Public Function GetClassName(doc, tagName, InnerText)
        'Get the InnerText of an element by specifying the tagname and the inner text
        Dim Node As HtmlNode = doc.DocumentNode.SelectSingleNode("//" & tagName & "[.=""" & InnerText & """]")
        If Not Node Is Nothing Then
            If Not Node.GetAttributeValue("class", "") Is Nothing Then
                Return Node.GetAttributeValue("class", "")
            End If
        End If
        Return ""
    End Function

    Public Function GetHTMLInsideTag(doc, tagName, className)
        'Gets the InnerHTML of an element by specifying the tagname and at least one of the classes
        Dim Node As HtmlNode = doc.DocumentNode.SelectSingleNode("//" & tagName & "[contains(@class,'" & className & "')]")
        If Not Node Is Nothing Then
            If Not Node.InnerHtml Is Nothing Then
                Return Node.InnerHtml
            End If
        End If
        Return ""
    End Function
    Public Function GetInnerTextCustom(doc, customXpath)
        'Get the InnerText of an element by specifying the tagname and unique classname
        Dim Node As HtmlNode = doc.DocumentNode.SelectSingleNode(customXpath)
        If Not Node Is Nothing Then
            If Not Node.InnerText Is Nothing Then
                Return Node.InnerText
            End If
        End If
        Return ""
    End Function
    Public Function GetInnerHTMLCustom(doc, customXpath)
        'Get the InnerText of an element by specifying the tagname and unique classname
        Dim Node As HtmlNode = doc.DocumentNode.SelectSingleNode(customXpath)
        If Not Node Is Nothing Then
            If Not Node.InnerText Is Nothing Then
                Return Node.InnerHtml
            End If
        End If
        Return ""
    End Function

    Public Function HTMLContains(html, stringSearch)
        If html.Contains(stringSearch) Then
            Return True
        Else
            Return False
        End If
    End Function
End Module
