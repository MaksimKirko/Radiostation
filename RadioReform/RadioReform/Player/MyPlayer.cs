using RadioReform.Collections;
using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace RadioReform.Player
{
    class MyPlayer
    {
        private MediaPlayer player = new MediaPlayer(); // system player 
        private System.Windows.Threading.DispatcherTimer timer; // system timer

        private MyCollection<Song> playQueue;
        private Song nowPlays; // song that plays right now
        private int timeleft = 0; // song time
        private bool isPaused = false; // paused flag
        private bool replay = false;        

        public MediaPlayer Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                isPaused = value;
            }
        }

        public bool Replay
        {
            get
            {
                return replay;
            }
            set
            {
                replay = value;
            }
        }

        public System.Windows.Threading.DispatcherTimer Timer
        {
            get
            {
                return timer;
            }
            set
            {
                timer = value;
            }
        }

        public int Timeleft
        {
            get
            {
                return timeleft;
            }
            set
            {
                timeleft = value;
            }
        }

        public Song NowPlays
        {
            get
            {
                return nowPlays;
            }
            set
            {
                nowPlays = value;
            }
        }

        public MyCollection<Song> PlayQueue
        {
            get
            {
                return playQueue;
            }
            set
            {
                playQueue = value;
            }
        }

        public MyPlayer()
        {
            player = new MediaPlayer();            
            timer = new System.Windows.Threading.DispatcherTimer();
            timeleft = 0;
            replay = false;
            isPaused = false;
            playQueue = new MyCollection<Song>();
        }

        public void PlaySong(Song selectedSong)
        {
            if (isPaused && selectedSong.Filename == nowPlays.Filename)
            {
                player.Play();
                isPaused = false;
                timer.Start();
            }
            else
            {
                nowPlays = selectedSong;
                if (File.Exists(nowPlays.Filename))
                {                    
                    player.Open(new Uri(nowPlays.Filename, UriKind.Relative));
                    player.Volume = 1;
                    player.Play();
                    timeleft = nowPlays.Duration;

                    if(timer != null)
                    {
                        timer.Stop();
                    }
                    timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 1);
                    timer.Tick += TimerTick;
                    timer.Start();
                }
            }
        }

        public void Pause()
        {
            timer.Stop();
            isPaused = true;
            player.Pause();
        }

        public void NextSong()
        {
            int npIndex = playQueue.IndexOf(nowPlays);
            if (npIndex < playQueue.Count - 1)
            {
                npIndex++;
            }
            else
            {
                npIndex = 0;
            }
            nowPlays = playQueue[npIndex];
            PlaySong(nowPlays);
        }

        public void PreviousSong()
        {
            int npIndex = playQueue.IndexOf(nowPlays);
            if (npIndex > 0)
            {
                npIndex--;
            }
            else
            {
                npIndex = playQueue.Count - 1;
            }
            nowPlays = playQueue[npIndex];
            PlaySong(nowPlays);
        }

        public void TimerTick(object sender, EventArgs e)
        {
            if (timeleft > 0)
            {
                timeleft -= 1;
            }
            else
            {
                timer.Stop();
            }
        }

        public void ChangePosition(double value)
        {
            try
            {
                timeleft = nowPlays.Duration - Convert.ToInt32(value);
                if (timeleft <= 0)
                {
                    timer.Stop();
                }
                player.Position = new TimeSpan(0, 0, (nowPlays.Duration - timeleft));
            }
            catch (Exception ex) { }
        }
    }
}
