using System;
using System.Diagnostics;
using System.IO;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures;
using BadgerBadgerBadger.Properties;

namespace BadgerBadgerBadger
{
    public delegate void BadgerPlayerEventHandler(BadgerPlayerInterface sender, EventType type);

    public enum EventType
    {
        Start,
        Pause,
        Stop
    }

    public class BadgerPlayerInterface : IDisposable
    {
        private static string _BadgerLibDir;
        private VlcMediaPlayer BadgerPlayer;

        public event BadgerPlayerEventHandler OnBadgerStarted = delegate { };
        public event BadgerPlayerEventHandler OnBadgerPaused = delegate { };
        public event BadgerPlayerEventHandler OnBadgerStopped = delegate { };
        public event BadgerPlayerEventHandler OnBadgerEnded = delegate { };

        /// <summary>
        /// Extracts the VLC library
        /// </summary>
        public static void ExtractLib()
        {
            if (!Directory.Exists(_BadgerLibDir))
            {
                using (var MS = new MemoryStream(Resources.vlc, false))
                {
                    apak.APak.Unpack(MS, _BadgerLibDir);
                }
            }
        }

        /// <summary>
        /// Kills the library directory
        /// </summary>
        public static void KillLib()
        {
            if (Directory.Exists(_BadgerLibDir))
            {
                Directory.Delete(_BadgerLibDir, true);
            }
        }

        /// <summary>
        /// Static initializer
        /// </summary>
        static BadgerPlayerInterface()
        {
            using (var P = Process.GetCurrentProcess())
            {
                _BadgerLibDir = Path.Combine(Path.GetDirectoryName(P.MainModule.FileName), "lib");
            }
        }

        /// <summary>
        /// Gets the media file name
        /// </summary>
        public string BadgerFileName
        { get; private set; }

        /// <summary>
        /// Gets or sets the current media position as seconds
        /// </summary>
        public float BadgerPosition
        {
            get
            {
                return BadgerPlayer.Position * (BadgerLength / 1000);
            }
            set
            {
                BadgerPlayer.Position = value * 1000 / BadgerLength;
            }
        }

        /// <summary>
        /// Gets or sets the current media position as a percentage
        /// </summary>
        public float BadgerPositionPercentage
        {
            get
            {
                return BadgerPlayer.Position;
            }
            set
            {
                BadgerPlayer.Position = value;
            }
        }

        /// <summary>
        /// Gets the media length in Milliseconds
        /// </summary>
        public long BadgerLength
        {
            get
            {
                return BadgerPlayer.Length;
            }
        }

        /// <summary>
        /// Gets if the media is paused
        /// </summary>
        public bool BadgerPaused
        {
            get
            {
                return BadgerPlayer.State == MediaStates.Paused;
            }
        }

        /// <summary>
        /// Gets or sets the current volume
        /// </summary>
        public int BadgerVolume
        {
            get
            {
                return BadgerPlayer.Audio.Volume;
            }
            set
            {
                BadgerPlayer.Audio.Volume = value;
            }
        }

        /// <summary>
        /// Initializes a new VLC player
        /// </summary>
        /// <param name="FileName">Media file</param>
        public BadgerPlayerInterface(string FileName)
        {
            this.BadgerFileName = FileName;

            if (!Directory.Exists(_BadgerLibDir))
            {
                throw new InvalidOperationException($"VLC library not installed at {_BadgerLibDir}");
            }
            BadgerPlayerInit();
        }

        private void BadgerPlayerInit()
        {
            //The player is architecture specific
            BadgerPlayer = new VlcMediaPlayer(new DirectoryInfo(Path.Combine(_BadgerLibDir, IntPtr.Size == 4 ? "x86" : "x64")));
            BadgerPlayer.Stopped += delegate
            {
                OnBadgerStopped(this, EventType.Stop);
            };
            BadgerPlayer.Paused += delegate
            {
                OnBadgerPaused(this, EventType.Pause);
            };
            BadgerPlayer.EndReached += delegate
            {
                OnBadgerEnded(this, EventType.Stop);
            };
            BadgerPlayer.SetMedia(new FileInfo(BadgerFileName));
        }

        public void Dispose()
        {
            if (BadgerPlayer != null)
            {
                BadgerStop();
                BadgerPlayer.Dispose();
                BadgerPlayer = null;
            }
        }

        /// <summary>
        /// Stops the current media
        /// </summary>
        public void BadgerStop()
        {
            if (BadgerPlayer.State != MediaStates.Stopped)
            {
                BadgerPlayer.Stop();
            }
        }

        /// <summary>
        /// Plays the current media
        /// </summary>
        public void BadgerPlay()
        {
            if (BadgerPlayer.State == MediaStates.Ended)
            {
                var V = BadgerVolume;
                BadgerPlayerInit();
                BadgerVolume = V;
                BadgerPlayer.Play();
            }
            else if (BadgerPlayer.State != MediaStates.Playing)
            {
                BadgerPlayer.Play();
                OnBadgerStarted(this, EventType.Start);
            }

        }

        /// <summary>
        /// Pauses/Unpauses the current media
        /// </summary>
        public void BadgerPause()
        {
            if (BadgerPlayer.State == MediaStates.Paused)
            {
                BadgerPlayer.Play();
            }
            else
            {
                BadgerPlayer.Pause();
            }
        }

        /// <summary>
        /// Seeks the current media relative to the current position
        /// </summary>
        /// <param name="Offset">Position offset in seconds</param>
        public void BadgerSeek(float Offset)
        {
            BadgerPosition += Offset;
        }
    }
}
