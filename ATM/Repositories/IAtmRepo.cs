using ATM.Models;

namespace ATM
{
    internal interface IAtmRepo
    {
        IList<BanknoteCount> GetBanknotes();
    }
}
