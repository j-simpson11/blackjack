using static System.Console;

Random rnd = new Random();

object[] deck = new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 'J', 'Q', 'K', 'A' };

bool handComplete = false;
int cash = 500;
int bet;
int decision = 0; //1-hit | 2-stand | 3-double | 4-split
int winner = 0; //1-Player | 2-Dealer | 3-Push | 4-player21 | 5-dealer21
ConsoleKeyInfo contGame;

const string title = "B L A C K J A C K";

string playerName = "Player";
string dealerName = "Dealer";
List<object> playerHand = new List<object>();
List<object> dealerHand = new List<object>();

Title();

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

    Write("Enter bet amount: ");
    string? betSt = ReadLine();

    bool betParse = true;
    betParse = int.TryParse(betSt, out betAmt);

    if (betParse && betAmt <= cash)
    {

        if (betAmt > 0 && betAmt <= cash)
        {
            cash -= betAmt;
            return betAmt;
        }
        else if (betAmt > cash || !betParse || betAmt > cash)
        {
            betAmt = 0;
            Bet();
        }
        else { betAmt = Bet(); }
    }
    return betAmt = 0;
}

int DealCards(List<object> hand)
{
    int card;
    card = rnd.Next(0, deck.Length);

    hand.Add(deck[card]);
    return card;
}

int Score(List<object> hand)
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

    if (score > 21 && hand.Contains('A'))
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (Convert.ToChar(hand[i]) == 'A' /*&& i <= hand.Count*/)
            {
                if (Convert.ToChar(hand[i]) == 'A' && score > 21)
                {
                    score -= 10;
                    if (score <= 21 && score > 17)
                    {
                        return score;
                    }
                }
            }
        }

    }

    return score;
}

void Display()
{
    if ((playerHand.Count == 2 && decision == 0) /*def*/
        || (playerHand.Count > 2 && decision == 1))//hit
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
        WriteLine($"{playerName}: {Score(playerHand)}\n");
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

        WriteLine("\n\n----------------------------------------------\n");
        WriteLine($"{playerName}: {Score(playerHand)}\n");
        WriteLine($"{dealerName}: {Score(dealerHand)}\n");


    }
}

void PlayerDecision()
{
    ConsoleKeyInfo key;
    if (cash >= bet * 2)
    {
        Write("(H) Hit | (S) Stand | (D) Double");
    }
    else { Write("(H) Hit | (S) Stand"); }
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
    else if (key.KeyChar == 'd')
    {
        if (cash >= bet * 2)
        {
            decision = 3;
            cash -= bet;
            bet = bet * 2;
            DealCards(playerHand);
            Display();
        }
        else
        {
            Display();
        }
    }
    else
    {
        Display();
    }
}

void DealerAction()
{
    while ((decision == 2 || decision == 3) && Score(dealerHand) < 17 && Score(playerHand) < 21)
    {
        Thread.Sleep(1000);
        DealCards(dealerHand);
        Display();
    }

    Display();
}

int WinConditions()
{
    while (winner == 0)
    {
        if (Score(playerHand) == 21)
        {
            winner = 4;
            Winner();
        }
        else if (Score(dealerHand) == 21)
        {
            winner = 5;
        }
        else if (Score(playerHand) <= 21 && Score(playerHand) > Score(dealerHand) ||
            (Score(playerHand) <= 21 && Score(dealerHand) > 21))
        {
            winner = 1;
            Winner();
        }
        else if ((Score(playerHand) > 21) ||
            (Score(dealerHand) <= 21 && Score(dealerHand) > Score(playerHand)) ||
            (Score(dealerHand) <= 21 && Score(playerHand) > 21))
        {
            winner = 2;
            Winner();
        }
        else if (Score(playerHand) == Score(dealerHand))
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
        WriteLine("Player Wins\n");
    }
    else if (winner == 2)
    {
        bet = 0;
        WriteLine("Dealer Wins\n");
    }
    else if (winner == 3)
    {
        cash += bet;
        bet = 0;
        WriteLine("Push\n");
    }
    else if (winner == 4)
    {
        cash += bet * 3;
        bet = 0;
        WriteLine("Player Blackjack\n");
    }
    else if (winner == 5)
    {
        bet = 0;
        WriteLine("Dealer Blackjack");
    }
}

void ContGame()
{
    handComplete = false;

    playerHand.Clear();
    dealerHand.Clear();
    bet = 0;
    decision = 0;
    winner = 0;

    contGame = ReadKey();
    if (contGame.KeyChar == 'y' || contGame.KeyChar == 1)
    {
        Clear();
        handComplete = false;
    }
    else if (contGame.KeyChar == 'n' || contGame.KeyChar == 2)
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
    do
    {
        bet = Bet();
    } while (bet <= 0);

    DealCards(playerHand);
    DealCards(playerHand);
    DealCards(dealerHand);
    DealCards(dealerHand);

    Score(playerHand);
    Score(dealerHand);

    Display();
    while (Score(playerHand) < 21 && (decision != 2 && decision != 3))
    {
        PlayerDecision();
        if ((decision == 1) && Score(playerHand) < 21)
        {
            PlayerDecision();
        }
    }

    if (Score(playerHand) < 21)
    {
        DealerAction();
    }
    WinConditions();

    handComplete = true;

    if (cash <= 0)
    {
        WriteLine("You lose!");
        ReadKey();
        handComplete = true;
    }
    else
    {
        Write("Play another hand? (Y/N) ");

        ContGame();
    }
}
