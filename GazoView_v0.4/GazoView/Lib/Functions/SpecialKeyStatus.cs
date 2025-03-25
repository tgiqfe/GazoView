using System.Windows.Input;

namespace GazoView.Lib.Functions
{
    internal class SpecialKeyStatus
    {
        public static bool IsCtrlPressed()
        {
            return
                (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down;
        }

        public static bool IsShiftPressed()
        {
            return
                (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down;
        }
    }
}
