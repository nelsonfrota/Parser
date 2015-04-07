using System;
using System.Collections.Generic;

namespace Parser
{
    class Game
    {
        public int TotalKills { get; set; }
        public List<string> Players { get; set; }
        public Dictionary<String, Int32> PlayerKills { get; set; }        
    }
}
