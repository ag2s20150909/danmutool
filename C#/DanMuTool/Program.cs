using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DanMuTool
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonTool.secondToAssTime(100);
            string s = "abcdef";
            Console.WriteLine(s);
            s=CommonTool.RBG2GBR(s);
            Console.WriteLine(s);
            s = "12:23";
            CommonTool.praseTimeString(s);
            TencentVideo.get("https://v.qq.com/x/cover/mzc002001kt6n30/k0033zdd4ug.html");
            //IqiyiVideo.get("https://www.iqiyi.com/v_19rxv1nwag.html");
            Console.ReadKey();
        }

       
    }
}
