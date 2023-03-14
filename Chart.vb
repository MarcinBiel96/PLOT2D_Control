Public Class Chart
    Private plot As Bitmap
    Private gfx As Graphics
    Private dataset() As Double
    Private pointset() As PointF
    Private zoomY As Double = -1
    Private dispY As Double = 0
    Private zoomX As Double = 1
    Private dispX As Double = 0
    Private tickY As Double = 50
    Private tickX As Double = 100
    Private initialised As Boolean
    Private Ytags() As Integer
    Private Xtags() As Integer
    Private Gridcolor As Pen
    Private p_frame As Pen
    Private textcolor As Brush
    Private SelectionEnabled As Boolean
    Public Property Grid_color As Color
    Public Property Text_color As Color
    Public Property Frame_color As Color
    Public Property Selection_color As Color

    Public Property Zoom_Vertical() As String
        Get
            Return -Me.zoomY
        End Get
        Set(ByVal value As String)
            If value > 0 Then
                Me.zoomY = -value
                RePaint(Nothing, Nothing)
            End If
        End Set
    End Property
    Public Property Zoom_Horizontal() As String
        Get
            Return Me.zoomX
        End Get
        Set(ByVal value As String)
            If value > 0 Then
                Me.zoomX = value
                RePaint(Nothing, Nothing)
            End If
        End Set
    End Property
    Public Property Offset_Horizontal() As String
        Get
            Return Me.dispX
        End Get
        Set(ByVal value As String)
            Me.dispX = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Offset_Vertical() As String
        Get
            Return Me.dispY
        End Get
        Set(ByVal value As String)
            Me.dispY = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Ticks_Horizontal() As String
        Get
            Return Me.tickY
        End Get
        Set(ByVal value As String)
            If value > 0 Then
                Me.tickY = value
                RePaint(Nothing, Nothing)
            End If
        End Set
    End Property
    Public Property Ticks_Vertical() As String
        Get
            Return Me.tickX
        End Get
        Set(ByVal value As String)
            If value > 0 Then
                Me.tickX = value
                RePaint(Nothing, Nothing)
            End If
        End Set
    End Property
    Public Property Selection_Enabled() As Boolean
        Get
            Return Me.SelectionEnabled
        End Get
        Set(ByVal value As Boolean)
            Me.SelectionEnabled = value
        End Set
    End Property

    Public Sub Plot2D(Data() As Double)
        dataset = Data
        RePaint(Nothing, Nothing)
    End Sub
    Public Sub AutoScaleY()
        zoomY = -(Height - 93) / ((dataset.Max) - (dataset.Min))
        dispY = -(dataset.Max + dataset.Min) / 2 * -zoomY
        tickY = ((dataset.Max) - (dataset.Min)) / 4
    End Sub

    Private Sub Chart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Grid_color = Color.DimGray
        Text_color = Color.White
        Frame_color = Color.White
        Selection_color = Color.FromArgb(128, 0, 0, 255)
        Dim data(1000) As Double
        ReDim dataset(1000)
        For i = 0 To 1000
            dataset(i) = 10 * Math.Sin(i / 25)
        Next
        AutoScaleY()
        Plot2D()
        initialised = True
    End Sub
    Private Sub Plot2D()
        Dim i As Integer
        Dim p As Pen = New Pen(ForeColor)
        selectionActive = False
        Gridcolor = New Pen(Grid_color)
        p_frame = New Pen(Frame_color)
        textcolor = New SolidBrush(Text_color)
        plot = New Bitmap(Width - 90, Height - 90)
        ReDim pointset(dataset.Length - 1)
        For i = 0 To dataset.Length - 1
            pointset(i) = New PointF(i * zoomX - dispX, dataset(i) * zoomY + plot.Height \ 2 - dispY)
        Next
        gfx = Graphics.FromImage(plot)
        i = 0
        ReDim Ytags(0)
        ReDim Xtags(0)
        While plot.Height \ 2 + 1 - zoomY * i * tickY < plot.Height
            Array.Resize(Ytags, Ytags.Length + 2)
            gfx.DrawLine(Gridcolor, 0, plot.Height \ 2 + CType(zoomY * i * tickY, Integer), plot.Width, plot.Height \ 2 + CType(zoomY * i * tickY, Integer))
            Ytags(Ytags.Length - 2) = plot.Height \ 2 + CType(zoomY * i * tickY, Integer)
            gfx.DrawLine(Gridcolor, 0, plot.Height \ 2 - CType(zoomY * i * tickY, Integer), plot.Width, plot.Height \ 2 - CType(zoomY * i * tickY, Integer))
            Ytags(Ytags.Length - 1) = plot.Height \ 2 - CType(zoomY * i * tickY, Integer)
            i += 1
        End While
        i = 0
        While zoomX * i * tickX < plot.Width
            Array.Resize(Xtags, Xtags.Length + 1)
            gfx.DrawLine(Gridcolor, CType(zoomX * i * tickX, Integer), 0, CType(zoomX * i * tickX, Integer), plot.Height)
            Xtags(Xtags.Length - 2) = CType(zoomX * i * tickX, Integer)
            i += 1
        End While
        gfx.DrawLines(p, pointset) 'wykres
    End Sub
    Private Sub Chart_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        e.Graphics.DrawImage(plot, 79, 11)
        e.Graphics.DrawRectangle(p_frame, 78, 10, plot.Width + 1, plot.Height + 1)
        Dim sf As New StringFormat
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        For i = 2 To Ytags.Length - 1
            e.Graphics.DrawString((Ytags(i) - plot.Height \ 2 + dispY) / zoomY, Font, textcolor, New RectangleF(0, Ytags(i) + 3, 78, 20), sf)
        Next
        If selectionActive Then
            e.Graphics.FillRectangle(New SolidBrush(Selection_color), Math.Max(Math.Min(selectionbegin + selectionend, selectionbegin), 80), 11, Math.Max(-selectionend, selectionend), plot.Height)
        End If
        e.Graphics.RotateTransform(-90)
        e.Graphics.TranslateTransform(-plot.Height - 90, 68)
        For i = 1 To Xtags.Length - 1
            e.Graphics.DrawString((Xtags(i) + dispX) / zoomX, Font, textcolor, New RectangleF(0, Xtags(i) + 3, 78, 20), sf)
        Next
    End Sub
    Private Sub RePaint(sender As Object, e As EventArgs) Handles Me.Resize, Me.ForeColorChanged, Me.BackColorChanged
        If initialised Then
            Plot2D()
            Refresh()
        End If
    End Sub

    Private selectionActive As Boolean
    Private selectionbegin As Integer
    Private selectionend As Integer
    Public SelectBegin As Integer
    Public SelectEnd As Integer
    Private selectionInProgress As Boolean
    Private Sub Chart_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If SelectionEnabled Then
            If e.Button = MouseButtons.Left AndAlso e.X > 80 AndAlso e.X < Width - 11 AndAlso e.Y > 10 AndAlso e.Y < Height - 79 Then
            selectionbegin = e.X
            selectionend = 0
            selectionInProgress = True
            selectionActive = True
        Else selectionActive = False
        End If
            Refresh()
        End If
    End Sub
    Private Sub Chart_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If SelectionEnabled Then
            If selectionInProgress Then
                If e.X < 79 Then
                    selectionend = 79 - selectionbegin
                ElseIf e.X > Width - 11 Then
                    selectionend = Width - 11 - selectionbegin
                Else
                    selectionend = e.X - selectionbegin
                End If
                Refresh()
            End If
        End If
    End Sub
    Private Sub Chart_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        If SelectionEnabled Then
            If e.Button = MouseButtons.Left Then
                selectionInProgress = False
                If selectionend = 0 Then
                    selectionActive = False
                Else
                    SelectBegin = (selectionbegin - 79 + dispX) / zoomX
                    SelectEnd = (selectionbegin - 79 + selectionend + dispX) / zoomX
                End If
            End If
        End If
    End Sub
End Class
