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
        bool isOpenEyesSkipped = false;
        bool _isSolarSystemLoaded = false;

        void Start() {
            // Skip flash screen.
            var titleScreenAnimation = FindObjectOfType<TitleScreenAnimation>();
            titleScreenAnimation.SetValue("_fadeDuration", 0);
            titleScreenAnimation.SetValue("_gamepadSplash", false);
            titleScreenAnimation.SetValue("_introPan", false);
            titleScreenAnimation.Invoke("FadeInTitleLogo");

            // Skip menu fade.
            var titleAnimationController = FindObjectOfType<TitleAnimationController>();
            titleAnimationController.SetValue("_logoFadeDelay", 0.001f);
            titleAnimationController.SetValue("_logoFadeDuration", 0.001f);
            titleAnimationController.SetValue("_optionsFadeDelay", 0.001f);
            titleAnimationController.SetValue("_optionsFadeDuration", 0.001f);
            titleAnimationController.SetValue("_optionsFadeSpacing", 0.001f);

            // Couldn't figure out how to tell if the resume button was ready to be clicked
            // so I'm just waiting 0.5 seconds.
            Invoke("Resume", 0.5f);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name == "SolarSystem") {
                _isSolarSystemLoaded = true;
            }
        }

        void LateUpdate() {
            if (!isOpenEyesSkipped && _isSolarSystemLoaded) {
                isOpenEyesSkipped = true;


                // Skip wake up animation.
                var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>();
                cameraEffectController.OpenEyes(0, true);
                cameraEffectController.SetValue("_wakeLength", 0f);
                cameraEffectController.SetValue("_waitForWakeInput", false);

                // Skip wake up prompt.
                LateInitializerManager.pauseOnInitialization = false;
                Locator.GetPauseCommandListener().RemovePauseCommandLock();
                Locator.GetPromptManager().RemoveScreenPrompt(cameraEffectController.GetValue<ScreenPrompt>("_wakePrompt"));
                OWTime.Unpause(OWTime.PauseType.Sleeping);
                cameraEffectController.Invoke("WakeUp");

                // Enable all inputs immedeately.
                OWInput.ChangeInputMode(InputMode.Character);
                typeof(OWInput).SetValue("_inputFadeFraction", 0f);
                GlobalMessenger.FireEvent("TakeFirstFlashbackSnapshot");
            }
        }

        void Resume() {
            // Simulate "resume game" button press.
            ExecuteEvents.Execute(GameObject.Find("Button-ResumeGame"), null, ExecuteEvents.submitHandler);
        }
    }
}
