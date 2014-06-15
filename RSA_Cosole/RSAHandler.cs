using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSA_Cosole
{
    /// <summary>
    /// Contains all neccessary methods for en- and decrypting byte[] via RSA
    /// 
    /// author: Tobias Arendt
    /// </summary>
    public class RSAHandler
    {
        #region Member
        //Array of all prime number from 3 to nMax(set in ctor)
        private int[] m_arPrimeNums = null;
        #endregion

        #region Ctor

        /// <summary>
        /// Finding all neccessary prime numbers
        /// </summary>
        /// <param name="nMax">Upper bound for prime numbers</param>
        public RSAHandler(int nMax)
        {
            m_arPrimeNums = SieveOfErathostenes(nMax);
        } 
        #endregion

        #region Crypt
        /// <summary>
        /// Encrypts via RSA
        /// </summary>
        /// <param name="nArMessage">Messsage to encrypt</param>
        /// <param name="nPrime1">Prime number</param>
        /// <param name="nPrime2">Prime number</param>
        /// <param name="nN">OUT n will be calculated in the method</param>
        /// <param name="nD">OUT d will be calculated in the method</param>
        /// <returns>Cyphertext</returns>
        public int[] Encrypt(byte[] nArMessage, int nPrime1, int nPrime2, out int nN, out int nD)
        {
            int[] nArCypher = new Int32[nArMessage.Length];

            nN = nPrime1 * nPrime2;
            int nPhiOfN = (nPrime1 - 1) * (nPrime2 - 1);
            int nE = GetRelativelyPrime(nPhiOfN);

            nD = modInverse(nE, nPhiOfN);

            for (int i = 0; i < nArMessage.Length; i++)
            {
                int nM = nArMessage[i];
                Int32 nCypher = (Int32)BigInteger.ModPow(nM, nE, nN);//nM ^ nE % nN 
                nArCypher[i] = nCypher;
            }
            return nArCypher;
        }

      

        /// <summary>
        /// Decrypts via RSA
        /// </summary>
        /// <param name="nArCypher">Cyphertext to decrypt</param>
        /// <param name="nN">n</param>
        /// <param name="nD">d</param>
        /// <returns>Plaintext</returns>
        public byte[] Decrypt(byte[] nArCypher, int nN, int nD)
        {
            int[] nIntAr = ByteArToIntAr(nArCypher);
            byte[] nArMessage = new byte[nIntAr.Length];

            for (int i = 0; i < nIntAr.Length; i++)
            {
                int nCur = (Int32)BigInteger.ModPow(nIntAr[i], nD, nN);//nM ^ nD % nN 
                nArMessage[i] = Convert.ToByte(nCur);
            }

            return nArMessage;
        }
        
        #endregion

        #region Mathematic methods

        /// <summary>
        /// Base to e-th power
        /// </summary>
        /// <param name="nBase">Base</param>
        /// <param name="nE">Exponent</param>
        /// <returns></returns>
        private long Pow(int nBase, int nE)
        {
            long nRet = 1;
            for (int i = 0; i < Math.Abs(nE); i++)
            {
                nRet *= nBase;
            }
            return nRet;
        }

        /// <summary>
        /// Checks if nCur is an primenumber.
        /// </summary>
        /// <param name="nCur">Number to check</param>
        /// <returns>true when potentialPN is a prime number</returns>
        public bool IsPrimeNumber(int potentialPN)
        {
            return m_arPrimeNums.Contains<int>(potentialPN);
        }

        /// <summary>
        /// Returns a number, that's ralatively prime(teilerfremd) to nCur.
        /// </summary>
        /// <param name="nCur">Number to find a ralatively prime for</param>
        /// <returns>A relatively prime number to nCur</returns>
        private int GetRelativelyPrime(int nCur)
        {
            int nE = -1;
            bool blnFound = false;

            //Searches begining from 3 for an realatively prime to nCur. Stops search when it was found.
            for (int i = 3; (i < (nCur / 2)) && (!blnFound); i++)
            {           
                int nGgt = ggT(i, nCur);
                if (nGgt == 1)
                {
                    blnFound = true;
                    nE = i;
                }
                else { /*not prime to each other*/ }
            }
            return nE;
            
        }

        /// <summary>
        /// Find the greates common divisor of n1 and n2. Iterative solution.
        /// </summary>
        /// <param name="n1">Number one</param>
        /// <param name="n2">Number two</param>
        /// <returns>The greates common divisor of n1 and n2</returns>
        private int ggT(int n1, int n2)
        {
            int nRet = 0;

            n1 = (n1 < 0)?n1 * (-1):n1;//Absolute           

            while (n2 > 0)
            {
                nRet = n1 % n2;
                n1 = n2;
                n2 = nRet;
            }
            return n1;
        }

        /// <summary>
        /// Modular inverse
        /// </summary>
        /// <param name="n1">Number one</param>
        /// <param name="n2">Number two</param>
        /// <returns>The modular inverse of n1 and n2</returns>
        int modInverse(int n1, int n2)
        {
            int n3 = n2;
            int n4 = 0;
            int n5 = 1;
            while (n1 > 0)
            {
                int n6 = n3 / n1;
                int nLastN1 = n1;
                n1 = n3 % nLastN1;
                n3 = nLastN1;
                int nLastN5 = n5;
                n5 = n4 - n6 * nLastN5;
                n4 = nLastN5;
            }
            n4 %= n2;
            n4 = (n4 < 0) ? (n4 + n2) % n2 : n4;
            return n4;
        }

        /// <summary>
        /// Uses the sieve of Erathostenes to find prime numbers upto nMax.
        /// </summary>
        /// <param name="nMax"></param> Upperbound of prime numbers in the particular instance.
        /// <returns>All Prime numbers from 3 to nMax</returns>
        private int[] SieveOfErathostenes(int nMax)
        {
            int[] nArRet = null;
            bool[] blnArAnalysed = new bool[nMax + 1]; //Mapping all number that are not a prime number

            // iterate upto square of nMac
            for (int i = 2; i < Math.Ceiling(Math.Sqrt(nMax)); i++)
            {
                if (!blnArAnalysed[i])
                {
                    int j = i;
                    //Set all multiples true
                    while (j * i <= nMax)
                    {
                        blnArAnalysed[j * i] = true;
                        j++;
                    }
                }
            }

            //Generate nArRet
            List<int> l = new List<int>();
            for (int i = 3; i < blnArAnalysed.Length; i++)
            {
                if (!blnArAnalysed[i])
                {
                    l.Add(i);
                }
                else { /*not a prime number*/}
            }
            nArRet = l.ToArray<int>();

            return nArRet;
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts int[] to byte[]
        /// </summary>
        /// <param name="intAr">Int-Array</param>
        /// <returns>Byte-Array</returns>
        public byte[] IntArToByteAr(int[] intAr)
        {
            byte[] nByteAr = new byte[intAr.Length * 4];
            for (int i = 0; i < intAr.Length; i++)
                Array.Copy(BitConverter.GetBytes(intAr[i]), 0, nByteAr, 4 * i, 4);
            return nByteAr;
        }

        /// <summary>
        /// Converts byte[] to int[]
        /// </summary>
        /// <param name="nByteAr">Byte-Array</param>
        /// <returns>Int-Array</returns>
        public int[] ByteArToIntAr(byte[] nByteAr)
        {
            int[] nIntAr = new int[nByteAr.Length / 4];
            for (int i = 0; i < nByteAr.Length; i += 4)
                nIntAr[i / 4] = BitConverter.ToInt32(nByteAr, i);
            return nIntAr;
        } 
        #endregion

        
    }
}
