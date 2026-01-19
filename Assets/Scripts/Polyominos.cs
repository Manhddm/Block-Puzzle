
public static class Polyominos
{
// Trong Polyominos.cs

    public readonly static int[][,] Shapes = new int[][,]
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
        if (index < 0 || index >= Shapes.Length) return Shapes[0];
        return Shapes[index];
    }
    

}
