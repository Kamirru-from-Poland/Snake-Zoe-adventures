const byte MapSize = 20;
const byte X = MapSize * 2;//rows
const byte Y = MapSize;//columns
const byte StartingSize = 5;
ushort ms = 100;//dellay
char[,] board = new char[X, Y];
sbyte[,] boardTTL = new sbyte[X, Y];
List<int> snakeX = new List<int>();
List<int> snakeY = new List<int>();
ConsoleKeyInfo ActuallyPressedKey = new ConsoleKeyInfo();
bool swapedArrows = false;
for (int i = 0; i < StartingSize; i++)
{
    snakeX.Add(X / 2 - i);
    snakeY.Add(Y / 2);
}
Console.Title = "Snake Zoe adventures ♪";
Console.ForegroundColor = ConsoleColor.Green;
Console.CursorVisible = false;
Directions currentDirection = Directions.Right;//default
Random Rnd = new Random();
static void Eraser(char[,] board, sbyte[,] boardTTL)
{
    for (int i = 0; i < Y; i++)
    {
        board[0, i] = '█';
        board[X - 1, i] = '█';
        boardTTL[0, i] = -2;
        boardTTL[X - 1, i] = -2;
    }
    for (int i = 0; i < X; i++)
    {
        board[i, 0] = '█';
        board[i, Y - 1] = '█';
        boardTTL[i, 0] = -2;
        boardTTL[i, Y - 1] = -2;
    }
    for (int i = 1; i < Y - 1; i++)
        for (int j = 1; j < X - 1; j++)
        {
            if (boardTTL[j, i] > 0)
            {
                boardTTL[j, i]--;
            }
            else
            {
                board[j, i] = ' ';
                boardTTL[j, i] = 0;
            }
        }
    Console.SetCursorPosition(0, 0);
}
static void Painter(char[,] board, sbyte[,] boardTTL, List<int> snakeX, List<int> snakeY, bool currentlyRunning = true, char head = '҈')
{
    board[snakeX[0], snakeY[0]] = head;
    boardTTL[snakeX[0], snakeY[0]] = -1;
    for (int i = 1; i < snakeX.Count; i++)
    {
        board[snakeX[i], snakeY[i]] = '҈';
        boardTTL[snakeX[i], snakeY[i]] = -1;
    }
    int points = head != '⸗' ? snakeX.Count - 5 : snakeX.Count - 4;
    Console.WriteLine($"Points: ₿{points}\t\tAuthor: Kamirru");
    for (int i = 0; i < Y; i++)
    {
        for (int j = 0; j < X; j++)
            Console.Write(board[j, i]);
        Console.WriteLine();
    }
    //test
    //for (int i = 0; i < Y; i++)//temp only
    //{
    //   for (int j = 0; j < X; j++)
    //       Console.Write(boardTTL[j, i]);
    //   Console.WriteLine();
    //}
    //test
    if (!currentlyRunning) Console.WriteLine($"You achieved ₿{points} points\nSee you next time ♫");
    Console.WriteLine("Use side arrows to control Zoe ♥");
    Console.WriteLine("[ẟ] - a sweet friut");
    Console.WriteLine("[⸗] - a rotten rat");
    Console.WriteLine("[₰] - it reverses Zoe");
    Console.WriteLine("[↔] - it swaps arrows");
    Console.WriteLine("[?] - a random gift");
}
static void CutSnake(List<int> snakeX, List<int> snakeY)
{
    snakeX.RemoveAt(snakeX.Count - 1);
    snakeY.RemoveAt(snakeY.Count - 1);
}
static void CollisionEvents(char[,] board, List<int> snakeX, List<int> snakeY, ref Directions currentDirection, ref bool swapedArrows, Random rnd)
{
    byte RandomEvent = 0;
    if (board[snakeX[0], snakeY[0]] == '?')
    {
        RandomEvent = (byte)rnd.Next(1, 5);
    }
    if (board[snakeX[0], snakeY[0]] == 'ẟ' || RandomEvent == 1)
    {
        Console.Beep(11111, 150);
    }
    else if (board[snakeX[0], snakeY[0]] == '⸗' || RandomEvent == 2)
    {
        CutSnake(snakeX, snakeY);
        CutSnake(snakeX, snakeY);
    }
    else if (board[snakeX[0], snakeY[0]] == '₰' || RandomEvent == 3)
    {
        snakeX.Reverse();
        snakeY.Reverse();
        CutSnake(snakeX, snakeY);
        switch (currentDirection)
        {
            case Directions.Right:
                currentDirection = Directions.Left;
                break;
            case Directions.Down:
                currentDirection = Directions.Up;
                break;
            case Directions.Left:
                currentDirection = Directions.Right;
                break;
            case Directions.Up:
                currentDirection = Directions.Down;
                break;
        }
    }
    else if (board[snakeX[0], snakeY[0]] == '↔' || RandomEvent == 4)
    {
        swapedArrows = !swapedArrows;
        CutSnake(snakeX, snakeY);
    }
    else
    {
        CutSnake(snakeX, snakeY);
    }
}
static void Delayer(ushort ms, Directions currentDirection)
{
    switch (currentDirection)
    {
        case Directions.Right:
            Thread.Sleep(ms);
            break;
        case Directions.Down:
            Thread.Sleep((int)Math.Truncate(ms * 1.5));
            break;
        case Directions.Left:
            Thread.Sleep(ms);
            break;
        case Directions.Up:
            Thread.Sleep((int)Math.Truncate(ms * 1.5));
            break;
    }
}
static void Forwarder(Directions currentDirection, List<int> snakeX, List<int> snakeY)
{
    switch (currentDirection)
    {
        case Directions.Right:
            snakeX.Insert(0, (int)snakeX[0] + 1);
            snakeY.Insert(0, (int)snakeY[0]);
            break;
        case Directions.Down:
            snakeX.Insert(0, (int)snakeX[0]);
            snakeY.Insert(0, (int)snakeY[0] + 1);
            break;
        case Directions.Left:
            snakeX.Insert(0, (int)snakeX[0] - 1);
            snakeY.Insert(0, (int)snakeY[0]);
            break;
        case Directions.Up:
            snakeX.Insert(0, (int)snakeX[0]);
            snakeY.Insert(0, (int)snakeY[0] - 1);
            break;
    }
}
for (; ; )
{
    Eraser(board, boardTTL);
    while (Console.KeyAvailable)
    {//this can not be a function, due to it as a function work incorrectly
        ActuallyPressedKey = Console.ReadKey(true);
        var key = ActuallyPressedKey.Key;
        if (swapedArrows)
        {
            if (key == ConsoleKey.LeftArrow)
                key = ConsoleKey.RightArrow;
            else
                key = ConsoleKey.LeftArrow;
        }
        if (key == ConsoleKey.LeftArrow)//counterclockwise
            switch (currentDirection)
            {
                case Directions.Right:
                    currentDirection = Directions.Up;
                    break;
                case Directions.Down:
                    currentDirection = Directions.Right;
                    break;
                case Directions.Left:
                    currentDirection = Directions.Down;
                    break;
                case Directions.Up:
                    currentDirection = Directions.Left;
                    break;
            }
        if (key == ConsoleKey.RightArrow)//clockwise
            switch (currentDirection)
            {
                case Directions.Right:
                    currentDirection = Directions.Down;
                    break;
                case Directions.Down:
                    currentDirection = Directions.Left;
                    break;
                case Directions.Left:
                    currentDirection = Directions.Up;
                    break;
                case Directions.Up:
                    currentDirection = Directions.Right;
                    break;
            }
    }
    Forwarder(currentDirection, snakeX, snakeY);
    //board[30,10]= 'ẟ';
    //board[31,10]= 'ẟ';
    //board[32,10]= 'ẟ';
    //board[33,10]= 'ẟ';
    //board[13,12]= '⸗';
    //board[13,11]= '⸗';
    //board[16,10]= '₰';
    //board[18,10]= '↔';
    //board[20,10]= '?';//test
    for (; ; )// to rem (boardTTL[snakeX[0], snakeY[0]) 
    {
        sbyte rndX = (sbyte)Rnd.Next(1, X - 2);
        sbyte rndY = (sbyte)Rnd.Next(1, Y - 2);
        sbyte rndUnit = (sbyte)Rnd.Next(1, 10);
        char unit = 'ẟ';
        switch (rndUnit)
        {
            case 5:
                unit = '⸗';
                break;
            case 6:
                unit = '₰';
                break;
            case 7:
                unit = '↔';
                break;
            case 8:
            case 9:
                unit = '?';
                break;
        }
        if (boardTTL[rndX, rndY] == 0)
        {
            boardTTL[rndX, rndY] = 30;
            board[rndX, rndY] = unit;
            break;
        }
    }


    if (board[snakeX[0], snakeY[0]] == '█')//this can not be a function, due to it as a function work incorrectly
    {
        CutSnake(snakeX, snakeY);
        Console.Clear();
        Painter(board, boardTTL, snakeX, snakeY, false, '█');
        break;
    }
    CollisionEvents(board, snakeX, snakeY, ref currentDirection, ref swapedArrows, Rnd);
    if (snakeX.Count <= 4)//this can not be a function, due to it as a function work incorrectly
    {
        Console.Clear();
        Painter(board, boardTTL, snakeX, snakeY, false, '⸗');
        break;
    }
    Painter(board, boardTTL, snakeX, snakeY);
    Delayer(ms, currentDirection);
}
Console.ReadKey();