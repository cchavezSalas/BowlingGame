using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    /// <summary>
    /// This class is an extension from Frame class
    /// represents the last frame of the game
    /// </summary>
    public class LastFrame : Frame
    {
        public int Shot03 { get; }

        public LastFrame(int ordinal, string name, int shot01, int shot02, int shot03) : base(ordinal, name, shot01, shot02)
        {
            Shot03 = shot03;
        }

        /// <summary>
        /// Gets the Printable Text for Pinfalls for this frame
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public override string GetPrintablePinfalls(char separator) 
        {
            string shot1Str = this.Shot01 == 10 ? "X" : this.Shot01.ToString();
            string shot2Str = this.Shot02 ==10?"X":this.Shot02.ToString();
            string shot3Str = this.Shot03 ==10? "X" : this.Shot03.ToString();
            return shot1Str + separator + shot2Str + separator + shot3Str;
        }


        /// <summary>
        /// Gets the printable score text for this frame
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public override string GetPrintableScore(char separator)
        {
            return base.GetPrintableScore(separator);
        }

    }
}
