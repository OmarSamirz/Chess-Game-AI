using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Classes
{
    public static class Options
    {
        private static bool music = false;
        private static bool sounds = true;
        private static string difficulity = "Medium";
        private static SoundPlayer movePiecePlayer = new SoundPlayer(@"D:\College Years\Year - 02\Semester - 02\Algorithms Design and Analysis\Project\Chess Final Project\Chess\Chess\Sounds\MovePiece.wav");
        private static SoundPlayer buttonClickPlayer = new SoundPlayer(@"D:\College Years\Year - 02\Semester - 02\Algorithms Design and Analysis\Project\Chess Final Project\Chess\Chess\Sounds\ButtonClick.wav");
        private static SoundPlayer musicPlayer = new SoundPlayer(@"D:\College Years\Year - 02\Semester - 02\Algorithms Design and Analysis\Project\Chess Final Project\Chess\Chess\Sounds\Music.wav");


        public static bool Music { get { return music; } set { music = value; } }
        public static bool Sounds { get { return sounds; } set { sounds = value; } }
        public static string Difficulity { get {  return difficulity; } set {  difficulity = value; } }
        public static SoundPlayer MovePiecePlayer { get {  return movePiecePlayer; } }
        public static SoundPlayer ButtonClickPlayer { get { return  buttonClickPlayer; } }
        public static SoundPlayer MusicPlayer { get { return musicPlayer; } }


        public static void MovePiecePlay(bool IsAI)
        {
            if (!sounds && IsAI)
                return;

            MovePiecePlayer.Load();
            MovePiecePlayer.PlaySync();
        }

        public static void ButtonClickPlay()
        {
            if (!sounds)
                return;

            ButtonClickPlayer.Load();
            ButtonClickPlayer.Play();
        }

        public static void MusicPlay()
        {
            if (!music)
                return;

            MusicPlayer.Load();
            MusicPlayer.Play();
        }
    }
}
