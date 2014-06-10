using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Cosole
{
    public class FileHandler
    {
      
        public bool TryReadFileByteWise(string strFileName, out byte[] nArFile)
        {
            bool blnSuccess = false;
            nArFile = new byte[0];

            try
            {
                nArFile = File.ReadAllBytes(strFileName);
                blnSuccess = true;
            }
            catch
            {}

            return blnSuccess;
        }

        public bool WriteFileByteWise(string strFileName, byte[] nArFile)
        {
            bool blnSuccess = false;
            try
            {
                File.WriteAllBytes(strFileName,nArFile);
                blnSuccess = true;
            }
            catch
            { }
            return blnSuccess;
        }

        public string GetNewFileName(string strFile, string strPostfix)
        {
            string strPath = Path.GetDirectoryName(strFile);
            string strBlankFIle = Path.GetFileNameWithoutExtension(strFile);
            string strExtension = Path.GetExtension(strFile);
            string strNewFile = strBlankFIle + strPostfix + strExtension;
            strNewFile = Path.Combine(strPath, strNewFile);
            return strNewFile;
        }

    }
}
