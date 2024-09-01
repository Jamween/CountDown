using System;
using System.Linq;
using System.Timers;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Countdown
{
    public partial class MainPage : ContentPage
    {
        private string wordEntered = string.Empty; // Stores the word entered by the player
        private char[] letters = new char[9]; // Array to hold the 9 letters for the game
        private char[] player1Letters = new char[9]; // Array to store Player 1's letters

        private System.Timers.Timer timer; // Timer to manage countdown
        private int totalTime; // Total time left for the round
        private int i = 0; // Index for letter placement
        private int player1Points = 0; // Points for Player 1
        private int player2Points = 0; // Points for Player 2
        private int playerTurn = 1; // 1 for Player 1, 2 for Player 2
        private int roundNum = 1; // Current round number

        private char[] vowelList; // List of available vowels
        private char[] consonantList; // List of available consonants

        public MainPage()
        {
            InitializeComponent();
            ResetLetterLists(); // Initialize vowel and consonant lists
            scoreboard(); // Update the scoreboard with initial values
        }

        // Initializes the lists of vowels and consonants
        private void ResetLetterLists()
        {
            vowelList = new char[] { 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'I', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U' };
            consonantList = new char[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
        }

        // Manages the dictionary of valid words
        public class DictionaryManager
        {
            private HashSet<string> _wordSet; // Set of valid words

            public DictionaryManager()
            {
                LoadDictionary(); // Load words into the set
            }

            private void LoadDictionary()
            {
                _wordSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Initialize with case-insensitive comparison
                try
                {
                    var filePath = Path.Combine(FileSystem.AppDataDirectory, "WordList.txt"); // Path to the word list file
                    if (File.Exists(filePath))
                    {
                        var lines = File.ReadAllLines(filePath); // Read all lines from the file
                        foreach (var line in lines)
                        {
                            _wordSet.Add(line.Trim()); // Add each word to the set
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., file read errors)
                }
            }

            public bool IsWordValid(string word)
            {
                return _wordSet.Contains(word.Trim()); // Check if the word is in the set
            }
        }

        // Handles the Submit button click
        private void ClickedSubmit(object sender, EventArgs e)
        {
            wordEntered = EnterWord.Text; // Get the word entered by the player

            if (string.IsNullOrEmpty(wordEntered))
            {
                DisplayAlert("null answer", "Please enter a word", "Okay"); // Alert if no word is entered
                return;
            }

            letterCheck(); // Check if the word is valid

            if (playerTurn == 1)
            {
                playerTurn = 2; // Switch to Player 2's turn
                displayPlayersTurn(); // Update the display to show Player 2's turn

                if (roundNum == 1)
                {
                    // For the first round, Player 2 uses the same letters as Player 1
                    clearGrid(); // Clear the grid

                    // Reset the grid for Player 2 using Player 1's letters
                    for (int j = 0; j < player1Letters.Length; j++)
                    {
                        if (player1Letters[j] != '\0')
                        {
                            AddLetterToGrid(player1Letters[j], j); // Add Player 1's letters to the grid
                            letters[j] = player1Letters[j]; // Save letters for Player 2
                        }
                    }
                }

                EnterWord.Text = ""; // Clear the text entry
            }
            else
            {
                // If it was Player 2's turn, the round is over
                clearGrid(); // Clear the grid
                newRound.IsVisible = true; // Show button to start a new round
                stopGame(); // Disable input until the next round starts
            }
        }

        // Clears the grid and resets letter index
        private void clearGrid()
        {
            CountDownGrid.Children.Clear(); // Clear all children from the grid
            EnterWord.Text = ""; // Clear the text entry
            i = 0; // Reset the letter index
        }

        // Handles giving a vowel to the player
        public void GiveVowel(object sender, EventArgs e)
        {
            if (i >= 9)
            {
                DisplayAlert("Limit reached", "Only 9 letters can be chosen", "Okay"); // Alert if 9 letters have been chosen
                return;
            }

            if (vowelList.Length == 0)
            {
                DisplayAlert("No more vowels available", "Please try again later.", "Okay"); // Alert if no vowels are left
                return;
            }

            char vowel = vowelList[Random.Shared.Next(vowelList.Length)]; // Randomly select a vowel
            letters[i] = vowel; // Add the vowel to the letters array
            player1Letters[i] = vowel; // Store for Player 1

            AddLetterToGrid(vowel, i); // Add the vowel to the grid
            i++; // Increment the letter index

            if (i == 9)
            {
                play.IsVisible = true; // Show play button if 9 letters are chosen
            }

            vowelList = vowelList.Where(v => v != vowel).ToArray(); // Remove the chosen vowel from the list
        }

        // Handles giving a consonant to the player
        public void GiveConsonant(object sender, EventArgs e)
        {
            if (i >= 9)
            {
                DisplayAlert("Limit reached", "Only 9 letters can be chosen", "Okay"); // Alert if 9 letters have been chosen
                return;
            }

            if (consonantList.Length == 0)
            {
                DisplayAlert("No more consonants", "No more consonants available", "Okay"); // Alert if no consonants are left
                return;
            }

            char consonant = consonantList[Random.Shared.Next(consonantList.Length)]; // Randomly select a consonant
            letters[i] = consonant; // Add the consonant to the letters array
            player1Letters[i] = consonant; // Store for Player 1

            AddLetterToGrid(consonant, i); // Add the consonant to the grid
            i++; // Increment the letter index

            if (i == 9)
            {
                play.IsVisible = true; // Show play button if 9 letters are chosen
            }

            consonantList = consonantList.Where(c => c != consonant).ToArray(); // Remove the chosen consonant from the list
        }

        // Adds a letter to the grid at the specified index
        private void AddLetterToGrid(char letter, int index)
        {
            Label label = new Label
            {
                Text = letter.ToString(), // Set the text of the label to the letter
                FontSize = 75, // Set the font size
                HorizontalOptions = LayoutOptions.Center, // Center horizontally
                VerticalOptions = LayoutOptions.Center // Center vertically
            };

            CountDownGrid.Children.Add(label); // Add the label to the grid
            Grid.SetRow(label, 0); // Set the row position
            Grid.SetColumn(label, index); // Set the column position
        }

        // Handles the Play button click
        private void playBtn(object sender, EventArgs e)
        {
            playGame(); // Start the game
            startTimer(); // Start the timer
        }

        // Starts the game
        private void playGame()
        {
            playerTurn = 1; // Start with Player 1
            i = 0; // Reset letter index
            newRound.IsVisible = false; // Hide new round button
            CountDownGrid.IsVisible = true; // Show the grid
            answerLayout.IsVisible = true; // Show answer layout
            play.IsVisible = false; // Hide play button
            timerLbl.IsVisible = true; // Show timer label
            showTurn.IsVisible = true; // Show current turn label
            rounds.IsVisible = true; // Show round label
            displayPlayersTurn(); // Display which player's turn it is
            startTimer(); // Start the timer
        }

        // Displays the current player's turn
        private void displayPlayersTurn()
        {
            showTurn.Text = playerTurn == 1 ? $"{Players.name1}'s Turn" : $"{Players.name2}'s Turn";
        }

        // Stops the game and disables input
        private void stopGame()
        {
            EnterWord.IsEnabled = false; // Disable word entry
            SubmitEntry.IsEnabled = false; // Disable submit button
            play.IsVisible = true; // Show play button for a new round
        }

        // Enables input and starts a new game
        private void startGame()
        {
            EnterWord.IsEnabled = true; // Enable word entry
            SubmitEntry.IsEnabled = true; // Enable submit button
        }

        // Updates the scoreboard with the current points
        private void scoreboard()
        {
            lbl1.Text = $"{Players.name1}: {player1Points}"; // Update Player 1's score
            lbl2.Text = $"{Players.name2}: {player2Points}"; // Update Player 2's score
        }

        // Checks if the entered word is valid and updates scores
        private void letterCheck()
        {
            string wordEntry = wordEntered.ToUpper(); // Convert entered word to uppercase
            string lettersSaved = new string(letters); // Convert letters array to string
            int wordLength = 0; // Length of the valid word

            foreach (char c in wordEntry)
            {
                if (lettersSaved.Contains(c))
                {
                    wordLength++; // Increase the length if the letter is in the saved letters
                }
            }

            if (wordLength == wordEntry.Length)
            {
                int points = wordEntry.Length; // Points based on word length
                if (playerTurn == 1)
                {
                    player1Points += points; // Add points to Player 1
                }
                else
                {
                    player2Points += points; // Add points to Player 2
                }
                DisplayAlert("Good Job", "The word is correct", "Okay"); // Alert for correct word
            }
            else
            {
                DisplayAlert("Incorrect", "The word is incorrect", "Okay"); // Alert for incorrect word
            }
            scoreboard(); // Update the scoreboard
            clearGrid(); // Clear the grid for the next round
        }

        // Starts a new round or ends the game if all rounds are complete
        private void startRound(object sender, EventArgs e)
        {
            if (roundNum > 6) // Check if the game has ended
            {
                EndGame(); // Call the method to end the game and declare the winner
                return;
            }

            roundNum++; // Increment round number
            rounds.Text = $"Round: {roundNum}"; // Update round label

            i = 0; // Reset letter index
            CountDownGrid.Children.Clear(); // Clear the grid
            EnterWord.IsEnabled = true; // Enable word entry
            SubmitEntry.IsEnabled = true; // Enable submit button

            if (roundNum == 1 && playerTurn == 2)
            {
                // For the first round, Player 2 uses the same letters as Player 1
                for (int j = 0; j < player1Letters.Length; j++)
                {
                    if (player1Letters[j] != '\0')
                    {
                        AddLetterToGrid(player1Letters[j], j); // Add Player 1's letters to the grid
                        letters[j] = player1Letters[j]; // Save letters for Player 2
                        i++;
                    }
                }
            }
            else
            {
                // Reset letters array for a new round
                Array.Clear(letters, 0, letters.Length);
                Array.Clear(player1Letters, 0, player1Letters.Length); // Clear Player 1's letters
                play.IsVisible = true; // Allow new letter selection
                Vowel.IsEnabled = true; // Enable the Vowel button
                Consonant.IsEnabled = true; // Enable the Consonant button
            }

            // Reset vowel and consonant lists for the new round
            ResetLetterLists();

            // Toggle player turn
            playerTurn = (playerTurn == 1) ? 2 : 1;
            displayPlayersTurn(); // Display the current player's turn
            newRound.IsVisible = false; // Hide new round button
            CountDownGrid.IsVisible = true; // Show the grid
            answerLayout.IsVisible = true; // Show answer layout
            play.IsVisible = false; // Hide play button
            timerLbl.IsVisible = true; // Show timer label

            // Restart the timer
            startTimer();
        }

        // Saves the game result to the history
        private void SaveGameResult()
        {
            var gameResult = new GameResult
            {
                Timestamp = DateTime.Now, // Record the current timestamp
                Player1Name = Players.name1,
                Player1Score = player1Points,
                Player2Name = Players.name2,
                Player2Score = player2Points
            };

            var historyManager = new GameHistoryManager();
            historyManager.AddGameResult(gameResult); // Add the game result to the history
        }

        // Ends the game and displays the results
        private void EndGame()
        {
            stopGame(); // Stop the game

            string winner;
            if (player1Points > player2Points)
            {
                winner = Players.name1; // Player 1 wins
            }
            else if (player2Points > player1Points)
            {
                winner = Players.name2; // Player 2 wins
            }
            else
            {
                winner = "No one, it's a tie!"; // Tie
            }

            DisplayAlert("Game Over", $"{winner} wins! \n{Players.name1}: {player1Points} points \n{Players.name2}: {player2Points} points", "New Game"); // Display winner

            SaveGameResult(); // Save the game result

            // Offer to start a new game
            var startNewGame = DisplayAlert("New Game", "Would you like to start a new game?", "Yes", "No").Result;
            if (startNewGame)
            {
                ResetGame(); // Reset and start a new game
            }
        }

        // Resets the game state and starts a new game
        private void ResetGame()
        {
            player1Points = 0; // Reset Player 1's points
            player2Points = 0; // Reset Player 2's points
            roundNum = 0; // Reset round number
            scoreboard(); // Update the scoreboard
            startRound(this, EventArgs.Empty); // Start a new game by starting the first round
        }

        // Starts or restarts the timer
        private void startTimer()
        {
            if (timer != null)
            {
                timer.Stop(); // Stop the previous timer if it exists
                timer.Dispose(); // Dispose of the previous timer
            }

            timer = new System.Timers.Timer(1000); // Create a new timer with a 1-second interval
            timer.Elapsed += TimerElapsed; // Attach the elapsed event handler
            timer.AutoReset = true; // Reset the timer automatically
            timer.Start(); // Start the timer
            totalTime = 30; // Set timer duration to 30 seconds

            Device.BeginInvokeOnMainThread(() =>
            {
                timerLbl.Text = $"Time: {totalTime}"; // Update the timer label
            });
        }

        // Handles timer elapsed events
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            totalTime--; // Decrement total time
            Device.BeginInvokeOnMainThread(() =>
            {
                timerLbl.Text = $"Time: {totalTime}"; // Update the timer label
                if (totalTime <= 0)
                {
                    timer.Stop(); // Stop the timer when time is up
                    timerLbl.Text = "Time's up!"; // Display time's up message
                    stopGame(); // Stop the game
                    newRound.IsVisible = true; // Show the button to start the next round
                }
            });
        }
    }
}
