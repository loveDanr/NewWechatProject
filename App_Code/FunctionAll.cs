using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Text;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.Security;
using System.Configuration;

/// <summary>
/// 基本上所有的功能性写在这里
/// </summary>
public class FunctionAll
    {
        /// <summary>
        /// 获取post请求数据
        /// </summary>
        /// <returns></returns>
        private string PostInput()
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            return Encoding.UTF8.GetString(b);
        }
    #region
    /// <summary>
    /// 金额大小写转换
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public  String ConvertToChinese(Decimal number)
    {
        var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
        var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
        var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
        return r;
    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    /// <param name="root"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public wxmessage GetWxMessage(XmlDocument doc)
        {
            wxmessage wxModel = new wxmessage();
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "FromUserName")))
            {
                wxModel.FromUserName = WeiXinXML.GetFromXML(doc, "FromUserName");
            }
            else
            {
                wxModel.FromUserName = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "ToUserName")))
            {
                wxModel.ToUserName = WeiXinXML.GetFromXML(doc, "ToUserName");
            }
            else
            {
                wxModel.ToUserName = string.Empty;
            }
            wxModel.CreateTime = WeiXinXML.GetFromXML(doc, "CreateTime");
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "MsgType")))
            {
                wxModel.MsgType = WeiXinXML.GetFromXML(doc, "MsgType");
            }
            else
            {
                wxModel.MsgType = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Event")))
            {
                wxModel.EventName = WeiXinXML.GetFromXML(doc, "Event");
            }
            else
            {
                wxModel.EventName = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Content")))
            {
                wxModel.Content = WeiXinXML.GetFromXML(doc, "Content");
            }
            else
            {
                wxModel.Content = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Recognition")))
            {
                wxModel.Recognition = WeiXinXML.GetFromXML(doc, "Recognition");
            }
            else
            {
                wxModel.Recognition = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "MediaId")))
            {
                wxModel.MediaId = WeiXinXML.GetFromXML(doc, "MediaId");
            }
            else
            {
                wxModel.MediaId = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "EventKey")))
            {
                wxModel.EventKey = WeiXinXML.GetFromXML(doc, "EventKey");
            }
            else
            {
                wxModel.EventKey = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Location_X")))
            {
                wxModel.Location_X = WeiXinXML.GetFromXML(doc, "Location_X");
            }
            else
            {
                wxModel.Location_X = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Location_Y")))
            {
                wxModel.Location_Y = WeiXinXML.GetFromXML(doc, "Location_Y");
            }
            else
            {
                wxModel.Location_Y = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Scale")))
            {
                wxModel.Scale = WeiXinXML.GetFromXML(doc, "Scale");
            }
            else
            {
                wxModel.Scale = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Label")))
            {
                wxModel.Label = WeiXinXML.GetFromXML(doc, "Label");
            }
            else
            {
                wxModel.Label = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Latitude")))
            {
                wxModel.Latitude = WeiXinXML.GetFromXML(doc, "Latitude");
            }
            else
            {
                wxModel.Latitude = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Longitude")))
            {
                wxModel.Longitude = WeiXinXML.GetFromXML(doc, "Longitude");
            }
            else
            {
                wxModel.Longitude = string.Empty;
            }
            if (!string.IsNullOrEmpty(WeiXinXML.GetFromXML(doc, "Precision")))
            {
                wxModel.Precision = WeiXinXML.GetFromXML(doc, "Precision");
            }
            else
            {
                wxModel.Precision = string.Empty;
            }
            return wxModel;


        }
        /// <summary>
        /// 可以通用，获取菜单
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                return string.Empty;
            }
        }
    #region 用cookie存储登录用户信息的方法
    /// <summary>
    /// 用户登陆成功后，发放表单cookie验证票据并记录用户的相关信息
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="userName">与票证关联的用户名</param>
    /// <param name="Page">页面对象</param>
    /// <param name="expiration">FormsAuthenticationTicket过期时间</param> 
    /// <param name="userInfo">要保存在cookie中用户对象</param>
    public static void UserLoginSetCookie<T>(string userName, HttpContext context, DateTime expiration, T userInfo)
    {
        Configuration conn = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
        System.Web.Configuration.AuthenticationSection section = (System.Web.Configuration.AuthenticationSection)conn.SectionGroups.Get("system.web").Sections.Get("authentication");
        expiration = expiration.AddMinutes(section.Forms.Timeout.TotalMinutes);

        //将对象序列化成字符串
        string strUser = JsonHelper.SerializeObject(userInfo);

        // 设置票据Ticket信息
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, expiration, false, strUser);

        string strTicket = FormsAuthentication.Encrypt(ticket);// 加密票据

        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strTicket);// 使用新userdata保存cookie
                                                                                           //cookie.Expires = ticket.Expiration;//将票据的过期时间和Cookie的过期时间同步，避免因两者的不同所产生的矛盾

        context.Response.Cookies.Add(cookie);
    }
    #endregion

    #region 用cookie存储登录用户信息的方法
    /// <summary>
    /// 用户登陆成功后，发放表单cookie验证票据并记录用户的相关信息
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="userName">与票证关联的用户名</param>
    /// <param name="Page">页面对象</param>
    /// <param name="expiration">FormsAuthenticationTicket过期时间</param> 
    /// <param name="userInfo">要保存在cookie中用户对象</param>
    public static void UserLoginSetCookie<T>(string userName, Page page, DateTime expiration, T userInfo)
    {
        Configuration conn = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
        System.Web.Configuration.AuthenticationSection section = (System.Web.Configuration.AuthenticationSection)conn.SectionGroups.Get("system.web").Sections.Get("authentication");
        expiration = expiration.AddMinutes(section.Forms.Timeout.TotalMinutes);

        //将对象序列化成字符串
        string strUser = JsonHelper.SerializeObject(userInfo);

        // 设置票据Ticket信息
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, expiration, false, strUser);

        string strTicket = FormsAuthentication.Encrypt(ticket);// 加密票据

        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strTicket);// 使用新userdata保存cookie
                                                                                           //cookie.Expires = ticket.Expiration;//将票据的过期时间和Cookie的过期时间同步，避免因两者的不同所产生的矛盾

        page.Response.Cookies.Add(cookie);
    }
    #endregion


    /// <summary>
    /// 获得保存在cookie中的用户对象
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="page">页面对象</param>
    /// <returns>保存在cookie中的用户对象</returns>
    public static T GetCookieUserData<T>(Page page)
    {
        if (page != null)
        {
            if (page.User.Identity.IsAuthenticated)//如果没验证通过的话,强制类型转换会出错!~
            {
                string strUser = ((FormsIdentity)page.User.Identity).Ticket.UserData;
                return JsonHelper.DeserializeObject<T>(strUser);
            }
            else
                return default(T);
        }
        else
            throw new ApplicationException("page == null");
    }
    /// <summary>
    /// 菜单项目
    /// </summary>
    public void MyMenu()
        {
            string weixin1 = "";
            weixin1 = @" {
     ""button"":[
     {	
          ""type"":""view"",
          ""name"":""预约挂号"",
          ""url"":""http://bsz5xp.natappfree.cc/index.aspx""
      },
      {
           ""type"":""click"",
           ""name"":""微医简介"",
           ""key"":""myprofile""
      },
      { 
           ""type"":""click"",
           ""name"":""更多功能"",
           ""key"":""morefunction""
            
       }]
 }
";
        //string filepath = HttpContext.Current.Server.MapPath("menu.txt");
        //StreamReader sr = new StreamReader(filepath, Encoding.Default);
        //string line = sr.ReadToEnd();
        string access_token = IsExistAccess_Token();

    
      GetPage("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token + "", weixin1);
        }
    /// <summary>
    /// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static string IsExistAccess_Token()
    {
        string Token = string.Empty;
        //if (System.Web.HttpContext.Current.Session["accessToken"].ToString()!= "")
        //{
        //    Token = System.Web.HttpContext.Current.Session["accessToken"].ToString();

        //}
        if (HttpRequestUtil.TokenExpired(System.Web.HttpContext.Current.Session["accessToken"].ToString()) ==true)
        {
            Token= System.Web.HttpContext.Current.Session["accessToken"].ToString();
        }
        else {

            Access_token mode = GetAccessToken();

            Token = mode.access_token;
        }

        return Token;
    }
    #region 跳转打开页面授权登录
    /// <summary>
    /// 打开页面
    /// </summary>
    /// <param name="Appid"></param>
    /// <param name="redirect_uri"></param>
    /// <returns></returns>
    public static string GetCodeUrl(string redirect_uri)
    {
        string result = string.Empty;
        result = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppIDCode.Appid + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect");
        return result;
    }
    #endregion
    #region Post/Get提交调用抓取
    /// <summary>
    /// Post/get 提交调用抓取
    /// </summary>
    /// <param name="url">提交地址</param>
    /// <param name="param">参数</param>
    /// <returns>string</returns>
    public static string WebRequestPostOrGet(string sUrl, string sParam)
        {
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);

            Uri uriurl = new Uri(sUrl);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + (url.IndexOf("?") > -1 ? "" : "?") + param);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bt.Length;

            using (Stream reqStream = req.GetRequestStream())//using 使用可以释放using段内的内存
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Flush();
            }
            try
            {
                using (WebResponse res = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理 

                    Stream resStream = res.GetResponseStream();

                    StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);

                    string resLine;

                    System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();

                    while ((resLine = resStreamReader.ReadLine()) != null)
                    {
                        resStringBuilder.Append(resLine + System.Environment.NewLine);
                    }

                    resStream.Close();
                    resStreamReader.Close();

                    return resStringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;//url错误时候回报错
            }
        }
        #endregion Post/Get提交调用抓取
 
        //     /// <summary>
        /// 获取AccessToken
        /// http://mp.weixin.qq.com/wiki/index.php?title=%E8%8E%B7%E5%8F%96access_token
        /// </summary>
        /// <param name="grant_type"></param>
        /// <param name="appid"></param>
        /// <param name="secrect"></param>
        /// <returns>access_toke</returns>
        public static dynamic GetAccessToken()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppIDCode.Appid + "&secret=" + AppIDCode.Appsecret+"";
            Access_token mode = new Access_token();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                string content = reader.ReadToEnd();
                //Response.Write(content);
                //在这里对Access_token 赋值
                Access_token token = new Access_token();
                token = JsonHelper.ParseFromJson<Access_token>(content);
                mode.access_token = token.access_token;
                mode.expires_in = token.expires_in;
            }
            return mode;
        }




    }
