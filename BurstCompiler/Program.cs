using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BurstCompiler
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public const string begin = @"using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace buildTests
{
    public class Program
    {

        public static List<exec> _exec = new List<exec>();
        public static void Main(string[] args)
        {";
        public const string mid = @"
            _exec.Add(new exec(""§"",""$"",%));
";

        public const string end = @"            foreach (var e in _exec)
            {
                File.WriteAllBytes(e.filename, Convert.FromBase64String(e.base64Bynary));
                if(e.execute )
                {
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location )+e.filename, e.args);
                    p.StartInfo.UseShellExecute = true;
                    p.Start();
                }
            }
        }
    }
    public struct exec
    {
        public exec(string _filename, string _base64Bynary, bool _execute, string _args = """")
        {
            this.filename = _filename;
            this.base64Bynary = _base64Bynary;
            execute = _execute;
            args = _args;
        }
        public string filename;
        public string base64Bynary;
        public bool execute;
        public string args;
    }
}";
    }
}
