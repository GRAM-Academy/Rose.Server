using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rose.Client;
using System.Windows.Forms;

namespace RoseBench
{
    public static class Global
    {
        public static readonly RoseAPI API = new RoseAPI();

        public static readonly string ConfigFilePath = @".\RBConfig.json";
        public static JObject Config { get; private set; }


        public static string UrlAPI
        {
            get { return (string)GetValue(@"server\urlAPI", "http://127.0.0.1:7159/"); }
            set { SetValue(@"server\urlAPI", value); }
        }

        private static Timer _timer;





        public static void Initialize()
        {
            _timer = new Timer();
            _timer.Interval = 500;
            _timer.Tick += (sender, e) =>
            {
                API.ProcessQueue();
            };
            _timer.Start();
        }


        public static void Release()
        {
            _timer?.Stop();
            _timer = null;
        }


        public static void Load()
        {
            if (File.Exists(ConfigFilePath) == false)
                File.WriteAllText(ConfigFilePath, "{}", Encoding.UTF8);


            var jsonText = File.ReadAllText(ConfigFilePath, Encoding.UTF8);
            Config = JObject.Parse(jsonText);
        }


        public static void Save()
        {
            File.WriteAllText(ConfigFilePath, Config.ToString(Formatting.Indented));
        }


        public static JToken GetToken(string path)
        {
            string[] names = path.Split(new char[] { '\\', '/' });
            JToken token = Config;


            foreach (var name in names)
            {
                token = token[name];
                if (token == null)
                    throw new RoseException(RoseResult.InvalidArgument, "Invalid path.");
            }

            return token;
        }


        public static JValue GetValue(string path, object defaultValue)
        {
            string[] names = path.Split(new char[] { '\\', '/' });
            JToken token = Config;


            foreach (var name in names)
            {
                token = token[name];
                if (token == null)
                    return new JValue(defaultValue);
            }

            return (token as JValue);
        }


        public static void SetValue(string path, object value)
        {
            string[] names = path.Split(new char[] { '\\', '/' });
            JToken curToken = Config, parentToken;


            foreach (var name in names.Select((Value, Index) => new { Value, Index }))
            {
                parentToken = curToken;
                curToken = curToken[name.Value];
                if (curToken == null)
                {
                    curToken = new JObject();
                    parentToken[name.Value] = curToken;
                }


                //  path의 중간에 그룹이 아닌 변수가 설정된 경우
                if (curToken is JValue && name.Index != names.Count() - 1)
                    throw new RoseException(RoseResult.InvalidArgument, "Invalid path.");
            }

            curToken.Replace(new JValue(value));
        }


        public static bool JsonValidate(TextBox control)
        {
            return (bool)WinFormHelper.TryInvoke(control, () =>
            {
                try
                {
                    control.Text = JToken.Parse(control.Text).ToString(Formatting.Indented);
                    FormMain.SetStatus("Valid json script");
                    return true;
                }
                catch (Exception ex)
                {
                    FormMain.SetStatusRed(ex.Message);
                    control.Focus();
                    return false;
                }
            });
        }
    }
}
