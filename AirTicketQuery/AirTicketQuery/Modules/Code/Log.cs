namespace AirTicketQuery.Modules.Code
{
    using System;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    /// <summary>
    /// Summary description for Log
    /// </summary>
    public class Log
    {
        ////<!-- OFF, FATAL, ERROR, WARN, INFO, DEBUG, Trace -->  
        private static Log logUtil;
        private static object obj = new object();
        private string logFile = System.IO.Path.Combine(SysUtil.GetAssemblyDirectory(), "logs/AirTicketQuery.log");
        private Logger mlog;
        private Logger logIns
        {
            get
            {
                if (this.mlog == null)
                {
                    this.InitialiseLogger(this.logFile, NLog.LogLevel.Trace);
                }

                return this.mlog;
            }
        }

        private static Log INS
        {
            get
            {
                if (logUtil == null)
                    logUtil = new Log();
                return logUtil;
            }
        }

        private void DoLogFormat(NLog.LogLevel pLogLevel, string strFormat, params object[] args)
        {
            lock (obj)
            {
                this.logIns.Log(pLogLevel, strFormat, args);
            }
        }

        private void DoLog(NLog.LogLevel pLogLevel, string message)
        {
            this.DoLog(pLogLevel, message, null);
        }

        private void DoLog(NLog.LogLevel pLogLevel, string message, Exception ex)
        {
            lock (obj)
            {
                if (ex == null)
                    this.logIns.Log(pLogLevel, message);
                else
                    this.logIns.LogException(pLogLevel, message, ex);
            }
        }

        private void InitialiseLogger(string filename, NLog.LogLevel logLevel)
        {
            try
            {
                LoggingConfiguration config = new LoggingConfiguration();
                string fileTargetName = "Log_File";
                FileTarget fileTarget = new FileTarget();
                config.AddTarget(fileTargetName, fileTarget);

                fileTarget.Header = "====Header===Start time = ${longdate} Machine = ${machinename} Product version = ${gdc:item=version}\r\n";
                fileTarget.Footer = "====Footer===End time =${longdate} \r\n";

                fileTarget.FileName = filename;
                fileTarget.ArchiveAboveSize = 5 * 1024 * 1024; // max file size - 5*1024*1024 = 5MB
                fileTarget.Layout = "[Begin]${longdate} [${level}] ${callsite:includeSourcePath=true} ${message} ${exception:format=ToString,StackTrace} ${newline}";

                LoggingRule ruleFile = new LoggingRule("*", logLevel, fileTarget);
                config.LoggingRules.Add(ruleFile);
                LogManager.Configuration = config;

                this.mlog = LogManager.GetLogger(fileTargetName);
            }
            catch
            {
            }
        }

        public static void LogErr(string message)
        {
            LogErr(message, null);
        }

        public static void LogErr(Exception ex)
        {
            LogErr(string.Empty, ex);
        }

        public static void LogErr(string message, Exception ex)
        {
            Log.INS.DoLog(LogLevel.Error, message, ex);
        }

        public static void LogInfo(string message)
        {
            Log.INS.DoLog(LogLevel.Info, message);
        }

        public static void LogInfoFormat(string strFormat, params object[] args)
        {
            Log.INS.DoLogFormat(LogLevel.Info, strFormat, args);
        }

        public static void LogWarn(string message)
        {
            Log.INS.DoLog(LogLevel.Warn, message);
        }

        public static void LogWarnFormat(string strFormat, params object[] args)
        {
            Log.INS.DoLogFormat(LogLevel.Warn, strFormat, args);
        }

        public static void LogDebug(string message)
        {
            Log.INS.DoLog(LogLevel.Debug, message);
        }

        public static void LogDebugFormat(string strFormat, params object[] args)
        {
            Log.INS.DoLogFormat(LogLevel.Debug, strFormat, args);
        }
    }
}