using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreField;
    [SerializeField]
    private int score = 0;
    [SerializeField]
    private Sprite[] _lives;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeScore(int change)
    {
        score += change;
        _scoreField.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        _livesDisplay.sprite = _lives[lives];
        if (lives <= 0)
        {
            _restartText.SetActive(true);
            StartCoroutine(FlickerGameOverRoutine());
        }
    }

    private IEnumerator FlickerGameOverRoutine()
    {
        while (true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(1f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    

}
