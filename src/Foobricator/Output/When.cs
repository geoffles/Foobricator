using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Sources;
using Foobricator.Tools;

namespace Foobricator.Output
{
    /// <summary>
    /// Encapusulates a condition
    /// </summary>
    public class When : IDebugInfoProvider
    {
        /// <summary>
        /// Logical condition operators
        /// </summary>
        /// <remarks>
        /// Gt,Eq and Lt are combined as an OR, while NOT is combined as an AND.
        /// i.e. NOT (EQ or GT or LT)
        /// </remarks>
        [Flags]
        public enum Op
        {
            /// <summary>
            /// Test equal
            /// </summary>
            Eq = 1<<1,
            /// <summary>
            /// Test greater than
            /// </summary>
            Gt = 1<<2,
            /// <summary>
            /// Test less than
            /// </summary>
            Lt = 1<<3,
            /// <summary>
            /// Logically not all other ops
            /// </summary>
            Not = 1<<4
        }

        /// <summary>
        /// The target object to evaluate against
        /// </summary>
        public readonly ISource Source;

        /// <summary>
        /// The value to compare <c>Source</c> to
        /// </summary>
        public readonly object RightHandSide;

        /// <summary>
        /// Logical operator. Can be combination.
        /// </summary>
        public readonly Op Operator;

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Initialise a new instance against a reference
        /// </summary>
        public When(DataReference reference, Op @operator, object rightHandSide)
            :this(reference.Dereference() as ISource, @operator, rightHandSide)
        {
        }

        /// <summary>
        /// Initalise a new instance against a source
        /// </summary>
        public When(ISource source, Op @operator, object rightHandSide)
        {
            Source = source;
            RightHandSide = rightHandSide;
            Operator = @operator;
        }

        /// <summary>
        /// Tests the conditions and returns the result
        /// </summary>
        public bool True()
        {
            object value = Source.GetItem();

            if (value is IList<object>)
            {
                value = ((IList<object>) value)[0];
            }

            if (value is string)
            {
                return StringCompare((string)value);
            }

            if (value is int)
            {
                return IntCompare((int) value);
            }

            if (value is decimal)
            {
                return DecimalCompare((decimal) value);
            }
            
            return false;
        }

        private bool DecimalCompare(decimal value)
        {
            Func<bool> comp = () => false;
            if (Operator.HasFlag(Op.Eq))
            {
                var comp1 = comp;
                comp = () => comp1() || (value == (decimal) RightHandSide);
            }
            if (Operator.HasFlag(Op.Lt))
            {
                var comp1 = comp;
                comp = () => comp1() || (value < (decimal)RightHandSide);
            }
            if (Operator.HasFlag(Op.Gt))
            {
                var comp1 = comp;
                comp = () => comp1() || (value > (decimal)RightHandSide);
            }
            if (Operator.HasFlag(Op.Not))
            {
                var comp1 = comp;
                comp = () => !comp1();
            }

            return comp();
        }

        private bool IntCompare(int value)
        {
            Func<bool> comp = () => false;
            if (Operator.HasFlag(Op.Eq))
            {
                var comp1 = comp;
                comp = () => comp1() || (value == (int)RightHandSide);
            }
            if (Operator.HasFlag(Op.Lt))
            {
                var comp1 = comp;
                comp = () => comp1() || (value < (int)RightHandSide);
            }
            if (Operator.HasFlag(Op.Gt))
            {
                var comp1 = comp;
                comp = () => comp1() || (value > (int)RightHandSide);
            }
            if (Operator.HasFlag(Op.Not))
            {
                var comp1 = comp;
                comp = () => !comp1();
            }

            return comp();
        }

        private bool StringCompare(string value)
        {
            var compareResult = String.Compare(value, (string) RightHandSide, StringComparison.Ordinal);
            Func<bool> comp = () => false;
            if (Operator.HasFlag(Op.Eq))
            {
                var comp1 = comp;
                comp = () => comp1() || (compareResult == 0);
            }
            if (Operator.HasFlag(Op.Lt))
            {
                var comp1 = comp;
                comp = () => comp1() || (compareResult < 0);
            }
            if (Operator.HasFlag(Op.Gt))
            {
                var comp1 = comp;
                comp = () => comp1() || (compareResult > 0);
            }
            if (Operator.HasFlag(Op.Not))
            {
                var comp1 = comp;
                comp = () => !comp1();
            }

            return comp();
        }

        
    }
}
