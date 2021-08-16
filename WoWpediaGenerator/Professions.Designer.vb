<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Professions
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.btnFetchLinks = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtProfession = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtItemID = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnGeneratePage = New System.Windows.Forms.Button()
        Me.txtExpansion = New System.Windows.Forms.TextBox()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.btnItemPage = New System.Windows.Forms.Button()
        Me.btnOutputLinks = New System.Windows.Forms.Button()
        Me.chkDropBoss = New System.Windows.Forms.CheckBox()
        Me.txtItemName = New System.Windows.Forms.TextBox()
        Me.btnQueueItems = New System.Windows.Forms.Button()
        Me.txtItemQueueLength = New System.Windows.Forms.TextBox()
        Me.btnClearQueue = New System.Windows.Forms.Button()
        Me.cboItems = New System.Windows.Forms.ComboBox()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.txtLoadProgress = New System.Windows.Forms.TextBox()
        Me.cboNewPagesOnly = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'WebView21
        '
        Me.WebView21.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebView21.CreationProperties = Nothing
        Me.WebView21.Location = New System.Drawing.Point(758, 6)
        Me.WebView21.Name = "WebView21"
        Me.WebView21.Size = New System.Drawing.Size(643, 623)
        Me.WebView21.TabIndex = 0
        Me.WebView21.Text = "WebView21"
        Me.WebView21.ZoomFactor = 1.0R
        '
        'btnFetchLinks
        '
        Me.btnFetchLinks.Location = New System.Drawing.Point(12, 12)
        Me.btnFetchLinks.Name = "btnFetchLinks"
        Me.btnFetchLinks.Size = New System.Drawing.Size(139, 50)
        Me.btnFetchLinks.TabIndex = 1
        Me.btnFetchLinks.Text = "Fetch Links"
        Me.btnFetchLinks.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(12, 154)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(740, 475)
        Me.TextBox1.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(330, 13)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(220, 49)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtProfession
        '
        Me.txtProfession.Location = New System.Drawing.Point(637, 14)
        Me.txtProfession.Name = "txtProfession"
        Me.txtProfession.Size = New System.Drawing.Size(114, 20)
        Me.txtProfession.TabIndex = 4
        Me.txtProfession.Text = "Tailoring"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(563, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Profession"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(561, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Expansion ID"
        '
        'txtItemID
        '
        Me.txtItemID.Location = New System.Drawing.Point(637, 66)
        Me.txtItemID.Name = "txtItemID"
        Me.txtItemID.Size = New System.Drawing.Size(114, 20)
        Me.txtItemID.TabIndex = 8
        Me.txtItemID.Text = "172320"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(578, 69)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Item ID"
        '
        'btnGeneratePage
        '
        Me.btnGeneratePage.Location = New System.Drawing.Point(637, 91)
        Me.btnGeneratePage.Name = "btnGeneratePage"
        Me.btnGeneratePage.Size = New System.Drawing.Size(115, 26)
        Me.btnGeneratePage.TabIndex = 10
        Me.btnGeneratePage.Text = "Generate Page"
        Me.btnGeneratePage.UseVisualStyleBackColor = True
        '
        'txtExpansion
        '
        Me.txtExpansion.Location = New System.Drawing.Point(637, 40)
        Me.txtExpansion.Name = "txtExpansion"
        Me.txtExpansion.Size = New System.Drawing.Size(114, 20)
        Me.txtExpansion.TabIndex = 5
        '
        'btnInsert
        '
        Me.btnInsert.Location = New System.Drawing.Point(637, 123)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(114, 25)
        Me.btnInsert.TabIndex = 11
        Me.btnInsert.Text = "Insert"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'btnItemPage
        '
        Me.btnItemPage.Location = New System.Drawing.Point(277, 65)
        Me.btnItemPage.Name = "btnItemPage"
        Me.btnItemPage.Size = New System.Drawing.Size(175, 46)
        Me.btnItemPage.TabIndex = 12
        Me.btnItemPage.Text = "Item Page"
        Me.btnItemPage.UseVisualStyleBackColor = True
        '
        'btnOutputLinks
        '
        Me.btnOutputLinks.Location = New System.Drawing.Point(12, 66)
        Me.btnOutputLinks.Name = "btnOutputLinks"
        Me.btnOutputLinks.Size = New System.Drawing.Size(259, 44)
        Me.btnOutputLinks.TabIndex = 13
        Me.btnOutputLinks.Text = "Output Links"
        Me.btnOutputLinks.UseVisualStyleBackColor = True
        '
        'chkDropBoss
        '
        Me.chkDropBoss.AutoSize = True
        Me.chkDropBoss.Location = New System.Drawing.Point(477, 74)
        Me.chkDropBoss.Name = "chkDropBoss"
        Me.chkDropBoss.Size = New System.Drawing.Size(90, 17)
        Me.chkDropBoss.TabIndex = 14
        Me.chkDropBoss.Text = "Drop By Boss"
        Me.chkDropBoss.UseVisualStyleBackColor = True
        '
        'txtItemName
        '
        Me.txtItemName.Location = New System.Drawing.Point(517, 95)
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(114, 20)
        Me.txtItemName.TabIndex = 16
        '
        'btnQueueItems
        '
        Me.btnQueueItems.Location = New System.Drawing.Point(157, 10)
        Me.btnQueueItems.Name = "btnQueueItems"
        Me.btnQueueItems.Size = New System.Drawing.Size(139, 50)
        Me.btnQueueItems.TabIndex = 17
        Me.btnQueueItems.Text = "Queue Items"
        Me.btnQueueItems.UseVisualStyleBackColor = True
        '
        'txtItemQueueLength
        '
        Me.txtItemQueueLength.Location = New System.Drawing.Point(12, 116)
        Me.txtItemQueueLength.Name = "txtItemQueueLength"
        Me.txtItemQueueLength.Size = New System.Drawing.Size(114, 20)
        Me.txtItemQueueLength.TabIndex = 18
        '
        'btnClearQueue
        '
        Me.btnClearQueue.Location = New System.Drawing.Point(132, 116)
        Me.btnClearQueue.Name = "btnClearQueue"
        Me.btnClearQueue.Size = New System.Drawing.Size(120, 20)
        Me.btnClearQueue.TabIndex = 19
        Me.btnClearQueue.Text = "Clear Queue"
        Me.btnClearQueue.UseVisualStyleBackColor = True
        '
        'cboItems
        '
        Me.cboItems.FormattingEnabled = True
        Me.cboItems.Location = New System.Drawing.Point(258, 117)
        Me.cboItems.Name = "cboItems"
        Me.cboItems.Size = New System.Drawing.Size(214, 21)
        Me.cboItems.TabIndex = 21
        '
        'txtLoadProgress
        '
        Me.txtLoadProgress.Location = New System.Drawing.Point(531, 128)
        Me.txtLoadProgress.Name = "txtLoadProgress"
        Me.txtLoadProgress.Size = New System.Drawing.Size(100, 20)
        Me.txtLoadProgress.TabIndex = 22
        '
        'cboNewPagesOnly
        '
        Me.cboNewPagesOnly.AutoSize = True
        Me.cboNewPagesOnly.Location = New System.Drawing.Point(478, 116)
        Me.cboNewPagesOnly.Name = "cboNewPagesOnly"
        Me.cboNewPagesOnly.Size = New System.Drawing.Size(105, 17)
        Me.cboNewPagesOnly.TabIndex = 23
        Me.cboNewPagesOnly.Text = "New Pages Only"
        Me.cboNewPagesOnly.UseVisualStyleBackColor = True
        '
        'Professions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1410, 641)
        Me.Controls.Add(Me.cboNewPagesOnly)
        Me.Controls.Add(Me.txtLoadProgress)
        Me.Controls.Add(Me.cboItems)
        Me.Controls.Add(Me.btnClearQueue)
        Me.Controls.Add(Me.txtItemQueueLength)
        Me.Controls.Add(Me.btnQueueItems)
        Me.Controls.Add(Me.txtItemName)
        Me.Controls.Add(Me.chkDropBoss)
        Me.Controls.Add(Me.btnOutputLinks)
        Me.Controls.Add(Me.btnItemPage)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.btnGeneratePage)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtItemID)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtExpansion)
        Me.Controls.Add(Me.txtProfession)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnFetchLinks)
        Me.Controls.Add(Me.WebView21)
        Me.Name = "Professions"
        Me.Text = "Professions"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents btnFetchLinks As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents txtProfession As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtItemID As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnGeneratePage As Button
    Friend WithEvents txtExpansion As TextBox
    Friend WithEvents btnInsert As Button
    Friend WithEvents btnItemPage As Button
    Friend WithEvents btnOutputLinks As Button
    Friend WithEvents chkDropBoss As CheckBox
    Friend WithEvents txtItemName As TextBox
    Friend WithEvents btnQueueItems As Button
    Friend WithEvents txtItemQueueLength As TextBox
    Friend WithEvents btnClearQueue As Button
    Friend WithEvents cboItems As ComboBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents txtLoadProgress As TextBox
    Friend WithEvents cboNewPagesOnly As CheckBox
End Class
