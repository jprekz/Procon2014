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
        string FileName;
        int PloblemID;

        public ClientLibrary()
        {
            //練習場のURLに設定されています
            ServerURL = "http://procon2014-practice.oknct-ict.org";

            //writing after
            ProblemLocation = "/problem/ppm/";

            SubmitLocation = "/SubmitAnswer";

            FileSavePath = "C:\\Users\\" + Environment.UserName + "\\Desktop\\";

            ProblemFileNameFormat = "prob{0:D2}.ppm";

        }

        public string GetProblemPath(int Ploblemid)
        {

            PloblemID = Ploblemid;

            FileName = string.Format(ProblemFileNameFormat, Ploblemid);

            var URI = ServerURL + ProblemLocation + Ploblemid;

            if (File.Exists(FileSavePath + FileName) == true)
            {
                return FileSavePath + FileName;
            }

            else
            {
                using (var webclient = new WebClient())
                {
                    //本番用URL
                    //resource =  webclient.DownloadData("http://" + ServerURL + ProblemLocation + GetProblemFileName(ProblemID));

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

                    webclient.DownloadFile(URI, FileSavePath + FileName);
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
                byte[] respons;
                string playerid = "3965475495";
                using (var wc = new WebClient())
                {
                    var AnswerCollention = new NameValueCollection();
                    AnswerCollention.Add("playerid", playerid);
                    AnswerCollention.Add("ploblemid", PloblemID.ToString());
                    AnswerCollention.Add("answer", ans);
                    respons = wc.UploadValues(ServerURL + SubmitLocation, AnswerCollention);



                }


            }
        }
    }
}