Public Class UserControl1
    Dim plot As Bitmap
    Dim gfx As Graphics
    Dim dataset() As Double
    Dim pointset() As PointF
    Dim zoomY As Double = -50
    Dim dispY As Double = 0
    Dim zoomX As Double = 1
    Dim dispX As Double = 0

    Private Sub UserControl1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''''''''''''''''''''''
        Dim data(1000) As Double
        ReDim dataset(1000)
        For i = 0 To 1000
            dataset(i) = Math.Sin(i / 10)
        Next
        ''''''''''''''''''''''
        plot = New Bitmap(Width, Height)
        ReDim pointset(dataset.Length - 1)
        For i = 0 To dataset.Length - 1
            pointset(i) = New PointF(i * zoomX - dispX, dataset(i) * zoomY + Height \ 2 + 1 - dispY)
        Next
        gfx = Graphics.FromImage(plot)
        gfx.DrawLine(Pens.DimGray, 0, Height \ 2 + 1 - CType(dispY, Integer), Width, Height \ 2 + 1 - CType(dispY, Integer)) 'oś X
        gfx.DrawLines(Pens.Lime, pointset) 'wykres
    End Sub

    Private Sub UserControl1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        e.Graphics.DrawImage(plot, 0, 0)
    End Sub
End Class
