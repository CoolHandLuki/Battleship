using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading; // Threading can be used for functions like sleep

namespace Battleship
{
    public class Player
    {
        public int wins { get; set; }
        public int losses { get; set; }
        static int rows = 4, columns = 4;
        public Board board;

        public Player()
        {
            wins = 0;
            losses = 0;
            CreateNewBoard();
        }

        public void CreateNewBoard()
        {
            board = new Board(rows, columns);
        }
        
        public void Won()
        {
            wins += 1;
            CreateNewBoard();
        }

        public void Lost()
        {
            losses -= 1;
            CreateNewBoard();
        }

        public void TargetCell()
        {
            int selectedRow = 0;
            int selectedColumn = 0;
            ConsoleKey pressedButton;
            do
            {
                pressedButton = Console.ReadKey(true).Key;
                switch (pressedButton)
                {
                    case ConsoleKey.LeftArrow:
                        if (selectedColumn == 0)
                        {
                            selectedColumn = board.columns - 1;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                        else
                        {
                            selectedColumn -= 1;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        if (selectedColumn == board.columns - 1)
                        {
                            selectedColumn = 0;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                        else
                        { 
                            selectedColumn += 1;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        if (selectedRow == 0)
                        {
                            selectedRow = board.rows - 1;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                        else
                        {
                            selectedRow -= 1;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        if (selectedRow == board.rows - 1)
                        {
                            selectedRow = 0;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                        else
                        {
                            selectedRow += 1;
                            board.PrintBoard(selectedRow, selectedColumn);
                            break;
                        }
                }
                //Console.WriteLine(pressedButton);
                //Console.WriteLine("new selRow: {0}, new selCol: {1}", selectedRow, selectedColumn);
            } while (pressedButton != ConsoleKey.Enter);
        }
    }

    public class Board
    {
        public static char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public int rows { get; set; }
        public int columns { get; set; }
        public int ships { get; set; }
        public TileType[,] board { get; set; }

        public enum TileType : byte
        {
            untouched = 0,
            ship = 1,
            hit = 2,
            miss = 3
        }

        public Board(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            board = new TileType[rows, columns];
        }

        public void PrintBoard()
        {
            PrintBoard(false, null, null);
        }

        public void PrintBoard(bool developer)
        {
            PrintBoard(developer, null, null);
        }

        public void PrintBoard(int selectRow, int selectCol)
        {
            PrintBoard(false, selectRow, selectCol);
        }

        public void PrintBoard(bool developer, int? selectRow, int? selectCol)
        {
            ConsoleColor oldBackground = Console.BackgroundColor;
            ConsoleColor oldForeground = Console.ForegroundColor;

            Console.Clear();
            for (byte i = 0; i < rows; i++)
            { 
                for (byte j = 0; j < columns; j++)
                {
                    if (selectRow != null && selectCol != null && i == selectRow && j == selectCol)
                    {
                        SwapConsoleColors();
                    }
                    TileType t = board[i, j];
                    switch (t)
                    {
                        case TileType.hit:
                            Console.Write("X");
                            break;
                        case TileType.miss:
                            Console.Write("o");
                            break;
                        case TileType.ship:
                            if (developer == true)
                                Console.Write("s");
                            else
                                Console.Write(".");
                                break;
                        case TileType.untouched:
                            Console.Write(".");
                            break;
                    }
                    if (selectRow != null && selectCol != null && i == selectRow && j == selectCol)
                    {
                        SwapConsoleColors();
                    }
                    Console.Write(" ");
                }
            Console.Write("\n");
            }
        }

        public void SwapConsoleColors()
        {
            ConsoleColor tmp = Console.BackgroundColor;
            Console.BackgroundColor = Console.ForegroundColor;
            Console.ForegroundColor = tmp;
        }
    }

    class Program
    {
        // Write logic for the game "Battleship"
        // Have a two dimensional array created as our battle grid and ships spawned in random locations
        // Draw the grid
        // Provide visual feedback to differentiate:    1) untouched tiles
        //                                              2) shot tiles without a ship (misses)
        //                                              3) shot tiles with a ship (hits)+
        // Also mark the rows and columns with values so it's easier for the user to orient themselves
        // Have the player guess cells until the ship is destroyed
        // Provide options to start over or quit

        // Global variables for our game
        static byte rows, columns, playerShips;
        static bool dev = false;
        // Create an array containing the letters of the alphabet to later use as a visual aid for the x-axis
        static char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        // A custom enum which stores all the states a tile can be in
        enum TileType : byte
        {
            untouched = 0,
            ship = 1,
            hit = 2,
            miss = 3
        }

        // Declare the array which will store our battlefield as a global variable because it will
        // be changed a lot during the runtime of our program
        static TileType[,] battleFieldArray;

        // A function to create our battlefield array
        static TileType[,] CreateBattlefield()
        {
            // Get random number generators for rows and colums
            Random RandomNumberGenerator = new Random();

            battleFieldArray = new TileType[rows, columns];

            // Populate the array with water tiles
            for (byte i = 0; i < rows; i++)
                for (byte j = 0; j < columns; j++)
                    battleFieldArray[i, j] = TileType.untouched;

            // Place the ships at random locations
            for (byte i = 0; i < playerShips; i++)
            {
                byte rRow = (byte)RandomNumberGenerator.Next(rows);
                byte rCol = (byte)RandomNumberGenerator.Next(columns);
                if (battleFieldArray[rRow, rCol] != TileType.untouched)
                    // We hit a tile that already contains a ship. Retry by decrementing the counter by 1.
                  i--;
                else
                    battleFieldArray[rRow, rCol] = TileType.ship;
            }
            return battleFieldArray;
        }

        static void WriteEmptyLine(string spacer)
        {
            // We need different calls to WriteLine depending on whether \t or (a) whitespace(s) are used for spacing
            if (spacer == "\t")
                // The column length to spacer ratio has been determined by trial and error and could probably be solved
                // in a smarter way than some if else statements with hard coded limits 
                // if someone were to actually think about the problem.
                if (columns < 12)
                    Console.WriteLine("{0}", String.Concat(Enumerable.Repeat(spacer, columns + 1)));
                else
                    Console.WriteLine("{0}", String.Concat(Enumerable.Repeat(spacer, columns + 2)));
            else
                Console.WriteLine("{0}", String.Concat(Enumerable.Repeat(spacer, columns + 1)));
        }

        static void PrintBattleFieldArray()
        {
            // Declare a variable to contain our spacer so we can have a flexible interface
            string spacer;
            // The maximum amount of rows and columns that fit the screen when using tab as a spacer is 13
            if (columns <= 13)
                spacer = "\t";
            else
                spacer = " ";

            // write a alphabetical character to mark the columns for visual orientation
            // For now we introduced a bug if the user selects a greater amount columns
            // than there are letters in the English alphabet. Either limit columns to 26
            // or provide some potentially endless combination generator
            // (like in Excel A B ... Y Z AA AB ... etc)
            Console.Write(spacer);
            if (spacer != "\t")
                Console.Write(" ");
            for (byte i = 0; i < columns; i++)
                Console.Write("{0}{1}", alpha[i], spacer);
            Console.WriteLine();
            if (spacer == "\t")
                // If we are using tab as a spacer add an empty line between the header and the playing field
                //  to format the rows and columns a bit more evenly
                WriteEmptyLine(spacer);

            for (byte i = 0; i < rows; i++)
            {
                // ... write spacer to make the battlefield more pleasant to read
                // write the row number for visual orientation
                Console.Write("{0}{1}", i + 1, spacer);
                {
                    // ... for each column ...
                    for (byte j = 0; j < columns; j++)
                    // ... append the columns in that row to the write buffer ...
                    // ... deciding what to print based on a switch statement of our enum type.
                    {
                        switch (battleFieldArray[i, j])
                        {
                            case TileType.untouched:
                                Console.Write(".{0}", spacer);
                                break;
                            case TileType.ship:
                                if (dev == true)
                                    Console.Write("s{0}", spacer); // Print the ships in case developer mode is enabled
                                else
                                    Console.Write(".{0}", spacer);
                                break;
                            case TileType.hit:
                                Console.Write("X{0}", spacer);
                                break;
                            case TileType.miss:
                                Console.Write("O{0}", spacer);
                                break;
                        }
                    }
                }
                // Complete the line at the end of each column loop
                Console.WriteLine();
                if (spacer == "\t")
                    // If we are using tab as a spacer add an empty line between each line to format the rows
                    // and columns a bit more evenly
                    WriteEmptyLine(spacer);
            }
        }

        static void SetupEnvironment()
        {
            // Set up some environment variables
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            // for now limit rows and columns so we don't have to deal with formatting too much
            byte maxRows = 26;
            byte maxColumns = 28;

            // Does the user want to customize some of the game's options?
            bool customGame = false;

            Console.WriteLine("Hello, do you want to customize some options or play with default settings? [c/D]");
            string input = Console.ReadLine();
            if (input.Length == 0 || input.ToUpper() == "D")
            { }
            else if (input.ToUpper() == "C")
                customGame = true;
            else if (input == "dev")
                dev = true;
            else
            {
                Console.WriteLine("{0} is neither \"c\" nor \"d\", I'm gonna assume, that you want to play with the default settings.", input);
                Console.ReadLine();
            }

            if (customGame == false)
            {
                // How many ships of each type will each player start with?
                playerShips = 6;
                // What are the dimensions of our battle field?
                rows = 6;
                columns = 6;
            }
            else
            {
                bool inputIsValid = false;
                while (inputIsValid == false)
                {
                    Console.WriteLine("How many rows should the playing field have? Please enter a number between 1 and {0}", maxRows);
                    input = Console.ReadLine();
                    if (Byte.TryParse(input, out rows))
                        if (rows > 0 && rows <= maxRows)
                            inputIsValid = true;
                }
                inputIsValid = false;
                while (inputIsValid == false)
                {
                    Console.WriteLine("How many columns should the playing field have? Please enter a number between 1 and {0}", maxColumns);
                    input = Console.ReadLine();
                    if (Byte.TryParse(input, out columns))
                        if (columns > 0 && columns <= maxColumns)
                            inputIsValid = true;
                }
                inputIsValid = false;
                while (inputIsValid == false)
                {
                    Console.WriteLine("How many ships should each player have. Please enter a number between 1 and {0}", columns * rows - 1);
                    input = Console.ReadLine();
                    if (Byte.TryParse(input, out playerShips))
                        if (playerShips > 0 && playerShips <= columns * rows - 1)
                            inputIsValid = true;
                }
            }
            // Call CreateBattlefield to set up the grid
            TileType[,] battleField = CreateBattlefield();
        }

        static void ShootAtShips()
        {
            byte[] targetCoordinates = GetTargetInput();
            // Console.WriteLine("rows is: {0} and columns is {1}", rows, columns);
            // Console.WriteLine("targetColumn is: {0}", targetCoordinates[0]);
            // Console.WriteLine("targetRow is : {0}", targetCoordinates[1]);
            switch (battleFieldArray[targetCoordinates[0],targetCoordinates[1]])
            {
                case TileType.untouched:
                    battleFieldArray[targetCoordinates[0], targetCoordinates[1]] = TileType.miss;
                    break;
                case TileType.ship:
                    battleFieldArray[targetCoordinates[0], targetCoordinates[1]] = TileType.hit;
                    playerShips--;
                    break;
                default:
                    break;
            }

        }

        static byte[] GetTargetInput()
        // The return value is a byte array which stores the targetRow in its 0th position
        // and the targetColumn in its 1st position
        {
            byte[] targetCoordinates = new byte[2];
            string input;
            bool columnIsValid = false;
            bool rowIsValid = false;

            // Keep on looping until we've received valid input
            while (columnIsValid == false)
            {
                Console.WriteLine("Please enter your target column: ");
                input = Console.ReadLine();
                // Is the entered string in the right size?
                if (input.Length != 1)
                    continue;
                // Is the character alphabetical?
                if (char.IsLetter(input[0]) == false)
                    continue;
                // Convert the letter into uppercase
                input = input.ToUpper();
                // Convert the letter into its positional value on the playing field
                for (byte i = 0; i < columns; i++)
                {
                    if (alpha[i] == input[0])
                    {
                        targetCoordinates[1] = i;
                        columnIsValid = true;
                        break;
                    }
                }
            }

            while (rowIsValid == false)
            {
                int tmp; // Is there a way to do this without "bridging" from string to byte via int?

                Console.WriteLine("Please enter your target row: ");
                input = Console.ReadLine();

                // Does the string parse into a number?
                if (!int.TryParse(input, out tmp))
                    continue;

                tmp--; // Decrement tmp because our battflefield array indeces begin at 0 not at 1

                // Is the column within the array index range of 0 and rows?)
                if (tmp >= 0 && tmp < rows)
                {
                    targetCoordinates[0] = (byte)tmp;
                    // We have valid input for both our columns and our rows!
                    rowIsValid = true;
                }
                else
                    continue;
            }
            return targetCoordinates;
        }

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            bool developer;

            if (args.Length > 1)
                if (args[0] == "-dev" && args[1] == "1")
                    Console.WriteLine("Developer mode enabled.");
            developer = true;
            Player player1 = new Player();
            player1.board = new Board(4, 5);
            player1.TargetCell();

            Console.ReadLine();
            return;

            ////Console.WriteLine("args.Length = {0}", args.Length);
            //// Parse whether developer mode should be enabled or not
            //if (args.Length > 1)
            //    if (args[0] == "-dev" && args[1] == "1")
            //        Console.WriteLine("Developer mode enabled.");
            //        dev = true;
            //// Setup
            //SetupEnvironment();

            //// Begin the gameplay loop
            //while (playerShips > 0)
            //{
            //    PrintBattleFieldArray();
            //    ShootAtShips();
            //}
            //PrintBattleFieldArray();
            //Console.WriteLine("You won!");
            //Console.ReadLine();
        }
    }
}