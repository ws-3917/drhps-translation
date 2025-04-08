using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "CHARACTER", menuName = "Deltaswap/ChatboxCharacter", order = 0)]
public class CHATBOXCHAR : ScriptableObject
{
    public enum TextStyleTypes
    {
        Default = 1867431062,
        Papyrus = 2127624236,
        Sans = 3053743
    }

    public Sprite CharacterIcon;

    public AudioClip CharacterSound;

    [Space(15f)]
    public float CharacterIconWidth = 231f;

    public float CharacterIconHeight = 182f;

    [Space(15f)]
    public bool CharacterHasTalkingAnimation;

    public Sprite CharacterTalkingIcon;

    public bool TellRecieverIfChatting;

    [Space(15f)]
    public TMP_FontAsset CharacterFont;

    public float CharacterFontSize = 64f;

    [Header("Edit monospace StyleSheet in font folder to add")]
    public TextStyleTypes TextStyleSheet = TextStyleTypes.Default;

    public bool GiveCharacterBulletpoint = true;

    [Header("1 = Default Speed | 2 = 50% slower | 0.5 = 50% faster")]
    public float TextSpeedMultiplier = 1f;

    public Color TextColor = Color.white;
}
