using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MetodSimplex.Configure
{
    public class Matrix : ICloneable
    {
        public List<List<Func>> values;
        

        public Matrix() { }
        public Matrix(List<List<Func>> table)
        {
            this.values = table;
        }

        public int GaussMethod(List<Func> basis)
        {
            bool isBasis = false;
            bool isOnes = true;
            foreach (Func Func in basis)
            {
                if (Func != 0)
                {
                    isBasis = true;
                    if (Func != 1)
                        isOnes = false;
                }
            }
            if (isBasis)
            {
                if (SolveMatrix(basis, isOnes) == -1)
                    return -1;
                else return 1;
            }
            else
            {

                if (SolveMatrix() == -1)
                    return -1;
                else return 1;
            }

        }


        //поиск максимального элемента сортировкой по столбцу
        private void SortRows(int SortIndex)
        {
            Func MaxElement = values[SortIndex][SortIndex];
            int MaxElementIndex = SortIndex;
            for (int i = SortIndex + 1; i < values.Count(); i++)
            {
                if (values[i][SortIndex] > MaxElement)
                {
                    MaxElement = values[i][SortIndex];
                    MaxElementIndex = i;
                }
            }

            //теперь найден максимальный элемент ставим его на верхнее место
            if (MaxElementIndex > SortIndex)//если это не первый элемент
            {
                for (int i = 0; i < values[0].Count(); i++)
                {
                    Func Temp = values[MaxElementIndex][i];
                    values[MaxElementIndex][i] = values[SortIndex][i];
                    values[SortIndex][i] = Temp;
                }
            }
        }

        public int SolveMatrix()
        {
            for (int i = 0; i < Math.Min(values.Count(), values[0].Count); i++)
            {
                SortRows(i);
                Func m = values[i][i];
                for (int j = 0; j < values[0].Count; j++)
                {
                    values[i][j] /= m;
                }

                //прямой ход метода Гаусса
                for (int j = 0; j < values.Count(); j++)
                {
                    if (i != j && m != 0)
                    {
                        Func MultElement = values[j][i];
                        for (int k = i; k < values[0].Count(); k++)
                        {
                            values[j][k] -= values[i][k] * MultElement;
                        }
                    }
                    
                }
            }

            int z = values[0].Count() - 1;
            for (int i = 0; i < values.Count() - 1; i++)
            {
                if (values[i][z].Sign == -1)
                    return -1;
            }

            //ищем решение
            for (int i = (values.Count() - 1); i >= 0; i--)
            {
                for (int j = (values.Count() - 1); j > i; j--)
                    if (values[i][i] == 0)
                        if (values[values[0].Count()][i] == 0)
                            return 2; //множество решений
                        else
                            return 1; //нет решения
            }
            return 0;
        }

        public int SolveMatrix(List<Func> basis, bool isOnes)
        {
            int i;
            //переставить столбцы.
            List<Func> newBasis = new List<Func>();
            List<List<Func>> newMatrix = new List<List<Func>>();
            for (int k = 0; k < values[0].Count; k++)
            {
                newMatrix.Add(new List<Func>());
                for (int l = 0; l < values.Count; l++)
                {
                    newMatrix[k].Add(new Func());
                    newMatrix[k][l] = values[l][k];
                }
            }
            List<List<Func>> matrix = new List<List<Func>>();
            for (i = 0; i < values[0].Count - 1; i++)
            {
                if (basis[i] != 0)
                {
                    matrix.Add(newMatrix[i]);
                    newBasis.Add(new Func(i));
                }
            }
            for (i = 0; i < values[0].Count - 1; i++)
            {
                if (basis[i] == 0)
                {
                    matrix.Add(newMatrix[i]);
                    newBasis.Add(new Func(i));
                }
            }

            matrix.Add(newMatrix[i]);

            for (int k = 0; k < values.Count; k++)
            {
                for (int l = 0; l < values[0].Count; l++)
                {
                    values[k][l] = matrix[l][k];
                }
            }

            for (i = 0; i < values.Count(); i++)
            {
                SortRows(i);
                Func m = values[i][i];
                if (m.Numerator == 0)
                {
                    return -1;
                }
                for (int j = 0; j < values[0].Count; j++)
                {
                    values[i][j] /= m;
                }

                //прямой ход метода Гаусса
                for (int j = 0; j < values.Count(); j++)
                {
                    if (i != j && m != 0)
                    {
                        Func MultElement = values[j][i];
                        for (int k = i; k < values[0].Count(); k++)
                        {
                            values[j][k] -= values[i][k] * MultElement;
                        }
                    }
                   
                }
            }

            //если остался отрицательный свободный член, то вектор не подходит для решения
            int z = values[0].Count() - 1;
            for (i = 0; i < values.Count() - 1; i++)
            {
                if (values[i][z].Sign == -1)
                    return -1;
            }

            //ищем решение
            for (i = (values.Count() - 1); i >= 0; i--)
            {
                for (int j = (values.Count() - 1); j > i; j--)
                    if (values[i][i] == 0)
                        if (values[values[0].Count()][i] == 0)
                            return 2; //множество решений
                        else
                            return 1; //нет решения
            }
            if (isOnes)
            {
                return 0;
            }

            if (!isOnes)
            {
                int j = 0;
                //проверка на соответсвие
                for (i = 0; i < basis.Count; i++)
                {
                    if (basis[i] == 0)
                        continue;
                    else if (basis[i] != values[j][values[j].Count - 1])
                        return -1;
                    j++;
                }

            }
            return 0;


        }

        public override String ToString()
        {
            String S = "";
            for (int i = 0; i < values.Count; i++)
            {
                S += "\r\n";
                for (int j = 0; j < values[0].Count; j++)
                {
                    S += values[i][j].ToString() + "\t";
                }
            }
            return S;
        }


        public object Clone()
        {
            return new Matrix
            {
                values = this.values
            };
        }
    }
}

