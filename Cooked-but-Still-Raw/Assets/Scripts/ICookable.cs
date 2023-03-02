using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICookable {

    float CookingTime { get; }
    float BurningTime { get; }
    void Liquize();
    void CookedUp();
    void BurnedUp();

}
