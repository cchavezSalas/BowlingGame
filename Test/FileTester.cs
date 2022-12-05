using Logic.FileManagement;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test
{
    public class FileTester
    {

        private  string _validFileCreated;
        private  string _notValidFileCreated;

        [SetUp]
        public void SetUpFileTests()
        {

            this._validFileCreated = "validFileDummy.txt";
            CreateFile(this._validFileCreated);

            /*
             The second one has a wrong extension
             */

            this._notValidFileCreated = "notValidDummy.log";
            CreateFile(this._notValidFileCreated);
            
        
        }


        private void CreateFile(string fileName)
        {

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string[] lines =
                {
                "Chris 10", "Chris 7", "John F"
                };
            File.WriteAllLines(fileName,lines);
        }





        [Test]
        public void FileNotFound()
        {
            string filePathNotExisting = @"D:\newdirectory\filenotexists.txt";
            SourceManager sm = new SourceManager(filePathNotExisting);
            bool isFileValid = sm.IsValidFile();
            Assert.IsFalse(isFileValid);
        }


        [Test]
        public void NotValidFormat()
        {
            string filePathNotExisting = this._notValidFileCreated;
            SourceManager sm = new SourceManager(filePathNotExisting);
            bool isFileValid = sm.IsValidFile();
            Assert.IsFalse(isFileValid);

        }

        [Test]
        public void ValidFileFormat()
        {
            string filePath = this._validFileCreated;
            SourceManager sm = new SourceManager(filePath);
            bool isFileValid = sm.IsValidFile();
            Assert.IsTrue(isFileValid);
        }


    }
}
