using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
namespace LFNet.BdMovie
{
    public class Class1:System.Web.IHttpModule
    {
        public void Init(HttpApplication context)
        {
           //context.Context.Server.t
        }

        public void Dispose()
        {
           
        }
    }

    /// <summary>
    /// 文件跟踪复制器
    /// </summary>
    public class FileMonitorCopier
    {
        public string File { get; set; }
        public string DestFile { get; set; }
        private long Position = 0L;

        public FileMonitorCopier(string file, string destFile)
        {
            File = file;
            DestFile = destFile;
        }

        public void Copy()
        {
            FileStream fileStream=new FileStream(File,FileMode.Open,FileAccess.Read);
            fileStream.Position = Position;
            byte[] data = new byte[4096*256]; //4kb*256
            int l=  fileStream.Read(data, 0, data.Length);
            int lastdataPos =-1;//初始为数组外面
            for (int i = l-1; i >=0; i--)
            {
                if (data[i] != 0x0)
                {
                    lastdataPos = i;
                    break;
                }
            }

            if (lastdataPos > -1) //发现有效数据
            {
                FileStream desStream=new FileStream(DestFile,FileMode.Append,FileAccess.Write);
                desStream.Position = Position;
                desStream.Write(data, 0, lastdataPos + 1);
                desStream.Close();
                data = null;
                Position += lastdataPos+1;
            }
        }
        
    }

    
}
