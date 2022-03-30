// See https://aka.ms/new-console-template for more information
using ATM;
using ATM.Models;

IAtmRepo repo = new AtmRepoInMemmory ();
IAtm atm = new Atm(repo);
var exit = false;
while (!exit)
{
    try
    {
        Console.Clear();
        Console.WriteLine("Банкноты в банкомате:");
        PrintBanknotes(repo.GetBanknotes());

        Console.WriteLine("Введите сумму:");
        var sum = Convert.ToInt32(Console.ReadLine());
        var result = atm.GetBanknotes(sum);
        Console.WriteLine($"Получите банкноты на сумму {sum}:");
        PrintBanknotes(result);
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Произошла ошибка:\n{ex.Message}");
    }
    finally
    {
        Console.WriteLine($"Чтобы продолжить нажмите Enter (любая другая клавиша - выход)");
        var key = Console.ReadKey();
        if (key.Key != ConsoleKey.Enter)
            exit = true;
    }
}

static void PrintBanknotes(IList<BanknoteCount> banknotes)
{
    foreach (var banknote in banknotes)
    {
        Console.WriteLine($"Купюра: {banknote.Banknote.Denomination} Количество: {banknote.Count}");
    }
}