using System;
using Discord;
using UnityEngine;

public class Discord_Controller : MonoBehaviour
{
    private global::Discord.Discord discord;

    public static Discord_Controller instance;

    private bool isDiscordInitialized;

    private long startTimeUnix = DateTimeOffset.Now.AddHours(-666.0).ToUnixTimeSeconds();

    private void Start()
    {
        instance = this;
        InitializeDiscord();
        if (isDiscordInitialized)
        {
            ChangeActivity();
        }
    }

    private void InitializeDiscord()
    {
        try
        {
            discord = new global::Discord.Discord(1228410356700025012L, 1uL);
            isDiscordInitialized = true;
            Debug.Log("Discord initialized successfully.");
        }
        catch (ResultException ex)
        {
            Debug.LogWarning("Failed to initialize Discord: " + ex.Message);
            Debug.LogWarning("Common reasons are either, discord not being open or no internet connection.");
            isDiscordInitialized = false;
        }
    }

    private void OnDisable()
    {
        if (discord != null)
        {
            discord.Dispose();
            Debug.Log("Discord disposed.");
        }
    }

    public void ChangeActivity(ActivityType Type = ActivityType.Playing, string Name = "", string State = "", string Details = "* WE ARE BUSY EXPERIMENTING.", string LargeImage = "hypotheticalicon_default", string LargeText = "DELTARUNE - Hypothesis", string VesselText = "* (A vessel, it looks content.)")
    {
        if (!isDiscordInitialized)
        {
            Debug.LogWarning("Discord is not initialized, skipping ChangeActivity.");
            return;
        }
        ActivityManager activityManager = discord.GetActivityManager();
        Activity activity = default(Activity);
        activity = ((!Application.isEditor || DRHDebugManager.instance.DebugModeEnabled) ? new Activity
        {
            Type = Type,
            Name = Name,
            State = State,
            Details = Details,
            Assets =
            {
                LargeImage = LargeImage,
                LargeText = LargeText,
                SmallImage = "soul",
                SmallText = VesselText
            },
            Timestamps =
            {
                Start = startTimeUnix
            }
        } : new Activity
        {
            Type = Type,
            Name = Name,
            State = "[Hypoaster is hard at work]",
            Details = "* LET ME WRITE MORE HYPOTHESES!!",
            Assets =
            {
                LargeImage = "hypotheticalicon_default",
                LargeText = "DELTARUNE - Hypothesis",
                SmallImage = "unity",
                SmallText = "In unity editor, no leaks! tee-hee!"
            },
            Timestamps =
            {
                Start = startTimeUnix
            }
        });
        activityManager.UpdateActivity(activity, delegate
        {
            Debug.Log("Activity Updated!");
        });
    }

    private void Update()
    {
        if (!isDiscordInitialized || discord == null)
        {
            return;
        }
        try
        {
            discord.RunCallbacks();
        }
        catch (ResultException ex)
        {
            Debug.LogWarning("Discord.RunCallbacks() failed: " + ex.Message);
            Debug.LogWarning("Disabling Discord integration to prevent further issues.");
            isDiscordInitialized = false;
            if (discord != null)
            {
                discord.Dispose();
                discord = null;
            }
        }
    }
}
