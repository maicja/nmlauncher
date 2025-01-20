using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft_Launcher
{
    public static class mslogin
    {
        public static string[] addmsacc()
        {
            try
            {
                //logging in to microsoft account
                string coderesponse = new WebClient().DownloadString("https://login.microsoftonline.com/consumers/oauth2/v2.0/devicecode?client_id=499546d9-bbfe-4b9b-a086-eb3d75afb78f&scope=XboxLive.signin%20offline_access");
                string usercode = coderesponse.Replace("\"user_code\":\"", ">").Split('>')[1].Split('"')[0];
                string verifyurl = coderesponse.Replace("\"verification_uri\":\"", ">").Split('>')[1].Split('"')[0];
                string devicecode = coderesponse.Replace("\"device_code\":\"", ">").Split('>')[1].Split('"')[0];
             
                Process.Start(verifyurl + "?otc=" + usercode);

                MessageBox.Show($"Go to {verifyurl + "?otc=" + usercode} and enter code {usercode} to continue\r\n\r\nClose this window after logging in or else login will fail", "Microsoft login");

                //getting mstoken + refreshtoken
                string spamrequest = $"client_id=499546d9-bbfe-4b9b-a086-eb3d75afb78f&code={devicecode}&grant_type=urn%3Aietf%3Aparams%3Aoauth%3Agrant-type%3Adevice_code";
                WebRequest webRequest = WebRequest.Create("https://login.microsoftonline.com/consumers/oauth2/v2.0/token");
                webRequest.Timeout = 15000;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Cookie", "esctx=PAQABAAEAAAAmoFfGtYxvRrNriQdPKIZ-N1P_MEcnyzSjOqDSJiFM7mL9hTzkvRZDiQnF8oxfy0UDFCE9QVbyMFtCPdcO7LTrN_eGz0Fuk4sXL38g7azDkEZ82a4qVcU6FQsDj0wGq0tu4dHlTuSQU_n5UY5vnjtNLyvxkfvQixzDk-GA7-flAXkwJhTHDb-gnlM60MPN54sgAA; stsservicecookie=estsfd; fpc=Aq7Yu-WVxD9KsIRK63eSKH1jHbnPFAAAALiBXN0OAAAA; x-ms-gateway-slice=estsfd");
                webRequest.ContentLength = (long)spamrequest.Length;
                StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
                streamWriter.Write(spamrequest.ToCharArray(), 0, spamrequest.Length);
                streamWriter.Close();
                StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string spamresponse = streamReader.ReadToEnd();
                string tokenspam = spamresponse.Replace("\"access_token\":\"", ">").Split('>')[1].Split('"')[0];
                string refreshtokenspam = spamresponse.Replace("\"refresh_token\":\"", ">").Split('>')[1].Split('"')[0];



                //getting authenticate token
                string authrequest = $"{{\r\n   \"Properties\" : {{\r\n      \"AuthMethod\" : \"RPS\",\r\n      \"RpsTicket\" : \"d={tokenspam}\",\r\n      \"SiteName\" : \"user.auth.xboxlive.com\"\r\n   }},\r\n   \"RelyingParty\" : \"http://auth.xboxlive.com\",\r\n   \"TokenType\" : \"JWT\"\r\n}}";
                WebRequest authwebRequest = WebRequest.Create("https://user.auth.xboxlive.com/user/authenticate");
                authwebRequest.Timeout = 15000;
                authwebRequest.Method = "POST";
                authwebRequest.ContentType = "application/json";
                authwebRequest.ContentLength = (long)authrequest.Length;
                StreamWriter authstreamWriter = new StreamWriter(authwebRequest.GetRequestStream());
                authstreamWriter.Write(authrequest.ToCharArray(), 0, authrequest.Length);
                authstreamWriter.Close();
                StreamReader authstreamReader = new StreamReader(authwebRequest.GetResponse().GetResponseStream());
                string authresponse = authstreamReader.ReadToEnd();
                string authtoken = authresponse.Replace("\"Token\":\"", ">").Split('>')[1].Split('"')[0];


                //getting authorizetoken + uhs
                string authorizerequest = $"{{\r\n   \"Properties\" : {{\r\n      \"SandboxId\" : \"RETAIL\",\r\n      \"UserTokens\" : [\r\n         \"{authtoken}\"\r\n      ]\r\n   }},\r\n   \"RelyingParty\" : \"rp://api.minecraftservices.com/\",\r\n   \"TokenType\" : \"JWT\"\r\n}}";
                WebRequest authorizewebRequest = WebRequest.Create("https://xsts.auth.xboxlive.com/xsts/authorize");
                authorizewebRequest.Timeout = 15000;
                authorizewebRequest.Method = "POST";
                authorizewebRequest.ContentType = "application/json";
                authorizewebRequest.ContentLength = (long)authorizerequest.Length;
                StreamWriter authorizestreamWriter = new StreamWriter(authorizewebRequest.GetRequestStream());
                authorizestreamWriter.Write(authorizerequest.ToCharArray(), 0, authorizerequest.Length);
                authorizestreamWriter.Close();
                StreamReader authorizestreamReader = new StreamReader(authorizewebRequest.GetResponse().GetResponseStream());
                string authorizeresponse = authorizestreamReader.ReadToEnd();
                string authorizetoken = authorizeresponse.Replace("\"Token\":\"", ">").Split('>')[1].Split('"')[0];
                string uhs = authorizeresponse.Replace("\"uhs\":\"", ">").Split('>')[1].Split('"')[0];

                //getting minecraft accesstoken
                string loginreqrequest = $"{{\r\n   \"platform\" : \"PC_LAUNCHER\",\r\n   \"xtoken\" : \"XBL3.0 x={uhs};{authorizetoken}\"\r\n}}";
                WebRequest loginreqwebRequest = WebRequest.Create("https://api.minecraftservices.com/launcher/login");
                loginreqwebRequest.Timeout = 15000;
                loginreqwebRequest.Method = "POST";
                loginreqwebRequest.ContentType = "application/json";
                loginreqwebRequest.ContentLength = (long)loginreqrequest.Length;
                StreamWriter loginreqstreamWriter = new StreamWriter(loginreqwebRequest.GetRequestStream());
                loginreqstreamWriter.Write(loginreqrequest.ToCharArray(), 0, loginreqrequest.Length);
                loginreqstreamWriter.Close();
                StreamReader loginreqstreamReader = new StreamReader(loginreqwebRequest.GetResponse().GetResponseStream());
                string loginreqresponse = loginreqstreamReader.ReadToEnd();
                string mcaccesstoken = loginreqresponse.Replace("\"access_token\" : \"", ">").Split('>')[1].Split('"')[0];

                //gettings username + uuid
                WebClient premiumprofileweb = new WebClient();
                premiumprofileweb.Headers.Add("Authorization", $"Bearer {mcaccesstoken}");
                string getprofileresponse = premiumprofileweb.DownloadString("https://api.minecraftservices.com/minecraft/profile");
                string username = getprofileresponse.Replace("\"name\" : \"", ">").Split('>')[1].Split('"')[0];
                string uuid = getprofileresponse.Split('[')[0].Replace("\"id\" : \"", ">").Split('>')[1].Split('"')[0];


                bool isadded = false;
                if (mcaccesstoken.Length > 1)
                {
                    if (username.Length > 1)
                    {
                        if (uuid.Length > 1)
                        {
                            isadded = true;
                            return new string[] { username, uuid, refreshtokenspam, mcaccesstoken };
                        }
                    }
                }
                if (!isadded)
                {
                    MessageBox.Show($"There was en error with adding account. Collected info:\r\nusername: {username}\r\nUUID: {uuid}\r\ntoken: {mcaccesstoken}");
                    return new string[] { "error", "error" };
                }
                return new string[] { "error", "error" };
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was en error with adding account: " + ex.Message, "ERROR");
                return new string[] { "error", "error" };
            }
        }
        public static string RefreshAccountData(string refreshtoken)
        {
            try
            {
                //getting mstoken + refreshtoken
                string spamrequest = $"client_id=499546d9-bbfe-4b9b-a086-eb3d75afb78f&grant_type=refresh_token&refresh_token={refreshtoken}";
                WebRequest webRequest = WebRequest.Create("https://login.microsoftonline.com/consumers/oauth2/v2.0/token");
                webRequest.Timeout = 15000;
                webRequest.Method = "POST";
                webRequest.ContentLength = (long)spamrequest.Length;
                StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
                streamWriter.Write(spamrequest.ToCharArray(), 0, spamrequest.Length);
                streamWriter.Close();
                StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string spamresponse = streamReader.ReadToEnd();
                string tokenspam = spamresponse.Replace("\"access_token\":\"", ">").Split('>')[1].Split('"')[0];
                string refreshtokenspam = spamresponse.Replace("\"refresh_token\":\"", ">").Split('>')[1].Split('"')[0];

                //getting authenticate token
                string authrequest = $"{{\r\n   \"Properties\" : {{\r\n      \"AuthMethod\" : \"RPS\",\r\n      \"RpsTicket\" : \"d={tokenspam}\",\r\n      \"SiteName\" : \"user.auth.xboxlive.com\"\r\n   }},\r\n   \"RelyingParty\" : \"http://auth.xboxlive.com\",\r\n   \"TokenType\" : \"JWT\"\r\n}}";
                WebRequest authwebRequest = WebRequest.Create("https://user.auth.xboxlive.com/user/authenticate");
                authwebRequest.Timeout = 15000;
                authwebRequest.Method = "POST";
                authwebRequest.ContentType = "application/json";
                authwebRequest.ContentLength = (long)authrequest.Length;
                StreamWriter authstreamWriter = new StreamWriter(authwebRequest.GetRequestStream());
                authstreamWriter.Write(authrequest.ToCharArray(), 0, authrequest.Length);
                authstreamWriter.Close();
                StreamReader authstreamReader = new StreamReader(authwebRequest.GetResponse().GetResponseStream());
                string authresponse = authstreamReader.ReadToEnd();
                string authtoken = authresponse.Replace("\"Token\":\"", ">").Split('>')[1].Split('"')[0];

                //getting authorizetoken + uhs
                string authorizerequest = $"{{\r\n   \"Properties\" : {{\r\n      \"SandboxId\" : \"RETAIL\",\r\n      \"UserTokens\" : [\r\n         \"{authtoken}\"\r\n      ]\r\n   }},\r\n   \"RelyingParty\" : \"rp://api.minecraftservices.com/\",\r\n   \"TokenType\" : \"JWT\"\r\n}}";
                WebRequest authorizewebRequest = WebRequest.Create("https://xsts.auth.xboxlive.com/xsts/authorize");
                authorizewebRequest.Timeout = 15000;
                authorizewebRequest.Method = "POST";
                authorizewebRequest.ContentType = "application/json";
                authorizewebRequest.ContentLength = (long)authorizerequest.Length;
                StreamWriter authorizestreamWriter = new StreamWriter(authorizewebRequest.GetRequestStream());
                authorizestreamWriter.Write(authorizerequest.ToCharArray(), 0, authorizerequest.Length);
                authorizestreamWriter.Close();
                StreamReader authorizestreamReader = new StreamReader(authorizewebRequest.GetResponse().GetResponseStream());
                string authorizeresponse = authorizestreamReader.ReadToEnd();
                string authorizetoken = authorizeresponse.Replace("\"Token\":\"", ">").Split('>')[1].Split('"')[0];
                string uhs = authorizeresponse.Replace("\"uhs\":\"", ">").Split('>')[1].Split('"')[0];

                //getting mcaccesstoken
                string loginreqrequest = $"{{\r\n   \"platform\" : \"PC_LAUNCHER\",\r\n   \"xtoken\" : \"XBL3.0 x={uhs};{authorizetoken}\"\r\n}}";
                WebRequest loginreqwebRequest = WebRequest.Create("https://api.minecraftservices.com/launcher/login");
                loginreqwebRequest.Timeout = 15000;
                loginreqwebRequest.Method = "POST";
                loginreqwebRequest.ContentType = "application/json";
                loginreqwebRequest.ContentLength = (long)loginreqrequest.Length;
                StreamWriter loginreqstreamWriter = new StreamWriter(loginreqwebRequest.GetRequestStream());
                loginreqstreamWriter.Write(loginreqrequest.ToCharArray(), 0, loginreqrequest.Length);
                loginreqstreamWriter.Close();
                StreamReader loginreqstreamReader = new StreamReader(loginreqwebRequest.GetResponse().GetResponseStream());
                string loginreqresponse = loginreqstreamReader.ReadToEnd();
                string mcaccesstoken = loginreqresponse.Replace("\"access_token\" : \"", ">").Split('>')[1].Split('"')[0];

                //getting username + UUID
                WebClient premiumprofileweb = new WebClient();
                premiumprofileweb.Headers.Add("Authorization", $"Bearer {mcaccesstoken}");
                string getprofileresponse = premiumprofileweb.DownloadString("https://api.minecraftservices.com/minecraft/profile");
               
                //MessageBox.Show(getprofileresponse);
                string username = getprofileresponse.Replace("\"name\" : \"", ">").Split('>')[1].Split('"')[0];
                string uuid = getprofileresponse.Split('[')[0].Replace("\"id\" : \"", ">").Split('>')[1].Split('"')[0];
                return $"{username}|{uuid}|{refreshtokenspam}|{mcaccesstoken}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Account refresh error: " + ex.Message, "Microsoft login");
                return "error|error";
            }
        }
    }
}
