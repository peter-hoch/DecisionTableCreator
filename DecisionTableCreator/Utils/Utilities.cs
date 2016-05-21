using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.Utils
{
    public class Utilities
    {
        public static int AtLeastValue(int atLeast, int value)
        {
            if (atLeast > value)
            {
                return atLeast;
            }
            return value;
        }
    }
}
