using UnityEngine;

public class GameOverMenu : BasicMenu
{
    [SerializeField] GameObject _gameOverMenu;
    private void OnEnable()
    {
       PlayerOnShipInput.GameOver += ShowGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerOnShipInput.GameOver -= ShowGameOverMenu;
    }

    private void ShowGameOverMenu()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
