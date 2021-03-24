using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Handin2_LAdeskab.Classes
{
    public class LogDisplay : ILogDisplay
    {
        private string logFile = "logfile.txt"; // Navnet på systemets log-fil
        public void PrintToLog(string logMsg)
        {
            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(logFile))
            {
                sw.WriteLine(logMsg);
            }

        }
    }
}
