using System;
using System.Collections.Generic;
using System.Text;

namespace RelationMatrix
{
    public interface IInteractiveGrid
    {
        event EventHandler<GridChangeEventArgs> ObjectChanged;
        void ToggleCell(int row, int col);
        void Resize(int newSize, bool keepValues);
        void Clear();

        bool TryDecrementSize(bool keepValues);
        bool TryIncrementSize(bool keepValues);

        int Size { get; }
    }
}
