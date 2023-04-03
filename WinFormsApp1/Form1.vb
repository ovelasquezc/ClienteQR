Imports System.IO
Imports iText.IO.Font.Constants
Imports iText.IO.Image
Imports iText.Kernel.Colors
Imports iText.Kernel.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports iText.Kernel.Pdf.Canvas

Imports iText.Layout
Imports iText.Layout.Element
Imports iText.Layout.Properties
Imports QRCoder
Imports System.Drawing
Imports System.Drawing.Imaging
Imports Image = iText.Layout.Element.Image

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnAddText_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "PDF files (*.pdf)|*.pdf"
        openFileDialog.Title = "Select a PDF File"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Dim inputFile As String = openFileDialog.FileName
            Dim outputFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(inputFile), "output.pdf")
            '            Dim text As String = "Text to add to every page"
            Dim text As String = TextBox1.Text

            AddTextToPDF(inputFile, outputFile, text)

            MessageBox.Show("Text added to every page!")
        End If
    End Sub

    Public Sub AddTextToPDF(ByVal inputFile As String, ByVal outputFile As String, ByVal text As String)
        Dim reader As New PdfReader(inputFile)
        Dim writer As New PdfWriter(outputFile)
        Dim pdfDoc As New PdfDocument(reader, writer)
        Dim document As New Document(pdfDoc)
        Dim pageCount As Integer = pdfDoc.GetNumberOfPages()
        Dim pageSize As PageSize = pdfDoc.GetDefaultPageSize()
        Dim font As PdfFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA)
        Dim fontSize As Single = 12
        Dim qrCodeWidth As Integer = 50
        Dim qrCodeHeight As Integer = 50
        Dim qrCodeMargin As Integer = 10

        For i As Integer = 1 To pageCount
            Dim page As PdfPage = pdfDoc.GetPage(i)
            Dim canvas As PdfCanvas = New PdfCanvas(page)
            Dim textWidth As Single = font.GetWidth(text, fontSize)
            ' Add text to page
            canvas.BeginText()
            canvas.SetFontAndSize(font, fontSize)
            canvas.MoveText(pageSize.GetLeft() + 50, pageSize.GetTop() - 30)
            canvas.ShowText(text)
            canvas.EndText()

            ' Add QR code to page
            '            Dim qrGenerator As New QRCodeGenerator()
            '            Dim qrCodeData As QRCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q)
            '           Dim qrCode As New QRCode(qrCodeData)
            '          Dim qrCodeBitmap As Bitmap = qrCode.GetGraphic(qrCodeWidth, System.Drawing.Color.Black, System.Drawing.Color.White)
            '         Dim qrCodeByteArray As Byte() = ImageToByteArray(qrCodeBitmap, ImageFormat.Png)
            '        Dim qrCodeImage As System.Drawing.Image = New System.Drawing.Image(ImageDataFactory.Create(qrCodeByteArray)).SetFixedPosition(pageSize.GetLeft() + pageSize.GetWidth() - qrCodeMargin - qrCodeWidth, pageSize.GetBottom() + qrCodeMargin)
            '       document.Add(qrCodeImage)
            Dim gen As New QRCodeGenerator
            Dim data = gen.CreateQrCode("https://documentosdirce.uni.edu.pe/docid=" & text, QRCodeGenerator.ECCLevel.Q)
            Dim code As New QRCode(data)
            PictureBox1.Image = code.GetGraphic(6)

            Dim CodeBitmap As Bitmap = code.GetGraphic(qrCodeWidth, System.Drawing.Color.Black, System.Drawing.Color.White, False)
            'Dim CodeBitmap As Bitmap = code.GetGraphic(20)
            Dim CodeByteArray As Byte() = ImageToByteArray(CodeBitmap, ImageFormat.Png)
            Dim CodeImage As Image = New Image(ImageDataFactory.Create(CodeByteArray)).SetFixedPosition(pageSize.GetLeft() + pageSize.GetWidth() - qrCodeMargin - qrCodeWidth, pageSize.GetBottom() + qrCodeMargin).SetHeight(qrCodeHeight)


            '          Dim pic As iText.Layout.Element.Image

            'pic = PictureBox1.Image
            'pic.SetFixedPosition(pageSize.GetLeft() + pageSize.GetWidth() - qrCodeMargin - qrCodeWidth, pageSize.GetBottom() + qrCodeMargin)
            document.Add(CodeImage)


        Next

        document.Close()
    End Sub


    Private Function ImageToByteArray(ByVal image As System.Drawing.Image, ByVal format As ImageFormat) As Byte()
        Using ms As New MemoryStream()
            image.Save(ms, format)
            Return ms.ToArray()
        End Using
    End Function


End Class
