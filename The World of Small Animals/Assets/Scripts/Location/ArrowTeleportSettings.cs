using UnityEngine;

[CreateAssetMenu(menuName = "Data/Arrow Teleport/Arrow Teleport Settings", order = 0)]
    public class ArrowTeleportSettings : ScriptableObject
    {

        [Header("Спрайт при наведении на стрелку")]
        [SerializeField]
        private Sprite activeMouseEnterSprite;

        [Header("Спрайт при обычном состоянии")]
        [SerializeField]
        private Sprite idleSprite;

    [Header("Задержка перед телепортом")]
    [SerializeField]
    private float timeOutTeleport = 3;

    public Sprite ActiveMouseEnterSprite { get => activeMouseEnterSprite; }
        public Sprite IdleSprite { get => idleSprite; }
    public float TimeOutTeleport { get => timeOutTeleport; }
}