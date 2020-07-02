using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanMuTool
{
    class IqiyiVideo
    {
        string title;
        string vid;
        string tgid;
        int duration;
        List<Danmu> danmus = new List<Danmu>();
        public static void get(string url)
        {
            IqiyiVideo video = new IqiyiVideo();
            video.danmus = new List<Danmu>();
            string html = CommonTool.get(url);
            video.vid = CommonTool.regFind(html, @"param\['tvid'\] = ""(.*)"";");
            video.title = CommonTool.regFind(html, @"<meta property=""og:title""content=""(.*?)""/>");
            string d = CommonTool.regFind(html, @"""duration"":""(.*?)""");
            Console.WriteLine(d);
            html = null;
            video.duration = CommonTool.praseTimeString(d);
            Console.WriteLine(video.vid + "\n" + video.title + "\n" + video.duration);
            video.getDanmu(video.vid, 1);
          
            //video.getTgrgetID(video.vid);
            //Console.WriteLine(video.tgid);
            //for (int i = 0; i < video.duration; i += 30)
            //{
             //   video.danmus.AddRange(video.getDanmu(video.tgid, i));
             //   System.Threading.Thread.Sleep(500);//睡眠500毫秒，也就是0.5秒
           // }
            //video.danmus.Sort();
            ////Console.WriteLine(video.danmus);
            //CommonTool.printDanmuList(video.danmus);
            //CommonTool.SaveDanmuList(video.danmus, video.title);
           //*/

            Console.ReadKey();

        }
        public  List<Danmu> getDanmu(string tvid,int time)
        {
            List<Danmu> ls = new List<Danmu>();
            string s1 = tvid.Substring(tvid.Length-4,2);
            string s2 = tvid.Substring(tvid.Length-2);
            string s3 = tvid;
            string url = "https://cmts.iqiyi.com/bullet/" + s1 + "/" + s2 + "/" + s3 + "_300_"+time+".z";
            string json = CommonTool.getBytes(url);
            Console.WriteLine("SSSS"+json);
            return ls;
        }
    }
}
