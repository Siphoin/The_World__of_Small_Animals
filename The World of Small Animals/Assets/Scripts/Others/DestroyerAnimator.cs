using UnityEngine;
public class DestroyerAnimator : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        if (!TryGetComponent(out _animator))
        {
            throw new DestroyerAnimatorException($"{name} not have component Animator");
        }
    }
    public void RemoveAnimator()
    {
        Destroy(_animator);
        Destroy(this);
    }

    public void RemoveAnimator(float time)
    {
        Destroy(_animator, time);
        Destroy(this, time + 0.02f);
    }
}