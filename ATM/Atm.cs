using ATM.Models;

namespace ATM
{
    internal class Atm : IAtm
    {
        IList<BanknoteCount> _banknotes;

        public Atm(IAtmRepo repo)
        {
            _banknotes = repo.GetBanknotes();
        }

        public IList<BanknoteCount> GetBanknotes(int sum)
        {
            if(!CheckOnMin(sum, out int min))
                throw new Exception($"Введите сумму, которую можно разделить без остатка на {min}");
            if(sum<0)
                throw new Exception($"Сумма отрицательная");
            var bancnotes = GetBanknotes(sum, _banknotes
                .Where(x => x.Banknote.Denomination <= sum)
                .OrderByDescending(x => x.Banknote.Denomination).ToList());
            return bancnotes;
        }

        private bool CheckOnMin(int sum, out int min)
        {
            min = _banknotes.Min(x => x.Banknote.Denomination);
            if ((sum % min) > 0)
                return false;
            return true;
        }

        private IList<BanknoteCount> GetBanknotes(int sum, IList<BanknoteCount> orderBanknotes)
        {
            IList<BanknoteCount> banknotes = new List<BanknoteCount>();
            var remainder = sum;
            foreach (var banknote in orderBanknotes)
            {
                var result = GetCountBanknotes(remainder, banknote);
                remainder = result.remainder;
                if (result.count > 0)
                {
                    banknotes.Add(new BanknoteCount
                    {
                        Banknote = banknote.Banknote,
                        Count = result.count
                    });
                }
                if (result.remainder == 0)
                    break;
            }
            if (remainder > 0)
            {
                orderBanknotes.Remove(orderBanknotes.First());
                if (orderBanknotes.Any())
                    banknotes = GetBanknotes(sum, orderBanknotes);
                else {
                    throw new Exception("В банкомате нет необходимоой суммы");
                }
            }
            return banknotes;
        }

        private (int count, int remainder) GetCountBanknotes(int sum, BanknoteCount banknote)
        {
            var countBanknotes = Math.DivRem(sum, banknote.Banknote.Denomination, out int remainder);
            if (banknote.Count < countBanknotes)
            {
                remainder += (countBanknotes - banknote.Count)* banknote.Banknote.Denomination;
                countBanknotes = banknote.Count;
            }
            return new(countBanknotes, remainder);
        }

        public IList<BanknoteCount> GetCountBanknotesInAtm()
            => _banknotes;
    }
}
