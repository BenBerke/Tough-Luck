using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    [SerializeReference] float animLenghtSeconds;
    [HideInInspector] public bool animPlaying;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public IEnumerator Rotate(bool isFirst)
    {
        animPlaying = true;
        if (isFirst)
        animator.Play("CameraRotateToFirstPlayer");
        else
        animator.Play("CameraRotateToSecondPlayer");
        yield return new WaitForSeconds(animLenghtSeconds);
        animPlaying = false;
    }
}
