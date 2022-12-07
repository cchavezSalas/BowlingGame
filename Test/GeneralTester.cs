using Logic;
using Logic.ConversionBowlingTypes;
using Logic.Error;
using Logic.FileManagement;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Test
{
    public class GeneralTester
    {

        private  string _validFileCreated;
        private  string _notValidFileCreated;
        private string _notValidFileDataCreated;

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

            this._notValidFileDataCreated = "notValidData.txt";
            CreateFileChancesNotOk(this._notValidFileDataCreated);
        
        }

        #region Helpers

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
            File.WriteAllLines(fileName, lines);
        }

        private void CreateFileChancesNotOk(string fileName)
        {

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string[] lines =
                {
                "Chris 10", "Chris 7", "John F","Jhon -1" //the last one is not ok
                };
            File.WriteAllLines(fileName, lines);
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

                if (!IsEqual(item, lstChance02[index]))
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
            chances.ForEach(c =>
            {
                string controlLine = $" chance order : {c.ChanceOrder}  name: {c.Name} pins: {c.PinsKnocked}";
                sbuilder.AppendLine(controlLine);
            });
            //Console.WriteLine(sbuilder.ToString());

            TestContext.WriteLine(sbuilder.ToString());

        }

        private void print(string text)
        {
            TestContext.WriteLine(text);
        }


        #endregion Helpers


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




        [Test]
        public void TestNotValidChaces()
        {
            bool isCorrectExceptionThrown = false;

            try
            {
                var sm = new SourceManager(this._notValidFileDataCreated);
                sm.SetChancesData();

            }
            catch (ManagedException ex)
            {

                if (ex.Type == ManagedExceptionType.NotValidInfo)
                {
                    isCorrectExceptionThrown = true;
                }

            }
            catch (Exception ex)
            {

                print(ex.ToString());
            }

            Assert.IsTrue(isCorrectExceptionThrown);
        }

        [Test]
        public void TestValidChancesToPlayerSetConversion()
        {
            try
            {
                List<Chance> lstChances = new List<Chance>();

                lstChances.Add(new Chance() { ChanceOrder = 0, Name = "Chris", PinsKnocked = 0 });
                lstChances.Add(new Chance() { ChanceOrder = 1, Name = "John", PinsKnocked = 5 });
                lstChances.Add(new Chance() { ChanceOrder = 2, Name = "Chris", PinsKnocked = 5 });
                lstChances.Add(new Chance() { ChanceOrder = 3, Name = "Chris", PinsKnocked = 3 });
                lstChances.Add(new Chance() { ChanceOrder = 4, Name = "John", PinsKnocked = 4 });


                //expected:

                List<PlayerSet> lstPlayerSetExpecte = new List<PlayerSet>();


                PlayerSet playerSet1 = new PlayerSet()
                {
                    PlayerName = "Chris",
                    Chances = new List<Chance>() { new Chance() { ChanceOrder=0, Name = "Chris", PinsKnocked =0}
            , new Chance() { ChanceOrder =1, Name="Chris", PinsKnocked = 5}
            , new Chance() { ChanceOrder =2, Name="Chris", PinsKnocked = 3}
            }
                };

                PlayerSet playerSet2 = new PlayerSet()
                {
                    PlayerName = "John",
                    Chances = new List<Chance>() {
                new Chance() { ChanceOrder =0, Name="John", PinsKnocked = 5},
                new Chance() { ChanceOrder =1, Name="John", PinsKnocked = 4},
                }

                };

                lstPlayerSetExpecte.Add(playerSet1);
                lstPlayerSetExpecte.Add(playerSet2);

                lstPlayerSetExpecte.ForEach(p => print(p));



                var lstPlayerSetGenerated = new BowlingTypeConversor().GetPlayerSetsByChances(lstChances);

                print("generated:");

                lstPlayerSetGenerated.ForEach(p => print(p));



                
                Assert.IsTrue(AreEqual(lstPlayerSetExpecte[0], lstPlayerSetGenerated[0])
                    && AreEqual(lstPlayerSetExpecte[1], lstPlayerSetGenerated[1]));
            }
            catch (Exception ex)
            {

                print(ex.ToString());
            }

        }



        /// <summary>
        /// Copy of valid, but little chage in John pins knocked in expected
        /// </summary>
        [Test]
        public void TestNotValidChancesToPlayerSetConversion()
        {
            try
            {
                List<Chance> lstChances = new List<Chance>();

                lstChances.Add(new Chance() { ChanceOrder = 0, Name = "Chris", PinsKnocked = 0 }); 
                lstChances.Add(new Chance() { ChanceOrder = 1, Name = "John", PinsKnocked = 4 }); //changed to 4
                lstChances.Add(new Chance() { ChanceOrder = 2, Name = "Chris", PinsKnocked = 5 });
                lstChances.Add(new Chance() { ChanceOrder = 3, Name = "Chris", PinsKnocked = 3 });
                lstChances.Add(new Chance() { ChanceOrder = 4, Name = "John", PinsKnocked = 4 });


                //expected:

                List<PlayerSet> lstPlayerSetExpecte = new List<PlayerSet>();


                PlayerSet playerSet1 = new PlayerSet()
                {
                    PlayerName = "Chris",
                    Chances = new List<Chance>() { new Chance() { ChanceOrder=0, Name = "Chris", PinsKnocked =0}
            , new Chance() { ChanceOrder =1, Name="Chris", PinsKnocked = 5}
            , new Chance() { ChanceOrder =2, Name="Chris", PinsKnocked = 3}
            }
                };

                PlayerSet playerSet2 = new PlayerSet()
                {
                    PlayerName = "John",
                    Chances = new List<Chance>() {
                new Chance() { ChanceOrder =0, Name="John", PinsKnocked = 5},
                new Chance() { ChanceOrder =1, Name="John", PinsKnocked = 4},
                }

                };

                lstPlayerSetExpecte.Add(playerSet1);
                lstPlayerSetExpecte.Add(playerSet2);

                lstPlayerSetExpecte.ForEach(p => print(p));



                var lstPlayerSetGenerated = new BowlingTypeConversor().GetPlayerSetsByChances(lstChances);

                print("generated:");

                lstPlayerSetGenerated.ForEach(p => print(p));


                Assert.IsTrue(!AreEqual(lstPlayerSetExpecte[0], lstPlayerSetGenerated[0]) || !AreEqual(lstPlayerSetExpecte[1], lstPlayerSetGenerated[1]));

                //if any of the conditions is not equal set to true



                //Assert.IsTrue(AreEqual(lstPlayerSetExpecte[0], lstPlayerSetGenerated[0])
                //    && AreEqual(lstPlayerSetExpecte[1], lstPlayerSetGenerated[1]));
            }
            catch (Exception ex)
            {

                print(ex.ToString());
            }

        }




        private void print(PlayerSet playerSet)
        {
            print(" =========== player set =========");
            print(playerSet.PlayerName);
            print(playerSet.Chances);
            print(" =========== end player set ======");
        }

        private bool AreEqual(PlayerSet ps1, PlayerSet ps2)
        {
            bool result = true;

            if (ps1.PlayerName != ps2.PlayerName)
            {
                result = false;
            }

            if (!AreEqual(ps1.Chances, ps2.Chances))
            {
                result = false;
            }

            return result;
        
        }


        /*Not intended for testing*/
        //[Test]
        //public void ExecutableTest()
        //{
        //    try
        //    {
        //        string filepath = @"C:\Temp\examples\ExampleBowling.txt";
        //        new Application(filepath).Start();
        //        Assert.IsTrue(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        print(ex.ToString());
        //        Assert.Fail();
        //    }
        //}


    }
}
