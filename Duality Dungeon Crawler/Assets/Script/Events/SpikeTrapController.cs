using UnityEngine;
using System.Collections;
using CaptainCoder.Core;
using CaptainCoder.Audio;

public class SpikeTrapController : MonoBehaviour
{

    public MutablePosition Position;
    public float Delay = 3f;
    public float Offset = 0f;
    public GameObject Spikes;
    public AudioSource PopOutAudio;
    public AudioSource RetractAudio;

    private Coroutine coroutine;

    public void Start()
    {
        PopOutAudio.volume = MusicController.Instance.SFXVolume;
        RetractAudio.volume = MusicController.Instance.SFXVolume;
        MusicController.Instance.OnSFXVolumeChange += (volume) =>
        {
            PopOutAudio.volume = volume;
            RetractAudio.volume = volume;
        };
    }

    public void OnEnable()
    {
        coroutine = StartCoroutine(HandleTrap());
    }

    public void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    

    private IEnumerator HandleTrap()
    {
        yield return new WaitForSeconds(Offset);
        while (true)
        {
            SpikesDown();
            yield return new WaitForSeconds(1f);
            SpikesUp();
            yield return new WaitForSeconds(Delay - 1f);
            

        }
    }

    private void SpikesUp()
    {
        Spikes.transform.localScale = new Vector3(1, 2, 1);
        PopOutAudio.Play();
        if (GameManager.Instance.Player.Effects.HasFlag(PlayerEffect.Float)) { return; }

        if (PlayerMovementController.Instance.Position == Position.Freeze())
        {
            MessageController.Display("Ouch!");
            GameManager.Instance.PlayerStats.Stat(DualStat.BodyMind).Value -= 20;
            GameManager.Instance.Hurt();
        }
    }

    private void SpikesDown()
    {
        Spikes.transform.localScale = new Vector3(1, .75f, 1);
        RetractAudio.Play();
    }

}