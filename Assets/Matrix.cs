using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Matrix
{
    private int rows;
    private int columns;
    public int Rows
    {
        get { return this.rows; }
        private set { this.rows = value; }
    }
    public int Columns
    {
        get { return this.columns; }
        private set { this.columns = value; }
    }

    public float this[int row, int column]
    {
        get
        {
            return values[row][column];
        }
        set
        {
            this.values[row][column] = value;
        }
    }
    private float[][] values;
    public Matrix(int rowI, int columnI)
    {
        Rows = rowI;
        columns = columnI;
        values = new float[rowI][];
        for (int i = 0; i < rowI; i++)
        {
            values[i] = new float[columnI];
            foreach (int j in values[i])
            {
                values[i][j] = 0;
            }
        }
    }
    public Matrix(float[] n)
    {
        Rows = n.Length;
        columns = 1;
        values = new float[Rows][];
        for (int i = 0; i < Rows; i++)
        {
            values[i] = new float[columns];
            values[i][0] = n[i];
        }
    }

    public static Matrix operator +(Matrix a) => a;
    public static Matrix operator -(Matrix a) => a.Negate(a);
    public static Matrix operator +(Matrix a, Matrix b) => a.Add(b, true);
    public static Matrix operator -(Matrix a, Matrix b) => a.Add(b, false);
    public static Matrix operator +(Matrix a, int b) => a.eAdd(a, b, true);
    public static Matrix operator -(Matrix a, int b) => a.eAdd(a, b, false);
    public static Matrix operator +(Matrix a, double b) => a.eAdd(a, b, true);
    public static Matrix operator -(Matrix a, double b) => a.eAdd(a, b, false);
    public static Matrix operator +(Matrix a, float b) => a.eAdd(a, b, true);
    public static Matrix operator -(Matrix a, float b) => a.eAdd(a, b, false);
    public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(a, b);
    public static Matrix operator *(Matrix a, float b) => a.Scale(a, b);
    public static Matrix operator *(float b, Matrix a) => a.Scale(a, b);
    public static Matrix operator *(Matrix a, int b) => a.Scale(a, b);
    public static Matrix operator *(int b, Matrix a) => a.Scale(a, b);
    public static Matrix operator *(Matrix a, double b) => a.Scale(a, b);
    public static Matrix operator *(double b, Matrix a) => a.Scale(a, b);
    //public static Matrix operator ^(float b, Matrix a) => a.Scale(a, b);



    private Matrix Negate(Matrix a)
    {
        Matrix retVal = new Matrix(Rows, columns);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                retVal[i, j] = -this[i, j];
            }
        }
        return retVal;
    }
    private Matrix Add(Matrix other, bool add)
    {
        if (this.Rows == other.Rows && this.columns == other.columns)
        {
            Matrix retVal = new Matrix(Rows, columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (add)
                    {
                        retVal[i, j] = this[i, j] + other[i, j];
                    }
                    else
                    {
                        retVal[i, j] = this[i, j] - other[i, j];
                    }
                }
            }
            return retVal;
        }
        else
        {
            throw new System.Exception("Matrix sizes don't match");
        }
    }
    public Matrix eWiseMultiply(Matrix other)
    {
        if (this.Rows == other.Rows && this.columns == other.columns)
        {
            Matrix retVal = new Matrix(Rows, columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                        retVal[i, j] = this[i, j] * other[i, j];
                }
            }
            return retVal;
        }
        else
        {
            throw new System.Exception("Matrix sizes don't match");
        }
    }
    private Matrix eAdd(Matrix a, float b, bool add)
    {
        Matrix retVal = new Matrix(Rows, columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (add)
                {
                    retVal[i, j] = this[i, j] += b;
                }
                else
                {
                    retVal[i, j] = this[i, j] -= b;
                }
            }
        }
        return retVal;
    }
    private Matrix eAdd(Matrix a, int b, bool add)
    {
        Matrix retVal = new Matrix(Rows, columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (add)
                {
                    retVal[i, j] = this[i, j] += b;
                }
                else
                {
                    retVal[i, j] = this[i, j] -= b;
                }
            }
        }
        return retVal;
    }
    private Matrix eAdd(Matrix a, double b, bool add)
    {
        Matrix retVal = new Matrix(Rows, columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (add)
                {
                    retVal[i, j] = this[i, j] += (float)b;
                }
                else
                {
                    retVal[i, j] = this[i, j] -= (float)b;
                }
            }
        }
        return retVal;
    }
    private Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.Columns == b.Rows)
        {
            Matrix retVal = new Matrix(Rows, b.columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    for (int k = 0; k < b.Columns; k++)
                    {
                        retVal[i, k] += a[i, j] * b[j, k];
                    }
                }
            }
            return retVal;
        }
        else
        {
            throw new System.Exception("Matrix size mismatch");
        }
    }
    private Matrix Scale(Matrix a, float b)
    {
        Matrix retVal = new Matrix(Rows, columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                retVal[i, j] = this[i, j] * b;
            }
        }
        return retVal;
    }
    private Matrix Scale(Matrix a, int b)
    {
        Matrix retVal = new Matrix(Rows, columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                retVal[i, j] = this[i, j] * b;
            }
        }
        return retVal;
    }
    private Matrix Scale(Matrix a, double b)
    {
        Matrix retVal = new Matrix(Rows, columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                retVal[i, j] = (float)(this[i, j] * b);
            }
        }
        return retVal;
    }

    public Matrix ScaleRow(int row, float scale)
    {
        Matrix retVal = new Matrix(Rows, columns);
        retVal = this.duplicate();
        for (int i = 0; i < Columns; i++)
        {
            retVal[row, i] *= scale;
        }
        return retVal;
    }
    public Matrix AddRow(int Rows, int rowD)
    {
        Matrix retVal = new Matrix(Rows, columns);
        retVal = this.duplicate();
        for (int i = 0; i < Columns; i++)
        {
            retVal[rowD, i] += retVal[Rows, i];
        }
        return retVal;
    }
    public Matrix SubtractRow(int Rows, int rowD)
    {
        Matrix retVal = new Matrix(Rows, columns);
        retVal = this.duplicate();
        for (int i = 0; i < Columns; i++)
        {
            retVal[rowD, i] -= retVal[Rows, i];
        }
        return retVal;
    }
    public Matrix duplicate()
    {
        Matrix retVal = new Matrix(Rows, columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                retVal[i, j] = this[i, j];
            }
        }
        return retVal;
    }
    public Matrix Inverse()
    {
        if (Rows == Columns)
        {
            Matrix retVal = new Matrix(Rows, columns);
            for (int i = 0; i < Rows; i++)
            {

                for (int j = i; j < Columns; j++)
                {

                }
            }
            return retVal;
        }
        else
        {
            throw new System.Exception("Cannot calculate inverse of a non-square matrix");
        }

    }

    public Matrix Transpose()
    {
        Matrix retVal = new Matrix(Columns, Rows);
        for(int i = 0; i < Columns; i++)
        {
            for(int j = 0; j < Rows; j++)
            {
                retVal[i, j] = this[j, i];
            }
        }
        return retVal;

    }
    public void SetRow(int row, params float[] n)
    {
        if (n.Length == Columns)
        {
            for (int i = 0; i < n.Length; i++)
            {
                values[row][i] = n[i];
            }
        }
    }
    public string print()
    {
        string retString = "\n";
        for (int i = 0; i < Rows; i++)
        {
            string line = "";
            for (int j = 0; j < Columns; j++)
            {
                line += values[i][j] + ",";
            }
            retString += line + "\n";
        }
        Debug.Log(retString);
        return retString;
    }
    public float[] toArray()
    {
        float[] retVal = new float[Rows * Columns];
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                retVal[i + j] = values[j][i];
            }
        }
        return retVal;
    }
}
