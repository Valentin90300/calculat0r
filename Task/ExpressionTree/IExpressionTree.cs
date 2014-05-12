using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal interface IExpressionTree
    {
        ExpressionTreeNode CreateRootNode(ILexeme value);
        ExpressionTreeNode CreateChildNode(ExpressionTreeNode parentNode, ParentBranchKind parentBranchKind, ILexeme value);
        NumberLexemeBase Calculate();
    }
}
