using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFryable {

    float FryingTime { get; }
    float BurningTime { get; }
    void FriedUp();
    void BurnedUp();

}
