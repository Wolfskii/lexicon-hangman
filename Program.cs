using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Hangman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] words = { "ice", "ball", "dimension" };
            StringBuilder incorrectLetters = new StringBuilder(10);
            string currentWord = GetRandomWordFromArray(words).ToUpper();

            char[] correctLetters = new char[currentWord.Length];
            int amtCorrectLetters = 0;
            int triesLeft = 10;

            Console.WriteLine(currentWord);
            DisplayHiddenWord(currentWord);

            string guess = GetGuessFromUser();

            if (IsGuessInWord(guess, currentWord))
            {
                if (guess.Length == 1)
                {
                    correctLetters[amtCorrectLetters++] = guess[0]; // Add the guessed letter as char after last char in array
                    Console.WriteLine("Arr: " + new string(correctLetters));

                    Console.WriteLine("You guessed a correct letter!");
                }
                else if (guess.Length == currentWord.Length)
                {
                    Console.WriteLine("Congrats, you've won!");
                }
                else
                {
                    triesLeft--;
                    Console.WriteLine("Word was incorrect!");
                }
            }
            else
            {
                if (guess.Length == 1)
                {
                    triesLeft--;
                    incorrectLetters.Append(guess[0] + " ");
                    Console.WriteLine("Letter was incorrect!");
                }
                else if (guess.Length > 1)
                {
                    triesLeft--;
                    Console.WriteLine("Word was incorrect!");
                }
            }
        }

        static string GetGuessFromUser()
        {
            bool validGuess = false;
            string guess = "";

            while (!validGuess)
            {
                Console.WriteLine("Please enter a guess:");
                guess = Console.ReadLine();

                if (StringContainsOnlyLetters(guess))
                {
                    validGuess = true;
                }
                else
                {
                    Console.WriteLine("Can only guess letters or whole word, try again!\n");
                }
            }

            return guess.ToUpper();
        }

        static void DisplayHiddenWord(string currentWord)
        {
            // TODO: Check with correctLetters first to show them in this output
            string hiddenWord = "";

            for (int i = 0; i < currentWord.Length; i++)
            {
                hiddenWord += "_ ";
            }

            Console.WriteLine(hiddenWord);
        }

        static bool IsGuessInWord(string guess, string currentWord)
        {
            return currentWord.Contains(guess);
        }

        static bool IsGuessInWord(char guess, string currentWord)
        {
            return currentWord.Contains(guess);
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
