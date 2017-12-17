using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind
{
    public class Guess
    {
        public PegCombination _combination;
        public int _colorMatches = 0;
        public int _positionMatches = 0;
        PegCompareResult[] _result;

        public Guess(PegCombination p_combination)
        {
            _combination = new PegCombination(p_combination._color.Length);
            for (int i = 0; i < _combination._color.Length; i++)
            {
                _combination._color[i] = p_combination._color[i];
            }
            _colorMatches = 0;
            _positionMatches = 0;
        }

        public void Compare(PegCombination p_combination)
        {
            _result = _combination.CompareTo(p_combination);

            _colorMatches = 0;
            _positionMatches = 0;

            for (int i = 0; i < _result.Length; i++)
            {
                switch (_result[i])
                {
                    case PegCompareResult.MatchColor:
                        _colorMatches++;
                        break;

                    case PegCompareResult.MatchColorAndPosition:
                        _positionMatches++;
                        break;
                }
            }
        }

        public bool IsPlausibleFor(PegCombination p_combination)
        {
            int colorMatchesLeft = this._colorMatches;
            int positionMatchesLeft = this._positionMatches;
            for (int i = 0; i < _result.Length; i++)
            {
                if (_combination.ContainsColor(p_combination._color[i]))
                {
                    if (_combination._color[i] == p_combination._color[i])
                    {
                        if (positionMatchesLeft > 0) positionMatchesLeft--;
                        else return false;
                    }
                    else
                    {
                        if (colorMatchesLeft > 0) colorMatchesLeft--;
                        else return false;
                    }
                }
            }
            if (colorMatchesLeft > 0) return false;
            if (positionMatchesLeft > 0) return false;
            return true;
        }
    }


}
