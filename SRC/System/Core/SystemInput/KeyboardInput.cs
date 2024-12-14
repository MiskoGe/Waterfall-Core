using Waterfall.System.Core.SystemInput.Keys;

namespace Waterfall.System.Core.SystemInput
{
    public class KeyboardInput
    {
        public string CurrentInput = "";
        public int CurrLine, CurrChar, MaxLength = int.MaxValue;
        public bool SpecialCharracters = true, AllowDots = true, AllowArrows = true, AllowUpDown = false, OnlyNums = false;
        public static bool AlreadyUsed;
        public void Monitor()
        {
            AlreadyUsed = true;
            KeyHandler.KeyboardAcceleration();
            bool enterChar = true;
            char charToEnter = ' ';
            while (KeyHandler.KeyAvailable)
            {
                KeyInfo key = KeyHandler.ReadKey();
                switch (key.Key)
                {
                    #region Main keys
                    case KeyboardKey.A:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'a';
                            break;
                        }
                        else
                        {
                            charToEnter = 'A';
                            break;
                        }
                    case KeyboardKey.B:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'b';
                            break;
                        }
                        else
                        {
                            charToEnter = 'B';
                            break;
                        }
                    case KeyboardKey.C:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'c';
                            break;
                        }
                        else
                        {
                            charToEnter = 'C';
                            break;
                        }
                    case KeyboardKey.D:
                        if (!key.CapsLock && !key.Shift && !key.Control)
                        {
                            charToEnter = 'd';
                            break;
                        }
                        else
                        {
                            charToEnter = 'D';
                            break;
                        }
                    case KeyboardKey.E:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'e';
                            break;
                        }
                        else
                        {
                            charToEnter = 'E';
                            break;
                        }
                    case KeyboardKey.F:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'f';
                            break;
                        }
                        else
                        {
                            charToEnter = 'F';
                            break;
                        }
                    case KeyboardKey.G:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'g';
                            break;
                        }
                        else
                        {
                            charToEnter = 'G';
                            break;
                        }
                    case KeyboardKey.H:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'h';
                            break;
                        }
                        else
                        {
                            charToEnter = 'H';
                            break;
                        }
                    case KeyboardKey.I:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'i';
                            break;
                        }
                        else
                        {
                            charToEnter = 'I';
                            break;
                        }
                    case KeyboardKey.J:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'j';
                            break;
                        }
                        else
                        {
                            charToEnter = 'J';
                            break;
                        }
                    case KeyboardKey.K:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'k';
                            break;
                        }
                        else
                        {
                            charToEnter = 'K';
                            break;
                        }
                    case KeyboardKey.L:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'l';
                            break;
                        }
                        else
                        {
                            charToEnter = 'L';
                            break;
                        }
                    case KeyboardKey.M:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'm';
                            break;
                        }
                        else
                        {
                            charToEnter = 'M';
                            break;
                        }
                    case KeyboardKey.N:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'n';
                            break;
                        }
                        else
                        {
                            charToEnter = 'N';
                            break;
                        }
                    case KeyboardKey.O:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'o';
                            break;
                        }
                        else
                        {
                            charToEnter = 'O';
                            break;
                        }
                    case KeyboardKey.P:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'p';
                            break;
                        }
                        else
                        {
                            charToEnter = 'P';
                            break;
                        }
                    case KeyboardKey.Q:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'q';
                            break;
                        }
                        else
                        {
                            charToEnter = 'Q';
                            break;
                        }
                    case KeyboardKey.R:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'r';
                            break;
                        }
                        else
                        {
                            charToEnter = 'R';
                            break;
                        }
                    case KeyboardKey.S:

                        if (!key.CapsLock && !key.Shift && !key.Control)
                        {
                            charToEnter = 's';

                        }
                        else if (!key.Control)
                        {
                            charToEnter = 'S';

                        }
                        else
                        {
                            enterChar = false;
                        }

                        break;
                    case KeyboardKey.T:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 't';
                            break;
                        }
                        else
                        {
                            charToEnter = 'T';
                            break;
                        }
                    case KeyboardKey.U:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'u';
                            break;
                        }
                        else
                        {
                            charToEnter = 'U';
                            break;
                        }
                    case KeyboardKey.V:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'v';
                            break;
                        }
                        else
                        {
                            charToEnter = 'V';
                            break;
                        }
                    case KeyboardKey.W:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'w';
                            break;
                        }
                        else
                        {
                            charToEnter = 'W';
                            break;
                        }
                    case KeyboardKey.X:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'x';
                            break;
                        }
                        else
                        {
                            charToEnter = 'X';
                            break;
                        }
                    case KeyboardKey.Y:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'y';
                            break;
                        }
                        else
                        {
                            charToEnter = 'Y';
                            break;
                        }
                    case KeyboardKey.Z:
                        if (!key.CapsLock && !key.Shift)
                        {
                            charToEnter = 'z';
                            break;
                        }
                        else
                        {
                            charToEnter = 'Z';
                            break;
                        }

                    case KeyboardKey.D0:

                        if (!key.Shift)
                        {
                            charToEnter = '0';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = ')';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D1:
                        if (!key.Shift)
                        {
                            charToEnter = '1';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '!';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D2:
                        if (!key.Shift)
                        {
                            charToEnter = '2';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '@';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D3:
                        if (!key.Shift)
                        {
                            charToEnter = '3';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '#';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D4:
                        if (!key.Shift)
                        {
                            charToEnter = '4';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '$';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D5:
                        if (!key.Shift)
                        {
                            charToEnter = '5';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '%';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D6:
                        if (!key.Shift)
                        {
                            charToEnter = '6';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '^';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D7:
                        if (!key.Shift)
                        {
                            charToEnter = '7';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '&';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D8:
                        if (!key.Shift)
                        {
                            charToEnter = '8';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '*';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.D9:
                        if (!key.Shift)
                        {
                            charToEnter = '9';
                        }
                        else if (SpecialCharracters)
                        {
                            charToEnter = '(';
                        }
                        else
                            enterChar = false;
                        break;
                    case KeyboardKey.Spacebar:
                        charToEnter = ' ';
                        break;
                    case KeyboardKey.OemPeriod:

                        if (AllowDots && SpecialCharracters)
                        {
                            if (!key.Shift)
                            {
                                if (AllowDots && SpecialCharracters)
                                    charToEnter = '.';
                                else
                                {
                                    enterChar = false;
                                    break;
                                }
                            }
                            else if (SpecialCharracters)
                            {
                                charToEnter = '>';
                            }
                            else
                            {
                                enterChar = false;
                                break;
                            }
                        }
                        break;
                    case KeyboardKey.OemComma:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }

                        if (!key.Shift)
                        {
                            charToEnter = ',';
                        }
                        else
                        {
                            charToEnter = '<';
                        }
                        break;
                    case KeyboardKey.Oem1:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = ';';
                        }
                        else
                        {
                            charToEnter = ':';
                        }
                        break;
                    case KeyboardKey.Oem2:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = '/';
                        }
                        else
                        {
                            charToEnter = '?';
                        }
                        break;
                    case KeyboardKey.Oem3:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = '`';
                        }
                        else
                        {
                            charToEnter = '~';
                        }
                        break;
                    case KeyboardKey.Oem4:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = '[';
                        }
                        else
                        {
                            charToEnter = '{';
                        }
                        break;
                    case KeyboardKey.Oem5:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = '\\';
                        }
                        else
                        {
                            charToEnter = '|';
                        }
                        break;
                    case KeyboardKey.Oem6:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = ']';
                        }
                        else
                        {
                            charToEnter = '}';
                        }
                        break;
                    case KeyboardKey.Oem7:

                        if (!key.Shift)
                        {
                            if (SpecialCharracters)
                                charToEnter = '\'';
                            else
                            {
                                enterChar = false;
                                break;
                            }
                        }
                        else
                        {
                            if (!SpecialCharracters)
                            {
                                enterChar = false;
                                break;
                            }
                            charToEnter = '"';
                        }
                        break;
                    case KeyboardKey.OemPlus:
                    case KeyboardKey.Plus:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = '=';
                        }
                        else
                        {
                            charToEnter = '+';
                        }
                        break;
                    case KeyboardKey.Minus:
                    case KeyboardKey.OemMinus:
                        if (!SpecialCharracters)
                        {
                            enterChar = false;
                            break;
                        }
                        if (!key.Shift)
                        {
                            charToEnter = '-';
                        }
                        else
                        {
                            charToEnter = '_';
                        }
                        break;
                    #endregion
                    default:
                        enterChar = false;
                        break;
                    case KeyboardKey.Backspace:
                        enterChar = false;
                        if (!AllowArrows)
                        {
                            if (CurrentInput.Length > 0)
                            {
                                CurrChar -= 1;
                                CurrentInput = CurrentInput.Remove(CurrentInput.Length - 1, 1);
                            }
                        }
                        else
                        {
                            if (!AllowUpDown)
                            {
                                if (CurrChar > 0)
                                {
                                    CurrChar -= 1;
                                    CurrentInput = CurrentInput.Remove(CurrChar, 1);
                                }
                            }
                        }

                        break;
                    case KeyboardKey.Enter:
                        enterChar = false;
                        HandleEnter();
                        break;
                    case KeyboardKey.RightArrow:
                        {
                            enterChar = false;
                            if (CurrentInput.Length > CurrChar)
                                CurrChar++;
                        }
                        break;
                    case KeyboardKey.LeftArrow:
                        {
                            enterChar = false;
                            if (CurrChar > 0)
                                CurrChar--;
                        }
                        break;
                    case KeyboardKey.UpArrow:
                        {
                            if (CurrLine > 0 && AllowUpDown)
                            {

                            }
                            enterChar = false;
                        }
                        break;
                    case KeyboardKey.DownArrow:
                        {
                            if (AllowUpDown)
                            {

                            }
                            enterChar = false;
                        }
                        break;
                    case KeyboardKey.Tab:
                        {

                        }
                        break;
                }
                if (enterChar)
                {
                    if (AllowArrows && CurrentInput.Length > 0)
                    {
                        if (OnlyNums)
                        {
                            if (charToEnter != '0' && charToEnter != '1' && charToEnter != '2' && charToEnter != '3' && charToEnter != '4' && charToEnter != '5' && charToEnter != '6' && charToEnter != '7' && charToEnter != '8' && charToEnter != '9')
                                return;
                        }
                        if (CurrentInput.Length > MaxLength)
                            return;
                        string prefix = CurrentInput.Substring(0, CurrChar);
                        string suffix = CurrentInput.Substring(CurrChar);
                        CurrentInput = prefix + charToEnter + suffix;
                    }
                    else
                    {
                        if (OnlyNums)
                        {
                            if (charToEnter != '0' && charToEnter != '1' && charToEnter != '2' && charToEnter != '3' && charToEnter != '4' && charToEnter != '5' && charToEnter != '6' && charToEnter != '7' && charToEnter != '8' && charToEnter != '9')
                                return;
                        }
                        CurrentInput += charToEnter.ToString();
                    }


                    if (AllowArrows)
                        CurrChar++;
                }
            }
        }
        public virtual void HandleEnter() { }
    }
}
