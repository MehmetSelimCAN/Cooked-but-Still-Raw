using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICuttable {

    float CuttingProcessCountMax { get; }
    void SlicedUp();

}
