namespace AvaloniaCalculator.Calculator;

public record UnaryExpression(string Operator, Expression Operand) : Expression;
