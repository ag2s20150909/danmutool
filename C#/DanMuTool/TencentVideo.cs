using System;
using System.Collections.Generic;

namespace DanMuTool
{
    class TencentVideo
    {
        Object temp;
        string title;
        string vid;
        string tgid;
        int duration;
        List<Danmu> danmus = new List<Danmu>();
        public static void get(string url)
        {
            TencentVideo video = new TencentVideo();
            video.danmus = new List<Danmu>();
            string html = CommonTool.get(url);
            video.vid = CommonTool.regFind(html, @"\&vid=(.*)&ptag=");
            video.title = CommonTool.regFind(html, @"\<title\>(.*)\<\/title\>").Replace("_1080P在线观看平台", "").Replace("_腾讯视频", "");
            string d=CommonTool.regFind(html, @"""duration"":""(.*?)""");
            html = null;
            int.TryParse(d, out video.duration);
            Console.WriteLine(video.vid + "\n" + video.title+"\n"+video.duration);
            video.getTgrgetID(video.vid);
            Console.WriteLine(video.tgid);
            //video.duration = 2;
            for(int i = 0; i < video.duration; i += 30)
            {
                video.danmus.AddRange(video.getDanmu(video.tgid, i));
                System.Threading.Thread.Sleep(500);//睡眠500毫秒，也就是0.5秒
            }
            video.danmus.Sort();
            //Console.WriteLine(video.danmus);
            CommonTool.printDanmuList(video.danmus);
            CommonTool.SaveDanmuList(video.danmus, video.title);


            Console.ReadKey();

        }

        public List<Danmu> getDanmu(string tgid,int time)
        {
            List<Danmu> ls = new List<Danmu>();
            string url = "http://mfm.video.qq.com/danmu?timestamp="+time+"&target_id=" + tgid;
            string json = CommonTool.get(url);
            Newtonsoft.Json.Linq.JObject jos = Newtonsoft.Json.Linq.JObject.Parse(json);
            foreach(Newtonsoft.Json.Linq.JObject jo in jos["comments"])
            {
                Console.WriteLine("a:" + jo["content"]);
                Console.WriteLine("a:" + jo["timepoint"]);
                Console.WriteLine("Color:" + jo["content_style"].ToString());
               

                
                //Console.WriteLine("a:" + jo["content_style"]["gradient_colors"]);
              
                //Console.WriteLine("a:" + jo["content_style"]["gradient_colors"]);
                Danmu d = new Danmu();
                d.content = jo["content"].ToString();
                d.time = (int)jo["timepoint"];
                if (jo["content_style"].ToString().Length > 9)
                {
                    Newtonsoft.Json.Linq.JObject jos1 = Newtonsoft.Json.Linq.JObject.Parse(jo["content_style"].ToString());
                    this.temp = jos1;
                    d.color = jos1["gradient_colors"][0].ToString();
                }
                else
                {
                    d.color = "ffffff";
                }
           
                ls.Add(d);
            }

            return ls;
        }

        public void getTgrgetID(string vid)
        {
            string url = "http://bullet.video.qq.com/fcgi-bin/target/regist?otype=json&vid=" + vid;
            string html = CommonTool.get(url);
            this.tgid = CommonTool.regFind(html, @"targetid"":""(.*)""");
            
            
        }
    }
}
