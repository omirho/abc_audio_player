' Class Form1 which is displayed on the start of the application.
' Contains functions: Form1_Load
'                     Play()
'                     NextTrack()
'                     PreviousTrack()
'                     Button[1:6]_Click
'                     TrackBar[1:3]_Scroll
'                     Timer1_Tick
'                     ListBox2_Click

Public Class Form1

    ' Play Function
    ' Plays the selected item in the listbox1(listbox2)
    ' Called by other functions: NextTrack(), PreviousTrack(), Button1_Click, ListBox2_DoubleClick

    Private Sub Play()
        If AxWindowsMediaPlayer1.playState = WMPLib.WMPPlayState.wmppsPaused Then   ' Check if the current playstate is paused
            AxWindowsMediaPlayer1.Ctlcontrols.play()                                ' If paused, play the paused item
        Else
            AxWindowsMediaPlayer1.URL = ListBox1.SelectedItem                       ' If not paused, play the selected item in listbox1(listbox2)
        End If
        Button1.Text = "Pause"                                                      ' Change the text of the play button when the function is invoked.
    End Sub

    ' NextTrack Function
    ' Plays the next item in the listbox1(listbox2)
    ' Called by other functions: Timer1_Tick, Button3_Click

    Private Sub NextTrack()
        Try
            Dim s = ListBox1.SelectedIndex                  ' Take the selected index of the hidden listbox
            Dim f = ListBox2.SelectedIndex                  ' Take the selected index of the visible listbox
            If s <> -1 Then                                 ' Check if any item is selected
                ListBox1.SelectedIndex = s + 1              ' If some item is selected, select the next item
                ListBox2.SelectedIndex = f + 1
                Play()                                      ' Play the selected item
            End If                                          ' Don't do anything if no item is selected
        Catch ex As Exception                               ' If above operations fail, put the player in the start state
            AxWindowsMediaPlayer1.URL = ""
            ListBox2.SelectedIndex = -1
            Button1.Text = "Play"
        End Try
    End Sub

    Private Sub PreviousTrack()
        Try
            Dim s = ListBox1.SelectedIndex
            Dim f = ListBox2.SelectedIndex
            ListBox1.SelectedIndex = s - 1
            ListBox2.SelectedIndex = f - 1
            Play()
        Catch ex As Exception
            AxWindowsMediaPlayer1.URL = ""
            ListBox2.SelectedIndex = -1
            Button1.Text = "Play"
        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim open As New OpenFileDialog
        Try
            open.Title = "Open Music"
            open.FileName = ""
            open.Multiselect = True
            open.Filter = "Audio Files (*.mp3, *.wav, *.aac, *.amr, *.m4a, *.ogg, *.wma)|*.mp3;*.wav;*.aac;*.amr;*.m4a;*.ogg;*.wma|All Files (*.*)|*.*"
            If open.ShowDialog = Windows.Forms.DialogResult.OK Then
                For Each track As String In open.FileNames
                    ListBox1.Items.Add(track)
                Next
                For Each trackname As String In open.SafeFileNames
                    ListBox2.Items.Add(trackname)
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            TrackBar3.Maximum = AxWindowsMediaPlayer1.currentMedia.duration
            TrackBar3.Value = AxWindowsMediaPlayer1.Ctlcontrols.currentPosition
        Catch ex As Exception

        End Try
        ListBox1.SelectedIndex = ListBox2.SelectedIndex
        If AxWindowsMediaPlayer1.playState = WMPLib.WMPPlayState.wmppsStopped Then
            Try
                NextTrack()
            Catch ex As Exception
                AxWindowsMediaPlayer1.URL = ""
                ListBox2.SelectedIndex = -1
            End Try
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button1.Text = "Play" Then
            If ListBox2.SelectedIndex <> -1 Then
                Button1.Text = "Pause"
                Play()
            End If
        ElseIf Button1.Text = "Pause" Then
            Button1.Text = "Play"
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        AxWindowsMediaPlayer1.URL = ""
        ListBox2.SelectedIndex = -1
        Button1.Text = "Play"
    End Sub

    ' Button3_Click function
    ' Invokes the NextTrack() function
    ' Triggered by the click of Next Button

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        NextTrack()
    End Sub

    ' Button4_Click function
    ' Invokes the PreviousTrack() function
    ' Triggered by the click of Previous Button

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        PreviousTrack()
    End Sub

    ' TrackBar1_Scroll function
    ' Controls the volume of the playback
    ' Triggered by the scolling of the volume bar

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        AxWindowsMediaPlayer1.settings.volume = TrackBar1.Value                     ' Set the volume of the windows media player element to the value of the trackbar
    End Sub

    ' TrackBar2_Scroll function
    ' Controls the speaker balance of the playback
    ' Triggered by the scolling of the balance bar

    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
        AxWindowsMediaPlayer1.settings.balance = TrackBar2.Value                ' Set the balance of the windows media player element to the value of the trackbar
    End Sub

    ' Form1_Load function
    ' Initializes the volume and balance trackbars with the system volume and balance respectively

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TrackBar1.Value = AxWindowsMediaPlayer1.settings.volume                     ' Initialize volume
        TrackBar2.Value = AxWindowsMediaPlayer1.settings.balance                    ' Initialize balance
    End Sub

    ' Button6_Click function
    ' Sets the speaker balance to default i.e equal on both sides ie 0

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        TrackBar2.Value = 0
        AxWindowsMediaPlayer1.settings.balance = 0
    End Sub

    ' TrackBar3_Scroll function
    ' Controls the seek of the playback(progress of the playback(seekbar))
    ' Triggered by the scolling of the seek bar

    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
        AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = TrackBar3.Value
    End Sub

    Private Sub ListBox2_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox2.DoubleClick
        ListBox1.SelectedIndex = ListBox2.SelectedIndex
        Try
            If AxWindowsMediaPlayer1.URL = "" And ListBox2.SelectedIndex <> -1 Then
                Play()
            ElseIf ListBox1.SelectedItem.ToString <> AxWindowsMediaPlayer1.URL Then
                AxWindowsMediaPlayer1.URL = ""
                Play()
            End If
        Catch
        End Try
    End Sub

End Class
