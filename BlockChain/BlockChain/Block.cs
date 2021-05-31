using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BlockChain
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public int Nonce { get; set; } = 0; //
        //proof of work ile üretilen coinlerde
        //nonce değeri ile bloğuk hashi tekrar hashlenir
        //    Nonce değerini bilen miner hak kazanır

        public Block(DateTime timeStamp,string previousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
           // Hash = CalculateHash();
        }
        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inBytes = Encoding.ASCII.GetBytes($"{TimeStamp}--{PreviousHash ?? ""}--{JsonConvert.SerializeObject(Transactions)}--{Nonce}");
            byte[] outBytes = sha256.ComputeHash(inBytes);
            return Convert.ToBase64String(outBytes);
        }
        public void Mine(int diffuculty)
        {
            var leadingZeros = new string('0', diffuculty); //örneğin difficulty iki olursa 00 şeklinde solda iki tane sıfır olur
            while(this.Hash==null || this.Hash.Substring(0,diffuculty)!=leadingZeros)
            {                                  //hashin solunda iki tane sıfır olursa kanıtlanmış olacak
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}
