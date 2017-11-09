using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace doubleIntegral
{
    class term
    {
        public frac c;
        public int xp;
        public int yp;
        public term(int coe, int x_p=0, int y_p=0)
        {
            c = new frac(coe); xp = x_p; yp = y_p;
        }
        public term(frac coe, int x_p=0, int y_p=0)
        {
            c = coe; xp = x_p; yp = y_p;
        }
        public term(term t)
        {
            c = t.c; xp = t.xp; yp = t.yp;
        }
        public term integralY()
        {
            term temp = new term(this);
            temp.yp++;
            temp.c /= new frac(temp.yp);
            return temp;
        }
        public poly substitudeY(poly y_x)
        {
            for (int i = 0; i < y_x.count; i++)
                if (y_x.terms[i].yp > 0)
                    throw new Exception("y_x is not a function of x!");
            poly temp = new poly();
            temp.addTerm(this);
            for (int i = 0; i < yp; i++)
            {
                for (int j = 0; j < temp.count; j++)
                    temp.terms[j].yp--;
                temp *= y_x;
            }
            for (int i = 0; i < temp.count; i++)
                if (temp.terms[i].yp > 0)
                    throw new Exception("yp > 0 after substitude!");
            return temp;
        }
        public term integralX()
        {
            term temp = new term(this);
            temp.xp++;
            temp.c /= new frac(temp.xp);
            return temp;
        }
        public poly substitudeX(poly x_y)
        {
            for (int i = 0; i < x_y.count; i++)
                if (x_y.terms[i].xp > 0)
                    throw new Exception("x_y is not a function of x!");
            poly temp = new poly();
            temp.addTerm(this);
            for (int i = 0; i < xp; i++)
            {
                for (int j = 0; j < temp.count; j++)
                    temp.terms[j].xp--;
                temp *= x_y;
            }
            for (int i = 0; i < temp.count; i++)
                if (temp.terms[i].xp > 0)
                    throw new Exception("xp > 0 after substitude!");
            return temp;
        }
        public frac value(frac x, frac y)
        {
            frac a = frac.pow(x, xp);
            frac b = frac.pow(y, yp);
            return new frac(a * b * c);
        }
        public frac value(int x, int y)
        {
            frac a = frac.pow(new frac(x), xp);
            frac b = frac.pow(new frac(y), yp);
            return new frac(a * b * c);
        }
        public static term operator *(term t1, term t2)
        {
            return new term(t1.c * t2.c, t1.xp + t2.xp, t1.yp + t2.yp);
        }
        public override string ToString()
        {
            return string.Format("{0}x{1}y{2}", c, xp, yp);
        }
        public void print()
        {
            Console.WriteLine("{0}", this);
        }
    }
    class poly
    {
        public term[] terms = new term[30];
        public int count;
        public poly()
        {
            count = 0;
        }
        public poly(term t)
        {
            addTerm(t);
        }
        public poly(poly p)
        {
            count = p.count;
            for (int i = 0; i < count; i++)
                terms[i] = new term(p.terms[i]);
        }
        public int canJoin(ref term t)
        {
            for (int i = 0; i < count; i++)
            {
                if (t.xp == terms[i].xp && t.yp == terms[i].yp)
                    return i;
            }
            return -1;
        }
        public void addTerm(term t)
        {
            int i;
            frac zero = new frac(0);
            if ((i = canJoin(ref t))>=0)
            {
                terms[i].c += t.c;
                if (terms[i].c == zero)
                    delTerm(i);
            }
            else if (count < terms.Length)
            {
                terms[count] = new term(t);
                count++;
                if (terms[count-1].c == zero)
                    delTerm(count-1);
            }
            else
                throw new Exception("poly is full!");
        }
        public void delTerm(int index)
        {
            for (int i = index ; i < count-1; i++)
            {
                terms[i] = terms[i + 1];
            }
            count--;
        }
        public void extend(poly p)
        {
            for (int i = 0; i < p.count; i++)
                this.addTerm(p.terms[i]);
        }
        public poly substitudeY(poly y_x)
        {
            poly temp = new poly();
            for (int i = 0; i < count; i++)
            {
                temp += terms[i].substitudeY(y_x);
            }
            return temp;
        }
        public poly substitudeX(poly x_y)
        {
            poly temp = new poly();
            for (int i = 0; i < count; i++)
            {
                temp += terms[i].substitudeX(x_y);
            }
            return temp;
        }
        public poly integralX()
        {
            poly temp = new poly();
            for (int i = 0; i < count; i++)
            {
                temp.addTerm(terms[i].integralX());
            }
            return temp;
        }
        public poly integralY()
        {
            poly temp = new poly();
            for (int i = 0; i < count; i++)
            {
                temp.addTerm(terms[i].integralY());
            }
            return temp;
        }
        public frac value(frac x, frac y)
        {
            frac s = new frac(0);
            for (int i = 0; i < count; i++)
            {
                s += terms[i].value(x, y);
            }
            return s;
        }
        public frac value(int x, int y)
        {
            frac s = new frac(0);
            for (int i = 0; i < count; i++)
            {
                s += terms[i].value(x, y);
            }
            return s;
        }
        public static poly operator +(poly p1, poly p2)
        {
            poly a = new poly();
            a.extend(p1);
            a.extend(p2);
            return a;
        }
        public static poly operator -(poly p1, poly p2)
        {
            poly a = new poly();
            term neg = new term(-1);
            a.extend(p1);
            a.extend(neg*p2);
            return a;
        }
        public static poly operator *(term t, poly p)
        {
            poly result = new poly(p);
            for (int i = 0; i < p.count; i++)
            {
                result.terms[i] *= t;
            }
            return result;
        }
        public static poly operator *(poly p,term t)
        {
            poly result = new poly(p);
            for (int i = 0; i < p.count; i++)
            {
                result.terms[i] *= t;
            }
            return result;
        }
        public static poly operator *(poly p1, poly p2)
        {
            poly result = new poly();
            for (int i = 0; i < p1.count; i++)
            {
                result += p1.terms[i] * p2;
            }
            return result;
        }
        
        public override string ToString()
        {
            string s = "";
            for(int i=0;i<count;i++)
            {
                s += terms[i].ToString() + " ";
            }
            return s;
        }
        public void print()
        {
            Console.WriteLine(this);
        }
    }
}
