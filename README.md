# RPN_Graphical_Calculator
Reverse Polish Notation Graphical Calculator


Reverse Polish Notation Graphical Calculator Developer : Tokunbo Quaye Date : February 2015

This project implements a graphical calculator using the Reverse Polish Notation. It was written in C# and developed using Visual Studio 2013 .NET Framework Version 4.5.50938

Project Description:

Normal, or infix, mathematical notation requires parentheses to write certain expressions. For instance, when we write (3 + 9) * 9 parentheses are needed to force the addition to happen before the multiplication. RPN, or postfix notation, is a technique for writing expressions without need for parentheses. Instead, subexpressions are grouped by position. In RPN we build an expression by writing an operator's arguments followed by the operator itself. So 3 + 9 is written 3 9 +, and (3 + 9) * 9 is written as 3 9 + 9 *. Likewise 2 * (7 - 4) is written 2 7 4 - *.

The calculator runs in two modes: Strict Mode & Extended Mode

The main algorithm for the Strict Mode goes as follows :

•While there are input tokens left, Read the next token from input.
◦If the token is a valid number ◾Push it onto the stack.

◦Otherwise, the token is an operator (operator here includes both operators and functions). ◾ If there are fewer than 2 values on the stack ◾ (Error) The user has not input sufficient values in the expression.

◾ Else ◾ Pop the top 2 values from the stack.
◾ Evaluate the operator, with the values as arguments.
◾ Push the returned results, if any, back onto the stack.




•If there is only one value in the stack That value is the result of the calculation.


•Otherwise, there are more values in the stack (Error) The user input has too many values.

Strict mode also accepts semi-valid expressions such as "2.3 3.3+". It processes it as "2.3 3.3 +"

When in Extended Mode, it does the following in addition to Strict Mode functionality

•If input is an operator, result is 0.
•accepts hex values as input. Expression must be preceded by the keyword "hex"
•If an operator has only 1 operand, the answer is the operand.
•If more than 1 value is left on the stack after the expression has been processed, the anser is the most recent
• value that was pushed on the stack. Discard the other numbers.
•If not in Extended Mode, but the expression starts with "hex" keyword, automatically put calculator in hex mode
 


 
   


