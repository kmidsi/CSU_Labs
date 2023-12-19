using System;


class Program
{
    static void Main()
    {
        Console.Write("Введите математическое выражение: ");
        string expression = Console.ReadLine();
        List<object> expressionToWork = ExpressionToWork(expression);

        List<object> rpn = ConvertToRPN(expressionToWork);
        Console.WriteLine($"ОПЗ: {string.Join(" ", rpn)}");
        double result = Calculate(rpn);
        Console.WriteLine($"Результат: {result}");
    }

    static List<object> ExpressionToWork(string expression) // убирает пробелы из введенного выражения и "склеивает" многозначные числа
    {
        List<object> result = new List<object>();
        string num = "";

        foreach (char symbol in expression)
        {
            if (symbol != ' ')
            {
                if (!char.IsDigit(symbol))
                {
                    if (num != "") result.Add(num);
                    result.Add(symbol);
                    num = "";
                }
                else
                {
                    num += symbol;
                }
            }
        }

        if (num != "") result.Add(num);

        return result;
    }

    static int Priority(object operation) // определяет приоритет операций
    {
        switch (operation)
        {
            case '+': case '-': return 1;
            case '*': case '/': return 2;
            default: return 0;
        }
    }

    static List<object> ConvertToRPN(List<object> expression) // преобразует выражение в опз
    {
        Stack<object> operations = new Stack<object>();
        List<object> result = new List<object>();

        foreach (object symbol in expression)
        {
            if (symbol is string)
            {
                result.Add(symbol);
            }
            else if (symbol.Equals('+') || symbol.Equals('-') || symbol.Equals('*') || symbol.Equals('/'))
            {
                while (operations.Count > 0 && Priority(operations.Peek()) >= Priority(symbol))
                {
                    result.Add(operations.Pop());
                }
                operations.Push(symbol);
            }
            else if (symbol.Equals('('))
            {
                operations.Push(symbol);
            }
            else if (symbol.Equals(')'))
            {
                while (operations.Count > 0 && !operations.Peek().Equals('('))
                {
                    result.Add(operations.Pop());
                }
                operations.Pop();
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

    static double Calculate(List<object> rpn) // считает итоговое выражение
    {

        Stack<double> result = new();

        foreach (object symbol in rpn)
        {
            if (symbol is string)
            {
                result.Push(Convert.ToDouble(symbol));
            }
            else if (symbol is char)
            {
                double secondNum = result.Pop();
                double firstNum = result.Pop();
                char operation = (char)symbol;
                double resultedNum = CountNumber(operation, firstNum, secondNum);
                result.Push(resultedNum);
            }

        }

        return result.Pop();
    }
}