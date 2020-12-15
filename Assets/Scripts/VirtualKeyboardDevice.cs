using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[InputControlLayout(displayName = "Virtual Keyboard", stateType = typeof(KeyboardState))]
public class VirtualKeyboardDevice : InputDevice, IInputUpdateCallbackReceiver
{
    private InputDevice actualKeyboard;
    private KeyboardState keyboardState;

    static VirtualKeyboardDevice()
    {
        InputSystem.RegisterLayout<VirtualKeyboardDevice>();
    }

    protected override void FinishSetup()
    {
        base.FinishSetup();

        actualKeyboard = InputSystem.GetDevice("Keyboard");
        keyboardState = new KeyboardState();
    }

    public void OnUpdate()
    {
        actualKeyboard.CopyState<KeyboardState>(out keyboardState);
        InputSystem.QueueStateEvent(this, keyboardState);
    }
}
