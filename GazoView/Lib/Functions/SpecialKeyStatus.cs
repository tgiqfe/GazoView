using System.Windows.Input;

namespace GazoView.Lib.Functions
{
    internal class SpecialKeyStatus
    {
        public static bool IsCtrlPressed()
        {
            return
                Keyboard.GetKeyStates(Key.LeftCtrl).HasFlag(KeyStates.Down) ||
                Keyboard.GetKeyStates(Key.RightCtrl).HasFlag(KeyStates.Down);
        }

        public static bool IsShiftPressed()
        {
            return
                Keyboard.GetKeyStates(Key.LeftShift).HasFlag(KeyStates.Down) ||
                Keyboard.GetKeyStates(Key.RightShift).HasFlag(KeyStates.Down);
        }

        public static bool IsAltPressed()
        {
            return
                Keyboard.GetKeyStates(Key.LeftAlt).HasFlag(KeyStates.Down) ||
                Keyboard.GetKeyStates(Key.RightAlt).HasFlag(KeyStates.Down);
        }
    }
}
