using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSA_Cosole
{
    class Program
    {
        private static FileHandler m_FileHandler = null;
        private static RSAHandler m_RSAHandler = null;
      

        static void Main(string[] args)
        {
            m_FileHandler = new FileHandler();
            m_RSAHandler = new RSAHandler();
            Menue();
            
        }

        static void Menue()
        {
            bool blnExit = false;
            
             while (!blnExit)
             {
                 Console.Clear();
                 DrawMenue();


                 ConsoleKey cki = Console.ReadKey().Key;
                 switch (cki)
                 {
                     case ConsoleKey.E:
                     case ConsoleKey.D:
                         string strFile = InputFileName();
                         byte[] nArFile = null;
                         if ((!String.IsNullOrEmpty(strFile))&&m_FileHandler.TryReadFileByteWise(strFile,out nArFile))
                         {
                            Console.WriteLine("Do you want to see the file bytewise? <Y / AnyKey>");
                           
                            ConsoleKey ck = Console.ReadKey().Key;
                         
                            if (ck == ConsoleKey.Y)
                            { 
                                ShowByteAr(nArFile);
                            }else{/*Do not show*/}

                            if (cki == ConsoleKey.E)
                            {
                                HandleEncryption(nArFile,strFile);
                            }
                            else
                            {
                                HandleDecryption(nArFile,strFile);
                            }
                         }
                         
                         else
                         {
                             Console.WriteLine("Canceled!");
                             Thread.Sleep(3000);
                         }
                         break;
                     
                     
                     case ConsoleKey.Escape:
                         blnExit = true;
                         break;                 }
             }
        }

        private static void HandleDecryption(byte[] nArCypher,string strFile)
        {
            long nN = InputNumber();           
            byte[] nArMessage = null;
            if (nN > 0)
            {
                nArMessage = m_RSAHandler.Decrypt(nArMessage, nN);
            }
            else
            {
                Console.WriteLine("Canceled!");
                Thread.Sleep(3000);
            }
           
        }

        private static void HandleEncryption(byte[] nArMessage, string strFile)
        {
            byte[] nArCypher = null;
            long nPrime1 = InputPrimeNumber();
            if (nPrime1 > 0)
            {
                long nPrime2 = InputPrimeNumber();
                if (nPrime2 > 0)
                {
                    Int64 nN;
                    nArCypher = m_RSAHandler.Encrypt(nArMessage,nPrime1,nPrime2,out nN);
                    Console.WriteLine("Ecryption done. N: "+((long)nN).ToString());

                    Console.WriteLine("Do you want to see the encrypted file bytewise? <Y / AnyKey>");
                    ConsoleKey ck = Console.ReadKey().Key;
                    if (ck == ConsoleKey.Y)
                    {
                        ShowByteAr(nArCypher);
                    }
                    else { /*Do not show*/}
           
                    string strNewFile = m_FileHandler.GetNewFileName(strFile, "_Encrypted");
                    try
                    {
                        m_FileHandler.WriteFileByteWise(strNewFile, nArCypher);
                        Console.WriteLine("Encrypted file written: "+strNewFile);
                    }
                    catch
                    {
                        Console.WriteLine("Error while writing encrypted file: " + strNewFile);
                    }


                }
                else
                {
                    Console.WriteLine("Canceled!");
                    Thread.Sleep(3000);
                }
            }
            else
            {
                Console.WriteLine("Canceled!");
                Thread.Sleep(3000);
            }
        }

      

        private static void ShowByteAr(byte[] nArFile)
        {
            throw new NotImplementedException();
        }

        private static void DrawMenue()
        {
            Console.WriteLine("RSA:\r\n");
            Console.WriteLine("EnCrypt file    <E>");
            Console.WriteLine("Decrypt file   <D>");
            Console.WriteLine("Exit         <ESC>\r\n");

        }



        private static int InputPrimeNumber()
        {
            int nRet = 0;
            int nCur = 0;
            string strCur = String.Empty;
            bool blnIsValid = false;
            bool blnExit = false;
            string strPath = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;
            Console.WriteLine("\r\n\r\n");
            while (!blnIsValid && !blnExit)
            {

                Console.WriteLine("Enter a valid primenumber between 100 and 10,000:");
                strCur = Console.ReadLine();
                if (strCur.ToLower() == "cancel")
                {
                    blnExit = true;
                }
                else if (Int32.TryParse(strCur, out nCur))
                {
                    if (m_RSAHandler.IsPrimeNumber(nCur))
                    {
                        nRet = nCur;
                        blnIsValid = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Not a primenumner between 100 and 10,000: " + strCur);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a primenumner between 100 and 10,000: " + strCur);
                    Console.WriteLine("Enter \"cancel\" to get back to the main-menue");
                }
            }

            return nRet;
        }

        private static int InputNumber()
        {
            int nRet = 0;
            int nCur = 0;
            string strCur = String.Empty;
            bool blnIsValid = false;
            bool blnExit = false;
            string strPath = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;
            Console.WriteLine("\r\n\r\n");
            while (!blnIsValid && !blnExit)
            {

                Console.WriteLine("Enter a valid number:");
                strCur = Console.ReadLine();
                if (strCur.ToLower() == "cancel")
                {
                    blnExit = true;
                }
                else if (Int32.TryParse(strCur, out nCur))
                {
                        nRet = nCur;
                        blnIsValid = true;                   
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not valid number: " + strCur);
                    Console.WriteLine("Enter \"cancel\" to get back to the main-menue");
                }
            }

            return nRet;
        }

        

        private static string InputFileName()
        {
            string strRet = String.Empty;
            string strCur = String.Empty;
            bool blnIsValid = false;
            bool blnExit = false;
            string strPath = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;
            Console.Clear();
            while (!blnIsValid&&!blnExit)
            {
                
                Console.WriteLine("Enter a valid filename:");
                strCur = Console.ReadLine();
                if (strCur.ToLower() == "cancel")
                {
                    blnExit = true;
                }
                else if (File.Exists(Path.Combine(strPath, strCur)))
                {
                    strRet = Path.Combine(strPath, strCur);
                    blnIsValid = true;
                }
                else if (File.Exists(strCur))
                {
                    strRet = strCur;
                    blnIsValid = true;
                }
                else 
                { 
                    Console.Clear();
                    Console.WriteLine("Invalid File: "+strCur);
                    Console.WriteLine("Enter \"cancel\" to get back to the main-menue");
                }
            }

            return strRet;
        }

    }
}
