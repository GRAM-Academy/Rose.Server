using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;

namespace Rose.Engine.QueryParser
{
    internal class ConditionToken
    {
        private Argument _left, _right;
        private string _comparisonOperator;

        public Argument Left { get { return _left; } }
        public Argument Right { get { return _right; } }
        public string ComparisonOperator { get { return _comparisonOperator; } }





        public ConditionToken(string expr)
        {
            if (expr == null || expr.Length == 0)
                throw new AegisException(RoseResult.InvalidArgument, $"'{expr}' in the where clause is invalid.");

            expr = expr.Trim();


            //  왼쪽 인자 분석
            int copIndex = -1;


            //  변수를 가리키는 문자열
            if (expr[0] == '@')
                throw new AegisException(RoseResult.InvalidArgument, "Reference value cannot used in where clause.");

            //  Collection의 Key를 가리키는 문자열
            else if ((expr[0] >= 'a' && expr[0] <= 'z') || (expr[0] >= 'A' && expr[0] <= 'Z'))
            {
                FindCompOperator(expr, 0, ref copIndex);
                if (copIndex == -1)
                    throw new AegisException(RoseResult.InvalidArgument, $"'{expr}' in the where clause is invalid.");


                //  값과 비교연산자 사이의 공백 제거
                _left = new ReferenceValue(expr.Substring(0, copIndex - 1).Trim());
            }

            //  Value of string
            else if (expr[0] == '\'')
            {
                FindCompOperator(expr, 0, ref copIndex);
                if (copIndex == -1)
                    throw new AegisException(RoseResult.InvalidArgument, $"'{expr}' in the where clause is invalid.");


                //  값과 비교연산자 사이의 공백 제거
                string subStr = expr.Substring(1, copIndex - 1).TrimEnd();
                if (subStr[subStr.Length - 1] != '\'')
                    throw new AegisException(RoseResult.InvalidArgument, $"Quotes are not matched in '{expr}' expression.");

                _left = new Value(subStr.Substring(0, subStr.Length - 1));
            }

            //  Value of string
            else if (expr[0] == '"')
            {
                FindCompOperator(expr, 0, ref copIndex);
                if (copIndex == -1)
                    throw new AegisException(RoseResult.InvalidArgument, $"'{expr}' in the where clause is invalid.");


                //  값과 비교연산자 사이의 공백 제거
                string subStr = expr.Substring(1, copIndex - 1).TrimEnd();
                if (subStr[subStr.Length - 1] != '"')
                    throw new AegisException(RoseResult.InvalidArgument, $"Quotes are not matched in '{expr}' expression.");

                _left = new Value(subStr.Substring(0, subStr.Length - 1));
            }

            //  Value of numberic
            else
            {
                FindCompOperator(expr, 0, ref copIndex);
                if (copIndex == -1)
                    throw new AegisException(RoseResult.InvalidArgument, $"'{expr}' in the where clause is invalid.");


                //  long 혹은 double
                string val = expr.Substring(0, copIndex);
                long longVal;
                if (long.TryParse(val, out longVal) == true)
                    _left = new Value(longVal);
                else
                {
                    double doubleVal;
                    if (double.TryParse(val, out doubleVal) == false)
                        throw new AegisException(RoseResult.InvalidArgument, $"'{val}' is not valid type.");

                    _left = new Value(doubleVal);
                }
            }


            //  오른쪽 인자 분석
            ParseRightArgument(expr.Substring(copIndex + _comparisonOperator.Length));
        }


        private void FindCompOperator(string expr, int startIndex, ref int index)
        {
            string[] availableOp = { "==", "<=", ">=", "!=", "<", ">" };
            for (int i = 0; i < availableOp.Count(); ++i)
            {
                index = expr.Substring(startIndex).IndexOf(availableOp[i]);
                if (index != -1)
                {
                    _comparisonOperator = availableOp[i];
                    return;
                }
            }

            index = -1;
            _comparisonOperator = "";
        }


        private void ParseRightArgument(string expr)
        {
            expr = expr.Trim();

            //  변수를 가리키는 문자열
            if (expr[0] == '@')
                throw new AegisException(RoseResult.InvalidArgument, "Reference value cannot used in where clause.");

            //  Collection의 Key를 가리키는 문자열
            else if ((expr[0] >= 'a' && expr[0] <= 'z') || (expr[0] >= 'A' && expr[0] <= 'Z'))
            {
                _right = new ReferenceValue(expr);
            }

            //  Value of string
            else if (expr[0] == '\'')
            {
                if (expr[expr.Length - 1] != '\'')
                    throw new AegisException(RoseResult.InvalidArgument, $"Quotes are not matched in '{expr}' expression.");

                _right = new Value(expr.Substring(1, expr.Length - 2));
            }

            //  Value of string
            else if (expr[0] == '"')
            {
                if (expr[expr.Length - 1] != '"')
                    throw new AegisException(RoseResult.InvalidArgument, $"Quotes are not matched in '{expr}' expression.");

                _right = new Value(expr.Substring(1, expr.Length - 2));
            }

            //  Value of numberic
            else
            {
                //  long 혹은 double
                long longVal;
                if (long.TryParse(expr, out longVal) == true)
                    _right = new Value(longVal);
                else
                {
                    double doubleVal;
                    if (double.TryParse(expr, out doubleVal) == false)
                        throw new AegisException(RoseResult.InvalidArgument, $"'{expr}' is not valid type.");

                    _right = new Value(doubleVal);
                }
            }
        }
    }
}
