using RadioReform.Collections;
using RadioReform.Elements;
using RadioReform.Load;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RadioReform.Sort;
using RadioReform.Search;
using RadioReform.Player;
using Microsoft.Win32;
using System.IO;
using RadioReform.Save;

namespace RadioReform
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string fileSongs = "songs.xml";
        private string filePlaylists = "playlists.xml";

        //public MusicCollection musicCollection;
        public PlaylistCollection playlistCollection;

        private Playlist selectedPlaylist;
        private MyPlayer myPlayer;

        public MainWindow()
        {
            InitializeComponent();

            //musicCollection = new MusicCollection();
            playlistCollection = new PlaylistCollection();

            //XmlLoader<Song> readSongs = new XmlLoader<Song>(fileSongs);
            //musicCollection.songs = readSongs.GetItems();

            XmlLoader<Playlist> readPl = new XmlLoader<Playlist>(filePlaylists);
            playlistCollection.playlists = readPl.GetItems();

            PlaylistsPanelRefresh(playlistCollection.playlists);
            selectedPlaylist = playlistCollection.playlists[0];
            PlaylistInfoShow(selectedPlaylist);

            if (playlistCollection.playlists.Count > 0)
            {
                MusicListViewRefresh(playlistCollection.playlists[0].Songs);
            }

            myPlayer = new MyPlayer();
        }

        public void PlaylistsPanelRefresh(MyCollection<Playlist> playlists)
        {
            foreach (Playlist p in playlists)
            {
                PlaylistPanelAddItem(p);
            }
        }

        public void PlaylistPanelAddItem(Playlist p)
        {
            Button button = new Button();
            button.Content = "  " + p.Title;
            button.FontSize = 16;
            button.HorizontalContentAlignment = HorizontalAlignment.Left;
            button.Height = 40;
            button.Background = Brushes.White;
            button.Click += buttonPlaylistClick;
            playlistsPanel.Children.Add(button);
        }

        public void PlaylistPanelDeleteItem(Playlist p)
        {
            foreach (Button b in playlistsPanel.Children)
            {
                if (b.Content.ToString().TrimStart() == p.Title)
                {
                    playlistsPanel.Children.Remove(b);
                    break;
                }
            }
        }

        public void buttonPlaylistClick(object sender, RoutedEventArgs e)
        {
            Playlist p = playlistCollection.FindByName((sender as Button).Content.ToString().TrimStart());
            MusicListViewRefresh(p.Songs);
            selectedPlaylist = p;
            PlaylistInfoShow(selectedPlaylist);
        }

        public void PlaylistInfoShow(Playlist p)
        {
            labelPlaylistTitle.Content = p.Title;
            try
            {
                var img = new BitmapImage(new Uri(p.PicturePath));
                buttonNewPlaylistImage.Background = new ImageBrush(img);
            }
            catch (Exception ex)
            {

            }
        }

        public void MusicListViewRefresh(MyCollection<Song> songs) // showing playlists songs
        {
            musicListView.Items.Clear();
            if (songs != null)
            {
                foreach (Song s in songs)
                {
                    musicListView.Items.Add(s);
                }
            }
            else
            {
                MessageBox.Show("");
            }

            if ((musicListView.ContextMenu.Items[1] as MenuItem).Items.Count > 0)
            {
                (musicListView.ContextMenu.Items[1] as MenuItem).Items.Clear();
            }

            foreach (Playlist p in playlistCollection.playlists)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = p.Title;
                menuItem.Click += menuItemAddToPlaylist_Click;
                if (p.Title != "All songs")
                {
                    (musicListView.ContextMenu.Items[1] as MenuItem).Items.Add(menuItem);
                }
            }
        }

        //
        //
        //sorting in listview
        int clickCount = 0;

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            bool order = (clickCount == 0) ? true : false;
            clickCount = (clickCount == 0) ? 1 : 0;

            string columnText = (e.OriginalSource as GridViewColumnHeader).Content.ToString();

            ParameterizedSorting<Song> parameterizedSorting = new ParameterizedSorting<Song>();

            Dictionary<string, IComparer<Song>> sorts = new Dictionary<string, IComparer<Song>>();
            sorts.Add("ID", new IdComparer());
            sorts.Add("Title", new TitleComparer());
            sorts.Add("Artist", new ArtistComparer());
            sorts.Add("Album", new AlbumComparer());
            sorts.Add("Year", new YearComparer());
            sorts.Add("Duration", new DurationComparer());
            sorts.Add("Plays", new PlaysComparer());

            if (playlistCollection.playlists[0] != null)
            {
                parameterizedSorting.SetParams(sorts[columnText], playlistCollection.playlists[0].Songs, order);
            }

            parameterizedSorting.Sort();
            MusicListViewRefresh(playlistCollection.playlists[0].Songs);
        }

        //
        //
        //searching in listview
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchable = textBoxSearch.Text;

            if (searchable == "")
            {
                MusicListViewRefresh(playlistCollection.playlists[0].Songs);
            }
            else
            {
                ParameterizedSearching searching = new ParameterizedSearching();
                Dictionary<string, ISearchParameter> finalSearchParams = new Dictionary<string, ISearchParameter>();

                //Dictionary<string, ISearchParameter> searchParams = new Dictionary<string, ISearchParameter>();

                finalSearchParams.Add("Title", new SearchByTitleParam(searchable));
                finalSearchParams.Add("Artist", new SearchByArtistParam(searchable));
                finalSearchParams.Add("Album", new SearchByAlbumParam(searchable));
                finalSearchParams.Add("Year", new SearchByYearParam(searchable));
                finalSearchParams.Add("Tags", new SearchByTagsParam(searchable));
                finalSearchParams.Add("Categories", new SearchByCategoriesParam(searchable));
                finalSearchParams.Add("Genres", new SearchByGenresParam(searchable));

                foreach (CheckBox item in comboBoxSearchParams.Items)
                {
                    if (item.IsChecked == false)
                    {
                        finalSearchParams.Remove(item.Content.ToString());
                    }
                }

                MusicListViewRefresh(searching.getSearchResults(finalSearchParams, playlistCollection.playlists[0].Songs));
            }
        }

        //
        //
        // work with player
        private void SongTimerTick(object sender, EventArgs e)
        {
            if (myPlayer.Timeleft > 0)
            {
                sliderTimeleft.Value = myPlayer.NowPlays.Duration - myPlayer.Timeleft;
                labelTimeleft.Content = myPlayer.Timeleft;
            }
            else
            {
                if (!myPlayer.Replay)
                {
                    myPlayer.NextSong();
                }
                else
                {
                    myPlayer.PlaySong(myPlayer.NowPlays);
                }
                sliderTimeleft.Maximum = myPlayer.NowPlays.Duration;
                labelNowPlaying.Content = myPlayer.NowPlays.Artist + " - " + myPlayer.NowPlays.Title;
                myPlayer.Timer.Tick += new EventHandler(SongTimerTick);
            }
        }

        public void PlayQueueLoading()
        {

        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlaylist.Title != "Now playing")
            {
                myPlayer.PlayQueue.Add(musicListView.SelectedItem as Song);
            }
            myPlayer.PlaySong(musicListView.SelectedItem as Song);
            sliderTimeleft.Maximum = myPlayer.NowPlays.Duration;
            labelNowPlaying.Content = myPlayer.NowPlays.Artist + " - " + myPlayer.NowPlays.Title;
            myPlayer.Timer.Tick += new EventHandler(SongTimerTick);
        }

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            myPlayer.Pause();
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            myPlayer.PreviousSong();
            sliderTimeleft.Maximum = myPlayer.NowPlays.Duration;
            labelNowPlaying.Content = myPlayer.NowPlays.Artist + " - " + myPlayer.NowPlays.Title;
            myPlayer.Timer.Tick += new EventHandler(SongTimerTick);
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            myPlayer.NextSong();
            sliderTimeleft.Maximum = myPlayer.NowPlays.Duration;
            labelNowPlaying.Content = myPlayer.NowPlays.Artist + " - " + myPlayer.NowPlays.Title;
            myPlayer.Timer.Tick += new EventHandler(SongTimerTick);
        }

        private void buttonReplay_Click(object sender, RoutedEventArgs e)
        {
            myPlayer.Replay = (myPlayer.Replay == true) ? false : true;
        }

        private void buttonNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            MusicListViewRefresh(myPlayer.PlayQueue);
            labelPlaylistTitle.Content = "Now playing";
        }

        private void sliderTimeleft_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myPlayer.ChangePosition(sliderTimeleft.Value);
        }

        //
        //
        // work with playlists
        private bool edit = false;

        private void windowDisable()
        {
            GridPlaylistInfo.Visibility = Visibility.Hidden;
            GridPlaylistAdd.Visibility = Visibility.Visible;
            playlistsPanel.IsEnabled = false;
            GridPlayer.IsEnabled = false;
            musicListView.IsEnabled = false;
            StackPanelMenu.IsEnabled = false;
            StackPanelSongsButtons.IsEnabled = false;
        }

        private void windowEnable()
        {
            GridPlaylistInfo.Visibility = Visibility.Visible;
            GridPlaylistAdd.Visibility = Visibility.Hidden;
            playlistsPanel.IsEnabled = true;
            GridPlayer.IsEnabled = true;
            musicListView.IsEnabled = true;
            StackPanelMenu.IsEnabled = true;
            StackPanelSongsButtons.IsEnabled = true;

            textBoxPlaylistTitle.Text = "";
            labelPicture.Content = "";
            buttonNewPlaylistImage = null;
        }

        private void buttonAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            windowDisable();
        }

        private void buttonPlaylistEdit_Click(object sender, RoutedEventArgs e)
        {
            edit = true;
            windowDisable();
            textBoxPlaylistTitle.Text = selectedPlaylist.Title;
            labelPicture.Content = selectedPlaylist.PicturePath;
            if (selectedPlaylist.PicturePath != "Unknown" && selectedPlaylist.PicturePath != null)
            {
                try
                {
                    var img = new BitmapImage(new Uri(selectedPlaylist.PicturePath));
                    buttonNewPlaylistImage.Background = new ImageBrush(img);
                }
                catch (Exception ex) { }
            }
        }

        private void buttonAddPlaylistsPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Изображения(*.BMP; *.JPG; *.GIF)| *.BMP; *.JPG; *.GIF | All files(*.*) | *.*";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                labelPicture.Content = myDialog.FileName;
                var img = new BitmapImage(new Uri(myDialog.FileName));
                buttonImage.Background = new ImageBrush(img);
            }
        }

        private void buttonPlaylistCancel_Click(object sender, RoutedEventArgs e)
        {
            windowEnable();
        }

        private void buttonPlaylistSave_Click(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                if (String.IsNullOrEmpty(textBoxPlaylistTitle.Text) || playlistCollection.GetNames().Contains(textBoxPlaylistTitle.Text))
                {
                    textBoxPlaylistTitle.BorderBrush = Brushes.Red;
                    return;
                }
                Playlist p = new Playlist(textBoxPlaylistTitle.Text, labelPicture.Content.ToString(), new MyCollection<Song>());
                playlistCollection.playlists.AddItem(p);
                PlaylistPanelAddItem(p);
            }
            else
            {
                if (playlistCollection.GetNames().Contains(textBoxPlaylistTitle.Text))
                {
                    if (textBoxPlaylistTitle.Text != selectedPlaylist.Title)
                    {
                        textBoxPlaylistTitle.BorderBrush = Brushes.Red;
                        return;
                    }
                }
                Playlist p = new Playlist();
                p.Title = textBoxPlaylistTitle.Text;
                try
                {
                    p.PicturePath = labelPicture.Content.ToString();
                }
                catch (Exception ex) { }
                p.Songs = selectedPlaylist.Songs;
                PlaylistPanelDeleteItem(selectedPlaylist);
                PlaylistPanelAddItem(p);
                playlistCollection.playlists.EditItem(selectedPlaylist, p);
                selectedPlaylist = p;
            }

            windowEnable();
        }

        private void buttonPlaylistDelete_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Do you want to delete \"" + selectedPlaylist.Title + "\"?";
            string caption = "Delete";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            if (result == MessageBoxResult.Yes)
            {
                if (selectedPlaylist.Title.Equals("All songs"))
                {
                    MessageBox.Show("Sorry, you can't delete this playlist");
                    return;
                }
                else
                {
                    playlistCollection.playlists.DeleteItem(playlistCollection.FindByName(selectedPlaylist.Title));
                    PlaylistPanelDeleteItem(selectedPlaylist);
                    selectedPlaylist = playlistCollection.playlists[0];
                    MusicListViewRefresh(selectedPlaylist.Songs);
                }
            }
            else
            {
                return;
            }
        }

        private void buttonSongAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Музыка(*.mp3;)|*.mp3; ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;

            string filename = "";
            if (myDialog.ShowDialog() == true)
            {
                filename = myDialog.FileName;
            }

            if (String.IsNullOrEmpty(filename))
            {
                return;
            }

            Song newSong = new Song();
            newSong = newSong.ParseFromFile(filename);

            selectedPlaylist.Songs.AddItem(newSong);
            if (selectedPlaylist.Title != "All songs")
            {
                playlistCollection.FindByName("All songs").Songs.AddItem(newSong);
            }
            MusicListViewRefresh(selectedPlaylist.Songs);
        }

        private void buttonAddFolder_Click(object sender, RoutedEventArgs e)
        {
            string[] files = new string[0];

            System.Windows.Forms.FolderBrowserDialog FBD = new System.Windows.Forms.FolderBrowserDialog();
            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                files = Directory.GetFiles(FBD.SelectedPath);
            }

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    if (s.EndsWith(".mp3"))
                    {
                        Song newSong = new Song();
                        newSong = newSong.ParseFromFile(s);

                        selectedPlaylist.Songs.AddItem(newSong);
                        if (selectedPlaylist.Title != "All songs")
                        {
                            playlistCollection.FindByName("All songs").Songs.AddItem(newSong);
                        }
                        MusicListViewRefresh(selectedPlaylist.Songs);
                    }
                }
                MusicListViewRefresh(selectedPlaylist.Songs);
            }
        }

        private void buttonSongEdit_Click(object sender, RoutedEventArgs e)
        {
            if (musicListView.SelectedItem != null)
            {
                windowDisable();
                GridPlaylistAdd.Visibility = Visibility.Hidden;
                addEditBlock.Visibility = Visibility.Visible;

                Song selectedSong = (musicListView.SelectedItem as Song);

                textBox.Text = selectedSong.Title;
                textBox1.Text = selectedSong.Artist;
                textBox2.Text = selectedSong.Album;
                textBox3.Text = selectedSong.Year.ToString();
                textBox5.Text = selectedSong.Raiting.ToString();
                foreach (string s in selectedSong.Genres)
                {
                    textBox7.Text += s + ", ";
                }
                foreach (string s in selectedSong.Categories)
                {
                    textBox7.Text += s + ", ";
                }
                foreach (string s in selectedSong.Tags)
                {
                    textBox7.Text += s + ", ";
                }
                textBox10.Text = selectedSong.ID.ToString();
                label14.Content = selectedSong.Filename;
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            label14.Content = "";

            addEditBlock.Visibility = Visibility.Hidden;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Song selectedSong = (musicListView.SelectedItem as Song);
            Song song = new Song();

            song.Title = textBox.Text;
            song.Artist = textBox1.Text;
            song.Album = textBox2.Text;
            song.Year = Convert.ToInt32(textBox3.Text);
            song.Raiting = Convert.ToDouble(textBox5.Text);
            song.Tags = song.ParseFromAddEditBlock(textBox9.Text);
            song.Categories = song.ParseFromAddEditBlock(textBox8.Text);
            song.Genres = song.ParseFromAddEditBlock(textBox7.Text);
            song.ID = Convert.ToInt32(textBox10.Text);
            song.Filename = label14.Content.ToString();
            song.NumOfPlays = selectedSong.NumOfPlays;
            song.Duration = selectedSong.Duration;

            selectedPlaylist.Songs.EditItem(selectedSong, song);
            addEditBlock.Visibility = Visibility.Hidden;
            MusicListViewRefresh(selectedPlaylist.Songs);
            windowEnable();
        }

        private void buttonFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Музыка(*.mp3;)|*.mp3; ";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                label14.Content = myDialog.FileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Do you want to exit?";
            string caption = "Exit";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            XmlSaver<Playlist> writePl = new XmlSaver<Playlist>(filePlaylists, playlistCollection.playlists);
            writePl.Write();

            MessageBox.Show("Saved");
        }

        private void buttonSongDelete_Click(object sender, RoutedEventArgs e)
        {
            Song song = musicListView.SelectedItem as Song;
            string messageBoxText = "Do you want to delete \"" + song.Title + "\"?";
            string caption = "Delete";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (result == MessageBoxResult.Yes)
            {
                if (selectedPlaylist.Title.Equals("All songs"))
                {
                    foreach (Playlist pl in playlistCollection.playlists)
                    {
                        pl.Songs.DeleteItem(playlistCollection.FindSongByName(song.Title, pl));
                    }
                }
                else
                {
                    selectedPlaylist.Songs.DeleteItem(song);
                }
            }
            MusicListViewRefresh(selectedPlaylist.Songs);
        }

        private void toPlayQueue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myPlayer.PlayQueue.Add(musicListView.SelectedItem as Song);
            }
            catch (Exception ex) { }
        }

        public void menuItemAddToPlaylist_Click(object sender, RoutedEventArgs e) // add song to some playlist (CONTEXT MENU!)
        {
            if (musicListView.SelectedItem != null)
            {
                playlistCollection.FindByName((sender as MenuItem).Header.ToString()).Songs.AddItem(
                    musicListView.SelectedItem as Song);
            }
        }
    }
}
