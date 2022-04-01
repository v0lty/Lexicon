using System;
using System.Linq;
using System.Text;

using static System.Console;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            Hangman hangman = new Hangman();

            do {
                Clear();
                hangman.Reset();                
                hangman.PrintLogo();
                WriteLine("\n\nWelcome to Hangman!");
                WriteLine("Press any key to start playing...");
                ReadKey();

                string message = null;

                do {
                    Clear();
                    hangman.PrintLogo();
                    hangman.PrintHangman();
                    WriteLine("Number of wrong guesses:");
                    hangman.PrintGuesses();
                    WriteLine("Secret word:");
                    hangman.PrintSecretWord();
                    WriteLine("\n\nAlphabet:");
                    hangman.PrintAlphabet();
                    WriteLine("\n");

                    // check needs to occur before HandleInput() so that everything
                    // is drawn on last loop, same goes for messages..
                    if (hangman.HaveCorrectWord() || hangman.WrongGuessesCount >= 10)
                        break;

                    WriteLine(message ?? "");

                    var input = Input.RequestInput("Enter your guess:");

                    if (hangman.CheckGuess(input)) {
                        message = $"You have already guessed '{input}'.";
                        continue;
                    }

                    if (hangman.Guess(input))
                        message = "Correct guess!";
                    else
                        message = "Wrong guess, try again!";                    
                }
                while (true);

                if (hangman.HaveCorrectWord() && hangman.WrongGuessesCount < 10)
                {
                    ForegroundColor = ConsoleColor.Green;
                    WriteLine($"Congratulation you won!");
                    ForegroundColor = ConsoleColor.White;
                }
                else {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine($"Game Over! Correct word was '{hangman.SecretWord}'.");
                    ForegroundColor = ConsoleColor.White;
                }

            }
            while (Input.RequestConfirmation("Do you want to play again?"));
        }
    }
}