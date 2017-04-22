using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalMinimum.Grid
{

    public interface IGridder
    {
        Coordinate Coordinate(Vector3 position, Space space);
        Vector3 Position(Coordinate coordinate, Space space);
        Vector3 GetWorldPosition(Vector3 position);

        bool IsValidPosition(Coordinate pos);

        int Width { get; }
        int Height { get; }

        Vector3 Normal { get; }

    }
}