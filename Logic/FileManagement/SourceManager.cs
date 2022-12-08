using Logic.Error;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logic.FileManagement
{
    /// <summary>
    /// Process all raw data from input file
    /// </summary>
    public class SourceManager
    {
        private string _filePath;
        private readonly string _validExtension = ".txt";
        private List<Chance> _chances;

        public SourceManager(string filePath)
        {
            this._filePath = filePath;
        }


        /// <summary>
        /// Checks if file has right extension
        /// </summary>
        /// <returns></returns>
        public bool IsValidFile()
        {

            if (!File.Exists(this._filePath))
            {
                return false;
            }

            var fileExt = Path.GetExtension(this._filePath);
            if (fileExt != this._validExtension)
            {
                return false;
            }


            return true;

        }

        /// <summary>
        /// Tries to extract Chance (name and knockedpins)
        /// </summary>
        public void SetChancesData()
        {

            try
            {

                TrySetChances();
                ValidateChanceData();

            }
            catch (ManagedException mgEx)
            {

                var str = mgEx.Description;
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// Checks if a chance has right number of pins (ex: number not >10)
        /// </summary>
        /// <exception cref="ManagedException"></exception>
        private void ValidateChanceData()
        {
            this._chances.ForEach(ch =>
            {

                if (!RulesValidator.ValidateChance(ch))
                {
                    throw new ManagedException(ManagedExceptionType.NotValidInfo, "Not a valid chance");
                }

            });



        }


        /// <summary>
        /// Returns All Raw Chances from input file
        /// </summary>
        /// <returns></returns>
        public List<Chance> GetChances()
        {
            return this._chances;
        }

        /// <summary>
        /// Takes all Lines from file and sets in a list<string>
        /// </summary>
        /// <returns>true if success</returns>
        /// <exception cref="ManagedException"></exception>
        private bool TrySetChances()
        {

            bool wasSuccessful = false;
            try
            {

                var lstLines = TryGetLinesFromFile();
                this._chances = GetChances(lstLines);
                wasSuccessful = true;

            }
            catch (Exception ex)
            {
                throw new ManagedException(ManagedExceptionType.Convertion, ex.Message);
            }
            return wasSuccessful;
        }


        /// <summary>
        /// Read Lines from File
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ManagedException"></exception>
        private List<string> TryGetLinesFromFile()
        {
            try
            {
                string[] readText = File.ReadAllLines(this._filePath);

                List<string> result = new List<string>();
                result.AddRange(readText);
                return result;
            }
            catch (Exception ex)
            {

                throw new ManagedException(ManagedExceptionType.File, ex.Message);
            }


        }


        /// <summary>
        /// Converts all Chances from a given list of lines
        /// </summary>
        /// <param name="lstStrChances"></param>
        /// <returns>List of Chances</returns>
        private List<Chance> GetChances(List<string> lstStrChances)
        {
            //var chances = lstStrChances.Select(t=>ConvertLineToChance(t)).ToList();

            List<Chance> result = new List<Chance>();
            int counter = 0;
            foreach (var item in lstStrChances)
            {
                var chance = ConvertLineToChance(item, counter);
                
                result.Add(chance);


                counter++;
            }

            return result;
        }



        /// <summary>
        /// Converts a line (from input) to a Chance
        /// Also translates an F to 0
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="lineNumber">index of line</param>
        /// <returns>a Chance object</returns>
        /// <exception cref="ManagedException"></exception>
        private Chance ConvertLineToChance(string line, int lineNumber)
        {
            string[] variables = line.Split(' ');

            if (variables.Length != 2)
            {
                throw new ManagedException(ManagedExceptionType.Convertion, "Not two fields");
            }

            if (variables[1].ToLower() == "f")
            {
                variables[1] = "0";
            }

            try
            {

                Chance chance = new Chance();
                chance.ChanceOrder = lineNumber;
                chance.PinsKnocked = int.Parse(variables[1]);
                chance.Name = variables[0];
                return chance;
            }
            catch (Exception ex)
            {
                throw new ManagedException(ManagedExceptionType.Convertion, ex.Message);
            }


        }




    }
}
