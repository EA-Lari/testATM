using ATM.Models;

namespace ATM
{
    internal interface IAtm
    {
        IList<BanknoteCount> GetBanknotes(int sum);

        IList<BanknoteCount> GetCountBanknotesInAtm();
    }
}
