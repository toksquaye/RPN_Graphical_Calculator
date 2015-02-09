using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPN_Graphical_Calculator
{
    static class Program
    {
        /// <summary>
        /// Is this a valid hex # containing character 0-9 a-f A-F ?
        /// </summary>
        public static bool IsHexNum(string hexVal)
        {
            foreach (char c in hexVal)
            {
                if (('0' <= c && c <= '9') || ('a' <= c && c <= 'f') || ('A' <= c && c <= 'F'))
                    continue;
                else 
                    return false;
            }
            return true;
        }

        public class RPN_Calculator:ICalcSession
        {
            private Stack expression_values;
            private Dictionary<string, string> History;
            private int HistoryCursor, 
                CurrentIndex, 
                NewestIndex, 
                OldestIndex;
            private bool hexExpression;

            private Mode calcMode;
            public Mode CalcMode
            {
                get { return calcMode; }
                set { calcMode = value; }
            }

            internal enum Mode
            {
                STRICT,
                EXTENDED
            }
            public RPN_Calculator()
            {
                expression_values = new Stack();
                History = new Dictionary<string,string>();
                CurrentIndex = 0;
                HistoryCursor = NewestIndex = OldestIndex = -1;  //initialize all indexers
                calcMode = Mode.STRICT; //set all default values
                hexExpression = false;
            }

            /// <summary>
            /// 
            /// push value on stack if valid and return true. else return false
            /// if its an operator, do calculation if possible and push answer on stack
            /// if not possible, return false
            /// if in extended mode and there is only 1 operand for this operator discard operator 
            /// and leave operand on the stack. If not in extended mode, flag this as an invalid expression
            /// 
            /// </summary>           
            private bool pushVal(string token_val)
            {
                double r_operand, l_operand, d_val;
                switch (token_val)
                    {
                        case "+": 
                            if (expression_values.Count > 1)
                            { 
                                
                                r_operand = Convert.ToDouble(expression_values.Pop());
                                l_operand = Convert.ToDouble(expression_values.Pop());
                                var ans = l_operand + r_operand;
                                expression_values.Push(ans);
                            }
                            else
                            {
                                //if in extended mode and there is only 1 operand for this operator
                                // discard operator and leave lone operand on the stack. accept expression as valid
                                //if not in extended mode, flag this as an invalid expression
                                if (this.calcMode != Mode.EXTENDED)
                                    return false; //invalid expression
                            }
                            break;
                        case "-":
                            if (expression_values.Count > 1)
                            { 
                                r_operand = Convert.ToDouble(expression_values.Pop());
                                l_operand = Convert.ToDouble(expression_values.Pop());
                                var ans = l_operand - r_operand;
                                expression_values.Push(ans);
                            }
                            else
                            {
                                //if in extended mode and there is only 1 operand for this operator
                                // discard operator and make operand the answer. accept expression as valid
                                //if not in extended mode, flag this as an invalid expression
                                if (this.calcMode != Mode.EXTENDED)
                                    return false; //invalid expression
                            }
                            break;
                        case "*":
                            if (expression_values.Count > 1)
                            {
                                r_operand = Convert.ToDouble(expression_values.Pop());
                                l_operand = Convert.ToDouble(expression_values.Pop());
                                var ans = l_operand * r_operand;
                                expression_values.Push(ans);
                            }
                            else
                            {
                                //if in extended mode and there is only 1 operand for this operator
                                // discard operator and make operand the answer. accept expression as valid
                                //if not in extended mode, flag this as an invalid expression
                                if (this.calcMode != Mode.EXTENDED)
                                    return false; //invalid expression
                            }
                            break;
                        case "/":
                            if (expression_values.Count > 1)
                            {
                                r_operand = Convert.ToDouble(expression_values.Pop());
                                l_operand = Convert.ToDouble(expression_values.Pop());
                                var ans = l_operand / r_operand;
                                expression_values.Push(ans);
                            }
                            else
                            {
                                //if in extended mode and there is only 1 operand for this operator
                                // discard operator and make operand the answer. accept expression as valid
                                //if not in extended mode, flag this as an invalid expression
                                if (this.calcMode != Mode.EXTENDED)
                                    return false; //invalid expression
                            }
                            break;
                    default:
                            if(double.TryParse(token_val,out d_val)) 
                                expression_values.Push(d_val);
                            else return false;
                        break;
                }//switch

                //if in extended mode and stack is empty, push 0 on stack as final answer so as not to error out.
                if (this.calcMode == Mode.EXTENDED && expression_values.Count == 0)
                    expression_values.Push(0);
                return true;
            }

            /// <summary>
            /// Eval(s) returns string s'.  If s is a valid string expression 
            /// as defined in the problem set desciption, then s' is the result
            /// of computing s.
            /// If s is an invalid expression, s' contains the substring "Error".
            /// If s is a semi-valid expression, s' either contains the substring
            /// "Error" or is a the string representing the result of computing s.
            /// Effects: Appends s to the Newest end of History. 
            /// Sets HistoryCursor to position Current.
            /// 
            /// 
            /// 
            /// </summary>
            public string Eval(string expr)
            {
                
                double dval;
                bool expr_valid = true;
                hexExpression = false;
                string answer;

                var val_array = Regex.Split(expr, "\\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray() ;  //split expression into an array of tokens
                int index = 0; string token;
                while(index < val_array.Length && expr_valid)
                {
                    token = val_array[index++];
                    switch (token)
                    {
                        case "hex": //if first token in "hex", flag current expression as hex 
                            if ((index - 1) == 0)
                            {
                                hexExpression = true;
                                if (calcMode != Mode.EXTENDED) //if not manually set into Extended mode by user, then set it.
                                    calcMode = Mode.EXTENDED;
                            }
                            break;
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            if (!pushVal(token))
                            {
                                expr_valid = false;
                            }
                            break;
                        default:
                            if(hexExpression) //if its a valid hex, push on stack. else, invalid expression
                            {
                                if (IsHexNum(token))
                                {
                                    expression_values.Push(Convert.ToInt32(token, 16));//convert to int, push on stack
                                }

                                else
                                {
                                    expr_valid = false;
                                    continue;
                                }
                                
                            }
                            else  // if it's not an hex expression
                            { 
                                if (double.TryParse(token, out dval)) //if token is valid number, push onto stack
                                {
                                    expression_values.Push(dval);
                                }
                                else //not a valid #
                                {
                                    //check if its part of a semi-valid expression. ex 3.4+
                                    if (Regex.IsMatch(token, ".+[+-/*]$"))
                                    {
                                        var oper = token.Last(); // get the operator
                                        var new_token = token.Remove(token.Length - 1);//get token before the operator
                                        //if token isn't valid, flag this expression as invalid. otherwise, it's pushed on the stack by pushVal
                                        if (!pushVal(new_token))
                                        {
                                            expr_valid = false;
                                            continue;
                                        }
                                        //call pushVal to calculate new token using extracted operator and push on the stack. 
                                        //if False is returned, flag this expression as invalid
                                        if (!pushVal(oper.ToString()))
                                        {
                                            expr_valid = false;
                                            continue;
                                        }
                                    }
                                    else //not part of a semi-valid expression. flag as invalid expression
                                    {
                                        expr_valid = false;
                                        continue;
                                    }
                                }

                            }// not an hex expression
                            break;
                    }
                    
                }

                // Strict Mode : If more that 1 token left in stack at this point, 
                // it mustve been an invalid string. OR it noticed that expression is
                // invalid while parsing the string, then flag as invalid expression
                // Extended Mode : If more than 1 token left, answer is the most recent # pushed
                // the stack. Discard the rest
                //
                if (expression_values.Count > 1 || !expr_valid)
                {
                    if (this.CalcMode == Mode.EXTENDED) 
                    {
                        if (expression_values.Count == 0)  //if in extended mode and stack is empty, push 0 onto the stack as the answer
                        {
                            expression_values.Push(0);
                            expr_valid = true;
                        }
                        else
                        {   //in extended mode and stack has more than 1 value. keep most recent # and discard the rest
                            var finalAnswer = expression_values.Pop(); //get last evaluated token
                            expression_values.Clear(); //discard the rest;
                            expression_values.Push(finalAnswer);    //put last evaluated token back on the stack as the final answer
                            expr_valid = true;
                        }
                    }
                    else
                    {
                        if (expr_valid) expr_valid = false; //set to false only if it isnt already
                        if (expression_values.Count > 0)
                            expression_values.Clear();
                    }
                }
                

                // if expr_valid is still true, then pop the answer is atop of the stack
                if (expr_valid)
                {
                    if (hexExpression)
                    {
                        int ianswer = Convert.ToInt32(expression_values.Pop());
                        answer = ianswer.ToString("X"); //convert to an hex
                    }
                    else
                        answer = expression_values.Pop().ToString();
                }
                else
                    answer = "Error";

                try
                {
                    History.Add(expr, answer);  //add given expression & answer to history  
                }
                catch (ArgumentException)
                {
                    //duplicate entry. exit function without incrementing historyCursor
                    return answer;
                }
                NewestIndex = HistoryCursor = CurrentIndex++;    //Increment NewestIndex
                if(History.Count == 1)      //if this is the first element, set oldestindex & newestindex to 0
                {
                    OldestIndex = 0;  
                }
                return answer;
            }


            // HistoryForward() returns "" when History is empty or 
            //   HistoryCursor is at position Current.
            //   Returns Newest History element when HistoryCursor is at Newest.
            //   Otherwise returns History element at position one newer than
            //   HistoryCursor position.
            // Effects: If HistoryCursor is at position older than Newest, sets
            //   HistoryCursor one position newer.
            public string HistoryForward()
            {                
                //if History is empty or HistoryCursor is at current postion, return blank
                if (History.Count == 0 || HistoryCursor == CurrentIndex)    
                    return "";
                if(HistoryCursor < NewestIndex  && HistoryCursor >= OldestIndex)
                {
                    return History.ElementAt(++HistoryCursor).ToString(); //set HistoryCursor one position newer and return it                
                }
                if(HistoryCursor == NewestIndex)
                    return History.ElementAt(HistoryCursor++).ToString();
                return "";
            }

            // HistoryBack() returns "" when History is empty.
            //   Returns Oldest History element when HistoryCursor is at Oldest
            //   Otherwise returns History element one older than HistroyCursor
            //   position.
            // Effects: If possible, sets HistoryCursor one position older.
            public string HistoryBack()
            {
                if(History.Count == 0)
                    return "";

                if (HistoryCursor > OldestIndex)
                    return History.ElementAt(--HistoryCursor).ToString(); //Set HistoryCursor one position older and return it.
                
                if (HistoryCursor == OldestIndex)
                    return History.ElementAt(HistoryCursor).ToString(); //return oldest element when HistoryCursor is at Oldest
                
                return "";//default
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           Application.Run(new Form1());
           /* RPN_Calculator test = new RPN_Calculator();
            var a = test.HistoryForward();
            a = test.HistoryBack();
            var ans = test.Eval("3 4 + 2 7 * -");
            ans = test.Eval("-2.3 3.0 *");
            ans = test.Eval("1.0 2.3+");
            ans = test.Eval("x");
            a = test.HistoryBack();
            a = test.HistoryBack();
            a = test.HistoryForward();
            ans = test.Eval("1 +");
            ans = test.Eval("1.0 2.3+");
            a = test.HistoryForward();
            ans = test.Eval("11 +");
            ans = test.Eval("1 2 -3 +");
            //ans = test.Eval("34 9 9 -");
            //ans = test.Eval("-2.0 3.1 *");
            */
        }
    }
}
