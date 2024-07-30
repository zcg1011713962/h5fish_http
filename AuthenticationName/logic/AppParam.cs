public class SecretInfo
{
    public string AppId { set; get; } = "ace555e66e4b4c71806808f167f4502c";
    public string BizId { set; get; } = "1101999999";
    public string SecKey { set; get; } = "08a30c4b6d2df60db6c2e465ef069250";

    public SecretInfo(bool isTest)
    {
        if (!isTest) // 正式数据
        {
            AppId = "e23afa65b7a74abfa4678428eda8a961";
            SecKey = "7c66bf4490afd3a096ec4b480a19ab08";
            BizId = "1199004369";
        }
    }
}

public class URLInfo
{
    public string CHECK_URL = @"https://api.wlc.nppa.gov.cn/idcard/authentication/check";

    public string QUERY_URL = @"http://api2.wlc.nppa.gov.cn/idcard/authentication/query?ai={0}";

    public string UPDATA_URL = @"http://api2.wlc.nppa.gov.cn/behavior/collection/loginout";

    public bool IsTest
    {
        set; get;
    }

    public URLInfo(bool isTest)
    {
        IsTest = isTest;

        if (isTest)
        {
            // 由后台提供测试码,需要设置下
            CHECK_URL = @"https://wlc.nppa.gov.cn/test/authentication/check/{0}";

            QUERY_URL = @"https://wlc.nppa.gov.cn/test/authentication/query/{0}?ai={1}";

            UPDATA_URL = @"https://wlc.nppa.gov.cn/test/collection/loginout/{0}";
        }
    }

    public string getCheckURL(string testCode)
    {
        if (IsTest)
        {
            return string.Format(CHECK_URL, testCode);
        }

        return CHECK_URL;
    }

    public string getQueryURL(string ai, string testCode)
    {
        if (IsTest)
        {
            return string.Format(QUERY_URL, testCode, ai);
        }

        return string.Format(QUERY_URL, ai);
    }

    public string getUpDataURL(string testCode)
    {
        if (IsTest)
        {
            return string.Format(UPDATA_URL, testCode);
        }

        return UPDATA_URL;
    }
}


