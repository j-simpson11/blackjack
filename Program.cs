//Create a deck
object[] Deck = new object[]
{
    2, 3, 4, 5, 6 , 7, 8, 9, 10, 'J', 'Q', 'K', 'A'
};

Random rnd = new Random();

int Draw() //Method to pick random number as 'card'
{
    int card = rnd.Next(0, Deck.Length);
    return card;
};


//Deal hand to player. Display both cards
Console.WriteLine("PLAYER");

object[] playerHand = new object[] //array for player's hand
{
    Deck[Draw()], Deck[Draw()]
};

int[] playerCardCount = new int[playerHand.Length];
{

};


int playerCardVal = 0;

for (int i = 0; i < playerHand.Length; i++)
{
    if (Convert.ToInt32(playerHand[i]) > 10 && Convert.ToInt32(playerHand[i]) != 65)
    {
        playerCardVal = 10;
        playerCardCount[i] = playerCardVal;
    }
    else if (Convert.ToInt32(playerHand[i]) >= 0 && Convert.ToInt32(playerHand[i]) <= 10)
    {
        playerCardVal = Convert.ToInt32(playerHand[i]);
        playerCardCount[i] = playerCardVal;
    }
    else if (Convert.ToInt32(playerHand[i]) == 65)
    {
        playerCardVal = 11;
        playerCardCount[i] = playerCardVal;
    }
    Console.Write("CARD: {0} ", playerHand[i]);
    Console.WriteLine("VALUE: {0}", playerCardCount[i]);
}

Console.WriteLine(String.Empty);

//Deal hand to dealer. Display first card
Console.WriteLine("DEALER");

object[] dealerHand = new object[] //array for dealer's hand
{
    Deck[Draw()], Deck[Draw()]
};

int[] dealerCardCount = new int[dealerHand.Length];
{

};


int dealerCardVal = 0;

for (int i = 0; i < dealerHand.Length; i++)
{
    if (Convert.ToInt32(dealerHand[i]) > 10 && Convert.ToInt32(dealerHand[i]) != 65)
    {
        dealerCardVal = 10;
        dealerCardCount[i] = dealerCardVal;
    }
    else if (Convert.ToInt32(dealerHand[i]) >= 0 && Convert.ToInt32(dealerHand[i]) <= 10)
    {
        dealerCardVal = Convert.ToInt32(dealerHand[i]);
        dealerCardCount[i] = dealerCardVal;
    }
    else if (Convert.ToInt32(dealerHand[i]) == 65)
    {
        dealerCardVal = 11;
        dealerCardCount[i] = dealerCardVal;
    }
    Console.Write("CARD: {0} ", dealerHand[i]);
    Console.WriteLine("VALUE: {0}", dealerCardCount[i]);
}

Console.WriteLine(String.Empty);

int playerTotal = 0;
int dealerTotal = 0;


//~~~~Calculate totals~~~~

//add card values in player hand
foreach (var VARIABLE in playerCardCount)
{
    playerTotal += VARIABLE;
}

if (playerTotal > 21 && playerCardCount.Length == 2)
{
    playerTotal -= 10;
}

Console.WriteLine("Player: {0}", playerTotal);

//add card values in dealer hand
foreach (var VARIABLE in dealerCardCount)
{
    dealerTotal += VARIABLE;
}

if (dealerTotal > 21 && dealerCardCount.Length == 2)
{
    dealerTotal -= 10;
}

Console.WriteLine("Dealer: {0}", dealerTotal);

Console.WriteLine();

//Hit/Stand


//Calculate winner

Console.WriteLine(GetWinner());


string GetWinner()
{
    string winner = String.Empty;
    if (playerTotal > dealerTotal && playerTotal != 21)
    {
        winner = "Player Wins.";
    }
    else if (playerTotal == dealerTotal) { winner = "Push."; }
    else if (playerTotal < dealerTotal && dealerTotal != 21)
    {
        winner = "Dealer Wins.";
    }
    else if (playerTotal == 21 && playerCardCount.Length == 2)
    {
        winner = "PLAYER BLACK JACK.";
    }
    else if (dealerTotal == 21 && dealerCardCount.Length == 2)
    {
        winner = "DEALER BLACK JACK.";
    }
    else if (playerTotal == dealerTotal) { winner = "Push."; }

    if (playerTotal > 21 && dealerTotal < 21)
    {
        winner = "Dealer Wins.";
    }
    else if (dealerTotal > 21 && playerTotal < 21)
    {
        winner = "Player Wins.";
    }

    return winner;
};