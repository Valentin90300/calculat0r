using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal class ExpressionTreeNode
    {
        internal ExpressionTreeNode Parent
        { 
            get; private set;
        }

        internal ExpressionTreeNode LeftChild
        { 
            get; set; 
        }

        internal ExpressionTreeNode RightChild
        { 
            get; set; 
        }

        internal ILexeme Value 
        { 
            get; private set; 
        }

        internal ExpressionTreeNode(ExpressionTreeNode parent, ILexeme value)
        {
            this.Parent = parent;
            this.Value = value;
        }

        internal void Update(ILexeme value)
        {
            this.Value = value;

            this.LeftChild = null;
            this.RightChild = null;
        }
    }
}
