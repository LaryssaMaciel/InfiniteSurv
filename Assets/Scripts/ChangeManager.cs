using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeManager : MonoBehaviour
{
    public List<string> lista = new List<string>();
    [SerializeField] private string item = "";
    [SerializeField] private int itemNum = 0;

    private Player player;
    public Text texto;
    
    void Start()
    {
        texto = GameObject.Find("txtArma").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lista.Add("axe");
    }

    void Update() 
    {
        if (itemNum > lista.Count -1) { itemNum = 0; }
        texto.text = item;

        switch (lista[itemNum])
        {
            case "axe":
                item = "Axe";
                player.tipoAtaque = "axe";
                break;
            case "tiro":
                item = "Gun";
                player.tipoAtaque = "tiro";
                break;
            // case "cura":
            //     item = "Health\n" + player.vidasExtra.ToString();
            //     break;
        }
    }

    public void Troca()
    {
        itemNum++;
        print(lista[itemNum]);
    }

    // public void Curar_()
    // {
    //     if (lista[itemNum] == "cura") { player.Curar(); }
    // }
}