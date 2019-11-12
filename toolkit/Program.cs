using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace toolkit
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// 

        public static readonly string TMP = Path.GetTempPath() + "#TOOLBOX#\\";


        static readonly string IDENTY01 = string.Join("-", Enumerable.Range(0, 100).ToArray());
        static readonly string IDENTY02 = string.Join("#", Enumerable.Range(0, 100).ToArray());
        [STAThread]
        static void Main()
        {
            Directory.CreateDirectory(TMP);
            File.WriteAllBytes(TMP + "7z.exe", Resource1._7z_exe);
            File.WriteAllBytes(TMP + "7z.dll", Resource1._7z_dll);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
                       
        }
        public static FileStream ne_file;
        internal static string pack(List<Item> _ls, string arch)
        {
            string state = "";
            try
            {
                WRITE(ne_file, Resource1.unPack);
                WRITE(ne_file, encoding.GetBytes(IDENTY01));

                state = "Copied DePacker";

                string name = Path.GetTempPath() + new Random().Next(0, 100).ToString();
                var fs = File.Open(name, FileMode.OpenOrCreate);

                var serializer = new XmlSerializer(typeof(List<Item>));
                serializer.Serialize(fs, _ls);
                fs.Close();
                fs = File.OpenRead(name);
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);

                var xml_64 = Convert.ToBase64String(buffer);

                state = "Informations Generated";

                WRITE(ne_file, encoding.GetBytes(xml_64));

                WRITE(ne_file, encoding.GetBytes(IDENTY02));

                state = "Informations Copied";
                var a = File.OpenRead(arch);
                WRITE(ne_file, READ(a, (int)a.Length));
                state = "Data Writed";
                try
                {
                    a.Close();
                    ne_file.Close();
                    a.Dispose();
                    ne_file.Dispose();
                }
                catch { }
                state = "Files Colsed";
                File.Delete(arch);
                state = "TempFiles Deleated";
            }
            catch (Exception e)
            {
                return e.Message + "\n" + state;
            }
            return "Erfolgreich erstellt!\n";
        }

        private static void ReOpen()
        {
            var pos = ne_file.Position;
            var name = ne_file.Name;
            ne_file.Close();
            ne_file = File.Open(name, FileMode.Append);
            if (ne_file.Position != pos)
            {
                ne_file.Position = pos;
            }
        }

        public static Encoding encoding => Encoding.Unicode;

        static void WRITE(FileStream fs, byte[] buffer)
        {
            fs.Write(buffer, 0, (int)buffer.Length);
        }

        static byte[] READ(FileStream fs, int count)
        {
            byte[] buffer = new byte[count];
            var b = fs.Read(buffer, 0, count);

            var tmp = new byte[b];
            Array.Copy(buffer, 0, tmp, 0, b);
            return tmp;
        }
        public static string[] SevenZip = new string[] { "-bd -bb2 a \"%\" \"&\"", "-bd -bb2 -ao x \"%.7z\" -o\"&\"", "\"$\\7z.exe\"" };
    }



    [Serializable]
    public class Item
    {
        public Item(Int32 id, string Name)
        {
            this.Id = id;
            this.Name = Name;
        }
        public Item()
        {

        }

        public Int32 Id { get; set; }
        public String Name { get; set; }
    }

}
