using Logic.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    /// <summary>
    /// Represents a Game
    /// a Game has a Player and a list of Frames
    /// This class manages all the score of its frames and its own score
    /// </summary>
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
    
        /// <summary>
        /// Generate scores
        /// </summary>
        public void Calculate()
        {
            this.CalculateScores();
            this.CalculateCurrentScores();
        }


        /// <summary>
        /// Get all the "Pinfalls" Text row
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string GetPinfallsText(char separator)
        {

            StringBuilder sb = new StringBuilder();
            this.LstFrames.OrderBy(f => f.Ordinal).ToList().ForEach(f1 => { sb.Append(f1.GetPrintablePinfalls(separator)
                ); });
            return sb.ToString();
        }

        /// <summary>
        /// Get all the Score Text row
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string GetScoreText(char separator)
        {
            StringBuilder sb = new StringBuilder();
            this.LstFrames.OrderBy(f => f.Ordinal).ToList().ForEach(f1 => { sb.Append(f1.GetPrintableScore(separator)); }); ;
            return sb.ToString();
        }


        /// <summary>
        /// Calculate score for each frame
        /// </summary>
        /// <exception cref="ManagedException"></exception>
        private void CalculateScores()
        {
            int index = 0;
            foreach (var frame in LstFrames.OrderBy(f => f.Ordinal))
            {

                switch (frame.GetSpareType())
                {
                    case SpareType.Full:
                        {
                            SetFullFrameScore (frame, LstFrames, index);
                        }
                        break;
                    case SpareType.Half:
                        {
                            SetHalfFrameScore(frame, LstFrames, index);
                        }
                        break;
                    case SpareType.None:
                        {
                            SetFrameScore(frame, index);        
                        }
                        break;
                    default:
                        throw new ManagedException(ManagedExceptionType.NotValidInfo, "No valid spare type");
                        
                }
                index++;

            }


        }


        /// <summary>
        /// Calculate Current Scores for each frame
        /// </summary>
        private void CalculateCurrentScores()
        {
            foreach (var frame in LstFrames)
            {
                frame.SetCurrentPunctuation(GetRecursiveScore(LstFrames, frame.Ordinal));    
            }

            this.Score = LstFrames.LastOrDefault().CurrentPunctuation;
        }

        /// <summary>
        /// Calculate Recursive Current score 
        /// </summary>
        /// <param name="lstFrames">list for frames</param>
        /// <param name="index"> current index</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets "None" Frame score
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="index"></param>
        private void SetFrameScore(Frame frame, int index)
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

        /// <summary>
        /// Sets score for a Half Spare Frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="lstFrames"></param>
        /// <param name="index"></param>
        private void SetHalfFrameScore(Frame frame, List<Frame> lstFrames, int index)
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

        /// <summary>
        /// Sets Score for a Full Spare Frame
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="lstFrames"></param>
        /// <param name="index"></param>
        /// <exception cref="ManagedException"></exception>
        private void SetFullFrameScore(Frame frame, List<Frame> lstFrames, int index)
        {
            if (index < 8)
            {
                var punctuationAdd = lstFrames[index + 1].Shot01;
                    
                int punctuationAdd2 = 0; 
                if (lstFrames[index + 1].Shot01 == 10)
                {
                    punctuationAdd2 = lstFrames[index + 2].Shot01;
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
