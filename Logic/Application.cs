using Logic.Error;
using Logic.FileManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class Application
    {
        private string _filePath;
        public Application(string filePath)
        {
            this._filePath = filePath;

        }

        public void Start()
        {
            try
            {
                //instace of source manager
                var sm = new SourceManager(this._filePath);

                //get raw chances
                sm.SetChancesData();
                var lstChances = sm.GetChances();

                //instatiate conversor
                var conversor = new ConversionBowlingTypes.BowlingTypeConversor();


                //all player Sets
                var lstPlayerSet = conversor.GetPlayerSetsByChances(lstChances);

                //get all games
                var lstGames = conversor.GetGamesByPlayerSets(lstPlayerSet);

                //calculate scoring of all games
                lstGames.ForEach(g => g.Calculate());

                //Print Game resultos
                Printer.Printer printer = new Printer.Printer(lstGames);
                printer.Generate();
                Printer.Printer.PrintToConsole(printer.GetPrintableText());

                

            }

            catch (ManagedException ex1)
            {
                Printer.Printer.PrintToConsole("There's an error in the input data");
                Printer.Printer.PrintToConsole(ex1.Message);

            }
            catch (Exception ex)
            {

                Printer.Printer.PrintToConsole(ex.ToString());
            }
        
        }



    }
}
