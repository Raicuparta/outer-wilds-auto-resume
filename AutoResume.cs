using OWML.ModHelper;
using OWML.ModHelper.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OWML.AutoResume
{
    public class AutoResume : ModBehaviour
    {
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
        }

        void Resume() {
            ExecuteEvents.Execute(GameObject.Find("Button-ResumeGame"), null, ExecuteEvents.submitHandler);
        }
    }
}
