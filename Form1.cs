using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPN_Graphical_Calculator
{
    public partial class Form1 : Form
    {
        Program.RPN_Calculator calculator;
   


        public Form1()
        {
            InitializeComponent();
            calculator = new Program.RPN_Calculator();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            resultLabel.Text = "";
        }

        //HistoryBackButton
        private void button1_Click(object sender, EventArgs e)
        {
            string hist = calculator.HistoryBack();
            hist = hist.Replace("[", "Input: ").Replace(",", "     Result: ").Replace("]", "");
            resultLabel.Text = hist;
        }

        //EnterButton
        private void button3_Click(object sender, EventArgs e)
        {
            string expr = InputText.Text;
            resultLabel.Text = calculator.Eval(expr);

            //update Mode of calculator if it was changed during processing of expression
            if ((calculator.CalcMode == Program.RPN_Calculator.Mode.EXTENDED) && (ModeLabel.Text != "EXTENDED"))
            {
                ModeLabel.Text = "EXTENDED";
                listBox1.SelectedIndex = 1;
            }
            
        }

        //HistoryForwardButton
        private void button2_Click(object sender, EventArgs e)
        {
            string hist = calculator.HistoryForward();
            hist = hist.Replace("[", "Input: ").Replace(",", "     Result: ").Replace("]", "");
            resultLabel.Text = hist;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set mode to STRICT or EXTENDED depending on input from user
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    calculator.CalcMode = Program.RPN_Calculator.Mode.STRICT; 
                    ModeLabel.Text = "STRICT"; break;
                case 1:
                    calculator.CalcMode = Program.RPN_Calculator.Mode.EXTENDED; 
                    ModeLabel.Text = "EXTENDED";  break;
                default:
                    calculator.CalcMode = Program.RPN_Calculator.Mode.STRICT; 
                    ModeLabel.Text = "STRICT";   break;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = 0; //default is STRICT mode
        }
    }
}
