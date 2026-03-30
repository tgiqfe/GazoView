using System.Windows.Input;

namespace GazoView.Lib.Functions
{
    internal class SpecialKeyStatus
    {
        /// <summary>
        /// if Ctrl key is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool IsCtrPressed()
        {
            return
                (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down;
        }

        /// <summary>
        /// if Shift key is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool IsShiftPressed()
        {
            return
                (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down;
        }
    }
}
