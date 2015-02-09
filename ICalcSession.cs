using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Graphical_Calculator
{
    // Implementations of ICalcSession represent a rpn calculator which 
    //   operates on strings and maintains a history of all inputs.
    // State: Each ICalcSession object maintains a sequence of strings
    //   called History and a position relative to the sequence, 
    //   HistoryCursor.  HistoryCursor may refer to positions in History
    //   from Oldest to Newest, and special position Current.
    //   For example:
    //       History sequence:          A      B     C      D
    //       HistoryCursor positions: Oldest   .     .    Newest  Current
    //       Directions                 <--older--    --newer-->
    interface ICalcSession
    {
        // Eval(s) returns string s'.  If s is a valid string expression 
        //   as defined in the problem set desciption, then s' is the result
        //   of computing s.
        //   If s is an invalid expression, s' contains the substring "Error".
        //   If s is a semi-valid expression, s' either contains the substring
        //   "Error" or is a the string representing the result of
        //   computing s.
        // Effects: Appends s to the Newest end of History. 
        //   Sets HistoryCursor to position Current.
        string Eval(string s);

        // HistoryForward() returns "" when History is empty or 
        //   HistoryCursor is at position Current.
        //   Returns Newest History element when HistoryCursor is at Newest.
        //   Otherwise returns History element at position one newer than
        //   HistoryCursor position.
        // Effects: If HistoryCursor is at position older than Newest, sets
        //   HistoryCursor one position newer.
        string HistoryForward();

        // HistoryBack() returns "" when History is empty.
        //   Returns Oldest History element when HistoryCursor is at Oldest
        //   Otherwise returns History element one older than HistroyCursor
        //   position.
        // Effects: If possible, sets HistoryCursor one position older.
        string HistoryBack();
    }

}
