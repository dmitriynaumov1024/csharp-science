using System;
using System.Collections.Generic;
using System.Text;

namespace RelationMatrix_Winforms
{
    class XButtonCoordinated: XButton
    {
        public int Row, Col;
        public XButtonCoordinated(string text): base(text) { }
        public XButtonCoordinated(string text, int row, int col): this(text) 
        { 
            this.Row = row;
            this.Col = col;
        }
    }
}
