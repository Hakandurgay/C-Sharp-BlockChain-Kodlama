using System;
using System.Collections.Generic;
using WebSocketSharp;
using System.Text;
using Newtonsoft.Json;

namespace BlockChain
{
    public class P2PClient
    {
        IDictionary<string,WebSocket> wsDict=new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            if (!wsDict.ContainsKey(url))
            {
                WebSocket ws=new WebSocket(url);
                ws.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Merhaba client")
                    {
                        Console.WriteLine(e.Data);
                    }
                    else
                    {
                        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if (newChain.IsValid() && newChain.Chain.Count > Program.ourBlockchain.Chain.Count) //gelen chaindeki transactionlar daha büyükse fazla transaction vardır. kendi blockchainimize eklememeiz gerek
                        {
                            List<Transaction> newTransactions=new List<Transaction>();
                            newTransactions.AddRange(newChain.PendingTransations);
                            newTransactions.AddRange(Program.ourBlockchain.PendingTransations);
                            newChain.PendingTransations = newTransactions;
                            Program.ourBlockchain = newChain;
                        }
                    }
                };
                ws.Connect();
                ws.Send("Merhaba Client");
                ws.Send(JsonConvert.SerializeObject(Program.ourBlockchain));
                wsDict.Add(url,ws);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in wsDict)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers=new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }

            return servers;
        }

        public void Close()
        {
            foreach (var item in wsDict)
            {
                item.Value.Close();
            }
        }
    }
}
