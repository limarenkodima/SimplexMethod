using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MetodSimplex.Configure
{
    public class Func
    {
        private int numerator;
        private int denominator;
        private int sign;

        public int Numerator { get => numerator; set => numerator = value; }
        public int Denominator { get => denominator; set => denominator = value; }
        public int Sign { get => sign; set => sign = value; }

        public Func() { }
        public Func(int num, int denom)
        {
            if (num < 0)
            {
                sign = -1;
                numerator = -num;
            }
            else
            {
                sign = 1;
                numerator = num;
            }
            denominator = denom;
        }
        public Func(int k)
        {
            numerator = Math.Abs(k);
            denominator = 1;
            sign = k < 0 ? -1 : 1;
        }
        public Func(string str)
        {
            str.Replace(" ", "");
            if (str.StartsWith("-"))
            {
                sign = -1;
                str = str.Remove(0, 1);
            }
            else
                sign = 1;

            //целые числа
            if (!str.Contains(",") && !str.Contains(".") && !str.Contains("/"))
            {
                int k = Convert.ToInt32(str);
                this.numerator = k;
                this.denominator = 1;
            }

            //десятичные дроби
            else if (str.Contains(",") || str.Contains("."))
            {
                string[] str1 = str.Split(new char[] { ',', '.' });
                if (str1.Length != 2)
                {
                    throw new Exception();
                }
                else
                {
                    numerator = Convert.ToInt32(str1[0] + str1[1]);
                    denominator = Convert.ToInt32(Math.Pow(10, str1[1].Length));
                }
            }
            else if (str.Contains("/"))
            {
                string[] str1 = str.Split(new char[] { '/' });
                if (str1.Length != 2 || Convert.ToInt32(str1[1]) == 0)
                {
                    throw new Exception();
                    //MessageBox.Show("Некорректное значение. Дробь с нулем в знаменателе.");
                }
                else
                {
                    try
                    {
                        numerator = Convert.ToInt32(str1[0]);
                        denominator = Convert.ToInt32(str1[1]);
                    }
                    catch (Exception e)
                    {
                        throw new Exception();
                        //MessageBox.Show("Некорректное значение. Дробь с нулем в знаменателе.");
                    }
                }
            }
            else
                throw new Exception();
        }

        //переопределяем операторы
        #region Операторы сложения и вычитания
        public static Func operator +(Func a, Func b)
        {
            int c = a.sign * a.numerator * b.denominator + b.sign * b.numerator * a.denominator;
            int d = a.denominator * b.denominator;
            return new Func(c, d);
        }
        public static Func operator +(int a, Func b)
        { return new Func(a) + b; }

        public static Func operator +(Func a, int b)
        { return b + a; }

        public static Func operator -(Func a, Func b)
        {
            Func c = new Func();
            int k = a.sign * a.numerator * b.denominator - b.sign * (b.numerator * a.denominator);
            c.numerator = Math.Abs(k);
            c.denominator = a.denominator * b.denominator;
            c.sign = (k > 0) ? 1 : -1;
            return c;
        }

        public static Func operator -(int a, Func b)
        { return new Func(a) - b; }

        public static Func operator -(Func a, int b)
        { return b - a; }

        #endregion
        #region Операторы деления и умножения
        public static Func operator /(Func a, Func b)

        {
            Func c = new Func();
            c.numerator = a.numerator * b.denominator;
            c.denominator = a.denominator * b.numerator;
            c.sign = a.sign * b.sign;
            return c;
        }
        public static Func operator /(int a, Func b)
        { return new Func(a) / b; }

        public static Func operator /(Func a, int b)
        { return b / a; }

        public static Func operator *(Func a, Func b)
        {
            Func c = new Func();
            c.numerator = a.numerator * b.numerator;
            c.denominator = a.denominator * b.denominator;
            c.sign = a.sign * b.sign;
            return c;
        }
        public static Func operator *(int a, Func b)
        { return new Func(a) * b; }

        public static Func operator *(Func a, int b)
        { return b * a; }

        #endregion
        public override string ToString()
        {
            Reduce();
            if (numerator == 0)
            {
                return "0";
            }
            string result;
            if (sign < 0)
            {
                result = "-";
            }
            else
            {
                result = "";
            }
            if (numerator == denominator)
            {
                return result + "1";
            }
            if (denominator == 1)
            {
                return result + numerator;
            }
            return result + numerator + "/" + denominator;
        }



        //наибольший общий делитель
        public int GCD(int c, int b)
        {

            while (b != 0)
            {
                int d = c % b;
                c = b;
                b = d;
            }
            return c;
        }

        //наименьшее общее кратное
        public int SCM(int a, int b)
        {
            return a * b / GCD(a, b);
        }
        //сокращение дроби
        public Func Reduce()
        {
            Func result = this;
            if (result.numerator == 0)
            {
                return result;
            }
            int k = GCD(result.numerator, result.denominator);
            result.denominator /= k;
            result.numerator /= k;
            return result;
        }

        // Перегрузка оператора "унарный минус"
        public static Func operator -(Func a)
        {
            return a.GetWithChangedSign();
        }
        // Перегрузка оператора "++"
        public static Func operator ++(Func a)
        {
            return a + 1;
        }
        // Перегрузка оператора "--"
        public static Func operator --(Func a)
        {
            return a - 1;
        }
        // Возвращает дробь с противоположным знаком
        private Func GetWithChangedSign()
        {
            return new Func(-numerator * sign, denominator);
        }
        // Мой метод Equals
        public bool Equals(Func that)
        {
            Func a = Reduce();
            Func b = that.Reduce();
            return a.numerator == b.numerator &&
            a.denominator == b.denominator &&
            a.sign == b.sign;
        }
        // Переопределение метода Equals
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Func)
            {
                result = this.Equals(obj as Func);
            }
            return result;
        }
        // Переопределение метода GetHashCode
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.sign * (this.numerator * this.numerator + this.denominator * this.denominator));
        }
        // Перегрузка оператора "Равенство" для двух дробей
        public static bool operator ==(Func a, Func b)
        {
            object aAsObj = a as object;
            object bAsObj = b as object;
            if (aAsObj == null || bAsObj == null)
            {
                return aAsObj == bAsObj;
            }
            return a.Equals(b);
        }
        // Перегрузка оператора "Равенство" для дроби и числа
        public static bool operator ==(Func a, int b)
        {
            return a == new Func(b);
        }
        // Перегрузка оператора "Равенство" для числа и дроби
        public static bool operator ==(int a, Func b)
        {
            return new Func(a) == b;
        }
        // Перегрузка оператора "Неравенство" для двух дробей
        public static bool operator !=(Func a, Func b)
        {
            return !(a == b);
        }
        // Перегрузка оператора "Неравенство" для дроби и числа
        public static bool operator !=(Func a, int b)
        {
            return a != new Func(b);
        }
        // Перегрузка оператора "Неравенство" для числа и дроби
        public static bool operator !=(int a, Func b)
        {
            return new Func(a) != b;
        }
        #region Операторы сравнения
        // Метод сравнения двух дробей
        // Возвращает	 0, если дроби равны
        //				 1, если this больше that
        //				-1, если this меньше that
        private int CompareTo(Func that)
        {
            if (Equals(that))
            {
                return 0;
            }
            Func a = Reduce();
            Func b = that.Reduce();
            int m = a.numerator * a.sign * b.denominator;
            int k = b.numerator * b.sign * a.denominator;
            if (a.numerator * a.sign * b.denominator > b.numerator * b.sign * a.denominator)
            {
                //int m = a.numerator * a.sign * b.denominator;
                //int k = b.numerator * b.sign * a.denominator;
                return 1;
            }
            return -1;
        }
        // Перегрузка оператора ">" для двух дробей
        public static bool operator >(Func a, Func b)
        {
            return a.CompareTo(b) > 0;
        }
        // Перегрузка оператора ">" для дроби и числа
        public static bool operator >(Func a, int b)
        {
            return a > new Func(b);
        }
        // Перегрузка оператора ">" для числа и дроби
        public static bool operator >(int a, Func b)
        {
            return new Func(a) > b;
        }
        // Перегрузка оператора "<" для двух дробей
        public static bool operator <(Func a, Func b)
        {
            return a.CompareTo(b) < 0;
        }
        // Перегрузка оператора "<" для дроби и числа
        public static bool operator <(Func a, int b)
        {
            return a < new Func(b);
        }
        // Перегрузка оператора "<" для числа и дроби
        public static bool operator <(int a, Func b)
        {
            return new Func(a) < b;
        }
        // Перегрузка оператора ">=" для двух дробей
        public static bool operator >=(Func a, Func b)
        {
            return a.CompareTo(b) >= 0;
        }
        // Перегрузка оператора ">=" для дроби и числа
        public static bool operator >=(Func a, int b)
        {
            return a >= new Func(b);
        }
        // Перегрузка оператора ">=" для числа и дроби
        public static bool operator >=(int a, Func b)
        {
            return new Func(a) >= b;
        }
        // Перегрузка оператора "<=" для двух дробей
        public static bool operator <=(Func a, Func b)
        {
            return a.CompareTo(b) <= 0;
        }
        // Перегрузка оператора "<=" для дроби и числа
        public static bool operator <=(Func a, int b)
        {
            return a <= new Func(b);
        }
        // Перегрузка оператора "<=" для числа и дроби
        public static bool operator <=(int a, Func b)
        {
            return new Func(a) <= b;
        }
        #endregion
    }
}
