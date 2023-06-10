using System.Drawing;
using System.Runtime.InteropServices;

internal class Program
{

    enum Direction
    {
        Up, Right, Donw, Left
    }

    private static void Main(string[] args)
    {
        // Game Snake
        var speed = 100;
        var foodPosition = Point.Empty;
        var screenSize = new Size(70, 20);
        var snake = new List<Point>();
        var longSnake = 1;
        var score = 0;
        var position = new Point(0, 9);
        snake.Add(position);
        var direction = Direction.Donw;

        // Pintar Pantalla
        PaintScreen(screenSize);

        // Implementar Marcador
        SetScore(0);

        // While (movimiento)
        while (HayMovimiento(snake, position, longSnake, screenSize))
        {
            // Controlar la velocidad
            Thread.Sleep(speed);

            direction = GetDirection(direction);

            position = NextPosition(direction, position);

            if (position.Equals(foodPosition))
            {
                foodPosition = Point.Empty;
                longSnake++;
                score++;

                SetScore(score);
            }

            if (foodPosition == Point.Empty)
                foodPosition = ShowFood(screenSize, snake);

        }

        Console.ResetColor();
        Console.SetCursorPosition(screenSize.Width / 2 - 4, screenSize.Height / 2);
        Console.Write("Game Over");

        Thread.Sleep(2000);
        Console.ReadKey();
    }

    private static void PaintScreen(Size size)
    {
        Console.WindowHeight = size.Height + 2;
        Console.WindowWidth = size.Width + 2;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.Title = "Game Snake";
        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.White;
        Console.Clear();

        Console.BackgroundColor = ConsoleColor.Black;

        for (int row = 0; row < size.Height; row++)
        {
            for (int col = 0; col < size.Width; col++)
            {
                Console.SetCursorPosition(col + 1, row + 1);
                Console.Write(" ");
            }
        }
    }
    private static void SetScore(int score)
    {
        Console.SetCursorPosition(1, 0);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write($"Scores: {score}");
    }

    private static Direction GetDirection(Direction currentDirection)
    {
        if (!Console.KeyAvailable) return currentDirection;

        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                currentDirection = Direction.Up;
                break;
            case ConsoleKey.RightArrow:
                currentDirection = Direction.Right;
                break;
            case ConsoleKey.DownArrow:
                currentDirection = Direction.Donw;
                break;
            case ConsoleKey.LeftArrow:
                currentDirection = Direction.Left;
                break;
        }

        return currentDirection;

    }

    private static Point NextPosition(Direction direction, Point currentPosition)
    {
        Point nextPosition = new Point(currentPosition.X, currentPosition.Y);

        switch (direction)
        {
            case Direction.Up:
                nextPosition.Y--;
                break;
            case Direction.Right:
                nextPosition.X++;
                break;
            case Direction.Donw:
                nextPosition.Y++;
                break;
            case Direction.Left:
                nextPosition.X--;
                break;
        }

        return nextPosition;
    }

    private static Point ShowFood(Size screenSize, List<Point> snake)
    {
        var foodPosition = Point.Empty;
        var headSnake = snake.Last();
        var rnd = new Random();

        do
        {
            var x = rnd.Next(0, screenSize.Width - 1);
            var y = rnd.Next(0, screenSize.Height - 1);

            if (snake.All(p => p.X != x || p.Y != y) &&
                Math.Abs(x - headSnake.X) + Math.Abs(y - headSnake.Y) > 8)
            {
                foodPosition = new Point(x, y);
            }

        } while (foodPosition == Point.Empty);

        Console.BackgroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(foodPosition.X + 1, foodPosition.Y + 1);

        Console.Write(" ");

        return foodPosition;
    }

    private static bool HayMovimiento(List<Point> snake, Point position, int longSnake, Size sizeScreen)
    {
        var lastPoint = snake.Last();

        if (lastPoint.Equals(position)) return true;

        if (snake.Any(x => x.Equals(position))) return false;

        // Verificar que choquemos con alguna pared, y no retroceda
        if (position.X < 0 || position.X >= sizeScreen.Width || position.Y < 0 || position.Y >= sizeScreen.Height) return false;

        //Cuerpo de la Serpiente
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.SetCursorPosition(lastPoint.X + 1, lastPoint.Y + 1);
        Console.Write(" ");

        snake.Add(position);

        // Cabeza de la Serpiente
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(position.X + 1, position.Y + 1);
        Console.Write(" ");

        // Controlar la Longitud de la Serpiente
        if (snake.Count > longSnake)
        {
            var removePoint = snake[0];

            snake.RemoveAt(0);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(removePoint.X + 1, removePoint.Y + 1);
            Console.Write(" ");
        }

        return true;
    }

}