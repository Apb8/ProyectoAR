using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    // Referencia al Animator
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Método para activar/desactivar el estado de habla
    public void SetTalkingState(bool IsTalking)
    {
        if (!IsAnyEmotionActive())
        {
            animator.SetBool("IsTalking", IsTalking);
        }
    }

    // Método para el estado Refuse
    public void SetRefuseState(bool IsRefusing)
    {
        SetEmotionState("IsRefusing", IsRefusing);
    }

    // Método para el estado Question
    public void SetQuestionState(bool IsQuestion)
    {
        SetEmotionState("IsQuestioning", IsQuestion);
    }

    // Método para el estado Happy
    public void SetHappyState(bool IsHappy)
    {
        SetEmotionState("IsHappy", IsHappy);
    }

    // Método privado para manejar estados emocionales
    private void SetEmotionState(string parameter, bool state)
    {
        // Si activamos una emoción, desactivamos las demás
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

    // Comprueba si alguna emoción está activa
    private bool IsAnyEmotionActive()
    {
        return animator.GetBool("IsRefusing") ||
               animator.GetBool("IsQuestioning") ||
               animator.GetBool("IsHappy");
    }

    // Método para resetear todos los estados (volver a Idle)
    public void ResetAllStates()
    {
        ResetAllEmotions();
        animator.SetBool("IsTalking", false);
    }
}