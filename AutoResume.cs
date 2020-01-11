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
        PlayerCameraEffectController _cameraEffectController;

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

            _cameraEffectController = FindObjectOfType<PlayerCameraEffectController>();

            Invoke("Resume", 0.5f);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name == "SolarSystem") {
                _isSolarSystemLoaded = true;
            }
        }

        void LateUpdate() {
            // Skip wake up animation.
            if (!isOpenEyesSkipped && _isSolarSystemLoaded) {
                _cameraEffectController.OpenEyes(0, true);
                _cameraEffectController.SetValue("_wakeLength", 0f);
                _cameraEffectController.SetValue("_waitForWakeInput", false);

                LateInitializerManager.pauseOnInitialization = false;
                Locator.GetPauseCommandListener().RemovePauseCommandLock();
                Locator.GetPromptManager().RemoveScreenPrompt(_cameraEffectController.GetValue<ScreenPrompt>("_wakePrompt"));
                OWTime.Unpause(OWTime.PauseType.Sleeping);
                _cameraEffectController.Invoke("WakeUp");

                isOpenEyesSkipped = true;
            }
        }

        void Resume() {
            // Simulate "resume game" button press.
            ExecuteEvents.Execute(GameObject.Find("Button-ResumeGame"), null, ExecuteEvents.submitHandler);
        }
    }
}
