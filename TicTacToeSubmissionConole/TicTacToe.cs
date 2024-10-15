using System;
using TicTacToeRendererLib;
using TicTacToeRendererLib.Enums;
using TicTacToeRendererLib.Renderer;

namespace TicTacToeSubmissionConole
{
    // Represents the Tic-Tac-Toe game logic and flow
    public class TicTacToe
    {
        private TicTacToeConsoleRenderer gameRenderer;
        public GameSymbol[,] gameBoard;
        public GameSymbol currentSymbol;

        // Enumeration representing the possible states of a cell on the game board
        public enum GameSymbol
        {
            Empty,
            Cross,
            Circle
        }

        // Initializes a new instance of the TicTacToeGame class
        public TicTacToe()
        {
            gameRenderer = new TicTacToeConsoleRenderer(10, 6);
            gameBoard = new GameSymbol[3, 3];
            currentSymbol = GameSymbol.Cross;  // X starts first
        }

        // Starts and manages the main game loop
        public void Run()
        {
            bool isGameActive = true;
            while (isGameActive)
            {
                gameRenderer.Render();  // Render the current state of the board

                Console.SetCursorPosition(2, 19);
                Console.Write($"Player {currentSymbol}, make your move");

                int selectedRow = -1, selectedColumn = -1;
                bool isValidMove = false;

                // Handle player input and move validation
                while (!isValidMove)
                {
                    try
                    {
                        // Get row input
                        ClearConsoleLine(20);
                        Console.SetCursorPosition(2, 20);
                        Console.Write("Enter Row (0, 1, 2): ");
                        selectedRow = int.Parse(Console.ReadLine());

                        // Get column input
                        ClearConsoleLine(22);
                        Console.SetCursorPosition(2, 22);
                        Console.Write("Enter Column (0, 1, 2): ");
                        selectedColumn = int.Parse(Console.ReadLine());

                        // Validate the move
                        if (IsValidPosition(selectedRow, selectedColumn))
                        {
                            if (gameBoard[selectedRow, selectedColumn] == GameSymbol.Empty)
                            {
                                gameBoard[selectedRow, selectedColumn] = currentSymbol;
                                isValidMove = true;
                            }
                            else
                            {
                                DisplayMessage(23, "Invalid move, cell is already occupied.");
                            }
                        }
                        else
                        {
                            DisplayMessage(23, "Invalid move, enter values between 0 and 2.");
                        }
                    }
                    catch
                    {
                        DisplayMessage(23, "Invalid input, enter a number.");
                    }
                }

                // Map the game symbol to the renderer's player enum
                TicTacToeRendererLib.Enums.PlayerEnum mappedSymbol = (currentSymbol == GameSymbol.Cross)
                    ? TicTacToeRendererLib.Enums.PlayerEnum.X
                    : TicTacToeRendererLib.Enums.PlayerEnum.O;

                gameRenderer.AddMove(selectedRow, selectedColumn, mappedSymbol, true);

                // Check for win or draw
                if (IsWinningMove(selectedRow, selectedColumn))
                {
                    DisplayMessage(24, $"Player {currentSymbol} wins!");
                    isGameActive = false;
                }
                else if (IsBoardFull())
                {
                    DisplayMessage(24, "It's a draw!");
                    isGameActive = false;
                }
                else
                {
                    // Switch to the other player
                    currentSymbol = (currentSymbol == GameSymbol.Cross) ? GameSymbol.Circle : GameSymbol.Cross;
                }
            }
        }

        // Checks if the last move resulted in a win
        // Returns true if the move resulted in a win, false otherwise
        private bool IsWinningMove(int row, int col)
        {
            return (gameBoard[row, 0] == currentSymbol && gameBoard[row, 1] == currentSymbol && gameBoard[row, 2] == currentSymbol) || // Check row
                   (gameBoard[0, col] == currentSymbol && gameBoard[1, col] == currentSymbol && gameBoard[2, col] == currentSymbol) || // Check column
                   (gameBoard[0, 0] == currentSymbol && gameBoard[1, 1] == currentSymbol && gameBoard[2, 2] == currentSymbol) || // Check main diagonal
                   (gameBoard[0, 2] == currentSymbol && gameBoard[1, 1] == currentSymbol && gameBoard[2, 0] == currentSymbol);  // Check other diagonal
        }

        // Checks if the game board is completely filled
        // Returns true if the board is full, false otherwise
        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameBoard[i, j] == GameSymbol.Empty)
                        return false;
                }
            }
            return true;
        }

        // Checks if the given position is within the valid range of the game board
        // Returns true if the position is valid, false otherwise
        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row <= 2 && col >= 0 && col <= 2;
        }

        // Clears a specific line in the console
        private void ClearConsoleLine(int line)
        {
            Console.SetCursorPosition(0, line);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        // Displays a message at a specific line in the console
        private void DisplayMessage(int line, string message)
        {
            Console.SetCursorPosition(2, line);
            Console.WriteLine(message);
        }
    }
}