using static System.Console;

Random rnd = new Random();

object[] deck = new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 'J', 'Q', 'K', 'A' };

bool handComplete = false;
int cash = 500;
int bet;
int decision = 0; //1-hit | 2-stand | 3-double | 4-split
int winner = 0; //1-Player | 2-Dealer | 3-Push
ConsoleKeyInfo contGame;

const string title = "B L A C K J A C K";

string playerName = "Player";
string dealerName = "Dealer";
List<object> playerHand = new List<object>();
List<object> dealerHand = new List<object>();

//Title();

while (!handComplete)
{
    GameLoop();
}

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
    //string betParse;

    Write("Enter bet amount: ");
    string betSt = ReadLine();

    bool betParse = int.TryParse(betSt, out betAmt);

    if (betParse)
    {
        if (betAmt > 0 && betAmt <= cash)
        {
            cash -= betAmt;
        }
        else if (betAmt > cash)
        {
            betAmt = Bet();
        }
        else { betAmt = Bet(); }
    }
    else if(!betParse && betAmt <= 0) { Bet(); }

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
        foreach(object card in hand)
        {
            //Convert.ToChar(card) == 'A' ? score -= 10 : score += 0 ;
            for (int i = 0; i < hand.Count; i++)
            {
                if (i == 0)
                {
                    score += 0;
                }
                else if(Convert.ToChar(card) == 'A' && score > 21)
                {
                    score -= 10;
                }
            }
        }
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
    else if (decision == 2 || decision == 3)//stand or double view
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
    else if (playerHand.Count > 2 && decision == 1)//hit view
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
    Write("(H) Hit | (S) Stand | (D) Double: ");
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
    else if(key.KeyChar == 'd')
    {
        if(cash >= bet * 2)
        {
            decision = 3;
            cash -= bet;
            bet = bet * 2;
            DealCards(playerHand);
            Display();
        }
    }
    else
    {
        decision = 0; //Display();
    }
}

void DealerAction()
{
    while((decision == 2 || decision == 3) && CalculateScore(dealerHand) < 17)
        {
            Thread.Sleep(1000);
            DealCards(dealerHand);
            Display();
        }

    Display();
}

int WinConditions()
{
    while(winner == 0)
    {
        if(CalculateScore(playerHand) <= 21 && CalculateScore(playerHand) > CalculateScore(dealerHand) || 
            (CalculateScore(playerHand) <= 21 && CalculateScore(dealerHand) > 21))
        {
            winner = 1;
            Winner();
        }
        else if((CalculateScore(playerHand) > 21) || 
            (CalculateScore(dealerHand) <= 21 && CalculateScore(dealerHand) > CalculateScore(playerHand)) || 
            (CalculateScore(dealerHand) <= 21 && CalculateScore(playerHand) > 21))
        {
            winner = 2;
            Winner();
        }
        else if(CalculateScore(playerHand) == CalculateScore(dealerHand))
        {
            winner = 3;
            Winner();
        }
    }

    return winner;
}

void Winner()
{
    if (winner == 1)
    {
        cash += bet * 2;
        bet = 0;
        WriteLine("Player Wins");
    }
    else if(winner == 2)
    {
        bet = 0;
        WriteLine("Dealer Wins");
    }
    else if(winner == 3)
    {
        cash += bet;
        bet = 0;
        WriteLine("Push");
    }
}

void ContGame()
{
    Write("Play another hand? (Y/N): ");
    contGame = ReadKey();
    if (contGame.KeyChar == 'y')
    {
        Clear();
        playerHand.Clear();
        dealerHand.Clear();
        bet = 0;
        decision = 0;
        winner = 0;
        handComplete = false;
    }
    else if (contGame.KeyChar == 'n')
    {
        handComplete = true;
    }
    else
    {
        ContGame();
    }
}

void GameLoop()
{
    bet = Bet();

    DealCards(playerHand);
    DealCards(playerHand);
    DealCards(dealerHand);
    DealCards(dealerHand);

    CalculateScore(playerHand);
    CalculateScore(dealerHand);

    Display();
    while(CalculateScore(playerHand) < 21 && (decision != 2 && decision != 3))
    {
        PlayerDecision();
        if ((decision == 1) && CalculateScore(playerHand) < 21)
        {
            PlayerDecision();
        }
    }

    if(CalculateScore(playerHand) < 21)
    {
        DealerAction();
    }
    WinConditions();

    handComplete = true;

    ContGame();

    if (cash <= 0)
    {
        WriteLine("You lose!");
        ReadKey();
        handComplete = true;
    }
}
