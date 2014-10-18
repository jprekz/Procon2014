using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Specialized;

namespace ProconFileIO
{
    public class ClientLibrary
    {
        string ServerURL;
        string FileSavePath;
        string ProblemLocation;
        string SubmitLocation;
        string ProblemFileNameFormat;
        string playerid;

        string FileName;
        int PloblemID;

        public string Respons;

        public ClientLibrary()
        {

            ServerURL = "http://172.16.1.2";
            playerid = "3965475495";
            ServerURL = "http://localhost";
            playerid = "1";

            //writing after
            ProblemLocation = "/problem/";

            //SubmitFormにしろと言われたけどこれでおk?->おk
            SubmitLocation = "/SubmitAnswer";

            FileSavePath = "C:\\Users\\" + Environment.UserName + "\\Desktop\\";

            ProblemFileNameFormat = "prob{0:D2}.ppm";

        }

        public string GetProblemPath(int Ploblemid)
        {

            PloblemID = Ploblemid;

            FileName = string.Format(ProblemFileNameFormat, Ploblemid);

            var URI = ServerURL + ProblemLocation + FileName;

            using (var webclient = new WebClient())
            {
                //本番用URL
                //resource =  webclient.DownloadData(ServerURL + ProblemLocation + GetProblemFileName(ProblemID));

                /*
                 * やめました
                 * byte[] resource;
                 * resource =  webclient.DownloadData(ServerURL + ProblemLocation + ProblemID);
                 *
                 * using (var SaveFile = File.OpenWrite(FileSavePath + GetProblemFileName(ProblemID)))
                 * {
                 *      SaveFile.Write(resource, 0, resource.Length);
                 * }
                 */
                try
                {
                    webclient.DownloadFile(URI, FileSavePath + FileName);
                }
                catch (WebException err)
                {
                    return err.Status.ToString();
                }
            }

            return FileSavePath + FileName;
        }

        public void SubmitAnswer(string ans)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SubmitThread));
            t.IsBackground = true;
            t.Start(ans);
        }

        private void SubmitThread(object param)
        {       
            string ans = (string)param;

            using (var webclient = new WebClient())
            {
                using (var wc = new WebClient())
                {
                    var AnswerCollention = new NameValueCollection();
                    AnswerCollention.Add("playerid", playerid);
                    AnswerCollention.Add("problemid", PloblemID.ToString());
                    AnswerCollention.Add("answer", ans);

                    //本番
                    Respons = Encoding.UTF8.GetString(wc.UploadValues(ServerURL + SubmitLocation, AnswerCollention));
                    //テスト
                    //Respons = "TEST RESPONS"; Thread.Sleep(1000);

                }
                OnReturnRespons(new EventArgs());
            }
        }

        public event EventHandler ReturnRespons;
        private void OnReturnRespons(EventArgs e)
        {
            if (null != ReturnRespons) ReturnRespons(this, e);
        }

    }
}