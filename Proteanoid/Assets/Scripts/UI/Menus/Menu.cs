using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Animator))]
public abstract class Menu<T> : Singleton<T> where T : Menu<T>
{
    protected CanvasGroup canvasGroup;
    protected Animator animator;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        CloseMenu();
    }
    public virtual void OpenMenu() {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        animator.SetBool("isMenuOpen", true);
    }
    public virtual void CloseMenu() {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        animator.SetBool("isMenuOpen", false);
    }
}
