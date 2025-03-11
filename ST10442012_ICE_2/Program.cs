
/*
 *      Kaveer Lala ST10442012@vcconnect.edu.za
 *      
 *      This is my main class that will run the audio player.
 * 
 * 
 */





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace ST10442012_ICE_2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the instance of the audio player and run the menu 
            AudioPlayer player = new AudioPlayer();
            player.ShowMenu();
        }
    }
}
