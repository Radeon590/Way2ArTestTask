using UnityEngine;

namespace DefaultNamespace
{
    public class AnimatableModel : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator => animator;
    }
}