using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GRBL_Plotter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = MainForm.textfCTBCode;            
        }
          
        private void button1_Click(object sender, EventArgs e)
        {            
            vypocitaj(-100);            // počítaj aj menšie z ako vstupná hodnota
        }
        public void vypocitaj(int vyskaAjMensia)
        {

            int poziciaPolaSuradnice = 0;
            int riadok = 1;
            String[,] riadkyStlpce;
            string subor;
            double cisloZ = 0;
            double cisloX = 0;
            double cisloY = 0;
            double cisloI = 0;
            double cisloJ = 0;
            double obvod = 0;
            double vyskaPocitania = Convert.ToDouble(numericUpDown1.Value);
            double vyskaPocitaniaMenejAko = Convert.ToDouble(numericUpDown2.Value);
            string poslednaG = "";       // pre riadky, v ktorých sa nemení G súradnica a nie je uvedená
            double predoslaX = 0;
            double predoslaY = 0;
            double dolnaZ = 0;
            double vzdialenost;
            double celkovaVzdialenost = 0;            
            double uhol = 0;    
            byte quartal = 1;
            if (textBox1.Text != "")
            {
                subor = (textBox1.Text);
                string[] otvorSubor = Regex.Split(subor, "\r\n"); //File.ReadAllLines(fCTBCode.Text);
                foreach (string linia in otvorSubor)
                {
                    riadok++;
                }
                riadkyStlpce = new string[riadok, 16];
                riadok = 0;

                // naparsovanie textu do polí riadkov a stĺpcov
                foreach (string linia in otvorSubor)
                {
                    char[] znaky = new char[linia.Length];
                    znaky = linia.ToCharArray();
                    string komentar = "";
                    for (int i = 0; i < znaky.Length; i++)
                    {
                        if (znaky[i] == '(')
                        {
                            int pocetOtvaracichZatvoriek = 0;
                            int pocetZatvaracichZatvoriek = 0;
                            i--;
                            do
                            {
                                i++;
                                komentar += znaky[i];
                                if (znaky[i] == '(')
                                {
                                    pocetOtvaracichZatvoriek++;
                                }
                                if (znaky[i] == ')')
                                {
                                    pocetZatvaracichZatvoriek++;
                                }


                            }
                            while (pocetOtvaracichZatvoriek != pocetZatvaracichZatvoriek || znaky[i] == '(');

                            /*     if(znaky[i] == ')' && pocetOtvaracichZatvoriek == pocetZatvaracichZatvoriek) {
                                     riadkyStlpce[riadok, 14] = komentar;
                                 }
                                 */
                        }
                        switch (znaky[i])
                        {
                            case 'G':

                                if (riadkyStlpce[riadok, 0] == null)
                                {
                                    poziciaPolaSuradnice = 0;
                                }
                                else
                                {
                                    riadkyStlpce[riadok, 0] += " G";
                                }
                                continue;

                            case 'M':
                                poziciaPolaSuradnice = 1;
                                continue;

                            case 'X':
                                poziciaPolaSuradnice = 2;
                                continue;

                            case 'Y':
                                poziciaPolaSuradnice = 3;
                                continue;

                            case 'Z':
                                poziciaPolaSuradnice = 4;
                                continue;

                            case 'C':
                                poziciaPolaSuradnice = 5;
                                continue;

                            case 'I':
                                poziciaPolaSuradnice = 6;
                                continue;

                            case 'J':
                                poziciaPolaSuradnice = 7;
                                continue;

                            case 'F':
                                poziciaPolaSuradnice = 8;
                                continue;

                            case 'S':
                                poziciaPolaSuradnice = 9;
                                continue;

                            case 'T':
                                poziciaPolaSuradnice = 10;
                                continue;

                            case ')':
                                riadkyStlpce[riadok, 14] = komentar;
                                continue;

                            default:
                                string znakyString = znaky[i].ToString();
                                if (znakyString != null && znakyString != " ")
                                    riadkyStlpce[riadok, poziciaPolaSuradnice] += znakyString;
                                continue;
                        }
                    }
                    riadok++;

                }                

                // otváranie riadkov a stĺpcov
                for (int r = 0; r < riadok; r++)
                {
                    //inicializácia premenných, úprava desatinnej čiarky
                    string G = riadkyStlpce[r, 0];
                    if (G != null)
                    {
                        G = G.Replace('.', ',');
                    }
                    if (G == null)
                    {
                        G = poslednaG;
                    }
                    string X = riadkyStlpce[r, 2];
                    if (X != null)
                    {
                        X = X.Replace('.', ',');
                    }
                    string Y = riadkyStlpce[r, 3];
                    if (Y != null)
                    {
                        Y = Y.Replace('.', ',');
                    }

                    string Z = riadkyStlpce[r, 4];
                    if (Z != null)
                    {
                        Z = Z.Replace('.', ',');
                    }
                    string I = riadkyStlpce[r, 6];
                    if (I != null)
                    {
                        I = I.Replace('.', ',');
                    }

                    string J = riadkyStlpce[r, 7];
                    if (J != null)
                    {
                        J = J.Replace('.', ',');
                    }

                    if (X != null)
                    {
                        cisloX = Convert.ToDouble(X);
                    }
                    if (Y != null)
                    {
                        cisloY = Convert.ToDouble(Y);
                    }
                    if (Z != null)
                    {
                        cisloZ = Convert.ToDouble(Z);
                    }

                    if (G == "2" || G == "02" || G == "3" || G == "03")
                    {
                        if (I != null || J != null)
                        {
                            if (I != null)
                            {
                                cisloI = Double.Parse(I);
                            }
                            if (I == null)
                            {
                                cisloI = 0;
                            }
                            if (J != null)
                            {
                                cisloJ = Double.Parse(J);
                            }
                            if (J == null)
                            {
                                cisloJ = 0;
                            }
                        }

                        if (true)
                        {
                            if(vyskaAjMensia == -100)
                            {
                                if (cisloZ >= vyskaPocitaniaMenejAko)
                                {
                                    dolnaZ = 0;                                    
                                }
                                else if (cisloZ < vyskaPocitaniaMenejAko)
                                {
                                    dolnaZ = 1;
                                }
                            }
                            else
                            {
                                if (cisloZ > vyskaPocitania || cisloZ < vyskaPocitania)
                                { 
                                    dolnaZ = 0;                                    
                                }
                                else if (cisloZ == vyskaPocitania)
                                {
                                    dolnaZ = 1;

                                }
                            }
                        }

                        //predoslaX predoslaY     - štartovcí bod

                        Double stredX = predoslaX + cisloI;
                        Double stredY = predoslaY + cisloJ;

                        Double polomer = Math.Sqrt(Math.Pow((stredX - predoslaX), 2) + Math.Pow((stredY - predoslaY), 2));
                        //Double vzdialenost2 = Math.Sqrt(Math.Pow((stredX - cisloX), 2) + Math.Pow((stredY - cisloY), 2));
                        Double vzdialenostStartKonec = Math.Sqrt(Math.Pow((predoslaX - cisloX), 2) + Math.Pow((predoslaY - cisloY), 2));



                        //MessageBox.Show( "Vzdialenosť 1: "+ polomer + "Vzdialenosť 2: "+ vzdialenost2 + "rozdiel je: " + rozdiel);

                        if (G == "2" || G == "02") // v smere hod. ručičiek
                        {
                            double bodNaproti180X = predoslaX + (2 * cisloI);
                            double bodNaproti180Y = predoslaY + (2 * cisloJ);
                            /*
                             rozdelenie na quartly. Quartály priraďujem k prvému bodu"predoslaX,predoslaY"

                             II |  I
                            ____|____
                                |
                            III |  IV

                            */

                            if (predoslaX >= stredX && predoslaY > stredY) { quartal = 1; }
                            if (predoslaX < stredX && predoslaY >= stredY) { quartal = 2; }
                            if (predoslaX <= stredX && predoslaY < stredY) { quartal = 3; }
                            if (predoslaX > stredX && predoslaY <= stredY) { quartal = 4; }

                            //bod vľavo alebo vpravo do 180st.?
                            if (quartal == 1)
                            {
                                if (cisloX > bodNaproti180X && cisloY < predoslaY)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }

                                }
                                if (cisloX < predoslaX && cisloY > bodNaproti180Y)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);
                                    uhol = 360 - uhol;

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                    
                                }
                            }

                            if (quartal == 2)
                            {
                                if (cisloX > predoslaX && cisloY > bodNaproti180Y)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }

                                if (cisloX < bodNaproti180X && cisloY < predoslaY)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);
                                    uhol = 360 - uhol;

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }
                            }
                            if (quartal == 3)
                            {
                                if (cisloX < bodNaproti180X && cisloY > predoslaY)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }

                                if (cisloX > predoslaX && cisloY < bodNaproti180Y)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);
                                    uhol = 360 - uhol;

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }
                            }
                            if (quartal == 4)
                            {
                                if (cisloX < predoslaX && cisloY < bodNaproti180Y)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }

                                if (cisloX > bodNaproti180X && cisloY > predoslaY)
                                {
                                    Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                    double radiany = Math.Asin(sinusUhla);
                                    uhol = RadianyNaStupne(radiany);
                                    uhol = uhol * 2;
                                    uhol = Math.Round(uhol, 3);
                                    uhol = Math.Abs(uhol);
                                    uhol = 360 - uhol;

                                    obvod = 2 * Math.PI * polomer * (uhol / 360);

                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }
                            }



                            if (bodNaproti180X == cisloX && bodNaproti180Y == cisloY)
                            {
                                obvod = (2 * Math.PI * polomer) / 2;
                                if (dolnaZ == 1)
                                {
                                    celkovaVzdialenost = celkovaVzdialenost + obvod;
                                }
                            }

                            //360
                            else if (predoslaX == cisloX && predoslaY == cisloY)
                            {
                                obvod = 2 * Math.PI * polomer;
                                if (dolnaZ == 1)
                                {
                                    celkovaVzdialenost = celkovaVzdialenost + obvod;
                                }
                            }
                            else
                            {
                                // error
                            }
                        }
                        if (G == "3" || G == "03") // v protismere hod. ručičiek
                        {
                            double bodNaproti180X = predoslaX + (2 * cisloI);
                            double bodNaproti180Y = predoslaY + (2 * cisloJ);
                            /*
                               rozdelenie na quartly. Quartály priraďujem k prvému bodu"predoslaX,predoslaY"

                               II |  I
                              ____|____
                                  |
                              III |  IV

                              */

                            if (predoslaX >= stredX && predoslaY > stredY) { quartal = 1; }
                            if (predoslaX < stredX && predoslaY >= stredY) { quartal = 2; }
                            if (predoslaX <= stredX && predoslaY < stredY) { quartal = 3; }
                            if (predoslaX > stredX && predoslaY <= stredY) { quartal = 4; }
                            if (true)
                            {

                                if (quartal == 1)
                                {
                                    if (cisloX < predoslaX && cisloY > bodNaproti180Y)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                        // MessageBox.Show("Uhol je: " + uhol + "obvod je: "+obvod);

                                    }
                                    if (cisloX > bodNaproti180X && cisloY < predoslaY)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);
                                        uhol = 360 - uhol;

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                        //MessageBox.Show("Uhol je: " + uhol);
                                    }
                                }

                                else if (quartal == 2)
                                {
                                    if (cisloX < bodNaproti180X && cisloY < predoslaY)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                    }

                                    if (cisloX > predoslaX && cisloY > bodNaproti180Y)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);
                                        uhol = 360 - uhol;

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                    }
                                }
                                else if (quartal == 3)
                                {
                                    if (cisloX > predoslaX && cisloY < bodNaproti180Y)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                    }

                                    if (cisloX < bodNaproti180X && cisloY > predoslaY)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);
                                        uhol = 360 - uhol;

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                    }
                                }
                                else if (quartal == 4)
                                {
                                    if (cisloX > bodNaproti180X && cisloY > predoslaY)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                    }

                                    if (cisloX < predoslaX && cisloY < bodNaproti180Y)
                                    {
                                        Double sinusUhla = (vzdialenostStartKonec / 2) / polomer;
                                        double radiany = Math.Asin(sinusUhla);
                                        uhol = RadianyNaStupne(radiany);
                                        uhol = uhol * 2;
                                        uhol = Math.Round(uhol, 3);
                                        uhol = Math.Abs(uhol);
                                        uhol = 360 - uhol;

                                        obvod = 2 * Math.PI * polomer * (uhol / 360);

                                        if (dolnaZ == 1)
                                        {
                                            celkovaVzdialenost = celkovaVzdialenost + obvod;
                                        }
                                    }
                                }


                                //180
                                else if (bodNaproti180X == cisloX && bodNaproti180Y == cisloY)
                                {
                                    obvod = (2 * Math.PI * polomer) / 2;
                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }

                                //360
                                else if (predoslaX == cisloX && predoslaY == cisloY)
                                {
                                    obvod = 2 * Math.PI * polomer;
                                    if (dolnaZ == 1)
                                    {
                                        celkovaVzdialenost = celkovaVzdialenost + obvod;
                                    }
                                }
                                else
                                {
                                    // error
                                }
                            }

                        }
                        //MessageBox.Show("Príkazy G2 a G3 nie sú podporované", "Chyba pri spracovávaní G-kódu, riadok " + riadok,
                        /*  MessageBox.Show("Príkazy G2 a G3 nie sú pripočítané", "Chyba pri spracovávaní riadku " + riadok,
                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation
  );
                          break;*/
                        predoslaX = cisloX;
                        predoslaY = cisloY;                        
                    }

                    if (G == "0" || G == "00" || G == "1" || G == "01")
                    {
                        
                            if(vyskaAjMensia == -100)
                            {
                                if (cisloZ >= vyskaPocitaniaMenejAko)
                                { 
                                    dolnaZ = 0;                                   
                                }
                                else if (cisloZ < vyskaPocitaniaMenejAko)
                                {
                                    dolnaZ = 1;
                                }
                            }
                            else
                            {
                                if (cisloZ > vyskaPocitania || cisloZ < vyskaPocitania)
                                { 
                                    dolnaZ = 0;                                    
                                }
                                else if (cisloZ == vyskaPocitania)
                                {
                                    dolnaZ = 1;

                                }
                            }                           
                        
                        
                        if (X != null || Y != null)
                        {
                            if (dolnaZ == 1)
                            {
                                vzdialenost = Math.Sqrt(Math.Pow((cisloX - predoslaX), 2) + Math.Pow((cisloY - predoslaY), 2));
                                celkovaVzdialenost = celkovaVzdialenost + vzdialenost;
                                //MessageBox.Show("Dĺžka dráhy vyrezávania je: " + celkovaVzdialenost/1000 + " metrov.");
                            }
                            predoslaX = cisloX;
                            predoslaY = cisloY;
                        }
                    }
                    poslednaG = G;

                    if (riadok == r + 2)
                    {
                        MessageBox.Show("Dĺžka dráhy vyrezávania je: " + celkovaVzdialenost / 1000 + " metrov.");
                    }
                }
            }
        }

        private double RadianyNaStupne(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            vypocitaj(0);
        }
    }
}
