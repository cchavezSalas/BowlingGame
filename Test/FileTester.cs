using Logic;
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
            

            //Third Case, valid format , but chances not ok

        
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

        private void CreateFileChancesNotOk(string fileName)
        {

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string[] lines =
                {
                "Chris 10", "Chris 7", "John F","4 Joan" //the last one is not ok
                };
            File.WriteAllLines(fileName, lines);
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


        [Test]
        public void SuccessChanceConversion()
        {

            List<Chance> lstExpected = new List<Chance>();
            lstExpected.Add(new Chance() { ChanceOrder = 0, Name = "Chris" , PinsKnocked = 10});
            lstExpected.Add(new Chance() { ChanceOrder = 1, Name = "Chris" , PinsKnocked  =7 });
            lstExpected.Add(new Chance() { ChanceOrder = 2, Name = "John", PinsKnocked = 0 });


            var sm = new SourceManager(this._validFileCreated);
            sm.SetChancesData();
                var lstChances = sm.GetChances();


            print(lstChances);
            print(lstExpected);

            //Assert.That(lstChances, Is.EqualTo(lstExpected));
            Assert.IsTrue(AreEqual(lstChances,lstExpected));
        
        }


        [Test]
        public void FailChanceConversion()
        {

            List<Chance> lstExpected = new List<Chance>();
            lstExpected.Add(new Chance() { ChanceOrder = 0, Name = "Chrissss", PinsKnocked = 10 });
            lstExpected.Add(new Chance() { ChanceOrder = 1, Name = "Chris", PinsKnocked = 8 });
            lstExpected.Add(new Chance() { ChanceOrder = 2, Name = "John", PinsKnocked = 0 });


            var sm = new SourceManager(this._validFileCreated);
            sm.SetChancesData();
            var lstChances = sm.GetChances();


            print(lstChances);
            print(lstExpected);

            //Assert.That(lstChances, Is.EqualTo(lstExpected));
            Assert.IsFalse(AreEqual(lstChances, lstExpected));

        }
        private bool IsEqual(Chance chance01, Chance chance02)
        {
            bool result = true;

            if (chance01.ChanceOrder != chance02.ChanceOrder || chance01.Name != chance02.Name || chance01.PinsKnocked != chance02.PinsKnocked)
            {
                result = false;
            }

            return result;
        }


        private bool AreEqual(List<Chance> lstChance01, List<Chance> lstChance02)
        {
            bool result = true;

            

            if (lstChance01.Count != lstChance02.Count) return false;
            
            
            int index = 0;
            foreach (var item in lstChance01)
            {

                if ( !IsEqual(item , lstChance02[index]))
                {
                    result = false;
                    break;
                }

               index++;
            }


            return result;
        }




        private void print(List<Chance> chances)
        {

            StringBuilder sbuilder = new StringBuilder();
            chances.ForEach(c => {
                string controlLine = $" chance order : {c.ChanceOrder}  name: {c.Name} pins: {c.PinsKnocked}";
                sbuilder.AppendLine(controlLine);
            });
            //Console.WriteLine(sbuilder.ToString());

            TestContext.WriteLine(sbuilder.ToString());
        
        }


    }
}
