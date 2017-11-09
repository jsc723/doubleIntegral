using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace doubleIntegral
{
    
    // 分数类
    public struct frac  
    {
        public int X;
        public  int Y;
        public double value
        {
            get { return (double)X / Y; }
        }
        public frac(int a, int b = 1)
        {
            X = a; Y = b;
        }
        public frac(frac s)
        {
            X = s.X;
            Y = s.Y;
        }
        public override string ToString()
        {
            if (Y == 1 || X == 0)
                return string.Format("+({0})", X);
            else if (X > 0 && Y > 0 || X < 0 && Y < 0)
                return string.Format("+({0}/{1})", X, Y);
            else if (X < 0 && Y > 0)
                return string.Format("-({0}/{1})", -1 * X, Y);
            else
                return string.Format("-({0}/{1})", X, -1 * Y);
        }
        
        static public int gcd(int a, int b)
        {
            return (a % b == 0) ? b : gcd(b, a % b);
        }
        
        //加法运算a/b+c/d
        static public frac sum(int a, int b, int c, int d)
        {
            if (b == 0 || d == 0)
                throw new Exception("Devide by 0!");
            int x = (a * d + b * c) / gcd(a * d + b * c, b * d);
            int y = (b * d) / gcd(a * d + b * c, b * d);
            return new frac(x, y);
        }
        static public frac sum(frac frac1, frac frac2)
        { 
            return sum(frac1.X, frac1.Y, frac2.X, frac2.Y); 
        }
        public static frac operator +(frac lhs, frac rhs)
        {
            return sum(lhs.X, lhs.Y, rhs.X, rhs.Y);
        }
        //减法运算
        static public frac sub(int a, int b, int c, int d)
        {
            frac frac0 = sum(a, b, -1 * c, d); 
            return frac0;
        }

        static public frac sub(frac frac1, frac frac2)
        {
            frac frac0 = sum(frac1.X, frac1.Y, -1 * frac2.X, frac2.Y); return frac0;
        }
        public static frac operator -(frac lhs, frac rhs)
        {
            return sub(lhs.X, lhs.Y, rhs.X, rhs.Y);
        }
        //乘法运算
        static public frac mul(int a, int b, int c, int d)
        {
            if (b == 0 || d == 0)
                throw new Exception("Devide by 0!");
            int x = a * c / gcd(a * c, b * d);
            int y = b * d / gcd(a * c, b * d); 
            return new frac(x, y);
        }
        static public frac mul(frac frac1, frac frac2)
        {
            return mul(frac1.X, frac1.Y, frac2.X, frac2.Y); 
        }
        public static frac operator *(frac lhs, frac rhs)
        {
            return mul(lhs.X, lhs.Y, rhs.X, rhs.Y);
        }
        //除法运算
        static public frac dev(int a, int b, int c, int d)
        { 
            return mul(a, b, d, c);
        }
        static public frac dev(frac frac1, frac frac2)
        {
            return mul(frac1.X, frac1.Y, frac2.Y, frac2.X);
        }
        public static frac operator /(frac lhs, frac rhs)
        {
            return dev(lhs.X, lhs.Y, rhs.X, rhs.Y);
        }
        public static frac pow(frac f, int p)
        {
            frac ret = new frac(1);
            for (int i = 0; i < p; i++)
                ret *= f;
            return ret;
        }
        public static bool operator ==(frac f1, frac f2)
        {
            return f1.value == f2.value;
        }
        public static bool operator !=(frac f1, frac f2)
        {
            return f1.value != f2.value;
        }
        
        //打印分数
        public void print()
        {
            Console.WriteLine("{0}",this);
        }
    }
}
