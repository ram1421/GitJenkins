using EAGetMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitJenkins
{
    public partial class Build : Form
    {
        public Build()
        {
            InitializeComponent();
        }
        private static readonly HttpClient client = new HttpClient();
        private async void Build_Load(object sender, EventArgs e)
        {

            // Gmail IMAP4 server is "imap.gmail.com"
            // Get these values from App.config
            MailServer oServer = new MailServer("imap.gmail.com",
                        "@gmail.com", "", ServerProtocol.Imap4);
            MailClient oClient = new MailClient("TryIt");

            // Set SSL connection,
            oServer.SSLConnection = true;

            // Set 993 IMAP4 port
            oServer.Port = 993;

            try
            {
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos();
                for (int i = 0; i < infos.Length; i++)
                {
                    MailInfo info = infos[i];
                    Console.WriteLine("Index: {0}; Size: {1}; UIDL: {2}",
                        info.Index, info.Size, info.UIDL);

                    // Download email from GMail IMAP4 server
                    Mail oMail = oClient.GetMail(info);

                    //1st layer to isolate the spam
                    if(oMail.From == new MailAddress("noreply@github.com"))
                    {
                        // check 
                    }

                    // 2nd layer - this is an additional layer of security to isolate the spam or wrong emails. 
                    // Checking if the configured secret in github maps
                    string password = ((EAGetMail.HeaderItem)(oMail.Headers[oMail.Headers.SearchKey("Approved")])).HeaderValue;

                    Console.WriteLine("From: {0}", oMail.From.ToString());
                    Console.WriteLine("Subject: {0}\r\n", oMail.Subject);

                    // Get the repo name from the emailbody
                    string[] lines = oMail.HtmlBody.Split(
                                     new[] { "\r\n", "\r", "\n" },
                                     StringSplitOptions.None);

                    string homePath = lines.FirstOrDefault(t => t.Contains("Home"));

                    string repoName = homePath.Substring(homePath.LastIndexOf('/'));

                    // Passing the reponame retrived from Htmlbody as a parameter to invoke the build.
                    // Crucial to have the same name 
                    await postRequest(repoName.Trim());


                    //TODO:
                    // if success -> Mark the email as read or delete. 
                    // if failure to invoke the build -> notify with the exception message. 

                }

                // Quit and purge emails marked as deleted from Gmail IMAP4 server.
                oClient.Quit();
            }
            catch (Exception ep)
            {
                Console.WriteLine(ep.Message);
            }
        }

        private static async Task postRequest(string repoName = "FreeStyleJenkins")
        {
            var values = new Dictionary<string, string>();
            var content = new FormUrlEncodedContent(values);

            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(
            System.Text.ASCIIEncoding.ASCII.GetBytes(
            string.Format("{0}:{1}", "username", "apiToken"))));

            //send POST to URL with username and token
            var response = await client.PostAsync(string.Format("http://username:token@JenkinsURL:8080/job/{0}/build?token=Token", repoName), content);
            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
