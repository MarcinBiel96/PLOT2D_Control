Public Class Chart
    Private plot As Bitmap
    Private gfx As Graphics
    Public dataset() As Double
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
    Private markers() As Integer
    Private unitmultipX As Double = 1
    Private unitmultipY As Double = 1

    Private lw As Single = 1
    Private Gridcolorp As Pen
    Private p_framep As Pen
    Private textcolorb As Brush
    Private SelectionEnabled As Boolean

    Private gridcolor As Color = Color.DimGray
    Private textcolor As Color = Color.White
    Private framecolor As Color = Color.White
    Private selectioncolor As Color = Color.FromArgb(128, 0, 0, 255)
    Public Property Line_width() As Single
        Get
            Return Me.lw
        End Get
        Set(ByVal value As Single)
            Me.lw = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Grid_color() As Color
        Get
            Return Me.Gridcolor
        End Get
        Set(ByVal value As color)
            Me.gridcolor = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Text_color As Color
        Get
            Return Me.textcolor
        End Get
        Set(ByVal value As color)
            Me.textcolor = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Frame_color As Color
        Get
            Return Me.Framecolor
        End Get
        Set(ByVal value As color)
            Me.framecolor = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Selection_color As Color
        Get
            Return Me.Selectioncolor
        End Get
        Set(ByVal value As color)
            Me.selectioncolor = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Zoom_Vertical() As Double
        Get
            Return -Me.zoomY
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                Me.zoomY = -value
                RePaint(Nothing, Nothing)
            End If
        End Set
    End Property
    Public Property Zoom_Horizontal() As Double
        Get
            Return Me.zoomX
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                Me.zoomX = value
                RePaint(Nothing, Nothing)
            End If
        End Set
    End Property
    Public Property Offset_Horizontal() As Double
        Get
            Return Me.dispX
        End Get
        Set(ByVal value As Double)
            Me.dispX = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Offset_Vertical() As Double
        Get
            Return Me.dispY
        End Get
        Set(ByVal value As Double)
            Me.dispY = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Ticks_Horizontal() As Double
        Get
            Return Me.tickY
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                Me.tickY = value
                RePaint(Nothing, Nothing)
            End If
        End Set
    End Property
    Public Property Ticks_Vertical() As Double
        Get
            Return Me.tickX
        End Get
        Set(ByVal value As Double)
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
    Public Property Unit_MutiplierX() As Double
        Get
            Return Me.unitmultipX
        End Get
        Set(ByVal value As Double)
            Me.unitmultipX = value
            RePaint(Nothing, Nothing)
        End Set
    End Property
    Public Property Unit_Mutipliery() As Double
        Get
            Return Me.unitmultipY
        End Get
        Set(ByVal value As Double)
            Me.unitmultipY = value
            RePaint(Nothing, Nothing)
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
        If initialised Then RePaint(Nothing, Nothing)
    End Sub

    Public Sub AddMarker(PointNo As Integer)
        Array.Resize(markers, markers.Length + 1)
        markers(markers.Length - 1) = PointNo
    End Sub
    Public Sub ClearMarkers()
        ReDim markers(0)
    End Sub

    Private Sub Chart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim data(1000) As Double
        ReDim dataset(1000)
        For i = 0 To 1000
            dataset(i) = 10 * Math.Sin(i / 25)
        Next
        'AutoScaleY()
        ReDim markers(0)
        Plot2D()
        initialised = True
    End Sub
    Private Sub Plot2D()
        Dim i As Integer
        Dim p As Pen = New Pen(ForeColor, lw)
        selectionActive = False
        Gridcolorp = New Pen(gridcolor)
        p_framep = New Pen(framecolor)
        textcolorb = New SolidBrush(textcolor)
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
            gfx.DrawLine(Gridcolorp, 0, plot.Height \ 2 + CType(zoomY * i * tickY, Integer), plot.Width, plot.Height \ 2 + CType(zoomY * i * tickY, Integer))
            Ytags(Ytags.Length - 2) = plot.Height \ 2 + CType(zoomY * i * tickY, Integer)
            gfx.DrawLine(Gridcolorp, 0, plot.Height \ 2 - CType(zoomY * i * tickY, Integer), plot.Width, plot.Height \ 2 - CType(zoomY * i * tickY, Integer))
            Ytags(Ytags.Length - 1) = plot.Height \ 2 - CType(zoomY * i * tickY, Integer)
            i += 1
        End While
        i = 0
        While zoomX * i * tickX < plot.Width
            Array.Resize(Xtags, Xtags.Length + 1)
            gfx.DrawLine(Gridcolorp, CType(zoomX * i * tickX, Integer), 0, CType(zoomX * i * tickX, Integer), plot.Height)
            Xtags(Xtags.Length - 2) = CType(zoomX * i * tickX, Integer)
            i += 1
        End While
        gfx.DrawLines(p, pointset) 'wykres
        gfx.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        For i = 1 To markers.Length - 1
            Dim pointxy As New PointF(markers(i) * zoomX - dispX, dataset(markers(i)) * zoomY + plot.Height \ 2 - dispY)
            gfx.FillEllipse(textcolorb, pointxy.X - 5, pointxy.Y - 5, 9, 9)
        Next
    End Sub
    Private Sub Chart_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        e.Graphics.DrawImage(plot, 79, 11)
        e.Graphics.DrawRectangle(p_framep, 78, 10, plot.Width + 1, plot.Height + 1)
        Dim sf As New StringFormat
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        For i = 2 To Ytags.Length - 1
            e.Graphics.DrawString(((Ytags(i) - plot.Height \ 2 + dispY) / zoomY) * unitmultipY, Font, textcolorb, New RectangleF(0, Ytags(i) + 3, 78, 20), sf)
        Next
        If selectionActive Then
            e.Graphics.FillRectangle(New SolidBrush(Selection_color), Math.Max(Math.Min(selectionbegin + selectionend, selectionbegin), 80), 11, Math.Max(-selectionend, selectionend), plot.Height)
        End If
        e.Graphics.RotateTransform(-90)
        e.Graphics.TranslateTransform(-plot.Height - 90, 68)
        For i = 1 To Xtags.Length - 1
            e.Graphics.DrawString(((Xtags(i) + dispX) / zoomX) * unitmultipX, Font, textcolorb, New RectangleF(0, Xtags(i) + 3, 78, 20), sf)
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
            If e.X > 80 AndAlso e.X < Width - 11 AndAlso e.Y > 10 AndAlso e.Y < Height - 79 Then
                If e.Button = MouseButtons.Left Then
                    selectionbegin = e.X
                    selectionend = 0
                    selectionInProgress = True
                    selectionActive = True
                ElseIf e.Button = MouseButtons.Right And selectionActive Then
                    selectionActive = False
                    ZoomToSelection()
                End If
            Else
                selectionActive = False
            End If
            Refresh()
        End If
    End Sub
    Private Sub ZoomToSelection()
        zoomX = (Width - 90) / (SelectEnd - SelectBegin)
        dispX = SelectBegin * zoomX
        tickX = 100 / zoomX
        RePaint(Nothing, Nothing)
    End Sub
    Public Sub AutoSacaleX()
        zoomX = (Width - 90) / dataset.Length
        dispX = 0
        tickX = 100 / zoomX
        RePaint(Nothing, Nothing)
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
                    If SelectEnd < SelectBegin Then
                        Dim tmp As Integer = SelectEnd
                        SelectEnd = SelectBegin
                        SelectBegin = tmp
                    End If

                    If SelectBegin < 0 Then SelectBegin = 0
                    If SelectEnd < 0 Then SelectEnd = 0
                    If SelectEnd > dataset.Length - 1 Then SelectEnd = dataset.Length - 1
                    If SelectBegin > dataset.Length - 1 Then SelectBegin = dataset.Length - 1
                End If
            End If
        End If
    End Sub
End Class
