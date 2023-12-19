using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalcOOP;


class Program
{
    static void Main()
    {
        Console.Write("Введите математическое выражение: ");
        string expression = Console.ReadLine();
        List<Token> expressionToWork = ExpressionToWork(expression);
        List<Token> rpn = ConvertToRPN(expressionToWork);
        double result = Calculate(rpn);
        Console.WriteLine($"Результат: {result}");
    }

    static List<Token> ExpressionToWork(string expression) // убирает пробелы из введенного выражения и "склеивает" многозначные числа
    {
        List<Token> result = new ();
        string num = "";

        foreach (char symbol in expression)
        {
            if (char.IsDigit(symbol) || symbol == ',') // Вроде как подружил с дробями
            {
                num += symbol;
            }
            else
            {
                if (num != "")
                {
                    Number tempNum = new() { value = double.Parse(num) };
                    result.Add(tempNum);
                    num = "";
                }
                if (symbol == '+' || symbol == '-' || symbol == '*' || symbol == '/')
                {
                    Operation op = new() { symbol = symbol };
                    result.Add(op);
                }
                else if (symbol == '(' || symbol == ')')
                {
                    Parenthesis bracket = new();
                    if (symbol == '(')
                    {
                        bracket.opening = true;
                    }
                    result.Add(bracket);
                }
            }
        }

        if (num != "") 
        {
            Number tempNum = new() { value = double.Parse(num) };
            result.Add(tempNum);
        }


        return result;
    }

    static int Priority(Token operation) // определяет приоритет операций
    {
        if (operation is Operation op)
        {
            switch (op.symbol)
            {
                case '+': case '-': return 1;
                case '*': case '/': return 2;
                default: return 0;
            }
        }
        return 0;
    }

    static List<Token> ConvertToRPN(List<Token> expression) // преобразует выражение в опз
    {
        Stack<Token> operations = new();
        List<Token> result = new();

        foreach (Token symbol in expression)
        {
            if (symbol is Number number)
            {
                result.Add(number);
            }
            else if (symbol is Operation operation)
            {
                while (operations.Count > 0 && Priority(operations.Peek()) >= Priority(symbol))
                {
                    result.Add(operations.Pop());
                }
                operations.Push(operation);
            }
            else if (symbol is Parenthesis bracket)
            {
                if (bracket.opening)
                {
                    operations.Push(bracket);
                }
                else
                {
                    while (operations.Count > 0 && operations.Peek() is not Parenthesis)
                    {
                        result.Add(operations.Pop());
                    }
                    operations.Pop();
                }
            }
        }
        while (operations.Count > 0)
        {
            result.Add(operations.Pop());
        }

        return result;
    }

    static double CountNumber(object operation, double fisrtNum, double secondNum) // подсчитывает число
    {
        switch (operation)
        {
            case '+': return fisrtNum + secondNum;
            case '-': return fisrtNum - secondNum;
            case '*': return fisrtNum * secondNum;
            case '/': return fisrtNum / secondNum;
            default: return 0;
        }
    }

    static double Calculate(List<Token> rpn) // считает итоговое выражение
    {

        Stack<double> result = new();

        foreach (Token symbol in rpn)
        {
            if (symbol is Number num)
            {
                result.Push(num.value);
            }
            else if (symbol is Operation operation)
            {
                double secondNum = result.Pop();
                double firstNum = result.Pop();
                char operationTemp = operation.symbol;
                double resultedNum = CountNumber(operationTemp, firstNum, secondNum);
                result.Push(resultedNum);
            }

        }

        return result.Pop();
    }
}