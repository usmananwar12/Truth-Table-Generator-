using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        public static Form1 instance;
        static int Precedence(char op)
        {
            if (op == '~') // not
                return 1;
            else if (op == '&') // and
                return 3;
            else if (op == '^') // xor
                return 4;
            else if (op == '|') // or
                return 5;
            return 0;
        }

        static string ToPostfix(string infix)
        {
            Stack<char> op = new Stack<char>();
            char smbl;
            string postfix = "";

            for (int i = 0; i < infix.Length; ++i)
            {
                smbl = infix[i];
                if (char.IsLetterOrDigit(smbl))
                {
                    postfix += smbl;
                }
                else if (smbl == '(')
                {
                    op.Push(smbl);
                }
                else if (smbl == ')')
                {
                    while (op.Count > 0 && op.Peek() != '(')
                    {
                        char top = op.Pop();
                        postfix += top;
                    }
                    if (op.Count > 0 && op.Peek() == '(')
                    {
                        op.Pop();
                    }
                    else
                    {
                        instance.ShowErrorMessage("Invalid set of brackets.");
                        return "";
                    }
                }
                else
                {
                    while (op.Count > 0 && Precedence(op.Peek()) >= Precedence(smbl))
                    {
                        char top = op.Pop();
                        postfix += top;
                    }
                    op.Push(smbl);
                }
            }

            while (op.Count > 0)
            {
                char top = op.Pop();
                postfix += top;
            }

            return postfix;
        }

        static void InitializeTruthValues(int[,] truthTable, int numRows, int variables)
        {
            for (int i = 0; i < numRows; ++i)
            {
                int value = 0;
                for (int j = 0; j < variables; ++j)
                {
                    value = (value << 1) | truthTable[i, j];
                }
                truthTable[i, variables] = value;
            }
        }

        static int EvaluateExpression(string postfixExpression, int[] variableValues, int numVariables)
        {
            Stack<int> operandStack = new Stack<int>();
            foreach (char c in postfixExpression)
            {
                if (char.IsLetterOrDigit(c))
                {
                    operandStack.Push(variableValues[c - 'A']);
                }
                else
                {
                    int operand2 = operandStack.Pop();
                    int operand1;
                    if (c != '~')
                        operand1 = operandStack.Pop();
                    else
                    {
                        operand1 = -1;
                    }

                    switch (c)
                    {
                        case '&':
                            operandStack.Push(operand1 & operand2);
                            break;
                        case '|':
                            operandStack.Push(operand1 | operand2);
                            break;
                        case '^':
                            operandStack.Push(operand1 ^ operand2);
                            break;
                        case '~':
                            operandStack.Push(operand2 == 0 ? 1 : 0);
                            break;
                    }
                }
            }
            return operandStack.Peek();
        }

       
        static int[] GetRow(int[,] arr, int rowIndex, int numVariables)
        {
            int[] row = new int[numVariables];
            for (int j = 0; j < numVariables; ++j)
            {
                row[j] = arr[rowIndex, j];
            }
            return row;
        }

        public static void TruthTable(string exp, int numVar, char[] var)
        {
            int numRows = (int)Math.Pow(2, numVar);
            int[,] arr = new int[numRows, numVar + 1];
            for (int i = 0; i < numRows; ++i)
            {
                for (int j = 0; j < numVar; ++j)
                {
                    arr[i, j] = (i >> (numVar - 1 - j)) & 1;
                }
                arr[i, numVar] = 0;
            }
            InitializeTruthValues(arr, numRows, numVar);
            for (int i = 0; i < numRows; ++i)
            {
                arr[i, numVar] = EvaluateExpression(exp, GetRow(arr, i, numVar), numVar);
            }
            Form2.PrintTruthTable(arr, numRows, numVar, var);
        }


        public static bool IsInArr(char[] arr, char x)
        {
            foreach (char c in arr)
            {
                if (c == x)
                {
                    return true;
                }
            }
            return false;
        }

        public Form1()
        {
            InitializeComponent();
            instance = this; 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void InputExp_Click(object sender, EventArgs e)
        {

        }
        private bool IsInputValid(string expression)
        {
            foreach (char c in expression)
            {
                if (char.IsDigit(c) && !char.IsUpper(c) && c != '&' && c != '|' && c != '~' && c != '^')
                {
                    return false;
                }
            }
            return true;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 Form = new Form2();
            Form.Show();
            string exp;
            char[] var;
            int numVar = 0, numVar2 = 0;
            exp = textBox1.Text;

            if (!IsInputValid(exp))
            {
                ShowErrorMessage("Invalid characters in the input expression. Please use only letters, digits, and valid operators.");
                return;
            }
            exp = ToPostfix(exp);

            HashSet<char> uniqueVars = new HashSet<char>();

            foreach (char c in exp)
            {
                if (char.IsLetterOrDigit(c))
                {
                    uniqueVars.Add(c);
                }
            }

            numVar = uniqueVars.Count;
            var = new char[numVar + 1];

            foreach (char uniqueVar in uniqueVars)
            {
                var[numVar2] = uniqueVar;
                numVar2++;
            }

            var[numVar2] = 'X';
            TruthTable(exp, numVar, var);
        }

    }
}
