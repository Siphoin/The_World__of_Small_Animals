using UnityEngine;

public class DestroyerAnimator : MonoBehaviour
{
    private const float TIME_OUT_DESTROY_COMPONENT_ANIMATOR = 0.02f;

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

        Destroy(this, time + TIME_OUT_DESTROY_COMPONENT_ANIMATOR);
    }
}