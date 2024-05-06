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
            // Hassaslýk için kaydýrma çubuðunu baþlat ve sýnýf düzeyindeki deðiþkene ata
            trackBarHassaslik = InitializeTrackBar("Hassaslýk", "lblHassaslik", lblHassaslik);

            // Miktar için kaydýrma çubuðunu baþlat ve sýnýf düzeyindeki deðiþkene ata
            trackBarMiktar = InitializeTrackBar("Miktar", "lblMiktar", lblMiktar);

            // Kirlilik için kaydýrma çubuðunu baþlat ve sýnýf düzeyindeki deðiþkene ata
            trackBarKirlilik = InitializeTrackBar("Kirlilik", "lblKirlilik", lblKirlilik);

            // Buton için olay atamasý
            Button btnCalculate = new Button();
            btnCalculate.Text = "Hesapla";
            btnCalculate.Size = new System.Drawing.Size(100, 30); // Buton boyutunu ayarla
            btnCalculate.Location = new System.Drawing.Point(this.ClientSize.Width / 2 - btnCalculate.Width / 2, 200); // Formun ortasýnda ve aþaðýda yerleþtir
            btnCalculate.Click += BtnCalculate_Click;
            this.Controls.Add(btnCalculate);

            Button btnRecalculate = new Button();
            btnRecalculate.Text = "Tekrar Hesapla";
            btnRecalculate.Size = new System.Drawing.Size(100, 30); // Buton boyutunu ayarla
            btnRecalculate.Location = new System.Drawing.Point(btnCalculate.Right + 10, btnCalculate.Top); // Hesapla butonunun saðýna yerleþtir
            btnRecalculate.Click += BtnRecalculate_Click;
            this.Controls.Add(btnRecalculate);
        }

        private TrackBar InitializeTrackBar(string labelText, string labelName, Label label)
        { 

           

            // Label ve TrackBar oluþtur
            TrackBar trackBar = new TrackBar();
            trackBar.Minimum = 0;
            trackBar.Maximum = 10;
            trackBar.Value = 5; // Baþlangýç deðeri
            trackBar.TickStyle = TickStyle.None; // Ýsteðe baðlý olarak iþaretçi stilleri
            trackBar.Width = 200; // Geniþlik ayarla
            trackBar.Location = new System.Drawing.Point(label.Left, label.Bottom + 5); // Label'in altýna yerleþtir

            // Deðer deðiþtiðinde tetiklenecek olayý ekle
            trackBar.ValueChanged += (sender, e) =>
            {
                // TrackBar'dan deðeri al ve etikete yaz
                label.Text = $"{labelText} Deðeri: {trackBar.Value}";
            };

            // Forma ekle
            this.Controls.Add(trackBar);

            return trackBar;
        }
        private void BtnRecalculate_Click(object sender, EventArgs e)
        {
            // Önce sonuç panelini temizle
            ClearResultPanel();

            // Yeniden hesaplama iþlevini çaðýr
            Recalculate();
        }

        private void ClearResultPanel()
        {
            // paneli temizle sonuçlarý
            resultPanel.Controls.Clear();
        }

        private void Recalculate()
        {
            // Giriþ deðerlerini trackbarlardan al
            Hassaslik hassaslik = (Hassaslik)trackBarHassaslik.Value;
            Miktar miktar = (Miktar)trackBarMiktar.Value;
            Kirlilik kirlilik = (Kirlilik)trackBarKirlilik.Value;

            // Mamdani çýkarým mekanizmasý ile sonuçlarý hesapla
            double deterjanMiktari = MamdaniDeterjanMiktari(hassaslik, miktar, kirlilik);
            double sure = MamdaniSure(hassaslik, miktar, kirlilik);
            double donusHizi = MamdaniDonusHizi(hassaslik, miktar, kirlilik);

            // Aðýrlýk merkezi (centroid) hesaplamasý
            double centroidDeterjanMiktari = CalculateCentroid(deterjanMiktari);
            double centroidSure = CalculateCentroid(sure);
            double centroidDonusHizi = CalculateCentroid(donusHizi);

            DrawGraph("Deterjan Miktarý", centroidDeterjanMiktari, Color.Blue, 0);
            DrawGraph("Süre", centroidSure, Color.Red, 150);
            DrawGraph("Dönüþ Hýzý", centroidDonusHizi, Color.Green, 300);




            // Sonuçlarý göster
            ShowResult($"Deterjan Miktarý: {deterjanMiktari}\nSüre: {sure}\nDönüþ Hýzý: {donusHizi}");


            

        }
        // Aðýrlýk merkezi (centroid) hesaplamasý
        private double CalculateCentroid(double value)
        {
            double totalWeight = 0;
            double weightedSum = 0;

            // Her bir deðerin aðýrlýðý ve çarpýmý hesapla
            for (int i = 0; i <= 10; i++)
            {
                double weight = UyelikFonksiyonuKucuk(i) + UyelikFonksiyonuOrta(i) + UyelikFonksiyonuBuyuk(i);
                totalWeight += weight;
                weightedSum += weight * i;
            }

            // Toplam aðýrlýklara bölerek aðýrlýk merkezini bul
            if (totalWeight == 0) 
                return 0;

            return weightedSum / totalWeight;
        }
        // Grafik çizme fonksiyonu
        private void DrawGraph(string label, double value, Color color, int yOffset)
        {
            int barWidth = 50; 
            int barHeight = (int)value * 10;
            int startX = 50; 
            int startY = 100 + yOffset; 
            using (Graphics g = resultPanel.CreateGraphics())
            {
                // Çubuðun çizimindeki yükseklik ve geniþlik deðerlerindeki deðiþiklikleri uygula
                g.FillRectangle(new SolidBrush(color), startX, startY - barHeight, barWidth, barHeight);

                // Etiketi çubuða baðlamak için x ve y koordinatlarýný ayarla
                int labelX = startX + (barWidth - (int)g.MeasureString(label, Font).Width) / 2;
                int labelY = startY + 10;

                // Etiketi çiz
                g.DrawString(label, Font, Brushes.Black, labelX, labelY);
            }
        }




        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            // Giriþ deðerlerini trackbarlardan al
            Hassaslik hassaslik = (Hassaslik)trackBarHassaslik.Value;
            Miktar miktar = (Miktar)trackBarMiktar.Value;
            Kirlilik kirlilik = (Kirlilik)trackBarKirlilik.Value;

            // Mamdani çýkarým mekanizmasý ile sonuçlarý hesapla
            double deterjanMiktari = MamdaniDeterjanMiktari(hassaslik, miktar, kirlilik);
            double sure = MamdaniSure(hassaslik, miktar, kirlilik);
            double donusHizi = MamdaniDonusHizi(hassaslik, miktar, kirlilik);

            // Aðýrlýk merkezi (centroid) hesaplamasý
            //double centroidDeterjanMiktari = CalculateCentroid(deterjanMiktari);
            //double centroidSure = CalculateCentroid(sure);
            //double centroidDonusHizi = CalculateCentroid(donusHizi);

            // Sonuçlarý göster
            ShowResult($"Deterjan Miktarý: {deterjanMiktari}\nSüre: {sure}\nDönüþ Hýzý: {donusHizi}");

            //DrawGraph("Deterjan Miktarý", centroidDeterjanMiktari, Color.Blue, 0);
            //DrawGraph("Süre", centroidSure, Color.Red, 150);
            //DrawGraph("Dönüþ Hýzý", centroidDonusHizi, Color.Green, 300);



        }


        private double MamdaniDeterjanMiktari(Hassaslik hassaslik, Miktar miktar, Kirlilik kirlilik)
        {
            // Deterjan miktarý kategorileri ve üyelik fonksiyonlarý
            double[,] deterjanMiktarUyelikFonksiyonlari = new double[3, 3]
            {
                { UyelikFonksiyonuKucuk((int)hassaslik), UyelikFonksiyonuKucuk((int)miktar), UyelikFonksiyonuKucuk((int)kirlilik) },
                { UyelikFonksiyonuOrta((int)hassaslik), UyelikFonksiyonuOrta((int)miktar), UyelikFonksiyonuOrta((int)kirlilik) },
                { UyelikFonksiyonuBuyuk((int)hassaslik), UyelikFonksiyonuBuyuk((int)miktar), UyelikFonksiyonuBuyuk((int)kirlilik) }
            };

            // Deterjan miktarý kategorileri ve üyelik deðerlerinin çarpýmý
            double[,] deterjanMiktarCarpimlari = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    deterjanMiktarCarpimlari[i, j] = deterjanMiktarUyelikFonksiyonlari[i, j] * MiktarUyelikDegeri((Miktar)i);
                }
            }

            // Aðýrlýklý ortalama hesaplama
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

            if (toplamUyelikDegeri == 0) // Sýfýra bölme hatasýný engellemek için kontrol
                return 0;

            return toplamCarpim / toplamUyelikDegeri;
        }

        // Deterjan Miktarý için üyelik fonksiyonu - Küçük
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
        // Miktar üyelik deðeri
        private double MiktarUyelikDegeri(Miktar miktar)
        {
            if (miktar == Miktar.Az)
                return 2;
            else if (miktar == Miktar.Orta)
                return 5;
            else
                return 8;
        }

        // Mamdani çýkarým mekanizmasý ile Süre hesaplama
        private double MamdaniSure(Hassaslik hassaslik, Miktar miktar, Kirlilik kirlilik)
        {
            // Süre kategorileri ve üyelik fonksiyonlarý
            double[,] sureUyelikFonksiyonlari = new double[3, 3]
            {
                { UyelikFonksiyonuSureDusuk((int)hassaslik), UyelikFonksiyonuSureDusuk((int)miktar), UyelikFonksiyonuSureDusuk((int)kirlilik) },
                { UyelikFonksiyonuSureOrta((int) hassaslik), UyelikFonksiyonuSureOrta((int) miktar), UyelikFonksiyonuSureOrta((int) kirlilik) },
                { UyelikFonksiyonuSureYuksek((int) hassaslik), UyelikFonksiyonuSureYuksek((int) miktar), UyelikFonksiyonuSureYuksek((int) kirlilik) }
            };

            // Süre kategorileri ve üyelik deðerlerinin çarpýmý
            double[,] sureCarpimlari = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sureCarpimlari[i, j] = sureUyelikFonksiyonlari[i, j] * SureUyelikDegeri((Sure)i);
                }
            }

            // Aðýrlýklý ortalama hesaplama
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

            if (toplamUyelikDegeri == 0) // Sýfýra bölme hatasýný engellemek için kontrol
                return 0;

            return toplamCarpim / toplamUyelikDegeri;
        }

        // Süre için üyelik fonksiyonu - Düþük
        private double UyelikFonksiyonuSureDusuk(double deger)
        {
            if (deger <= 3)
                return 1;
            else if (deger > 3 && deger < 5)
                return (5 - deger) / 2;
            else
                return 0;
        }

        // Süre için üyelik fonksiyonu - Orta
        private double UyelikFonksiyonuSureOrta(double deger)
        {
            if (deger >= 3 && deger <= 7)
                return (deger - 3) / 4;
            else
                return 0;
        }


        // Süre için üyelik fonksiyonu - Yüksek
        private double UyelikFonksiyonuSureYuksek(double deger)
        {
            if (deger >= 5)
                return 1;
            else if (deger > 3 && deger < 5)
                return (deger - 3) / 2;
            else
                return 0;
        }

        // Süre üyelik deðeri
        private double SureUyelikDegeri(Sure sure)
        {
            if (sure == Sure.Dusuk)
                return 2;
            else if (sure == Sure.Orta)
                return 5;
            else
                return 8;
        }

        // Mamdani çýkarým mekanizmasý ile Dönüþ Hýzý hesaplama
        private double MamdaniDonusHizi(Hassaslik hassaslik, Miktar miktar, Kirlilik kirlilik)
        {
            // Dönüþ hýzý kategorileri ve üyelik fonksiyonlarý
            double[,] donusHiziUyelikFonksiyonlari = new double[3, 3]
            {
                { UyelikFonksiyonuDonusDusuk((int) hassaslik), UyelikFonksiyonuDonusDusuk((int) miktar), UyelikFonksiyonuDonusDusuk((int) kirlilik) },
                { UyelikFonksiyonuDonusOrta((int) hassaslik), UyelikFonksiyonuDonusOrta((int) miktar), UyelikFonksiyonuDonusOrta((int) kirlilik) },
                { UyelikFonksiyonuDonusYuksek((int) hassaslik), UyelikFonksiyonuDonusYuksek((int) miktar), UyelikFonksiyonuDonusYuksek( (int) kirlilik) }
            };

            // Dönüþ hýzý kategorileri ve üyelik deðerlerinin çarpýmý
            double[,] donusHiziCarpimlari = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    donusHiziCarpimlari[i, j] = donusHiziUyelikFonksiyonlari[i, j] * DonusHiziUyelikDegeri((DonusHizi)i);
                }
            }

            // Aðýrlýklý ortalama hesaplama
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

            if (toplamUyelikDegeri == 0) // Sýfýra bölme hatasýný engellemek için kontrol
                return 0;

            return toplamCarpim / toplamUyelikDegeri;
        }

        // Dönüþ Hýzý için üyelik fonksiyonu - Düþük
        private double UyelikFonksiyonuDonusDusuk(double deger)
        {
            if (deger <= 3)
                return 1;
            else if (deger > 3 && deger < 5)
                return (5 - deger) / 2;
            else
                return 0;
        }

        // Dönüþ Hýzý için üyelik fonksiyonu - Orta
        private double UyelikFonksiyonuDonusOrta(double deger)
        {
            if (deger >= 3 && deger <= 7)
                return Math.Min(deger - 3, 7 - deger) / 4;
            else
                return 0;
        }

        // Dönüþ Hýzý için üyelik fonksiyonu - Yüksek
        private double UyelikFonksiyonuDonusYuksek(double deger)
        {
            if (deger >= 5)
                return 1;
            else if (deger > 3 && deger < 5)
                return (deger - 3) / 2;
            else
                return 0;
        }

        // Dönüþ Hýzý üyelik deðeri
        // Dönüþ Hýzý üyelik deðeri
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
                    return 2; // Varsayýlan olarak düþük hýzý döndür
            }
        }


    }
}
