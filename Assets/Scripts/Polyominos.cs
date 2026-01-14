using UnityEngine;

public static class Polyominos
{
// Trong Polyominos.cs

    public readonly static int[][,] shapes = new int[][,]
    {
        new int[,] { { 1 } },
        new int[,] { { 1, 1 } },
        new int[,] { { 1 }, { 1 } },
        new int[,] { { 1, 1, 1 } },
        new int[,] { { 1 }, { 1 }, { 1 } },
        new int[,] { { 1, 1, 1, 1 } },
        new int[,] { { 1 }, { 1 }, { 1 }, { 1 } },
        new int[,] { { 1, 1, 1, 1, 1 } },
        new int[,] { { 1 }, { 1 }, { 1 }, { 1 }, { 1 } },
        new int[,] 
        { 
            { 1, 1 }, 
            { 1, 1 } 
        },
        new int[,] 
        { 
            { 1, 1, 1 }, 
            { 1, 1, 1 }, 
            { 1, 1, 1 } 
        },
        new int[,] 
        { 
            { 1, 0 }, 
            { 1, 1 } 
        },
        new int[,] 
        { 
            { 0, 1 }, 
            { 1, 1 } 
        },
        new int[,] 
        { 
            { 1, 1 }, 
            { 1, 0 } 
        },
        new int[,] 
        { 
            { 1, 1 }, 
            { 0, 1 } 
        },
        new int[,]
        {
            { 1, 0, 0 },
            { 1, 0, 0 },
            { 1, 1, 1 }
        },
        new int[,]
        {
            { 0, 0, 1 },
            { 0, 0, 1 },
            { 1, 1, 1 }
        },
        new int[,]
        {
            { 1, 1, 1 },
            { 1, 0, 0 },
            { 1, 0, 0 }
        },
        new int[,]
        {
            { 1, 1, 1 },
            { 0, 0, 1 },
            { 0, 0, 1 }
        }
    };

    public static int[,] GetShape(int index)
    {
        if (index < 0 || index >= shapes.Length) return shapes[0];
        return shapes[index];
    }
    
    public static int[,] GetRandomShape()
    {
        return shapes[Random.Range(0, shapes.Length)];
    }

}
