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

    public Card(string name, string definition, string action, bool isAttackCard, ComputerLayer touchedLayer, int damage, Color cardColor)
    {
        this.name = name.ToUpper();
        this.definition = definition;
        this.action = action;
        this.isAttackCard = isAttackCard;
        this.damage = damage;
        this.cardColor = cardColor;
        this.touchedLayer = touchedLayer;

    }

    public Color getCardColor()
    {
        return cardColor;
    }

    /*public void setCardColor(float r, float v, float b)
    {
        cardColor = new Color(r, v, b);

    }*/

    public void setDamage(int damage)
    {
        if (isAttackCard && damage > 0)
            damage *= -1;
    }

    public int getDamage()
    {        
        return damage;
    }


}
