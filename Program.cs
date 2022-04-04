using System;
using System.IO;
using System.Reflection;
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

            // Move provided (or your own) words.txt file to the Desktop (or change path below)!
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"words.txt"); ;
            string[] words = GetWordsFromTextFile(filePath);

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
            DisplayHangMan(triesLeft);
            Console.WriteLine();
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
            // Only remove a try if letter hasn't been used before
            if (!incorrectLetters.ToString().Contains(guess))
            {
                triesLeft--;
                incorrectLetters.Append(guess[0] + " ");
                Console.WriteLine("Letter was incorrect!");
            }
        }

        static void CorrectLetter(ref char[] correctLetters, ref int amtCorrectLetters, string guess)
        {
            // Only add to correct letters if hasn't been added before
            if (!ExistsInCharArray(correctLetters, guess))
            {
                correctLetters[amtCorrectLetters++] = guess[0]; // Add the guessed letter as char after last char in array
                Console.WriteLine($"You guessed a correct letter ({guess})!");
                Console.WriteLine(correctLetters.Length);
            }
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

        static void DisplayHangMan(int triesLeft)
        {
            switch (triesLeft)
            {
                case 10:
                    Console.WriteLine("\n\n\n\n\n\n");
                    break;

                case 9:
                    Console.WriteLine("\n\n\n\n\n\n_____________");
                    break;

                case 8:
                    Console.WriteLine("\n  |\n  |\n  |\n  |\n  |\n__|__________");
                    break;

                case 7:
                    Console.WriteLine("   _______\n  | /\n  |/\n  |\n  |\n  |\n__|__________");
                    break;

                case 6:
                    Console.WriteLine("   _______\n  | /     |\n  |/\n  |\n  |\n  |\n__|__________");
                    break;

                case 5:
                    Console.WriteLine("   _______\n  | /     |\n  |/     (_)\n  |\n  |\n  |\n__|__________");
                    break;

                case 4:
                    Console.WriteLine("   _______\n  | /     |\n  |/     (_)\n  |       |\n  |       |\n  |\n__|__________");
                    break;

                case 3:
                    Console.WriteLine("   _______\n  | /     |\n  |/     (_)\n  |      \\|\n  |       |\n  |      \n__|__________");
                    break;

                case 2:
                    Console.WriteLine("   _______\n  | /     |\n  |/     (_)\n  |      \\|/\n  |       |\n  |      \n__|__________");
                    break;

                case 1:
                    Console.WriteLine("   _______\n  | /     |\n  |/     (_)\n  |      \\|/\n  |       |\n  |      / \n__|__________");
                    break;

                case 0:
                    Console.WriteLine("   _______\n  | /     |\n  |/     (_)\n  |      \\|/\n  |       |\n  |      / \\\n__|__________");
                    break;
            }
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

        static string[] GetWordsFromTextFile(string filePath)
        {
            try
            {
                File.Exists(filePath);

                StreamReader sr = new StreamReader(filePath);
                string readFile = sr.ReadToEnd();
                sr.Close();

                string[] words = readFile.Split(',');

                return words;
            }
            catch (IOException)
            {
                Console.WriteLine("Error! Problem reading specified filepath.");
            }

            return null;
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

        static bool ExistsInCharArray(char[] array, string s)
        {
            if (new string(array).Contains(s))
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
