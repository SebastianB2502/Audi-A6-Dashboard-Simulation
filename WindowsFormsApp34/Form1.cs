using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp34
{
    public partial class Form1 : Form
    {
        private turom turometru;
        private turom vitezometru;
        private bit semnalStanga;
        private bit semnalDreapta;
        private Timer timerInstrumente;
        private Timer timerSemnalizare;
        private bool instrumentePornite = false;
        private bool tastaturaPornita = false;
        private double valoareTurometru = 0;
        private double valoareVitezometru = 0;
        private bool accelerare = false;
        private Button btnStartStop;
        private Label labelAudi;
        private BufferedPanel panelInstrumente;
        private bool stangaPornit = false;
        private bool dreaptaPornit = false;
        private bool avariePornit = false;
        private double nivelCombustibil = 100; // Nivelul initial de combustibil (100%)
        private double temperatura = 50; // Temperatura initiala (50°C)
        private int treaptaCurenta = 0; // 0 km/h = P (Park), 1-8 = trepte
        private Label labelTreapta; // Label pentru afisarea treptei curente

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            SetupComponents();
        }

        private void SetupComponents()
        {
            // Creez un panel pentru double buffering
            panelInstrumente = new BufferedPanel();
            panelInstrumente.Dock = DockStyle.Fill;
            panelInstrumente.Paint += Panel_Paint;
            panelInstrumente.BackColor = Color.Black;
            this.Controls.Add(panelInstrumente);
         
            timerInstrumente = new Timer();
            timerInstrumente.Interval = 50;
            timerInstrumente.Tick += TimerInstrumente_Tick;

            // Configurare timer pentru semnalizare
            timerSemnalizare = new Timer();
            timerSemnalizare.Interval = 500;
            timerSemnalizare.Tick += TimerSemnalizare_Tick;

            // Configurare buton Start/Stop
            btnStartStop = new Button();
            btnStartStop.Text = "Start/Stop";
            btnStartStop.Location = new Point(475, 600);
            btnStartStop.Size = new Size(150, 40);
            btnStartStop.BackColor = Color.White;
            btnStartStop.Click += BtnStartStop_Click;
            panelInstrumente.Controls.Add(btnStartStop);

            // Text Audi
            labelAudi = new Label();
            labelAudi.Text = "Audi";
            labelAudi.Font = new Font("Arial", 32, FontStyle.Bold);
            labelAudi.ForeColor = Color.White;
            labelAudi.BackColor = Color.Transparent;
            labelAudi.AutoSize = true;
            labelAudi.Location = new Point(480, 80);
            panelInstrumente.Controls.Add(labelAudi);

            // Label pentru treapta curentă
            labelTreapta = new Label();
            labelTreapta.Text = "P";
            labelTreapta.Font = new Font("Arial", 32, FontStyle.Bold);
            labelTreapta.ForeColor = Color.White;
            labelTreapta.BackColor = Color.Transparent;
            labelTreapta.AutoSize = true;
            labelTreapta.Location = new Point(490, 500);
            panelInstrumente.Controls.Add(labelTreapta);

            // Text A6
            Label labelA6 = new Label();
            labelA6.Text = "A6";
            labelA6.Font = new Font("Arial", 32, FontStyle.Bold);
            labelA6.ForeColor = Color.White;
            labelA6.BackColor = Color.Transparent;
            labelA6.AutoSize = true;
            labelA6.Location = new Point(490, 140);
            panelInstrumente.Controls.Add(labelA6);

            
            this.Size = new Size(1100, 700);
            this.BackColor = Color.Black;

            
            timerInstrumente.Start();
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            // indicatoare de temperatura
            DrawTemperatureIndicators(g, 20, 100);

            // indicatoarele pentru combustibil
            DrawFuelIndicators(g, 1020, 100);

            // turometru (stânga)
            turometru = new turom(g, 100, 50, 350, 135, 270, 40, 0, 6, "x1000/min");
            turometru.setval(valoareTurometru);

            // vitezometru (dreapta)
            vitezometru = new turom(g, 650, 50, 350, 135, 270, 27, 0, 300, "km/h");
            vitezometru.setval(valoareVitezometru);

            //  semnalizari
            semnalStanga = new bit(g, 150, 500, 50, 3, Color.LightGreen);
            semnalDreapta = new bit(g, 900, 500, 50, 4, Color.LightGreen);

            semnalStanga.setval(stangaPornit);
            semnalDreapta.setval(dreaptaPornit);
        }

        private void DrawTemperatureIndicators(Graphics g, int x, int y)
        {
            int width = 15;
            int height = 250;
            int spacing = 10;
            int segments = 8;
            int segmentHeight = (height - (segments - 1) * spacing) / segments;

            // Segmente aprinse in functie de temperatura
            int segmenteAprinse = (int)((temperatura - 50) / 10); // 50°C este minimul, fiecare segment reprezintă 10°C
            if (segmenteAprinse < 0) segmenteAprinse = 0;
            if (segmenteAprinse > 8) segmenteAprinse = 8;

            // Culori pentru segmente - toate albe
            Color[] colors = new Color[] {
                Color.White,   
                Color.White,  
                Color.White,   
                Color.White,   
                Color.White,   
                Color.White,   
                Color.White,   
                Color.White    
            };

            // Desenez segmentele de jos în sus
            for (int i = segments - 1; i >= 0; i--)
            {
                int yPos = y + i * (segmentHeight + spacing);
                using (var brush = new SolidBrush(i >= segments - segmenteAprinse ? colors[i] : Color.FromArgb(40, 40, 40)))
                {
                    g.FillRectangle(brush, x, yPos, width, segmentHeight);
                }
            }

            // valorile de temperatura
            using (var font = new Font("Arial", 12, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.White))
            {
                g.DrawString("130", font, brush, x + width + 4, y);
                g.DrawString("90", font, brush, x + width + 4, y + height/2 - 10);
                g.DrawString("50", font, brush, x + width + 4, y + height - 20);
            }
        }

        private void DrawFuelIndicators(Graphics g, int x, int y)
        {
             int width = 15;
            int height = 250;
            int spacing = 10;
            int segments = 8;
            int segmentHeight = (height - (segments - 1) * spacing) / segments;

            // Segmente aprinse in functie de nivelul de combustibil
            int segmenteAprinse = (int)(nivelCombustibil / 12.5); // 100% / 8 segmente = 12.5% per segment
            if (segmenteAprinse < 0) segmenteAprinse = 0;
            if (segmenteAprinse > 8) segmenteAprinse = 8;

            // Culori pentru segmente - alb sus, roșu si portocaliu jos
            Color[] colors = new Color[] {
                Color.White,    
                Color.White,    
                Color.White,    
                Color.White,    
                Color.White,
                Color.Orange,     
                Color.OrangeRed,     
                Color.Red       
            };

            // Desenez segmentele de sus in jos
            for (int i = segments - 1; i >= 0; i--)
            {
                int yPos = y + i * (segmentHeight + spacing);
                using (var brush = new SolidBrush(i >= segments - segmenteAprinse ? colors[i] : Color.FromArgb(40, 40, 40)))
                {
                    g.FillRectangle(brush, x, yPos, width, segmentHeight);
                }
            }
            
            using (var font = new Font("Arial", 12, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.White))
            {
                g.DrawString("F", font, brush, x - 20, y); // F - Full 
                g.DrawString("R", font, brush, x - 20, y + height - 20); // R - Reserve
            }
        }

        private void TimerSemnalizare_Tick(object sender, EventArgs e)
        {
            if (avariePornit)
            {
                stangaPornit = !stangaPornit;
                dreaptaPornit = !dreaptaPornit;
            }
            else if (stangaPornit)
            {
                stangaPornit = !stangaPornit;
            }
            else if (dreaptaPornit)
            {
                dreaptaPornit = !dreaptaPornit;
            }
            panelInstrumente.Invalidate();
        }

        private void TimerInstrumente_Tick(object sender, EventArgs e)
        {
            if (tastaturaPornita && accelerare)
            {
                // Crestere turometru in functie de treapta
                if (valoareTurometru < 6)
                {
                    // Creste turatia in functie de treapta
                    double rataCresterii = 0.05;
                    if (treaptaCurenta > 0)
                    {
                        // Crestere turatie pana la 5900 RPM
                        if (valoareTurometru < 5.9)
                        {
                            rataCresterii = 0.08;
                        }
                        // Scade rapid la 2000 RPM cand se schimba treapta
                        else if (valoareTurometru > 5.9)
                        {
                            rataCresterii = -0.3;
                        }
                        // Creste din nou dupa ce scade la 2000 RPM
                        else if (valoareTurometru < 2.0)
                        {
                            rataCresterii = 0.08;
                        }
                    }
                    valoareTurometru += rataCresterii;

                    if (valoareTurometru >= 1.0) // Incepe sa creasca viteza dupa 1000 RPM
                    {
                        // Crestere viteza in functie de treapta si turatie
                        double multiplicatorViteza = 0.8;
                        if (treaptaCurenta > 0)
                        {
                            // Ajustam multiplicatorul in functie de treapta
                            switch (treaptaCurenta)
                            {
                                case 1:
                                    multiplicatorViteza = 0.4; // Crestere mai lenta in prima treapta
                                    break;
                                case 2:
                                    multiplicatorViteza = 0.5;
                                    break;
                                case 3:
                                    multiplicatorViteza = 0.6;
                                    break;
                                case 4:
                                    multiplicatorViteza = 0.7;
                                    break;
                                case 5:
                                    multiplicatorViteza = 0.8;
                                    break;
                                case 6:
                                    multiplicatorViteza = 0.9;
                                    break;
                                case 7:
                                    multiplicatorViteza = 1.0;
                                    break;
                                case 8:
                                    multiplicatorViteza = 1.1; // Crestere mai rapida in ultima treapta
                                    break;
                            }
                        }
                        valoareVitezometru += (valoareTurometru * multiplicatorViteza);
                        
                        // Scade combustibilul in functie de viteza si treapta
                        nivelCombustibil -= (valoareVitezometru * 0.001 * (1 + treaptaCurenta * 0.1));
                        if (nivelCombustibil < 0) nivelCombustibil = 0;
                        
                        // Creste temperatura in functie de viteza si treapta
                        if (temperatura < 90)
                        {
                            temperatura += (valoareVitezometru * 0.005 * (1 + treaptaCurenta * 0.05));
                            if (temperatura > 90) temperatura = 90;
                        }

                        // Schimbare automata a treptelor in functie de viteza si turatie
                        if (valoareTurometru >= 2.5) // Schimbam treapta doar daca turatia este suficienta
                        {
                            if (valoareVitezometru >= 0 && valoareVitezometru < 20)
                                treaptaCurenta = 1;
                            else if (valoareVitezometru >= 20 && valoareVitezometru < 50)
                                treaptaCurenta = 2;
                            else if (valoareVitezometru >= 50 && valoareVitezometru < 80)
                                treaptaCurenta = 3;
                            else if (valoareVitezometru >= 80 && valoareVitezometru < 120)
                                treaptaCurenta = 4;
                            else if (valoareVitezometru >= 120 && valoareVitezometru < 160)
                                treaptaCurenta = 5;
                            else if (valoareVitezometru >= 160 && valoareVitezometru < 200)
                                treaptaCurenta = 6;
                            else if (valoareVitezometru >= 200 && valoareVitezometru < 240)
                                treaptaCurenta = 7;
                            else if (valoareVitezometru >= 240 && valoareVitezometru < 300)
                                treaptaCurenta = 8;
                        }

                        
                        if (treaptaCurenta == 0)
                            labelTreapta.Text = "P";
                        else
                            labelTreapta.Text = "D" + treaptaCurenta;
                    }
                }
                else
                {
                    valoareTurometru = 6;
                }

                if (valoareVitezometru > 300)
                {
                    valoareVitezometru = 300;
                }
            }
            else
            {
                // Scadere naturala a turometrului
                if (valoareTurometru > 0.8)
                {
                    valoareTurometru -= 0.15;
                }
                else if (valoareTurometru > 0)
                {
                    valoareTurometru -= 0.05;
                }

                // Scadere naturala a vitezei
                if (valoareVitezometru > 0)
                {
                    valoareVitezometru -= 1.5;
                }

                // Scadere naturala a temperaturii
                if (temperatura > 50)
                {
                    temperatura -= 0.03;
                }

                if (valoareTurometru < 0) valoareTurometru = 0;
                if (valoareVitezometru < 0) valoareVitezometru = 0;

                // Actualizarea treptei in functie de viteza cand scade
                if (valoareVitezometru > 0)
                {
                    if (valoareVitezometru >= 0 && valoareVitezometru < 20)
                        treaptaCurenta = 1;
                    else if (valoareVitezometru >= 20 && valoareVitezometru < 50)
                        treaptaCurenta = 2;
                    else if (valoareVitezometru >= 50 && valoareVitezometru < 80)
                        treaptaCurenta = 3;
                    else if (valoareVitezometru >= 80 && valoareVitezometru < 120)
                        treaptaCurenta = 4;
                    else if (valoareVitezometru >= 120 && valoareVitezometru < 160)
                        treaptaCurenta = 5;
                    else if (valoareVitezometru >= 160 && valoareVitezometru < 200)
                        treaptaCurenta = 6;
                    else if (valoareVitezometru >= 200 && valoareVitezometru < 240)
                        treaptaCurenta = 7;
                    else if (valoareVitezometru >= 240 && valoareVitezometru < 300)
                        treaptaCurenta = 8;

                    // Actualizare afisaj treapta
                    labelTreapta.Text = "D" + treaptaCurenta;
                }
                else
                {
                    // Resetare treapta cand masina se opreste
                    treaptaCurenta = 0;
                    labelTreapta.Text = "P";
                }
            }

            panelInstrumente.Invalidate();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                accelerare = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!tastaturaPornita) return;

            switch (e.KeyCode)
            {
                case Keys.S:
                    if (!avariePornit)
                    {
                        OpresteSemnalizari();
                        stangaPornit = true;
                        timerSemnalizare.Start();
                    }
                    break;
                case Keys.D:
                    if (!avariePornit)
                    {
                        OpresteSemnalizari();
                        dreaptaPornit = true;
                        timerSemnalizare.Start();
                    }
                    break;
                case Keys.A:
                    OpresteSemnalizari();
                    avariePornit = true;
                    stangaPornit = true;
                    dreaptaPornit = true;
                    timerSemnalizare.Start();
                    break;
                case Keys.O:
                    OpresteSemnalizari();
                    break;
                case Keys.W:
                    accelerare = true;
                    break;
            }
            panelInstrumente.Invalidate();
        }

        private void OpresteSemnalizari()
        {
            timerSemnalizare.Stop();
            stangaPornit = false;
            dreaptaPornit = false;
            avariePornit = false;
            panelInstrumente.Invalidate();
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            tastaturaPornita = !tastaturaPornita;
            if (!tastaturaPornita)
            {
                OpresteSemnalizari();
                accelerare = false;
                treaptaCurenta = 0;
                labelTreapta.Text = "P";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    // Clasa pentru double buffering
    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }

    public class turom
    {
        System.Drawing.Graphics zona_des;
        System.Drawing.Bitmap img;
        Graphics g;
        System.Drawing.Pen creion_g = new System.Drawing.Pen(System.Drawing.Color.White, 2);
        System.Drawing.Pen creion_g2 = new System.Drawing.Pen(System.Drawing.Color.White, 3);
        System.Drawing.Pen creion_r = new System.Drawing.Pen(System.Drawing.Color.Red, 2);
        System.Drawing.Pen creion_r2 = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
        System.Drawing.Font font_arial = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
        System.Drawing.Font font_arial2 = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
        System.Drawing.SolidBrush pens_a = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        System.Drawing.SolidBrush pens_r = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
        System.Drawing.SolidBrush pens_rad = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        int x0;
        int y0;
        int wd;
        int alfa_st;
        int alfa_w;
        double vmin;
        double vmax;
        int gd;
        string um;

        public turom(System.Drawing.Graphics desen, int x, int y, int wi, int gr_st, int gr_w, int gr_d, double vmn, double vmx, string u_m)
        {
            zona_des = desen;
            x0 = x;
            y0 = y;
            wd = wi;
            alfa_st = gr_st;
            alfa_w = gr_w;
            gd = gr_d;
            vmin = vmn;
            vmax = vmx;
            um = u_m;
        }

        public void setval(double val)
        {
            int x1, x2, xt, y1, y2, yt;
            int xc = wd / 2;
            int yc = wd / 2;
            int raza = (wd - wd / 3) / 2;
            int sd = 0;
            int nrd = 0;
            double val_c = val;
            int val_gr;
            double val_a = vmin;
            double alfa_r = 0;
            int alfa_gr = 0;
            int lt = wd / 12;
            img = new Bitmap(wd + 2, wd + 2, zona_des);
            g = Graphics.FromImage(img);
            g.FillRectangle(pens_rad, 0, 0, wd + 2, wd + 2);


            using (var brush = new SolidBrush(Color.FromArgb(40, 40, 40)))
            {
                g.FillEllipse(brush, 10, 10, wd - 20, wd - 20);
            }

            // liniutele mici pentru ambele instrumente
            for (int grad = alfa_st; grad <= alfa_st + alfa_w; grad += 5)
            {
                double alfa_r_mic = 2 * Math.PI * (360 - grad) / 360;
                int x1_mic = Convert.ToInt32(xc + (wd / 2 - 10) * Math.Cos(alfa_r_mic));
                int y1_mic = Convert.ToInt32(yc - (wd / 2 - 10) * Math.Sin(alfa_r_mic));
                int x2_mic = xc + Convert.ToInt32((wd / 2 - lt/2 - 10) * Math.Cos(alfa_r_mic));
                int y2_mic = yc - Convert.ToInt32((wd / 2 - lt/2 - 10) * Math.Sin(alfa_r_mic));
                
                // Pentru turometru, ultimele 8 liniute mici sa fie rosii
                if (um == "x1000/min" && grad >= alfa_st + alfa_w - 40)
                {
                    g.DrawLine(creion_r, x1_mic, y1_mic, x2_mic, y2_mic);
                }
                // Pentru vitezometru, ultimele 5 liniute mici sa fie rosii
                else if (um == "km/h" && grad >= alfa_st + alfa_w - 20)
                {
                    g.DrawLine(creion_r, x1_mic, y1_mic, x2_mic, y2_mic);
                }
                else
                {
                    g.DrawLine(creion_g, x1_mic, y1_mic, x2_mic, y2_mic);
                }
            }

            alfa_r = 0;
            alfa_gr = alfa_st;
            while (alfa_gr <= alfa_st + alfa_w)
            {
                alfa_r = 2 * Math.PI * (360 - alfa_gr) / 360;
                x1 = Convert.ToInt32(xc + (wd / 2 - 10) * Math.Cos(alfa_r));
                y1 = Convert.ToInt32(yc - (wd / 2 - 10) * Math.Sin(alfa_r));
                x2 = xc + Convert.ToInt32((wd / 2 - lt - 10) * Math.Cos(alfa_r));
                y2 = yc - Convert.ToInt32((wd / 2 - lt - 10) * Math.Sin(alfa_r));
                xt = xc - lt / 2 + Convert.ToInt32((raza) * Math.Cos(alfa_r));
                yt = yc - 10 - Convert.ToInt32((raza) * Math.Sin(alfa_r));

                if (sd == 0)
                {
                    if (um == "x1000/min") // Pentru turometru
                    {
                        
                        if (val_a >= vmax - 0.5)
                            g.DrawLine(creion_r2, x1, y1, x2, y2);
                        else if (val_a == 6) 
                            continue;
                        else
                            g.DrawLine(creion_g2, x1, y1, x2, y2);
                                                   
                        if (Math.Round(val_a, 0) % 1 == 0 && val_a != 6)
                        {
                            g.DrawString(Math.Round(val_a, 0).ToString(), font_arial, pens_a, xt, yt);
                        }
                    }
                    else // Pentru vitezometru
                    {
                        g.DrawString(Math.Round(val_a, 0).ToString(), font_arial, pens_a, xt, yt);
                        if (val_a >= vmax - 30) 
                            g.DrawLine(creion_r2, x1, y1, x2, y2);
                        else
                            g.DrawLine(creion_g2, x1, y1, x2, y2);
                    }
                    sd = 1;
                }
                else
                {
                    if (um == "x1000/min" && val_a >= vmax - 0.5) // Doar pentru turometru
                        g.DrawLine(creion_r, x1, y1, x2, y2);
                    else if (um == "x1000/min" && val_a == 6) 
                        continue;
                    else
                        g.DrawLine(creion_g, x1, y1, x2, y2);
                    sd = 0;
                }
                
                alfa_gr += gd;
                nrd++;
                val_a = vmin + nrd * (vmax - vmin) / Convert.ToDouble(alfa_w / gd);
            }

            // Desenam linia pentru valoarea 6 și cifra 6 la final pentru turometru
            if (um == "x1000/min")
            {
                double alfa_r_6 = 2 * Math.PI * (360 - (alfa_st + alfa_w)) / 360;
                int x1_6 = Convert.ToInt32(xc + (wd / 2 - 10) * Math.Cos(alfa_r_6));
                int y1_6 = Convert.ToInt32(yc - (wd / 2 - 10) * Math.Sin(alfa_r_6));
                int x2_6 = xc + Convert.ToInt32((wd / 2 - lt - 10) * Math.Cos(alfa_r_6));
                int y2_6 = yc - Convert.ToInt32((wd / 2 - lt - 10) * Math.Sin(alfa_r_6));
                int xt_6 = xc - lt / 2 + Convert.ToInt32((raza) * Math.Cos(alfa_r_6));
                int yt_6 = yc - 10 - Convert.ToInt32((raza) * Math.Sin(alfa_r_6));
                
                g.DrawLine(creion_r2, x1_6, y1_6, x2_6, y2_6);
                g.DrawString("6", font_arial, pens_a, xt_6, yt_6);
            }

            // Desenez ac indicator
            val_gr = Convert.ToInt32(alfa_st + alfa_w * (val_c - vmin) / (vmax - vmin));
            alfa_r = 2 * Math.PI * (360 - val_gr) / 360;
            int x = xc + Convert.ToInt32((wd / 2 - 20) * Math.Cos(alfa_r));
            int y = yc - Convert.ToInt32((wd / 2 - 20) * Math.Sin(alfa_r));
            using (Pen shadowPen = new Pen(Color.FromArgb(100, 0, 0, 0), 4))
            {
                g.DrawLine(shadowPen, xc + 2, yc + 2, x + 2, y + 2);
            }
            g.DrawLine(creion_r2, xc, yc, x, y);

            // Desenez cerc central
            using (var brush = new SolidBrush(Color.Red))
            {
                g.FillEllipse(brush, (wd - lt) / 2, (wd - lt) / 2, lt, lt);
            }

            // Desenez valoare digitala
            if (wd > 100)
            {
                g.DrawString(Math.Round(val, 0).ToString(), font_arial2, pens_a, 
                    wd/2 - 10, wd/2 + 30);

                // Adaug textul "1/min x 1000" doar pentru turometru
                if (um == "x1000/min")
                {
                    using (var font = new Font("Arial", 10, FontStyle.Regular))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.DrawString("1/min x 1000", font, brush, wd/2 - 35, wd/2 + 80);                        
                        g.DrawString("1", font_arial, pens_a, wd/2 - 120, wd/4 + 90);
                        g.DrawString("3", font_arial, pens_a, wd/2 - 30, wd/6);
                        g.DrawString("5", font_arial, pens_a, wd/2 + 100, wd/4 + 40);
                    }
                }
                // Adaug textul "km/h" pentru vitezometru
                else if (um == "km/h")
                {
                    using (var font = new Font("Arial", 12, FontStyle.Regular))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.DrawString("km/h", font, brush, wd/2 - 20, wd/4);
                    }
                }
            }

            zona_des.DrawImage(img, x0, y0);
        }
    }

    public class bit
    {
        System.Drawing.Graphics zona_des;
        System.Drawing.SolidBrush pens_bit;
        System.Drawing.Bitmap img;
        Graphics g;
        System.Drawing.Pen creion_contur = new System.Drawing.Pen(System.Drawing.Color.DarkGreen);
        System.Drawing.SolidBrush pens_stins = new System.Drawing.SolidBrush(System.Drawing.Color.DarkGray);
        System.Drawing.SolidBrush pens_rad = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        int x0;
        int y0;
        int wd;
        int tp;

        public bit(System.Drawing.Graphics desen, int x, int y, int wi, int tip, Color culoare_bit)
        {
            zona_des = desen;
            pens_bit = new SolidBrush(culoare_bit);
            x0 = x;
            y0 = y;
            wd = wi;
            tp = tip;
        }

        public void setval(bool val)
        {
            img = new Bitmap(wd + 2, wd + 2, zona_des);
            g = Graphics.FromImage(img);

            g.FillRectangle(pens_rad, 0, 0, wd + 2, wd + 2);

            if (val) 
            {
                if (tp == 3) // Stanga
            {
                Point[] puncte = {
                    new Point(wd/2, 0),
                    new Point(wd/2, wd/4),
                    new Point(wd, wd/4),
                    new Point(wd, wd*3/4),
                    new Point(wd/2, wd*3/4),
                    new Point(wd/2, wd),
                    new Point(0, wd/2)
                };

                    g.FillPolygon(pens_bit, puncte);
                g.DrawPolygon(creion_contur, puncte);
            }
            else if (tp == 4) // Dreapta
            {
                Point[] puncte = {
                    new Point(0, wd/4),
                    new Point(wd/2, wd/4),
                    new Point(wd/2, 0),
                    new Point(wd, wd/2),
                    new Point(wd/2, wd),
                    new Point(wd/2, wd*3/4),
                    new Point(0, wd*3/4)
                };

                    g.FillPolygon(pens_bit, puncte);
                g.DrawPolygon(creion_contur, puncte);
                }
            }

            zona_des.DrawImage(img, x0, y0);
        }
    }
}
