
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarAction : MonoBehaviour
{
    private Image _image;

    [SerializeField]
    private float _timeToDrain = 0.25f;

    [SerializeField]
    private Gradient _healthBarGradient;

    private float _target = 1f;

    private Color _newHealthBarColor;

    private Coroutine drainHealthBarCoroutime;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.color = _healthBarGradient.Evaluate(_target);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateHealthBar(float RemainingHp, float HP)
    {
        _target = RemainingHp / HP;
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

            _image.color = Color.Lerp(currentColor, _newHealthBarColor, (elapsedTime / _timeToDrain));
            yield return null;
        }
    }

    private void CheckHealthBarGradientAmmount()
    {
        _newHealthBarColor = _healthBarGradient.Evaluate(_target);
    }
}
