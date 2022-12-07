using Logic.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Game
    {
        public string Player { get; private set; }

        public List<Frame> LstFrames { get; private set; }

        public int Score { get; private set; }
        

        public Game(List<Frame> lstFrames, string player)
        {
            LstFrames = lstFrames;
            Player = player;
        }
    
        public void Start() { Console.WriteLine("We are playing"); }

        public void Calculate()
        {
            this.CalculateScores();
            this.CalculateCurrentScores();
        }

        public string GetPinfallsText(char separator)
        {

            StringBuilder sb = new StringBuilder();
            this.LstFrames.OrderBy(f => f.Ordinal).ToList().ForEach(f1 => { sb.Append(f1.GetPrintablePinfalls(separator)
                //+separator.ToString()
                ); });
            return sb.ToString();
        }

        public string GetScoreText(char separator)
        {
            StringBuilder sb = new StringBuilder();
            this.LstFrames.OrderBy(f => f.Ordinal).ToList().ForEach(f1 => { sb.Append(f1.GetPrintableScore(separator)); }); ;
            return sb.ToString();
        }

        private void CalculateScores()
        {
            int index = 0;
            foreach (var frame in LstFrames.OrderBy(f => f.Ordinal))
            {

                switch (frame.GetSpareType())
                {
                    case SpareType.Full:
                        {
                            SetFullFramePunctuation (frame, LstFrames, index);
                        }
                        break;
                    case SpareType.Half:
                        {
                            SetHalfFramePunctuation(frame, LstFrames, index);
                        }
                        break;
                    case SpareType.None:
                        {
                            SetFramePunctuation(frame, index);
                            //frame.SetFramePunctuation(frame.Shot01 + frame.Shot02);
                        }
                        break;
                    default:
                        throw new ManagedException(ManagedExceptionType.NotValidInfo, "No valid spare type");
                        
                }
                index++;

            }


        }

        private void CalculateCurrentScores()
        {
            foreach (var frame in LstFrames)
            {
                frame.SetCurrentPunctuation(GetRecursiveScore(LstFrames, frame.Ordinal));    
            }

            this.Score = LstFrames.LastOrDefault().CurrentPunctuation;
        }

        private int GetRecursiveScore(List<Frame> lstFrames, int index)
        {
            if (index > 0)
            {
                return lstFrames[index].FramePunctuation+ GetRecursiveScore(lstFrames, index - 1);
            }
            else {
                return lstFrames[index].FramePunctuation;
            }
        }


        private void SetFramePunctuation(Frame frame, int index)
        {

            if (index < 9)
            {
                frame.SetFramePunctuation(frame.Shot01 + frame.Shot02);
            }
            else
            {
                LastFrame lastFrame = (LastFrame)frame;
                frame.SetFramePunctuation(lastFrame.Shot01+ lastFrame.Shot02+ lastFrame.Shot03);
            }
        }

        private void SetHalfFramePunctuation(Frame frame, List<Frame> lstFrames, int index)
        {
            if (index < 9)
            {
                var punctuationAdd = lstFrames[index + 1].Shot01;
                    //+ lstFrames[index + 1].Shot02;
                frame.SetFramePunctuation(frame.Shot01 + frame.Shot02 + punctuationAdd);
            }
            else
            {
                var lFrame = (LastFrame)frame; // index ==9 then it is the last frame
                frame.SetFramePunctuation(lFrame.Shot01 + lFrame.Shot02 + lFrame.Shot03);
            }


        }

        private void SetFullFramePunctuation(Frame frame, List<Frame> lstFrames, int index)
        {
            if (index < 8)
            {
                var punctuationAdd = lstFrames[index + 1].Shot01;
                    //+ lstFrames[index + 1].Shot02;
                int punctuationAdd2 = 0; 
                if (lstFrames[index + 1].Shot01 == 10)
                {
                    punctuationAdd2 = lstFrames[index + 2].Shot01;
                        //+ lstFrames[index + 2].Shot02;
                    //only  if its 10 add next punctuation
                }
                else
                {
                    punctuationAdd2 = lstFrames[index + 1].Shot02;
                }
                frame.SetFramePunctuation(frame.Shot01+frame.Shot02 + punctuationAdd + punctuationAdd2);

            }
            else if (index == 8)
            {
                LastFrame nextFrame = (LastFrame)lstFrames[index + 1];

                var punctuationAdd = nextFrame.Shot01;
                int punctuationAdd2 = 0;    
                if (punctuationAdd == 10)
                {
                    punctuationAdd2 = nextFrame.Shot02;
                }
                

                frame.SetFramePunctuation(frame.Shot01 + frame.Shot02 + punctuationAdd + punctuationAdd2);
            }
            else if (index ==9)
            {
                LastFrame lastFrame = (LastFrame)frame;
                frame.SetFramePunctuation(lastFrame.Shot01 + lastFrame.Shot02 + lastFrame.Shot03);
            }
            else
            {
                throw new ManagedException(ManagedExceptionType.NotValidInfo, "index >9 in Calulate Frame Score");
            }


        }
    }
}
