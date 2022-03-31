using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using static System.Console;

namespace Assignment2
{
    public class Hangman
    {
        private static readonly string hangManLogo = 
          @" _   _                                         " + "\n"
        + @"| | | |                                        " + "\n"
        + @"| |_| | _________   ____ ____  ___  ____ ____  " + "\n"
        + @"|  _  |/ _  |  _ \ / _  |  _ \/ _ \/ _  |  _ \ " + "\n"
        + @"| | | ||(_| | | | | (_| | | | | | ||(_| | | | |" + "\n"
        + @"|_| |_|\____|_| |_|\__, |_| |_| |_|\____|_| |_|" + "\n"
        + @"                    __/ |                      " + "\n"                      
        + @"                   |___/                       ";
        private static readonly string hangMan =
          @" ___    " + "\n"
        + @"|   |   " + "\n"
        + @"|   O   " + "\n"
        + @"|  ~|~  " + "\n"
        + @"|  / \  " + "\n"
        + @"|       " + "\n";

        private readonly RandomWord randomWord = new RandomWord();
        private StringBuilder wrongGuesses;
        private int wrongGuessesCount;
        private char[] correctGuesses;
        private string secretWord;
        private string status;       

        public void Run()
        {
            WriteLogo();
            WriteLine("\n\nWelcome to Hangman!");
            WriteLine("Press any key to start playing...");
            ReadKey();

            do /*main loop*/ 
            {
                secretWord = randomWord.Next();                
                correctGuesses = new char[secretWord.Length];
                wrongGuesses = new StringBuilder();
                wrongGuessesCount = 0;
                status = null;

                do /*game loop*/ 
                {
                    Clear();
                    WriteLogo();
                    WriteHangman();
                    WriteGuesses();
                    WriteSecretWord();
                    WriteLetters();
                    
                    // check needs to happend before HandleInput() so that everything
                    // is drawn on last loop
                    if (correctGuesses.SequenceEqual(secretWord.ToCharArray()) 
                     || wrongGuessesCount >= 10)
                        break;

                    WriteLine(status ?? "");
                    HandleInput(RequestInput("Enter your guess:"));                    
                }
                while (true);
                        
                if (wrongGuessesCount < 10)             
                    WriteLine($"Congratulation you won!");             
                else
                    WriteLine($"Game Over! Correct word was '{secretWord}'.");
            }
            while (RequestConfirmation("Do you want to play again?"));
        }

        private void HandleInput(string input)
        {
            if (input.Length > 1) /*word*/
            {
                if (input == secretWord) {
                    correctGuesses = input.ToCharArray();
                    status = null;
                }
                else /*wrong word*/ {
                    wrongGuessesCount++;
                    status = "Wrong guess, try again!";
                }
            }
            else /*letter*/ {

                if (correctGuesses.Contains<char>(input[0]) || wrongGuesses.ToString().Contains(input[0])) {
                    // ignore guess and input
                    status = $"You have already guessed '{input[0]}'.";
                    return;
                }

                int i = secretWord.IndexOf(input[0]), j = 0;
                if (i >= 0) {
                    do { 
                        correctGuesses[i] = secretWord[i];
                        // same letter may exist more than once
                        i = secretWord.IndexOf(input[0], Math.Min(i + 1, secretWord.Length));
                        j++;
                    }
                    while (i > 0);
                    status = $@"'{input[0]}' was fount {j} time{((j > 1) ? "s" : "")} in the word!";
                }
                else /*wrong letter*/ {
                    wrongGuesses.Append(input);
                    wrongGuessesCount++;
                    status = "Wrong guess, try again!";
                }
            }
        }

        private void WriteLogo()
        {
            WriteLine(hangManLogo);
        }

        private void WriteHangman()
        {
            for (int i = 0; i < hangMan.Length; i++)
            {
                if ((wrongGuessesCount >= 1 && (i == 9 || i == 18 || i == 27 || i == 36 || i == 45)) // pole
                 || (wrongGuessesCount >= 2 && (i <= 4))    // beam
                 || (wrongGuessesCount >= 3 && (i == 13))   // rope
                 || (wrongGuessesCount >= 4 && (i == 22))   // head
                 || (wrongGuessesCount >= 5 && (i == 31))   // neck
                 || (wrongGuessesCount >= 6 && (i == 30))   // larm
                 || (wrongGuessesCount >= 7 && (i == 32))   // rarm
                 || (wrongGuessesCount >= 8 && (i == 39))   // lleg
                 || (wrongGuessesCount >= 9 && (i == 41)))  // rleg
                {
                    ForegroundColor = ConsoleColor.Red;
                }
                else {
                    ForegroundColor = ConsoleColor.DarkGray;
                }

                Write(hangMan[i]);
            }
            ForegroundColor = ConsoleColor.White;
            WriteLine();
        }

        private void WriteGuesses()
        {
            WriteLine("Number of wrong guesses:");  

            if (wrongGuessesCount > 0)
                ForegroundColor = ConsoleColor.Red;

            Write($"{wrongGuessesCount}\n\n");
            ForegroundColor = ConsoleColor.White;
        }

        private void WriteSecretWord()
        {
            WriteLine("Secret word:");

            for (int i = 0; i < secretWord.Length; i++)
            {
                string guesses = new string(correctGuesses);

                if (guesses.Contains(secretWord[i].ToString()))
                    Write($"{secretWord[i]} ");
                else
                    Write($"_ ");
            }
            WriteLine("\n");
        }

        private void WriteLetters()
        {
            WriteLine("Letters:");

            for (char c = 'A'; c <= 'Z'; c++)
            {
                ForegroundColor 
                    = correctGuesses.Contains<char>(c)
                    ? ConsoleColor.Green
                    : wrongGuesses.ToString().Contains(c)
                    ? ConsoleColor.Red
                    : ConsoleColor.White;

                Write($"{c} ");
            }

            ForegroundColor = ConsoleColor.White;
            WriteLine("\n");
        }

        private bool RequestConfirmation(string message)
        {
            ConsoleKey input;

            do {
                Write($"{message} [y/n]:");
                input = ReadKey(false).Key;

                if (input != ConsoleKey.Enter)
                    WriteLine();

            } while (input != ConsoleKey.Y 
                  && input != ConsoleKey.N);

            return (input == ConsoleKey.Y);
        }

        private string RequestInput(string message)
        {
            while (true) 
            {
                WriteLine(message);
                var input = ReadLine().ToUpper();

                if (Regex.Matches(input, @"[A-Za-z]").Count > 0)
                {
                    return input;
                }
                else
                    WriteLine("Accepted characters and words with letters A to Z, try again.");
            }
        }
    }
}