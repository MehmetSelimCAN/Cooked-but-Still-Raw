using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFryable {

    float FryingTimerMax { get; }
    void FriedUp();
    void BurnedUp();

}
