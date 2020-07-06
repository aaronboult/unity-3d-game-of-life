using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;

    void Start()
    {
        Cell_Update();

        Universe.universe_updater += new Universe.universe_update(Cell_Update);
    }

    void Cell_Update()
    {
        if (Universe.Valid_Cell_Position(transform.position.FloorToInts()))
        {
            alive = Universe.Cell_Alive(transform.position.FloorToInts());
            gameObject.SetActive(alive);
        }
    }
}
