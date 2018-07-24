using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// AppIDCode 的摘要说明
/// </summary>
//海口互联网医院 
//appid = "wx7c90b55d449081c4";
//正式开发者密码 7e5c8c48730fa3f5b59762799512ad31
//加密钥匙VCZuOnYCPVf1pDUg7VPLd8q57WzlJQEfsedHVkzhzhp

//测试的appid    wx3fd9b86495ce9609     开发者密码6f6a79781e36f7c69e48533f209bc226  密钥j8XRu1TG0egoKhdKngbGI886R9ooPMIhoUZPOdhp9GL
/// <summary>
/// 全局设置appid和appsecret密码
/// </summary>
public class AppIDCode
{
    //测试号
    private static string appid = ConfigurationManager.AppSettings["appid"];//"wx473f64169b80670d";
    private static string appsecret = ConfigurationManager.AppSettings["appsecret"];//"c0a4bbb6c21b7fdb03057da0a6920628";
    private static string aesKey = ConfigurationManager.AppSettings["aesKey"];//"VCZuOnYCPVf1pDUg7VPLd8q57WzlJQEfsedHVkzhzhp";
    private static string key = ConfigurationManager.AppSettings["key"];//"key";
    //海口互联网医院
    //private static string appid = "wx7c90b55d449081c4";
    //private static string appsecret = "7e5c8c48730fa3f5b59762799512ad31";
    //private static string aesKey = "VCZuOnYCPVf1pDUg7VPLd8q57WzlJQEfsedHVkzhzhp";
    //private static string key = "key";

    /// <summary>
    /// 验证时用的key
    /// </summary>
    public static string Key
    {
        get { return key; }
        set { key = value; }
    }
    /// <summary>
    /// 加密解密用的key
    /// </summary>
    public static string AesKey
    {
        get { return aesKey; }
        set { aesKey = value; }
    }
    /// <summary>
    /// appid
    /// </summary>
    public static string Appid
    {
        get { return appid; }
        set { appid = value; }
    }

    /// <summary>
    /// app密钥
    /// </summary>
    public static string Appsecret
    {
        get { return appsecret; }
        set { appsecret = value; }
    }
}