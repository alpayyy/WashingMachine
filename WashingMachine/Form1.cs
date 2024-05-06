using System;
using System.Windows.Forms;


namespace WashingMachine
{
    public partial class Form1 : Form
    {
        private TrackBar trackBarHassaslik;
        private TrackBar trackBarMiktar;
        private TrackBar trackBarKirlilik;

        private enum Hassaslik { Dusuk, Orta, Yuksek }
        private enum Miktar { Az, Orta, Cok }
        private enum Kirlilik { Az, Orta, Cok }
        private enum Sure { Dusuk, Orta, Yuksek }
        private enum DonusHizi { Dusuk, Orta, Yuksek }

        private Panel resultPanel;

        public Form1()
        {
            InitializeComponent();
            InitializeTrackBars();
            InitializeResultPanel();
        }

        private void InitializeResultPanel()
        {
            resultPanel = new Panel();
            resultPanel.Size = new System.Drawing.Size(300, 150);
            resultPanel.Location = new System.Drawing.Point(this.ClientSize.Width / 2 - resultPanel.Width / 2, 250);
            resultPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(resultPanel);
        }

        private void ShowResult(string resultText)
        {
            Label resultLabel = new Label();
            resultLabel.Text = resultText;
            resultLabel.AutoSize = true;
            resultLabel.Location = new System.Drawing.Point(10, 10);
            resultPanel.Controls.Add(resultLabel);


        }
       

        private void InitializeTrackBars()
        {
            // Hassasl�k i�in kayd�rma �ubu�unu ba�lat ve s�n�f d�zeyindeki de�i�kene ata
            trackBarHassaslik = InitializeTrackBar("Hassasl�k", "lblHassaslik", lblHassaslik);

            // Miktar i�in kayd�rma �ubu�unu ba�lat ve s�n�f d�zeyindeki de�i�kene ata
            trackBarMiktar = InitializeTrackBar("Miktar", "lblMiktar", lblMiktar);

            // Kirlilik i�in kayd�rma �ubu�unu ba�lat ve s�n�f d�zeyindeki de�i�kene ata
            trackBarKirlilik = InitializeTrackBar("Kirlilik", "lblKirlilik", lblKirlilik);

            // Buton i�in olay atamas�
            Button btnCalculate = new Button();
            btnCalculate.Text = "Hesapla";
            btnCalculate.Size = new System.Drawing.Size(100, 30); // Buton boyutunu ayarla
            btnCalculate.Location = new System.Drawing.Point(this.ClientSize.Width / 2 - btnCalculate.Width / 2, 200); // Formun ortas�nda ve a�a��da yerle�tir
            btnCalculate.Click += BtnCalculate_Click;
            this.Controls.Add(btnCalculate);

            Button btnRecalculate = new Button();
            btnRecalculate.Text = "Tekrar Hesapla";
            btnRecalculate.Size = new System.Drawing.Size(100, 30); // Buton boyutunu ayarla
            btnRecalculate.Location = new System.Drawing.Point(btnCalculate.Right + 10, btnCalculate.Top); // Hesapla butonunun sa��na yerle�tir
            btnRecalculate.Click += BtnRecalculate_Click;
            this.Controls.Add(btnRecalculate);
        }

        private TrackBar InitializeTrackBar(string labelText, string labelName, Label label)
        { 

           

            // Label ve TrackBar olu�tur
            TrackBar trackBar = new TrackBar();
            trackBar.Minimum = 0;
            trackBar.Maximum = 10;
            trackBar.Value = 5; // Ba�lang�� de�eri
            trackBar.TickStyle = TickStyle.None; // �ste�e ba�l� olarak i�aret�i stilleri
            trackBar.Width = 200; // Geni�lik ayarla
            trackBar.Location = new System.Drawing.Point(label.Left, label.Bottom + 5); // Label'in alt�na yerle�tir

            // De�er de�i�ti�inde tetiklenecek olay� ekle
            trackBar.ValueChanged += (sender, e) =>
            {
                // TrackBar'dan de�eri al ve etikete yaz
                label.Text = $"{labelText} De�eri: {trackBar.Value}";
            };

            // Forma ekle
            this.Controls.Add(trackBar);

            return trackBar;
        }
        private void BtnRecalculate_Click(object sender, EventArgs e)
        {
            // �nce sonu� panelini temizle
            ClearResultPanel();

            // Yeniden hesaplama i�levini �a��r
            Recalculate();
        }

        private void ClearResultPanel()
        {
            // paneli temizle sonu�lar�
            resultPanel.Controls.Clear();
        }

        private void Recalculate()
        {
            // Giri� de�erlerini trackbarlardan al
            Hassaslik hassaslik = (Hassaslik)trackBarHassaslik.Value;
            Miktar miktar = (Miktar)trackBarMiktar.Value;
            Kirlilik kirlilik = (Kirlilik)trackBarKirlilik.Value;

            // Mamdani ��kar�m mekanizmas� ile sonu�lar� hesapla
            double deterjanMiktari = MamdaniDeterjanMiktari(hassaslik, miktar, kirlilik);
            double sure = MamdaniSure(hassaslik, miktar, kirlilik);
            double donusHizi = MamdaniDonusHizi(hassaslik, miktar, kirlilik);

            // A��rl�k merkezi (centroid) hesaplamas�
            double centroidDeterjanMiktari = CalculateCentroid(deterjanMiktari);
            double centroidSure = CalculateCentroid(sure);
            double centroidDonusHizi = CalculateCentroid(donusHizi);

            DrawGraph("Deterjan Miktar�", centroidDeterjanMiktari, Color.Blue, 0);
            DrawGraph("S�re", centroidSure, Color.Red, 150);
            DrawGraph("D�n�� H�z�", centroidDonusHizi, Color.Green, 300);




            // Sonu�lar� g�ster
            ShowResult($"Deterjan Miktar�: {deterjanMiktari}\nS�re: {sure}\nD�n�� H�z�: {donusHizi}");


            

        }
        // A��rl�k merkezi (centroid) hesaplamas�
        private double CalculateCentroid(double value)
        {
            double totalWeight = 0;
            double weightedSum = 0;

            // Her bir de�erin a��rl��� ve �arp�m� hesapla
            for (int i = 0; i <= 10; i++)
            {
                double weight = UyelikFonksiyonuKucuk(i) + UyelikFonksiyonuOrta(i) + UyelikFonksiyonuBuyuk(i);
                totalWeight += weight;
                weightedSum += weight * i;
            }

            // Toplam a��rl�klara b�lerek a��rl�k merkezini bul
            if (totalWeight == 0) 
                return 0;

            return weightedSum / totalWeight;
        }
        // Grafik �izme fonksiyonu
        private void DrawGraph(string label, double value, Color color, int yOffset)
        {
            int barWidth = 50; 
            int barHeight = (int)value * 10;
            int startX = 50; 
            int startY = 100 + yOffset; 
            using (Graphics g = resultPanel.CreateGraphics())
            {
                // �ubu�un �izimindeki y�kseklik ve geni�lik de�erlerindeki de�i�iklikleri uygula
                g.FillRectangle(new SolidBrush(color), startX, startY - barHeight, barWidth, barHeight);

                // Etiketi �ubu�a ba�lamak i�in x ve y koordinatlar�n� ayarla
                int labelX = startX + (barWidth - (int)g.MeasureString(label, Font).Width) / 2;
                int labelY = startY + 10;

                // Etiketi �iz
                g.DrawString(label, Font, Brushes.Black, labelX, labelY);
            }
        }




        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            // Giri� de�erlerini trackbarlardan al
            Hassaslik hassaslik = (Hassaslik)trackBarHassaslik.Value;
            Miktar miktar = (Miktar)trackBarMiktar.Value;
            Kirlilik kirlilik = (Kirlilik)trackBarKirlilik.Value;

            // Mamdani ��kar�m mekanizmas� ile sonu�lar� hesapla
            double deterjanMiktari = MamdaniDeterjanMiktari(hassaslik, miktar, kirlilik);
            double sure = MamdaniSure(hassaslik, miktar, kirlilik);
            double donusHizi = MamdaniDonusHizi(hassaslik, miktar, kirlilik);

            // A��rl�k merkezi (centroid) hesaplamas�
            //double centroidDeterjanMiktari = CalculateCentroid(deterjanMiktari);
            //double centroidSure = CalculateCentroid(sure);
            //double centroidDonusHizi = CalculateCentroid(donusHizi);

            // Sonu�lar� g�ster
            ShowResult($"Deterjan Miktar�: {deterjanMiktari}\nS�re: {sure}\nD�n�� H�z�: {donusHizi}");

            //DrawGraph("Deterjan Miktar�", centroidDeterjanMiktari, Color.Blue, 0);
            //DrawGraph("S�re", centroidSure, Color.Red, 150);
            //DrawGraph("D�n�� H�z�", centroidDonusHizi, Color.Green, 300);



        }


        private double MamdaniDeterjanMiktari(Hassaslik hassaslik, Miktar miktar, Kirlilik kirlilik)
        {
            // Deterjan miktar� kategorileri ve �yelik fonksiyonlar�
            double[,] deterjanMiktarUyelikFonksiyonlari = new double[3, 3]
            {
                { UyelikFonksiyonuKucuk((int)hassaslik), UyelikFonksiyonuKucuk((int)miktar), UyelikFonksiyonuKucuk((int)kirlilik) },
                { UyelikFonksiyonuOrta((int)hassaslik), UyelikFonksiyonuOrta((int)miktar), UyelikFonksiyonuOrta((int)kirlilik) },
                { UyelikFonksiyonuBuyuk((int)hassaslik), UyelikFonksiyonuBuyuk((int)miktar), UyelikFonksiyonuBuyuk((int)kirlilik) }
            };

            // Deterjan miktar� kategorileri ve �yelik de�erlerinin �arp�m�
            double[,] deterjanMiktarCarpimlari = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    deterjanMiktarCarpimlari[i, j] = deterjanMiktarUyelikFonksiyonlari[i, j] * MiktarUyelikDegeri((Miktar)i);
                }
            }

            // A��rl�kl� ortalama hesaplama
            double toplamUyelikDegeri = 0;
            double toplamCarpim = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    toplamUyelikDegeri += deterjanMiktarUyelikFonksiyonlari[i, j];
                    toplamCarpim += deterjanMiktarCarpimlari[i, j];
                }
            }

            if (toplamUyelikDegeri == 0) // S�f�ra b�lme hatas�n� engellemek i�in kontrol
                return 0;

            return toplamCarpim / toplamUyelikDegeri;
        }

        // Deterjan Miktar� i�in �yelik fonksiyonu - K���k
        private double UyelikFonksiyonuKucuk(int deger)
        {
            if (deger <= 3)
                return 1;
            else if (deger > 3 && deger < 5)
                return (5 - deger) / 2;
            else
                return 0;
        }

        private double UyelikFonksiyonuOrta(int deger)
        {
            if (deger >= 3 && deger <= 7)
                return Math.Min(deger - 3, 7 - deger) / 4;
            else
                return 0;
        }

        private double UyelikFonksiyonuBuyuk(int deger)
        {
            if (deger >= 5)
                return 1;
            else if (deger > 3 && deger < 5)
                return (deger - 3) / 2;
            else
                return 0;
        }
        // Miktar �yelik de�eri
        private double MiktarUyelikDegeri(Miktar miktar)
        {
            if (miktar == Miktar.Az)
                return 2;
            else if (miktar == Miktar.Orta)
                return 5;
            else
                return 8;
        }

        // Mamdani ��kar�m mekanizmas� ile S�re hesaplama
        private double MamdaniSure(Hassaslik hassaslik, Miktar miktar, Kirlilik kirlilik)
        {
            // S�re kategorileri ve �yelik fonksiyonlar�
            double[,] sureUyelikFonksiyonlari = new double[3, 3]
            {
                { UyelikFonksiyonuSureDusuk((int)hassaslik), UyelikFonksiyonuSureDusuk((int)miktar), UyelikFonksiyonuSureDusuk((int)kirlilik) },
                { UyelikFonksiyonuSureOrta((int) hassaslik), UyelikFonksiyonuSureOrta((int) miktar), UyelikFonksiyonuSureOrta((int) kirlilik) },
                { UyelikFonksiyonuSureYuksek((int) hassaslik), UyelikFonksiyonuSureYuksek((int) miktar), UyelikFonksiyonuSureYuksek((int) kirlilik) }
            };

            // S�re kategorileri ve �yelik de�erlerinin �arp�m�
            double[,] sureCarpimlari = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sureCarpimlari[i, j] = sureUyelikFonksiyonlari[i, j] * SureUyelikDegeri((Sure)i);
                }
            }

            // A��rl�kl� ortalama hesaplama
            double toplamUyelikDegeri = 0;
            double toplamCarpim = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    toplamUyelikDegeri += sureUyelikFonksiyonlari[i, j];
                    toplamCarpim += sureCarpimlari[i, j];
                }
            }

            if (toplamUyelikDegeri == 0) // S�f�ra b�lme hatas�n� engellemek i�in kontrol
                return 0;

            return toplamCarpim / toplamUyelikDegeri;
        }

        // S�re i�in �yelik fonksiyonu - D���k
        private double UyelikFonksiyonuSureDusuk(double deger)
        {
            if (deger <= 3)
                return 1;
            else if (deger > 3 && deger < 5)
                return (5 - deger) / 2;
            else
                return 0;
        }

        // S�re i�in �yelik fonksiyonu - Orta
        private double UyelikFonksiyonuSureOrta(double deger)
        {
            if (deger >= 3 && deger <= 7)
                return (deger - 3) / 4;
            else
                return 0;
        }


        // S�re i�in �yelik fonksiyonu - Y�ksek
        private double UyelikFonksiyonuSureYuksek(double deger)
        {
            if (deger >= 5)
                return 1;
            else if (deger > 3 && deger < 5)
                return (deger - 3) / 2;
            else
                return 0;
        }

        // S�re �yelik de�eri
        private double SureUyelikDegeri(Sure sure)
        {
            if (sure == Sure.Dusuk)
                return 2;
            else if (sure == Sure.Orta)
                return 5;
            else
                return 8;
        }

        // Mamdani ��kar�m mekanizmas� ile D�n�� H�z� hesaplama
        private double MamdaniDonusHizi(Hassaslik hassaslik, Miktar miktar, Kirlilik kirlilik)
        {
            // D�n�� h�z� kategorileri ve �yelik fonksiyonlar�
            double[,] donusHiziUyelikFonksiyonlari = new double[3, 3]
            {
                { UyelikFonksiyonuDonusDusuk((int) hassaslik), UyelikFonksiyonuDonusDusuk((int) miktar), UyelikFonksiyonuDonusDusuk((int) kirlilik) },
                { UyelikFonksiyonuDonusOrta((int) hassaslik), UyelikFonksiyonuDonusOrta((int) miktar), UyelikFonksiyonuDonusOrta((int) kirlilik) },
                { UyelikFonksiyonuDonusYuksek((int) hassaslik), UyelikFonksiyonuDonusYuksek((int) miktar), UyelikFonksiyonuDonusYuksek( (int) kirlilik) }
            };

            // D�n�� h�z� kategorileri ve �yelik de�erlerinin �arp�m�
            double[,] donusHiziCarpimlari = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    donusHiziCarpimlari[i, j] = donusHiziUyelikFonksiyonlari[i, j] * DonusHiziUyelikDegeri((DonusHizi)i);
                }
            }

            // A��rl�kl� ortalama hesaplama
            double toplamUyelikDegeri = 0;
            double toplamCarpim = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    toplamUyelikDegeri += donusHiziUyelikFonksiyonlari[i, j];
                    toplamCarpim += donusHiziCarpimlari[i, j];
                }
            }

            if (toplamUyelikDegeri == 0) // S�f�ra b�lme hatas�n� engellemek i�in kontrol
                return 0;

            return toplamCarpim / toplamUyelikDegeri;
        }

        // D�n�� H�z� i�in �yelik fonksiyonu - D���k
        private double UyelikFonksiyonuDonusDusuk(double deger)
        {
            if (deger <= 3)
                return 1;
            else if (deger > 3 && deger < 5)
                return (5 - deger) / 2;
            else
                return 0;
        }

        // D�n�� H�z� i�in �yelik fonksiyonu - Orta
        private double UyelikFonksiyonuDonusOrta(double deger)
        {
            if (deger >= 3 && deger <= 7)
                return Math.Min(deger - 3, 7 - deger) / 4;
            else
                return 0;
        }

        // D�n�� H�z� i�in �yelik fonksiyonu - Y�ksek
        private double UyelikFonksiyonuDonusYuksek(double deger)
        {
            if (deger >= 5)
                return 1;
            else if (deger > 3 && deger < 5)
                return (deger - 3) / 2;
            else
                return 0;
        }

        // D�n�� H�z� �yelik de�eri
        // D�n�� H�z� �yelik de�eri
        private double DonusHiziUyelikDegeri(DonusHizi donusHizi)
        {
            int hiz = (int)donusHizi;
            switch (hiz)
            {
                case 0:
                    return 2;
                case 1:
                    return 5;
                case 2:
                    return 8;
                default:
                    return 2; // Varsay�lan olarak d���k h�z� d�nd�r
            }
        }


    }
}
