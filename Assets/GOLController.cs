using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOLController : MonoBehaviour
{
    //Singleton
    public static GOLController _instance;
    public static GOLController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GOLController>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("GOL_Controller");
                    _instance = container.AddComponent<GOLController>();
                }
            }

            return _instance;
        }
    }

    //Data structure
    public struct Box
    {
        public Box(int x, int y, int z, bool in_alive, int max, GameObject gameO)
        {
            X = x;
            Y = y;
            Z = z;
            Alive = in_alive;
            Model = gameO;
            counter = 0;
            maxLife = max;
        }

        public int X;
        public int Y;
        public int Z;
        public int counter;
        public int maxLife;
        public bool Alive;
        public GameObject Model;
    }

    private Box[,,] stateMatrix;

    //Model
    public GameObject cube;

    //Distance
    public float distance;
    public int maxLife;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize matrix
        stateMatrix = new Box[30, 30, 30];

        for (int i = 0; i < stateMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < stateMatrix.GetLength(1); j++)
            {
                for (int n = 0; n < stateMatrix.GetLength(2); n++)
                {
                    stateMatrix[i, j, n] = new Box(i, j, n, false, maxLife, null);
                }
            }
        }

        //Initialize game state
        boxPosition(ref stateMatrix[20, 20, 20]);
        boxPosition(ref stateMatrix[21, 20, 19]);
        boxPosition(ref stateMatrix[20, 20, 19]);
        boxPosition(ref stateMatrix[19, 20, 19]);

        boxPosition(ref stateMatrix[18, 20, 18]);
        boxPosition(ref stateMatrix[19, 20, 18]);
        boxPosition(ref stateMatrix[20, 20, 18]);
        boxPosition(ref stateMatrix[21, 20, 18]);
        boxPosition(ref stateMatrix[22, 20, 18]);

        boxPosition(ref stateMatrix[20, 21, 20]);
        boxPosition(ref stateMatrix[19, 21, 18]);
        boxPosition(ref stateMatrix[20, 21, 18]);
        boxPosition(ref stateMatrix[21, 21, 18]);

        boxPosition(ref stateMatrix[20, 19, 20]);
        boxPosition(ref stateMatrix[19, 19, 18]);
        boxPosition(ref stateMatrix[20, 19, 18]);
        boxPosition(ref stateMatrix[21, 19, 18]);

        //InvokeRepeating("updateBoxes", 0.0f, 0.5f);
    }
    // Update is called once per frame
    void Update()
    { 
        for(int i = 0; i < stateMatrix.GetLength(0); i++)
        {
            for(int j = 0; j < stateMatrix.GetLength(1); j++)
            {
                for(int n = 0; n < stateMatrix.GetLength(2); n++)
                {
                    //Calculate neighbours
                    int alive = 0;

                    //X axis
                    if(i + 1 < stateMatrix.GetLength(0) && stateMatrix[i + 1,j,n].Alive == true)
                    {
                        alive++;
                    }

                    if (i - 1 >= 0 && stateMatrix[i - 1, j, n].Alive == true)
                    {
                        alive++;
                    }

                    //Y axis
                    if (j + 1 < stateMatrix.GetLength(1) && stateMatrix[i, j + 1, n].Alive == true)
                    {
                        alive++;
                    }

                    if (j - 1 >= 0 && stateMatrix[i, j - 1, n].Alive == true)
                    {
                        alive++;
                    }

                    //Z axis
                    if (n + 1 < stateMatrix.GetLength(2) && stateMatrix[i, j, n + 1].Alive == true)
                    {
                        alive++;
                    }

                    if (n - 1 >= 0 && stateMatrix[i, j, n - 1].Alive == true)
                    {
                        alive++;
                    }

                    //---- X-Y axis ----
                    //(x+1 y+1)
                    if (i + 1 < stateMatrix.GetLength(0) && j + 1 < stateMatrix.GetLength(1) && stateMatrix[i + 1, j + 1, n].Alive == true)
                    {
                        alive++;
                    }
                    //(x-1 y+1)
                    if (i - 1 >= 0 && j + 1 < stateMatrix.GetLength(1) && stateMatrix[i - 1, j + 1, n].Alive == true)
                    {
                        alive++;
                    }
                    //(x+1 y-1)
                    if (i + 1 < stateMatrix.GetLength(0) && j - 1 >= 0 && stateMatrix[i + 1, j - 1, n].Alive == true)
                    {
                        alive++;
                    }
                    //(x-1 y-1)
                    if (i - 1 >= 0 && j - 1 >= 0 && stateMatrix[i - 1, j - 1, n].Alive == true)
                    {
                        alive++;
                    }

                    //---- Z-Y axis ----
                    //(z+1 y+1)
                    if (n + 1 < stateMatrix.GetLength(2) && j + 1 < stateMatrix.GetLength(1) && stateMatrix[i, j + 1, n + 1].Alive == true)
                    {
                        alive++;
                    }
                    //(z-1 y+1)
                    if (n - 1 >= 0 && j + 1 < stateMatrix.GetLength(1) && stateMatrix[i, j + 1, n - 1].Alive == true)
                    {
                        alive++;
                    }
                    //(z+1 y-1)
                    if (n + 1 < stateMatrix.GetLength(2) && j - 1 >= 0 && stateMatrix[i, j - 1, n + 1].Alive == true)
                    {
                        alive++;
                    }
                    //(z-1 y-1)
                    if (n - 1 >= 0 && j - 1 >= 0 && stateMatrix[i, j - 1, n - 1].Alive == true)
                    {
                        alive++;
                    }

                    //---- X-z axis ----
                    //(x+1 z+1)
                    if (i + 1 < stateMatrix.GetLength(0) && n + 1 < stateMatrix.GetLength(1) && stateMatrix[i + 1, j, n + 1].Alive == true)
                    {
                        alive++;
                    }
                    //(x-1 z+1)
                    if (i - 1 >= 0 && n + 1 < stateMatrix.GetLength(1) && stateMatrix[i - 1, j, n + 1].Alive == true)
                    {
                        alive++;
                    }
                    //(x+1 z-1)
                    if (i + 1 < stateMatrix.GetLength(0) && n - 1 >= 0 && stateMatrix[i + 1, j, n - 1].Alive == true)
                    {
                        alive++;
                    }
                    //(x-1 z-1)
                    if (i - 1 >= 0 && n - 1 >= 0 && stateMatrix[i - 1, j, n - 1].Alive == true)
                    {
                        alive++;
                    }

                    stateMatrix[i, j, n].counter++;

                    renderBox(applyRules(alive, stateMatrix[i, j, n]), ref stateMatrix[i, j, n]);
                }
            }
        }
    }

    private bool applyRules(int alive, Box box)
    {
        bool change = false;

        if (box.Alive)
        {
            if (alive > 2 || alive < 2)
            {
                box.counter++;
                box.counter++;
            }

            if(box.counter >= box.maxLife)
            {
                change = true;
            }
        }
        else
        {
            if (alive > 3)
            {
                change = true;
            }
        }

        return change;
    }

    private void renderBox(bool change, ref Box box)
    {
        if (change)
        {
            if (!box.Alive)
            {
                boxPosition(ref box);
            }
            else
            {
                boxDestroy(ref box);
            }
        }
    }

    private void boxPosition(ref Box box)
    {
        box.Alive = true;
        box.counter = 0;
        box.Model = Instantiate(cube);
        box.Model.transform.position = new Vector3(box.X * distance, box.Y * distance, box.Z * distance);
    }

    public void destroyBoxByPosition(Vector3 position)
    {
        boxDestroy( ref stateMatrix[(int)(position.x / distance), (int)(position.y / distance), (int)(position.z / distance)]);
    }

    private void boxDestroy(ref Box box)
    {
        Destroy(box.Model);
        box.Model = null;
        box.Alive = false;
        box.counter = 0;
    }

    public Box[,,] getStateMatrix()
    {
        return stateMatrix;
    }
}
