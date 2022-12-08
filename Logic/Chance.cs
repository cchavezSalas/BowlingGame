using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    /// <summary>
    /// Basic Type
    /// every line in input file maps to this type
    /// </summary>
    public class Chance
    {
        
        public int ChanceOrder { get; set; }
        public string Name { get; set; }
        public int PinsKnocked { get; set; }

    }


}
