using UnityEngine;

public class RouletteBallAudio : MonoBehaviour
{
   [SerializeField] private AudioSource audioSource;
   [SerializeField] private AudioClip ballRollClip;
   [SerializeField] private AudioClip ballStopClip;

   public void PlayRollSound()
   {
      if (audioSource.clip != ballRollClip)
      {
         audioSource.clip = ballRollClip;
         audioSource.loop = true;
         audioSource.Play();
      }
   }
   public void StopRollSound()
   {
      if (audioSource.isPlaying)
      {
         audioSource.loop = false;
         audioSource.clip = ballStopClip;
         audioSource.Play();
      }
   }
}
