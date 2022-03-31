using ATM.Models;

namespace ATM
{
    internal class AtmRepoTextFile : IAtmRepo
    {
        private readonly string _path;
        public AtmRepoTextFile(string path)
        {
            _path = path;
        }

        public IList<BanknoteCount> GetBanknotes()
        {
            var lines = File.ReadAllLines(_path);
            var banknotes = new List<BanknoteCount>();
            foreach (var l in lines)
            {
                var arr = l.Split(';');
                var banknote = new BanknoteCount
                {
                    Banknote = new Banknote { Denomination = Convert.ToInt32(arr[0]) },
                    Count = Convert.ToInt32(arr[1])
                };
                banknotes.Add(banknote);
            }
            return banknotes;
        }
    }
}
