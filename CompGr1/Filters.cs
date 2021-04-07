using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace CompGr1
{
    abstract class Filters
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);
        public virtual Bitmap processImage (Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i=0; i<sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }
        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }

    //переопределенная calculateNewPixelColor, инверсия
    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R,
                                                255 - sourceColor.G,
                                                255 - sourceColor.B);
            return resultColor;
        }
    }

    class MatrixFilter : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            //цветовые компоненты результирующего цвета
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            for (int l = -radiusY; l <= radiusY; l++)
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
                );
        }
    }

    class BlurFilter : MatrixFilter
    {
        public BlurFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
        }
    }

    // Фильтр Гаусса
    class GaussianFilter : MatrixFilter
    {
        public void createGaussianKernel(int radius, float sigma)
        {
            // определяем размер ядра
            int size = 2 * radius + 1;
            // создаём ядро фильтра
            kernel = new float[size, size];
            // коэффициент нормировки ядра
            float norm = 0;
            // расчитывает ядро линейного фильтра
            for (int i = -radius; i <= radius; i++)
                for (int j = -radius; j <= radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            // нормируем ядро
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;
        }
        public GaussianFilter()
        {
            createGaussianKernel(3, 2);
        }
    }

    // Оттенки серого
    class GrayScaleFilter : Filters 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B);
            return Color.FromArgb(Clamp(Intensity, 0, 255), Clamp(Intensity, 0, 255), Clamp(Intensity, 0, 255));
        }
    }

    //Сепия
    class SepiaFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B);
            double k = 25;
            int R, G, B;
            R = Clamp((int)(Intensity + 2 * k), 0, 255);
            G = Clamp((int)(Intensity + 0.5 * k), 0, 255);
            B = Clamp((int)(Intensity - 1 * k), 0, 255);
            return Color.FromArgb(R, G, B); 
        }
    }

    //увеличение яркости
    class BrightnessFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int k = 10;
            Color resultColor = Color.FromArgb(Clamp(sourceColor.R + k, 0, 255), Clamp(sourceColor.G + k, 0, 255), Clamp(sourceColor.B + k, 0, 255));
            return resultColor;
        }
    }

    //Серый мир
    class GreyWorld : Filters
    {
        public double avgR;
        public double avgG;
        public double avgB;
        public double avgAll;
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color = sourceImage.GetPixel(x, y);
            return Color.FromArgb(
                Clamp((int)(color.R * avgAll / avgR), 0, 255),
                Clamp((int)(color.G * avgAll / avgG), 0, 255),
                Clamp((int)(color.B * avgAll / avgB), 0, 255)
            );
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            double sumR = 0;
            double sumG = 0;
            double sumB = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color curColor = sourceImage.GetPixel(i, j);
                    sumR += curColor.R;
                    sumG += curColor.G;
                    sumB += curColor.G;
                }
            }

            avgR = sumR / (sourceImage.Width * sourceImage.Height);
            avgG = sumG / (sourceImage.Width * sourceImage.Height);
            avgB = sumB / (sourceImage.Width * sourceImage.Height);

            avgAll = (avgR + avgB + avgG) / 3;

            return base.processImage(sourceImage, worker);
        }
    }

    //линейное растяжение
    class LinearRasting : Filters
    {
        private double intensityMax = -100000;
        private double intensityMin = 1000000;

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color curColor = sourceImage.GetPixel(x, y);
            double intensity = curColor.R * 0.36 + 0.53 * curColor.G + 0.11 * curColor.B;
            double newIntensity = (intensity - intensityMin) * (255.0 / (intensityMax - intensityMin));
            double scale = newIntensity / intensity;
            return Color.FromArgb(
                Clamp((int)(scale * curColor.R), 0, 255),
                Clamp((int)(scale * curColor.G), 0, 255),
                Clamp((int)(scale * curColor.B), 0, 255)
            );
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color curColor = sourceImage.GetPixel(i, j);
                    double intensity = curColor.R * 0.36 + 0.53 * curColor.G + 0.11 * curColor.B;
                    if (intensity > intensityMax)
                        intensityMax = intensity;
                    if (intensity < intensityMin)
                        intensityMin = intensity;
                }
            }
            return base.processImage(sourceImage, worker);
        }
    }

    //перенос
    class Shift : Filters
        {
            protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                int nX = (int)(x + 50);
                int nY = (int)(y);
                if (nX > sourceImage.Width - 1 || nY > sourceImage.Height - 1)
                    return Color.LightGray;
                return sourceImage.GetPixel(nX, nY);
            }
        };

    //поворот на 90
    class Turn : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {

            double x0 = Convert.ToInt32(sourceImage.Width / 2), y0 = Convert.ToInt32(sourceImage.Height / 2);
            int nX = Clamp((int)((x - x0) * Math.Cos(Math.PI / 2) - (y - y0) * Math.Sin(Math.PI / 2) + x0), 0, sourceImage.Width - 1);
            int nY = Clamp((int)((x - x0) * Math.Sin(Math.PI / 2) + (y - y0) * Math.Cos(Math.PI / 2) + y0), 0, sourceImage.Height - 1);
            if (sourceImage.Height > sourceImage.Width)
            {
                int area = (sourceImage.Height - sourceImage.Width) / 2;
                if (y < area || y > area + sourceImage.Width)
                    return Color.Black;
            }
            else
            {
                int area = (sourceImage.Width - sourceImage.Height) / 2;
                if (x < area || x > area + sourceImage.Height)
                    return Color.Black;
            }
            return sourceImage.GetPixel(nX, nY);
        }
    }

    //волны1
    class Waves1 : Filters
    {

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int nX = Clamp((int)(x + 20 * Math.Sin((2 * Math.PI * y) / 60)), 0, sourceImage.Width - 1);
            int nY = y;
            return sourceImage.GetPixel(nX, nY);
        }
    }

    //волны2
    class Waves2 : Filters
    {

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int nX = Clamp((int)(x + 20 * Math.Sin((2 * Math.PI * y) / 30)), 0, sourceImage.Width - 1);
            int nY = y;
            return sourceImage.GetPixel(nX, nY);
        }
    }

    //эффект стекла
    class Glass : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Random random = new Random();
            double num = random.NextDouble();
            int nX = Clamp((int)(x + (num - 0.5) * 10), 0, sourceImage.Width - 1);
            int nY = Clamp((int)(y + (num - 0.5) * 10), 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(nX, nY);
        }
    }

    //Резкость
    class SharpnessFilter : MatrixFilter
    {
        public SharpnessFilter()
        {
            kernel = new float[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
        }
    }

    //еще резкость
    class ClearFilter : MatrixFilter
    {
        public ClearFilter()
        {
            kernel = new float[3, 3] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
        }
    }

    //размытие
    class MotionBlur : MatrixFilter
    {
        public MotionBlur(int n)
        {
            kernel = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        kernel[i, j] = 1.0f / n;
                    else
                        kernel[i, j] = 0;
                }
            }

        }
    }

    //Фильтр Собеля
    class SobelsFilter : MatrixFilter
        {
            private float[,] kerX = null;
            private float[,] kerY = null;
            public SobelsFilter()
            {
                kerY = new float[3, 3]{ { -1f, -2f, -1f },
                                        { 0f, 0f, 0f },
                                        { 1f, 2f, 1f } };
                kerX = new float[3, 3]{ { -1f, 0f, 1f },
                                        { -2f, 0f, 2f },
                                        { -1f, 0f, 1f } };
            }
            protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                float R, G, B;
                kernel = kerX;
                Color gradX = base.calculateNewPixelColor(sourceImage, x, y);
                kernel = kerY;
                Color gradY = base.calculateNewPixelColor(sourceImage, x, y);
                R = (float)Math.Sqrt(gradX.R * gradX.R + gradY.R * gradY.R);
                G = (float)Math.Sqrt(gradX.G * gradX.G + gradY.G * gradY.G);
                B = (float)Math.Sqrt(gradX.B * gradX.B + gradY.B * gradY.B);
                return Color.FromArgb(Clamp((int)R, 0, 255), Clamp((int)G, 0, 255), Clamp((int)B, 0, 255));

            }
        }

    //медианный фильтр
    class MedianFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int rad = 2;
            if (x < rad || x >= sourceImage.Width - 1 - rad || y < rad || y >= sourceImage.Height - 1 - rad)
                return sourceImage.GetPixel(x, y);
            double[] arrR = new double[(rad * 2 + 1) * (rad * 2 + 1)];
            double[] arrB = new double[(rad * 2 + 1) * (rad * 2 + 1)];
            double[] arrG = new double[(rad * 2 + 1) * (rad * 2 + 1)];
            int count = 0;
            for (int i = -rad; i <= rad; i++)
                for (int j = -rad; j <= rad; j++, count++)
                {
                    int idX = Clamp(x + i, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + j, 0, sourceImage.Height - 1);
                    Color nearColor = sourceImage.GetPixel(idX, idY);
                    arrR[count] = nearColor.R;
                    arrG[count] = nearColor.G;
                    arrB[count] = nearColor.B;
                }
            Array.Sort(arrR);
            Array.Sort(arrG);
            Array.Sort(arrB);
            return Color.FromArgb(Clamp((int)arrR[4], 0, 255), Clamp((int)arrG[4], 0, 255), Clamp((int)arrG[4], 0, 255)); ;
        }
    }

    //мат морфология
    class Morphology : Filters
    {
        //структурный элемент
        protected float[,] matr;
        protected bool dilation;
        //матрица 3х3 
        protected Morphology() 
        {
            matr = new float[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        }
        public Morphology(float[,] matr)
        {
            this.matr = matr;
        }

        protected override Color calculateNewPixelColor(Bitmap Source, int W, int H)
        {
            int radX = matr.GetLength(0) / 2;
            int radY = matr.GetLength(1) / 2;
            int BITmaxR = 0; int BITmaxG = 0; int BITmaxB = 0; // черный
            int BITminR = 255; int BITminG = 255; int BITminB = 255;//белый
            for (int j = -radY; j <= radY; j++)
                for (int i = -radX; i <= radX; i++)
                {
                    //расширение
                    if (dilation) 
                    {
                        if ((matr[i + radX, j + radY] == 1) && (Source.GetPixel(W + i, H + j).R > BITmaxR))
                            BITmaxR = Source.GetPixel(W + i, H + j).R;
                        if ((matr[i + radX, j + radY] == 1) && (Source.GetPixel(W + i, H + j).G > BITmaxG))
                            BITmaxG = Source.GetPixel(W + i, H + j).G;
                        if ((matr[i + radX, j + radY] == 1) && (Source.GetPixel(W + i, H + j).B > BITmaxB))
                            BITmaxB = Source.GetPixel(W + i, H + j).B;

                    }
                    //сужение
                    else 
                    {
                        if ((matr[i + radX, j + radY] == 1) && (Source.GetPixel(W + i, H + j).R < BITminR))

                            BITminR = Source.GetPixel(W + i, H + j).R;
                        if ((matr[i + radX, j + radY] == 1) && (Source.GetPixel(W + i, H + j).G < BITminG))

                            BITminG = Source.GetPixel(W + i, H + j).G;
                        if ((matr[i + radX, j + radY] == 1) && (Source.GetPixel(W + i, H + j).B < BITminB))

                            BITminB = Source.GetPixel(W + i, H + j).B;
                    }
                }
            if (dilation)
                return Color.FromArgb(BITmaxR, BITmaxG, BITmaxB);
            else return Color.FromArgb(BITminR, BITminG, BITminB);

        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            int radX = matr.GetLength(0) / 2;
            int radY = matr.GetLength(1) / 2;
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = radX; i < sourceImage.Width - radX; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = radY; j < sourceImage.Height - radY; j++)  
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
            }
            return resultImage;
        }
    };

    //расширение
    class Dilation : Morphology 
    {
        public Dilation()
        {
            dilation = true;
        }
        public Dilation(float[,] matr)
        {
            this.matr = matr;
            dilation = true;
        }
    }

    //сужение
    class Erosion : Morphology 
    {
        public Erosion()
        {
            dilation = false;
        }
        public Erosion(float[,] matr)
        {
            this.matr = matr;
            dilation = false;
        }
    }

    //открытие
    class Opening : Morphology 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int W, int H)
        {
            return sourceImage.GetPixel(W, H);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters erosion = new Erosion(matr);
            Filters dilat = new Dilation(matr);
            //сужение
            Bitmap result = erosion.processImage(sourceImage, worker);
            //расширение
            result = dilat.processImage(result, worker); 
            return result;
        }
    }

    //закрытие
    class Closing : Morphology 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int W, int H)
        {
            return sourceImage.GetPixel(W, H);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters erosion = new Erosion(matr);
            Filters dilat = new Dilation(matr);
            //расширение
            Bitmap result = dilat.processImage(sourceImage, worker);
            //сужение
            result = erosion.processImage(result, worker);
            return result;
        }
    }

    class BlackHat : Morphology
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int W, int H)
        {
            return sourceImage.GetPixel(W, H);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters Op = new Opening();
            Bitmap result = Op.processImage(sourceImage, worker);

            //суть: открытие минус исходное изображение
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    int newR = Clamp(result.GetPixel(i, j).R - sourceImage.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(result.GetPixel(i, j).G - sourceImage.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(result.GetPixel(i, j).B - sourceImage.GetPixel(i, j).B, 0, 255);
                    resultImage.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return resultImage;
        }
    }
}
