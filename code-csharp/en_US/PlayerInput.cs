using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public KeyCode Key_Up = KeyCode.UpArrow;

    public KeyCode Key_Down = KeyCode.DownArrow;

    public KeyCode Key_Left = KeyCode.LeftArrow;

    public KeyCode Key_Right = KeyCode.RightArrow;

    public KeyCode Key_Confirm = KeyCode.Z;

    public KeyCode Key_Cancel = KeyCode.X;

    public KeyCode Key_Menu = KeyCode.C;

    public KeyCode Key_Pause = KeyCode.Escape;

    public KeyCode Key_Sprint = KeyCode.LeftShift;

    public static Vector2 MovementInput;

    private Dictionary<KeyCode, bool> keyStates = new Dictionary<KeyCode, bool>();

    private static PlayerInput instance;

    public static PlayerInput Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(instance);
        LoadKeybinding();
    }

    private void Update()
    {
        CacheKeyState(Key_Up);
        CacheKeyState(Key_Down);
        CacheKeyState(Key_Left);
        CacheKeyState(Key_Right);
        CacheKeyState(Key_Sprint);
        MovementInput = GetMovementInput();
    }

    private void CacheKeyState(KeyCode key)
    {
        keyStates[key] = Input.GetKey(key);
    }

    public bool IsKeyHeld(KeyCode key)
    {
        bool value;
        return keyStates.TryGetValue(key, out value) && value;
    }

    public static void LoadKeybinding()
    {
        instance.Key_Up = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Up", 273);
        instance.Key_Left = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Left", 276);
        instance.Key_Right = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Right", 275);
        instance.Key_Down = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Down", 274);
        instance.Key_Confirm = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Confirm", 122);
        instance.Key_Cancel = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Cancel", 120);
        instance.Key_Menu = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Menu", 99);
        instance.Key_Sprint = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Sprint", 304);
        instance.Key_Pause = (KeyCode)PlayerPrefs.GetInt("Setting_Key_Pause", 27);
    }

    public static void RevertToDefaults()
    {
        instance.Key_Up = KeyCode.UpArrow;
        instance.Key_Left = KeyCode.LeftArrow;
        instance.Key_Right = KeyCode.RightArrow;
        instance.Key_Down = KeyCode.DownArrow;
        instance.Key_Confirm = KeyCode.Z;
        instance.Key_Cancel = KeyCode.X;
        instance.Key_Menu = KeyCode.C;
        instance.Key_Sprint = KeyCode.LeftShift;
        instance.Key_Pause = KeyCode.Escape;
        PlayerPrefs.SetInt("Setting_Key_Up", (int)instance.Key_Up);
        PlayerPrefs.SetInt("Setting_Key_Left", (int)instance.Key_Left);
        PlayerPrefs.SetInt("Setting_Key_Right", (int)instance.Key_Right);
        PlayerPrefs.SetInt("Setting_Key_Down", (int)instance.Key_Down);
        PlayerPrefs.SetInt("Setting_Key_Confirm", (int)instance.Key_Confirm);
        PlayerPrefs.SetInt("Setting_Key_Cancel", (int)instance.Key_Cancel);
        PlayerPrefs.SetInt("Setting_Key_Menu", (int)instance.Key_Menu);
        PlayerPrefs.SetInt("Setting_Key_Sprint", (int)instance.Key_Sprint);
        PlayerPrefs.SetInt("Setting_Key_Pause", (int)instance.Key_Pause);
        PlayerPrefs.Save();
    }

    public static bool HoldingAnyImportantKeys()
    {
        KeyCode[] array = new KeyCode[9] { instance.Key_Up, instance.Key_Down, instance.Key_Left, instance.Key_Right, instance.Key_Confirm, instance.Key_Cancel, instance.Key_Menu, instance.Key_Sprint, instance.Key_Pause };
        for (int i = 0; i < array.Length; i++)
        {
            if (Input.GetKey(array[i]))
            {
                return true;
            }
        }
        return false;
    }

    public static float GetHorizontalInput()
    {
        bool flag = Instance.IsKeyHeld(Instance.Key_Left);
        bool flag2 = Instance.IsKeyHeld(Instance.Key_Right);
        if (flag && !flag2)
        {
            return -1f;
        }
        if (flag2 && !flag)
        {
            return 1f;
        }
        return 0f;
    }

    public static float GetVerticalInput()
    {
        bool flag = Instance.IsKeyHeld(Instance.Key_Up);
        bool flag2 = Instance.IsKeyHeld(Instance.Key_Down);
        if (flag && !flag2)
        {
            return 1f;
        }
        if (flag2 && !flag)
        {
            return -1f;
        }
        return 0f;
    }

    public static Vector2 GetMovementInput()
    {
        bool num = Instance.IsKeyHeld(Instance.Key_Left);
        bool flag = Instance.IsKeyHeld(Instance.Key_Right);
        bool flag2 = Instance.IsKeyHeld(Instance.Key_Up);
        bool flag3 = Instance.IsKeyHeld(Instance.Key_Down);
        float num2 = 0f;
        float num3 = 0f;
        if (num)
        {
            num2 -= 1f;
        }
        if (flag)
        {
            num2 += 1f;
        }
        if (flag2)
        {
            num3 += 1f;
        }
        if (flag3)
        {
            num3 -= 1f;
        }
        return new Vector2(num2, num3).normalized;
    }
}
