using System;
using System.IO;
using System.Threading;

namespace LFNet.BdMovie
{
    /// <summary>
    ///     文件跟踪复制器
    /// </summary>
    public class FileMonitorCopier
    {
        private long Position;

        public FileMonitorCopier(string file, string destFile)
        {
            File = file;
            DestFile = destFile;
        }

        public string File { get; set; }
        public string DestFile { get; set; }

        public void Copy()
        {
            var fileStream = new FileStream(File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileStream.Position = Position;
            var data = new byte[4096*256]; //4kb*256
            int l = fileStream.Read(data, 0, data.Length);
            if (l == 0)
            {
                Console.WriteLine("复制完成");
                return;
            }
            int lastdataPos = -1; //初始为数组外面
            for (int i = l - 1; i >= 0; i--)
            {
                if (data[i] != 0x0)
                {
                    lastdataPos = i;
                    break;
                }
            }

            if (lastdataPos > -1) //发现有效数据
            {
                var desStream = new FileStream(DestFile, FileMode.OpenOrCreate, FileAccess.Write);
                desStream.Position = Position;
                desStream.Write(data, 0, lastdataPos + 1);
                desStream.Close();
                data = null;
                Position += lastdataPos + 1;
                Console.WriteLine("本次读取写入{0}，{1}/{2}", lastdataPos + 1, Position, fileStream.Length);
            }
            long len = fileStream.Length;
            fileStream.Close();
            if (Position <= len)
            {
                Thread.Sleep(10); //1s
                var thread = new Thread(Copy);
                thread.IsBackground = true;
                thread.Start();
            }
        }
    }
}