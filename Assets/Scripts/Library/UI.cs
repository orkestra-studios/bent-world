using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Utility.Colors;
using Utility.Scheduling;
using DG.Tweening;

public class UI : MonoBehaviour {

    public Text levelLabel, message;
    public Image overlay;
    public Button button;


    public int level {
        set {
            levelLabel.text = "Level " + value;
        }
    }

    public void Setup(int n) {
        level = n;
        message.color = Color.white.A(0);
        overlay.color = Color.black.A(0);
        button.gameObject.SetActive(false);
    }

    public void ShowMessage(string msgText) {
        message.text = msgText;
        message.transform.localScale = Vector3.zero;
        message.color = Color.white.A(0);

        message.transform.DOKill();
        message.transform.DOScale(Vector3.one, 0.8f)
                         .SetEase(Ease.OutElastic)
                         .Play();

        message.DOKill();
        message.DOColor(Color.white, 0.2f)
               .SetEase(Ease.InOutSine)
               .Play();

        overlay.color = Color.black.A(0);

        overlay.DOColor(Color.black.A(0.5f),0.3f)
               .SetEase(Ease.InOutSine)
               .Play();


        this.Schedule(()=>{
            button.gameObject.SetActive(true);
        }).After(0.5f);

    }

    void Update() {
        
    }

}
