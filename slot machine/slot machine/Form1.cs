using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace slot_machine
{

    public partial class Form1 : Form
    {
        //makes values act globally
        int plays = 0;
        int nudge = 0; int hold = 0;
        int Win = 0;

        int order = 0; int s1 = 0; int s2 = 0; int s3 = 0;

        bool h1 = false; bool h2 = false; bool h3 = false;
        //true == held
        Random rnd = new Random();

        //wheel images
        static Image yellow = slot_machine.Properties.Resources.star___yellow;
        static Image green = slot_machine.Properties.Resources.star___green;
        static Image blue = slot_machine.Properties.Resources.star___blue;
        static Image grey = slot_machine.Properties.Resources.star___grey;
        static Image pink = slot_machine.Properties.Resources.star___pink;
        static Image red = slot_machine.Properties.Resources.star___red;
        Image[] W1 = new Image[6];

        public Form1()
        {
            InitializeComponent();
            
            W1[0] = yellow; W1[1] = green; W1[2] = blue; W1[3] = grey; W1[4] = pink; W1[5] = red;
            //adds all images to array
            
            pictureBox1.Image = W1[0];
            pictureBox2.Image = W1[0];
            pictureBox3.Image = W1[0];

            button1.Text = "Nudge"; button2.Text = "Nudge"; button3.Text = "Nudge";
            button5.Text = "Hold"; button6.Text = "Hold"; button7.Text = "Hold";
            button8.Text = "Spin"; label4.Text = "Wins:" + Win + "";

            button1.BackColor = Color.Gray; button2.BackColor = Color.Gray; button3.BackColor = Color.Gray;
            button5.BackColor = Color.Gray; button6.BackColor = Color.Gray; button7.BackColor = Color.Gray;
            //hold/nudge button col

            button8.BackColor = Color.Gray;
            //spin button col

        }

        //holds
        public void changeH(ref int Win, int s1, int s2, int s3)
        {
            if (pictureBox5.BackColor == Color.Yellow) { pictureBox5.BackColor = Color.Black; }
            else { pictureBox4.BackColor = Color.Black; }
            win(ref Win, s1, s2, s3);
        }
        private void button5_Click(object sender, EventArgs e)
        {//hold 1
            button8.BackColor = Color.Gray;
            if (button5.BackColor != Color.Blue)
            { if (hold > 0) { button5.BackColor = Color.Blue; hold--; h1 = true; changeH(ref Win, s1, s2, s3); } }
        }
        private void button6_Click(object sender, EventArgs e)
        {//hold 2
            button8.BackColor = Color.Gray;
            if (button6.BackColor != Color.Blue)
            { if (hold > 0) { button6.BackColor = Color.Blue; hold--; h2 = true; changeH(ref Win, s1, s2, s3); } }
        }
        private void button7_Click(object sender, EventArgs e)
        {//hold 3
            button8.BackColor = Color.Gray;
            if (button7.BackColor != Color.Blue)
            { if (hold > 0) { button7.BackColor = Color.Blue; hold--; h3 = true; changeH(ref Win, s1, s2, s3); } }
        }

        //spin
        public void changetopbottom() 
        {
            pictureBox6.Image = W1[(s1 + 1) % 6]; pictureBox7.Image = W1[(s2 + 1) % 6]; pictureBox8.Image = W1[(s3 + 1) % 6];
            pictureBox9.Image = W1[(s1 + 5) % 6]; pictureBox10.Image = W1[(s2 + 5) % 6]; pictureBox11.Image = W1[(s3 + 5) % 6];
        }
        private void button8_Click(object sender, EventArgs e)
        {//spin button
            button8.BackColor = Color.Blue;
            spin(ref s1, ref s2, ref s3, ref plays, ref order, ref hold, ref nudge, ref h1, ref h2, ref h3);
        }

        public void spin(ref int s1, ref int s2, ref int s3, ref int plays, ref int order, ref int hold, ref int nudge, ref bool h1, ref bool h2, ref bool h3)
        {
            button5.BackColor = Color.Gray; button6.BackColor = Color.Gray; button7.BackColor = Color.Gray;
            plays++;

            calc(ref plays, ref order, ref hold, ref nudge, h1, h2, h3);
            int R1 = rnd.Next(0, 6);
            int R2 = rnd.Next(0, 6);
            int R3 = rnd.Next(0, 6);
            //spin check
            if (h1 == false && h2 == false && h3 == false)
            {
                pictureBox1.Image = W1[R1]; s1 = R1;
                pictureBox2.Image = W1[R2]; s2 = R2;
                pictureBox3.Image = W1[R3]; s3 = R3;
            }
            else if (h1 == false && h2 == false && h3 == true) 
            {
                pictureBox1.Image = W1[R1]; s1 = R1;
                pictureBox2.Image = W1[R2]; s2 = R2;
            }
            else if (h1 == false && h2 == true && h3 == false) 
            {
                pictureBox1.Image = W1[R1]; s1 = R1;
                pictureBox3.Image = W1[R3]; s3 = R3;
            }
            else if (h1 == false && h2 == true && h3 == true) 
            {
                pictureBox1.Image = W1[R1]; s1 = R1;
            }
            else if (h1 == true && h2 == false && h3 == false) 
            {
                pictureBox2.Image = W1[R2]; s2 = R2;
                pictureBox3.Image = W1[R3]; s3 = R3;
            }
            else if (h1 == true && h2 == false && h3 == true) 
            {
                pictureBox2.Image = W1[R2]; s2 = R2;
            }
            else if (h1 == true && h2 == true && h3 == false) 
            {
                pictureBox3.Image = W1[R3]; s3 = R3;
            }
            changetopbottom();
            win(ref Win, s1, s2, s3);
            h1 = false; h2 = false; h3 = false;
            //end of spin placement determination
        }

        //nudges
        public void changeN(ref int Win, int s1, int s2, int s3) 
        {
            if (pictureBox12.BackColor == Color.Yellow && pictureBox13.BackColor == Color.Yellow) { pictureBox12.BackColor = Color.Black; }
            else if (pictureBox13.BackColor == Color.Yellow && pictureBox14.BackColor == Color.Yellow) { pictureBox13.BackColor = Color.Black; }
            else { pictureBox14.BackColor = Color.Black; }
            win(ref Win, s1, s2, s3);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //nudge 1
            button8.BackColor = Color.Gray;
            if (nudge > 0)
            {
                nudge--; s1--;
                pictureBox1.Image = W1[(s1 + 6) % 6]; 
                changetopbottom();
                changeN(ref Win, s1, s2, s3);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //nudge 2
            button8.BackColor = Color.Gray;
            if (nudge > 0)
            {
                nudge--; s2--;
                pictureBox2.Image = W1[(s2 + 6)%6]; 
                changetopbottom();
                changeN(ref Win, s1, s2, s3);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //nudge 3
            button8.BackColor = Color.Gray;
            if (nudge > 0)
            {
                nudge--; s3--;
                pictureBox3.Image = W1[(s3 + 6)%6]; 
                changetopbottom();
                changeN(ref Win, s1, s2, s3);
            }
        }

        //win/calc
        public void calc(ref int plays, ref int order, ref int hold, ref int nudge, bool h1, bool h2, bool h3)
        {
            if ((plays % 3) == 0) { hold = 2; pictureBox4.BackColor = Color.Yellow; pictureBox5.BackColor = Color.Yellow; }
            else { hold = 0; pictureBox4.BackColor = Color.Black; pictureBox5.BackColor = Color.Black; }//every 3 plays hold == 2
            if ((plays % 5) == 0)
            { nudge = 3; pictureBox12.BackColor = Color.Yellow; pictureBox13.BackColor = Color.Yellow; pictureBox14.BackColor = Color.Yellow; }
            else
            { nudge = 0; pictureBox12.BackColor = Color.Black; pictureBox13.BackColor = Color.Black; pictureBox14.BackColor = Color.Black; }
            // every 5 plays nudge == 3
        }
        public void win(ref int Win, int s1,int s2, int s3) 
        { if (s1 == s2 && s1 == s3) { Win++; label4.Text = "Wins:"+Win+""; } }

    }
}
