#!/usr/bin/python3
# -*- coding: utf-8 -*-
import mode.tencent as tencent
import mode.iqiyi as iqiyi

def main():    
    print("获取腾讯视频/爱奇艺的弹幕并转化为ASS字幕\n输入\"exit\"退出。")
    url=input("播放页面地址：").strip().lower()
    if url=='exit':
        return
    elif 'v.qq.com' in url:
        tencent.get(url)
    elif 'www.iqiyi.com' in url:
        iqiyi.get(url)
    else:
        print("请输入正确的url.")
        main()
        
main()