using static System.Console;

Random rnd = new Random();
object[] deck = new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 'J', 'Q', 'K', 'A' };
bool currentHand = true;
int cash = 500;
int bet;
int decision = 0; //1-hit | 2-stand | 3-double | 4-split

const string title = "B L A C K J A C K";

string playerName = "Player";
string dealerName = "Dealer";
List<object> playerHand = new List<object>();
List<object> dealerHand = new List<object>();

void Title()
{
    for (int i = 0; i < title.Length; i++)
    {
        Write(title[i]);
        Thread.Sleep(50);
    }
    ReadKey();
}

int Bet()
{
    Clear();

    WriteLine($"CASH: {cash:C}");

    int betAmt;
    Write("Enter bet amount: ");
    betAmt = Convert.ToInt32(Console.ReadLine());
    if (betAmt > 0 && betAmt <= cash)
    {
        cash -= betAmt;
    }
    else if (betAmt > cash)
    {
        betAmt = Bet();
    }

    return betAmt;
}

int DealCards(List<object> hand)
{
    int card;
    card = rnd.Next(0, deck.Length);

    hand.Add(deck[card]);
    return card;
}

int CalculateScore(List<object> hand)
{
    int score = 0;

    foreach (object card in hand)
    {
        if (card is int)
        {
            score += Convert.ToInt32(card);
        }
        else if (card is char && card != deck[deck.Length - 1])
        {
            score += 10;
        }
        else { score += 11; }
    }

    while (score > 21 && hand.Contains('A'))
    {
        score -= 10;
    }

    return score;
}

void Display()
{
    if (playerHand.Count == 2 && decision == 0)//def
    {
        Clear();

        WriteLine("\nCASH: {0:C}\n", cash);
        WriteLine("\nCurrent Bet: {0:C}\n", bet);

        Write($"{playerName}:\t\t");
        foreach (object card in playerHand)
        {
            Write($"{card}\t");
            Thread.Sleep(100);
        }

        WriteLine("\n");

        Write($"{dealerName}:\t\t");
        for (int i = 0; i < dealerHand.Count; i++)
        {
            if (i == 0)
            {
                Write("[x]\t");
                Thread.Sleep(100);
            }
            else { Write($"{dealerHand[i]} "); }
            Thread.Sleep(100);
        }

        WriteLine("\n");
        WriteLine($"{playerName}: {CalculateScore(playerHand)}");
    }
    else if (decision == 2)//stand
    {
        Clear();

        WriteLine("\nCASH: {0:C}\n", cash);
        WriteLine("\nCurrent Bet: {0:C}\n", bet);

        Write($"{playerName}:\t\t");
        foreach (object card in playerHand)
        {
            Write($"{card}\t");
            Thread.Sleep(100);
        }

        WriteLine("\n");

        Write($"{dealerName}:\t\t");
        for (int i = 0; i < dealerHand.Count; i++)
        {
            Write($"{dealerHand[i]}\t");
            Thread.Sleep(100);
        }

        WriteLine("\n");
        WriteLine($"{playerName}: {CalculateScore(playerHand)}");
        WriteLine($"{dealerName}: {CalculateScore(dealerHand)}");
    }
    else if (playerHand.Count > 2 && decision == 1 || decision == 3)//hit or double view
    {
        Clear();

        WriteLine("\nCASH: {0:C}\n", cash);
        WriteLine("\nCurrent Bet: {0:C}\n", bet);

        Write($"{playerName}:\t\t");
        foreach (object card in playerHand)
        {
            Write($"{card}\t");
            Thread.Sleep(100);
        }

        WriteLine("\n");

        Write($"{dealerName}:\t\t");
        for (int i = 0; i < dealerHand.Count; i++)
        {
            if (i == 0)
            {
                Write("[x]\t");
                Thread.Sleep(100);
            }
            else { Write($"{dealerHand[i]} "); Thread.Sleep(100); }
        }

        WriteLine("\n");
        WriteLine($"{playerName}: {CalculateScore(playerHand)}");
    }
}

void PlayerDecision()
{
    ConsoleKeyInfo key;
    Write("(H) Hit | (S) Stand: ");
    key = Console.ReadKey();

    if (key.KeyChar == 'h')
    {
        decision = 1;
        DealCards(playerHand);
        Display();
    }
    else if (key.KeyChar == 's')
    {
        decision = 2;
        Display();
    }
    else
    {
        decision = 0; Display();
    }
}

void DealerAction()
{
    while(decision == 2 && CalculateScore(dealerHand) < 17)
        {
            Thread.Sleep(1000);
            DealCards(dealerHand);
            Display();
        }

    Display();
}

void GameLoop()
{
    Title();

    bet = Bet();

    DealCards(playerHand);
    DealCards(playerHand);
    DealCards(dealerHand);
    DealCards(dealerHand);

    CalculateScore(playerHand);
    CalculateScore(dealerHand);

    Display();
    while(CalculateScore(playerHand) < 21 && decision != 2)
    {
        PlayerDecision();
        if (decision == 1 && CalculateScore(playerHand) < 21)
        {
            PlayerDecision();
        }
    }
    DealerAction();
}

GameLoop();

ReadKey();
