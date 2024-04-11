using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Animator))]
public abstract class Menu<T> : Singleton<T> where T : Menu<T>
{
    protected CanvasGroup canvasGroup;
    protected Animator animator;
    public bool isMenuOpen;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        CloseMenu();
    }
    public virtual void OpenMenu() {
        SetMenuOpen(true);
    }
    public virtual void CloseMenu() {
        SetMenuOpen(false);
    }

    private void SetMenuOpen(bool b)
    {
        canvasGroup.interactable = b;
        canvasGroup.blocksRaycasts = b;
        animator.SetBool("isMenuOpen", b);
        isMenuOpen = b;
    }
}
