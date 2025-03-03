using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ModelAnimator : MonoBehaviour
{
    [SerializeField] private ModelSpawnManager modelSpawnManager;
    [SerializeField] private GameObject animationDependentUiElements;
    [SerializeField] private Dropdown dropdown;

    private Animator _currentAnimator;

    void Start()
    {
        modelSpawnManager.OnArAnchorChanged += OnArAnchorChanged;
        dropdown.onValueChanged.AddListener(OnDropdownValueChangedHandler);
    }

    private void OnDropdownValueChangedHandler(int arg0)
    {
        if (_currentAnimator != null)
        {
            _currentAnimator.Play(dropdown.options[arg0].text, 0);
        }
        else
        {
            Debug.LogError("Current animator is null");
        }
    }

    private void OnArAnchorChanged(ARAnchor arAnchor)
    {
        if (arAnchor != null && arAnchor.TryGetComponent(out Animator animator))
        {
            _currentAnimator = animator;
            dropdown.ClearOptions();
            dropdown.AddOptions(animator.runtimeAnimatorController.animationClips.Select(ac => ac.name).ToList());
            var currentOption =
                dropdown.options.SingleOrDefault(o => o.text == animator.GetCurrentAnimatorClipInfo(0)[0].clip.name); // TODO: optimize
            dropdown.value = currentOption != null ? dropdown.options.IndexOf(currentOption) : 0;
            animationDependentUiElements.SetActive(true);
            return;
        }
        animationDependentUiElements.SetActive(false);
    }
}