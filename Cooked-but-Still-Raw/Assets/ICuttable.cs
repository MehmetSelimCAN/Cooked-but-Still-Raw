using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICuttable {

    float ProcessCountMax { get; }
    void SlicedUp();

}
