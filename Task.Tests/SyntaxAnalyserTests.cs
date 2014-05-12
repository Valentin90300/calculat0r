using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task;

namespace Task.Tests.SyntaxAnalyserTestsNamespace
{
    #region Test Stuff
    public class LowOperatorPriority : IPriorityCategory
    {
        public PriorityCategoryRegisterBase PriorityCategoryRegister
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int GetPriorityNumber()
        {
            return 1;
        }
    }

    public class MiddleOperatorPriority : IPriorityCategory
    {
        public PriorityCategoryRegisterBase PriorityCategoryRegister
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int GetPriorityNumber()
        {
            return 2;
        }
    }

    public class HighOperatorPriority : IPriorityCategory
    {
        public PriorityCategoryRegisterBase PriorityCategoryRegister
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int GetPriorityNumber()
        {
            return 3;
        }
    }

    public class xBinaryOperator : BinaryOperatorLexemeBase
    {
        public xBinaryOperator() : base(new LowOperatorPriority()) { }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            throw new NotImplementedException();
        }
    }

    public class yBinaryOperator : BinaryOperatorLexemeBase
    {
        public yBinaryOperator() : base(new LowOperatorPriority()) { }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            throw new NotImplementedException();
        }
    }

    public class zBinaryOperator : BinaryOperatorLexemeBase
    {
        public zBinaryOperator() : base(new LowOperatorPriority()) { }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            throw new NotImplementedException();
        }
    }

    public class qBinaryOperator : BinaryOperatorLexemeBase
    {
        public qBinaryOperator() : base(new MiddleOperatorPriority()) { }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            throw new NotImplementedException();
        }
    }

    public class iUnaryOperator : UnaryOperatorLexemeBase
    {
        public iUnaryOperator() : base(new HighOperatorPriority()) { }

        public override NumberLexemeBase Perform(NumberLexemeBase operand)
        {
            throw new NotImplementedException();
        }
    }

    public class aIntOperand : NumberLexemeBase
    {
        public aIntOperand() : base(0) { }
    }

    public class bIntOperand : NumberLexemeBase
    {
        public bIntOperand() : base(0) { }
    }

    public class cIntOperand : NumberLexemeBase
    {
        public cIntOperand() : base(0) { }
    }

    public class dIntOperand : NumberLexemeBase
    {
        public dIntOperand() : base(0) { }
    }

    public class eIntOperand : NumberLexemeBase
    {
        public eIntOperand() : base(0) { }
    }

    public class TestExpressionLexeme : IExpressionLexeme
    {
        public ILexeme[] Content { get; set; }

        public TestExpressionLexeme(ILexeme[] content)
        {
            this.Content = content;
        }

        void IExpressionLexeme.SplitExpressionByBinaryOperatorWithLowestPriority
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme, 
            out IExpressionLexeme leftExpressionLexeme, 
            out IExpressionLexeme rightExpressionLexeme
        )
        {
            if (this.IsStateA())
            {
                this.SplitFromStateA
                (
                    out binaryOperatorLexeme,
                    out leftExpressionLexeme,
                    out rightExpressionLexeme
                );
            }
            else if (this.IsStateB())
            {
                this.SplitFromStateB
                (
                    out binaryOperatorLexeme,
                    out leftExpressionLexeme,
                    out rightExpressionLexeme
                );
            }
            else if (this.IsStateC())
            {
                this.SplitFromStateC
                (
                    out binaryOperatorLexeme,
                    out leftExpressionLexeme,
                    out rightExpressionLexeme
                );
            }
            else if (this.IsStateD())
            {
                this.SplitFromStateD
                (
                    out binaryOperatorLexeme,
                    out leftExpressionLexeme,
                    out rightExpressionLexeme
                );
            }
            else
            {
                if (this.IsStateE() || this.IsStateF())
                {
                    binaryOperatorLexeme = null;
                    leftExpressionLexeme = null;
                    rightExpressionLexeme = null;
                }
                else
                {
                    throw new AssertFailedException("Некорректное использование объекта зависимости IExpressionLexeme");
                }
            }
        }

        UnaryOperatorLexemeBase IExpressionLexeme.TakeUnaryOperatorWithLowestPriority()
        {
            if (this.IsStateE())
            {
                return this.TakeUnaryOperator();
            }
            else
            {
                if (this.IsStateF())
                {
                    return null;
                }
                else
                {
                    throw new AssertFailedException("Некорректное использование объекта зависимости IExpressionLexeme");
                }
            }
        }

        LexemeKind ILexeme.LexemeKind
        {
            get { return LexemeKind.WithVariableLength; }
        }

        IEnumerator<ILexeme> IEnumerable<ILexeme>.GetEnumerator()
        {
            return this.Content.Select(l => l).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Content.GetEnumerator();
        }

        private Boolean IsStateA()
        {
            var isPossibleStateA = this.Content.Select(l => l.GetType()).SequenceEqual
            (
                new Type[]
                {
                    typeof(aIntOperand),
                    typeof(xBinaryOperator),
                    typeof(bIntOperand),
                    typeof(yBinaryOperator),
                    typeof(TestExpressionLexeme),
                    typeof(zBinaryOperator),
                    typeof(iUnaryOperator),
                    typeof(eIntOperand)
                }
            );

            Boolean isStateA;
            if (isPossibleStateA)
            {
                var innerContent = (this.Content[4] as TestExpressionLexeme).Content;
                isStateA = innerContent.Select(l => l.GetType()).SequenceEqual
                (
                    new Type[]
                    {
                        typeof(cIntOperand),
                        typeof(qBinaryOperator),
                        typeof(dIntOperand)
                    }
                );
            }
            else
            {
                isStateA = false;
            }

            return isStateA;
        }

        private Boolean IsStateB()
        {
            var isPossibleStateB = this.Content.Select(l => l.GetType()).SequenceEqual
            (
                new Type[]
                {
                    typeof(aIntOperand),
                    typeof(xBinaryOperator),
                    typeof(bIntOperand),
                    typeof(yBinaryOperator),
                    typeof(TestExpressionLexeme)
                }
            );

            Boolean isStateB;
            if (isPossibleStateB)
            {
                var innerContent = (this.Content[4] as TestExpressionLexeme).Content;
                isStateB = innerContent.Select(l => l.GetType()).SequenceEqual
                (
                    new Type[]
                    {
                        typeof(cIntOperand),
                        typeof(qBinaryOperator),
                        typeof(dIntOperand)
                    }
                );
            }
            else
            {
                isStateB = false;
            }

            return isStateB;
        }

        private Boolean IsStateC()
        {
            var isStateC = this.Content.Select(l => l.GetType()).SequenceEqual
            (
                new Type[]
                {
                    typeof(aIntOperand),
                    typeof(xBinaryOperator),
                    typeof(bIntOperand)
                }
            );

            return isStateC;
        }

        private Boolean IsStateD()
        {
            var isStateD = this.Content.Select(l => l.GetType()).SequenceEqual
            (
                new Type[]
                {
                    typeof(cIntOperand),
                    typeof(qBinaryOperator),
                    typeof(dIntOperand)
                }
            );

            return isStateD;
        }

        private Boolean IsStateE()
        {
            var isStateE = this.Content.Select(l => l.GetType()).SequenceEqual
            (
                new Type[]
                {
                    typeof(iUnaryOperator),
                    typeof(eIntOperand)
                }
            );

            return isStateE;
        }

        private Boolean IsStateF()
        {
            if (this.Content.Length == 1)
            {
                if
                (
                    this.Content[0] is aIntOperand ||
                    this.Content[0] is bIntOperand ||
                    this.Content[0] is cIntOperand ||
                    this.Content[0] is dIntOperand ||
                    this.Content[0] is eIntOperand
                )
                {
                    return true;
                }
            }

            return false;
        }

        private void SplitFromStateA
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme,
            out IExpressionLexeme leftExpressionLexeme,
            out IExpressionLexeme rightExpressionLexeme
        )
        {
            binaryOperatorLexeme = new zBinaryOperator();
            leftExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                    {
                        new aIntOperand(),
                        new xBinaryOperator(),
                        new bIntOperand(),
                        new yBinaryOperator(),
                        new TestExpressionLexeme
                        (
                            new ILexeme[]
                            {
                                new cIntOperand(),
                                new qBinaryOperator(),
                                new dIntOperand()
                            }
                        )
                    }
            );
            rightExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                    {
                        new iUnaryOperator(),
                        new eIntOperand()
                    }
            );
        }

        private void SplitFromStateB
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme,
            out IExpressionLexeme leftExpressionLexeme,
            out IExpressionLexeme rightExpressionLexeme
        )
        {
            binaryOperatorLexeme = new yBinaryOperator();
            leftExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                {
                    new aIntOperand(),
                    new xBinaryOperator(),
                    new bIntOperand()
                }
            );
            rightExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                {
                    new cIntOperand(),
                    new qBinaryOperator(),
                    new dIntOperand()
                }
            );
        }

        private void SplitFromStateC
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme,
            out IExpressionLexeme leftExpressionLexeme,
            out IExpressionLexeme rightExpressionLexeme
        )
        {
            binaryOperatorLexeme = new xBinaryOperator();
            leftExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                {
                    new aIntOperand()
                }
            );
            rightExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                {
                    new bIntOperand()
                }
            );
        }

        private void SplitFromStateD
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme,
            out IExpressionLexeme leftExpressionLexeme,
            out IExpressionLexeme rightExpressionLexeme
        )
        {
            binaryOperatorLexeme = new qBinaryOperator();
            leftExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                {
                    new cIntOperand()
                }
            );
            rightExpressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                {
                    new dIntOperand()
                }
            );
        }

        private UnaryOperatorLexemeBase TakeUnaryOperator()
        {
            this.Content = new ILexeme[] { new eIntOperand() };
            return new iUnaryOperator();
        }
    }

    internal class TestExpressionAnalyser : IExpressionAnalyser
    {
        private Boolean alreadyCaller = false;

        IExpressionLexeme IExpressionAnalyser.Parse(LinkedList<ILexeme> lexemeList)
        {
            Assert.IsFalse(alreadyCaller, "Повторный вызов IExpressionAnalyser.Parse не допускается.");
            Assert.IsNotNull(lexemeList);

            var expectedLexemeTypeChain = new Type[]
            {
                typeof(aIntOperand),
                typeof(xBinaryOperator),
                typeof(bIntOperand),
                typeof(yBinaryOperator),
                typeof(OpeningBracketLexeme),
                typeof(cIntOperand),
                typeof(qBinaryOperator),
                typeof(dIntOperand),
                typeof(ClosingBracketLexeme),
                typeof(zBinaryOperator),
                typeof(iUnaryOperator),
                typeof(eIntOperand)
            };

            CollectionAssert.AreEqual(expectedLexemeTypeChain, lexemeList.Select(l => l.GetType()).ToArray());

            var expressionLexeme = new TestExpressionLexeme
            (
                new ILexeme[]
                {
                    new aIntOperand(),
                    new xBinaryOperator(),
                    new bIntOperand(),
                    new yBinaryOperator(),
                    new TestExpressionLexeme
                    (
                        new ILexeme[]
                        {
                            new cIntOperand(),
                            new qBinaryOperator(),
                            new dIntOperand()
                        }
                    ),
                    new zBinaryOperator(),
                    new iUnaryOperator(),
                    new eIntOperand()
                }
            );

            return expressionLexeme;
        }
    }

    internal class TestExpressionTree : IExpressionTree
    {
        private Int32 accessNumberCount = 0;

        public ExpressionTreeNode Root
        {
            get; set;
        }

        ExpressionTreeNode IExpressionTree.CreateRootNode(ILexeme value)
        {
            if (this.accessNumberCount == 0 && value.GetType() == typeof(zBinaryOperator))
            {
                this.accessNumberCount++;
                this.Root = new ExpressionTreeNode(null, new zBinaryOperator());

                return this.Root;
            }
            else
            {
                throw new AssertFailedException("Некорректное использование объекта зависимости IExpressionTree.");
            }
        }

        ExpressionTreeNode IExpressionTree.CreateChildNode
        (
            ExpressionTreeNode parentNode, 
            ParentBranchKind parentBranchKind, 
            ILexeme value
        )
        {
            Func<ILexeme, ExpressionTreeNode> CreateChildNode = l =>
            {
                var newNode = new ExpressionTreeNode(parentNode, l);
                if (parentBranchKind == ParentBranchKind.Left)
                {
                    parentNode.LeftChild = newNode;
                }
                else
                {
                    parentNode.RightChild = newNode;
                };

                this.accessNumberCount ++;
                return newNode;
            };

            if 
            (
                this.accessNumberCount == 1 && 
                parentNode.Value.GetType() == typeof(zBinaryOperator) && 
                parentBranchKind == ParentBranchKind.Left && 
                value.GetType() == typeof(yBinaryOperator)
            )
            {
                return CreateChildNode(new yBinaryOperator());
            }
            else if
            (
                this.accessNumberCount == 2 && 
                parentNode.Value.GetType() == typeof(yBinaryOperator) && 
                parentBranchKind == ParentBranchKind.Left && 
                value.GetType() == typeof(xBinaryOperator)
            )
            {
                return CreateChildNode(new xBinaryOperator());
            }
            else if
            (
                this.accessNumberCount == 3 &&
                parentNode.Value.GetType() == typeof(xBinaryOperator) &&
                parentBranchKind == ParentBranchKind.Left &&
                value.GetType() == typeof(aIntOperand)
            )
            {
                return CreateChildNode(new aIntOperand());
            }
            else if
            (
                this.accessNumberCount == 4 &&
                parentNode.Value.GetType() == typeof(xBinaryOperator) &&
                parentBranchKind == ParentBranchKind.Right &&
                value.GetType() == typeof(bIntOperand)
            )
            {
                return CreateChildNode(new bIntOperand());
            }
            else if
            (
                this.accessNumberCount == 5 &&
                parentNode.Value.GetType() == typeof(yBinaryOperator) &&
                parentBranchKind == ParentBranchKind.Right &&
                value.GetType() == typeof(qBinaryOperator)
            )
            {
                return CreateChildNode(new qBinaryOperator());
            }
            else if
            (
                this.accessNumberCount == 6 &&
                parentNode.Value.GetType() == typeof(qBinaryOperator) &&
                parentBranchKind == ParentBranchKind.Left &&
                value.GetType() == typeof(cIntOperand)
            )
            {
                return CreateChildNode(new cIntOperand());
            }
            else if
            (
                this.accessNumberCount == 7 &&
                parentNode.Value.GetType() == typeof(qBinaryOperator) &&
                parentBranchKind == ParentBranchKind.Right &&
                value.GetType() == typeof(dIntOperand)
            )
            {
                return CreateChildNode(new dIntOperand());
            }
            else if
            (
                this.accessNumberCount == 8 &&
                parentNode.Value.GetType() == typeof(zBinaryOperator) &&
                parentBranchKind == ParentBranchKind.Right &&
                value.GetType() == typeof(iUnaryOperator)
            )
            {
                return CreateChildNode(new iUnaryOperator());
            }
            else if
            (
                this.accessNumberCount == 9 &&
                parentNode.Value.GetType() == typeof(iUnaryOperator) &&
                parentBranchKind == ParentBranchKind.Right &&
                value.GetType() == typeof(eIntOperand)
            )
            {
                return CreateChildNode(new eIntOperand());
            }
            else
            {
                throw new AssertFailedException("Некорректное использование объекта зависимости IExpressionTree.");
            }
        }

        NumberLexemeBase IExpressionTree.Calculate()
        {
            throw new NotImplementedException();
        }
    }

    public class AnotherTestExpressionLexeme : IExpressionLexeme
    {
        private LinkedList<ILexeme> content = new LinkedList<ILexeme>();

        public AnotherTestExpressionLexeme()
        {
            this.content.AddLast(new iUnaryOperator());
        }

        public LexemeKind LexemeKind
        {
	        get { return LexemeKind.WithVariableLength; }
        }

        public IEnumerator<ILexeme> GetEnumerator()
        {
 	        return content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
 	        return (content as IEnumerable).GetEnumerator();
        }

        void IExpressionLexeme.SplitExpressionByBinaryOperatorWithLowestPriority
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme, 
            out IExpressionLexeme leftExpressionLexeme, 
            out IExpressionLexeme rightExpressionLexeme
        )
        {
 	        binaryOperatorLexeme = null;
            leftExpressionLexeme = null;
            rightExpressionLexeme = null;
        }

        UnaryOperatorLexemeBase IExpressionLexeme.TakeUnaryOperatorWithLowestPriority()
        {
 	        if (this.content.Count > 0)
            {
                var takedLexeme = this.content.First.Value as UnaryOperatorLexemeBase;
                this.content.Clear();
                return takedLexeme;
            }
            else
            {
                return null;
            }
        }
    }
    #endregion

    [TestClass]
    public class SyntaxAnalyserTests
    {
        [TestMethod]
        public void ValidExpressionParseTest()
        {
            // Arrange
            var testExpressionAnalyser = new TestExpressionAnalyser();
            var testExpressionTree = new TestExpressionTree();

            ISyntaxAnalyser syntaxAnalyser = new SyntaxAnalyser(testExpressionAnalyser, testExpressionTree);

            var lexemeLinkedList = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    new aIntOperand(),
                    new xBinaryOperator(),
                    new bIntOperand(),
                    new yBinaryOperator(),
                    new OpeningBracketLexeme(),
                    new cIntOperand(),
                    new qBinaryOperator(),
                    new dIntOperand(),
                    new ClosingBracketLexeme(),
                    new zBinaryOperator(),
                    new iUnaryOperator(),
                    new eIntOperand()
                }
            );

            // Act
            var resultedExpressionTree = syntaxAnalyser.Parse(lexemeLinkedList);

            // Assert
            Assert.IsNotNull(resultedExpressionTree);
            Assert.AreEqual(resultedExpressionTree, testExpressionTree);
            
            var rootNode = testExpressionTree.Root;
            
            Assert.IsNotNull(rootNode);
            Assert.IsNotNull(rootNode.Value);
            Assert.IsInstanceOfType(rootNode.Value, typeof(zBinaryOperator));

            Assert.IsNotNull(rootNode.LeftChild);
            Assert.IsNotNull(rootNode.LeftChild.Value);
            Assert.IsInstanceOfType(rootNode.LeftChild.Value, typeof(yBinaryOperator));

            Assert.IsNotNull(rootNode.RightChild);
            Assert.IsNotNull(rootNode.RightChild.Value);
            Assert.IsInstanceOfType(rootNode.RightChild.Value, typeof(iUnaryOperator));

            Assert.IsNotNull(rootNode.LeftChild.LeftChild);
            Assert.IsNotNull(rootNode.LeftChild.LeftChild.Value);
            Assert.IsInstanceOfType(rootNode.LeftChild.LeftChild.Value, typeof(xBinaryOperator));

            Assert.IsNotNull(rootNode.LeftChild.RightChild);
            Assert.IsNotNull(rootNode.LeftChild.RightChild.Value);
            Assert.IsInstanceOfType(rootNode.LeftChild.RightChild.Value, typeof(qBinaryOperator));

            Assert.IsNotNull(rootNode.RightChild);
            Assert.IsNotNull(rootNode.RightChild.Value);
            Assert.IsInstanceOfType(rootNode.RightChild.Value, typeof(iUnaryOperator));


            Assert.IsNotNull(rootNode.LeftChild.LeftChild.LeftChild.Value);
            Assert.IsNotNull(rootNode.LeftChild.LeftChild.RightChild.Value);
            Assert.IsNotNull(rootNode.LeftChild.RightChild.LeftChild.Value);
            Assert.IsNotNull(rootNode.LeftChild.RightChild.RightChild.Value);
            Assert.IsNotNull(rootNode.RightChild.RightChild.Value);

            var aLexeme = rootNode.LeftChild.LeftChild.LeftChild.Value;
            var bLexeme = rootNode.LeftChild.LeftChild.RightChild.Value;
            var cLexeme = rootNode.LeftChild.RightChild.LeftChild.Value;
            var dLexeme = rootNode.LeftChild.RightChild.RightChild.Value;
            var eLexeme = rootNode.RightChild.RightChild.Value;

            Assert.IsNotNull(aLexeme);
            Assert.IsInstanceOfType(aLexeme, typeof(aIntOperand));

            Assert.IsNotNull(bLexeme);
            Assert.IsInstanceOfType(bLexeme, typeof(bIntOperand));

            Assert.IsNotNull(cLexeme);
            Assert.IsInstanceOfType(cLexeme, typeof(cIntOperand));

            Assert.IsNotNull(dLexeme);
            Assert.IsInstanceOfType(dLexeme, typeof(dIntOperand));

            Assert.IsNotNull(eLexeme);
            Assert.IsInstanceOfType(eLexeme, typeof(eIntOperand));
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionDoesNotContainOperandLexemeException))]
        public void MissingOperandTest()
        {
            // Arrange
            var testExpressionAnalyserMock = new Mock<IExpressionAnalyser>();
            testExpressionAnalyserMock.Setup(m => m.Parse(It.IsAny<LinkedList<ILexeme>>())).Returns<LinkedList<ILexeme>>
            (
                lexemeLinkedListParameter =>
                {
                    Assert.IsNotNull(lexemeLinkedListParameter);
                    Assert.AreEqual(1, lexemeLinkedListParameter.Count);
                    Assert.IsNotNull(lexemeLinkedListParameter.First.Value);
                    Assert.IsInstanceOfType(lexemeLinkedListParameter.First.Value, typeof(iUnaryOperator));

                    return new AnotherTestExpressionLexeme();
                }
            );

            var testExpressionTreeMock = new Mock<IExpressionTree>(MockBehavior.Strict);

            ISyntaxAnalyser syntaxAnalyser = new SyntaxAnalyser
            (
                testExpressionAnalyserMock.Object, testExpressionTreeMock.Object
            );

            var lexemeLinkedList = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    new iUnaryOperator()
                }
            );

            // Act
            var resultedExpressionTree = syntaxAnalyser.Parse(lexemeLinkedList);
        }
    }
}
