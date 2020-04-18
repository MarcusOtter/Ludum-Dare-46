using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public void LoadNextLevel()
        {
            if (SceneTransitioner.Instance == null)
            {
                Debug.LogError("You need to have a scene transitioner in your scene.");
                return;
            }

            SceneTransitioner.Instance.LoadNextScene();
        }

        public void SetSfxVolume(float newVolume)
        {
            if (AudioManager.Instance == null)
            {
                Debug.LogError("You need to have an audio manager in your scene.");
                return;
            }

            AudioManager.Instance.SetVolume(sfx: newVolume);
        }

        public void SetMusicVolume(float newVolume)
        {
            if (AudioManager.Instance == null)
            {
                Debug.LogError("You need to have an audio manager in your scene.");
                return;
            }

            AudioManager.Instance.SetVolume(music: newVolume);
        }

        public void SetAmbienceVolume(float newVolume)
        {
            if (AudioManager.Instance == null)
            {
                Debug.LogError("You need to have an audio manager in your scene.");
                return;
            }

            AudioManager.Instance.SetVolume(ambience: newVolume);
        }
    }
}
