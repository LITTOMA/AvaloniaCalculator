using AvaloniaCalculator.Calculator;

namespace AvaloniaCalculator.UnitTest;

[TestFixture]
public class CalculatorTests
{
    private Calculator.Calculator _calculator;

    [SetUp]
    public void Setup()
    {
        // 在每个测试前初始化Calculator实例
        _calculator = new();
    }

    [Test]
    public void Test_Addition()
    {
        var expression = new BinaryExpression(new NumberExpression(5), '+', new NumberExpression(3));
        var result = _calculator.Evaluate(expression);
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void Test_Subtraction()
    {
        var expression = new BinaryExpression(new NumberExpression(5), '-', new NumberExpression(3));
        var result = _calculator.Evaluate(expression);
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Test_Multiplication()
    {
        var expression = new BinaryExpression(new NumberExpression(5), '*', new NumberExpression(3));
        var result = _calculator.Evaluate(expression);
        Assert.That(result, Is.EqualTo(15));
    }

    [Test]
    public void Test_Division()
    {
        var expression = new BinaryExpression(new NumberExpression(6), '/', new NumberExpression(3));
        var result = _calculator.Evaluate(expression);
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Test_DivideByZero()
    {
        var expression = new BinaryExpression(new NumberExpression(5), '/', new NumberExpression(0));
        Assert.Throws<DivideByZeroException>(() => _calculator.Evaluate(expression));
    }

    [Test]
    public void Test_SinFunction()
    {
        var expression = new FunctionExpression("sin", new NumberExpression(Math.PI / 2));
        var result = _calculator.Evaluate(expression);
        Assert.That(result, Is.EqualTo(1).Within(0.0001));
    }

    [TestCase("0", 1)]
    [TestCase("0.52359877559", 0.8660)] // Math.PI / 6
    [TestCase("0.78539816339", 0.7071)] // Math.PI / 4
    [TestCase("1.0471975512", 0.5)] // Math.PI / 3
    [TestCase("1.57079632679", 0)] // Math.PI / 2
    public void Test_CosFunction(string input, double expected)
    {
        var inputRadians = double.Parse(input);
        var expression = new FunctionExpression("cos", new NumberExpression(inputRadians));
        var result = _calculator.Evaluate(expression);
        Assert.That(result, Is.EqualTo(expected).Within(0.0001));
    }

    [Test]
    public void Test_UnaryNegation()
    {
        var expression = new UnaryExpression("-", new NumberExpression(5));
        var result = _calculator.Evaluate(expression);
        Assert.That(result, Is.EqualTo(-5));
    }
}


[TestFixture]
public class ParserTests
{
    [Test]
    public void TestParseInteger()
    {
        var parser = new Parser("42");
        var result = parser.Parse() as NumberExpression;
        Assert.IsNotNull(result);
        Assert.That(result.Value, Is.EqualTo(42));
    }

    [Test]
    public void TestParseFloat()
    {
        var parser = new Parser("3.14");
        var result = parser.Parse() as NumberExpression;
        Assert.IsNotNull(result);
        Assert.AreEqual(3.14, result.Value, 0.001);
    }

    [Test]
    public void TestSimpleAddition()
    {
        var parser = new Parser("1 + 2");
        var result = parser.Parse() as BinaryExpression;
        Assert.IsNotNull(result);
        Assert.That(result.Operator, Is.EqualTo('+'));
        Assert.That((result.Left as NumberExpression).Value, Is.EqualTo(1));
        Assert.That((result.Right as NumberExpression).Value, Is.EqualTo(2));
    }

    [Test]
    public void TestSimpleMultiplication()
    {
        var parser = new Parser("4 × 2");
        var result = parser.Parse() as BinaryExpression;
        Assert.IsNotNull(result);
        Assert.That(result.Operator, Is.EqualTo('*')); // Assuming the parser converts × to *
        Assert.That((result.Left as NumberExpression).Value, Is.EqualTo(4));
        Assert.That((result.Right as NumberExpression).Value, Is.EqualTo(2));
    }

    [Test]
    public void TestFunctionSin()
    {
        var parser = new Parser("sin(0)");
        var result = parser.Parse() as FunctionExpression;
        Assert.IsNotNull(result);
        Assert.That(result.FunctionName, Is.EqualTo("sin"));
        Assert.That((result.Argument as NumberExpression).Value, Is.EqualTo(0));
    }

    [Test]
    public void TestInvalidInput()
    {
        var parser = new Parser("invalid");
        Assert.Throws<InvalidOperationException>(() => parser.Parse());
    }

    // Add more tests as needed to cover more cases and complex expressions
    [Test]
    public void TestComplexExpression()
    {
        var parser = new Parser("sin(π ÷ 2) + 3 × 4");
        var result = parser.Parse() as BinaryExpression;
        Assert.IsNotNull(result);
        Assert.That(result.Operator, Is.EqualTo('+'));
        Assert.That((result.Left as FunctionExpression).FunctionName, Is.EqualTo("sin"));
        Assert.That((result.Right as BinaryExpression).Operator, Is.EqualTo('*'));
    }

    [Test]
    public void TestInvalidExpression()
    {
        var parser = new Parser("sin(π ÷ 2) + 3 ×");
        Assert.Throws<InvalidOperationException>(() => parser.Parse());
    }

    [Test]
    public void TestInvalidExpression2()
    {
        var parser = new Parser("sin(π ÷ 2) + 3 × 4 +");
        Assert.Throws<InvalidOperationException>(() => parser.Parse());
    }
}

