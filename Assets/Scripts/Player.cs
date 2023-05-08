using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private UIManager  _uiManager;
    [SerializeField] private int        _maxHealth;

    private int _health;

    void Start()
    {
        _health = _maxHealth;
    }

    public void Damage(int amount)
    {
        _health = Mathf.Max(_health - amount, 0);

        _uiManager.SetHealthFill((float)_health / _maxHealth);

        if (_health == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
