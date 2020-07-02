#https://cmts.iqiyi.com/bullet/tvid倒数4位的前两位/tvid最后两位/tvid_300_x.z
#x的计算方式为片子总时长除以300秒向上取整，即按每5分钟一个包。
#!/usr/bin/python3
# -*- coding: utf-8 -*-
import re
import requests
##数字格式化
def format(i):
    return "{:0>2d}".format(i)
##将秒转化为ASS的时间格式
def secondToString(time):
    mins=0
    second=int(time%60)
    minute=int((time%3600)/60)
    hours=int(time/3600)
    s="%s:%s:%s.%s" %(format(hours),format(minute),format(second),format(mins))
    #print(s)
    return s
def timeToSeconds(time):
    seconds=0
    data=time.split(':')
    #print(data)
    for i in range(len(data)):
        #print(len(data)-i)
        #print(i)
        if (len(data)-i) ==1:
            seconds+=int(data[i])
        elif (len(data)-i)==2:
            seconds+=60*int(data[i])
        elif (len(data)-i)==3:
            seconds+=3600*int(data[i])
    t1=int(seconds/300)
    t2=int(seconds%300)
    if(t2>0):
        t1+=1
    print(t1)
    return t1
def getDanmu(tvid,time):
    if time==0:
        return []
    import zlib
    try:
        import xml.etree.cElementTree as ET
    except ImportError:
        import xml.etree.ElementTree as ET
        
    ti1=tvid[len(tvid)-4:len(tvid)-2]
    ti2=tvid[len(tvid)-2:len(tvid)]
    ti3=time
    print(ti2)
    lis=[]
    url="https://cmts.iqiyi.com/bullet/%s/%s/%s_300_%s.z" %(ti1,ti2,tvid,time)
    print(url)
    res=requests.get(url).content
    zarray = bytearray(res)
    xml=zlib.decompress(zarray, 15+32).decode('utf-8')
    #print(xml)
    root = ET.fromstring(xml)
    print(root.tag)
    #print(root[1][0][1].iter('bulletInfo'))
    for item in root.iter('bulletInfo'):
        it={}
        it['content']=item[1].text##content
        it['time']=int(item[3].text)##time
        it['color']=item[5].text##color
        lis.append(it)
 
    return lis   
    #with open('./iqiyi.xml','w',encoding='utf-8') as f:
        #f.write(xml)
    #f.close()


def get(url):
    #param['tvid'] = "17079150900";
    r=requests.get(url)
    r.encoding = 'utf-8'
    vid=re.search('param\[\'tvid\'\] = "(.*)";',r.text).group(1)
    time=re.search('\"duration\":\"(.*?)\"',r.text).group(1)
    title=re.search('<meta property="og:title"content="(.*?)"/>',r.text).group(1)
    print(title)
    length=timeToSeconds(time)
    data=[]
    for i in range(length):
        data.extend(getDanmu(vid,i+1))
        
    #开始生成弹幕
    danmu=open(title+'.ass',mode='w',encoding='utf-8')
    ss='''[Script Info]
    ScriptType: v4.00+
    PlayResX: 1920
    PlayResY: 1080

    [V4+ Styles]
    Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding
    Style: Default,Microsoft YaHei,64,&H00FFFFFF,&H00FFFFFF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,1,0,4,20,20,20,134

    [Events]
    Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n'''
    danmu.write(ss)
    import random
    #读写主体部分
    for s in data:
        sttime=secondToString(s['time'])
        edtime=secondToString(s['time']+15)
        content=s['content']
        pos=random.randint(20,1060)
        ss="{\\move(1920,%s,-1920,%s)\c&H%s&}" %(pos,pos,s['color'])
        line="Dialogue: 0,%s,%s,Default,,0,0,0,,%s\n" %(sttime,edtime,ss+content)
        danmu.write(line)
    
if __name__ == '__main__':
    print('作为主程序运行')
    get('https://www.iqiyi.com/v_19rxv1nwag.html?')
else:
    print('爱奇艺模块初始化成功')
