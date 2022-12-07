using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Printer
{
    public class Printer
    {

        private readonly char _separator = '\t';
        private readonly char _lineDelimiter = '\n';
        private const int _numColumns = 10;
        private StringBuilder _textSaver = new StringBuilder();
        private List<Game> _lstGames;


        public static void PrintLineToConsole(string text)
        { 
            Console.WriteLine(text);
        }

        public static void PrintToConsole(string text)
        {
            Console.Write(text);
        }

        private void SetText(string text)
        {
            //Console.Write(text);
            _textSaver.Append(text);
        }

        public Printer(List<Game> lstGames)
        {
            this._lstGames = lstGames;
        }

        public void Generate()
        {
            SetText(this._lstGames);
        }

        private void SetText(Game game)
        {

            SetText(game.Player+_lineDelimiter);
            SetText("Pinfalls" +_separator+ game.GetPinfallsText(_separator)+_lineDelimiter);
            SetText("Score"  + game.GetScoreText(_separator)+_lineDelimiter);
            
        }

        public void SetText(List<Game> lstGames)
        {
            SetTextHeader();
            lstGames.OrderBy(g => g.Player).ToList().ForEach(g1=>SetText(g1));

        }


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

        public string GetPrintableText()
        { 
            return _textSaver.ToString();
        }
        


    }
}
