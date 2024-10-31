using DG.Tweening;
using UnityEngine;

public class AbstractAnimation : MonoBehaviour
{
    [SerializeField] protected float _duration = 0.3f;

    protected Tween _animTween;
    protected bool _animIsPlayind;
}
