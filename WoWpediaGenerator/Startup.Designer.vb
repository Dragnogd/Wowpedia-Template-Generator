<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Startup
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
        Me.btnAchievements = New System.Windows.Forms.Button()
        Me.btnProfessions = New System.Windows.Forms.Button()
        Me.chkLoadInstanceIDS = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btnAchievements
        '
        Me.btnAchievements.Location = New System.Drawing.Point(3, 3)
        Me.btnAchievements.Name = "btnAchievements"
        Me.btnAchievements.Size = New System.Drawing.Size(296, 107)
        Me.btnAchievements.TabIndex = 0
        Me.btnAchievements.Text = "Achievements"
        Me.btnAchievements.UseVisualStyleBackColor = True
        '
        'btnProfessions
        '
        Me.btnProfessions.Location = New System.Drawing.Point(3, 116)
        Me.btnProfessions.Name = "btnProfessions"
        Me.btnProfessions.Size = New System.Drawing.Size(296, 107)
        Me.btnProfessions.TabIndex = 1
        Me.btnProfessions.Text = "Professions"
        Me.btnProfessions.UseVisualStyleBackColor = True
        '
        'chkLoadInstanceIDS
        '
        Me.chkLoadInstanceIDS.AutoSize = True
        Me.chkLoadInstanceIDS.Location = New System.Drawing.Point(320, 27)
        Me.chkLoadInstanceIDS.Name = "chkLoadInstanceIDS"
        Me.chkLoadInstanceIDS.Size = New System.Drawing.Size(147, 17)
        Me.chkLoadInstanceIDS.TabIndex = 2
        Me.chkLoadInstanceIDS.Text = "Load Instance ID's (Slow)"
        Me.chkLoadInstanceIDS.UseVisualStyleBackColor = True
        '
        'Startup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(485, 229)
        Me.Controls.Add(Me.chkLoadInstanceIDS)
        Me.Controls.Add(Me.btnProfessions)
        Me.Controls.Add(Me.btnAchievements)
        Me.Name = "Startup"
        Me.Text = "Startup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnAchievements As Button
    Friend WithEvents btnProfessions As Button
    Friend WithEvents chkLoadInstanceIDS As CheckBox
End Class
