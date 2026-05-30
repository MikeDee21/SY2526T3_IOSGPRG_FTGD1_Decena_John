using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerLives;
    private bool _isAlive;

    [Header("Dash Settings")]
    [SerializeField] private int _dashCharge;
    [SerializeField] private int _maxDashCharge = 10;
    [SerializeField] private float _dashDuration = 5f;
    [SerializeField] private Image _dashFillImage;
    [SerializeField] private bool _isDashActive;

   
    private void Start()
    {
        _isAlive = true;
        _playerLives = 3;

        _dashCharge = 9;
        UpdateDashUI();
    }

    private void Update()
    {
        TrackPlayerLife();
    }

    #region Player HP functions
    public void GainExtraLife(int lifeup)
    {
        if (!_isAlive)
            return;

        _playerLives+= lifeup; 
    }
    public void HitPlayer(int dmg)
    {
        Debug.Log("Player got hit!");

        _playerLives -= dmg;

        Debug.Log($"Remaining Lives: {_playerLives}");
    }
    private void TrackPlayerLife()
    {
        if (_playerLives <= 0)
        {
            _isAlive = false;
            GameManager.Instance.CallGameOver(); 

        }
    }
    #endregion
    #region Dash functions
    public bool IsDashActive
    {
        get => _isDashActive;
    }
    public void BTN_ActivateDash()
    {
        if (_dashCharge < _maxDashCharge)
        {
            return;
        }

        if (_isDashActive)
        {
            return;
        }

        StartCoroutine(CO_DashState());
    }

    public void AddDashCharge(int chargeAmount)
    {
        if (_isDashActive)
        {
            return;
        }

        _dashCharge += chargeAmount;

        _dashCharge = Mathf.Clamp(
            _dashCharge,
            0,
            _maxDashCharge
        );

        Debug.Log(
            $"Dash Charge: {_dashCharge}/{_maxDashCharge}"
        );

        UpdateDashUI();
    }

    public void ActivateDash()
    {
        if (_dashCharge < _maxDashCharge)
        {
            return;
        }

        if (_isDashActive)
        {
            return;
        }

        StartCoroutine(CO_DashState());
    }

    #endregion

    private void UpdateDashUI()
    {
        _dashFillImage.fillAmount =
            (float)_dashCharge / _maxDashCharge;
    }
    private IEnumerator CO_DashState()
    {
        _isDashActive = true;

        _dashCharge = 0;
        UpdateDashUI();
   
    Time.timeScale = 2f;

        Debug.Log("DASH ACTIVATED");

        yield return new WaitForSecondsRealtime(
            _dashDuration
        );

        Time.timeScale = 1f;

        _isDashActive = false;

        Debug.Log("DASH ENDED");
    }
}
