using System;
using System.Collections.Generic;
using System.Text;

namespace RelationMatrix
{
    public class GridChangeEventArgs: EventArgs
    {
        public enum Change: byte
        {
            None,
            Resize,
            CellToggled
        }

        public Change WhatChanged = Change.None;
        public int? Row, Col;
    }
}
