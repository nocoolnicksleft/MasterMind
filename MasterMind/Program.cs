using System;
using System.Text;
using System.Drawing;
using Console = Colorful.Console;

namespace MasterMind
{




    public class Draw
    {
        public static char Get8BitChar(byte p_asciinum)
        {
            return Encoding.GetEncoding(437).GetChars(new byte[] { p_asciinum })[0];
        }

        public static void drawrect(Color color, int x, int y, int width, int height)
        {

            Color oldcolor = Console.BackgroundColor;

            Console.BackgroundColor = color;
            for (int iy = y; iy < (y + height); iy++)
            {
                for (int ix = x; ix < (x + width); ix++)
                {
                    Console.SetCursorPosition(ix, iy);
                    Console.Write(" ");
                }
            }

            Console.BackgroundColor = oldcolor;
        }

    }


    class Program
    {


        public static int numberofpegs = 4;
        public static int numberofguesses = 10;
        public static string nameofplayer = "";

        static void DrawHelp(int x, int y)
        {
            Console.ForegroundColor = Color.Black;
            Console.BackgroundColor = Color.Gray;
            Draw.drawrect(Color.Gray, x, y, 24, 22);
            Console.SetCursorPosition(x + 2, y + 1);
            Console.WriteLine("<1> Black" );
            Console.SetCursorPosition(x + 2, y + 2);
            Console.WriteLine("<2> White" );
            Console.SetCursorPosition(x + 2, y + 3);
            Console.WriteLine("<3> Red"   );
            Console.SetCursorPosition(x + 2, y + 4);
            Console.WriteLine("<4> Green" );
            Console.SetCursorPosition(x + 2, y + 5);
            Console.WriteLine("<5> Blue"  );
            Console.SetCursorPosition(x + 2, y + 6);
            Console.WriteLine("<6> Yellow");
            Console.SetCursorPosition(x + 2, y + 7);
            Console.WriteLine("<7> Orange");
            Console.SetCursorPosition(x + 2, y + 8);
            Console.WriteLine("<8> Brown");
            Console.SetCursorPosition(x + 2, y + 10);
            Console.WriteLine("<Enter> Go");
            Console.SetCursorPosition(x + 2, y + 12);
            Console.WriteLine("<F1> Reveal Secret");
            Console.SetCursorPosition(x + 2, y + 13);
            Console.WriteLine("<F2> Hide Secret");
            Console.SetCursorPosition(x + 2, y + 15);
            Console.WriteLine("<F4> New Game 4 Pegs");
            Console.SetCursorPosition(x + 2, y + 16);
            Console.WriteLine("<F5> New Game 5 Pegs");
            Console.SetCursorPosition(x + 2, y + 18);
            Console.WriteLine("<Esc> End");
        }

        static void Main(string[] args)
        {

            int currentpeg;
            PegCombination _editorCombination;
            Game _mygame;
            bool abort = false;

            const int topOffset = 5;


            void DrawCurrentGuessPeg()
            {
                Draw.drawrect(PegCombination.GetColor(_editorCombination._color[currentpeg]), 4 + (currentpeg * 3), topOffset + 24 - _mygame._currentguess * 2, 2, 1);
            }

            void DrawCombination(PegCombination p_combination, int x, int y)
            {
                // Console.SetCursorPosition(19 + (0 * 2), 24 - _mygame._currentguess * 2);
                Console.BackgroundColor = Color.Gray;
                Console.SetCursorPosition(x, y);
                for (int i = 0; i < p_combination._color.Length; i++)
                {
                    if (p_combination._color[i] == PegColors.Unassigned)
                    {
                        Console.ForegroundColor = Color.Red;
                        Console.Write("XX ");
                    }
                    else
                    {
                        Console.ForegroundColor = PegCombination.GetColor(p_combination._color[i]);
                        Console.Write(string.Format("{0}{0} ", Draw.Get8BitChar(219)), PegCombination.GetColor(p_combination._color[i]));
                    }
                }
            }

            void DrawGuessResult(Guess p_guess, int x, int y)
            {
                //Console.SetCursorPosition(19 + (0 * 2), 24 - _mygame._currentguess * 2);
                Console.BackgroundColor = Color.Gray;
                Console.SetCursorPosition(x, y);
                for (int i = 0; i < p_guess._positionMatches; i++)
                {
                    Console.ForegroundColor = Color.White;
                    Console.Write(Draw.Get8BitChar(220) + " ");
                }

                for (int i = 0; i < p_guess._colorMatches; i++)
                {
                    Console.ForegroundColor = Color.Black;
                    Console.Write(Draw.Get8BitChar(220) + " ");
                }

            }

            void CheckPlausibilities()
            {
                if (_editorCombination.IsValid)
                {
                    for (int i = 0; i < _mygame._currentguess; i++)
                    {
                        Console.SetCursorPosition(37, topOffset + 24 - i * 2);
                        Console.BackgroundColor = Color.Gray;

                        if (_mygame._guess[i].IsPlausibleFor(_editorCombination))
                        {
                            Console.ForegroundColor = Color.Green;
                            Console.Write("OK  ");
                        }
                        else
                        {
                            Console.ForegroundColor = Color.Red;
                            Console.Write("FAIL");
                        }
                    }
                }

            }

            void RemovePegIndicator()
            {

                Console.BackgroundColor = Color.Gray;
                Console.ForegroundColor = Color.Black;

                Draw.drawrect(Color.Gray, 3, topOffset + 24 - (_mygame._currentguess * 2) + 1, 13, 1);
            }

            void IndicatePeg()
            {
                RemovePegIndicator();

                Console.BackgroundColor = Color.Gray;
                Console.ForegroundColor = Color.Black;

                Console.SetCursorPosition(4 + currentpeg * 3, topOffset + 24 - (_mygame._currentguess * 2) + 1);
                Console.Write("^");
            }

            void IndicateGuess()
            {
                Draw.drawrect(Color.Black, 0, topOffset + 0, 3, 26);

                Console.SetCursorPosition(0, topOffset + 24 - (_mygame._currentguess * 2));
                Console.BackgroundColor = Color.Black;
                Console.ForegroundColor = Color.White;
                Console.Write(">");
            }

            void NextPeg()
            {
                if (currentpeg == (numberofpegs - 1)) currentpeg = 0;
                else currentpeg++;
                IndicatePeg();
            }

            void PrevPeg()
            {
                if (currentpeg == 0) currentpeg = (numberofpegs - 1);
                else currentpeg--;
                IndicatePeg();
            }

            void ClearPlausibility()
            {
                Draw.drawrect(Color.Gray, 34, topOffset + 4, 10, 22);
            }

            void HideSecret()
            {
                Draw.drawrect(Color.Gray, 3, topOffset + 0, 16, 3);
            }

            void RevealSecret()
            {
                DrawCombination(_mygame._secret, 4, topOffset + 1);
            }

            void Init()
            {
                Console.Title = "MasterMind Trainer 1.0";
                  
                Console.SetWindowSize(78, 40);
                Console.SetBufferSize(78, 40);
                Console.BackgroundColor = Color.Black;
                Console.Clear();

                Console.SetCursorPosition(0, 0);
                Console.WriteAscii("MASTERMIND", Color.Red);

                

                // Secret bar
                Draw.drawrect(Color.Gray, 3, topOffset + 0, 16, 3);

                // Guess indicator lane
                Draw.drawrect(Color.Black, 0, topOffset + 4, 3, 26);

                // Guess lane
                Draw.drawrect(Color.Gray, 3, topOffset + 4, 16, 22);

                // Answer lane
                Draw.drawrect(Color.Gray, 21, topOffset + 4, 11, 22);

                // Hint lane
                Draw.drawrect(Color.Gray, 34, topOffset + 4, 10, 22);

                // Help lane
                DrawHelp(46, topOffset + 4);

                _editorCombination = new PegCombination(numberofpegs);
                _mygame = new Game(numberofpegs, numberofguesses, false);

                currentpeg = 0;

                _editorCombination.Reset();

                // DrawCombination(_mygame._secret, 4, 1);
                DrawCombination(_editorCombination, 4, topOffset + 24 - _mygame._currentguess * 2);

                IndicateGuess();
                IndicatePeg();
            }

            Init();


            while (!abort)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.F1:
                        RevealSecret();
                        break;
                    case ConsoleKey.F2:
                        HideSecret();
                        break;
                    case ConsoleKey.F4:
                        numberofpegs = 4;
                        Init();
                        break;
                    case ConsoleKey.F5:
                        numberofpegs = 5;
                        Init();
                        break;
                    case ConsoleKey.LeftArrow:
                        PrevPeg();
                        break;
                    case ConsoleKey.RightArrow:
                        NextPeg();
                        break;
                    case ConsoleKey.D1:
                        _editorCombination.SetPeg(currentpeg, PegColors.Black);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.D2:
                        _editorCombination.SetPeg(currentpeg, PegColors.White);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.D3:
                        _editorCombination.SetPeg(currentpeg, PegColors.Red);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.D4:
                        _editorCombination.SetPeg(currentpeg, PegColors.Green);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.D5:
                        _editorCombination.SetPeg(currentpeg, PegColors.Blue);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.D6:
                        _editorCombination.SetPeg(currentpeg, PegColors.Yellow);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.D7:
                        _editorCombination.SetPeg(currentpeg, PegColors.Orange);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.D8:
                        _editorCombination.SetPeg(currentpeg, PegColors.Brown);
                        DrawCurrentGuessPeg();
                        CheckPlausibilities();
                        NextPeg();
                        break;
                    case ConsoleKey.Enter:
                        if (_mygame._secret == _editorCombination)
                        {
                            RevealSecret();

                            Console.SetCursorPosition(0, topOffset + 27);
                            Console.ForegroundColor = Color.Red;
                            Console.BackgroundColor = Color.Black;
                            Console.WriteLine(" You're a winner!");

                            if (nameofplayer == "")
                            {
                                Console.Write(" Whats your name? ");
                                string name = Console.ReadLine();
                                if (name.Trim() != "")
                                {
                                    nameofplayer = name.Trim();
                                }
                            }

                            Console.WriteLine("Press <Enter> to play again.");
                            Console.ReadLine();
                            Init();
                        }
                        else
                        {
                            if (_editorCombination.IsValid)
                            {
                                RemovePegIndicator();
                                DrawGuessResult(_mygame.MakeGuess(_editorCombination), 22, topOffset + 24 - (_mygame._currentguess - 1) * 2);

                                ClearPlausibility();

                                if (_mygame._currentguess < _mygame._numberofguesses)
                                {

                                    currentpeg = 0;
                                    IndicateGuess();
                                    IndicatePeg();

                                    _editorCombination.Reset();
                                    DrawCombination(_editorCombination, 4, topOffset + 24 - _mygame._currentguess * 2);
                                } else
                                {
                                    RevealSecret();
                                    Console.SetCursorPosition(0, topOffset + 27);
                                    Console.ForegroundColor = Color.Red;
                                    Console.BackgroundColor = Color.Black;
                                    Console.Write(" You're a loser.... Press <Enter> to try again.");
                                    Console.ReadLine();
                                    Init();
                                    
                                }
                            }
                        }
                        break;
                }

                if ((key.Key == ConsoleKey.C) && (key.Modifiers == ConsoleModifiers.Control)) abort = true;
            }

            Console.ReadLine();
        }
    }
}
