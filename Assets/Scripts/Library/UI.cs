using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Utility.Colors;
using Utility.Scheduling;
using Utility.Channels;
using DG.Tweening;

public class UI : MonoBehaviour {

    [SerializeField] private Text levelLabel, message, button;
    [SerializeField] private Image buttonImg, overlay;
    [SerializeField] private Image progressFill;

    public static UI appereance;

    public float progress {
        set {
            progressFill.fillAmount = Mathf.Lerp(progressFill.fillAmount, Mathf.Clamp(value, 0, 1), 0.05f);
        }
    }

    public int level {
        set {
            levelLabel.text = "Level " + value;
        }
    }

    void Awake() {
        appereance = this;
    }

    void Start() {
        HideElements();

        Channel.Subscribe("Level.Finished", (p)=> {
            ShowMessage("WELL DONE!", "Next Level");
        });

        Channel.Subscribe("Level.Failed", (p)=> {
            ShowMessage("OH NO!", "Play Again");
        });
    }

    public void HideElements() {
        buttonImg.gameObject.SetActive(false);
        buttonImg.color = Color.white.A(0);
        message.color = Color.white.A(0);
        overlay.color = Color.black.A(0);
    }

    public void ShowMessage(string msgText, string btnText = "Continue") {
        message.text = msgText;
        message.transform.localScale = Vector3.zero;
        message.color = Color.white.A(0);

        message.transform.DOKill();
        message.transform.DOScale(Vector3.one, 0.6f)
                         .SetEase(Ease.OutElastic)
                         .Play();

        message.DOKill();
        message.DOColor(Color.white, 0.2f)
               .SetEase(Ease.InOutSine)
               .Play();

        overlay.DOKill();
        overlay.DOColor(Color.black.A(0.5f), 0.5f)
               .SetEase(Ease.InOutSine)
               .Play();

        this.Schedule(()=> {
            button.text = btnText;
            button.color = Color.white.A(0);
            buttonImg.gameObject.SetActive(true);
            button.DOColor(Color.white, 0.4f)
                .SetEase(Ease.InOutSine)
                .Play();
            buttonImg.DOColor(Color.white, 0.32f)
                .SetEase(Ease.InOutSine)
                .Play();
        }).After(0.75f);

    }

    public void OnButtonPress() {
        Level.current.Reload();
    }

}
