using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        string expression = string.Empty;
        string temp = string.Empty;
        bool spec = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            expression = expression + (sender as Button).Content.ToString();
            Expression.Text = expression;
        }

        private void Simple_Click(object sender, RoutedEventArgs e)
        {
            expression = expression + (sender as Button).Content.ToString();
            Expression.Text = expression;
        }

        private void GetResult_Click(object sender, RoutedEventArgs e)
        {
            expression = expression.Replace(',', '.');

            if (spec)
            {
                spec = false;
                expression = temp + expression + ")";
            }

            FormatResult(Evaluate(expression).ToString());
        }

        private double Evaluate(string expression)
        {
            CodeDomProvider provider = new CSharpCodeProvider();
            string soursecode = string.Empty;
            soursecode += @"using System;
            class Program 
            { 
               public static void Main() { }
               public static double Result() { return " + expression + @"; }
            }";
            CompilerParameters param = new CompilerParameters();
            param.MainClass = "Program";
            CompilerResults result = provider.CompileAssemblyFromSource(param, soursecode);
            Type cl = result.CompiledAssembly.GetType("Program");
            MethodInfo mi = cl.GetMethod("Result");
            return (double)mi.Invoke(null, null);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            expression = "";
            Expression.Text = "";
            Result.Text = "";
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (expression.Length < 1) return;

            expression = expression.Remove(expression.Length - 1);
            Expression.Text = Expression.Text.Remove(Expression.Text.Length - 1);
        }

        private void Math_Click(object sender, RoutedEventArgs e)
        {
            double number = Convert.ToDouble(Expression.Text);
            switch ((sender as Button).Name.ToString())
            {
                case "Sin":
                    Expression.Text = $"Sin({number})";
                    FormatResult(Math.Sin(number).ToString());
                    break;
                case "Cos":
                    Expression.Text = $"Cos({number})";
                    FormatResult(Math.Cos(number).ToString());
                    break;
                case "Tan":
                    Expression.Text = $"Tg({number})";
                    FormatResult(Math.Tan(number).ToString());
                    break;
                case "Log10":
                    Expression.Text = $"Log10({number})";
                    FormatResult(Math.Log10(number).ToString());
                    break;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                GetResult_Click(null, null);
        }

        private void Pi_Click(object sender, RoutedEventArgs e)
        {
            expression = expression + Math.PI.ToString();
            Expression.Text = expression;
        }

        private void ChangeSign_Click(object sender, RoutedEventArgs e)
        {
            if (expression[0] == '-')
            {
                expression = expression.Substring(1);
                Expression.Text = expression;
            }
            else
            {
                expression = "-" + expression;
                Expression.Text = expression;
            }
        }

        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            Expression.Text = $"2√({expression})";
            FormatResult(Math.Sqrt(Convert.ToDouble(expression)).ToString());
        }

        private void Pow2_Click(object sender, RoutedEventArgs e)
        {
            Expression.Text = Expression.Text + "^2";
            FormatResult(Math.Pow(Convert.ToDouble(expression), 2).ToString());
        }

        private void SqrtN_Click(object sender, RoutedEventArgs e)
        {
            temp = $"Math.Pow({expression},1.0/";
            Expression.Text = "";
            spec = true;
        }

        private void PowN_Click(object sender, RoutedEventArgs e)
        {
            temp = $"Math.Pow({expression},";
            Expression.Text = "";
            spec = true;
        }

        private void Mod_Click(object sender, RoutedEventArgs e)
        {
            expression = expression + "%";
            Expression.Text = expression;
        }

        private void Round_Click(object sender, RoutedEventArgs e)
        {
            FormatResult(expression.Remove(expression.Length - 1));
        }

        private void Bin_Click(object sender, RoutedEventArgs e)
        {
            FormatResult(Convert.ToString(Convert.ToInt32(expression), 2));
        }

        private void Hex_Click(object sender, RoutedEventArgs e)
        {
            FormatResult(Convert.ToString(Convert.ToInt32(expression), 16));
        }

        private void Factorial_Click(object sender, RoutedEventArgs e)
        {
            Expression.Text = Expression.Text + "!";
            FormatResult(Fact(Convert.ToInt32(expression)).ToString());
        }

        private int Fact(int number)
        {
            int res = 1;
            for (int i = number; i > 1; i--)
                res *= i;
            return res;
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private void FormatResult(string res)
        {
            /*string result = string.Empty;
            if (Convert.ToDouble(res) > 1000)
            {
                int div = 0;
                for (int i = res.Length - 1; i > -1; i--)
                {
                    div++;
                    result += res[i];
                    if (div % 3 == 0)
                        result += " ";
                }
            }
            else
            {
                result = Reverse(res);
            }
            Result.Text = Reverse(result);*/
            Result.Text = res;
            Expression.Text = res;
            expression = res;
        }
    }
}