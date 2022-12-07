using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class LastFrame : Frame
    {
        public int Shot03 { get; }

        public LastFrame(int ordinal, string name, int shot01, int shot02, int shot03) : base(ordinal, name, shot01, shot02)
        {
            Shot03 = shot03;
        }

        public override string GetPrintablePinfalls(char separator) 
        {
            string shot1Str = this.Shot01 == 10 ? "X" : this.Shot01.ToString();
            string shot2Str = this.Shot02 ==10?"X":this.Shot02.ToString();
            string shot3Str = this.Shot03 ==10? "X" : this.Shot03.ToString();
            return shot1Str + separator + shot2Str + separator + shot3Str;
        }

        public override string GetPrintableScore(char separator)
        {
            return base.GetPrintableScore(separator);
        }

    }
}
