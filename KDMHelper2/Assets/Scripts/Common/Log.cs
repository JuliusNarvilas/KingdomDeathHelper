#if DEBUG_LOGS_OFF
    #undef DEBUG_LOGS
#elif UNITY_EDITOR || DEBUG || DEBUG_LOGS
    #define DEBUG_LOGS
#else
    #define DEBUG_LOGS_OFF
#endif

#if PRODUCTION_LOGS_OFF
    #undef PRODUCTION_LOGS
#else
    #define PRODUCTION_LOGS
#endif

using System.Diagnostics;

namespace Common
{
    /// <summary>
    /// Class for 
    /// </summary>
    public static class Log
    {
        public delegate void LoggerOutputLogTargetFunc(string i_Message, params object[] i_Args);
        public delegate void LoggerOutputAssertTargetFunc(bool i_Assert, string i_Message, params object[] i_Args);

        //Default assert function to bypass Unity overloaded function group assignment problems
        private static void DefaultAssertFunc(bool i_Assert, string i_Message, params object[] i_Args)
        {
            UnityEngine.Debug.AssertFormat(i_Assert, i_Message, i_Args);
        }

        /// <summary>
        /// Collection of logging callbacks for a particular build version
        /// </summary>
        public class LogStream
        {
            public LoggerOutputLogTargetFunc m_Log;
            public LoggerOutputLogTargetFunc m_Warning;
            public LoggerOutputLogTargetFunc m_Error;
            public LoggerOutputAssertTargetFunc m_Assert;

            public LogStream(LoggerOutputLogTargetFunc i_Log, LoggerOutputLogTargetFunc i_Warning, LoggerOutputLogTargetFunc i_Error, LoggerOutputAssertTargetFunc i_Assert)
            {
                m_Log = i_Log;
                m_Warning = i_Warning;
                m_Error = i_Error;
                m_Assert = i_Assert;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Debug
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Debug only log output for infomration.
        /// </summary>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("DEBUG_LOGS")]
        public static void DebugLog(string i_Message, params object[] i_Args)
        {
            Debug.m_Log(i_Message, i_Args);
        }

        /// <summary>
        /// Debug only log output for infomration with built in condition check.
        /// </summary>
        /// <param name="i_Condition">if set to <c>true</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("DEBUG_LOGS")]
        public static void DebugLogIf(bool i_Condition, string i_Message, params object[] i_Args)
        {
            if (i_Condition)
            {
                DebugLog(i_Message, i_Args);
            }
        }

        /// <summary>
        /// Debug only log output for warnings.
        /// </summary>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("DEBUG_LOGS")]
        public static void DebugLogWarning(string i_Message, params object[] i_Args)
        {
            Debug.m_Warning(i_Message, i_Args);
        }

        /// <summary>
        /// Debug only log output for warnings with built in condition check.
        /// </summary>
        /// <param name="i_Condition">if set to <c>true</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("DEBUG_LOGS")]
        public static void DebugLogWarningIf(bool i_Condition, string i_Message, params object[] i_Args)
        {
            if(i_Condition)
            {
                DebugLogWarning(i_Message, i_Args);
            }
        }

        /// <summary>
        /// Debug only log output for errors.
        /// </summary>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("DEBUG_LOGS")]
        public static void DebugLogError(string i_Message, params object[] i_Args)
        {
            Debug.m_Error(i_Message, i_Args);
        }

        /// <summary>
        /// Debug only log output for errors with built in condition check.
        /// </summary>
        /// <param name="i_Condition">if set to <c>true</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("DEBUG_LOGS")]
        public static void DebugLogErrorIf(bool i_Condition, string i_Message, params object[] i_Args)
        {
            if (i_Condition)
            {
                DebugLogError(i_Message, i_Args);
            }
        }

        /// <summary>
        /// Debug only log output for asserts.
        /// </summary>
        /// <param name="i_Assertion">if set to <c>false</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("DEBUG_LOGS")]
        public static void DebugAssert(bool i_Assertion, string i_Message, params object[] i_Args)
        {
            Debug.m_Assert(i_Assertion, i_Message, i_Args);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Production
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Log output for information that should occur even in production build.
        /// </summary>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("PRODUCTION_LOGS")]
        public static void ProductionLog(string i_Message, params object[] i_Args)
        {
            Production.m_Log(i_Message, i_Args);
        }

        /// <summary>
        /// Log output for information with built in condition check that should occur even in production build.
        /// </summary>
        /// <param name="i_Condition">if set to <c>true</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("PRODUCTION_LOGS")]
        public static void ProductionLogIf(bool i_Condition, string i_Message, params object[] i_Args)
        {
            if (i_Condition)
            {
                ProductionLog(i_Message, i_Args);
            }
        }

        /// <summary>
        /// Log output for warnings that should occur even in production build.
        /// </summary>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("PRODUCTION_LOGS")]
        public static void ProductionLogWarning(string i_Message, params object[] i_Args)
        {
            Production.m_Warning(i_Message, i_Args);
        }

        /// <summary>
        /// Log output for warnings with built in condition check that should occur even in production build.
        /// </summary>
        /// <param name="i_Condition">if set to <c>true</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("PRODUCTION_LOGS")]
        public static void ProductionLogWarningIf(bool i_Condition, string i_Message, params object[] i_Args)
        {
            if (i_Condition)
            {
                ProductionLogWarning(i_Message, i_Args);
            }
        }


        /// <summary>
        /// Log output for errors that should occur even in production build.
        /// </summary>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("PRODUCTION_LOGS")]
        public static void ProductionLogError(string i_Message, params object[] i_Args)
        {
            Production.m_Error(i_Message, i_Args);
        }

        /// <summary>
        /// Log output for errors with built in condition check that should occur even in production build.
        /// </summary>
        /// <param name="i_Condition">if set to <c>true</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("PRODUCTION_LOGS")]
        public static void ProductionLogErrorIf(bool i_Condition, string i_Message, params object[] i_Args)
        {
            if (i_Condition)
            {
                ProductionLogError(i_Message, i_Args);
            }
        }


        /// <summary>
        /// Log output for asserts that should occur even in production build.
        /// </summary>
        /// <param name="i_Assertion">if set to <c>false</c> the log is produced.</param>
        /// <param name="i_Message">The message.</param>
        /// <param name="i_Args">The arguments.</param>
        [Conditional("PRODUCTION_LOGS")]
        public static void ProductionAssert(bool i_Assertion, string i_Message, params object[] i_Args)
        {
            Debug.m_Assert(i_Assertion, i_Message, i_Args);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// The default log output function sets.
        /// </summary>
        private static readonly LogStream Default = new LogStream(
            UnityEngine.Debug.LogFormat, UnityEngine.Debug.LogWarningFormat, UnityEngine.Debug.LogErrorFormat, DefaultAssertFunc
        );


        /// <summary>
        /// Option to specify debug log output function sets.
        /// </summary>
        private static LogStream Debug = Default;
        /// <summary>
        /// Option to specify production log output function sets.
        /// </summary>
        private static LogStream Production = Default;
    }
}
