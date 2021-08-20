using System;

namespace RelationMatrix
{
    public class Relation: IRelation
    {
        private const int SizeUpperLimit = 20;

        public bool[,] Matrix { get; protected set; }
        public int Size { get; protected set; }

        public bool IsReflective 
        {
            get
            {
                for(int i=0; i<this.Size; i++)
                {
                    if(!this.Matrix[i, i]) return false;
                }
                return true;
            }
        }

        public bool IsAntireflective
        {
            get
            {
                for(int i=0; i<this.Size; i++)
                {
                    if(this.Matrix[i, i]) return false;
                }
                return true;
            }
        }

        public bool IsSymmetric
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                {
                    for(int j=i+1; j<this.Size; j++)
                    {
                        if(this.Matrix[i, j] != this.Matrix[j, i]) return false;
                    }
                }
                return true;
            }
        }

        public bool IsAntisymmetric
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                {
                    for(int j=i+1; j<this.Size; j++)
                    {
                        if(this.Matrix[i, j] && this.Matrix[j, i]) return false;
                    }
                }
                return true;
            }
        }

        public bool IsAssymetric
        {

            get
            {
                for(int i=0; i<this.Size; i++)
                {
                    if(this.Matrix[i, i]) return false;
                }

                for(int i=0; i<this.Size-1; i++)
                {
                    for(int j=i+1; j<this.Size; j++)
                    {
                        if(this.Matrix[i, j] == this.Matrix[j, i]) return false;
                    }
                }
                return true;
            }
        }

        public bool IsTransitive
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                {
                    for(int j=0; j<this.Size; j++) 
                    { 
                        if(this.Matrix[i, j])
                        {
                            for(int k=0; k<this.Size; k++)
                            {
                                if(this.Matrix[i, k] && !this.Matrix[k, j]) return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        public bool IsFull
        {
            get
            {
                for(int i=0; i<this.Size-1; i++)
                {
                    for(int j=i+1; j<this.Size; j++)
                    {
                        if(!(this.Matrix[i, j] || this.Matrix[j, i])) return false;
                    }
                }
                return true;
            }
        }

        public bool IsOrdered => (this.IsTransitive && this.IsAntisymmetric);

        public bool IsEquivalent => (this.IsTransitive && this.IsSymmetric && this.IsReflective);

        /// <summary>
        /// Resize in-place and optionally keep old matrix values.
        /// </summary>
        /// <param name="newSize">New size of matrix.</param>
        /// <param name="keepMatrixValues">Indicates whether old matrix should be kept.</param>
        public virtual void Resize(int newSize, bool keepMatrixValues=false)
        {
            if (newSize != this.Size)
            {
                // Back up old values
                var keepSize = Math.Min(this.Size, newSize);
                var oldMatrix = this.Matrix;

                // Resize
                this.Size = newSize;
                this.Matrix = new bool[newSize, newSize];

                // Restore matrix values
                if (keepMatrixValues)
                {
                    for(int i=0; i<keepSize; i++)
                    {
                        for(int j=0; j<keepSize; j++)
                        {
                            this.Matrix[i, j] = oldMatrix[i, j];
                        }
                    }
                }
            }
        }

        public Relation(int size)
        {
            this.Size = size;
            this.Matrix = new bool[size, size];
        }
    }
}
