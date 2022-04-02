using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Hangman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Game-settings
            int maxTries = 10;
            string[] words = { "ice", "ball", "dimension" };

            PlayGame(maxTries, words);
        }

        static void PlayGame(int maxTries, string[] words)
        {
            StringBuilder incorrectLetters = new StringBuilder(maxTries);
            string currentWord = GetRandomWordFromArray(words).ToUpper();
            char[] correctLetters = new char[currentWord.Length];
            int amtCorrectLetters = 0;
            int triesLeft = maxTries;
            bool gameInProgress = true;

            while (gameInProgress)
            {
                Console.Clear();
                DisplayGuessInfo(triesLeft, incorrectLetters, currentWord, correctLetters);
                Guess(ref gameInProgress, currentWord, ref correctLetters, ref amtCorrectLetters, ref triesLeft, ref incorrectLetters);
            }
        }

        static void Guess(ref bool gameInProgress, string currentWord, ref char[] correctLetters, ref int amtCorrectLetters, ref int triesLeft, ref StringBuilder incorrectLetters)
        {
            string guess = GetGuessFromUser(triesLeft, incorrectLetters, currentWord, correctLetters);

            if (IsGuessInWord(guess, currentWord))
            {
                if (guess.Length == 1)
                {
                    CorrectLetter(ref correctLetters, ref amtCorrectLetters, guess);

                    if (GameIsWon(currentWord, correctLetters))
                    {
                        Win(ref gameInProgress, triesLeft, incorrectLetters, currentWord, correctLetters);
                    }
                }
                else if (guess.Length == currentWord.Length)
                {
                    Win(ref gameInProgress, triesLeft, incorrectLetters, currentWord, correctLetters);
                }
                else
                {
                    IncorrectWord(ref triesLeft);
                }
            }
            else
            {
                if (guess.Length == 1)
                {
                    IncorrectLetter(ref triesLeft, guess, ref incorrectLetters);
                }
                else if (guess.Length > 1)
                {
                    IncorrectWord(ref triesLeft);
                }
            }

            if (triesLeft == 0)
            {
                Lose(ref gameInProgress, triesLeft, incorrectLetters, currentWord, correctLetters);
            }
        }

        static void DisplayGuessInfo(int triesLeft, StringBuilder incorrectLetters, string currentWord, char[] correctLetters)
        {
            DisplayTriesLeft(triesLeft);
            DisplayIncorrectGuesses(incorrectLetters);
            Console.WriteLine();
            DisplayHiddenWord(currentWord, correctLetters);
            Console.WriteLine();
        }

        static void Win(ref bool gameInProgress, int triesLeft, StringBuilder incorrectLetters, string currentWord, char[] correctLetters)
        {
            gameInProgress = false;
            Console.Clear();
            DisplayGuessInfo(triesLeft, incorrectLetters, currentWord, correctLetters);
            Console.WriteLine("\nCongrats, you've won!");
        }
        static void Lose(ref bool gameInProgress, int triesLeft, StringBuilder incorrectLetters, string currentWord, char[] correctLetters)
        {
            gameInProgress = false;
            Console.Clear();
            DisplayGuessInfo(triesLeft, incorrectLetters, currentWord, correctLetters);
            Console.WriteLine("\nShmucks! You've been hung and lost the game...");
        }

        static void IncorrectWord(ref int triesLeft)
        {
            triesLeft--;
            Console.WriteLine("Word was incorrect!");
        }

        static void IncorrectLetter(ref int triesLeft, string guess, ref StringBuilder incorrectLetters)
        {
            triesLeft--;
            incorrectLetters.Append(guess[0] + " ");
            Console.WriteLine("Letter was incorrect!");
        }

        static void CorrectLetter(ref char[] correctLetters, ref int amtCorrectLetters, string guess)
        {
            correctLetters[amtCorrectLetters++] = guess[0]; // Add the guessed letter as char after last char in array
            Console.WriteLine($"You guessed a correct letter ({guess})!");
        }

        static string GetGuessFromUser(int triesLeft, StringBuilder incorrectLetters, string currentWord, char[] correctLetters)
        {
            bool validGuess = false;
            string guess = "";

            while (!validGuess)
            {
                Console.Write("Please enter a guess: ");
                guess = Console.ReadLine();

                if (StringContainsOnlyLetters(guess))
                {
                    validGuess = true;
                }
                else
                {
                    Console.Clear();
                    DisplayGuessInfo(triesLeft, incorrectLetters, currentWord, correctLetters);
                    Console.WriteLine("Can only guess letters or whole word, try again!\n");
                }
            }

            return guess.ToUpper();
        }

        static void DisplayHiddenWord(string currentWord, char[] correctLetters)
        {
            Console.WriteLine(GetHiddenWord(currentWord, correctLetters));
        }

        static string GetHiddenWord(string currentWord, char[] correctLetters)
        {
            string hiddenWord = "";

            for (int i = 0; i < currentWord.Length; i++)
            {
                if (Array.IndexOf(correctLetters, currentWord[i]) > -1)
                {
                    hiddenWord += $"{currentWord[i]} "; // Adds the current letter if it exists in correct guessed letters
                }
                else
                {
                    hiddenWord += "_ ";
                }
            }

            return hiddenWord;
        }

        static void DisplayIncorrectGuesses(StringBuilder incorrectLetters)
        {
            Console.WriteLine($"Incorrect guesses: {incorrectLetters.ToString()}");
        }

        static void DisplayTriesLeft(int triesLeft)
        {
            Console.WriteLine($"Tries left: {triesLeft}");
        }

        static bool IsGuessInWord(string guess, string currentWord)
        {
            return currentWord.Contains(guess);
        }

        static bool IsGuessInWord(char guess, string currentWord)
        {
            return currentWord.Contains(guess);
        }

        static bool GameIsWon(string currentWord, char[] correctLetters)
        {
            string hiddenWord = GetHiddenWord(currentWord, correctLetters);

            return !hiddenWord.Contains("_");
        }

        static bool StringContainsOnlyLetters(string stringToValidate)
        {
            Regex onlyLetters = new Regex("^[a-zA-Z ]+$");

            if (onlyLetters.IsMatch(stringToValidate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static string GetRandomWordFromArray(string[] words)
        { 
            Random rand = new Random();
            int index = rand.Next(words.Length); // Get random index within the size of the array

            return words[index];
        }
    }
}
