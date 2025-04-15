using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    // Referencia al Animator
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // M�todo para activar/desactivar el estado de habla
    public void SetTalkingState(bool IsTalking)
    {
        if (!IsAnyEmotionActive())
        {
            animator.SetBool("IsTalking", IsTalking);
        }
    }

    // M�todo para el estado Refuse
    public void SetRefuseState(bool IsRefusing)
    {
        SetEmotionState("IsRefusing", IsRefusing);
    }

    // M�todo para el estado Question
    public void SetQuestionState(bool IsQuestion)
    {
        SetEmotionState("IsQuestioning", IsQuestion);
    }

    // M�todo para el estado Happy
    public void SetHappyState(bool IsHappy)
    {
        SetEmotionState("IsHappy", IsHappy);
    }

    // M�todo privado para manejar estados emocionales
    private void SetEmotionState(string parameter, bool state)
    {
        // Si activamos una emoci�n, desactivamos las dem�s
        if (state)
        {
            ResetAllEmotions();
            animator.SetBool("IsTalking", false);
        }

        animator.SetBool(parameter, state);
    }

    // Reinicia todos los estados emocionales
    private void ResetAllEmotions()
    {
        animator.SetBool("IsRefusing", false);
        animator.SetBool("IsQuestioning", false);
        animator.SetBool("IsHappy", false);
    }

    // Comprueba si alguna emoci�n est� activa
    private bool IsAnyEmotionActive()
    {
        return animator.GetBool("IsRefusing") ||
               animator.GetBool("IsQuestioning") ||
               animator.GetBool("IsHappy");
    }

    // M�todo para resetear todos los estados (volver a Idle)
    public void ResetAllStates()
    {
        ResetAllEmotions();
        animator.SetBool("IsTalking", false);
    }
}