using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    /// <summary>
    /// Groups a player with his/her raw chances (given from input)
    /// </summary>
    public class PlayerSet
    {

        public string PlayerName { get; set; }
        public List<Chance> Chances { get; set; }

    }
}
