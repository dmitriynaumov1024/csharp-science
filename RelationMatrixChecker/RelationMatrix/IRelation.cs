using System;

namespace RelationMatrix
{
    interface IRelation
    {
        int Size { get; }
        bool[,] Matrix { get; }
        
        bool IsReflective { get; }
        bool IsAntireflective { get; }
        bool IsSymmetric { get; }
        bool IsAntisymmetric { get; }
        bool IsAssymetric { get; }
        bool IsTransitive { get; }
        bool IsFull { get; }
        bool IsOrdered { get; }
        bool IsEquivalent { get; }
    }
}
