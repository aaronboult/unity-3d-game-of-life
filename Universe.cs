using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public static bool[,,] life;

    public delegate void universe_update();
    public static universe_update universe_updater = null;

    public bool universe_is_alive = true;

    public Mesh cell_mesh;
    public Material cell_material;

    [Range(0, 100)]
    public float percentage_life_chance = 10f;

    [Range(0f, 10f)]
    public float update_speed_seconds = 1f;

    public int size_x;
    public int size_y;
    public int size_z;

    private void Awake()
    {
        life = new bool[size_z, size_y, size_x];

        for (int z = 0; z < size_z; z++)
        {
            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
                {
                    if (Random.Range(0, 1000) <= percentage_life_chance * 10) // 1 in 10 chance the cell is alive
                    {
                        life[z, y, x] = true;
                    }
                    else
                    {
                        life[z, y, x] = false;
                    }

                    GameObject new_child = new GameObject("Cell");

                    new_child.AddComponent<MeshFilter>().mesh = cell_mesh;
                    new_child.AddComponent<MeshRenderer>().material = cell_material;

                    new_child.AddComponent<Cell>();
                    new_child.transform.SetParent(transform);
                    new_child.transform.localPosition = new Vector3(x, y, z);
                }
            }
        }
    }

    private void Start()
    {
        StartCoroutine(Begin());
    }

    public static bool Valid_Cell_Position(int[] coords)
    {
        if (life.GetLength(0) >= coords[2])
        {
            if (life.GetLength(1) >= coords[1])
            {
                if (life.GetLength(2) >= coords[0])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool Cell_Alive(int[] coords)
    {
        if (Valid_Cell_Position(coords))
        {
            return life[coords[2], coords[1], coords[0]];
        }

        return false;
    }

    private IEnumerator Begin()
    {
        while (universe_is_alive)
        {
            yield return new WaitForSeconds(update_speed_seconds);

            Update_Universe();

            universe_updater();
        }
    }

    private void Update_Universe()
    {
        bool[,,] new_life = (bool[,,]) life.Clone();

        for (int z = 0; z < size_z; z++)
        {
            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
                {
                    int neighbors = Get_Neighbors(x, y, z);
                    if (neighbors == 3 || (neighbors == 2 && life[z,y,x]))
                    {
                        new_life[z, y, x] = true;
                    }
                    else
                    {
                        new_life[z, y, x] = false;
                    }
                }
            }
        }

        life = new_life;
    }

    private int Get_Neighbors(int x, int y, int z)
    {
        int number_of_neighbors = 0;

        for (int z_offset = -1; z_offset <= 1; z_offset++)
        {
            for (int y_offset = -1; y_offset <= 1; y_offset++)
            {
                for (int x_offset = -1; x_offset <= 1; x_offset++)
                {
                    if (!(z_offset == 0 && y_offset == 0 && x_offset == 0))
                    {
                        if (Check_Neighbor(z + z_offset, y + y_offset, x + x_offset))
                        {
                            number_of_neighbors++;
                        }
                    }
                }
            }
        }

        return number_of_neighbors;
    }

    private bool Check_Neighbor(int z, int y, int x)
    {
        if ((z >= 0 && z < size_z) && (y >= 0 && y < size_y) && (x >= 0 && x < size_x))
        {
            if (life[z, y, x])
            {
                return true;
            }
        }

        return false;
    }
}