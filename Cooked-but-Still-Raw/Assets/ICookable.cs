using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICookable {

    float CookingTimerMax { get; }
    void CookedUp();
    void BurnedUp();

}
