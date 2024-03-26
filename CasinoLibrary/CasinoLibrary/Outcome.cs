using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoLibrary
{
    enum Colour { red, black, green };
    enum Type { odd, even, zero };
    
    internal class Outcome
    {
        public int number;
        public Colour colour;
        public Type type;

        public Outcome(int resultNum)
        {
            number = resultNum;
            determineColour();
        }

        internal void setOutcome(int resultNum)
        {
            number = resultNum;
            determineColour();
        }

        void determineColour()
        {
            if (number == 0)
            {
                colour = Colour.green;
                type = Type.zero;
            }
            else if (number / 2 == 1)
            {
                colour = Colour.red;
                type = Type.even;
            }
            else
            {
                colour = Colour.black;
                type = Type.odd;
            }
        }
    }
}
