using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Net;
using System.Web.UI;
using System.Web.Security;
using System.Xml.Linq;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Access_token mode = FunctionAll.GetAccessToken();
            Session["accessToken"] = mode.access_token;
            Session.Timeout = 120;
             
             

        }
        Valid();
        wxmessage wxGlobal = new wxmessage();
        FunctionAll fuc = new FunctionAll();
        XmlDocument doc = new XmlDocument();
        try
        {
            fuc.MyMenu();
            string postXmlStr = PostInput();
            if (!string.IsNullOrEmpty(postXmlStr))
            {
                doc.LoadXml(postXmlStr);
                XmlElement root = doc.DocumentElement;
                wxGlobal = fuc.GetWxMessage(doc);

                string result = "";
                string requestContent = "";
                //var rootElement = doc.DocumentElement;
                if (wxGlobal.MsgType == null)
                {
                    return;
                }
                else
                {

                    //获取用户发来的信息
                    switch (wxGlobal.MsgType)
                    {

                        case "text"://文本
                            requestContent = WeiXinXML.CreateTextMsg(doc, wxGlobal.Content);
                            if (wxGlobal.Content.Contains("￥"))
                            {
                                decimal req = Convert.ToDecimal(wxGlobal.Content.Replace("￥", ""));
                                result = WeiXinXML.CreateTextMsg(doc, fuc.ConvertToChinese(req));
                            }
                            else
                            {
                                result = WeiXinXML.CreateTextMsg(doc, TuLing.GetTulingMsg(wxGlobal.Content));
                                LogHelper.WriteLog(requestContent);
                                LogHelper.WriteLog(result);
                            }
                                break;
                            
                        case "location"://文本
                            result = WeiXinXML.ReArticle(wxGlobal.FromUserName, wxGlobal.ToUserName, "您附近的XXX", "XXXXXXXX", "http://119.29.20.29/image/test.jpg", FunctionAll.GetCodeUrl(Server.UrlEncode("http://q4chvj.natappfree.cc/index.aspx")));
                            break;
                        case "event":
                            switch (wxGlobal.EventName)
                            {
                                case "subscribe": //订阅
                                    result = WeiXinXML.subscribeRes(doc);
                                    break;
                                case "unsubscribe": //取消订阅
                                    break;
                                case "CLICK":

                                    if (wxGlobal.EventKey == "myprofile")
                                        result = WeiXinXML.CreateTextMsg(doc, "微医app - 原名 挂号网，用手机挂号，十分方便！更有医生咨询、智能分诊、院外候诊、病历管理等强大功能。\r\n" +
    "预约挂号 聚合全国超过900家重点医院的预约挂号资源\r\n" +
    "咨询医生 支持医患之间随时随地图文、语音、视频方式的沟通交流\r\n" +
    "智能分诊 根据分诊自测系统分析疾病类型，提供就诊建议\r\n" +
    "院外候诊 时间自由可控，不再无谓浪费\r\n" +
    "病历管理 病历信息统一管理，个人健康及时监测\r\n" +
    "贴心服务 医疗支付、报告提取、医院地图\r\n" +
    "权威保障 国家卫计委（原卫生部）指定的全国健康咨询及就医指导平台\r\n" +
    "[微医] 目前用户量大，有些不足我们正加班加点的努力完善，希望大家用宽容的心给 [微医] 一点好评，给我们一点激励，让 [微医] 和大家的健康诊疗共成长。");
                                    if (wxGlobal.EventKey == "morefunction")
                                      //  result = WeiXinXML.CreateTextMsg(doc, "更多功能正在开发中，敬请期待！");
                                    //if (wxGlobal.EventKey == "jkzx")
                                  result = WeiXinXML.ReArticle(wxGlobal.FromUserName, wxGlobal.ToUserName, "点击图片查看您附近的疾控中心", @"测试测试测试测试", "http://119.29.20.29/image/navi.jpg", ConfigurationManager.AppSettings["redirect_uri"].ToString());

                                    break;
                                case "LOCATION": //获取地理位置
                                                 //string city = fuc.GetLocation(wxGlobal.Latitude,wxGlobal.Longitude);
                                                 //result = WeiXinXML.CreateTextMsg(doc,city);
                                    break;
                            }

                            break;
                    }
                    //if (!string.IsNullOrWhiteSpace(sAppId))  //根据appid解密字符串
                    //{

                    //    WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, AESKey, sAppId);
                    //    string signature = context.Request["msg_signature"];
                    //    string timestamp = context.Request["timestamp"];
                    //    string nonce = context.Request["nonce"];
                    //    string stmp = "";
                    //    int ret = wxcpt.DecryptMsg(signature, timestamp, nonce, postXmlStr, ref stmp);
                    //    if (ret == 0)
                    //    {
                    //        doc = new XmlDocument();
                    //        doc.LoadXml(stmp);
                    //    }
                    //}
                }
                Response.Write(result);
            }

           
             else
                {
                Valid();
                return;
            }
        }
        //context.Response.Write(result);
        //context.Response.Flush();
        //LogHelper.WriteLog("系统回复的明文" + result);

        //    else
        //    {
        //    valid(context);
        //    return;
        //}
        catch(Exception ex)
        {
            throw;
        }
        

    }

    #region 公众号开发者验证方法
    /// <summary>
    /// 验证微信签名
    /// </summary>
    /// * 将token、timestamp、nonce三个参数进行字典序排序
    /// * 将三个参数字符串拼接成一个字符串进行sha1加密
    /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
    /// <returns></returns>
    private bool CheckSignature()
    {
        string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];
        string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
        string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];
        string[] ArrTmp = { AppIDCode.Key, timestamp, nonce };
        Array.Sort(ArrTmp);     //字典排序
        string tmpStr = string.Join("", ArrTmp);
        tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
        tmpStr = tmpStr.ToLower();
        if (tmpStr == signature)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Valid()
    {
        string echoStr = System.Web.HttpContext.Current.Request.QueryString["echoStr"];
        if (CheckSignature())
        {
            if (!string.IsNullOrEmpty(echoStr))
            {
                Response.Write(echoStr);
                Response.End();
            }
        }
    }
    #endregion

    #region 获取post请求数据
    /// <summary>
    /// 获取post请求数据
    /// </summary>
    /// <returns></returns>
    private string PostInput()
    {
        string data = "";
        Stream s = System.Web.HttpContext.Current.Request.InputStream;
        byte[] b = new byte[s.Length];
        s.Read(b, 0, (int)s.Length);
        data = Encoding.UTF8.GetString(b);
        return data;
    }
    #endregion

    protected void deleteMenu_Click(object sender, EventArgs e)
    {
        string access_token =FunctionAll.IsExistAccess_Token();

        FunctionAll.GetPage("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + access_token + "", ""); //删除菜单
    }

    protected void createMenu_Click(object sender, EventArgs e)
    {

    }
}