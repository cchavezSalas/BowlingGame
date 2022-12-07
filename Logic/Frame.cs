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

        public void SetFramePunctuation(int punctuation)
        {
            FramePunctuation = punctuation;
        }

        public void SetCurrentPunctuation(int punctuation)
        {
            CurrentPunctuation = punctuation;
        }

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


        public virtual string GetPrintableScore(char separator)
        {
            //if (this.Ordinal == 9)
            //{
            //    return this.CurrentPunctuation.ToString();
            //}
            //else
            //{
            //    return separator.ToString()+separator.ToString()+ this.CurrentPunctuation.ToString();
            //}

            return separator.ToString() + separator.ToString() + this.CurrentPunctuation.ToString();
        }


        public int Score {
            get {
                return 0;
            }
        }

    }
}
