using ConsoleApp4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Snake
{
    // Full-cell block: fills the whole console cell, so there are no gaps
    const string Block = "█";

    static void Main()
    {
        Console.WindowHeight = 18;
        Console.WindowWidth = 32;

        int screenwidth = Console.WindowWidth;
        int screenheight = Console.WindowHeight;
        int topWall = 0;
        int bottomWall = screenheight - 2;
        int leftWall = 0;
        int rightWall = screenwidth - 1;

        Pixel hoofd = new Pixel();
        hoofd.xPos = screenwidth / 2;
        hoofd.yPos = screenheight / 2;
        hoofd.schermKleur = ConsoleColor.Red;

        string movement = "RIGHT";

        List<Pixel> teljePositie = new List<Pixel>();
        teljePositie.Add(hoofd);

        int score = 0;

        // Create obstacle
        Obstakel obstacle = new Obstakel();
        obstacle.Generate(screenwidth, screenheight - 2);

        while (true)
        {
            Console.Clear();

            // Draw obstacle
            obstacle.Draw();

            // Draw snake
            foreach (Pixel pixel in teljePositie)
            {
                pixel.Draw(Block);
            }

            // Draw walls
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < screenwidth; i++)
            {
                Console.SetCursorPosition(i, topWall);
                Console.Write(Block);

                Console.SetCursorPosition(i, bottomWall);
                Console.Write(Block);
            }

            for (int i = 0; i < screenheight - 1; i++)
            {
                Console.SetCursorPosition(leftWall, i);
                Console.Write(Block);

                Console.SetCursorPosition(rightWall, i);
                Console.Write(Block);
            }

            // Display score at the bottom
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(1, screenheight - 1);
            Console.Write("Score: " + score);

            // Read keyboard input
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);

                switch (info.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (movement != "DOWN")
                            movement = "UP";
                        break;

                    case ConsoleKey.DownArrow:
                        if (movement != "UP")
                            movement = "DOWN";
                        break;

                    case ConsoleKey.LeftArrow:
                        if (movement != "RIGHT")
                            movement = "LEFT";
                        break;

                    case ConsoleKey.RightArrow:
                        if (movement != "LEFT")
                            movement = "RIGHT";
                        break;
                }
            }

            // Calculate new head position
            int newX = hoofd.xPos;
            int newY = hoofd.yPos;

            if (movement == "UP")
                newY--;

            if (movement == "DOWN")
                newY++;

            if (movement == "LEFT")
                newX--;

            if (movement == "RIGHT")
                newX++;

            // Check collision with walls
            if (newX <= leftWall ||
                newX >= rightWall ||
                newY <= topWall ||
                newY >= bottomWall)
            {
                ShowCrash(newX, newY);
                GameOver(score);
            }

            // Update head position
            hoofd.xPos = newX;
            hoofd.yPos = newY;

            // Check if snake eats obstacle
            bool ateObstacle = obstacle.CheckCollision(hoofd);

            if (ateObstacle)
            {
                score++;
                obstacle.Generate(screenwidth, screenheight - 2);
            }

            // Update snake body
            teljePositie.Insert(0, new Pixel(
                hoofd.xPos, hoofd.yPos, ConsoleColor.Green));

            // Remove last segment if food was not eaten
            if (!ateObstacle)
            {
                teljePositie.RemoveAt(teljePositie.Count - 1);
            }

            // Check collision with snake body
            for (int i = 1; i < teljePositie.Count; i++)
            {
                if (hoofd.xPos == teljePositie[i].xPos &&
                    hoofd.yPos == teljePositie[i].yPos)
                {
                    ShowCrash(hoofd.xPos, hoofd.yPos);
                    GameOver(score);
                }
            }

            // Control snake speed
            Thread.Sleep(300);
        }
    }

    // Draw the head at the crash position and pause briefly
    static void ShowCrash(int x, int y)
    {
        if (x >= 0 && x < Console.WindowWidth && y >= 0 && y < Console.WindowHeight)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write(Block);
        }

        Thread.Sleep(600);
    }

    // Display game over screen
    static void GameOver(int score)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;

        Console.SetCursorPosition(10, 6);
        Console.WriteLine("Game Over");

        Console.SetCursorPosition(8, 7);
        Console.WriteLine(" Score: " + score);

        Console.ReadKey();
        Environment.Exit(0);
    }
}
