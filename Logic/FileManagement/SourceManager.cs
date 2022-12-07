using Logic.Error;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logic.FileManagement
{
    public class SourceManager
    {
        private string _filePath;
        private readonly string _validExtension = ".txt";
        private List<Chance> _chances;

        public SourceManager(string filePath)
        {
            this._filePath = filePath;
        }



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



        public List<Chance> GetChances()
        {
            return this._chances;
        }


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
