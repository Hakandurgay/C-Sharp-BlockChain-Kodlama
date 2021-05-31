using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain
{
    public class Blockchain
    {
        
      public  IList<Transaction> PendingTransations = new List<Transaction>();
        public int Reward { get; set; } = 1; //minera verilecek olan ödül
        public IList<Block> Chain { get; set; }
        public int Diffuculty { get; set; } = 2;  //blocklar çok hızlı tahmin edilirse difficulty değeri algoritmalar tarafından arttırılarak kolay tahmin edilmesi zorlaştırılır
        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }
        public void InitializeChain()
        {
            Chain = new List<Block>();
        }
        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        public Block CreateGenesisBlock() //ilk block oluşturuldu
        {
            Block block=new Block(DateTime.Now, null, PendingTransations);
            block.Mine(Diffuculty);
            PendingTransations=new List<Transaction>();
            return block;
        }
 
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1; // en son bloğun indexinin bir fazlasına ekleniyor
            block.PreviousHash = latestBlock.Hash; //son bloğun hashi previous hash olur
            block.Hash = block.CalculateHash(); //eklenenen bloğun hashi
            block.Mine(this.Diffuculty); //bloğun çalıştığı kanıtlanana kadar bu fonksiyonda bekler, sonra block zincire eklenir
            Chain.Add(block);
        }

        public void CreateTransaction(Transaction transaction) //transactionlar biriktirildikten sonra aşağıdaki metodla toplu olarak çalıştırılır
        {
            PendingTransations.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAddress) //biriktirilen transactionlar burada bloğa eklenir
        {
            CreateTransaction(new Transaction(null, minerAddress, Reward));
            Block block = new Block(DateTime.Now,GetLatestBlock().Hash,PendingTransations);
            AddBlock(block);
            PendingTransations=new List<Transaction>();//bloğa yazıldıktan sonra eski transactionlar boşaltılarak yenisi oluşturulur
           
        }
        public bool IsValid()
        {
            for(int i=1;i<Chain.Count;i++ )
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];
                if(currentBlock.Hash!=currentBlock.CalculateHash())// iki hash değeri farklıysa blockchain invalid olur
                {                                   //hash değeri blok her oluştuğunda kurucu fonksiyon ile atanıyor.
                                                        //eğer data blok oluştuktan sonra değiştirilirse, değiştikten sonraki hash değeri de değişeceği için ilk oluşturulan hash ile şu anki hash burada kontrol edilmiş oluyor
                    return false;
                }
                if(currentBlock.PreviousHash!=previousBlock.Hash)
                {
                    return false;
                }           
            }
            return true;
        }

        public int GetBalance(string address)
        {
            int balance = 0;
            for (int i = 0; i < Chain.Count; i++)//blok içinde dolaşır
            {
                for (int j = 0; j < Chain[i].Transactions.Count; j++) //bloğun içindeki transactionları dolaşır
                {
                    var transaction = Chain[i].Transactions[j];
                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }
                    if (transaction.ToAddress == address) //para bu kişiye gönderilmiştir
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }
    }
}
