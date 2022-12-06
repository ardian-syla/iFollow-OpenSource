# iFollow Source Code
iFollow c# GUI with code. 
you can modify it as you want <3

# Author
```
MADE WITH HEART BY ArdianS :)
```
```
This is just a part of code that happen's to follow a user on instagram from csharp
```
```csharp
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
```
```
Don't forget to open Visual Studio and build the code
```
# Issuess

``` 
If any issue happen's, kindly open an Issue and i will help you instantly 
```

