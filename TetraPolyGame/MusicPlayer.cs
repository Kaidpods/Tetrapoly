using System.Windows.Media;

///<summary>
///This video game is being created for a College Assessment
///</summary>
///<para>Author: Byte Brothers (Peter Brammer)</para>
///<para>Date Created: 15/05/2024</para>
///<para>Version 1</para>

namespace TetraPolyGame
{

    /// <summary>
    /// Class for a media player
    /// Author: Peter Brammer
    /// </summary>
    public class MusicPlayer
    {
        private MediaPlayer? musicPlayer;
        public void MPlayer(string localPath)
        {
            var uri = new Uri(localPath, UriKind.RelativeOrAbsolute);
            musicPlayer = new MediaPlayer();
            musicPlayer.Open(uri);
            musicPlayer.Volume = 0.4;
            //attach a mediaended event
            musicPlayer.MediaEnded += OnMediaEnded;
            musicPlayer.Play();
        }

        /// <summary>
        /// Event to restart the music
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMediaEnded(object? sender, EventArgs e)
        {
            musicPlayer.Position = TimeSpan.Zero;
            musicPlayer.Play();
        }

        /// <summary>
        /// Stops the music
        /// </summary>
        public void MusicStop()
        {
            musicPlayer.Stop();
        }

    }

}