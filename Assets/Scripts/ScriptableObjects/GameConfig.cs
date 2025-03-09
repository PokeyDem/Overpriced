using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject{
  public int inventorySlotsInitialAmount = 16;
  public int rarityPriceIncreaseStep = 20; //Increase of item price for each rarity level in percents
}
