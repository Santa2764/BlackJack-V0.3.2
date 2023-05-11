using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Threading;
using System.Runtime.CompilerServices;

namespace BlackJack__V0._3._2_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Black Hohol Jack!";
            Console.OutputEncoding = Encoding.UTF8;

            Game game = new Game("Я");
            game.Start();

        }
    }
}