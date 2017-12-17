using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MasterMind
{

    public struct PegCombination
    {
        int _numberofpegs;

        public PegColors[] _color;

        public PegCombination(PegCombination p_combination)
        {
            _numberofpegs = p_combination._numberofpegs;
            _color = new PegColors[_numberofpegs];
            for (int i = 0; i < _numberofpegs; i++)
            {
                _color[i] = p_combination._color[i];
            }
        }

        public PegCombination(int p_numberOfPegs)
        {
            _numberofpegs = p_numberOfPegs;
            _color = new PegColors[_numberofpegs];
            for (int i = 0; i < _numberofpegs; i++)
            {
                _color[i] = PegColors.Unassigned;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < _numberofpegs; i++)
            {
                _color[i] = PegColors.Unassigned;
            }

        }

        public bool IsValid
        {
            get
            {
                return (!this.ContainsColor(PegColors.Unassigned));
            }
        }

        public void GenerateRandom(bool p_allowMultiple)
        {
            Random rnd = new Random();

            int count = Enum.GetValues(typeof(PegColors)).Length - 1;

            for (int i = 0; i < _numberofpegs; i++)
            {
                //_color[i] = (PegColors)rnd.Next(count)+1;
                PegColors nextcolor = (PegColors)rnd.Next(count) + 1;

                while (!p_allowMultiple && this.ContainsColor(nextcolor))
                {
                    nextcolor = (PegColors)rnd.Next(count) + 1;
                }

                _color[i] = nextcolor;

            }

        }

        public PegCompareResult[] CompareTo(PegCombination p_otherCombination)
        {
            PegCombination otherCombination = new PegCombination(p_otherCombination);

            PegCompareResult[] result = new PegCompareResult[_numberofpegs];

            for (int i = 0; i < _numberofpegs; i++)
            {
                if (this._color[i] == otherCombination._color[i])
                {
                    result[i] = PegCompareResult.MatchColorAndPosition;
                    otherCombination._color[i] = PegColors.Unassigned;
                }
                else result[i] = PegCompareResult.NoMatch;
            }

            for (int i = 0; i < _numberofpegs; i++)
            {
                    if (otherCombination.ContainsColor(this._color[i]))
                    {
                        result[i] = PegCompareResult.MatchColor;
                        otherCombination.RemoveFirstOfColor(this._color[i]);
                    }
            }

            return result;
        }

        public void RemoveFirstOfColor(PegColors p_color)
        {
            for (int i = 0; i < _numberofpegs; i++)
            {
                if (_color[i] == p_color)
                {
                    _color[i] = PegColors.Unassigned;
                }
            }
        }

        public bool ContainsColor(PegColors p_color)
        {
            for (int i = 0; i < _numberofpegs; i++)
            {
                if (_color[i] == p_color) return true;
            }
            return false;
        }

        public bool IsEqual(PegCombination othercombination)
        {
            return (this == othercombination);
        }

        public static bool operator ==(PegCombination a, PegCombination b)
        {
            if (a._numberofpegs != b._numberofpegs)
            {
                throw new Exception("Peg number mismatch");
            }
            for (int i = 0; i < a._numberofpegs; i++)
            {
                if (a._color[i] != b._color[i]) return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator !=(PegCombination a, PegCombination b)
        {
            if (a._numberofpegs != b._numberofpegs)
            {
                throw new Exception("Peg number mismatch");
            }
            for (int i = 0; i < a._numberofpegs; i++)
            {
                if (a._color[i] == b._color[i]) return false;
            }
            return true;
        }

        public void SetPeg(int pegno, PegColors color)
        {
            _color[pegno] = color;
        }

        static public Color GetColor(PegColors c)
        {
            switch (c)
            {
                case PegColors.Black:
                    return Color.Black;
                case PegColors.White:
                    return Color.White;
                case PegColors.Red:
                    return Color.Red;
                case PegColors.Green:
                    return Color.LimeGreen;
                case PegColors.Blue:
                    return Color.Blue;
                case PegColors.Yellow:
                    return Color.Yellow;
                case PegColors.Orange:
                    return Color.Orange;
                case PegColors.Brown:
                    return Color.Brown;
            }
            return Color.Pink;
        }

        static public ConsoleColor GetConsoleColor(PegColors c)
        {
            ConsoleColor result = ConsoleColor.Black;

            switch (c)
            {
                case PegColors.Black:
                    result = ConsoleColor.DarkGray;
                    break;
                case PegColors.White:
                    result = ConsoleColor.White;
                    break;
                case PegColors.Red:
                    result = ConsoleColor.Red;
                    break;
                case PegColors.Green:
                    result = ConsoleColor.Green;
                    break;
                case PegColors.Blue:
                    result = ConsoleColor.DarkBlue;
                    break;
                case PegColors.Yellow:
                    result = ConsoleColor.Yellow;
                    break;
                case PegColors.Orange:
                    result = ConsoleColor.Magenta;
                    break;
                case PegColors.Brown:
                    result = ConsoleColor.DarkRed;
                    break;
            }

            return result;
        }

    }
}
