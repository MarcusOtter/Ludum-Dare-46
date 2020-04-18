using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    [RequireComponent(typeof(Animator))]
    public class SceneTransitioner : MonoBehaviour
    {
        internal static SceneTransitioner Instance { get; private set; }

        [SerializeField] private string _leavingSceneTriggerName = "LeavingScene";
        [SerializeField] private string _enteringSceneTriggerName = "EnteringScene";

        private int _leavingSceneTriggerHash;
        private int _enteringSceneTriggerHash;

        private Animator _animator;

        private IEnumerator _activeLoadingSequence;

        private void Awake()
        {
            if (!IsAloneSingleton()) { return; }

            _animator = GetComponent<Animator>();

            _leavingSceneTriggerHash = Animator.StringToHash(_leavingSceneTriggerName);
            _enteringSceneTriggerHash = Animator.StringToHash(_enteringSceneTriggerName);
        }

        internal void LoadNextScene()
        {
            ChangeSceneTo(SceneManager.GetActiveScene().buildIndex + 1);
        }

        internal void ReloadScene()
        {
            ChangeSceneTo(SceneManager.GetActiveScene().buildIndex);
        }

        private void ChangeSceneTo(int buildIndex)
        {
            if (_activeLoadingSequence != null) { return; }

            // Start new coroutine
            _activeLoadingSequence = LoadScene(buildIndex);
            StartCoroutine(_activeLoadingSequence);
        }

        private IEnumerator LoadScene(int buildIndex)
        {
            _animator.SetTrigger(_leavingSceneTriggerHash);

            StartCoroutine(FadeOutAudio(1f));
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(buildIndex);
            yield return new WaitForSeconds(1f);

            _animator.SetTrigger(_enteringSceneTriggerHash);
            StartCoroutine(FadeInAudio(1f));
            _activeLoadingSequence = null;
        }

        // TODO: Use duration
        private IEnumerator FadeOutAudio(float duration)
        {
            var audioManager = AudioManager.Instance;
            if (audioManager == null) { yield break; }

            while (audioManager.MainVolume > 0)
            {
                audioManager.SetVolume(main: audioManager.MainVolume - 0.04f);
                yield return new WaitForSeconds(0.02f);
            }

            audioManager.SetVolume(main: 0);
        }

        // TODO: Use duration
        private IEnumerator FadeInAudio(float duration)
        {
            var audioManager = AudioManager.Instance;
            if (audioManager == null) { yield break; }

            while (audioManager.MainVolume < 1)
            {
                audioManager.SetVolume(main: audioManager.MainVolume + 0.04f);
                yield return new WaitForSeconds(0.02f);
            }

            audioManager.SetVolume(main: 0);
        }

        private bool IsAloneSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(transform.root.gameObject);
                return true;
            }

            Debug.LogWarning($"Deleting duplicate singleton \"{name}\". Don't put singletons in your scene - use the singleton spawner!");
            Destroy(gameObject);
            return false;
        }
    }
}