using Logic.Error;
using System.Collections.Generic;
using System.Linq;

namespace Logic.ConversionBowlingTypes
{
    /// <summary>
    /// this class converts
    /// list chances -> list Player Set
    /// List Player Set -> List Frame
    /// </summary>
    public class BowlingTypeConversor
    {

        /// <summary>
        /// Converts collection of Chances to List PlayerSet
        /// This method converts all raw chances given in input file
        /// and Makes a PlayerSet (PlayerName + player chances)
        /// </summary>
        /// <param name="lstChances"></param>
        /// <returns>A List of PlayerSet</returns>
        public List<PlayerSet> GetPlayerSetsByChances(List<Chance> lstChances)
        {
            List<PlayerSet> lstPlayerSets = new List<PlayerSet>();
            var lstNames = lstChances.Select(l => l.Name).Distinct();


            foreach (var name in lstNames)
            {
                var playerSet = GetPlayerSetByNameListChance(name, lstChances.Where(c => c.Name == name).ToList());
                lstPlayerSets.Add(playerSet);
            }

            return lstPlayerSets;
            
        }


        /// <summary>
        /// Instanciates a PlayerSet with given Name and Chances
        /// </summary>
        /// <param name="name">Name of player</param>
        /// <param name="lstChances">chances of player</param>
        /// <returns></returns>
        private  PlayerSet  GetPlayerSetByNameListChance(string name, List<Chance> lstChances)
        {

            PlayerSet playerSet = new PlayerSet();
            playerSet.PlayerName = name;

            var lstChancesi = lstChances.OrderBy(c => c.ChanceOrder).ToList();

            int counter = 0;

            foreach (var chancei in lstChancesi)
            {
                chancei.ChanceOrder = counter;
                counter++;
            }

            playerSet.Chances = lstChancesi;
            return playerSet;
        
        }




        /// <summary>
        /// Converts a PlayerSet to a List of Frames
        /// </summary>
        /// <param name="playerSet"></param>
        /// <returns>a list of Frames</returns>
        /// <exception cref="ManagedException"></exception>
        public List<Frame> GetFramesByPlayerSet(PlayerSet playerSet)
        {
            List<Frame> lstFrames = new List<Frame>();

            int maxFrame = 9;
            int currentFrame = 0;


            var lstChances =
            playerSet.Chances.OrderBy(c => c.ChanceOrder).ToList();


            for (int i = 0; i < lstChances.Count(); i++)
            {

                var chancei = lstChances[i];

                if (currentFrame>maxFrame)
                {
                    throw new ManagedException(ManagedExceptionType.NotValidInfo, "More Chances than expected");
                }


                if (currentFrame == maxFrame) //the last frame
                {

                    var pinsKnockedChancei1 = lstChances[i + 1] != null ? lstChances[i + 1].PinsKnocked : -1;
                    var pinsKnockedChancei2 = lstChances[i + 2] != null ? lstChances[i + 2].PinsKnocked : -1;

                    string outMessage = "";

                    if (!RulesValidator.IsLastValidFrame(chancei.PinsKnocked, pinsKnockedChancei1, pinsKnockedChancei2, out outMessage))
                    {
                        throw new ManagedException(ManagedExceptionType.NotValidInfo, "Pins last frame not valid " + "\n" + outMessage);
                    }
                    var lastFrame = new LastFrame(currentFrame, playerSet.PlayerName, chancei.PinsKnocked, pinsKnockedChancei1, pinsKnockedChancei2);
                    lstFrames.Add(lastFrame);


                    if (lstChances[i + 1] != null)
                    { i++; }

                    if (lstChances[i + 1] != null)
                    { i++; }

                    currentFrame++;
                }

                else
                {


                    if (chancei.PinsKnocked < 10)
                    {

                        var chancei1 = lstChances[i + 1];

                        if (!RulesValidator.IsValidFrame(chancei.PinsKnocked, chancei1.PinsKnocked))
                        {
                            throw new ManagedException(ManagedExceptionType.NotValidInfo, "Not valid Frame");
                        }

                        Frame frame = new Frame(currentFrame, playerSet.PlayerName, chancei.PinsKnocked, chancei1.PinsKnocked);
                        lstFrames.Add(frame);
                        currentFrame++;


                        i++; //jump next iteration 
                    }
                    else if (chancei.PinsKnocked == 10)
                    {
                        //pins knocked =10

                        Frame frame = new Frame(currentFrame, playerSet.PlayerName, chancei.PinsKnocked, 0);
                        lstFrames.Add(frame);
                        currentFrame++;
                    }
                    else
                    {
                        throw new ManagedException(ManagedExceptionType.NotValidInfo, "Pins knocked not in correct range");
                    }


                }
            }


            return lstFrames;
        }


        /// <summary>
        /// Given a List of PlayerSet, converts to a List of Games
        /// </summary>
        /// <param name="lstPlayerSet"></param>
        /// <returns></returns>
        public List<Game> GetGamesByPlayerSets(List<PlayerSet> lstPlayerSet)
        {
            List<Game> lstGames = new List<Game>();
            foreach (var item in lstPlayerSet)
            {
                lstGames.Add(new Game(GetFramesByPlayerSet(item), item.PlayerName));
            }
            return lstGames;
        }
        
        
    }
}
