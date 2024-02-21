using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGolf.ConfigClasses
{
    internal class StateTracker
    {

        /// <summary>
        /// Track which of the players are active as opposed to finished <br/>
        /// Players which index are true are still playing
        /// </summary>
        private bool[] _playersActive;

        /// <summary>
        /// Track which player's turn it is <br/>
        /// </summary>
        private int _currentPlayer;

        /// <summary>
        /// Array which stores the current score for the users. <br/>
        /// Player one's score is stored in index 0. player 2 at index 1 ...
        /// </summary>
        private int[] _score;

        /// <summary>
        /// Track if it is the first turn for this game
        /// </summary>
        private bool _isFirst;

        /// <summary>
        /// Variable which stores the users current  score
        /// </summary>
        public int[] Score
        {
            get { return _score; }
            set { _score = value; }
        }

        /// <summary>
        /// Track which player's turn it is
        /// </summary>
        public int CurrentPlayer
        {
            get { return _currentPlayer; }
            set { _currentPlayer = value; }
        }

        public bool[] PlayersActive
        {
            get { return _playersActive; }
            set { _playersActive = value; }
        }

        public bool isFirst
        {
            get { return _isFirst; }
            set { _isFirst = value; }
        }

        public void nextTurn()
        {
            // Increment to the next turn skipping player's who have already finished
            _currentPlayer = (_currentPlayer + 1) % _playersActive.Length;
            bool checkActive = _playersActive[_currentPlayer];
            while (!checkActive)
            {
                _currentPlayer = (_currentPlayer + 1) % _playersActive.Length;
                checkActive = _playersActive[_currentPlayer];
            }

            //  Set that this isn't the first turn
            isFirst = false;
        }

        /// <summary>
        /// Initialize the tracker
        /// </summary>
        public StateTracker()
        {
            _score = new int[4];
            _playersActive = new bool[4];

            // Initialize the arrays
            for(int i=0; i < 4; i++)
            {
                _score[i] = 0;
                _playersActive[i] = false;  
            }
            _playersActive[0] = true;

            _currentPlayer = 0;

            _isFirst = true;

        }
    }
}
