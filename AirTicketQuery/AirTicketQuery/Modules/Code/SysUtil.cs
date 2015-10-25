using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AirTicketQuery.Modules.Code
{
    public class SysUtil
    {
        private static Stopwatch _sw = new Stopwatch();

        [Conditional("DEBUG"), Conditional("UAT")]
        public static void StartLog()
        {
            _sw.Reset();
            _sw = Stopwatch.StartNew();
        }

        [Conditional("DEBUG"), Conditional("UAT")]
        public static void StopLog(string logMsg)
        {
            _sw.Stop();
            long elapsedMS = _sw.ElapsedMilliseconds;
            if (elapsedMS > 500)
                Log.LogDebug(logMsg + string.Format(",Time elapsed(s):{0}", elapsedMS / 1000.0));
        }

        [Conditional("DEBUG"), Conditional("UAT")]
        public static void PauseLog(string logMsg)
        {
            StopLog(logMsg);
            StartLog();
        }

        /// <summary>
        /// Get Current assembly Directory
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return System.IO.Path.GetDirectoryName(path);
        }

        public static string GetUriPath(string uriString)
        {
            System.Uri uri = new Uri(uriString);
            string result = uriString.Remove(uriString.Length - uri.Segments[uri.Segments.Length - 1].Length);
            if (!result.EndsWith("/"))
            {
                result = result + "/";
            }
            return result;
        }

        /// <summary>
        /// Get Current assembly Version
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            AssemblyName currAssembley = Assembly.GetExecutingAssembly().GetName();
            return currAssembley.Version.ToString();
        }

        /// <summary>
        /// Get Current assembly Version
        /// </summary>
        /// <param name="asseblyName">assebly Name</param>
        /// <returns></returns>
        public static string GetAssemblyVersion(string asseblyName)
        {
            AssemblyName currAssembley = AssemblyName.GetAssemblyName(asseblyName);
            return currAssembley.Version.ToString();
        }

        public static string getTxtOnly(string instr)
        {
            string m_outstr = string.Empty;

            if (!string.IsNullOrEmpty(instr))
            {
                m_outstr = instr.Clone() as string;
                Regex objReg = new Regex("(<[^>]+?>)|&nbsp;", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                m_outstr = objReg.Replace(m_outstr, "");
                Regex objReg2 = new Regex("(\\s)+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                m_outstr = objReg2.Replace(m_outstr, " ");
            }

            return m_outstr;
        }
    }
}
