using System;

namespace Countdown
{
    public class GameResult
    {
        // When the game result was recorded
        public DateTime Timestamp { get; set; }

        // Name of Player 1
        public string Player1Name { get; set; }

        // Score of Player 1
        public int Player1Score { get; set; }

        // Name of Player 2
        public string Player2Name { get; set; }

        // Score of Player 2
        public int Player2Score { get; set; }
    }
}
