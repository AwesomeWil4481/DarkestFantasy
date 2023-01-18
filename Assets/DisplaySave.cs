  using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySave : MonoBehaviour
{
    public int saveNumber;

    public GameObject[] portraits;
    public Sprite[] images;

    private void OnEnable()
    {
        List<characterStats> stats = SaveTheBooks.Instance().DisplayStats(saveNumber);

        foreach(characterStats c in stats)
        {
            foreach (Sprite i in images)
            {
                if (i.name == c.Name)
                {
                    print(c._position - 1);
                    portraits[c._position - 1].SetActive(true);
                    portraits[c._position - 1].GetComponent<Image>().sprite = i;
                }
            }
        }
    }
}
