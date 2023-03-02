using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICuttable {

    float CuttingProcessCount { get; }
    void SlicedUp();

}
