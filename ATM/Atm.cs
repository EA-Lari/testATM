using ATM.Models;

namespace ATM
{
    internal class Atm : IAtm
    {
        IList<BanknoteCount> _banknotes;
        int _maxSum;
        int _minSum;

        public Atm(IAtmRepo repo)
        {
            _banknotes = repo.GetBanknotes();
            _maxSum = _banknotes.Sum(x => x.Count * x.Banknote.Denomination);
            _minSum = _banknotes.Min(x => x.Banknote.Denomination);
        }

        public IList<BanknoteCount> GetBanknotes(int sum)
        {
            if (sum < 0)
                throw new Exception($"Сумма отрицательная");
            if(sum > _maxSum)
                throw new Exception($"В банкомате нет необходимоой суммы");
            if (sum < _minSum)
                throw new Exception($"Вводимая сумма слишком мала" +
                    $"\nВ банкомате есть банкноты номиналом:  {string.Join(',', _banknotes.Select(x => x.Banknote.Denomination))}");
            if (!CheckOnMin(sum))
                throw new Exception($"Введите сумму кратную одному из номиналов: {string.Join(',', _banknotes.Select(x => x.Banknote.Denomination))}");
            var bancnotes = GetBanknotes(sum, _banknotes
                .Where(x => x.Banknote.Denomination <= sum)
                .OrderByDescending(x => x.Banknote.Denomination).ToList());
            return bancnotes;
        }

        private bool CheckOnMin(int sum)
        {
            var result = false;
            var banknotes = _banknotes
                .Select(x => x.Banknote.Denomination)
                .OrderBy(x => x).ToArray();
            foreach (var banknote in banknotes)
            {
                if ((sum % banknote) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
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
