namespace AvaloniaCalculator.Calculator
{
    public record BinaryExpression(Expression Left, char Operator, Expression Right) : Expression;
}
