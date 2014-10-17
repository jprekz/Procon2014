using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace ProconFileIO
{
    public class ClientLibrary
    {
        string ServerURL;
        string FileSavePath;
        string ProblemLocation;
        string ProblemFileNameFormat;

        public ClientLibrary()
        {
            //練習場のURLに設定されています
            ServerURL = "http://procon2014-practice.oknct-ict.org";

            //writing after
            ProblemLocation = "/problem/ppm/";

            FileSavePath = "C:\\Users\\" + Environment.UserName + "\\Desktop\\";

            ProblemFileNameFormat = "prob{0:D2}.ppm";

        }
        
        public string GetProblemID (int ProblemID){

            var FileName = string.Format(ProblemFileNameFormat, ProblemID);

            var URI = ServerURL + ProblemLocation + ProblemID;

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

                    //やめました
                    //byte[] resource;
                    //resource =  webclient.DownloadData(ServerURL + ProblemLocation + ProblemID);
                    //
                    //using (var SaveFile = File.OpenWrite(FileSavePath + GetProblemFileName(ProblemID)))
                    //  {
                    //      SaveFile.Write(resource, 0, resource.Length);
                    //  }
                
                    webclient.DownloadFile(URI, FileSavePath + FileName); 
                }
            }

            return FileSavePath + FileName;
        }

        public void SubmitAnswer()
        {

        }
    }
}