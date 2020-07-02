using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanMuTool
{
    class Danmu : IComparable<Danmu>
    {
        public string content;
        public int time;
        public string color;

        public int CompareTo(Danmu other)
        {
            if (null == other)
            {
                return 1;//空值比较大，返回1
            }
            return this.time.CompareTo(other.time);//升序
           // return other.time.CompareTo(this.time);//降序
        }
    }
}
