#!/usr/bin/python3
# -*- coding: utf-8 -*-
import re
import requests,json
import random
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
    
def RGB2BGR(rgb):
    r=rgb[0:2]
    g=rgb[2:4]
    b=rgb[4:6]
    return b+g+r
###根据url获取腾讯视频弹幕
def get(url):
    r=requests.get(url)
    r.encoding = 'utf-8'
    vid=re.search('\&vid=(.*)&ptag=',r.text).group(1)
    title=re.search('\<title\>(.*)\<\/title\>',r.text).group(1).replace('_1080P在线观看平台','').replace('_腾讯视频','')
    print(title)
   
    run(vid,title)
    
###根据vid获取targetid
def getTargetId(vid):
    url="http://bullet.video.qq.com/fcgi-bin/target/regist?otype=json&vid="+vid
    #print(url)
    r=requests.get(url)
    r.encoding = 'utf-8'
    #print(r.text)
    targetid=re.search('targetid\"\:\"(.*)\"', r.text).group(1)
    #print(targetid)
    return targetid
###根据targetid和秒数获取30秒内的弹幕列表   
def getDanmu(tgid,time):
    danmu=[]
    url="http://mfm.video.qq.com/danmu?timestamp=%s&target_id=%s" %(time,tgid)
    r=requests.get(url)
    r.encoding = 'utf-8'
    #open('qq'+str(time)+'.json',mode='w',encoding='utf-8').write(r.text)
    d=json.loads(r.text, strict=False)
    #print(d.get("count"))
    print("正在获取："+secondToString(time))
    ds=d.get("comments")
    #if len(ds):
        #open('danmu.json',mode='w',encoding='utf-8').write(r.text)
    for c in ds:
        dc={}
        dc['time']=c.get("timepoint")
        dc['content']=c.get("content")
        dc['style']=c.get('content_style')
        danmu.append(dc)
    return danmu
    
###主体部分，最后生成ass字幕    
def run(vid,title):
    tgid=getTargetId(vid)
    i=0
    ls=[]
    tp=getDanmu(tgid,i)
    ls.extend(tp)
    while len(tp)>0:
        print(i)
        i+=30
        tp=getDanmu(tgid,i)
        ls.extend(tp)
        
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
    for s in ls:
        sttime=secondToString(s['time'])
        edtime=secondToString(s['time']+15)
        content=s['content']
        res=re.search('gradient_colors":\["(.*)","(.*)"\],"',s['style'])
        if res:
            color=res.group(1)
        else:
            color="ffffff"
        pos=random.randint(20,1060)
        ss="{\\move(1920,%s,-1920,%s)\c&H%s&}" %(pos,pos,RGB2BGR(color))
        line="Dialogue: 0,%s,%s,Default,,0,0,0,,%s\n" %(sttime,edtime,ss+content)
        danmu.write(line)

        
if __name__ == '__main__':
    print('作为主程序运行')
    get('https://v.qq.com/x/cover/mzc002001kt6n30/k0033zdd4ug.html')
else:
    print('腾讯模块初始化成功')

    

