using Newtonsoft.Json;
using System;

namespace BlockChain
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain ourBlockchain = new Blockchain();
            ourBlockchain.AddBlock(new Block(DateTime.Now, null, "{sender:Hakan,receiver=Ahmet,amaount:10}"));
            ourBlockchain.AddBlock(new Block(DateTime.Now, null, "{sender:Mehmet,receiver=Selim,amaount:15}"));
            ourBlockchain.AddBlock(new Block(DateTime.Now, null, "{sender:Ayşe,receiver=Fatma,amaount:20}"));
            Console.WriteLine(JsonConvert.SerializeObject(ourBlockchain, Formatting.Indented));
            Console.WriteLine("Gecerli Mi?: "+ourBlockchain.IsValid().ToString());


            //data değiştirilierek valid olup olmadığı kontrol ediliyor
            Console.WriteLine("Data değiştiriliyor...");
            ourBlockchain.Chain[1].Data = "{sender:xyz,receiver:sdgf,amount:144}";
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

            Console.ReadKey();
        }
    }
}
