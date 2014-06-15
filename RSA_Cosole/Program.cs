using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSA_Cosole
{
    /// <summary>
    /// Text based user interface for crypting and encrypting files via RSA
    /// Author: Tobias Arendt
    /// </summary>
    class TuiRSA
    {
        #region statics
        private static FileHandler m_FileHandler = null;
        private static RSAHandler m_RSAHandler = null; 
        #endregion

        #region Main
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">No Arguments used in this program</param>
        static void Main(string[] args)
        {
            m_FileHandler = new FileHandler();
            m_RSAHandler = new RSAHandler(10000);
            Menue();
        }
        
        #endregion  

        #region Crypting

        /// <summary>
        /// Handles input of neccessary data for encrypting the message. Triggers the encrypting if inupt is valid.
        /// </summary>
        /// <param name="nArMessage">Plaintext, that will be encrypted.</param> 
        /// <param name="strFile">Original filename. On writing the encrypted file it will be uses as prefix(Test.txt-->Test_Encrypted.txt)</param>
        private static void HandleEncryption(byte[] nArMessage, string strFile)
        {
            int[] nArCypher = null;
            int nPrime1 = InputPrimeNumber();
            if (nPrime1 > 0)
            {
                int nPrime2 = InputPrimeNumber();
                if (nPrime2 > 0)
                {
                    int nN;
                    int nD;
                    nArCypher = m_RSAHandler.Encrypt(nArMessage, nPrime1, nPrime2, out nN, out nD);


                    Console.WriteLine("Show encrypted file bytewise? <Y / AnyKey>");
                    ConsoleKey ck = Console.ReadKey().Key;
                    if (ck == ConsoleKey.Y)
                    {
                        ShowInput(nArCypher);
                    }
                    else { /*Do not show*/}

                    Console.WriteLine(String.Format("Ecryption done. Note down: n={0}  d={1}", nN, nD));

                    string strNewFile = m_FileHandler.GetNewFileName(strFile, "_Encrypted");
                    try
                    {
                        m_FileHandler.WriteFile(strNewFile, nArCypher);
                        Console.WriteLine("Encrypted file written: " + strNewFile);
                    }
                    catch
                    {
                        Console.WriteLine("Error while writing encrypted file: " + strNewFile);
                    }
                    Console.ReadKey();



                }
                else
                {
                    Console.WriteLine("Canceled!");

                }
            }
            else
            {
                Console.WriteLine("Canceled!");
            }
            Thread.Sleep(3000);//Let the user time to read it.
        }

        /// <summary>
        /// Handles input of neccessary data for decrypting the message. Triggers the decrypting if inupt is valid.
        /// </summary>
        /// <param name="nArCypher"> Plaintext, that will be encrypted.</param>
        /// <param name="strFile">Original filename. On writing the decrypted file it will be uses as prefix(Test.txt-->Test_Decrypted.txt)</param>
        private static void HandleDecryption(byte[] nArCypher, string strFile)
        {
            Console.WriteLine("\r\nPublic key n");
            int nN = InputNumber();
            byte[] nArMessage = null;
            if (nN > 0)
            {
                Console.WriteLine("\r\nPublic key d");
                int nD = InputNumber();
                if (nD > 0)
                {
                    nArMessage = m_RSAHandler.Decrypt(nArCypher, nN, nD);
                    if (nArMessage != null)
                    {
                        Console.WriteLine("Show the decrypted file bytewise? <Y / AnyKey>");
                        ConsoleKey ck = Console.ReadKey().Key;
                        if (ck == ConsoleKey.Y)
                        {
                            ShowInput(nArMessage);
                        }
                        else { /*Do not show*/}
                        string strNewFile = m_FileHandler.GetNewFileName(strFile, "_Decrypted");
                        try
                        {
                            m_FileHandler.WriteFile(strNewFile, nArMessage);
                            Console.WriteLine("Decrypted file written: " + strNewFile);
                        }
                        catch
                        {
                            Console.WriteLine("Error while writing decrypted file: " + strNewFile);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error while decrypted the file: \r\n" + strFile+ "\r\n\r\nIt doesn't seem te be an encryted file.\r\nPress any key to return to the main meue.");
                    }
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Canceled!");

                }
            }
            else
            {
                Console.WriteLine("Canceled!");

            }

        }
        #endregion

        #region Output

        /// <summary>
        /// Show the an int array intwise.
        /// </summary>
        /// <param name="nAr">The array, we want to show in the onsole</param>
        private static void ShowInput(int[] nAr)
        {
            Console.WriteLine();
            for (int i = 0; i < nAr.Length; i++)
            {
                Console.WriteLine(nAr[i].ToString().PadLeft(3, ' '));
            }
        }

        /// <summary>
        /// Show the an byte array bytewise in binary, decimal, hexadecimal and char view.
        /// </summary>
        /// <param name="nAr"></param>The array, we want to shown in console
        private static void ShowInput(byte[] nAr)
        {           
            string[] strOutputAsHex = (BitConverter.ToString(nAr)).Split('-');//Get Hexstring and split it into an Array
            Console.WriteLine("");
            for (int i = 0; i < nAr.Length; i++)
            {
                string strLine = String.Format("{0}   {1}   {2}   {3}", Convert.ToString(nAr[i], 2).PadLeft(8, '0'), nAr[i].ToString().PadLeft(3), strOutputAsHex[i], System.Text.Encoding.UTF8.GetString(new byte[1] { nAr[i] }));
                Console.WriteLine(strLine);
            }
        }

        /// <summary>
        /// Show all options
        /// </summary>
        private static void DrawMenue()
        {
            Console.WriteLine("RSA:\r\n");
            Console.WriteLine("Encrypt file    <E>");
            Console.WriteLine("Decrypt file    <D>");
            Console.WriteLine("Exit           <ESC>\r\n");
        } 
        #endregion

        #region Input

        /// <summary>
        /// Interaction with user. Shows all options and waits for input.
        /// </summary>
        static void Menue()
        {
            bool blnExit = false;

            while (!blnExit)
            {
                Console.Clear();
                DrawMenue();


                ConsoleKey cki = Console.ReadKey().Key;
                switch (cki)//Whichs key was pressed?
                {
                    case ConsoleKey.E:
                    case ConsoleKey.D:
                        string strFile = InputFileName();
                        byte[] nArFile = null;
                        if ((!String.IsNullOrEmpty(strFile)) && m_FileHandler.TryReadFileByteWise(strFile, out nArFile))
                        {
                            Console.WriteLine("Show the file bytewise? <Y / AnyKey>");

                            ConsoleKey ck = Console.ReadKey().Key;

                            if (ck == ConsoleKey.Y)
                            {
                                ShowInput(nArFile);
                            }
                            else { /*Do not show*/}

                            if (cki == ConsoleKey.E)
                            {
                                HandleEncryption(nArFile, strFile);
                            }
                            else
                            {
                                HandleDecryption(nArFile, strFile);
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
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the Input of a prime number.
        /// </summary>
        /// <returns>Primenumber. If user breaks up, 0 will be returned./returns>
        private static int InputPrimeNumber()
        {
            int nRet = 0;
            int nCur = 0;
            string strCur = String.Empty;
            bool blnIsValid = false;
            bool blnExit = false;
            string strPath = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;
            Console.WriteLine("\r\n\r\n");
            while (!blnIsValid && !blnExit)//Neither a valid primenumber nor the user wants to cancel.
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
                    {//Not a prime number - inform user and ask again
                        Console.Clear();
                        Console.WriteLine("Not a primenumner between 100 and 10,000: " + strCur);
                    }
                }
                else
                {//Not a number - inform user and ask again
                    Console.Clear();
                    Console.WriteLine("Not a primenumner between 100 and 10,000: " + strCur);
                    Console.WriteLine("Enter \"cancel\" to get back to the main-menue");
                }
            }

            return nRet;
        }

        /// <summary>
        /// Handles the Input of a number.
        /// </summary>
        /// <returns>The user input</returns>
        private static int InputNumber()
        {
            int nRet = 0;
            int nCur = 0;
            string strCur = String.Empty;
            bool blnIsValid = false;
            bool blnExit = false;
            string strPath = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;
            Console.WriteLine("\r\n");
            while (!blnIsValid && !blnExit)//Neither an valid number nor the user wants to cancel.
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
                {//Not a number - inform user and ask again
                    Console.Clear();
                    Console.WriteLine("Not valid number: " + strCur);
                    Console.WriteLine("Enter \"cancel\" to get back to the main-menue");
                }
            }

            return nRet;
        }


        /// <summary>
        /// Handles input of an valid(existing) filename
        /// </summary>
        /// <returns>The user input</returns>
        private static string InputFileName()
        {
            string strRet = String.Empty;
            string strCur = String.Empty;
            bool blnIsValid = false;
            bool blnExit = false;
            string strPath = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Directory.FullName;
            Console.Clear();
            while (!blnIsValid && !blnExit)
            {

                Console.WriteLine("Enter a valid filename:");
                strCur = Console.ReadLine();
                if (strCur.ToLower() == "cancel")
                {
                    blnExit = true;
                }
                else if (File.Exists(Path.Combine(strPath, strCur)))//check if file is in executable dir
                {//Cool
                    strRet = Path.Combine(strPath, strCur);
                    blnIsValid = true;
                }
                else if (File.Exists(strCur))//check path easily
                {//Cool
                    strRet = strCur;
                    blnIsValid = true;
                }
                else
                { //Not a - inform user and ask again
                    Console.Clear();
                    Console.WriteLine("Invalid File: " + strCur);
                    Console.WriteLine("Enter \"cancel\" to get back to the main-menue");
                }
            }

            return strRet;
        } 
        #endregion

    }
}
