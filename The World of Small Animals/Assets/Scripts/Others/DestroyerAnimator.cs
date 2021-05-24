using UnityEngine;
public class DestroyerAnimator : MonoBehaviour
{

    private Animator animator;
    private void Awake()
    {
        if (!TryGetComponent(out animator))
        {
            throw new DestroyerAnimatorException($"{name} not have component Animator");
        }
    }
    public void RemoveAnimator()
    {
        Destroy(animator);
        Destroy(this);
    }

    public void RemoveAnimator(float time)
    {
        Destroy(animator, time);
        Destroy(this, time + 0.02f);
    }
}