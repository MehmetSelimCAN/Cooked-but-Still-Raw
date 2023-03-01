using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICookable {

    float CookingTimerMax { get; }
    float BurningTimerMax { get; }
    void Liquize();
    void CookedUp();
    void BurnedUp();

}
