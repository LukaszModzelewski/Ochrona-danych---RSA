using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class RSA
    {
        private BigInteger c;
        private BigInteger rsa_d;
        private BigInteger rsa_n;
        private string msg;

        public BigInteger C { get { return c; } }
        public BigInteger Rsa_d { get { return rsa_d; } }
        public BigInteger Rsa_n { get { return rsa_n; } }

        public RSA(string msg)
        {
            this.msg = msg;
        }

        public BigInteger euklides(BigInteger e, BigInteger phi)
        {
            BigInteger x = BigInteger.Zero;
            BigInteger y = BigInteger.One;
            BigInteger lastx = BigInteger.One;
            BigInteger lasty = BigInteger.Zero;
            BigInteger temp;
            BigInteger phiOriginal = phi;

            while (!phi.Equals(BigInteger.Zero))
            {
                BigInteger q = BigInteger.Divide(e, phi);
                BigInteger r = e % phi;

                e = phi;
                phi = r;

                temp = x;
                x = BigInteger.Subtract(lastx, BigInteger.Multiply(q, x));
                lastx = temp;

                temp = y;
                y = BigInteger.Subtract(lasty, BigInteger.Multiply(q, y));
                lasty = temp;
            }
            if (lastx.CompareTo(BigInteger.Zero) > 0)
                return lastx;
            else
                return BigInteger.Subtract(phiOriginal, BigInteger.Negate(lastx));
        }

        public string Encrypt()
        {
            string p = "8954937300280438804244939471563100247837695704192373673128222819901439388583431335500737707702230367892764837438054304217178172911151362857419350093873829";
            string q = "11326960495280396859963951277476202902476795535605385467047045654941363025487995731611833231534039530362375614741751500907989679189162164468639573092827123";

            BigInteger rsa_p = BigInteger.Parse(p);
            BigInteger rsa_q = BigInteger.Parse(q);

            rsa_n = BigInteger.Multiply(rsa_p, rsa_q);

            BigInteger rsa_fn = BigInteger.Multiply((rsa_p - 1), (rsa_q - 1));

            BigInteger rsa_e = 13;

            rsa_d = euklides(rsa_e, rsa_fn);


            string message = msg;
            byte[] buffer2 = Encoding.Unicode.GetBytes(message);
            BigInteger m = new BigInteger(buffer2);
            c = BigInteger.ModPow(m, rsa_e, rsa_n);
            return Convert.ToBase64String(C.ToByteArray());
        }

        public string Descrypt(BigInteger C, BigInteger rsa_d, BigInteger rsa_n)
        {
            BigInteger M = BigInteger.ModPow(C, rsa_d, rsa_n);

            byte[] decoded2 = M.ToByteArray();

            if (decoded2[0] == 0)
            {
                decoded2 = decoded2.Where(b => b != 0).ToArray();
            }
            string message3 = UnicodeEncoding.Unicode.GetString(decoded2);
            return message3;
        }
    }
}
