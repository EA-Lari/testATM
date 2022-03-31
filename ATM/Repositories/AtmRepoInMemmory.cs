using ATM.Models;

namespace ATM
{
    internal class AtmRepoInMemmory : IAtmRepo
    {
        public IList<BanknoteCount> GetBanknotes()
        {
            var list = new List<BanknoteCount>()
            {
                new BanknoteCount{Banknote = new Banknote{ Denomination = 100}, Count = 10},
                new BanknoteCount{Banknote = new Banknote{ Denomination = 1000}, Count = 10},
                new BanknoteCount{Banknote = new Banknote{ Denomination = 200}, Count = 10},
                new BanknoteCount{Banknote = new Banknote{ Denomination = 5000}, Count = 10},
                new BanknoteCount{Banknote = new Banknote{ Denomination = 500}, Count = 10},
                new BanknoteCount{Banknote = new Banknote{ Denomination = 2000}, Count = 10}
            };
            return list;
        }
    }
}
