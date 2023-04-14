using UnityEngine;
using TMPro;

public class CannonballAmountUI : MonoBehaviour
{
    [SerializeField] private GameObject _notification;
    private TextMeshProUGUI _TMP;

    private void UpdateCannonballsAmount(int amt)
    {
        int lastAmt = System.Convert.ToInt32(_TMP.text);
        MakeNotification(amt - lastAmt);
        _TMP.text = amt.ToString();

    }

    private void OnEnable()
    {
        _TMP = GetComponent<TextMeshProUGUI>();
        PlayerShipCharacteristics.CannonballsAmtChanged += UpdateCannonballsAmount;
    }

    private void OnDisable()
    {
        PlayerShipCharacteristics.CannonballsAmtChanged -= UpdateCannonballsAmount;
    }

    private void MakeNotification(int amt)
    {
        GameObject notification = Instantiate(_notification, transform);
        TextMeshProUGUI textElement = notification.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        string notificationText = "";
        if(amt > 0)
            notificationText += "+ ";
        notificationText += amt.ToString();
        textElement.text = notificationText;
    }
}
