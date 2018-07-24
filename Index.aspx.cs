using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;

//public class OAuth_Token
//{
//    public OAuth_Token()
//    {

//        //
//        //TODO: 在此处添加构造函数逻辑
//        //
//    }
//    //access_token	网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
//    //expires_in	access_token接口调用凭证超时时间，单位（秒）
//    //refresh_token	用户刷新access_token
//    //openid	用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
//    //scope	用户授权的作用域，使用逗号（,）分隔
//    public string access_token { get; set; }
//    public string expires_in { get; set; }
//    public string refresh_token { get; set; }
//    public string openid { get; set; }
//    public string scope { get; set; }

//}

public partial class Index : System.Web.UI.Page
{//用户id
    public string openid = "";

    //公众号信息部分
    public string appid = AppIDCode.Appid;
    public string appsecret = AppIDCode.Appsecret;
    public string redirect_uri = ConfigurationManager.AppSettings["redirect_uri"];//"http://bsz5xp.natappfree.cc/index.aspx";
    public string scope = "snsapi_userinfo";
    Access_token acessToken = new Access_token();
    
    wxmessage wx = new wxmessage();
    #region 属性显示页面accesstoken
    public string accesstoken;
    
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OAuth_Token Model = new OAuth_Token();
            string s = Request.Url.ToString();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("code")))
            {
                string Code = Request.QueryString.Get("code");
                //获得Token
                 Model = Get_token(Code);
                if (Model.access_token != null)
                {
                    accesstoken = Model.refresh_token; //LogHelper.WriteTXT(accesstoken);
                    Session["accesstoken"] = accesstoken;
                }
                else
                {
                    Model = refresh_token(Session["accesstoken"].ToString());
                    //Session.Remove("accesstoken");
                }

                LoginUserInfo OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);

                //Response.Write("用户OPENID:" + OAuthUser_Model.openid + "<br>用户昵称:" + OAuthUser_Model.nickname + "<br>性别:" + OAuthUser_Model.sex + "<br>所在省:" + OAuthUser_Model.province + "<br>所在市:" + OAuthUser_Model.city + "<br>所在国家:" + OAuthUser_Model.country + "<br>头像地址:" + OAuthUser_Model.headimgurl + "<br>用户特权信息:" + OAuthUser_Model.privilege);
                //用cookie存一下登录用户的id
                FunctionAll.UserLoginSetCookie(OAuthUser_Model.openid, this.Page, DateTime.Now, OAuthUser_Model);
                LogHelper.WriteLog("用户OPENID:" + OAuthUser_Model.openid + "\n用户昵称:" + OAuthUser_Model.nickname + "\n性别:" + OAuthUser_Model.sex + "\n所在省:" + OAuthUser_Model.province + "\n所在市:" + OAuthUser_Model.city + "\n所在国家:" + OAuthUser_Model.country + "\n头像地址:" + OAuthUser_Model.headimgurl + "\n用户特权信息:" + OAuthUser_Model.privilege);
                return;
            }
            else
            {
                try
                {
                  
                 //   Response.Redirect(FunctionAll.GetCodeUrl(Server.UrlEncode(redirect_uri)), true);
                    Response.Redirect(FunctionAll.GetCodeUrl(Server.UrlEncode(redirect_uri)), true);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }


    }
    #region   获得Token
    protected OAuth_Token Get_token(string Code)
    {
        string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppIDCode.Appid + "&secret=" + appsecret + "&code=" + Code + "&grant_type=authorization_code");
        OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
        return Oauth_Token_Model;
    }
    #endregion

    #region 刷新Token
    protected OAuth_Token refresh_token(string REFRESH_TOKEN)
    {
        string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + AppIDCode.Appid + "&grant_type=refresh_token&refresh_token=" + REFRESH_TOKEN);
        OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
        return Oauth_Token_Model;
    }
    #endregion

    #region 获得用户信息
    protected LoginUserInfo Get_UserInfo(string REFRESH_TOKEN, string OPENID)
    {
        // Response.Write("获得用户信息REFRESH_TOKEN:" + REFRESH_TOKEN + "||OPENID:" + OPENID);
        string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID + "&lang=zh_CN");
        LoginUserInfo OAuthUser_Model = JsonHelper.ParseFromJson<LoginUserInfo>(Str);
        return OAuthUser_Model;
    }
    #endregion

    #region 把链接Json化
    public static string GetJson(string url)
    {
        WebClient wc = new WebClient();
        wc.Credentials = CredentialCache.DefaultCredentials;
        wc.Encoding = Encoding.UTF8;
        string returnText = wc.DownloadString(url);

        if (returnText.Contains("errcode"))
        {
            //可能发生错误
        }
        //Response.Write(returnText);
        return returnText;
    }
    #endregion

    #region 用code换取获取用户信息（包括非关注用户的）
    /// <summary>
    ///用code 换取获取用户信息（包括非关注用户的）
    /// </summary>
    /// <param name="Appid"></param>
    /// <param name="Appsecret"></param>
    /// <param name="Code">回调页面带的code参数</param>
    /// <returns>获取用户信息（json格式）</returns>
    public string GetUserInfo(string Appid, string Appsecret, string Code)
    {
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Appid, Appsecret, Code);
        string ReText = FunctionAll.WebRequestPostOrGet(url, "");//post/get方法获取信息
        Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
        if (!DicText.ContainsKey("openid"))
        {
            LogHelper.WriteLog("获取openid失败，错误码：" + DicText["errcode"].ToString());
            return "";
        }
        else
        {
            System.Web.HttpContext.Current.Session["Oauth_Token"] = DicText["access_token"];
            System.Web.HttpContext.Current.Session.Timeout = 7200;
            return FunctionAll.WebRequestPostOrGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + DicText["access_token"] + "&openid=" + DicText["openid"] + "&lang=zh_CN", "");
        }
    }
    #endregion
}