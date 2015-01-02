using System;
using System.IO;
using LFNet.BdMovie;

namespace BdMovie
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Start:
            Console.Write("请输入目录(当前目录请敲回车)：");
            try
            {
                string path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                {
                    path = AppDomain.CurrentDomain.BaseDirectory;
                }
                string[] files = Directory.GetFiles(path);
                if (files.Length == 0)
                {
                    Console.WriteLine("该目录没有任何文件。");
                    goto Start;
                }

                int i = 0;
                foreach (string file in files)
                {
                    Console.WriteLine("{0}.{1}", ++i, Path.GetFileName(file));
                }

                Console.Write("\n请输入你要处理的文件序号:");
                i = int.Parse(Console.ReadLine());
                string f = files[i - 1];
                Console.Write("\n请输入文件格式：");
                string ext = Console.ReadLine();
                string destFile = "";
                if (f.Contains(ext))
                {
                    destFile = f.Substring(0, f.IndexOf(ext) + ext.Length);
                    destFile = Path.Combine(Path.GetDirectoryName(destFile), "temp/", Path.GetFileName(destFile));
                }
                else
                {
                    destFile = Path.Combine(Path.GetDirectoryName(f), "temp/",
                        Path.GetFileNameWithoutExtension(f) + '.' + ext.TrimStart('.'));
                }
                ;
                string d = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(d))
                    Directory.CreateDirectory(d);
                new FileMonitorCopier(f, destFile).Copy();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            goto  Start;
        }
    }
}