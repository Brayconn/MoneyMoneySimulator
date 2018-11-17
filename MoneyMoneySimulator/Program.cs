using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyMoneySimulator.ExtensionMethods;

namespace MoneyMoneySimulator
{
    static class ExtensionMethods
    {
        /*
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        */

        public static void Add<T>(this T destination, IMoneyHolder source) where T : IMoneyHolder
            => destination.Add(source, source.Pennies, source.Nickels, source.Dimes, source.Quarters, source.Dollars);
        public static void Add<T>(this T destination, IMoneyHolder source, IMoneyHolder amount) where T : IMoneyHolder
            => destination.Add(source, amount.Pennies, amount.Nickels, amount.Dimes, amount.Quarters, amount.Dollars);
        public static void Add<T>(this T destination, IMoneyHolder source, int pennies, int nickels, int dimes, int quarters, int dollars) where T : IMoneyHolder
        {
            pennies = Math.Min(pennies, source.Pennies);
            nickels = Math.Min(nickels, source.Nickels);
            dimes = Math.Min(dimes, source.Dimes);
            quarters = Math.Min(quarters, source.Quarters);
            dollars = Math.Min(dollars, source.Dollars);

            destination.Pennies += pennies;
            source.Pennies -= pennies;

            destination.Nickels += nickels;
            source.Nickels -= nickels;

            destination.Dimes += dimes;
            source.Dimes -= dimes;

            destination.Quarters += quarters;
            source.Quarters -= quarters;

            destination.Dollars += dollars;
            source.Dollars -= dollars;
        }

        public static IMoneyHolder ToIMoneyHolder(this int value, SpinnerSpaces restriction = SpinnerSpaces.AnyCoins, IMoneyHolder maximum = null)
        {
            IMoneyHolder wallet = new Wallet();
            int actualAmount = value;
            actualAmount -= (wallet.Dollars = Math.Min(actualAmount / 100, maximum?.Dollars ?? int.MaxValue)) * 100;
            actualAmount -= (wallet.Quarters = (restriction != SpinnerSpaces.NoQuarters) ? Math.Min(actualAmount / 25, maximum?.Quarters ?? int.MaxValue) : 0) * 25;
            actualAmount -= (wallet.Dimes = (restriction != SpinnerSpaces.NoDimes) ? Math.Min(actualAmount / 10, maximum?.Dimes ?? int.MaxValue) : 0) * 10;
            actualAmount -= (wallet.Nickels = (restriction != SpinnerSpaces.NoNickels) ? Math.Min(actualAmount / 5, maximum?.Nickels ?? int.MaxValue) : 0) * 5;
            actualAmount -= (wallet.Pennies = Math.Min(actualAmount, maximum?.Pennies ?? int.MaxValue));
            return wallet;
        }

        public static int ToCents<T>(this T value) where T : IMoneyHolder
            => (value.Dollars * 100) + (value.Quarters * 25) + (value.Dimes * 10) + (value.Nickels * 5) + value.Pennies;
        public static decimal ToDollars<T>(this T value) where T : IMoneyHolder
            => decimal.Divide(value.ToCents(), 100);

        public static void Print(string text, bool halt = false)
        {
            Console.WriteLine(text);
            if(halt)
                Console.ReadKey(false);
        }
    }

    #region Money holders

    interface IMoneyHolder
    {
        int Pennies { get; set; }
        int Nickels { get; set; }
        int Dimes { get; set; }
        int Quarters { get; set; }
        int Dollars { get; set; }
    }

    /*
    class Wallet : IMoneyHolder
    {
        private int _pennies;
        public int Pennies
        {
            get => _pennies;
            set => _pennies = value.Clamp(0, 25);
        }
        private int _nickels;
        public int Nickels
        {
            get => _nickels;
            set => _nickels = value.Clamp(0, 25);
        }
        private int _dimes;
        public int Dimes
        {
            get => _dimes;
            set => _dimes = value.Clamp(0, 25);
        }
        private int _quarters;
        public int Quarters
        {
            get => _quarters;
            set => _quarters = value.Clamp(0, 25);
        }
        private int _dollars;
        public int Dollars
        {
            get => _dollars;
            set => _dollars = value.Clamp(0, 10);
        }
    }
    */

    class Wallet : IMoneyHolder
    {
        public int Pennies { get; set; }
        public int Nickels { get; set; }
        public int Dimes { get; set; }
        public int Quarters { get; set; }
        public int Dollars { get; set; }

        public Wallet(int Pennies = 0, int Nickles = 0, int Dimes = 0, int Quarters = 0, int Dollars = 0)
        {
            this.Pennies = Pennies;
            this.Nickels = Nickels;
            this.Dimes = Dimes;
            this.Quarters = Quarters;
            this.Dollars = Dollars;
        }
    }

    class OriginalBank : IMoneyHolder
    {
        public int Pennies { get; set; }
        public int Nickels { get; set; }
        public int Dimes { get; set; }
        public int Quarters { get; set; }
        public int Dollars { get; set; }

        public OriginalBank()
        {
            Pennies = Nickels = Dimes = Quarters = 24;
            Dollars = 10;
        }
    }

    class OriginalMoneyMoneyPot : IMoneyHolder
    {
        public int Pennies { get; set; }
        public int Nickels { get; set; }
        public int Dimes { get; set; }
        public int Quarters { get; set; }
        public int Dollars { get; set; }

        public OriginalMoneyMoneyPot()
        {
            Pennies = Nickels = Dimes = Quarters = 1;
            Dollars = 0;
        }
    }

    class InterestRate : IMoneyHolder
    {
        private readonly int _pennies;
        public int Pennies
        {
            get => _pennies;
            set { }
        }
        private readonly int _nickels;
        public int Nickels
        {
            get => _nickels;
            set { }
        }
        private readonly int _dimes;
        public int Dimes
        {
            get => _dimes;
            set { }
        }
        private readonly int _quarters;
        public int Quarters
        {
            get => _quarters;
            set { }
        }
        private readonly int _dollars;
        public int Dollars
        {
            get => _dollars;
            set { }
        }
        public InterestRate(int pennies, int nickles, int dimes, int quarters, int dollars)
        {
            this._pennies = pennies;
            this._nickels = nickles;
            this._dimes = dimes;
            this._quarters = quarters;
            this._dollars = dollars;
        }
    }

    #endregion

    #region Dice

    enum DiceRollContext
    {
        Movement,
        TurnOrder
    }

    interface IDice
    {
        int Roll();
    }

    class D6 : IDice
    {
        Random seed = new Random();
        public int Roll() => seed.Next(1, 7);
    }

    class D1 : IDice
    {
        public int Roll() => 1;
    }

    #endregion

    #region Token

    enum TokenContext
    {
        Normal
    }
    interface IToken
    {
        IGameBoard Board { get; set; }
        int Position { get; set; }
    }
    class OriginalToken : IToken
    {
        public IGameBoard Board { get; set; }
        //Starting at -1, so rolling a 1 lands on the first space (0)
        public int Position { get; set; } = -1;

        public OriginalToken(IGameBoard board)
        {
            this.Board = board;
        }
    }

    #endregion

    #region Rules
    
    class BankRefillEventArgs : EventArgs, IMoneyHolder
    {
        public bool CanExchange { get; set; } = false;
        public int Pennies { get; set; }
        public int Nickels { get; set; }
        public int Dimes { get; set; }
        public int Quarters { get; set; }
        public int Dollars { get; set; }

        public BankRefillEventArgs(IMoneyHolder moneyHolder)
            : this(moneyHolder.Pennies, moneyHolder.Nickels, moneyHolder.Dimes, moneyHolder.Quarters,moneyHolder.Dollars) { }
        public BankRefillEventArgs(int penniesNeeded, int nicklesNeeded, int dimesNeeded, int quartersNeeded, int dollarsNeeded)
        {
            this.Pennies = penniesNeeded;
            this.Nickels = nicklesNeeded;
            this.Dimes = dimesNeeded;
            this.Quarters = quartersNeeded;
            this.Dollars = dollarsNeeded;
        }
    }
    interface IOriginalRules
    {
        void TakeMoney(int amount, IMoneyHolder bank, SpinnerSpaces restriction, IMoneyHolder pot = null);
        void CollectFromPot(IMoneyHolder pot);
        void BankExchange(BankExchangeSpace space, IMoneyHolder bank);
        void BankExchange(BankRefillEventArgs e, IMoneyHolder bank);

        event EventHandler<BankRefillEventArgs> CallForBankRefill;
    }

    #endregion

    #region Players

    interface IPlayer
    {
        string Name { get; }

        IMoneyHolder Wallet { get; }

        int RollDice(IDice dice, DiceRollContext context);
        void MoveToken(IToken token, int spacesToMove, TokenContext context);
        SpinnerSpaces SpinSpinner(ISpinner spinner, SpinnerContext context);        
    }

    class OriginalPlayer : IPlayer, IOriginalRules
    {
        public string Name { get; }

        public IMoneyHolder Wallet { get; } = new Wallet();

        public int RollDice(IDice dice, DiceRollContext context) => dice.Roll();
        public void MoveToken(IToken token, int spacesToMove, TokenContext context) => token.Position += spacesToMove;
        public SpinnerSpaces SpinSpinner(ISpinner spinner, SpinnerContext context) => spinner.Spin();

        public void TakeMoney(int amount, IMoneyHolder bank, SpinnerSpaces restriction, IMoneyHolder pot = null)
        {
            IMoneyHolder destination = restriction == SpinnerSpaces.MoneyMoney ? pot : Wallet;
            IMoneyHolder amountToTake = new Wallet();

            //int amountOfDollars, amountOfQuarters, amountOfDimes, amountOfNickels, amountOfPennies;
            while (true)
            {
                int cents = (amountToTake = amount.ToIMoneyHolder(restriction, bank)).ToCents();
                if (cents != amount)
                {
                    BankRefillEventArgs brea = new BankRefillEventArgs((amount - cents).ToIMoneyHolder(restriction));
                    CallForBankRefill(this, brea);
                    if (brea.CanExchange == false)
                        return;
                }
                else
                    break;
            }
            destination.Add(bank, amountToTake);
        }

        public void CollectFromPot(IMoneyHolder pot)
        {
            Wallet.Add(pot);
        }

        private void DecrementLowest(ref int pennyNickles, ref int nickels, ref int dimes, ref int quarters)
        {
            if (nickels > 0)
                nickels--;
            else if (dimes > 0)
                dimes--;
            else if (quarters > 0)
                quarters--;
            else if (pennyNickles > 0)
                pennyNickles--;
        }
        private void DecrementHighest(ref int pennyNickles, ref int nickels, ref int dimes, ref int quarters)
        {
            if (quarters > 0)
                quarters--;
            else if (dimes > 0)
                dimes--;
            else if (nickels > 0)
                nickels--;
            else if (pennyNickles > 0)
                pennyNickles--;
        }
        private void Reset(out int pennyNickles, out int nickels, out int dimes, out int quarters, bool useQuarters = true, bool useDimes = true, bool useNickels = true, bool usePennyNickles = true)
        {
            //Pennies that don't add up to nickles aren't relevant, since you can't trade them
            pennyNickles = usePennyNickles ? Wallet.Pennies / 5 : 0;
            nickels = useNickels ? Wallet.Nickels : 0;
            dimes = useDimes ? Wallet.Dimes : 0;
            quarters = useQuarters ? Wallet.Quarters : 0;
        }
        public void BankExchange(BankExchangeSpace space, IMoneyHolder bank)
        {
            for(int i = 0; i < space.MaxExchanges; i++)
            {
                //Total up all the relevant money
                //int tradePennyNickles, tradeNickels, tradeDimes, tradeQuarters;
                Reset(out int tradePennyNickles, out int tradeNickels, out int tradeDimes, out int tradeQuarters);

                //Store the biggest coin these *could* add up to
                int tempValue = ((tradePennyNickles + tradeNickels) * 5) + (tradeDimes * 10) + (tradeQuarters * 25);
                int currentTarget;
                if (tempValue >= 100)
                    currentTarget = 100;
                else if (tempValue >= 25)
                    currentTarget = 25;
                else if (tempValue >= 10)
                    currentTarget = 10;
                else if (tempValue >= 5)
                    currentTarget = 5;
                else //I guess we aren't doing anything then...
                    currentTarget = 0;

                bool useQuarters = true;
                switch (currentTarget)
                {
                    //Quick decision
                    case 0:
                        tradePennyNickles = 0;
                        tradeNickels = 0;
                        tradeDimes = 0;
                        tradeQuarters = 0;
                        break;
                    case 5:
                        if (tradePennyNickles >= 1)
                        {
                            tradePennyNickles = 1;
                            tradeNickels = 0;
                            tradeDimes = 0;
                            tradeQuarters = 0;
                        }
                        else
                            goto case 0;
                        break;
                    case 10:
                        if (tradePennyNickles + tradeNickels >= 2)
                        {
                            tradePennyNickles = Math.Min(tradePennyNickles, 2);
                            tradeNickels = Math.Min(tradeNickels, 2 - tradeNickels);
                            tradeDimes = 0;
                            tradeQuarters = 0;
                        }
                        else
                            goto case 5;
                        break;

                    //Use the loop
                    case 25:
                        useQuarters = false;
                        goto case 100;
                    case 100:
                        //TODO use two for loops, one for the big decrement, then small decrement. might need to do somethin about cloning
                        bool useDimes, useNickels, usePennyNickels;
                        useDimes = useNickels = usePennyNickels = true;
                        bool wasLow = false;
                        int timesReset = 0;
                        while (true)
                        {
                            int currentValue = ((tradePennyNickles + tradeNickels) * 5) + (tradeDimes * 10) + (tradeQuarters * 25);
                            if (currentValue > currentTarget)
                            {
                                DecrementLowest(ref tradePennyNickles, ref tradeNickels, ref tradeDimes, ref tradeQuarters);
                                wasLow = false;
                            }
                            else if (currentValue < currentTarget)
                            {
                                //If we're low 2 times in a row, cancel and aim for 25
                                if (wasLow)
                                {
                                    if (currentTarget == 100)
                                    {
                                        currentTarget = 25;
                                        useQuarters = false;
                                        timesReset = 0;
                                    }
                                    if (currentTarget == 25)
                                    {
                                        goto case 5;
                                    }
                                }
                                Reset(out tradePennyNickles, out tradeNickels, out tradeDimes, out tradeQuarters, useQuarters, useDimes, useNickels, usePennyNickels);
                                timesReset++;
                                for (int j = 0; j < timesReset; j++)
                                    DecrementHighest(ref tradePennyNickles, ref tradeNickels, ref tradeDimes, ref tradeQuarters);
                                wasLow = true;
                            }
                            else //Break once we reach our target
                                break;
                        }
                        break;
                }

                //Actual trade
                if (currentTarget != 0)
                {
                    bank.Add(Wallet, tradePennyNickles * 5, tradeNickels, tradeDimes, tradeQuarters, 0);
                    Wallet.Add(bank, currentTarget.ToIMoneyHolder());
                    TakeMoney(space.Interest, bank, SpinnerSpaces.AnyCoins);
                }
            }
        }
        public event EventHandler<BankRefillEventArgs> CallForBankRefill = new EventHandler<BankRefillEventArgs>((o, e) => { });
        public void BankExchange(BankRefillEventArgs e, IMoneyHolder bank)
        {
            //TODO this is not even close to robust enough; needs to be able to convert to multiple types of coins
            while (e.Quarters > 0 && Wallet.Quarters >= 4 && bank.Dollars >= 1)
            {
                bank.Add(Wallet, 0, 0, 0, 4, 0);
                Wallet.Add(bank, 0, 0, 0, 0, 1);
                e.Quarters -= 4;
            }

            while (e.Dimes > 0 && Wallet.Dimes >= 2 && Wallet.Nickels >= 1 && bank.Quarters >= 1)
            {
                bank.Add(Wallet, 0, 1, 2, 0, 0);
                Wallet.Add(bank, 0, 0, 0, 1, 0);
                e.Dimes -= 2;
                e.Nickels -= 1;
            }

            while (e.Nickels > 0 && Wallet.Nickels >= 2 && bank.Dimes >= 1)
            {
                bank.Add(Wallet, 0, 2, 0, 0, 0);
                Wallet.Add(bank, 0, 0, 1, 0, 0);
                e.Nickels -= 2;
            }

            while (e.Pennies > 0 && Wallet.Pennies >= 5 && bank.Nickels >= 1)
            {
                bank.Add(Wallet, 5, 0, 0, 0, 0);
                Wallet.Add(bank, 0, 1, 0, 0, 0);
                e.Pennies -= 5;
            }
            //TODO call bank exchange if it can't do an exchange?
        }        

        public OriginalPlayer(string name)
        {
            this.Name = name;
        }
    }

    #endregion

    #region Spinner

    enum SpinnerContext
    {
        Normal
    }
    enum SpinnerSpaces
    {
        AnyCoins,
        NoQuarters,
        NoDimes,
        NoNickels,
        MoneyMoney
    }
    interface ISpinner
    {
        SpinnerSpaces[] Spaces { get; }
        SpinnerSpaces Spin();
    }
    class OriginalSpinner : ISpinner
    {
        public SpinnerSpaces[] Spaces { get; } = new SpinnerSpaces[]
        {
            SpinnerSpaces.AnyCoins,
            SpinnerSpaces.MoneyMoney,
            SpinnerSpaces.NoDimes,
            SpinnerSpaces.NoNickels,
            SpinnerSpaces.NoQuarters
        };
        public SpinnerSpaces Spin() => Spaces[new Random().Next(0, 4)];
    }

    #endregion

    #region Board

    interface IGameBoard
    {
        IMoneyHolder Bank { get; }
        IMoneyHolder MoneyMoneyPot { get; }
        ISpace[] Spaces { get; }
        ISpinner Spinner { get; }
    }
    class OriginalBoard : IGameBoard
    {
        public IMoneyHolder Bank { get; } = new OriginalBank();
        public IMoneyHolder MoneyMoneyPot { get; } = new OriginalMoneyMoneyPot();

        public ISpinner Spinner { get; } = new OriginalSpinner();
        public ISpace[] Spaces => new ISpace[]
        {
            new NormalSpace(17),
            new NormalSpace(55),
            new NormalSpace(25),
            new NormalSpace(12),
            new MoneyMoneySpace(),//$
			new NormalSpace(46),
            new BankExchangeSpace(),//(B)
			new NormalSpace(30),
            new NormalSpace(50),
            new NormalSpace(15),
            new NormalSpace(40),
            new MoneyMoneySpace(),//$
			new BankExchangeSpace(),//(B)
			new NormalSpace(13),
            new NormalSpace(31),
            new BankExchangeSpace(),//(B)
			new NormalSpace(34),
            new NormalSpace(51),
            new MoneyMoneySpace(),//$
			new NormalSpace(23),
            new NormalSpace(26),
            new BankExchangeSpace(),//(B)
			new NormalSpace(32),
            new NormalSpace(11),
            new BankExchangeSpace(),//(B)
			new MoneyMoneySpace(),//$
			new NormalSpace(44),
            new NormalSpace(22),
            new NormalSpace(44),
            new NormalSpace(33)
        };
    }

    #endregion

    #region Spaces

    enum SpaceTypes
    {
        Money,
        BankExchange,
        DollarSign
    }
    interface ISpace
    {        
        SpaceTypes Type { get; }
    }
    class NormalSpace : ISpace
    {
        public SpaceTypes Type => SpaceTypes.Money;
        public int Value { get; }
        public NormalSpace(int value)
        {
            this.Value = value;
        }
    }
    class BankExchangeSpace : ISpace
    {
        public SpaceTypes Type => SpaceTypes.BankExchange;
        public int MaxExchanges { get; }
        public int Interest { get; }

        public BankExchangeSpace(int maxExchanges = 1, int interest = 10)
        {
            this.MaxExchanges = maxExchanges;
            this.Interest = interest;
        }
    }
    class MoneyMoneySpace : ISpace
    {
        public SpaceTypes Type => SpaceTypes.DollarSign;
    }

    #endregion

    #region The game as a whole

    interface IGame<B,D,T,P,R> where B : IGameBoard where D : IDice where P : IPlayer, R where T : IToken
    {
        B[] Board { get; }
        D[] Die { get; }

        P[] Players { get; }
        T[] Tokens { get; }

        void Play();
    }

    class OriginalGame : IGame<OriginalBoard, D6, OriginalToken, OriginalPlayer, IOriginalRules>
    {
        public OriginalBoard[] Board { get; } = new[] { new OriginalBoard() };
        public D6[] Die { get; } = new[] { new D6() };

        public OriginalPlayer[] Players { get; }
        public OriginalToken[] Tokens { get; }

        private IPlayer[] GetTurnOrder(IPlayer[] players)
        {
            List<IPlayer> toRoll = new List<IPlayer>(players);
            Dictionary<IPlayer, int> turnOrder = new Dictionary<IPlayer, int>();
            do
            {
                //Roll all players who need rolling
                while (toRoll.Count > 0)
                {
                    int number = toRoll[0].RollDice(Die[0], DiceRollContext.TurnOrder);
                    if (turnOrder.ContainsKey(toRoll[0]))
                        turnOrder[toRoll[0]] = number;
                    else
                        turnOrder.Add(toRoll[0], number);
                    toRoll.RemoveAt(0);
                }
                //If duplicates exist
                if (turnOrder.Distinct().Count() != turnOrder.Count)
                {
                    //Pick each item
                    for (int i = 0; i < turnOrder.Count; i++)
                    {
                        //And check if any other item matches its number
                        for (int j = 0; j < turnOrder.Count; j++)
                        {
                            //If/when a match is found
                            if (i != j && turnOrder.ElementAt(i).Value == turnOrder.ElementAt(j).Value)
                            {
                                //Add it to the list
                                toRoll.Add(turnOrder.ElementAt(i).Key);
                                //And continue on
                                break;
                            }
                        }
                    }
                }
            } while (toRoll.Count > 0);

            //Sort the output
            var temp = turnOrder.ToList();
            temp.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            return temp.Select(x => x.Key).ToArray();
        }

        public OriginalGame(IPlayer[] players)
        {
            this.Players = GetTurnOrder(players).Select(x => (OriginalPlayer)x).ToArray();

            this.Tokens = new OriginalToken[Players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                Players[i].CallForBankRefill += ReFillBank;
                Tokens[i] = new OriginalToken(Board[0]);
            }
        }

        bool refillTriggered = false;
        private void ReFillBank(object sender, BankRefillEventArgs e)
        {
            if (refillTriggered)
                return;
            Print($"The bank needs refilling with {e.ToCents()}c");
            refillTriggered = true;
            foreach (var player in Players)
            {
                player.BankExchange(e, Board[0].Bank);
                if (e.Pennies <= 0 && e.Nickels <= 0 && e.Dimes <= 0 && e.Quarters <= 0 && e.Dollars <= 0)
                {
                    e.CanExchange = true;
                    Print($"{player.Name} succeeded!");
                    break;
                }
                else
                    Print($"{player.Name} failed...");
            }
            //if(!e.CanExchange)
                //Too bad so sad?
            refillTriggered = false;
        }

        public void Play()
        {
            int turnNumber = 0;
            while(true)
            {
                Print($"{Players[turnNumber].Name}'s turn");
                //Roll and move
                int diceResult = Players[turnNumber].RollDice(Die[0], DiceRollContext.Movement);
                Print($"{Players[turnNumber].Name} rolled a {diceResult}");
                Players[turnNumber].MoveToken(Tokens[turnNumber], diceResult, TokenContext.Normal);

                //Stop if someone's won
                if (Tokens.Any(x => x.Position >= Board[0].Spaces.Length))
                    break;

                var space = Board[0].Spaces[Tokens[turnNumber].Position];
                Print($"{Players[turnNumber].Name} landed on a {space.Type} space!" + (space.Type == SpaceTypes.Money ? $" (worth {((NormalSpace)space).Value}c)" : ""));
                switch (space.Type)
                {
                    case SpaceTypes.Money:
                        SpinnerSpaces spinnerResult = Players[turnNumber].SpinSpinner(Board[0].Spinner, SpinnerContext.Normal);
                        Print($"{Players[turnNumber].Name} spun a {spinnerResult}");
                        string moneyChange = (spinnerResult == SpinnerSpaces.MoneyMoney
                            ? $"The Money Money pot's total money went from {Board[0].MoneyMoneyPot.ToCents()} -> "
                            : $"{Players[turnNumber].Name} total money went from {Players[turnNumber].Wallet.ToCents()}c -> ");
                        Players[turnNumber].TakeMoney(((NormalSpace)space).Value, Board[0].Bank, spinnerResult, spinnerResult == SpinnerSpaces.MoneyMoney ? Board[0].MoneyMoneyPot : null);
                        Print(moneyChange + (spinnerResult == SpinnerSpaces.MoneyMoney
                            ? Board[0].MoneyMoneyPot.ToCents()
                            : Players[turnNumber].Wallet.ToCents()) + "c");
                        break;
                    case SpaceTypes.BankExchange:
                        string bankExchangeChange = $"{Players[turnNumber].Name} total money went from {Players[turnNumber].Wallet.ToCents()}c -> ";
                        Players[turnNumber].BankExchange((BankExchangeSpace)space, Board[0].Bank);
                        Print(bankExchangeChange + Players[turnNumber].Wallet.ToCents() + "c");
                        break;
                    case SpaceTypes.DollarSign:
                        Print($"{Players[turnNumber].Name} collected {Board[0].MoneyMoneyPot.ToCents()}c from the Money Money Pot!");
                        Players[turnNumber].CollectFromPot(Board[0].MoneyMoneyPot);
                        if(Board[0].Bank.Pennies == 0 || Board[0].Bank.Nickels == 0 || Board[0].Bank.Dimes == 0 || Board[0].Bank.Quarters == 0)
                        {
                            ReFillBank(this, new BankRefillEventArgs(
                                Board[0].Bank.Pennies == 0 ? 1 : 0,
                                Board[0].Bank.Nickels == 0 ? 1 : 0,
                                Board[0].Bank.Dimes == 0 ? 1 : 0,
                                Board[0].Bank.Quarters == 0 ? 1 : 0,
                                0));
                        }
                        Board[0].MoneyMoneyPot.Add(Board[0].Bank, 1, 1, 1, 1, 0);
                        Print($"The Money Money pot was refilled with {Board[0].MoneyMoneyPot.ToCents()}c");
                        break;
                }
                if (turnNumber == Players.Length - 1)
                    turnNumber = 0;
                else
                    turnNumber++;
            }
            Print($"{Players[turnNumber].Name} has reached the end! (and collected {Board[0].MoneyMoneyPot.ToCents()}c from the Money Money Pot)");
            Players[turnNumber].CollectFromPot(Board[0].MoneyMoneyPot);

            //Display the winner/sort
            List<OriginalPlayer> sorted = Players.ToList();
            sorted.Sort((x, y) => x.Wallet.ToCents().CompareTo(y.Wallet.ToCents()));
            sorted.Reverse();
            Print($"{sorted[0].Name} is the winner!",true);

            for (int i = 0; i < sorted.Count; i++)
                Print($"{i+1} - {sorted[i].Name}:\n" +
                    $"\t   {sorted[i].Wallet.Pennies} * 1c\n" +
                    $"\t + {sorted[i].Wallet.Nickels} * 5c\n" +
                    $"\t + {sorted[i].Wallet.Dimes} * 10c\n" +
                    $"\t + {sorted[i].Wallet.Quarters} * 25c\n" +
                    $"\t + {sorted[i].Wallet.Dollars} * 100c\n" +
                    $"\t--------------------------------\n" +
                    $"\t = ${sorted[i].Wallet.ToDollars()}\n");
            Print("Press any key to exit...", true);
        }

    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || !uint.TryParse(args[1], out uint playerCount))
                playerCount = 4;

            IPlayer[] players = new IPlayer[playerCount];
            for (int i = 0; i < players.Length; i++)
                players[i] = new OriginalPlayer("Player " + (i + 1));
            OriginalGame game = new OriginalGame(players);
            game.Play();
        }
    }
}
