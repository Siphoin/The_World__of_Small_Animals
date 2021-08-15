using UnityEngine;

[CreateAssetMenu(menuName = "Data/Arrow Teleport/Arrow Teleport Settings", order = 0)]
    public class ArrowTeleportSettings : ScriptableObject
    {

        [Header("Спрайт при наведении на стрелку")]
        [SerializeField]
        private Sprite _activeMouseEnterSprite;

        [Header("Спрайт при обычном состоянии")]
        [SerializeField]
        private Sprite _idleSprite;

        [Header("Задержка перед телепортом")]
        [SerializeField]
        private float _timeOutTeleport = 3;

        public Sprite ActiveMouseEnterSprite =>  _activeMouseEnterSprite; 
        public Sprite IdleSprite  => _idleSprite; 
        public float TimeOutTeleport  => _timeOutTeleport; 
}