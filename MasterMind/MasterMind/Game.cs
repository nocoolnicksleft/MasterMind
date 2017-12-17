using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind
{
    class Game
    {
        public int _numberofpegs = 4;
        public int _numberofguesses = 10;
        public bool _allowMultiple = false;

        public PegCombination _secret;
        public Guess[] _guess;

        public int _currentguess = 0;

        public Game(int p_numberOfPegs, int p_numberOfGuesses, bool p_allowMultiple)
        {
            _numberofpegs = p_numberOfPegs;
            _numberofguesses = p_numberOfGuesses;
            _allowMultiple = p_allowMultiple;
            _currentguess = 0;
            _secret = new PegCombination(_numberofpegs);
            _secret.GenerateRandom(_allowMultiple);
            _guess = new Guess[_numberofguesses];
        }

        public Guess MakeGuess(PegCombination p_combination)
        {
            _guess[_currentguess] = new Guess(p_combination);
            _guess[_currentguess].Compare(_secret);
            _currentguess++;
            return _guess[_currentguess - 1];
        }

    }


}
