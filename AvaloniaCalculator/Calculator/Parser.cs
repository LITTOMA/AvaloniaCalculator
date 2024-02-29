using AvaloniaCalculator.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AvaloniaCalculator.Calculator;

public class Parser
{
    private readonly Queue<string> tokens;
    private bool hasError = false;

    public Parser(string input)
    {
        if (!Regex.IsMatch(input, @"^[\dπ\.×÷\+\-\(\)\s\^%]+|sin|cos|tan|log$"))
        {
            Console.WriteLine("Error: Input contains illegal characters.");
            hasError = true;
            tokens = new Queue<string>();
            return;
        }

        var regex = new Regex(@"(\d+(\.\d+)?|\×|\÷|\+|\-|\(|\)|π|sin|cos|tan|log)");
        tokens = new Queue<string>(regex.Matches(input).Select(m => m.Value));

        if (!IsBalanced(input))
        {
            Console.WriteLine("Error: Parentheses are not balanced.");
            hasError = true;
        }
    }

    private static bool IsBalanced(string input)
    {
        var balance = 0;
        foreach (var ch in input)
        {
            if (ch == '(') balance++;
            else if (ch == ')') balance--;

            if (balance < 0) return false;
        }
        return balance == 0;
    }

    public Expression Parse()
    {
        if (hasError) throw new InvalidOperationException("Parsing error occurred.");
        return ParseExpression();
    }

    private Expression ParseExpression()
    {
        var expression = ParseTerm();

        while (tokens.Count > 0 && (tokens.Peek() == "+" || tokens.Peek() == "-"))
        {
            var op = tokens.Dequeue();
            var right = ParseTerm();
            expression = new BinaryExpression(expression, op[0], right);
        }

        return expression;
    }

    private Expression ParseTerm()
    {
        var term = ParseFactor();

        while (tokens.Count > 0 && (tokens.Peek() == "×" || tokens.Peek() == "÷"))
        {
            var op = tokens.Dequeue();
            var right = ParseFactor();
            term = new BinaryExpression(term, op == "×" ? '*' : '/', right);
        }

        return term;
    }

    private Expression ParseFactor()
    {
        if (tokens.Peek() == "(")
        {
            tokens.Dequeue(); // 移除左括号
            var expression = ParseExpression();
            tokens.Dequeue(); // 移除右括号
            return expression;
        }
        else if (tokens.Peek() == "π")
        {
            tokens.Dequeue(); // 移除π
            return new NumberExpression(Math.PI);
        }
        else if (new[] { "sin", "cos", "tan", "log" }.Contains(tokens.Peek()))
        {
            var functionName = tokens.Dequeue();
            tokens.Dequeue(); // 假设'('紧跟在函数名后面
            var argument = ParseExpression();
            tokens.Dequeue(); // 移除')'
            return new FunctionExpression(functionName, argument);
        }
        else
        {
            return new NumberExpression(double.Parse(tokens.Dequeue()));
        }
    }

}
