using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Sources;
using Foobricator.Tools;

namespace Foobricator.Output
{
    public class When : IDebugInfoProvider
    {
        [Flags]
        public enum Op
        {
            Eq = 1<<1,
            Gt = 1<<2,
            Lt = 1<<3,
            Not = 1<<4
        }

        public readonly ISource Source;

        public readonly object RightHandSide;

        public readonly Op Operator;

        public DebugInfo DebugInfo { get; set; }

        public When(DataReference reference, Op @operator, object rightHandSide)
            :this(reference.Dereference() as ISource, @operator, rightHandSide)
        {
        }

        public When(ISource source, Op @operator, object rightHandSide)
        {
            Source = source;
            RightHandSide = rightHandSide;
            Operator = @operator;
        }

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
