using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace ProconFileInput
{
    class ClientLibrary
    {
        
        public static string getProblemID (int ProblemID){
            //writing after
            var ServerURL = "";
            var PloblemLocation = "";
            var FileSavePath = "";

            byte[] resource;

            using (var webclient = new WebClient())
            {
                resource =  webclient.DownloadData("http://" + ServerURL + PloblemLocation + GetProblemFileName(ProblemID));
            }

            using (var SaveFile = File.OpenWrite(FileSavePath))
            {
                SaveFile.Write(resource, 0, resource.Length);
            }

            return FileSavePath;
        }

        public static string GetProblemFileName(int problemID)
        {
            string ProblemFileNameFormat = "prob{0:D2}.ppm";
            return string.Format(ProblemFileNameFormat, problemID);
        }
    }
}