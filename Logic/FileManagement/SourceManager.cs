using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logic.FileManagement
{
    public class SourceManager
    {
        private string _filePath;
        private readonly string _validExtension = ".txt";

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



        public List<Chance> GetChances()
        {

            throw new NotImplementedException();
            





        }


        private Chance ConvertLine(string line)
        {

            throw new NotImplementedException();
        
        }




    }
}
