using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

/*
 *  #Todo
 *  - FileSavePathの値の設定を外部から行えるようにする
 *      - XMLとかCSVとか？
 *  - エラー処理
 *      - タイムアウト(403も), 404, etc...
 */

namespace ProconFileInput
{
    public class ClientLibrary
    {
        
        public string GetProblemID (int ProblemID){

            //writing after
            var ServerURL = "http://procon2014-practice.oknct-ict.org";
            var ProblemLocation = "/problem/ppm/";
            var FileSavePath = "C:\\";

            byte[] resource;

            using (var webclient = new WebClient())
            {
               //resource =  webclient.DownloadData("http://" + ServerURL + ProblemLocation + GetProblemFileName(ProblemID));
               resource =  webclient.DownloadData("http://" + ServerURL + ProblemLocation + ProblemID);
            }

            using (var SaveFile = File.OpenWrite(FileSavePath))
            {
                SaveFile.Write(resource, 0, resource.Length);
            }

            return FileSavePath;
        }

        public string GetProblemFileName(int problemID)
        {
            string ProblemFileNameFormat = "prob{0:D2}.ppm";
            return string.Format(ProblemFileNameFormat, problemID);
        }
    }
}