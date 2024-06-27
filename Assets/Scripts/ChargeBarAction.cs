using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBarAction : MonoBehaviour
{
    private Image _image;

    [SerializeField]
    private float _timeToDrain = 0.25f;

    [SerializeField]
    private Gradient _chargeBarGradient;

    private float _target = 1f;

    private Color _newChargeBarColor;

    private Coroutine drainHealthBarCoroutime;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.color = _chargeBarGradient.Evaluate(_target);
        ClearChargeBar();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateChargeBar(float charge, float maxCharge)
    {
        _target = charge / maxCharge;
        drainHealthBarCoroutime = StartCoroutine(DrainHealthBar());
        CheckHealthBarGradientAmmount();
    }
    private IEnumerator DrainHealthBar()
    {
        float fillAmount = _image.fillAmount;
        Color currentColor = _image.color;

        float elapsedTime = 0f;
        while (elapsedTime < _timeToDrain)
        {
            elapsedTime += Time.deltaTime;

            _image.fillAmount = Mathf.Lerp(fillAmount, _target, (elapsedTime / _timeToDrain));

            _image.color = Color.Lerp(currentColor, _newChargeBarColor, (elapsedTime / _timeToDrain));
            yield return null;
        }
    }
    public void ClearChargeBar()
    {
        _target = 0;
        _image.fillAmount = _target;
        CheckHealthBarGradientAmmount();
        _image.color = _newChargeBarColor;
    }

    private void CheckHealthBarGradientAmmount()
    {
        _newChargeBarColor = _chargeBarGradient.Evaluate(_target);
    }
}
