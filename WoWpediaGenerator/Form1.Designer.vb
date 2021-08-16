<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.WebWowpedia = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.btnInsertToWowpedia = New System.Windows.Forms.Button()
        Me.btnAchievement = New System.Windows.Forms.Button()
        Me.txtWebpageSource = New System.Windows.Forms.TextBox()
        Me.chkDungeonRaidsTable = New System.Windows.Forms.CheckBox()
        Me.chkFoS = New System.Windows.Forms.CheckBox()
        Me.btnGenerateAll = New System.Windows.Forms.Button()
        Me.txtProcessing = New System.Windows.Forms.TextBox()
        Me.lbIncorrectPages = New System.Windows.Forms.ListBox()
        Me.btnGenerateList = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.btnFixCategories = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(13, 35)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(220, 21)
        Me.ComboBox1.TabIndex = 0
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(248, 12)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(677, 470)
        Me.TextBox1.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(850, 897)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(11, 262)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox4.Size = New System.Drawing.Size(230, 220)
        Me.TextBox4.TabIndex = 8
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(13, 177)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(220, 21)
        Me.ComboBox2.TabIndex = 9
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(13, 85)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(137, 17)
        Me.CheckBox1.TabIndex = 10
        Me.CheckBox1.Text = "Add Shadowlands Stub"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 134)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Current Category ID"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(118, 131)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(124, 20)
        Me.TextBox5.TabIndex = 11
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(13, 62)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(181, 17)
        Me.CheckBox2.TabIndex = 13
        Me.CheckBox2.Text = "Add links to Achievement Criteria"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(13, 108)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(135, 17)
        Me.CheckBox3.TabIndex = 14
        Me.CheckBox3.Text = "Add Achievement Stub"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 233)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(229, 23)
        Me.Button2.TabIndex = 15
        Me.Button2.Text = "Save list of achievements to file"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(150, 20)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Select Achievement"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(13, 204)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(137, 23)
        Me.Button3.TabIndex = 17
        Me.Button3.Text = "Generate Category Table"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 154)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 20)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Select Category"
        '
        'WebWowpedia
        '
        Me.WebWowpedia.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebWowpedia.CreationProperties = Nothing
        Me.WebWowpedia.Location = New System.Drawing.Point(929, 14)
        Me.WebWowpedia.Name = "WebWowpedia"
        Me.WebWowpedia.Size = New System.Drawing.Size(613, 906)
        Me.WebWowpedia.TabIndex = 19
        Me.WebWowpedia.Text = "WebView21"
        Me.WebWowpedia.ZoomFactor = 1.0R
        '
        'btnInsertToWowpedia
        '
        Me.btnInsertToWowpedia.Location = New System.Drawing.Point(766, 488)
        Me.btnInsertToWowpedia.Name = "btnInsertToWowpedia"
        Me.btnInsertToWowpedia.Size = New System.Drawing.Size(157, 36)
        Me.btnInsertToWowpedia.TabIndex = 20
        Me.btnInsertToWowpedia.Text = "Insert to Wowpedia"
        Me.btnInsertToWowpedia.UseVisualStyleBackColor = True
        '
        'btnAchievement
        '
        Me.btnAchievement.Location = New System.Drawing.Point(603, 488)
        Me.btnAchievement.Name = "btnAchievement"
        Me.btnAchievement.Size = New System.Drawing.Size(157, 36)
        Me.btnAchievement.TabIndex = 21
        Me.btnAchievement.Text = "Achievement Page"
        Me.btnAchievement.UseVisualStyleBackColor = True
        '
        'txtWebpageSource
        '
        Me.txtWebpageSource.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtWebpageSource.Location = New System.Drawing.Point(8, 810)
        Me.txtWebpageSource.Multiline = True
        Me.txtWebpageSource.Name = "txtWebpageSource"
        Me.txtWebpageSource.Size = New System.Drawing.Size(915, 110)
        Me.txtWebpageSource.TabIndex = 22
        '
        'chkDungeonRaidsTable
        '
        Me.chkDungeonRaidsTable.AutoSize = True
        Me.chkDungeonRaidsTable.Location = New System.Drawing.Point(11, 488)
        Me.chkDungeonRaidsTable.Name = "chkDungeonRaidsTable"
        Me.chkDungeonRaidsTable.Size = New System.Drawing.Size(132, 17)
        Me.chkDungeonRaidsTable.TabIndex = 23
        Me.chkDungeonRaidsTable.Text = "Dungeon/Raids Table"
        Me.chkDungeonRaidsTable.UseVisualStyleBackColor = True
        '
        'chkFoS
        '
        Me.chkFoS.AutoSize = True
        Me.chkFoS.Location = New System.Drawing.Point(11, 507)
        Me.chkFoS.Name = "chkFoS"
        Me.chkFoS.Size = New System.Drawing.Size(132, 17)
        Me.chkFoS.TabIndex = 24
        Me.chkFoS.Text = "Feat of Strength Table"
        Me.chkFoS.UseVisualStyleBackColor = True
        '
        'btnGenerateAll
        '
        Me.btnGenerateAll.Location = New System.Drawing.Point(156, 204)
        Me.btnGenerateAll.Name = "btnGenerateAll"
        Me.btnGenerateAll.Size = New System.Drawing.Size(86, 23)
        Me.btnGenerateAll.TabIndex = 25
        Me.btnGenerateAll.Text = "Generate All"
        Me.btnGenerateAll.UseVisualStyleBackColor = True
        '
        'txtProcessing
        '
        Me.txtProcessing.Location = New System.Drawing.Point(182, 488)
        Me.txtProcessing.Name = "txtProcessing"
        Me.txtProcessing.Size = New System.Drawing.Size(341, 20)
        Me.txtProcessing.TabIndex = 26
        '
        'lbIncorrectPages
        '
        Me.lbIncorrectPages.FormattingEnabled = True
        Me.lbIncorrectPages.Location = New System.Drawing.Point(8, 530)
        Me.lbIncorrectPages.Name = "lbIncorrectPages"
        Me.lbIncorrectPages.Size = New System.Drawing.Size(295, 264)
        Me.lbIncorrectPages.TabIndex = 27
        '
        'btnGenerateList
        '
        Me.btnGenerateList.Location = New System.Drawing.Point(493, 514)
        Me.btnGenerateList.Name = "btnGenerateList"
        Me.btnGenerateList.Size = New System.Drawing.Size(86, 23)
        Me.btnGenerateList.TabIndex = 28
        Me.btnGenerateList.Text = "Generate List"
        Me.btnGenerateList.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(380, 584)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(470, 20)
        Me.TextBox2.TabIndex = 29
        '
        'btnFixCategories
        '
        Me.btnFixCategories.Location = New System.Drawing.Point(603, 530)
        Me.btnFixCategories.Name = "btnFixCategories"
        Me.btnFixCategories.Size = New System.Drawing.Size(86, 23)
        Me.btnFixCategories.TabIndex = 30
        Me.btnFixCategories.Text = "Fix Categories"
        Me.btnFixCategories.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1554, 929)
        Me.Controls.Add(Me.btnFixCategories)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.btnGenerateList)
        Me.Controls.Add(Me.lbIncorrectPages)
        Me.Controls.Add(Me.txtProcessing)
        Me.Controls.Add(Me.btnGenerateAll)
        Me.Controls.Add(Me.chkFoS)
        Me.Controls.Add(Me.chkDungeonRaidsTable)
        Me.Controls.Add(Me.txtWebpageSource)
        Me.Controls.Add(Me.btnAchievement)
        Me.Controls.Add(Me.btnInsertToWowpedia)
        Me.Controls.Add(Me.WebWowpedia)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.CheckBox3)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Name = "Form1"
        Me.Text = "Wowpedia Template Generator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents CheckBox3 As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents WebWowpedia As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents btnInsertToWowpedia As Button
    Friend WithEvents btnAchievement As Button
    Friend WithEvents txtWebpageSource As TextBox
    Friend WithEvents chkDungeonRaidsTable As CheckBox
    Friend WithEvents chkFoS As CheckBox
    Friend WithEvents btnGenerateAll As Button
    Friend WithEvents txtProcessing As TextBox
    Friend WithEvents lbIncorrectPages As ListBox
    Friend WithEvents btnGenerateList As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents btnFixCategories As Button
End Class
