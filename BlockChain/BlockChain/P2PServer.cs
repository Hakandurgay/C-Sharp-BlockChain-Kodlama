using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockChain
{
    public class P2PServer:WebSocketBehavior
    {
        private WebSocketServer wss = null;
        private bool chainSynced = false;
        public void Start()
        {
            wss=new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
            Console.WriteLine($"server şu adreste başlatıldı ws://127.0.0.1:{Program.Port}");

        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Merhaba Server")
            {
                Console.WriteLine(e.Data);
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                if (newChain.IsValid() && newChain.Chain.Count > Program.ourBlockchain.Chain.Count) //gelen chaindeki transactionlar daha büyükse fazla transaction vardır. kendi blockchainimize eklememeiz gerek
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransations);
                    newTransactions.AddRange(Program.ourBlockchain.PendingTransations);
                    newChain.PendingTransations = newTransactions;
                    Program.ourBlockchain = newChain;
                }
            }

            if (!chainSynced)
            {
                Send(JsonConvert.SerializeObject(Program.ourBlockchain));
                chainSynced = true;
            }
        }
    }
}
