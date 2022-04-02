using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Hangman
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                DisplayHiddenWord(currentWord);
                Guess(ref gameInProgress, currentWord, ref correctLetters, ref amtCorrectLetters, ref triesLeft, ref incorrectLetters);
            }
        }

        static void Guess(ref bool gameInProgress, string currentWord, ref char[] correctLetters, ref int amtCorrectLetters, ref int triesLeft, ref StringBuilder incorrectLetters)
        {
            string guess = GetGuessFromUser();

            if (IsGuessInWord(guess, currentWord))
            {
                if (guess.Length == 1)
                {
                    CorrectLetter(ref correctLetters, ref amtCorrectLetters, guess);

                    if (amtCorrectLetters == currentWord.Length)
                    {
                        Win(ref gameInProgress);
                    }
                }
                else if (guess.Length == currentWord.Length)
                {
                    Win(ref gameInProgress);
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
                Lose(ref gameInProgress);
            }
        }

        static void Win(ref bool gameInProgress)
        {
            gameInProgress = false;
            Console.WriteLine("Congrats, you've won!");
        }
        static void Lose(ref bool gameInProgress)
        {
            gameInProgress = false;
            Console.WriteLine("Shmucks, you've been hung and lost the game!");
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
