using System;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaCalculator.Calculator
{
    public class Calculator
    {
        public double Evaluate(Expression expression)
        {
            return expression switch
            {
                NumberExpression n => n.Value,
                BinaryExpression b => EvaluateBinaryExpression(b),
                UnaryExpression u => EvaluateUnaryExpression(u),
                FunctionExpression f => EvaluateFunctionExpression(f),
                _ => throw new ArgumentException("Unsupported expression", nameof(expression)),
            };
        }

        private double EvaluateBinaryExpression(BinaryExpression b)
        {
            var left = Evaluate(b.Left);
            var right = Evaluate(b.Right);
            return b.Operator switch
            {
                '+' => left + right,
                '-' => left - right,
                '*' => left * right,
                '/' => right != 0 ? left / right : throw new DivideByZeroException(),
                '^' => Math.Pow(left, right),
                _ => throw new ArgumentException($"Unsupported operator {b.Operator}", nameof(b)),
            };
        }

        private double EvaluateUnaryExpression(UnaryExpression u)
        {
            var operand = Evaluate(u.Operand);
            return u.Operator switch
            {
                "+" => +operand,
                "-" => -operand,
                _ => throw new ArgumentException($"Unsupported unary operator {u.Operator}", nameof(u)),
            };
        }

        private double EvaluateFunctionExpression(FunctionExpression f)
        {
            var argument = Evaluate(f.Argument);
            return f.FunctionName.ToLower() switch
            {
                "sin" => Math.Sin(argument),
                "cos" => Math.Cos(argument),
                "tan" => Math.Tan(argument),
                "log" => Math.Log(argument),
                _ => throw new ArgumentException($"Unsupported function {f.FunctionName}", nameof(f)),
            };
        }
    }

}
