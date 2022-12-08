using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    /// <summary>
    /// Several rules to validate
    /// </summary>
    public static class RulesValidator
    {
        /// <summary>
        /// Controls that whe have a valid chance between 0 and 10
        /// </summary>
        /// <param name="chance"></param>
        /// <returns></returns>
        public static bool ValidateChance(Chance chance)
        {
            bool result = true;
            if (chance.PinsKnocked<0 || chance.PinsKnocked>10)
            {
                return false;
            }
            return result;
        }

        /// <summary>
        /// True if a Frame is valid (not for lastframe)
        /// </summary>
        /// <param name="pinsKnockedShot01"></param>
        /// <param name="pinsKnockedShot02"></param>
        /// <returns></returns>
        public static bool IsValidFrame(int pinsKnockedShot01, int pinsKnockedShot02)
        {
            bool valid = true;

            if ((pinsKnockedShot01 + pinsKnockedShot02) > 10)
            {
                return false;
            }


            return valid;
        }


        /// <summary>
        /// Validates Last Frame
        /// </summary>
        /// <param name="pinsKnocked"></param>
        /// <param name="pinsKnockedChancei1"></param>
        /// <param name="pinsKnockedChancei2"></param>
        /// <returns></returns>
        public static bool IsLastValidFrame(int pinsKnocked, int pinsKnockedChancei1, int pinsKnockedChancei2, out string message)
        {

            message = "";
            bool result = true;


            if (pinsKnocked == -1)
            {
                message = "first try should not be null";
                return false;
            }

            
            /*
             if there's no chance in shot2 or shot3 the default value is -1
             */

            if (pinsKnocked == 10 && pinsKnockedChancei1 == 10)
            {
                if (pinsKnockedChancei2 == -1)
                {
                    message = "las try en last frame cannot be null";
                    return false;
                }

            }

            if (pinsKnocked == 10)
            {
                if ((pinsKnockedChancei1 == -1) || (pinsKnockedChancei2 == -1))
                {
                    message = "there has to be two attempts in last frame";
                    return false;
                }

            }
            if ((pinsKnocked + pinsKnockedChancei1) == 10)
            {
                if (pinsKnockedChancei2 == -1)
                {
                    message = "third chance is mandatory when try 1 and try 2 is ten";
                    return false;
                }
            }

            if (pinsKnocked + pinsKnockedChancei1 < 10)
            {
                if (pinsKnockedChancei2 != -1)
                {
                    message = "no third chance is allowed when try 1 and try 2 is < 10";
                    return false;
                }
            }


            if (pinsKnocked < 10)
            {
                if (pinsKnockedChancei1 == -1)
                {
                    message = "first shot less than 10 then second is ";
                    return false;
                }

                if (pinsKnocked + pinsKnockedChancei1 > 10)
                {
                    message = "first shot less than 10 then first + second should be <=10";
                    return false;
                }
            
            }
            


            return result;

        }


        /// <summary>
        /// Validates Game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static bool IsValidGame(Game game)
        {
            bool result = true;
            if (game.LstFrames ==null || game.LstFrames.Count!=10)
            {
                result = false;
                return result;
            }
            return result;

        }


    }
}
