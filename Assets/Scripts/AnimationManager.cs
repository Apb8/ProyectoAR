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
    public void SetRefuseState(bool IsRefuse)
    {
        SetEmotionState("IsRefuse", IsRefuse);
    }

    // Método para el estado Question
    public void SetQuestionState(bool IsQuestion)
    {
        SetEmotionState("IsQuestion", IsQuestion);
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
        animator.SetBool("IsRefuse", false);
        animator.SetBool("IsQuestion", false);
        animator.SetBool("IsHappy", false);
    }

    // Comprueba si alguna emoción está activa
    private bool IsAnyEmotionActive()
    {
        return animator.GetBool("IsRefuse") ||
               animator.GetBool("IsQuestion") ||
               animator.GetBool("IsHappy");
    }

    // Método para resetear todos los estados (volver a Idle)
    public void ResetAllStates()
    {
        ResetAllEmotions();
        animator.SetBool("IsTalking", false);
    }
}