using UnityEngine;

// A component for animations that need to play sounds
public class AnimationSoundEffect : MonoBehaviour
{
    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("You need to have an audio manager in your scene.");
            return;
        }

        AudioManager.Instance.PlaySoundEffect(soundEffect);
    }
}
