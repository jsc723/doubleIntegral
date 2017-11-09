using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace doubleIntegral
{
    class Program
    {
        static frac fra(int n, int d = 1) { return new frac(n, d); }
        static term ter(int coe, int x_p = 0, int y_p = 0) { return new term(coe, x_p, y_p); }
        static term ter(frac coe, int x_p = 0, int y_p = 0) { return new term(coe, x_p, y_p); }
        static poly pol(term t) { return new poly(t); }
        static poly pol(int[,] termData)
        {
            poly p = new poly();
            for (int i = 0; i < termData.GetLength(0); i++)
            {
                p.addTerm(ter(termData[i, 0], termData[i, 1], termData[i, 2]));
            }
            return p;
        }
        static frac doubleItgYX(poly f, poly y_x0, poly y_x1, frac x0, frac x1)
        {
            frac res = new frac(0);
            f.print();
            poly iy_f = f.integralY();
            iy_f.print();
            poly f_x = iy_f.substitudeY(y_x1) - iy_f.substitudeY(y_x0);
            f_x.print();
            poly F = f_x.integralX();
            F.print();
            res = F.value(x1, new frac(0)) - F.value(x0, new frac(0));
            return res;
        }
        static void test()
        {
            poly f = pol(new int[,] { { 1, 1, 0 }, { 1, 0, 1 } });
            poly x = new poly(ter(fra(1, 6), 1));
            poly y_x1 = pol(new int[,] { { 3, 0, 0 }, { -1, 1, 0 } });
            poly y_x0 = new poly(ter(fra(1, 2), 1));
            doubleItgYX(x * f, y_x0, y_x1, fra(0), fra(2)).print();
        }
        static void test2()
        {
            poly f = pol(ter(2, 2, 1));
            poly z = pol(new int[,] { { 4, 0, 0 }, { -1, 2, 0 }, { -1, 0, 2 } });
            f.extend(pol(ter(2, 0, 2)) * z);
            f.extend(pol(ter(1, 1, 0)) * z);
            doubleItgYX(f, pol(ter(0)), pol(ter(1)), fra(0), fra(1)).print();
        }
        static void Main(string[] args)
        {
            test2();
            Console.ReadKey();
        }
    }
}
