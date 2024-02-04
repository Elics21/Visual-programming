using System;

public interface INotifyer
{
    void Notify(decimal balance);
}
public class Account
{
    private decimal _balance;
    private List<INotifyer> _notifyers;

    public Account()
    {
        _balance = 0;
        _notifyers = new List<INotifyer>();
    }

    public Account(decimal init_balance) {
        _balance = init_balance;
        _notifyers = new List<INotifyer>();
    }

    public void AddNotifyer(INotifyer notifyer)
    {
        _notifyers.Add(notifyer);
    }

    public void ChangeBalance(decimal value)
    {
        _balance += value;

        foreach (var not in _notifyers)
        {
            not.Notify(_balance);
        }
    }

    public decimal getBalance()
    {
        return _balance;
    }

    private void Notification()
    {

    }
}

public class EMailBalanceChangedNotifyer : INotifyer
{
    private string _email;

    public EMailBalanceChangedNotifyer(string email)
    {
        _email = email;
    }

    void INotifyer.Notify(decimal balance)
    {
            Console.WriteLine($"class: EMailBalanceChangedNotifyer | balance: {balance} ");
    }
}

public class SMSLowBalanceNotifyer : INotifyer
{
    private string _phone;
    private decimal _lowBalanceValue;

    public SMSLowBalanceNotifyer(string phone, decimal lowBalanceValue)
    {
        _phone = phone;
        _lowBalanceValue = lowBalanceValue;
    }

    void INotifyer.Notify(decimal balance)
    {
        if(balance < _lowBalanceValue)
        {
            Console.WriteLine($"class: SMSLowBalanceNotifyer | balance: {balance} " );
        }
    }
}


class Program
{
    static void Main()
    {
        Account userAccount = new Account(1000);

        EMailBalanceChangedNotifyer emailNotifyer = new EMailBalanceChangedNotifyer("example@email.com");
        userAccount.AddNotifyer(emailNotifyer);

        SMSLowBalanceNotifyer smsNotifyer = new SMSLowBalanceNotifyer("123456789", 900);
        userAccount.AddNotifyer(smsNotifyer);

        for(var i = 0; i < 100; i += 20) {
            Console.WriteLine($"change: {-i} ");

            userAccount.ChangeBalance(-i);

            Console.WriteLine("\n");
        }

        // decimal currentBalance = userAccount.getBalance();
        // Console.WriteLine($"Текущий баланс: {currentBalance}");

        Console.ReadLine();
    }
}
