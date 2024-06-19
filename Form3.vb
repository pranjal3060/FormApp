Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports System.Diagnostics

Public Class Form3

    Private ReadOnly client As HttpClient
    Private stopwatch As Stopwatch
    Private stopwatchRunning As Boolean = False
    Private WithEvents timer1 As New Timer()

    ' Constructor to accept HttpClient
    Public Sub New(httpClient As HttpClient)
        InitializeComponent()
        Me.client = httpClient

        ' Initialize Stopwatch
        stopwatch = New Stopwatch()

        ' Set up Timer1
        timer1.Interval = 1000 ' Interval in milliseconds (e.g., update every second)
    End Sub
    Public Class Submission
        Public Property Name As String
        Public Property Email As String
        Public Property Phone As String
        Public Property GithubLink As String
        Public Property StopwatchTime As String
    End Class
    ' Handle Submit button click
    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim submission As New Submission() With {
            .Name = TextBox1.Text,
            .Email = TextBox2.Text,
            .Phone = TextBox3.Text,
            .GithubLink = TextBox4.Text,
            .StopwatchTime = stopwatch.Elapsed.ToString("hh\:mm\:ss")
        }

        Dim json As String = JsonConvert.SerializeObject(submission)
        Dim content As New StringContent(json, Encoding.UTF8, "application/json")
        Dim response As HttpResponseMessage = Await client.PostAsync("submit", content)

        If response.IsSuccessStatusCode Then
            MessageBox.Show("Form submitted successfully.")
        Else
            MessageBox.Show("Failed to submit form.")
        End If
    End Sub

    ' Handle Toggle Stopwatch button click
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If stopwatchRunning Then
            ' Stop the stopwatch
            stopwatch.Stop()
            timer1.Stop() ' Stop timer1
            stopwatchRunning = False
            Button1.Text = "Start Stopwatch"
        Else
            ' Start the stopwatch
            stopwatch.Start()
            timer1.Start() ' Start timer1
            stopwatchRunning = True
            Button1.Text = "Stop Stopwatch"
        End If
    End Sub

    ' Timer1 Tick event handler to update UI with elapsed time
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles timer1.Tick
        TextBox4.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    ' KeyDown event for handling shortcuts
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        If e.Control AndAlso e.KeyCode = Keys.T Then
            Button2_Click(sender, e)
        End If


        If e.Control AndAlso e.KeyCode = Keys.S Then
            Button2_Click(sender, e)
        End If
    End Sub

    ' Form Load event to set up key previews for shortcuts
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub


End Class
