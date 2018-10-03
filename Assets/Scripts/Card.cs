using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {

    //Dégats infligés. Cette valeur sera multiplié par -1 si elle doit infligée des dégâts à l'adversaire afin de lui enlever des points de vie
    int damage;
    Color cardColor;

    // permet de savoir quelle couche de l'ordinateur va être touchée
    public ComputerLayer touchedLayer { get; set; }
    // bleue 0, 176, 240
    // rouge 255,0,0
    // vert 0, 176, 80

    //permet de savoir si la carte est une carte de type attaque (= inflige des dégâts) ou lors de type défense(=nous rajoute des points de vie)
    public bool isAttackCard { get; set; }    

    //Informations écrites sur la carte
    public string name { get; set; }
    public string definition { get; set; }
    public string action { get; set; }

    private float redColor;
    private float greenColor;
    private float blueColor;

    public Card(string name, string definition, string action, bool isAttackCard, ComputerLayer touchedLayer, int damage, Color cardColor)
    {
        this.name = name.ToUpper();
        this.definition = definition;
        this.action = action;
        this.isAttackCard = isAttackCard;
        this.damage = damage;
        //Permet de s'assurer que l'on ne s'est pas trompé lors de la déclaration du nombre de dégâts
        setDamage(this.damage);
        this.cardColor = cardColor;
        this.touchedLayer = touchedLayer;

        redColor = cardColor.r;
        greenColor = cardColor.g;
        blueColor = cardColor.b;

    }

    public Card() { }

    public Color getCardColor()
    {
        return cardColor;
    }

    public void setDamage(int damage)
    {
        if (isAttackCard && damage > 0)
            damage *= -1;
    }

    public int getDamage()
    {        
        return damage;
    }

    public string[] getCardinfosInAStringArray()
    {
        string[] tabInfos = new string[9];
        tabInfos[0] = name;
        tabInfos[1] = definition;
        tabInfos[2] = action;
        tabInfos[3] = isAttackCard.ToString();
        tabInfos[4] = touchedLayer.ToString();
        tabInfos[5] = damage.ToString();
        tabInfos[6] = redColor.ToString();
        tabInfos[7] = greenColor.ToString();
        tabInfos[8] = blueColor.ToString();
        return tabInfos;
    }

    public string splitCardToStringIntoInfos(string infos)
    {
        return "test";
    }


}
