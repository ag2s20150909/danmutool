using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DanMuTool
{
    class CommonTool
    {
        public static int AverageRandom(int old)
        {
            if (old + 80 < 1040)
            {
                return old + 80;
            }
            else
            {
                return 40;
            }
        }
        public static void SaveDanmuList(List<Danmu> data,string name)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(name+".ass");
            string s = "[Script Info]\nScriptType: v4.00+\nPlayResX: 1920\nPlayResY: 1080\n"
            + "\n[V4+ Styles]\nFormat: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding\n"
            + "Style: Default,Microsoft YaHei,64,&H00FFFFFF,&H00FFFFFF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,1,0,4,20,20,20,134\n"
            + "\n[Events]\nFormat: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";
            sw.Write(s);
            int pos = 40;
            foreach (Danmu d in data)
            {
                pos = CommonTool.AverageRandom(pos);
                string color = CommonTool.RBG2GBR(d.color);
                string time1 = CommonTool.secondToAssTime(d.time);
                string time2 = CommonTool.secondToAssTime(d.time+10);
                string line = "Dialogue: 0," + time1 + "," + time2 + ",Default,,0,0,0,,{\\move(1920,"+pos+",-1920,"+pos+")\\c&H"+color+"&}"+d.content+"\n";
                sw.Write(line);
                //ss = "{\\move(1920,%s,-1920,%s)\c&H%s&}" % (pos,pos,RGB2BGR(color))
                //line = "Dialogue: 0,%s,%s,Default,,0,0,0,,%s\n" % (sttime,edtime,ss + content)
                //Console.WriteLine("时间:" + d.time + "S颜色:" + d.color + "内容:" + d.content);
                //sw.Write("时间:" + d.time + "S颜色:" + d.color + "内容:" + d.content + "\n");
            }
            sw.Close();
        }
        public static int praseTimeString(string time)
        {
            int seconds = 0;
            string[] data = time.Split(':');
            for(int i = data.Length; i > 0; i--)
            {
                Console.WriteLine(data.Length-i);
                if (data.Length-i == 0)
                {
                    int temp = 0;
                    int.TryParse(data[i - 1], out temp);
                    seconds += temp;
                }else if (data.Length - i == 1)
                {
                    int temp = 0;
                    int.TryParse(data[i - 1], out temp);
                    seconds += temp*60;
                }
                else if (data.Length - i == 2)
                {
                    int temp = 0;
                    int.TryParse(data[i - 1], out temp);
                    seconds += temp * 3600;
                }
               
            }
            Console.WriteLine(seconds);
            return seconds;
        }
        /**
         * rgb颜色转化为ASS字幕使用的bgr格式
         * 
         */
        public static string RBG2GBR(string rgb)
        {
            if (rgb.Length != 6) { return rgb; }
            char[] d = rgb.ToCharArray();
            string bgr = d[4].ToString() + d[5].ToString() + d[2].ToString() + d[3] .ToString()+ d[0].ToString()+d[1].ToString();
            return bgr;
        }
        /**
         * 秒转化为ass格式的时间
         * 
         */
        public static string secondToAssTime(int second)
        {
            int s = second % 60;
            int m = (second % 3600) / 60;
            int h = second / 3600;
            Console.WriteLine(h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2"));
            return h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
        }
        public static void printDanmuList(List<Danmu> data)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter("names.txt");

            foreach (Danmu d in data)
            {
                Console.WriteLine("时间:"+d.time + "S颜色:" + d.color+"内容:"+d.content);
                sw.Write("时间:" + d.time + "S颜色:" + d.color + "内容:" + d.content+"\n");
            }
            sw.Close();
        }
        /***
         * 通过正则表达式从字符串中寻找匹配字符串
         */
        public static string regFind(string str,string reg)
        {
            if(System.Text.RegularExpressions.Regex.Matches(str, reg).Count < 1)
            {
                return "empty";
            }
            return System.Text.RegularExpressions.Regex.Matches(str, reg)[0].Groups[1].Value;
        }


        /***
         * HTTP get
         * */
        public static string get(string url)
        {
            //string url = "https://v.qq.com/x/cover/mzc002001kt6n30/k0033zdd4ug.html";
            System.Net.HttpWebRequest request;
            // 创建一个HTTP请求
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method="get";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;
        }

        /***
         * HTTP get
         * */
        public static string getBytes(string url)
        {
            //string url = "https://v.qq.com/x/cover/mzc002001kt6n30/k0033zdd4ug.html";
            System.Net.HttpWebRequest request;
            // 创建一个HTTP请求
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            var memoryStream = new System.IO.MemoryStream();
            //将基础流写入内存流
            const int bufferLength = 1024;
            byte[] buffer = new byte[bufferLength];
            int actual = response.GetResponseStream().Read(buffer, 0, bufferLength);
            if (actual > 0)
            {
                memoryStream.Write(buffer, 0, actual);
            }
            memoryStream.Position = 0;
            byte[] bArr = StreamToBytes(memoryStream);

            SharpLZW.LZWDecoder decoder = new SharpLZW.LZWDecoder();
            return decoder.DecodeFromCodes(bArr);
            //System.IO.StreamReader myreader = new System.IO.StreamReader(memoryStream, Encoding.UTF8);
            //string responseText = myreader.ReadToEnd();
            //myreader.Close();
            //return responseText;
        }

        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(System.IO.Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">原始流</param>
        /// <param name="output">目标流</param>
        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }
        
    }
}
