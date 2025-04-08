using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SecurePlayerPrefs : MonoBehaviour
{
    private static readonly string secretKey = "Hello, Have some respect and don't spoil the gambling experience. it's impossible to have fair gambling now-a-days. because of nosey people like you. please keep all of this between us, if you post this online i wont have any more gambling. no one will be impressed, it will be your fault!";

    public static bool HasDetectedFiltyHacker = false;

    public static void SetSecureInt(string key, int value, bool disableTotalCashDisplay = false)
    {
        int @int = PlayerPrefs.GetInt(key, 0);
        if (!disableTotalCashDisplay && GonerMenu.Instance != null && key == "TotalCash" && value > @int)
        {
            GonerMenu.Instance.ShowCashGain(value - @int);
        }
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.SetString(key + "_hash", GenerateHash(value));
    }

    public static int GetSecureInt(string key, int defaultValue = 0)
    {
        int @int = PlayerPrefs.GetInt(key, defaultValue);
        string @string = PlayerPrefs.GetString(key + "_hash", string.Empty);
        string text = GenerateHash(@int);
        if (PlayerPrefs.HasKey(key) && text != @string)
        {
            HasDetectedFiltyHacker = true;
        }
        int num = ((@string == text) ? @int : defaultValue);
        SetSecureInt("TotalCash", num);
        return num;
    }

    private static string GenerateHash(int value)
    {
        using SHA256 sHA = SHA256.Create();
        string s = value + secretKey;
        return BitConverter.ToString(sHA.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace("-", "").ToLowerInvariant();
    }
}
