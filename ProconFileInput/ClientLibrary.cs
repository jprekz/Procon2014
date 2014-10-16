using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

/*  #Todo
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
            var ProblemLocation = "/problem/ppm/";
            var FileSavePath = "C:\\Users\\" + Environment.UserName + "\\Desktop\\";

            if (FilehavedownloadedFlag(FileSavePath + GetProblemFileName(ProblemID)))
            {
                return FileSavePath + GetProblemFileName(ProblemID);
            }
            
            else
            {
                //練習場のURLに設定されています
                var ServerURL = "http://procon2014-practice.oknct-ict.org";

                //やめました
                //byte[] resource;

                using (var webclient = new WebClient())
                {
                    //本番用URL
                    //resource =  webclient.DownloadData("http://" + ServerURL + ProblemLocation + GetProblemFileName(ProblemID));
                    
                    //やめました
                    //resource =  webclient.DownloadData(ServerURL + ProblemLocation + ProblemID);

                    webclient.DownloadFile(ServerURL + ProblemLocation + ProblemID,  FileSavePath + GetProblemFileName(ProblemID));
                }


                /*やめました
                using (var SaveFile = File.OpenWrite(FileSavePath + GetProblemFileName(ProblemID)))
                {
                    SaveFile.Write(resource, 0, resource.Length);
                }
                */

                return FileSavePath + GetProblemFileName(ProblemID);
            }
        }
        public string GetProblemFileName(int problemID)
        {
            string ProblemFileNameFormat = "prob{0:D2}.ppm";
            return string.Format(ProblemFileNameFormat, problemID);
        }

        public bool FilehavedownloadedFlag(string FilePath)
        {
            return File.Exists(FilePath);
        }
    }
}