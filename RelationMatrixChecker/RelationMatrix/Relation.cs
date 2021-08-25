using System;

namespace RelationMatrix
{
    public class Relation: IRelation, IInteractiveGrid
    {
        private const int SizeUpperLimit = 14;

        public event EventHandler<GridChangeEventArgs> ObjectChanged;

        public bool[,] Matrix { get; protected set; }
        public int Size { get; protected set; }

        public bool IsReflective 
        {
            get
            {
                for(int i=0; i<this.Size; i++)
                    if(!this.Matrix[i, i]) return false;
                
                return true;
            }
        }

        public bool IsAntireflective
        {
            get
            {
                for(int i=0; i<this.Size; i++)
                    if(this.Matrix[i, i]) return false;

                return true;
            }
        }

        public bool IsSymmetric
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                    for(int j=i+1; j<this.Size; j++)
                        if(this.Matrix[i, j] != this.Matrix[j, i]) return false;
                    
                return true;
            }
        }

        public bool IsAntisymmetric
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                    for(int j=i+1; j<this.Size; j++)
                        if(this.Matrix[i, j] && this.Matrix[j, i]) return false;
                    
                return true;
            }
        }

        public bool IsAsymmetric
        {

            get
            {
                for(int i=0; i<this.Size; i++)
                    if(this.Matrix[i, i]) return false;
                

                for(int i=0; i<this.Size-1; i++)
                    for(int j=i+1; j<this.Size; j++)
                        if(this.Matrix[i, j] == this.Matrix[j, i]) return false;
                    
                return true;
            }
        }

        public bool IsTransitive
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                    for(int j=0; j<this.Size; j++) 
                        if(this.Matrix[i, j])
                            for(int k=0; k<this.Size; k++)
                                if(this.Matrix[i, k] && !this.Matrix[k, j]) return false;
                            
                return true;
            }
        }

        public bool IsFull
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                    for(int j=i+1; j<this.Size; j++)
                        if(!(this.Matrix[i, j] || this.Matrix[j, i])) return false;
                    
                return true;
            }
        }

        public bool IsOrdered => (this.IsTransitive && this.IsAntisymmetric);

        public bool IsEquivalent => (this.IsTransitive && this.IsSymmetric && this.IsReflective);

        /// <summary>
        /// Resize in-place and optionally keep old matrix values.
        /// </summary>
        /// <param name="newSize">New size of matrix.</param>
        /// <param name="keepValues">Indicates whether old matrix should be kept.</param>
        public virtual void Resize(int newSize, bool keepValues=false)
        {
            // Parameter value check
            if (newSize < 1 || newSize > SizeUpperLimit)
            {
                throw new ArgumentOutOfRangeException (
                    paramName: "newSize", 
                    message: $"Parameter newSize = {newSize} is out of [0...{SizeUpperLimit}] range."
                );
            }

            if (newSize != this.Size)
            {
                // Back up old values
                var keepSize = Math.Min(this.Size, newSize);
                var oldMatrix = this.Matrix;

                // Resize
                this.Size = newSize;
                this.Matrix = new bool[newSize, newSize];

                // Restore matrix values
                if (keepValues)
                {
                    for(int i=0; i<keepSize; i++)
                    {
                        for(int j=0; j<keepSize; j++)
                        {
                            this.Matrix[i, j] = oldMatrix[i, j];
                        }
                    }
                }

                this.ObjectChanged.Invoke (
                    this,
                    new GridChangeEventArgs {
                        WhatChanged = GridChangeEventArgs.Change.Resize
                    }
                );
            }
        }

        public virtual bool TryIncrementSize(bool keepValues)
        {
            try { 
                this.Resize(this.Size + 1, keepValues); 
                return true; 
            }
            catch (ArgumentOutOfRangeException) { 
                return false; 
            }
        }

        public virtual bool TryDecrementSize(bool keepValues)
        {
            try { 
                this.Resize(this.Size - 1, keepValues); 
                return true; 
            }
            catch (ArgumentOutOfRangeException) { 
                return false; 
            }
        }

        public void Clear()
        {
            this.Matrix = new bool[Size, Size];
            this.ObjectChanged.Invoke (
                this,
                new GridChangeEventArgs {
                    WhatChanged = GridChangeEventArgs.Change.Resize
                }
            );
        }

        public void ToggleCell(int row, int col)
        {
            this.Matrix[row, col] = !this.Matrix[row, col];
            this.ObjectChanged.Invoke (
                this, 
                new GridChangeEventArgs { 
                    WhatChanged = GridChangeEventArgs.Change.CellToggled, 
                    Col = col, 
                    Row = row
                }
            );
        }

        public Relation(int size)
        {
            // Parameter value check
            if (size < 0 || size > SizeUpperLimit)
            {
                throw new ArgumentOutOfRangeException (
                    paramName: "size", 
                    message: $"Parameter size = {size} is out of [0...{SizeUpperLimit}] range."
                );
            }
            this.Size = size;
            this.Matrix = new bool[size, size];
        }
    }
}
