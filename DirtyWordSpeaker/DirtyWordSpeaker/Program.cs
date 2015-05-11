using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirtyWordSpeaker
{
    class Program
    {
        public static void Main(string[] args)
        {
            Stream introStream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("DirtyWordSpeaker.Metadata.Intro.wav");

            SoundPlayer introPlayer = new SoundPlayer(introStream);

            List<SoundPlayer> files = LoadMediaFiles();

            introPlayer.Load();

            introPlayer.Play();

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Dirty Word Speaker - A good piece of software by Fat Man.");

            Console.WriteLine();

            Console.WriteLine("Aughh yea folks! Its time to listen to some dirty words");

            Console.WriteLine("Press any key to hear me speak some dirty words...");

            Console.ReadKey();

            Console.WriteLine();

            Random generator = new Random();

            while (true)
            {
                int nextIndex = generator.Next(0, files.Count);

                SoundPlayer word = files[nextIndex];

                Console.WriteLine(word.Tag);

                word.Play();

                Thread.Sleep(2200);
            }

        }

        private static List<SoundPlayer> LoadMediaFiles()
        {
            List<SoundPlayer> players = new List<SoundPlayer>();

            List<string> dirtyWords = new List<string>
            {
                "Ass.wav", "Bastard.wav", "Bitch.wav", "Cocksucker.wav", "Cunt.wav", "Damn.wav", "Fuck.wav", "Motherfucker.wav", "Penis.wav", "Piss.wav", "Shit.wav", "Tits.wav", "Vagina.wav"
            };

            foreach (string dirtyWord in dirtyWords)
            {
                Stream wordStream =
               Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("DirtyWordSpeaker.Metadata.{0}", dirtyWord));

                SoundPlayer player = new SoundPlayer(wordStream)
                {
                    Tag = dirtyWord.Replace(".wav", string.Empty)
                };

                player.Load();

                players.Add(player);
            }

            return players;
        }
    }
}
