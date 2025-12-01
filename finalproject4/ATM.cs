public class ATM
{
    private User _user;

    public ATM(User user)
    {
        _user = user;
    }

    public void CheckBalance()
    {
        Console.WriteLine($"tqveni balansi: {_user.Balance} $");
        Logger.Log($"user {_user.Name} {_user.Surname} - Sheamowma balansi . balansi: {_user.Balance} $");
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new Exception("tanxa dadebiti unda iyos!");

        _user.Balance += amount;

        Logger.Log($"user {_user.Name} {_user.Surname} - sheavso balansi  {amount} $. axali balansi : {_user.Balance} $");

        Console.WriteLine("balansi Sheivso!");
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new Exception("tanxa dadebiti unda iyos!");

        if (amount > _user.Balance)
            throw new Exception("balansze arasakmariasia!");

        _user.Balance -= amount;

        Logger.Log($"user {_user.Name} {_user.Surname} - gaanaxda {amount} $. axali balansi: {_user.Balance} $");

        Console.WriteLine("tanxa warmatebit ganaxda!");
    }
}
