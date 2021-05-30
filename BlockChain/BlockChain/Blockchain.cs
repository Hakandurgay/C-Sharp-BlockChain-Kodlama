using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain
{
    public class Blockchain
    {
        public IList<Block> Chain { get; set; }
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
            return new Block(DateTime.Now, null, "{}");
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
            Chain.Add(block);
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

    }
}
