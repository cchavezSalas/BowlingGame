using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public enum SpareType
    { 
        Full,
        Half,
        None
    }

    /// <summary>
    /// Represents a "Frame" of Bowling
    /// </summary>
    public class Frame
    {
        string Name { get; }

        public int Ordinal { get; }
        public int FramePunctuation { get; private set; } 
        public int Shot01 { get;  }
        public int Shot02 { get;  }

        public int CurrentPunctuation { get; private set; }

        public Frame(int ordinal, string name, int shot01, int shot02)
        {
            Ordinal = ordinal;
            Shot01 = shot01;
            Shot02 = shot02;
            Name = name;
        }

        /// <summary>
        /// Sets this frame score
        /// </summary>
        /// <param name="punctuation"></param>
        public void SetFramePunctuation(int punctuation)
        {
            FramePunctuation = punctuation;
        }

        /// <summary>
        /// Sets Current punctuation (sum of all score to this frame)
        /// </summary>
        /// <param name="punctuation"></param>
        public void SetCurrentPunctuation(int punctuation)
        {
            CurrentPunctuation = punctuation;
        }


        /// <summary>
        /// Gets the type of frame:
        /// Spare, Half Spare and None
        /// </summary>
        /// <returns>Type of Frame</returns>
        public SpareType GetSpareType()
        {
            SpareType result = SpareType.None;

            if (Shot01 == 10)
            {
                result = SpareType.Full;
            }

            if (Shot01 < 10 && Shot01 + Shot02 == 10)
            {
                result= SpareType.Half;
            }

            return result;
        }

        /// <summary>
        /// Gets the text of Pinfalls for this frame
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public virtual string GetPrintablePinfalls(char separator)
        {
            string result = "";

            switch (this.GetSpareType())
            {
                case SpareType.Full:
                    result= separator.ToString() + "X";
                    break;
                case SpareType.Half:
                    result= Shot01.ToString() + separator.ToString() + "/";
                    break;
                case SpareType.None:
                    result= Shot01.ToString() + separator.ToString() + Shot02.ToString();
                    break;
                default:
                    break;
            }

            return result+separator.ToString();
        }


        /// <summary>
        /// Get the text for printing the score
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public virtual string GetPrintableScore(char separator)
        {

            return separator.ToString() + separator.ToString() + this.CurrentPunctuation.ToString();
        }



    }
}
