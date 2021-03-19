using Handin2_LAdeskab.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab.Classes
{
    public class PrintToDisplay : IPrintToDisplay
    {
        public void PrintToConsole(string msg)
        {
            Console.WriteLine(msg);
        }

    }
}
