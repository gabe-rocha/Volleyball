using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data {

    public static float gameDuration = 30f;

    internal static readonly float DelayBeforeShowingFinalText = 4f;
    internal static readonly float DelayBeforeShowingScoredText = 1f;
    internal static readonly float DelayBeforeHidingScoredText = 2f;
    internal static readonly float DelayBeforeShowingFinalButtons = 5f;
    internal static readonly float fadeOutSpeed = 2f;

    internal static readonly Vector3 midTopOfTheScreen = new Vector3(0f, 4.21f, 0f);

    internal static readonly int MatchScoreTarget = 5;
    internal static readonly int CountdownValue = 2;
    internal static Player playerOne;
    internal static PlayerTwo playerTwo;
}