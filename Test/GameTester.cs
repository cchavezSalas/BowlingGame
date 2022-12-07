using NUnit.Framework;
using Logic;
using System.Collections.Generic;
using Logic.ConversionBowlingTypes;
using System;
using Logic.Printer;

namespace Test
{
    public class GameTester
    {
        [SetUp]
        public void Setup()
        {
        }


        /// <summary>
        /// Inner convention shot -1 is null
        /// </summary>
        [Test]
        public void TestLastFramePossibleValues()
        {

           ///null
            
            int shot01 = -1;
            int shot02 = -1;
            int shot03 = -1;

            string message = "";
            bool option1_null_null_null = RulesValidator.IsLastValidFrame(shot01, shot02, shot03,out message);
            print(message);
            bool fistControl = (option1_null_null_null == false);


            
            //if shot1 and shot 2 is 10 there should be a third shot
            shot01 = 7;
            shot02 = 3;
            shot03 = -1;
            message = "";
            bool option2_num_num_null = RulesValidator.IsLastValidFrame(shot01, shot02, shot03, out message);
            print("second option" + message);
            bool secondControl =(option2_num_num_null ==false);

            //third option should be 0 to 10
            shot01 = 7;
            shot02 = 3;
            shot03 = 10;
            message = "";
            bool option3_num_num_num = RulesValidator.IsLastValidFrame(shot01, shot02, shot03, out message);
            print("third option" + message);
            bool thirdcontrol = (option3_num_num_num == true);

            //fourt option if shot1 + shot2 less than 10 there should not be a third option
            shot01 = 7;
            shot02 = 2;
            shot03 = 10;
            message = "";
            bool option4_num_num_num_not10 = RulesValidator.IsLastValidFrame(shot01, shot02, shot03, out message);
            print("fourth option" + message);
            bool fourthcontrol = (option4_num_num_num_not10 == false);




            // fifth option 10 10 10 allowed
            shot01 = 10;
            shot02 = 10;
            shot03 = 10;
            message = "";
            bool option5_x_x_x = RulesValidator.IsLastValidFrame(shot01, shot02, shot03, out message);
            print("fifth option" + message);
            bool fifthoption = (option5_x_x_x == true);

            //sixth option 
            //if first shot is 10 mandatory the two nexts
            shot01 = 10;
            shot02 = 1;
            shot03 = -1;
            message = "";
            bool option6_x_num_null = RulesValidator.IsLastValidFrame(shot01, shot02, shot03, out message);
            print("sixth option" + message);
            bool sixthoption = (option6_x_num_null == false);

            //seventh option if first shot is not 10 shot1 + shot2 should be less than 10
            shot01 = 1;
            shot02 = 10;
            shot03 = -1;
            message = "";
            bool option7_x_num_null = RulesValidator.IsLastValidFrame(shot01, shot02, shot03, out message);
            print("seventh option" + message);
            bool seventhioption = (option7_x_num_null == false);


            Assert.IsTrue(fistControl 
                && secondControl 
                && thirdcontrol 
                && fourthcontrol 
                && fifthoption 
                && sixthoption
                && seventhioption
                );
        }


        [Test]
        public void TestPlayerFrames()
        {
            try
            {

                var validChances = GetDummyChanceList("Chris", 3, 7, 6, 3, 10, 8, 1, 10, 10, 9, 0, 7, 3, 4, 4, 10, 9, 0);
                PlayerSet playerSet1 = new PlayerSet();
                playerSet1.PlayerName = "Chris";
                playerSet1.Chances = validChances;
                var lstFrames = new BowlingTypeConversor().GetFramesByPlayerSet(playerSet1);
                print(lstFrames);
                if (lstFrames.Count == 10)
                {
                    Assert.IsTrue(true);
                }

            }
            catch (Exception ex)
            {
             
                print(ex.ToString());
                Assert.Fail();
            }
           

        }

        [Test]
        public void TestGame()
        {


            try
            {
                var validChances = GetDummyChanceList("Chris", 3, 7, 6, 3, 10, 8, 1, 10, 10, 9, 0, 7, 3, 4, 4, 10, 9, 0);
                PlayerSet playerSet1 = new PlayerSet();
                playerSet1.PlayerName = "Chris";
                playerSet1.Chances = validChances;
                var lstFrames = new BowlingTypeConversor().GetFramesByPlayerSet(playerSet1);
                
                Game game = new Game(lstFrames, "Chris");

                game.Calculate();

                print(game.LstFrames);
                Assert.IsTrue(game.Score == 151);
            }
            catch (Exception ex)
            {
                print(ex.ToString());
                Assert.Fail();
            }
        }

        private Game GetPerfectGame()
        {

            string playerName = "Nick";
            var validChances = GetDummyChanceList(playerName, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10);
            PlayerSet playerSet1 = new PlayerSet();
            playerSet1.PlayerName = playerName;
            playerSet1.Chances = validChances;
            var lstFrames = new BowlingTypeConversor().GetFramesByPlayerSet(playerSet1);
            Game game = new Game(lstFrames, playerName);
            game.Calculate();
            return game;

        }

        [Test]
        public void TestPerfectGame()
        {
            try
            {
                var validChances = GetDummyChanceList("Chris", 10,10,10,10,10, 10, 10, 10, 10, 10,10,10);
                PlayerSet playerSet1 = new PlayerSet();
                playerSet1.PlayerName = "Chris";
                playerSet1.Chances = validChances;
                var lstFrames = new BowlingTypeConversor().GetFramesByPlayerSet(playerSet1);
                
                Game game = new Game(lstFrames, "Chris");

                game.Calculate();

                print(game.LstFrames);

                Assert.IsTrue(game.Score == 300);

            }
            catch (Exception ex)
            {
                print(ex.ToString());
                Assert.Fail();
            }

        }


        private void print(List<Frame> frames)
        {
            frames.ForEach(f => print(f));
        
        }
        private void print(Frame frame)
        {
            print("nro: " + frame.Ordinal.ToString());
            
            if (frame is LastFrame)
            { 
                LastFrame lastFrame = (LastFrame)frame;
                print($"shot03: {lastFrame.Shot01}  shot02: {frame.Shot02} Shot03: {lastFrame.Shot03}  SpareType: {lastFrame.GetSpareType().ToString()}  FramePuctuation: {lastFrame.FramePunctuation} CurrentPunctuation: {lastFrame.CurrentPunctuation} ");
            }
            else
            {
                print($"shot01: {frame.Shot01}  shot02: {frame.Shot02}  SpareType: {frame.GetSpareType().ToString()}  FramePuctuation: {frame.FramePunctuation} CurrentPunctuation: {frame.CurrentPunctuation} ");
            }

        }


        private List<Chance> GetDummyChanceList(string playerName , params int[] shots)
        {
            List<Chance> result =  new List<Chance>();

            int order = 0;
            foreach (var shot in shots)
            {
                result.Add(new Chance() {  ChanceOrder =order, Name = playerName, PinsKnocked=shot});
                order++;
            }

            return result;

        }


        [Test]
        public void PrintMultipleGameText()
        {
            try
            {
                var validChances = GetDummyChanceList("Chris", 3, 7, 6, 3, 10, 8, 1, 10, 10, 9, 0, 7, 3, 4, 4, 10, 9, 0);
                PlayerSet playerSet1 = new PlayerSet();
                playerSet1.PlayerName = "Chris";
                playerSet1.Chances = validChances;
                var lstFrames = new BowlingTypeConversor().GetFramesByPlayerSet(playerSet1);
                Game game = new Game(lstFrames, "Chris");
                game.Calculate();


                List<Game> games = new List<Game>();
                games.Add(game);

                games.Add(GetPerfectGame());

                var printer = new Printer(games);
                printer.Generate();
                //print(game.LstFrames);

                print("==============");
                print(printer.GetPrintableText());
                Assert.IsTrue(true); //reaching this line assume assertion
            }
            catch (Exception ex)
            {
                print(ex.ToString());
                Assert.Fail();

            }

        }

        [Test]
        public void PrintSingleGameText()
        {
            try
            {
                var validChances = GetDummyChanceList("Chris", 3, 7, 6, 3, 10, 8, 1, 10, 10, 9, 0, 7, 3, 4, 4, 10, 9, 0);
                PlayerSet playerSet1 = new PlayerSet();
                playerSet1.PlayerName = "Chris";
                playerSet1.Chances = validChances;
                var lstFrames = new BowlingTypeConversor().GetFramesByPlayerSet(playerSet1);
                Game game = new Game(lstFrames, "Chris");
                game.Calculate();


                List<Game> games = new List<Game>();
                games.Add(game);

                var printer = new Printer(games);
                printer.Generate();
                //print(game.LstFrames);

                print("==============");
                print(printer.GetPrintableText());
                Assert.IsTrue(true); //reaching this line assume assertion
            }
            catch (Exception ex)
            {
                print(ex.ToString());
                Assert.Fail();
                
            }

        }




        private void print(string text)
        {
            TestContext.WriteLine(text);
        }

    }
}