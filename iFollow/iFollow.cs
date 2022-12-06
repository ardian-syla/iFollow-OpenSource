using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace iFollow
{
    public partial class iFollow : Form
    {
        private const string V = "5";
        public static CookieContainer CookieContainer = new CookieContainer();
        public static string ard { get; set; }
        public static int followed = 0;
        public static int counter = 0;
        public static int errors = 0;

        public static string PublicId = "";
        public static string PublicUsername = "";
        public static string PublicFollowers = "";
        public static string PublicFollowings = "";

        public iFollow()
        {
            InitializeComponent();
        }
        [DllImport("user32")]
        private static extern bool ReleaseCapture();

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wp, int lp);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 161, 2, 0);
            }
        }

            private void Form1_Load(object sender, EventArgs e)
            {
            WebClient web = new WebClient();
                string versionCheck = web.DownloadString("https://pastebin.com/raw/BACwM9fz");
                label5.Text = versionCheck;
            }
            private void textBox1_TextChanged(object sender, EventArgs e)
            {
            }

            private void label1_Click(object sender, EventArgs e)
            {
            }

            private void textBox1_TextChanged_1(object sender, EventArgs e)
            {
            }

            private void loginBTN_Click(object sender, EventArgs e)
            {
                if (String.IsNullOrWhiteSpace(userGet.Text) || String.IsNullOrEmpty(userGet.Text))
                {
                    MessageBox.Show("Target can't be empty", "iFollow");
                }
                else
                {
                    LoginWEB2(userGet.Text);
                }
            }
            static void FollowUser(string id, string sessionID)
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://i.instagram.com/api/v1/web/friendships/{id}/follow/");
                request.Method = "POST";
                request.Headers.Add("authority", "i.instagram.com");
                request.Accept = "*/*";
                request.Headers.Add("accept-language", "en-US,en;q=0.9");
                request.Headers.Add("cookie", $"sessionid={sessionID};");
                request.Headers.Add("origin", "https://www.instagram.com");
                request.Referer = "https://www.instagram.com/";
                request.ContentType = " application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
                request.Headers.Add("x-asbd-id", "198387");
                request.Headers.Add("x-csrftoken", "missing");
                request.Headers.Add("x-ig-app-id", "936619743392459");
                request.Headers.Add("x-ig-www-claim", "hmac.AR2pitRBXWeGMybAC8XIej3q4FpvQjB27u7pmysXYjSkIbBy");
                request.Headers.Add("x-instagram-ajax", "1006430745");


                HttpWebResponse Resp;
                try
                {
                    Resp = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    Resp = (HttpWebResponse)ex.Response;
                }
                int respCode = Convert.ToInt32(Resp.StatusCode);
                if (respCode == 404)
                {
                    errors++;
                    AutoClosingMessageBox.Show("Something went wrong, User doesnt exist or check your connection!", "iFollow", 5000);
                }
                else if (respCode == 200)
                {
                    StreamReader StreamReader = new StreamReader(Resp.GetResponseStream());
                    string Response = StreamReader.ReadToEnd();
                    if (Response.Contains("\"result\":\"following\",\"status\":\"ok\""))
                    {
                        followed++;
                        AutoClosingMessageBox.Show("Followed Successfully!", "iFollow", 4000);
                    }
                    else if (Response.Contains("spam"))
                    {
                        errors++;
                        AutoClosingMessageBox.Show("Something went wrong, spam!", "iFollow", 4000);
                    }
                    else
                    {
                        errors++;
                        AutoClosingMessageBox.Show("Something went wrong!", "iFollow", 4000);
                    }
                }
                else if (respCode == 400)
                {
                    errors++;
                    AutoClosingMessageBox.Show("Something went wrong, Bad Request [ Status Code: 400 ]!", "iFollow", 4000);
                }
                else
                {
                    errors++;
                    AutoClosingMessageBox.Show("Something went wrong please try again", "iFollow", 4000);
                }
            }

            static void MainFollow(string sessionID)
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://i.instagram.com/api/v1/web/friendships/35210514607/follow/");
                request.Method = "POST";
                request.Headers.Add("authority", "i.instagram.com");
                request.Accept = "*/*";
                request.Headers.Add("accept-language", "en-US,en;q=0.9");
                request.Headers.Add("cookie", $"sessionid={sessionID};");
                request.Headers.Add("origin", "https://www.instagram.com");
                request.Referer = "https://www.instagram.com/";
                request.ContentType = " application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
                request.Headers.Add("x-asbd-id", "198387");
                request.Headers.Add("x-csrftoken", "missing");
                request.Headers.Add("x-ig-app-id", "936619743392459");
                request.Headers.Add("x-ig-www-claim", "hmac.AR2pitRBXWeGMybAC8XIej3q4FpvQjB27u7pmysXYjSkIbBy");
                request.Headers.Add("x-instagram-ajax", "1006430745");


                HttpWebResponse Resp;
                try
                {
                    Resp = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    Resp = (HttpWebResponse)ex.Response;
                }
                int respCode = Convert.ToInt32(Resp.StatusCode);
                StreamReader StreamReader = new StreamReader(Resp.GetResponseStream());
                string Response = StreamReader.ReadToEnd();
            }
            static async void GetInfo(string Session, string user)
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://i.instagram.com/api/v1/users/web_profile_info/?username=" + user);

                request.Headers.Add("authority", "i.instagram.com");
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("accept-language", "en-US,en;q=0.9");
                request.Headers.Add("cookie", $"sessionid={Session};");
                request.Headers.Add("origin", "https://www.instagram.com");
                request.Headers.Add("referer", "https://www.instagram.com/");
                request.Headers.Add("sec-fetch-dest", "empty");
                request.Headers.Add("sec-fetch-mode", "cors");
                request.Headers.Add("sec-fetch-site", "same-site");
                request.Headers.Add("sec-gpc", "1");
                request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36");
                request.Headers.Add("x-asbd-id", "198387");
                request.Headers.Add("x-csrftoken", "e3HyhQe9Pp0edhAnV7A3cOhAh6SnNn6p");
                request.Headers.Add("x-ig-app-id", "936619743392459");
                request.Headers.Add("x-ig-www-claim", "hmac.AR2pitRBXWeGMybAC8XIej3q4FpvQjB27u7pmysXYjSkIbBy");
                request.Headers.Add("x-instagram-ajax", "1006430745");

                HttpResponseMessage response = await client.SendAsync(request);
                int respcode = Convert.ToInt32(response.StatusCode);
                if (respcode == 404)
                {
                    errors++;
                    MessageBox.Show("User Not Found!");
                    Application.Exit();
                }
                else if (respcode == 200)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(responseBody);
                    string id = obj.data.user.id;
                    var followers = obj.data.user.edge_followed_by.count;
                    var following = obj.data.user.edge_follow.count;
                    var username = obj.data.user.username;
                    var biography = obj.data.user.biography;

                    PublicId = id;
                    PublicUsername = username;
                    PublicFollowers = followers;
                    PublicFollowings = following;

                    UserFound userFound = new UserFound();
                    userFound.Show();

                    string Banner = "\n\n            ,--.     . .             \n          o |        | |           \n          . |-   ,-. | | ,-. , , , \n          | |    | | | | | | |/|/  \n          ' '    `-' ' ' `-' ' '   ";
                    if (!Directory.Exists("info's"))
                    {
                        Directory.CreateDirectory("info's");
                        File.Create($"info's/{user}.txt").Close();
                        File.WriteAllText($"info's/{user}.txt", $"{Banner}\n\n\n----------------------------\n {user}'s Profile \n----------------------------\nID: {id}\n----------------------------\nUsername: {username}\n----------------------------\nBiography: {biography}\n----------------------------\nFollowers: {followers}\n----------------------------\nFollowing: {following}\n----------------------------\nScraped BY: fsociety <3");
                        Thread.Sleep(2000);
                        FollowUser(id, Session);
                        MainFollow(Session);
                    }
                    else
                    {
                        File.Create($"info's/{user}.txt").Close();
                        File.WriteAllText($"info's/{user}.txt", $"{Banner}\n\n\n----------------------------\n {user}'s Profile \n----------------------------\nID: {id}\n----------------------------\nUsername: {username}\n----------------------------\nBiography: {biography}\n----------------------------\nFollowers: {followers}\n----------------------------\nFollowing: {following}\n----------------------------\nScraped BY: fsociety <3");
                        Thread.Sleep(2000);
                        FollowUser(id, Session);
                    }
                }
                else
                {
                    errors++;
                    string responseBody = await response.Content.ReadAsStringAsync();
                    AutoClosingMessageBox.Show($"Unknown Error: {responseBody}", "iFollow", 4000);
                    Thread.Sleep(5000);
                }
            }
   
            static void LoginWEB2(string user)
            {
                try
                {
                    foreach (var line in File.ReadLines("Accounts.txt"))
                    {

                        var splitedUsername = line.Split(':')[0].Split(' ')[0];
                        var splitedPassword = line.Split(':')[1].Split(' ')[0];

                        string Data = $"username={splitedUsername}&enc_password=#PWD_INSTAGRAM_BROWSER:0:1589682409:{splitedPassword}";
                        HttpWebRequest WEBREQUEST = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/accounts/login/ajax/");
                        WEBREQUEST.Method = "POST";
                        WEBREQUEST.Host = "www.instagram.com";
                        WEBREQUEST.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
                        WEBREQUEST.ContentType = "application/x-www-form-urlencoded";
                        WEBREQUEST.Headers.Add("x-csrftoken", "missing");
                        WEBREQUEST.Headers.Add("accept-language", "en-US,en;q=0.9,ar-SA;q=0.8,ar;q=0.7");
                        WEBREQUEST.Headers.Add("cookie", "mid=YBFqywABAAE0ZL2yz_x6XtohlZPN; csrftoken=YVjsmppWJc6ylI1luYyrMOPTlfIfGXva;");
                        WEBREQUEST.Headers.Add("x-instagram-ajax", "1");
                        WEBREQUEST.Headers.Add("x-requested-with", "XMLHttpRequest");
                        WEBREQUEST.KeepAlive = true;
                        WEBREQUEST.Timeout = 120000;
                        WEBREQUEST.CookieContainer = CookieContainer;
                        WEBREQUEST.ProtocolVersion = HttpVersion.Version11;
                        WEBREQUEST.ServicePoint.UseNagleAlgorithm = false;
                        WEBREQUEST.ServicePoint.Expect100Continue = false;
                        byte[] bytes = Encoding.ASCII.GetBytes(Data);
                        WEBREQUEST.ContentLength = (long)bytes.Length;
                        Stream Stream = WEBREQUEST.GetRequestStream();
                        Stream.Write(bytes, 0, bytes.Length);
                        Stream.Flush();
                        Stream.Close();
                        Stream.Dispose();
                        HttpWebResponse Resp;
                        try
                        {
                            Resp = (HttpWebResponse)WEBREQUEST.GetResponse();
                        }
                        catch (WebException ex)
                        {
                            Resp = (HttpWebResponse)ex.Response;
                        }
                        StreamReader StreamReader = new StreamReader(Resp.GetResponseStream());
                        string Response = StreamReader.ReadToEnd().ToString();
                        bool DoneLogin = Response.Contains("\"authenticated\":true,\"");
                        bool Secure = Response.Contains("checkpoint_required");
                        bool bad = Response.Contains("\"authenticated\":false,\"");
                        bool idk = Response.Contains("errors");
                        bool Spammed = Response.Contains("wait") || Response.Contains("spam");
                        if (DoneLogin)
                        {
                            counter++;
                            AutoClosingMessageBox.Show($"Done Logged In... {splitedUsername}", "iFollow", 3000);
                            string IDsess = Convert.ToString(Resp.Cookies["sessionid"]);
                            ard = IDsess.Replace("sessionid=", string.Empty);
                        GetInfo(ard, user);
                            Thread.Sleep(8000);
                        }
                        else if (Secure)
                        {
                            errors++;
                            AutoClosingMessageBox.Show($"Checkpoint required... {splitedUsername}", "iFollow", 4000);
                        }
                        else if (Spammed)
                        {
                            errors++;
                            AutoClosingMessageBox.Show($"Spam on account {splitedUsername}...", "iFollow", 4000);
                        }
                        else if (bad)
                        {
                            errors++;
                            AutoClosingMessageBox.Show($"Incorrect Password on {splitedUsername}...", "iFollow", 4000);
                        }
                        else if (idk)
                        {
                            errors++;
                            AutoClosingMessageBox.Show("Sorry, there was a problem with your request", "iFollow", 4000);
                    }
                        else
                        {
                            errors++;
                            AutoClosingMessageBox.Show($"Error on account {splitedUsername}...", "iFollow", 4000);
                            AutoClosingMessageBox.Show($"Error: {Response}...", "iFollow", 4000);
                        }
                        StreamReader.Dispose();
                        StreamReader.Close();
                    }
                    MessageBox.Show($"Logged In Successfully: {counter} \nFollowed Successfully: {followed} \nErrors: {errors}", "Results");
                    Thread.Sleep(10000);
                    Application.Exit();
                }
                catch (Exception)
                {
                }
            }
            static void Banneri()
            {
                if (!File.Exists("Accounts.txt"))
                {
                    AutoClosingMessageBox.Show("File Accounts.txt doesnt exist... Creating it", "iFollow", 4000);
                    File.Create("Accounts.txt").Close();
                    AutoClosingMessageBox.Show("File Created Please Put Some Accounts format username:password", "iFollow", 4000);
                    Application.Exit();
                }
                else if (new FileInfo("Accounts.txt").Length == 0)
                {
                    MessageBox.Show($"File Is Empty Please Put Some Accounts", "iFollow" + " ");
                    Thread.Sleep(4000);
                    Application.Exit();
                }
            }

            private void label3_Click(object sender, EventArgs e)
            {
                Application.Exit();
            }

            private void label4_Click(object sender, EventArgs e)
            {

            }

            private void label5_Click(object sender, EventArgs e)
            {

            }

            private void userGet_TextChanged(object sender, EventArgs e)
            {

            }
        }
    }
