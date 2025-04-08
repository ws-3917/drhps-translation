using System;
using System.Collections;
using UnityEngine;

public class Trailer_Chaos_VentManager : MonoBehaviour
{
    public bool InVentAnimation;

    public bool SpinKris;

    public GameObject idleSmoke;

    [SerializeField]
    private float JumpArcHeight = 3f;

    [SerializeField]
    private float JumpDuration = 0.5f;

    private Vector2 lastPosition;

    [SerializeField]
    private Vector3 JumpStartPos;

    public Vector3 JumpEndPos;

    private float timeElapsed;

    private bool isMoving;

    private bool isVerticalVent;

    private ParticleSystem storedSmoke;

    public AudioSource source;

    public static Trailer_Chaos_VentManager instance;

    private Vector2 initialDirection;

    private void Awake()
    {
        instance = this;
        Trailer_Chaos_Vent[] array = UnityEngine.Object.FindObjectsByType<Trailer_Chaos_Vent>(FindObjectsSortMode.None);
        foreach (Trailer_Chaos_Vent trailer_Chaos_Vent in array)
        {
            UnityEngine.Object.Instantiate(idleSmoke).transform.position = trailer_Chaos_Vent.transform.position;
        }
    }

    private void Update()
    {
        ArcUpdate();
    }

    private void ArcUpdate()
    {
        if (isMoving)
        {
            timeElapsed += Time.deltaTime;
            float num = timeElapsed / JumpDuration;
            Vector2 vector = CalculateArcPosition(num);
            PlayerManager.Instance.transform.position = vector;
            RotatePlayerDuringArc(num);
            if (num >= 1f)
            {
                isMoving = false;
                InVentAnimation = false;
                timeElapsed = 0f;
                PlayerManager.Instance.transform.position = JumpEndPos;
                PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
                SpinKris = false;
                CameraManager.instance.FollowPlayerY = true;
                storedSmoke.Stop();
            }
            else if (isVerticalVent)
            {
                CameraManager.instance.FollowPlayerY = true;
            }
            else
            {
                CameraManager.instance.FollowPlayerY = false;
            }
        }
    }

    private void RotatePlayerDuringArc(float t)
    {
        float f = Mathf.Lerp(0f, 360f, t) * (MathF.PI / 180f);
        Vector2 rotation = new Vector2(initialDirection.x * Mathf.Cos(f) - initialDirection.y * Mathf.Sin(f), initialDirection.x * Mathf.Sin(f) + initialDirection.y * Mathf.Cos(f));
        PlayerManager.Instance._PMove.RotatePlayerAnim(rotation);
    }

    public IEnumerator StartVent(Vector3 startPos, Vector3 endPos, bool isVertical, ParticleSystem storedsmoke)
    {
        if (!InVentAnimation)
        {
            storedSmoke = storedsmoke;
            InVentAnimation = true;
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            initialDirection = PlayerManager.Instance._PMove.CurrentPlayerRotation;
            while (Vector3.Distance(PlayerManager.Instance.transform.position, startPos) > 0.1f)
            {
                yield return null;
                PlayerManager.Instance.transform.position = Vector3.Lerp(PlayerManager.Instance.transform.position, startPos, 15f * Time.deltaTime);
            }
            yield return new WaitForSeconds(0.25f);
            storedSmoke.Play();
            isVerticalVent = isVertical;
            source.Play();
            JumpStartPos = PlayerManager.Instance.transform.position;
            JumpEndPos = endPos;
            timeElapsed = 0f;
            SpinKris = true;
            isMoving = true;
        }
    }

    private Vector2 CalculateArcPosition(float t)
    {
        Vector2 vector = Vector2.Lerp(JumpStartPos, JumpEndPos, t);
        float num = JumpArcHeight * Mathf.Sin(MathF.PI * t);
        return new Vector2(vector.x, vector.y + num);
    }
}
