using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace unPack {
    class Program {
        static FileStream my_file;
        static FileStream ne_file;
        static readonly string IDENTY01 = string.Join("-", Enumerable.Range(0, 100).ToArray());
        static readonly string IDENTY02 = string.Join("#", Enumerable.Range(0, 100).ToArray());
        static byte[] buffer = new byte[parts];
        const int parts = 10000;
        const int EncodingLength = 2;
        static string[] SevenZip = new string[] { "-bd -bb2 a \"%.7z\" \"&\"", "-bd -bb2 x \"%\" -o\"&\"", "\"$\\7z.exe\"" };

        public static string EXE = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public static string TMP = Path.GetTempPath() + "_\\";

        static void Main(string[] args) {
            Directory.CreateDirectory(TMP);
            File.WriteAllBytes(Path.GetDirectoryName(TMP) + "\\" + "7z.exe", Resource1._7z_exe);
            File.WriteAllBytes(Path.GetDirectoryName(TMP) + "\\" + "7z.dll", Resource1._7z_dll);

            unPack();
        }

        private static void unPack() {
            Console.WriteLine("start");
            my_file = File.OpenRead(EXE);
            var pos1 = FinDString(my_file, IDENTY01);
            my_file.Position = pos1;
            var pos2 = FinDString(my_file, IDENTY02);

            Console.WriteLine("pos1: " + pos1);
            Console.WriteLine("pos2: " + pos2);
            my_file.Position = pos1;

            int XML_Length = pos2 - pos1 - IDENTY01.Length *2;


            string xml_64 = encoding.GetString(READ(my_file, XML_Length));
            var xml = Convert.FromBase64String(xml_64.Substring(0, xml_64.Length));

            XmlSerializer ser = new XmlSerializer(typeof(List<Item>));
            Console.WriteLine(Encoding.UTF8.GetString(xml));
            List<Item> res = (List<Item>)ser.Deserialize(new MemoryStream(xml));

            foreach (var item in res) {
                Console.WriteLine("EXE: " + item.Id + ", " + item.Name);
            }

            var rnd = new Random().NextDouble().ToString() + ".7z";
            ne_file = File.Create(TMP + rnd);


            Console.WriteLine("filename: " + rnd);


            my_file.Position = pos2;
            for (int i = 0; ; i++) {
                var bu = READ(my_file, parts);
                if (bu.Length <= 0) break;
                WRITE(ne_file, bu);
            }


            Console.WriteLine("uncomp");

            var arch = TMP + rnd;
            Process p = new Process();

            var s7Module = TMP;
            var s7Out = TMP;

            var arges = SevenZip[1].Replace("%", arch).Replace("&", s7Out);
            var exe = SevenZip[2].Replace("$", s7Module);

            if (my_file != null)
                my_file.Close();
            if (ne_file != null)
                ne_file.Close();

            p.StartInfo = new ProcessStartInfo(exe, arges);
            p.StartInfo.UseShellExecute = false;
            p.Start();

            p.WaitForExit();


            Console.WriteLine("exec");

            foreach (var i in res) {
                if (i.Id == 1) {
                    var px = new Process();
                    px.StartInfo = new ProcessStartInfo(TMP + i.Name);
                    px.StartInfo.UseShellExecute = true;
                    px.Start();
                }
            }
        }

        public static int FinDString(FileStream fs, string identifire, int startpos = 0) {
            fs.Position = startpos;
            int pos = 0;
            for (int i = 0; i < 10000; i++) {
                buffer = READ(fs, parts);

                var enc = encoding.GetString(buffer);
                if (enc.Contains(identifire)) {
                    pos = PatternAt(buffer, encoding.GetBytes(identifire)).First() + (parts * i) + identifire.Length * EncodingLength;

                    break;
                }
                if (i == 9999) {
                    Console.WriteLine("ERROR");
                    throw new NotFiniteNumberException();
                }
            }
            return pos;
        }
        static void WRITE(FileStream fs, byte[] buffer) { fs.Write(buffer, 0, (int) buffer.Length); }
        static byte[] READ(FileStream fs, int count) {
            byte[] buffer = new byte[count];
            var b = fs.Read(buffer, 0, count);

            var tmp = new byte[b];
            Array.Copy(buffer, 0, tmp, 0, b);
            return tmp;
        }
        public static Encoding encoding { get { return Encoding.Unicode; } }
        public static IEnumerable<int> PatternAt(byte[] source, byte[] pattern) {
            for (int i = 0; i < source.Length; i++) {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern)) {
                    yield return i;
                }
            }
        }
        public static int IndexOf(byte[] array, byte[] pattern, int offset) {
            int success = 0;
            for (int i = offset; i < array.Length; i++) {
                if (array[i] == pattern[success]) {
                    success++;
                } else {
                    success = 0;
                }

                if (pattern.Length == success) {
                    return i - pattern.Length + 1;
                }
            }
            return -1;
        }
    }
    public class Item {
        public Int32 Id { get; set; }

        public String Name { get; set; }
    }
}
