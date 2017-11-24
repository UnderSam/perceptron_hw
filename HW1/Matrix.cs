using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW1
{
    class Matrix
    {
        private double[,] matrix;
        private int size_row;
        private int size_col;
        private string final_weight;

        public string Final_weight { get => final_weight; set => final_weight = value; }

        public int getSize_row()
        {
            return size_row;
        }
        public int getSize_col()
        {
            return size_col;
        }

        public Matrix(double[,] m, int row, int col)
        {
            matrix = m;
            size_row = row;
            size_col = col;
        }

        public double getMatrixXY(int row, int col)
        {
            return matrix[row, col];
        }

        public void setMatrixXY(int row, int col, double value)
        {
            matrix[row, col] = value;
        }

        public Matrix multiply(Matrix m)
        {
            if (size_col != m.getSize_row()) return null;
            //建立相乘後的矩陣
            double[,] res = new double[size_row, m.getSize_col()];
            double tempsum = 0;

            for (int i = 0; i < size_row; i++)
            {
                for (int j = 0; j < m.getSize_col(); j++)
                {
                    for (int k = 0; k < size_col; k++)
                    {
                        tempsum += matrix[i, k] * m.getMatrixXY(k, j);
                    }
                    res[i, j] = tempsum;
                    tempsum = 0;
                }
            }
            Matrix temp = new Matrix(res, size_row, m.getSize_col());
            return temp;
        }
        public Matrix add(Matrix m)
        {
            if (m.getSize_row() != size_row || m.getSize_col() != size_col) return null;
            double[,] res = new double[size_row, size_col];
            for (int i = 0; i < size_row; i++)
            {
                for (int j = 0; j < size_col; j++)
                {
                    res[i, j] = matrix[i, j] + m.getMatrixXY(i, j);
                }
            }
            Matrix temp = new Matrix(res, size_row, size_col);
            return temp;
        }
        public Matrix multiplyConstant(double scale)
        {
            for (int i = 0; i < size_row; i++)
            {
                for (int j = 0; j < size_col; j++)
                {
                    matrix[i, j] = matrix[i, j] * scale;
                }
            }
            return this;
        }
        public Matrix transpose()
        {
            double[,] res = new double[size_col, size_row];
            for (int i = 0; i < size_row; i++)
            {
                for (int j = 0; j < size_col; j++)
                {
                    res[j, i] = matrix[i, j];
                }
            }
            return new Matrix(res, size_col, size_row);
        }
        public void printMatrix()
        {
            for (int i = 0; i < size_row; i++)
            {
                string temp = "";
                for (int j = 0; j < size_col; j++)
                {
                    temp += matrix[i, j] + " ";
                }
                Console.WriteLine(temp);
            }
            Console.WriteLine("");
        }
        public String getWeightMatrix_STRING()
        {
            string temp="";
            for (int i = 0; i < size_row; i++)
            {
                for (int j = 0; j < size_col; j++)
                {
                    temp += "W[" + i + "," + j + "]=" + matrix[i, j] + "\n";
                }
               
            }
            return temp;
        }
    }
}
