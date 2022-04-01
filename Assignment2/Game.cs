using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
        public StringBuilder WrongGuesses { get; private set; }
        public List<string> WrongWordGuesses { get; private set; }
        public int WrongGuessesCount { get; private set; }
        public char[] CorrectGuesses { get; private set; }
        public int CorrectGuessesCount { get; private set; }
        public int TotalGuessesCount { get { return CorrectGuessesCount + WrongGuessesCount; } }
        public string SecretWord { get; private set; }

        public void Reset()
        {
            SecretWord = randomWord.Next();
            CorrectGuesses = new char[SecretWord.Length];
            WrongGuesses = new StringBuilder();
            WrongWordGuesses = new List<string>();
            WrongGuessesCount = 0;
        }

        public bool HaveCorrectWord()
        {
            return CorrectGuesses.SequenceEqual(SecretWord.ToCharArray());
        }

        public bool CheckGuess(string input)
        {
            if (input.Length == 1)
                return CorrectGuesses.Contains<char>(input[0]) || WrongGuesses.ToString().Contains(input[0]);

            return WrongWordGuesses.Contains(input);
        }

        public bool Guess(string input)
        {
            return (input.Length == 1) ? GuessCharacter(input[0]) : GuessWord(input);
        }

        private bool GuessCharacter(char input)
        {
            int i = SecretWord.IndexOf(input), j = 0;
            if (i >= 0)
            {
                do {
                    CorrectGuesses[i] = SecretWord[i];
                    // same letter may exist more than once
                    i = SecretWord.IndexOf(input, Math.Min(i + 1, SecretWord.Length));
                    j++;
                }
                while (i > 0);
                CorrectGuessesCount++;
                return true;
            }
            else /*wrong letter*/
            {
                WrongGuesses.Append(input);
                WrongGuessesCount++;
                return false;
            }
        }

        private bool GuessWord(string input)
        {
            if (input == SecretWord) {
                CorrectGuesses = input.ToCharArray();
                CorrectGuessesCount++;
                return true;
            }
            else /*wrong word*/ {
                WrongWordGuesses.Add(input);
                WrongGuessesCount++;                
                return false;
            }
        }

        public void PrintLogo()
        {
            WriteLine(hangManLogo);
        }

        public void PrintHangman()
        {
            for (int i = 0; i < hangMan.Length; i++)
            {
                if ((WrongGuessesCount >= 1 && (i == 9 || i == 18 || i == 27 || i == 36 || i == 45)) // pole
                 || (WrongGuessesCount >= 2 && (i <= 4))    // beam
                 || (WrongGuessesCount >= 3 && (i == 13))   // rope
                 || (WrongGuessesCount >= 4 && (i == 22))   // head
                 || (WrongGuessesCount >= 5 && (i == 31))   // neck
                 || (WrongGuessesCount >= 6 && (i == 30))   // larm
                 || (WrongGuessesCount >= 7 && (i == 32))   // rarm
                 || (WrongGuessesCount >= 8 && (i == 39))   // lleg
                 || (WrongGuessesCount >= 9 && (i == 41)))  // rleg
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

        public void PrintGuesses()
        {
            if (WrongGuessesCount > 0)
                ForegroundColor = ConsoleColor.Red;

            Write($"{WrongGuessesCount}\n\n");
            ForegroundColor = ConsoleColor.White;
        }

        public void PrintSecretWord()
        {
            string guesses = new string(CorrectGuesses);

            for (int i = 0; i < SecretWord.Length; i++)
            {
                if (guesses.Contains(SecretWord[i].ToString()))
                    Write($"{SecretWord[i]} ");
                else
                    Write($"_ ");
            }
        }

        public void PrintAlphabet()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                ForegroundColor 
                    = CorrectGuesses.Contains<char>(c)
                    ? ConsoleColor.Green
                    : WrongGuesses.ToString().Contains(c)
                    ? ConsoleColor.Red
                    : ConsoleColor.White;

                Write($"{c} ");
            }

            ForegroundColor = ConsoleColor.White;
        } 
    }
}