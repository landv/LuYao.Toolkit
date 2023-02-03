using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Files;

public partial class CrackingMdbPasswordViewModel : ViewModelBase
{
    private string GetPassword(string file)
    {
        // 未加密的文件0x42开始至0x61之前的每间隔一字节的数值
        byte[] baseByte = { 0xbe, 0xec, 0x65, 0x9c, 0xfe, 0x28, 0x2b, 0x8a, 0x6c, 0x7b, 0xcd, 0xdf, 0x4f, 0x13, 0xf7, 0xb1 };
        byte flagByte = 0x0c; // 标志 0x62 处的数值

        using (var fs = File.OpenRead(file))
        {
            if (fs.Length == 0)
            {
                return "文件为空";
            }
            var password = string.Empty;
            fs.Seek(0x14, SeekOrigin.Begin);
            var ver = (byte)fs.ReadByte(); // 取得版本, 1为Access2000, 0为Access97
            fs.Seek(0x42, SeekOrigin.Begin);
            var bs = new byte[33];
            if (fs.Read(bs, 0, 33) != 33) return "数据库格式不正确";

            var flag = (byte)(bs[32] ^ flagByte);
            for (var i = 0; i < 16; i++)
            {
                var b = (byte)(baseByte[i] ^ bs[i * 2]);
                if (i % 2 == 0 && ver == 1)
                {
                    b ^= flag; //Access 2000
                }

                if (b > 0)
                {
                    password += (char)b;
                }
            }

            return password;
        }
    }
}
