using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aegis;

namespace Rose.Server
{
    public static class LogMedia
    {
        private static TextBox _textBox;
        private static StreamWriter _textFile;





        public static void SetTextBoxLogger(TextBox tb)
        {
            if (tb == null)
            {
                Logger.Written -= TextBoxLog;
                _textBox = null;
                return;
            }

            _textBox = tb;
            Logger.Written += TextBoxLog;
        }


        public static void SetTextFileLogger(string path, string filePrefix)
        {
            if (path == null)
            {
                Logger.Written -= TextFileLog;
                _textFile = null;
                return;
            }

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            string filename = Path.Combine(path, string.Format("{0}{1}_{2:D2}{3:D2}.log",
                                                    filePrefix, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            _textFile = new StreamWriter(filename, true);
            Logger.Written += TextFileLog;
        }


        public static void SetOutputLogger(bool isSet)
        {
            if (isSet == false)
            {
                Logger.Written -= OutputLog;
                return;
            }

            Logger.Written += OutputLog;
        }


        public static void ReleaseAllLogger()
        {
            Logger.Written -= TextBoxLog;
            Logger.Written -= TextFileLog;
            Logger.Written -= OutputLog;


            _textBox = null;

            StreamWriter textFileStream = _textFile;
            textFileStream?.Close();
            _textFile = null;
        }


        private static void TextBoxLog(int mask, string log)
        {
            TextBox tb = _textBox;
            if (tb == null)
                return;


            Action action = () =>
            {
                tb.Text += $"{log}\r\n";
                tb.SelectionStart = tb.TextLength;
                tb.ScrollToCaret();
            };

            if (tb.InvokeRequired)
                tb.BeginInvoke(action);
            else
                action();
        }


        private static void TextFileLog(int mask, string log)
        {
            string text = string.Format("[{0}/{1} {2}:{3}:{4}] {5}",
                                    DateTime.Now.Month, DateTime.Now.Day,
                                    DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                                    log);

            _textFile?.WriteLine(text);
            _textFile?.Flush();
        }


        private static void OutputLog(int mask, string log)
        {
            string text = string.Format("[{0}/{1} {2}:{3}:{4}] {5}",
                                        DateTime.Now.Month, DateTime.Now.Day,
                                        DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                                        log);

            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}
