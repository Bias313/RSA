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
        #region Read
        /// <summary>
        /// Tries to read file bytewise and returns success. The read file is written to the out variable.
        /// </summary>
        /// <param name="strFileName"></param> file to read
        /// <param name="nArFile"></param> OUT - read File
        /// <returns></returns>
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
            { }

            return blnSuccess;
        }
        
        #endregion

        #region Write
        /// <summary>
        /// Writes byte array to the file
        /// </summary>
        /// <param name="strFileName">Target filename</param>
        /// <param name="nArFile">File to Write</param>
        /// <returns>Success</returns>
        public bool WriteFile(string strFileName, byte[] nArFile)
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
    
        /// <summary>
        /// Writes and int array converted to a byte array to the file
        /// </summary>
        /// <param name="strNewFile">Target filename</param>
        /// <param name="nIntAr">Array to write to file</param>
        /// <returns></returns>
        public bool WriteFile(string strNewFile, int[] nIntAr)
        {
            byte[] nByteAr = IntArToByteAr(nIntAr);
            return WriteFile(strNewFile, nByteAr);
        }     

        #endregion

        #region Helpfull

        /// <summary>
        /// Appends an postfix to an existing filename while keeping the path and extension
        /// </summary>
        /// <param name="strFile">original filename</param>
        /// <param name="strPostfix">postfix</param>
        /// <returns></returns>
        public string GetNewFileName(string strFile, string strPostfix)
        {
            string strPath = Path.GetDirectoryName(strFile);
            string strBlankFIle = Path.GetFileNameWithoutExtension(strFile);
            string strExtension = Path.GetExtension(strFile);
            string strNewFile = strBlankFIle + strPostfix + strExtension;
            strNewFile = Path.Combine(strPath, strNewFile);
            return strNewFile;
        }
       
        /// <summary>
        /// Writes byte array to the file
        /// </summary>
        /// <param name="intAr">Int-Array to convert</param>
        /// <returns>Byte-Array</returns>
        public byte[] IntArToByteAr(int[] intAr)
        {
            byte[] nByteAr = new byte[intAr.Length * 4];
            for (int i = 0; i < intAr.Length; i++)
                Array.Copy(BitConverter.GetBytes(intAr[i]), 0, nByteAr, 4 * i, 4);
            return nByteAr;
        }
        #endregion

       
    }
}
