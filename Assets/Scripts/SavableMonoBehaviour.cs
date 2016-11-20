using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SavableMonoBehaviour: MonoBehaviour {
    public abstract SavableObject getSavableObject();
}
