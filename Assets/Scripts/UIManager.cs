using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _healthFill;
    [SerializeField] private RectTransform _staminaFill;
    [SerializeField] private RectTransform _weaponFill;
    [SerializeField] private GameObject _weaponInfo;

    private float _healthFillSize;
    private float _staminaFillSize;
    private float _weaponFillSize;

    void Start()
    {
        _healthFillSize     = _healthFill.rect.width;
        _staminaFillSize    = _staminaFill.rect.width;
        _weaponFillSize     = _weaponFill.rect.width;
    }

    public void SetHealthFill(float fill)
    {
        _healthFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fill * _healthFillSize);
    }

    public void SetStaminaFill(float fill)
    {
        _staminaFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fill * _staminaFillSize);
    }

    public void SetWeaponFill(float fill)
    {
        _weaponFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fill * _weaponFillSize);
    }

    public void ShowWeaponInfo()
    {
        _weaponInfo.SetActive(true);
    }

    public void HideWeaponInfo()
    {
        _weaponInfo.SetActive(false);
    }

}
