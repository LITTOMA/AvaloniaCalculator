namespace AvaloniaCalculator.Calculator
{
    public record FunctionExpression(string FunctionName, Expression Argument) : Expression;
}
