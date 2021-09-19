using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MetodSimplex.Visual;

namespace MetodSimplex.Configure
{
    public class Table : ICloneable
    {
        private Matrix matrix;
        private List<Func> basis;
        private List<int> basisVariables;
        private List<int> freeVariables;
        private List<Func> Window2;
        private List<Func> function;
        public bool isWindow2;
        public bool isArtifical;
        public List<bool> ArtificalBasis;
        private List<Func> old_function;
        int kindOfWindow2 = -1;

        internal List<Func> Answer { get => Window2; set => Window2 = value; }
        internal Matrix Matrix { get => matrix; set => matrix = value; }
        public List<int> BasisVariables { get => basisVariables; set => basisVariables = value; }
        public List<int> FreeVariables { get => freeVariables; set => freeVariables = value; }
        public List<Func> Function { get => function; set => function = value; }
        public List<Func> Basis { get => basis; set => basis = value; }
        public int KindOfWindow2 { get => kindOfWindow2; set => kindOfWindow2 = value; }
        public List<Func> Old_function { get => old_function; set => old_function = value; }

        public Table()
        {
        }

        public Table(Table table1, bool isFirst, bool artifical)
        {
            int i;
            Func l;
            matrix = table1.matrix;
            basis = table1.basis;
            function = table1.function;
            old_function = table1.old_function;
            basisVariables = table1.basisVariables;
            freeVariables = table1.freeVariables;
            kindOfWindow2 = table1.kindOfWindow2;
            Window2 = table1.Window2;
            ArtificalBasis = table1.ArtificalBasis;
            if (artifical)
            {
                isArtifical = true;
                if (isFirst)
                {
                    ArtificalBasis = new List<bool>();
                    isArtifical = true;
                    for (i = 0; i < matrix.values[0].Count - 1 - matrix.values.Count; i++)
                    {
                        ArtificalBasis.Add(false);
                    }
                    for (int j = i; j < matrix.values[0].Count - 1; j++)
                    {
                        ArtificalBasis.Add(true);
                    }
                }
            }
            isWindow2 = true;

            List<List<Func>> values = matrix.values;

            if (isFirst)
            {
                Window2 = new List<Func>();
                basisVariables = new List<int>();
                freeVariables = new List<int>();
                for (i = 0; i < basis.Count; i++)
                {
                    if (basis[i] != 0)
                    {
                        basisVariables.Add(i);
                    }
                    else freeVariables.Add(i);
                }

                for (i = 0; i < values.Count(); i++)
                {

                    for (int j = 0; j < values.Count(); j++)
                    {
                        values[i].Remove(values[i][0]);
                    }
                }
                values.Add(new List<Func>());

                Window2 = new List<Func>();
                for (i = 0; i < values[0].Count() - 1; i++)
                {

                    Window2.Add(new Func(0));
                    for (int j = 0; j < values.Count - 1; j++)
                    {
                        Func z = values[j][i];
                        Func w = function[basisVariables[j]];
                        Window2[i] += values[j][i] * function[basisVariables[j]];
                    }

                    l = new Func(this.function[freeVariables[i]].ToString() + "");
                    l.Sign *= -1;
                    Window2[i] += l;
                    Window2[i].Sign *= -1;
                }

                int m = i;
                Window2.Add(new Func(0));
                for (int j = 0; j < values.Count - 1; j++)
                {
                    Window2[m] += values[j][m] * this.function[basisVariables[j]];
                }
                Window2[values[0].Count() - 1].Sign *= -1;

                values[values.Count - 1] = Window2;
            }
            this.matrix = new Matrix(values);
            //MessageBox.Show(matrix.ToString());

        }

        public Table StepSimplexMethod(int row, int col)
        {
            //вектор базисных переменных
            //ищем опорный элемент
            if (row == -1 && col == -1)
            {
                int j = 0, i = 0;
                col = 0;
                row = 0;
                Func min = new Func(-1);
                bool edge = true;
                for (i = 0; i < matrix.values[0].Count; i++)
                {
                    edge = true;
                    for (j = 0; j < matrix.values.Count; j++)
                    {
                        if (matrix.values[j][i] >= 0)
                        {
                            edge = false;
                            break;
                        }
                    }
                    if (edge)
                    {
                        isWindow2 = true;
                        kindOfWindow2 = 0;
                        return this;
                    }
                }

                col = FindMinCol();
                if (col == -1)
                {
                    isWindow2 = true;
                    kindOfWindow2 = 1;
                    return this;
                }
                //ищем минимальное соотношение в столбце
                row = FindRow(col);
                if (row == -1)
                {
                    isWindow2 = true;
                    kindOfWindow2 = 0;
                    return this;
                }

            }
            if (row != -1 && col != -1)
                isWindow2 = false;
            if (row == -1 || col == -1)
            {
                isWindow2 = true;
                kindOfWindow2 = 0;
                return this;
            }
            //обработать строку
            List<List<Func>> table = new List<List<Func>>();
            List<Func> table_row;
            for (int i = 0; i < matrix.values.Count; i++)
            {
                table_row = new List<Func>();
                table.Add(table_row);
            }

            List<Func> support_row = new List<Func>();
            for (int i = 0; i < matrix.values[row].Count; i++)
            {
                if (i == col)
                {
                    support_row.Add(1 / matrix.values[row][col]);
                    continue;
                }
                else
                {
                    support_row.Add(matrix.values[row][i] / matrix.values[row][col]);
                }
            }
            table[row] = support_row;
            //обработать все остальное
            for (int i = 0; i < matrix.values.Count; i++)
            {
                if (i == row)
                    continue;

                for (int j = 0; j < matrix.values[0].Count; j++)
                {
                    if (j == col)
                    {
                        Func z = new Func(matrix.values[row][col].ToString());
                        z.Sign *= -1;
                        Func m = matrix.values[i][j];
                        Func t = matrix.values[i][j] / z;
                        table[i].Add(t);
                        continue;
                    }
                    Func temp = matrix.values[i][col] * table[row][j];
                    temp -= matrix.values[i][j];
                    temp.Sign *= -1;
                    table[i].Add(temp);
                }
            }
            
            matrix = new Matrix(table);
            int k = basisVariables[row];
            basisVariables[row] = freeVariables[col];
            freeVariables[col] = k;


            //поменять местами переменные в базисе и свободную
            if (isArtifical && ArtificalBasis[freeVariables[col]])
            {
                for (int i = 0; i < matrix.values.Count; i++)
                    matrix.values[i].RemoveAt(col);
                freeVariables.RemoveAt(col);

            }

            return this;
        }
        public int FindMinCol()
        {
            bool isWindow2 = true;
            int last_row = matrix.values.Count - 1;
            int min_col = -1, i = 0;
            int max = matrix.values[last_row].Count - 1;
            for (i = 0; i < max; i++)
            {
                if (min_col == -1)
                {
                    Func x = matrix.values[last_row][i];
                    Func m = new Func("-0");
                    if (matrix.values[last_row][i] < new Func("-0") && matrix.values[last_row][i].Numerator != 0)
                    {
                        min_col = i;
                        isWindow2 = false;
                    }
                }
                else if (matrix.values[last_row][i] <= matrix.values[last_row][min_col] && matrix.values[last_row][i] < 0)
                {
                    Func m = matrix.values[last_row][i];
                    min_col = i;
                    isWindow2 = false;
                }
            }
            if (isWindow2)
            {
                kindOfWindow2 = 1;
                return -1;
            }
            isWindow2 = true;
            for (i = 0; i < matrix.values.Count; i++)
            {
                if (matrix.values[i][min_col] > 0)
                {
                    isWindow2 = false;
                    break;
                }
            }
            if (isWindow2)
            {
                kindOfWindow2 = 0;
                return -1;
            }
            return min_col;
        }
        public int IsEndOfArtificialBasis()
        {
            int max_row = matrix.values.Count - 1;
            int max_col = matrix.values[max_row].Count - 1;
            for (int i = 0; i < max_col; i++)
            {
                if (matrix.values[max_row][i].Sign == -1)
                    return 0;
            }
            if (matrix.values[max_row][max_col].Numerator != 0)
            {
                return -1;
            }
            return 1;
        }
        public int FindRow(int col)
        {
            if (col == -1)
                return -1;
            int i, j, row = -1;
            Func min = new Func("-1");
            Func m = new Func();
            for (i = 0; i < matrix.values.Count - 1; i++)
            {
                if (matrix.values[i][col] > 0)
                {
                    if (min == -1)
                    {
                        min = matrix.values[i][matrix.values[i].Count - 1] / matrix.values[i][col];
                        row = i;
                    }
                    else
                    {
                        m = matrix.values[i][matrix.values[i].Count - 1] / matrix.values[i][col];
                        if (m < min && (m >= 0 || m.Numerator == 0))
                        {
                            min = m;
                            row = i;
                        }
                    }
                }
            }

            return row;
        }
        public void MakeFunction()
        {
            int i;
            Window2 = new List<Func>();
            for (i = 0; i < matrix.values[0].Count() - 1; i++)
           
            {

                Window2.Add(new Func(0));
                for (int j = 0; j < matrix.values.Count - 1; j++)
                {
                    Func z = matrix.values[j][i];
                    Func w = old_function[basisVariables[j]];
                    Window2[i] += matrix.values[j][i] * old_function[basisVariables[j]];
                }

                Func l = new Func(this.old_function[freeVariables[i]].ToString() + "");
                l.Sign *= -1;
                Window2[i] += l;
                Window2[i].Sign *= -1;
            }

            int m = i;
            Window2.Add(new Func(0));
            for (int j = 0; j < matrix.values.Count - 1; j++)
            {
                Window2[m] += matrix.values[j][m] * this.old_function[basisVariables[j]];
            }
            Window2[matrix.values[0].Count() - 1].Sign *= -1;
            matrix.values[matrix.values.Count - 1] = Window2;
        }

        public object Clone()
        {
            return new Table
            {
                Window2 = this.Window2,
                Matrix = this.Matrix,
                BasisVariables = this.BasisVariables,
                FreeVariables = this.FreeVariables,
                Function = this.Function,
                Basis = this.Basis,
                KindOfWindow2 = this.KindOfWindow2,
                Old_function = this.Old_function,
                ArtificalBasis = this.ArtificalBasis,
                isWindow2 = this.isWindow2,
                isArtifical = this.isArtifical

            };
        }
    }

}
