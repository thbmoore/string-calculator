using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StringCalculatorProject
{
    public class StringCalculator
    {
        public static void Main(string[] args)
        {
            // Concatenate arguments into a single string input.
            string expression = String.Join("", args);

            // Remove spaces
            expression = expression.Replace(" ", "");

            // Validations
            if (ValidateInput(expression))
            {
                try
                {
                    int result = Evaluate(expression);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception evaluating expression: {0}", e.Message);
                }
            }
        }

        public static bool ValidateInput(string expression)
        {
            // Verify that the input only contains numbers and operators.
            bool validInput = true;
            if (string.IsNullOrEmpty(expression))
            {
                Console.WriteLine("Input expression is empty");
                validInput = false;
            }

            if (validInput)
            {
                Regex rgx = new Regex(@"[^0-9\*/\+\-\(\)]");
                MatchCollection matches = rgx.Matches(expression);
                if (matches.Count > 0)
                {
                    validInput = false;
                    Console.WriteLine("Input expression contains invalid characters:");
                    foreach (Match match in matches)
                        Console.WriteLine("   " + match.Value);
                }
            }

            if (validInput)
            {
                // Validate matching parentheses
                int countOpen = expression.Split('(').Length - 1;
                int countClose = expression.Split(')').Length - 1;
                if (countOpen != countClose)
                {
                    validInput = false;
                    Console.WriteLine("Input expression contains mismatched parentheses");
                }
            }
            return validInput;
        }

        public static int Evaluate(string expression)
        {
            // Evaluate the expression
            Console.WriteLine(expression);

            // Add an outer set of parentheses
            expression = string.Format("({0})", expression);

            // Locate and Evaluate each set of parentheses from inner to outer. 
            bool moreParens = true;
            while (moreParens)
            {
                int lastOpenParens = 0;
                int firstCloseParens = 0;
                for (int j = 0; j < expression.Length; j++)
                {
                    if (expression[j] == '(')
                    {
                        lastOpenParens = j;
                    }
                    else if (expression[j] == ')')
                    {
                        firstCloseParens = j;
                        break;
                    }
                }
                if (firstCloseParens > 0)
                {
                    // Extract the substring in the inner-most set of parentheses
                    string sub = expression.Substring(lastOpenParens, firstCloseParens - lastOpenParens + 1);
                    // Calculate the expression in parentheses
                    string result = Calculate(sub);
                    // Replace the calculated substring in the original expression
                    expression = expression.Replace(sub, result);
                    Console.WriteLine(string.Format(" = {0}", expression));
                }
                else
                {
                    moreParens = false;
                }
            }
            return Convert.ToInt32(expression);
        }

        public static bool IsOperator(string s)
        {
            // Check if the input string is an operator.
            if (s.Length == 1 && "*/+-".Contains(s))
            {
                return true;
            }
            return false;
        }

        public static List<string> HandleUnaryOperators(List<string> elements)
        {
            // Signs should be considerd part of the operand, not an operator. 
            // Identify two cases:
            // - The first character is a sign followed by a number. Example: -5*2
            // - A sign is preceded by an operator and followed by a number. Example: 5*-2

            int number;
            List<string> newElements = new List<string>();
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i] == "-" || elements[i] == "+")
                {
                    if (i == 0 && elements.Count > 1)
                    {
                        if (Int32.TryParse(elements[1], out number))
                        {
                            newElements.Add(string.Format("{0}{1}", elements[i], elements[i + 1]));
                            i++;
                        }
                    }
                    else if ((i > 0) && IsOperator(elements[i - 1]) &&
                        elements.Count > (i + 1) && Int32.TryParse(elements[i + 1], out number))
                    {
                        newElements.Add(string.Format("{0}{1}", elements[i], elements[i + 1]));
                        i++;
                    }
                    else
                    {
                        newElements.Add(elements[i]);
                    }
                }
                else
                {
                    newElements.Add(elements[i]);
                }
            }
            return newElements;
        }

        public static string ApplyOperator(string op, string operand1, string operand2)
        {
            // Performs an operation on two operands
            int int1 = Convert.ToInt32(operand1);
            int int2 = Convert.ToInt32(operand2);
            int result = 0;
            if (op == "*")
            {
                result = int1 * int2;
            }
            else if (op == "/")
            {
                result = int1 / int2;
            }
            else if (op == "+")
            {
                result = int1 + int2;
            }
            else if (op == "-")
            {
                result = int1 - int2;
            }
            return result.ToString();
        }

        public static string Calculate(string expression)
        {
            if (String.IsNullOrEmpty(expression))
            {
                throw new Exception("Can not calculate empty expression");
            }

            // Remove parentheses
            expression = expression.Replace("(", "");
            expression = expression.Replace(")", "");

            if (String.IsNullOrEmpty(expression))
            {
                throw new Exception("Can not calculate empty expression");
            }

            // Add pipes to the operators to allow splitting the expression
            // while retaining the operators.
            expression = expression.Replace("*", "|*|");
            expression = expression.Replace("/", "|/|");
            expression = expression.Replace("+", "|+|");
            expression = expression.Replace("-", "|-|");

            // Split the expression into its elements (operators and operands)
            string[] elementArray = expression.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            // Convert the array to a list 
            List<string> elements = new List<string>(elementArray);

            // Handle + and - signs
            elements = HandleUnaryOperators(elements);

            // Apply operators in order of precedence
            string[] operatorGroups = { "*/", "+-" };
            foreach (string opGroup in operatorGroups)
            {
                bool opFound = true;
                while (opFound && elements.Count > 1)
                {
                    opFound = false;
                    List<string> newElements = new List<string>();
                    for (int i = 0; i < elements.Count; i++)
                    {
                        if (!opFound && opGroup.Contains(elements[i]))
                        {
                            // This is an operator with the current precedence. 
                            // Apply the operator to adjacent operands.
                            opFound = true;
                            if (i > 0)
                            {
                                string result = ApplyOperator(elements[i], elements[i - 1], elements[i + 1]);
                                newElements.Add(result);
                                i++;
                            }
                        }
                        else
                        {
                            if (!opFound && elements.Count > i + 1 && opGroup.Contains(elements[i + 1]))
                            {
                                // The next entry is an operator with the current precedence. 
                                // We'll handle this entry on the next loop.
                                continue;
                            }
                            else
                            {
                                // We've either already procesed an operator this loop
                                // or the following item is not an operator with the current precedence.
                                // Copy the item to the updated list.
                                newElements.Add(elements[i]);
                            }
                        }
                    }
                    elements = newElements;
                }
            }
            return elements[0];
        }
    }
}
