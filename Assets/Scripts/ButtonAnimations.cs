using DG.Tweening;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    public void ButtonPressed()
    {
        transform.DOShakePosition(0.2f, 5f, 10, 45, false, false, ShakeRandomnessMode.Full);
        transform.DOScale(0.95f, 0.1f).OnComplete(()=> 
        {
            transform.DOScale(1f, 0.1f); 
        });
    }
    public void ButtonRotate()
    {
        transform.DORotate(new Vector3(0f, 0f, 360f * 4f), 2f,RotateMode.LocalAxisAdd).SetEase(Ease.InOutFlash);
        transform.DOMoveX(2000f, 1f).OnComplete(() =>
        {
            transform.DOMoveX(-2000f, 0f);
            transform.DOMoveX(540f, 1f);
        });
    }
}
