using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerAggroCheckable
{
    bool IsAggroed { get; set; }
    bool IsInRange { get; set; }

    void SetAggroStatus(bool IsAggroed);
    void SetInRangeStatus(bool IsInRange);
}
