Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json


Public Class Form2
    Private ReadOnly client As HttpClient
    Private currentIndex As Integer = 0

    Public Sub New(httpClient As HttpClient)
        ' This call is required by the designer.
        InitializeComponent()

        Me.client = httpClient
        LoadSubmission(currentIndex)
    End Sub

    Private Async Sub LoadSubmission(index As Integer)
        Dim response As HttpResponseMessage = Await client.GetAsync($"read?index={index}")
        If response.IsSuccessStatusCode Then
            Dim responseBody As String = Await response.Content.ReadAsStringAsync()
            Dim submission As Submission = JsonConvert.DeserializeObject(Of Submission)(responseBody)
            If submission IsNot Nothing Then
                DisplaySubmission(submission)
            Else
                MessageBox.Show("No more submissions.")
            End If
        Else
            MessageBox.Show("Failed to fetch submission.")
        End If
    End Sub
    Public Class Submission
        Public Property Name As String
        Public Property Email As String
        Public Property Phone As String
        Public Property GithubLink As String
        Public Property StopwatchTime As String
    End Class

    Private Sub DisplaySubmission(submission As Submission)
        TextBox1.Text = submission.Name
        TextBox2.Text = submission.Email
        TextBox3.Text = submission.Phone
        TextBox4.Text = submission.GithubLink
        TextBox5.Text = submission.StopwatchTime
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If currentIndex > 0 Then
            currentIndex -= 1
            LoadSubmission(currentIndex)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        currentIndex += 1
        LoadSubmission(currentIndex)
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If currentIndex >= 0 Then
            Dim response As HttpResponseMessage = Await client.DeleteAsync($"delete?index={currentIndex}")
            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission deleted successfully.")
                ClearFormFields()
                LoadSubmission(currentIndex)
            Else
                MessageBox.Show("Failed to delete submission.")
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Enable editing mode
        EnableEditMode()
    End Sub

    Private Sub EnableEditMode()
        ' Enable editing of text boxes
        TextBox1.ReadOnly = False
        TextBox2.ReadOnly = False
        TextBox3.ReadOnly = False
        TextBox4.ReadOnly = False
        TextBox5.ReadOnly = False

        ' Change button texts and visibility
        Button4.Visible = False
        Button5.Visible = True
        Button6.Visible = True
    End Sub

    Private Sub DisableEditMode()
        ' Disable editing of text boxes
        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True
        TextBox5.ReadOnly = True

        ' Change button texts and visibility
        Button4.Visible = True
        Button5.Visible = False
        Button6.Visible = False
    End Sub

    Private Async Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' Save changes
        Dim submissionToUpdate As New Submission() With {
            .Name = TextBox1.Text,
            .Email = TextBox2.Text,
            .Phone = TextBox3.Text,
            .GithubLink = TextBox4.Text,
            .StopwatchTime = TextBox5.Text
        }

        Dim json As String = JsonConvert.SerializeObject(submissionToUpdate)
        Dim content As New StringContent(json, Encoding.UTF8, "application/json")
        Dim response As HttpResponseMessage = Await client.PutAsync($"update?index={currentIndex}", content)

        If response.IsSuccessStatusCode Then
            MessageBox.Show("Changes saved successfully.")
            DisableEditMode()
            LoadSubmission(currentIndex)
        Else
            MessageBox.Show("Failed to save changes.")
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ' Cancel editing
        DisableEditMode()
        LoadSubmission(currentIndex)
    End Sub

    Private Sub ClearFormFields()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
    End Sub

    Private Sub Form2_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Handle keyboard shortcuts
        If e.Control AndAlso e.KeyCode = Keys.P Then
            Button1_Click(sender, e)
        End If

        If e.Control AndAlso e.KeyCode = Keys.N Then
            Button2_Click(sender, e)
        End If

        If e.Control AndAlso e.KeyCode = Keys.D Then
            Button3_Click(sender, e)
        End If

        If e.Control AndAlso e.KeyCode = Keys.E Then
            Button4_Click(sender, e)
        End If

        If e.Control AndAlso e.KeyCode = Keys.S AndAlso Button5.Visible Then
            Button5_Click(sender, e)
        End If

        If e.Control AndAlso e.KeyCode = Keys.Escape AndAlso Button6.Visible Then
            Button6_Click(sender, e)
        End If
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set up initial form state
        Me.KeyPreview = True
        DisableEditMode()
    End Sub
End Class
