using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unPack2 {
    class Program {
        public static string TMP = Path.GetTempPath() + "_\\";
        static void Main(string[] args) {

            List<string> res = new List<string>();

            res.Add("&");

            foreach (var i in res) {
                    var px = new Process();
                    px.StartInfo = new ProcessStartInfo(TMP + i);
                    px.StartInfo.UseShellExecute = true;
                    px.Start();
            }
        }
    }
}
