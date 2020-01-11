using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace OWML.AutoResume
{
    public class AutoResume : ModBehaviour
    {
        bool _done = false;
        bool _rightScene = false;
        void Start() {
            FindObjectOfType<TitleScreenAnimation>().SetValue("_fadeDuration", 0);
            FindObjectOfType<TitleScreenAnimation>().SetValue("_gamepadSplash", false);
            FindObjectOfType<TitleScreenAnimation>().SetValue("_introPan", false);
            FindObjectOfType<TitleScreenManager>().Invoke("FadeInTitleLogo");


            FindObjectOfType<TitleAnimationController>().SetValue("_logoFadeDelay", 0.001f);
            FindObjectOfType<TitleAnimationController>().SetValue("_logoFadeDuration", 0.001f);
            FindObjectOfType<TitleAnimationController>().SetValue("_optionsFadeDelay", 0.001f);
            FindObjectOfType<TitleAnimationController>().SetValue("_optionsFadeDuration", 0.001f);
            FindObjectOfType<TitleAnimationController>().SetValue("_optionsFadeSpacing", 0.001f);

            Invoke("Resume", 0.5f);

            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            _rightScene = true;
        }

        void LateUpdate() {
            if (!_done && _rightScene) {
                _done = true;
                ModHelper.Console.WriteLine("late");
                FindObjectOfType<PlayerCameraEffectController>().OpenEyes(0, true);
                FindObjectOfType<PlayerCameraEffectController>().SetValue("_wakeLength", 0f);
                FindObjectOfType<PlayerCameraEffectController>().SetValue("_waitForWakeInput", false);

                LateInitializerManager.pauseOnInitialization = false;
                Locator.GetPauseCommandListener().RemovePauseCommandLock();
                Locator.GetPromptManager().RemoveScreenPrompt(FindObjectOfType<PlayerCameraEffectController>().GetValue<ScreenPrompt>("_wakePrompt"));
                OWTime.Unpause(OWTime.PauseType.Sleeping);
                FindObjectOfType<PlayerCameraEffectController>().Invoke("WakeUp");
            }
        }

        void Resume() {

            ExecuteEvents.Execute(GameObject.Find("Button-ResumeGame"), null, ExecuteEvents.submitHandler);
        }
    }
}
