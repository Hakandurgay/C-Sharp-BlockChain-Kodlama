using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain
{
    public class Transaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int Amount { get; set; }
        public Transaction(string fromAddress,string toAdress,int amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAdress;
            Amount = amount;
        }
    }
}
