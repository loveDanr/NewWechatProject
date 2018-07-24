﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// wxmessage 的摘要说明
/// </summary> 
    public class wxmessage
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string CreateTime { get; set; }
        public string MsgType { get; set; }
        public string EventName { get; set; }
        public string Content { get; set; }
        public string Recognition { get; set; }
        public string MediaId { get; set; }
        public string EventKey { get; set; }
        public string Location_X { get; set; }
        public string Location_Y { get; set; }
        public string Scale { get; set; }
        public string Label { get; set; }
        public string Latitude { get; set; } //经度
        public string Longitude { get; set; } //纬度
        public string Precision { get; set; }
    
}