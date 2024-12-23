﻿using QuickGraph.Data;
using System.Data;
using W500Core;

namespace W500
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //LbDatabase db = new LbDatabase();
            //Console.WriteLine("Enter LetterBoxed matrix as a 12-character <word>");
            //gaphutcinmos
            //var args1 = Console.ReadLine().Split(' ');
            //var word = args1[0];
            var word = "gaphutcinmos";
            LbService svc = new LbService();
            svc.Reset();
            svc.EnterBox();
            svc.FindShortestPath(null);
        }
        static void MainW500(string[] args)
        {
            W500Database db = new W500Database();
            db.Initialize();
            var words = db.GetAllWords();
            Console.WriteLine("Enter <word> <green> <yellow> <red>");

            while (!db.Done)
            {
                var args1 = Console.ReadLine().Split(' ');
                var word = args1[0];
                int green = Convert.ToInt32(args1[1]);
                int yellow = Convert.ToInt32(args1[2]);
                int red = Convert.ToInt32(args1[3]);
                var wordScoresLookup = db.GeneratePairs(word, green, yellow, red, words);
                words = db.EnterGuess(word, green, yellow, red, wordScoresLookup);
            }
        }
    }
}
