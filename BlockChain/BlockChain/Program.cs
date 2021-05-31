using Newtonsoft.Json;
using System;
using System.Reflection;

namespace BlockChain
{

    class Program
    {

        public static Blockchain ourBlockchain = new Blockchain();
        public static int Port = 0;
        public static P2PClient Client=new P2PClient();
        public static P2PServer Server = null;
        public static string Name = "Unknown";
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            
            //P2P haberleşerek programın çalışması

            //ourBlockchain.InitializeChain();
            //if (args.Length >= 1)
            //    Port = int.Parse(args[0]);
            //if (args.Length >= 2)
            //    Name = args[1];
            //if (Port > 0)
            //{
            //    Server = new P2PServer();
            //    Server.Start();
            //}
            //if (Name != "Unknown")
            //    Console.WriteLine($"Su anki Kullanıcı:{Name}");
            //Console.WriteLine("=========================");
            //Console.WriteLine("1. Server a Baglan");
            //Console.WriteLine("2. Transaction Ekle");
            //Console.WriteLine("3. Blockchain i Goster");
            //Console.WriteLine("4. Cikis");
            //Console.WriteLine("=========================");

            //int selection = 0;
            //while (selection != 4)
            //{
            //    switch (selection)
            //    {
            //        case 1:
            //            Console.WriteLine("Lütfen Server URL ini Girin:");
            //            string serverURL = Console.ReadLine();
            //            Client.Connect($"{serverURL}/Blockchain");
            //            break;
            //        case 2:
            //            Console.WriteLine("Lütfen Alici adini Girin");
            //            string receiverName = Console.ReadLine();
            //            Console.WriteLine("Miktari girin");
            //            string amount = Console.ReadLine();
            //            ourBlockchain.CreateTransaction(new Transaction(Name, receiverName, int.Parse(amount)));
            //            ourBlockchain.ProcessPendingTransactions(Name);
            //            Client.Broadcast(JsonConvert.SerializeObject(ourBlockchain));
            //            break;
            //        case 3:
            //            Console.WriteLine("Blockchain");
            //            Console.WriteLine(JsonConvert.SerializeObject(ourBlockchain, Formatting.Indented));
            //            break;

            //    }

            //    Console.WriteLine("Lütfen bir seçenek seçin");
            //    string action = Console.ReadLine();
            //    selection = int.Parse(action);
            //}

            //Client.Close();


            //ourBlockchain.AddBlock(new Block(DateTime.Now, null, "{sender:Hakan,receiver=Ahmet,amaount:10}"));
            //ourBlockchain.AddBlock(new Block(DateTime.Now, null, "{sender:Mehmet,receiver=Selim,amaount:15}"));
            //ourBlockchain.AddBlock(new Block(DateTime.Now, null, "{sender:Ayşe,receiver=Fatma,amaount:20}"));
            ourBlockchain.CreateTransaction(new Transaction("Selcuk","Ahmet",15)); 
            ourBlockchain.ProcessPendingTransactions("MinerHakan");//bunu çağırdıktan sonra block oluşturulur
            ourBlockchain.CreateTransaction(new Transaction("Ahmet", "Selcuk", 10));
            ourBlockchain.CreateTransaction(new Transaction("Ahmet", "Selcuk", 2));
            ourBlockchain.ProcessPendingTransactions("MinerHakan");



            DateTime finishTime = DateTime.Now;
            Console.WriteLine("Süre: " + (finishTime - startTime).ToString());
            Console.WriteLine("Selcuk balance:"+ourBlockchain.GetBalance("Selcuk").ToString());
            Console.WriteLine("Ahmet balance:"+ourBlockchain.GetBalance("Ahmet").ToString());
            Console.WriteLine("Ali balance:"+ourBlockchain.GetBalance("MinerHakan").ToString());


            Console.WriteLine(JsonConvert.SerializeObject(ourBlockchain, Formatting.Indented));
            Console.WriteLine("Gecerli Mi?: " + ourBlockchain.IsValid().ToString());

            /*
            //data değiştirilierek valid olup olmadığı kontrol ediliyor
            Console.WriteLine("Data değiştiriliyor...");
            //ourBlockchain.Chain[1].Data = "{sender:xyz,receiver:sdgf,amount:144}";
            Console.WriteLine("data değiştirildi Gecerli Mi?: " + ourBlockchain.IsValid().ToString());
            //hash değiştirilerek valid olup olmayacağı kontrol ediliyor
            Console.WriteLine("Hash değiştiriliyor...");
            ourBlockchain.Chain[1].Hash = ourBlockchain.Chain[1].CalculateHash();
            Console.WriteLine("Gecerli Mi?: " + ourBlockchain.IsValid().ToString());
            Console.WriteLine("hash değiştirildi Gecerli Mi?: " + ourBlockchain.IsValid().ToString());
            ///
            ///makinelerin yüzde 51 ine sahip olan biri aşağıdakine benzer bir işlem yaparak blockchaini ele geçirebilir
            ourBlockchain.Chain[2].PreviousHash = ourBlockchain.Chain[1].Hash;
            ourBlockchain.Chain[2].Hash = ourBlockchain.Chain[2].CalculateHash(); //değiştirdiğimiz datanın yeni hashi atanıyor
            ourBlockchain.Chain[3].PreviousHash = ourBlockchain.Chain[2].Hash;
            ourBlockchain.Chain[3].Hash = ourBlockchain.Chain[3].CalculateHash();
            Console.WriteLine("tüm zincir değiştirildi ve Geçerli Mi?" + ourBlockchain.IsValid().ToString());
            */
            Console.ReadKey();

        }
    }
}
