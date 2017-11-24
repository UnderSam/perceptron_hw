using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW1
{
    public partial class Form1 : Form
    {
        private float training_Rate;
        private float threshold;
        private int limit_train;
        private int selectGroups;
        static private Boolean DEBUG;
        private Boolean reTry;
        private double[,] train_input = new double[10,8];
        private double[,] train_output = new double[10, 5];
        private double[] test_input = new double[8];
        private double[] test_output = new double[5];
        static private Boolean REV;
        private double[,] weights;
        private string final_weight;
        Matrix weights_matrix;
        public Form1()
        {
            ///////////////////////
            REV = true;
            DEBUG = false;
           
            ///////////////////////
            InitializeComponent();
            training_Rate = 1.0f;
            threshold = 1.0f;
            limit_train = 500;
            addTextToCombo();
            retry_button.Enabled = false;
            reTry = false;
            initializeGroup_Label();
            testGroup.Visible = false;
            trainButton.Enabled = false;
            
        }
        private void addTextToCombo()
        {
            for(int i =1;i<=10;i++)
            {
                train_group_list.Items.Add(i);
            }
        }
        private void initializeGroup_Label()
        {
           
            for (int i = 2; i <=11; i++)
            {
              
                GroupBox _gp = this.Controls.Find("groupBox" + i, false).FirstOrDefault() as GroupBox;
                foreach(Control ctl in _gp.Controls)
                {
                    if(ctl is Label)
                    {
                       
                        ctl.Click += new EventHandler(labelClick);
                        ctl.BackColor = Color.White;
                        
                    }
                }
                _gp.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!DEBUG)
                deb.Visible = false;
            train_rate_text.Text = training_Rate.ToString();
            Threshold_text.Text = threshold.ToString();
            train_time_text.Text = limit_train.ToString();
            Array.Clear(test_input, 0, test_input.Length);
            Array.Clear(test_output, 0, test_output.Length);
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 8; j++)
                    train_input[i, j] = 0;
        }

        private void trainButton_Click(object sender, EventArgs e)
        {
            limit_train = int.Parse(train_time_text.Text);
            training_Rate = float.Parse(train_rate_text.Text);
            threshold = float.Parse(Threshold_text.Text);
            retry_button.Enabled = true;
            trainButton.Enabled = false;
            train_rate_text.Enabled = false;
            Threshold_text.Enabled = false;
            testGroup.Visible = true;
            train_group_list.Enabled = false;
            reTry = true;
            setInputValue();
            setOutputValue();
            if (DEBUG) 
            {
                deb.Text = train_group_list.SelectedIndex.ToString();
                string msg = "train_input:\n";
                for (int i = 0; i <= train_group_list.SelectedIndex; i++)
                {
                    for (int j = 0; j < 8; j++)
                        msg += train_input[i, j] + " ";
                    msg += "\n";
                }
                MessageBox.Show(msg);
            } // print train_input
            if (DEBUG) 
            {
                deb.Text = train_group_list.SelectedIndex.ToString();
                string msg = "train_output:\n";
                for (int i = 0; i <= train_group_list.SelectedIndex; i++)
                {
                    for (int j = 0; j < 5; j++)
                        msg += train_output[i, j] + " ";
                    msg += "\n";
                }
                MessageBox.Show(msg);
            } // print train_output

            perceptron(train_input, train_output, train_group_list.SelectedIndex + 1, 8,training_Rate,limit_train);
        }

        private void setInputValue()
        {
            for (int i = 2; i <= train_group_list.SelectedIndex+2; i++)
            {

                GroupBox _gp = this.Controls.Find("groupBox" + i, false).FirstOrDefault() as GroupBox;
                foreach (Control ctl in _gp.Controls)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (ctl.Name == ("i" + (i-2)+ j))
                        {
                            if (ctl.BackColor == Color.Black)
                                train_input[i - 2,j - 1] = 1;
                            else
                                train_input[i - 2, j - 1] = 0;
                        }
                    }
                    
                }

            }
        }
        private void setOutputValue()
        {
            for (int i = 2; i <= train_group_list.SelectedIndex + 2; i++)
            {

                GroupBox _gp = this.Controls.Find("groupBox" + i, false).FirstOrDefault() as GroupBox;
                foreach (Control ctl in _gp.Controls)
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        if (ctl.Name == ("o" + (i - 2) + j))
                        {
                            if (ctl.BackColor == Color.Black)
                                train_output[i - 2, j - 1] = 1;
                            else
                                train_output[i - 2, j - 1] = 0;
                        }
                    }

                }

            }
        }

        private void chain_group_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                trainButton.Enabled = true;
                selectGroups = train_group_list.SelectedIndex+1;
                selectGroups += 1; //符合group的index
                for(int i =2;i<=11;i++)
                {
                    GroupBox _gp = this.Controls.Find("groupBox" + i,false).FirstOrDefault() as GroupBox;
                    if (i <= selectGroups)
                        _gp.Visible = true;
                    else
                        _gp.Visible = false;
                }
                if(DEBUG)
                {
                   
                }
            }
            catch(Exception except)
            {
                MessageBox.Show(except.ToString());
            }
        }
        private void retry_button_Click(object sender, EventArgs e)
        {
            trainButton.Enabled = true;
            retry_button.Enabled = false;
            train_rate_text.Enabled = true;
            Threshold_text.Enabled = true;
            train_group_list.Enabled = true;
            testGroup.Visible = false;
            reTry = false;
            
            Array.Clear(test_input, 0, test_input.Length);
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 8; j++)
                    train_input[i, j] = 0;
        }
        private void allClear(object sender,EventArgs e)
        {
            try
            {
                for (int i = 2; i <= 11; i++)
                {
                    GroupBox _gp = this.Controls.Find("groupBox" + i, false).FirstOrDefault() as GroupBox;
                    foreach (Control ctl in _gp.Controls)
                    {
                        if (ctl is Label)
                        {
                            ctl.BackColor = Color.White;
                        }
                    }
                }
                foreach(Control ctl in testGroup.Controls)
                {
                    if(ctl is Label)
                    {
                        ctl.BackColor = Color.White;
                    }
                }
            }catch{ }
        }
        private void labelClick(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            if (DEBUG)
                deb.Text = lb.Name;
            if (lb.BackColor == Color.Black)
            {
                lb.BackColor = Color.White;
            }
            else
            {
                lb.BackColor = Color.Black;
            }
        }
        private void TestInput(object sender,EventArgs e)
        {
            Label lb = (Label)sender;
            if (lb.BackColor == Color.Black)
            {
                lb.BackColor = Color.White;
            }
            else
            {
                lb.BackColor = Color.Black;
            }
        }
        private void test_but_Click(object sender, EventArgs e)
        {


            foreach (Control ctl in testGroup.Controls) {
                for (int i = 1; i <= 8; i++)
                {
                    if (ctl.Name == ("ti" + i))
                    {
                        if (ctl.BackColor == Color.Black)
                            test_input[i - 1] = 1;
                        else
                            test_input[i - 1] = 0;
                    }
                }
            }
            test(8);
            foreach (Control ctl in testGroup.Controls) {
                for (int i = 1; i <= 5; i++)
                {
                    if (ctl.Name == ("to" + i))
                    {
                        if (test_output[i - 1] == 1)
                            ctl.BackColor = Color.Black;
                        else
                            ctl.BackColor = Color.White;
                    }
                }
            }
            if (DEBUG)
            {
                string msg = "inp:";
                for (int i = 0; i < test_input.Length; i++)
                {
                    msg += test_input[i].ToString() + " ";
                }
                msg += "\noupt:";
                for (int i=0;i<test_output.Length;i++)
                {
                    msg += test_output[i].ToString()+" ";
                }
                MessageBox.Show(msg);
            }
            
        }

        /////////////////////////////////////////////////////////////
        private void perceptron(double[,] train_input,double[,] train_output,int patterns_size,int input_dimention,double learning_rate,int epoch)
        {
            Random rnd = new Random();
            int output_dimention = 5;

            //weight[j,i]
            weights = new double[output_dimention, input_dimention];
            for (int i = 0; i < output_dimention; i++)
            {
                for (int j = 0; j < input_dimention; j++)
                {
                    weights[i, j] = rnd.NextDouble();
                }
            }
            weights_matrix = new Matrix(weights, output_dimention, input_dimention);

            //依據pattern數量建立input陣列
            double[][,] inputs = new double[patterns_size][,];
            //初始化個別input陣列並將值放入
            for (int i = 0; i < patterns_size; i++)
            {
                inputs[i] = new double[input_dimention, 1];
                for (int j = 0; j < input_dimention; j++)
                {
                    inputs[i][j, 0] = train_input[i,j];
                }
            }
            //轉為matrix類別
            Matrix[] inputs_matrix = new Matrix[patterns_size];
            for (int i = 0; i < patterns_size; i++)
            {
                inputs_matrix[i] = new Matrix(inputs[i], input_dimention, 1);
            }

            double[][,] outputs_desire = new double[patterns_size][,];
            for (int i = 0; i < patterns_size; i++)
            {
                outputs_desire[i] = new double[output_dimention, 1];
                for (int j = 0; j < output_dimention; j++)
                {
                    outputs_desire[i][j, 0] = train_output[i,j];
                }
            }
            Matrix[] outputs_desire_matrix = new Matrix[patterns_size];
            for (int i = 0; i < patterns_size; i++)
            {
                outputs_desire_matrix[i] = new Matrix(outputs_desire[i], output_dimention, 1);
            }

            double[,] output_value = new double[output_dimention, 1];
            Matrix output_value_matrix = new Matrix(output_value, output_dimention, 1);

            {
                for (int i = 0; i < epoch; i++)
                {
                    bool check = true;
                    for (int j = 0; j < patterns_size; j++)
                    {
                        output_value_matrix = activation_function(weights_matrix.multiply(inputs_matrix[j]),threshold);
                        weights_matrix = output_value_matrix.multiplyConstant(-1)
                                        .add(outputs_desire_matrix[j])
                                        .multiply(inputs_matrix[j].transpose())
                                        .multiplyConstant(learning_rate)
                                        .add(weights_matrix);
                        weights_matrix.printMatrix();
                        final_weight = weights_matrix.getWeightMatrix_STRING();
                    }
                    for (int j = 0; j < patterns_size; j++)
                    {
                        for (int k = 0; k < output_dimention; k++)
                        {
                            if (activation_function(weights_matrix.multiply(inputs_matrix[j]),threshold).getMatrixXY(k, 0)
                                != outputs_desire_matrix[j].getMatrixXY(k, 0))
                            {
                                check = false;
                                break;
                            }
                        }
                    }
                    

                    if (check == true) {
                        MessageBox.Show("Success ! \n Train : " + i + " times");
                        break;
                    }
                }
            }
            
        }

        private Matrix activation_function(Matrix input,float threshold)
        {
            for (int i = 0; i < input.getSize_row(); i++)
            {
                if (input.getMatrixXY(i, 0) >= threshold) input.setMatrixXY(i, 0, 1);
                else input.setMatrixXY(i, 0, 0);
            }
            return input;
        }

        public void test(int input_dimention)
        {
            double[,] input_test = new double[input_dimention, 1];
            for (int i = 0; i < input_dimention; i++)
            {
                input_test[i, 0] = test_input[i];
            }
            Matrix input_test_matrix = new Matrix(input_test, input_dimention, 1);
            Matrix output_matrix = activation_function(weights_matrix.multiply(input_test_matrix),threshold);
            for (int i = 0; i < output_matrix.getSize_row(); i++)
            {
                test_output[i] = output_matrix.getMatrixXY(i, 0);
                Console.Write(test_output[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(final_weight);
        }
    }
}
