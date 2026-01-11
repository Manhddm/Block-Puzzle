using UnityEngine;

public static class Polyominos
{
    private readonly static int[][,] polyminos = new int[][,]
    {
        new int[,]
        {
            { 0, 0, 1 },
            { 0, 0, 1 },
            { 1, 1, 1 }
        }
    };
    public static int[,] Get(int index)    => polyminos[index];
    public static int Length => polyminos.Length;

    static Polyominos()
    {
        foreach (var polymino in polyminos)         
        {
            ReverseRow(polymino);
        }
    }

    private static void ReverseRow(int [,] polyomino)
    {
        var polyominoRows = polyomino.GetLength(0); 
        var polyominoColumns = polyomino.GetLength(1);
        for (int i = 0; i < polyominoRows/2; i++)
        {
            var topRow = i;
            var bottomRow = polyominoRows - 1 -i;
            for (int j = 0; j < polyominoColumns; j++)
            {
                (polyomino[bottomRow, j], polyomino[topRow, j]) = (polyomino[topRow, j], polyomino[bottomRow, j]);
            }
            
        }
    }

}
