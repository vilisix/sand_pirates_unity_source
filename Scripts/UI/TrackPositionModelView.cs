using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Завершение гонки
/// </summary>
public class TrackPositionModelView : MonoBehaviour
{
    public TrackPath trackPath;
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI place;
    public float timeToRenew = 1f;
    public Text listText;
    public Text listTextOutline;
    public Text posText;
    public Text posTextOutline;
    private void Start()
    {
        StartCoroutine(RenewCorut());
    }


    IEnumerator RenewCorut()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToRenew);
            string textList = "";
            int position;
            foreach (string rawLeader in trackPath.GetShipPositionTable(out position))
            {
                textList += rawLeader + "\n";
            }

            listText.text = textList;
            listTextOutline.text = textList;

            posText.text = position.ToString();
            posTextOutline.text = position.ToString();
        }
    }
}
