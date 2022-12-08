using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Printer
{
    /// <summary>
    /// Manages text generation and print
    /// </summary>
    public class Printer
    {

        private readonly char _separator = '\t';
        private readonly char _lineDelimiter = '\n';
        private const int _numColumns = 10;
        private StringBuilder _textSaver = new StringBuilder();
        private List<Game> _lstGames;

        /// <summary>
        /// prints line to console
        /// </summary>
        /// <param name="text"></param>
        public static void PrintLineToConsole(string text)
        { 
            Console.WriteLine(text);
        }

        /// <summary>
        /// prints text to console
        /// </summary>
        /// <param name="text"></param>
        public static void PrintToConsole(string text)
        {
            Console.Write(text);
        }

        /// <summary>
        /// Saves text to variable
        /// </summary>
        /// <param name="text"></param>
        private void SetText(string text)
        {
            //Console.Write(text);
            _textSaver.Append(text);
        }

        public Printer(List<Game> lstGames)
        {
            this._lstGames = lstGames;
        }

        /// <summary>
        /// Generates (saves in local variable) the output for all games
        /// </summary>
        public void Generate()
        {
            SetText(this._lstGames);
        }


        /// <summary>
        /// Saves in local variable output for a given game
        /// </summary>
        /// <param name="game"></param>
        private void SetText(Game game)
        {

            SetText(game.Player+_lineDelimiter);
            SetText("Pinfalls" +_separator+ game.GetPinfallsText(_separator)+_lineDelimiter);
            SetText("Score"  + game.GetScoreText(_separator)+_lineDelimiter);
            
        }

        /// <summary>
        /// Generates (saves in local variable) the output for all games called from Generate
        /// </summary>
        /// <param name="lstGames"></param>
        private void SetText(List<Game> lstGames)
        {
            SetTextHeader();
            lstGames.OrderBy(g => g.Player).ToList().ForEach(g1=>SetText(g1));

        }


        /// <summary>
        /// Sets in output the Text header (Frame 1 .... 10)
        /// </summary>
        private void SetTextHeader()
        {
            SetText("Frame"+_separator+_separator);
            
            for (int i = 1; i <= _numColumns; i++)
            {
                SetText(i.ToString());
                if (i<_numColumns)
                {
                    SetText(_separator.ToString()+_separator.ToString());
                }
                else
                {
                    SetText(_lineDelimiter.ToString());
                }
            }

        }


        /// <summary>
        /// Gets the Generated output
        /// </summary>
        /// <returns></returns>
        public string GetPrintableText()
        { 
            return _textSaver.ToString();
        }
        


    }
}
