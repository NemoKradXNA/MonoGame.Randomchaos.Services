
using System;
using System.IO;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Utilities
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A logger. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class Logger
    {
        /// <summary>   Name of the log. </summary>
        public static string LogName = "MonoGame.Randomchaos.ContentPipelineExtensions.log";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method to write to log file. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="data"> . </param>
        ///-------------------------------------------------------------------------------------------------

        public static void WriteToLog(string data)
        {
            StreamWriter sw = new StreamWriter(LogName, true);
            sw.WriteLine(string.Format("[{0:dd-MM-yyyy HH:mm:ss}] - {1}", DateTime.Now, data));
            sw.Close();
        }
    }
}
