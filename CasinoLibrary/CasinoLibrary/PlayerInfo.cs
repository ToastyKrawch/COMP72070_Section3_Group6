using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoLibrary
{
    public class PlayerInfo
    {
        public int bet { get; set; }
        public int balance { get; set; }

        public PlayerInfo() 
        { 
            bet = 0;
            balance = 1000;
        }
    }
}
