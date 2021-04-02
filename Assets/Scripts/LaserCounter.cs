using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserCounter : MonoBehaviour
{
    private Text _ammoCount;
    [SerializeField]
    private AnimationCurve _pulse;
    private Coroutine _animating = null;

    private void Start()
    {
        _ammoCount = transform.GetChild(0).GetComponent<Text>();
    }
    public void UpdateAmmoCount(int newAmmoCount, int maxAmmoCount)
    {
        _ammoCount.text = newAmmoCount.ToString() + "/" + maxAmmoCount.ToString();
        
    }

    public void OutOfAmmoAnimControl()
    {
        if (_animating == null)
        {
            _animating = StartCoroutine(OutOfAmmoAnim());
        }
    }

    private IEnumerator OutOfAmmoAnim()
    {
        float _timer = 2;
        float _currentTime = 0;
        Vector3 _sizeContainer = Vector3.one;
        while (_timer >= _currentTime)
        {
            _sizeContainer = Vector3.one * _pulse.Evaluate(_currentTime);
            transform.localScale = _sizeContainer;
            _currentTime += Time.deltaTime;
            yield return null;
        }
        _animating = null;
    }
}
