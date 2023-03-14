<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Chart
    Inherits System.Windows.Forms.UserControl

    'UserControl1 przesłania metodę dispose, aby wyczyścić listę składników.
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

    'Wymagane przez Projektanta formularzy systemu Windows
    Private components As System.ComponentModel.IContainer

    'UWAGA: następująca procedura jest wymagana przez Projektanta formularzy systemu Windows
    'Możesz to modyfikować, używając Projektanta formularzy systemu Windows. 
    'Nie należy modyfikować za pomocą edytora kodu.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'Chart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.Cyan
        Me.MinimumSize = New System.Drawing.Size(100, 100)
        Me.Name = "Chart"
        Me.Size = New System.Drawing.Size(500, 500)
        Me.ResumeLayout(False)

    End Sub

End Class
