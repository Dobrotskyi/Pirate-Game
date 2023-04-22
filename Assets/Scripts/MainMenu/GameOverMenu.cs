using UnityEngine;

public class GameOverMenu : BasicMenu
{
    [SerializeField] GameObject _gameOverMenu;
    private void OnEnable()
    {
        PlayerShipCharacteristics.GameOver += ShowGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerShipCharacteristics.GameOver -= ShowGameOverMenu;
    }

    private void ShowGameOverMenu()
    {
        CursorController.SetCursorOn();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
