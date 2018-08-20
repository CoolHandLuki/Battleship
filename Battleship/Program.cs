using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
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
        static TileType[,] CreateBattlefield(byte rows, byte columns, byte playerShips)
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

        static void PrintBattleFieldArray()
        {
            // Initialize a variable to contain our spacer so we can adjust it more easily
            string spacer = "\t";

            // write a alphabetical character to mark the columns for visual orientation
            // For now we introduced a bug if the user selects a greater amount columns
            // than there are letters in the alphabet. Either limit columns to 26
            // or provide some potentially endless combination generator
            // (like in Excel A B ... X Y Z AA AB AC ... etc)
            Console.Write(spacer);
            if (spacer != "\t")
                Console.Write(" ");
            for (byte i = 0; i < columns; i++)
                Console.Write("{0}{1}", alpha[i], spacer);
            Console.WriteLine();

            for (byte i = 0; i < rows; i++)
            {
                // ... write spacer to make the battlefield more pleasant to read
                // write the row number for visual orientation
                Console.Write("{0}{1}", i, spacer);
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
                                Console.Write("s{0}", spacer);
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
            }

            // Also print an empty new line at the end of each row loop to ensure that we have a line break.
            // fill it with the right amount of whitespaces. Otherwise .net doesn't apply Console.Backgroundcolor
            Console.WriteLine("{0}", String.Concat(Enumerable.Repeat(spacer, columns + 1)));

            // And a final readline
            Console.ReadLine();
        }

        static void SetupEnvironment()
        {
            // Set up some environment variables
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            // How many ships of each type will each player start with?
            playerShips = 8;
            // What are the dimensions of our battle field?
            rows = 8;
            columns = 6;

            if (playerShips > rows * columns)
            {
                Console.WriteLine("The amount of player ships ({0}) is larger than the total amount of " +
                "tiles (rows: {1} * {2} = {3}). Aborting!", playerShips, rows, columns, rows * columns);
                Console.ReadLine();
                return;
            }

            // Call CreateBattlefield to set up the grid
            TileType[,] battleField = CreateBattlefield(rows, columns, playerShips);
        }

        static void Main(string[] args)
        { 
            SetupEnvironment();
            PrintBattleFieldArray();
        }
    }
}