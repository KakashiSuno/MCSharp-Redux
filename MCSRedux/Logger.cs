using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;


namespace Minecraft_Server
{
    public class Logger
    {
        static PidgeonLogger _pidLog = new PidgeonLogger();

        public static void Write(string str) //Kept for backwards compatibility
        {
            _pidLog.LogMessage(str);
        }
        public static void WriteError(Exception ex)
        {
            _pidLog.LogError(ex);
        }
        public static string LogPath { get { return _pidLog.MessageLogPath; } set { _pidLog.MessageLogPath = value;} }
        public static string ErrorLogPath { get { return _pidLog.ErrorLogPath; } set { _pidLog.ErrorLogPath = value; } }

        //Everything is static..!
        public static void Dispose()
        {
            _pidLog.Dispose();
        }

    }
    /// <summary>
    /// Temporary class, will replace Logger completely once satisfied
    /// </summary>
    class PidgeonLogger : IDisposable
    {
        //TODO: Implement report back feature
        

        bool _disposed;
        bool _reportBack = false;
        string _messagePath;
        string _errorPath;
        object _lockObject = new object();
        Thread _workingThread;
        Queue<string> _messageCache = new Queue<string>();
        Queue<string> _errorCache = new Queue<string>(); //always handle this first!

        public PidgeonLogger()
        {
            _reportBack = Properties.reportBack;
            //Should be done as part of the config
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            _messagePath = "logs/" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".txt";
            _errorPath = "error.log";

            _workingThread = new Thread(new ThreadStart(WorkerThread));
            _workingThread.IsBackground = true;
            _workingThread.Start();
        }

        public string MessageLogPath
        {
            get { return _messagePath; }
            set { _messagePath = value; }
        }
        public string ErrorLogPath
        {
            get { return _errorPath; }
            set { _errorPath = value; }
        }

        public void LogMessage(string message)
        {
            if (_disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if (message!=null && message.Length > 0)
                lock (_lockObject) 
                {
                    _messageCache.Enqueue(message);
                    Monitor.Pulse(_lockObject);
                }
            
            //Should it error or passed null or zero string?
        }
        public void LogError(Exception ex)
        {
            if (_disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            StringBuilder sb = new StringBuilder();
            Exception e = ex;

            sb.AppendLine("----" + DateTime.Now + " ----");
            while (e != null)
            {
                sb.AppendLine(getErrorText(e));
                e = e.InnerException;
            }

            sb.AppendLine(new string('-', 25));
            lock (_lockObject) 
            { 
                _errorCache.Enqueue(sb.ToString());
                Monitor.Pulse(_lockObject);
            }
        }


        void WorkerThread()
        {
            while (!_disposed)
            {
                lock (_lockObject)
                {
                    if (_errorCache.Count > 0)
                        FlushCache(_errorPath, _errorCache);

                    if (_messageCache.Count > 0)
                        FlushCache(_messagePath, _messageCache);
                    Monitor.Wait(_lockObject, 500);
                }
            }
        }

        //Only call from within synchronised code or all hell will break loose
        void FlushCache(string path, Queue<string> cache)
        {
			FileStream fs = null;
			try
			{
				//TODO: not happy about constantly opening and closing a stream like this but I suppose its ok (Pidgeon)
				fs = new FileStream(path, FileMode.Append, FileAccess.Write);
				while (cache.Count > 0)
				{
					byte[] tmp = Encoding.Default.GetBytes(cache.Dequeue());
					fs.Write(tmp, 0, tmp.Length);
				}
				fs.Close();
			}
			catch
			{
				
			}
        }
        string getErrorText(Exception e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Type: " + e.GetType().Name);
            sb.AppendLine("Source: " + e.Source);
            sb.AppendLine("Message: " + e.Message);
            sb.AppendLine("Target: " + e.TargetSite.Name);
            sb.AppendLine("Trace: " + e.StackTrace);

            return sb.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                lock (_lockObject)
                {
                    if (_errorCache.Count > 0)
                    {
                        FlushCache(_errorPath, _errorCache);
                    }

                    _messageCache.Clear();
                    Monitor.Pulse(_lockObject);
                }
            }
        }

        #endregion
    }
}
