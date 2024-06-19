Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json

Public Class Form1

    Private ReadOnly client As HttpClient

    ' Constructor for Form1
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        client = New HttpClient()
        client.BaseAddress = New Uri("http://localhost:3000/api/")
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
    End Sub

    ' Button click event to open the ViewSubmission form
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim viewForm As New Form2(client)
        viewForm.ShowDialog()
    End Sub

    ' Button click event to open the CreateSubmission form
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim createForm As New Form3(client)
        createForm.ShowDialog()
    End Sub

    ' KeyDown event for handling shortcuts
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Shortcut for View Submissions (Ctrl + V)
        If e.Control AndAlso e.KeyCode = Keys.V Then
            Button1_Click(sender, e)
        End If

        ' Shortcut for Create New Submission (Ctrl + N)
        If e.Control AndAlso e.KeyCode = Keys.N Then
            Button2_Click(sender, e)
        End If
    End Sub

    ' Form Load event to set up key previews for shortcuts
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub

End Class
