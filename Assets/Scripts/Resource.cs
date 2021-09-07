using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    [SerializeField]
    public int allGold;
    [SerializeField]
    private Text txt;

    
    void Update()
    {
        txt.text = allGold + "gold";
    }
    

    //}
    //public void MinusGold(int gold)
    //{
    //    allGold -= gold;
    //}
    //public void PlusGold(int gold)
    //{
    //    allGold += gold;
    //}
}
