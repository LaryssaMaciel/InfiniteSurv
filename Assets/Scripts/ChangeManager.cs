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
    //public Text texto;
    public Sprite[] img;
    private Image image;
    
    void Start()
    {
        //texto = GameObject.Find("txtArma").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        image = GetComponent<Image>();
        lista.Add("axe");
    }

    void Update() 
    {
        if (itemNum > lista.Count -1) { itemNum = 0; }

        //if (texto != null) { texto.text = item; }

        switch (lista[itemNum])
        {
            case "axe":
                item = "Axe";
                image.sprite = img[0];
                player.tipoAtaque = "axe";
                break;
            case "tiro":
                item = "Gun";
                image.sprite = img[1];
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
        player.AudioManager(player.audioSource1, 2);
        print(lista[itemNum]);
    }

    // public void Curar_()
    // {
    //     if (lista[itemNum] == "cura") { player.Curar(); }
    // }
}