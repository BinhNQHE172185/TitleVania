using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerEdgeCheckable
{
    bool IsOutsideOfEdge { get; set; }

    void SetOutsideOfEdgeStatus(bool IsOutsideOfEdge);
}
