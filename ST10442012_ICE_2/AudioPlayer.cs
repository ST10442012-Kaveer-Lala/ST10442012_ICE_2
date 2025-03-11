/*
 * 
 *AudioPlayer Class:
 * This class is responsible for managing the playback of WAV audio files.
 * It allows the user to select a song, play, pause, unpause, and stop the playback.
 * The class uses the SoundPlayer class to play the audio files, and a separate thread 
 * for managing the playback process so that it doesn't block the main thread. 
 *
 * 
 * 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ST10442012_ICE_2
{
    class AudioPlayer
    {
        // File paths for the audio files
        private string[] wavFiles = { "C:/Users/User/source/repos/ST10442012_ICE_2/ST10442012_ICE_2/Audio/Timeless.wav",
                              "C:/Users/User/source/repos/ST10442012_ICE_2/ST10442012_ICE_2/Audio/Godsplan.wav",
                              "C:/Users/User/source/repos/ST10442012_ICE_2/ST10442012_ICE_2/Audio/Again.wav" };

        private string[] wavFileNames = { "Timeless.wav", "Godsplan.wav", "Again.wav" };
        private bool[] played = { false, false, false };

        private SoundPlayer player;
        private bool isPlaying = false;
        private bool isPaused = false;
        private int currentIndex = -1; // Track the current song
        private Thread playThread; // Declare the playback thread

        public void ShowMenu()
        {
            // Infinite loop to display until user selects an option to exit
            while (true)
            {
                // Clears the console screen
                Console.Clear();
                Console.WriteLine("Select a WAV file to play:");

                // Loop through the existing wav files and display them
                for (int i = 0; i < wavFiles.Length; i++)
                {
                    Console.ForegroundColor = played[i] ? ConsoleColor.Green : ConsoleColor.White;
                    // Display the numbered list of audio files
                    Console.WriteLine($"{i + 1}. {wavFileNames[i]}");
                }
                // Change the color if the file has been played before 
                Console.ResetColor();
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                int choice;

                if (!int.TryParse(input, out choice) || choice < 1 || choice > 4)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ReadKey();
                    continue;
                }

                if (choice == 4)
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }

                PlayWav(choice - 1);
            }
        }

        private void PlayWav(int index)
        {
            try
            {
                currentIndex = index;
                player = new SoundPlayer(wavFiles[index]);
                isPlaying = true;
                isPaused = false;

                // Stop previous thread if it's running
                if (playThread != null && playThread.IsAlive)
                {
                    playThread.Abort(); // Stop the thread if it's still running
                    Console.WriteLine("Previous playback stopped.");
                }

                // Create a new playback thread
                playThread = new Thread(() =>
                {
                    player.Play();
                });
                playThread.Start();

                Console.WriteLine($"Now playing: {wavFileNames[index]}");
                Console.WriteLine("Press 'P' to pause, 'U' to unpause, or 'S' to stop.");

                while (isPlaying)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.P)
                        {
                            PausePlayback();
                        }
                        else if (key == ConsoleKey.U)
                        {
                            UnpausePlayback();
                        }
                        else if (key == ConsoleKey.S)
                        {
                            StopPlayback();
                            Console.WriteLine("Playback stopped.");
                            break;
                        }
                    }
                    Thread.Sleep(200);
                }

                played[index] = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void PausePlayback()
        {
            if (isPlaying && !isPaused)
            {
                isPaused = true;
                player.Stop();
                Console.WriteLine("Playback paused. Press 'U' to unpause.");
            }
        }

        private void UnpausePlayback()
        {
            if (isPaused)
            {
                isPaused = false;
                Console.WriteLine("Resuming from the start...");
                PlayWav(currentIndex); // Restart the song
            }
        }

        private void StopPlayback()
        {
            isPlaying = false;
            isPaused = false;
            player?.Stop();

            // Abort the playback thread to ensure the song doesn't keep playing in the background
            if (playThread != null && playThread.IsAlive)
            {
                playThread.Abort();
                Console.WriteLine("Playback thread aborted.");
            }
        }
    }
}