using Cosmos.Core.Memory;
using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Waterfall.System.Core.CLI;
using Waterfall.System.Managment;
using Waterfall.System.Graphics;
using Waterfall.System.Processes;
using System.Drawing;
using Cosmos.System.Graphics;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class benchmark : WSHCommand
    {
        /// <summary>
        /// Provides a help note for the benchmark command.
        /// </summary>
        public override string HelpNote { get; set; } = "Benchmarks your hardware... in cosmos terms!";

        public benchmark()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {

            };
        }

        List<ulong> scores = new List<ulong>();
        ulong testStart;
        ulong testEnd;
        ulong currentCPUUptime;
        ulong afterCPUUptime;
        Watershell myShell;

        /// <summary>
        /// Executes the benchmark tests and displays results.
        /// </summary>
        /// <param name="Params">Command parameters.</param>
        /// <param name="myShellPrivate">Reference to the Watershell instance.</param>
        public override void Execute(string[] Params, Watershell myShellPrivate)
        {
            myShell = myShellPrivate;
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Green));
            myShell.CWriteLine($"Benchmark started!");
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
            myShell.CWriteLine($"Calculating CPU speed...");
            currentCPUUptime = CPU.GetCPUUptime();
            Thread.Sleep(1000);
            afterCPUUptime = CPU.GetCPUUptime();
            RAT.MinFreePages = 32768;
            double cpuSpeedGHz = (double)(afterCPUUptime - currentCPUUptime) / 1000000000;
            myShell.CWriteLine($"CPU speed is: {cpuSpeedGHz:F3} GHz.");

            RealTime.CPUSecond = (afterCPUUptime - currentCPUUptime);

            LogPart("Graphics test", myShell);
            PixelTest();
            EmptyCanvasTest();
            FilledCanvasTest();
            GetCanvasTest();
            DrawImageCanvasTest();
            DrawImageAlphaCanvasTest();
            LogPart("CPU test", myShell);
            CPULoadTest();
            CPULoadTestRandomNumbers();
            LogPart("Heap test", myShell);
            HeapCollectTest();
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
            myShell.CWriteLine($"");
            double weightedAverage = CalculateAverage(scores);
            myShell.CWriteLine($"Benchmark completed! Your result: {weightedAverage:F1} points.");
        }

        /// <summary>
        /// Calculates the average of scores from all tests.
        /// </summary>
        /// <param name="scores">List of scores.</param>
        /// <returns>The average score as a double.</returns>
        public double CalculateAverage(List<ulong> scores)
        {
            double weightedSum = 0;

            for (int i = 0; i < scores.Count; i++)
            {
                weightedSum += scores[i];
            }

            return weightedSum / scores.Count;
        }

        /// <summary>
        /// Tests pixel rendering performance by drawing individual points.
        /// </summary>
        public void PixelTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            for (int j = 0; j < GUI.ScreenHeight; j++)
            {
                for (int i = 0; i < GUI.ScreenWidth; i++)
                {
                    GUI.MainCanvas.DrawPoint(Color.FromArgb(0, 0, 0), i, j);
                }
                score++;
                GUI.MainCanvas.Display();
                if (CPU.GetCPUUptime() - testStart > RealTime.CPUSecond)
                    break;
            }
            testEnd = CPU.GetCPUUptime();
            scores.Add(score);
            LogScore("Pixel test", score, myShell);
        }

        /// <summary>
        /// Tests performance by repeatedly displaying an empty canvas.
        /// </summary>
        public void EmptyCanvasTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            while (CPU.GetCPUUptime() - testStart < RealTime.CPUSecond)
            {
                GUI.MainCanvas.Display();
                score++;
            }
            testEnd = CPU.GetCPUUptime();
            score = (ulong)(score * 0.6);
            scores.Add(score);
            LogScore("Empty canvas test", score, myShell);
        }

        /// <summary>
        /// Tests performance by repeatedly drawing a filled rectangle.
        /// </summary>
        public void FilledCanvasTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            while (CPU.GetCPUUptime() - testStart < RealTime.CPUSecond)
            {
                GUI.MainCanvas.DrawFilledRectangle(Color.Black, 0, 0, (int)GUI.ScreenWidth, (int)GUI.ScreenHeight);
                GUI.MainCanvas.Display();
                score++;
            }
            testEnd = CPU.GetCPUUptime();
            score = (ulong)(score * 0.9);
            scores.Add(score);
            LogScore("Filled canvas test", score, myShell);
        }

        /// <summary>
        /// Tests performance by capturing the canvas as a bitmap image.
        /// </summary>
        public void GetCanvasTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            Bitmap cache;
            while (CPU.GetCPUUptime() - testStart < RealTime.CPUSecond)
            {
                cache = GUI.MainCanvas.GetImage(0, 0, (int)GUI.ScreenWidth, (int)GUI.ScreenHeight);
                score++;
            }
            testEnd = CPU.GetCPUUptime();
            score = (ulong)(score * 3);
            scores.Add(score);
            LogScore("Get canvas test", score, myShell);
        }

        /// <summary>
        /// Tests performance by drawing and displaying an image repeatedly.
        /// </summary>
        public void DrawImageCanvasTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            Bitmap cache;
            cache = GUI.MainCanvas.GetImage(0, 0, (int)GUI.ScreenWidth, (int)GUI.ScreenHeight);
            while (CPU.GetCPUUptime() - testStart < RealTime.CPUSecond)
            {
                GUI.MainCanvas.DrawImage(cache, 0, 0);
                GUI.MainCanvas.Display();
                score++;
            }
            testEnd = CPU.GetCPUUptime();
            score = (ulong)(score * 2);
            scores.Add(score);
            LogScore("Draw image test", score, myShell);
        }

        /// <summary>
        /// Tests performance by drawing an image with alpha blending.
        /// </summary>
        public void DrawImageAlphaCanvasTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            Bitmap cache;
            cache = GUI.MainCanvas.GetImage(0, 0, (int)GUI.ScreenWidth / 6, (int)GUI.ScreenHeight / 6);
            while (CPU.GetCPUUptime() - testStart < RealTime.CPUSecond)
            {
                GUI.MainCanvas.DrawImageAlpha(cache, 0, 0);
                GUI.MainCanvas.Display();
                score++;
            }
            testEnd = CPU.GetCPUUptime();
            score = (ulong)(score * 7);
            scores.Add(score);
            LogScore("Draw image ALPHA test", score, myShell);
        }

        /// <summary>
        /// Tests heap memory allocation and collection.
        /// </summary>
        public void HeapCollectTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            while (CPU.GetCPUUptime() - testStart < RealTime.CPUSecond)
            {
                int[] array = new int[5242880];
                Heap.Collect();
                score++;
            }
            testEnd = CPU.GetCPUUptime();
            score = (ulong)(score * 4);
            scores.Add(score);
            LogScore("Heap collect test (20mb)", score, myShell);
            Heap.Collect();
        }
        private bool IsPrime(ulong number)
        {
            if (number < 2)
                return false;
            for (ulong i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }

        public void CPULoadTestRandomNumbers()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            Random rand = new Random();

            while (CPU.GetCPUUptime() - testStart < (RealTime.CPUSecond / 2))
            {
                rand.Next();
                score++;
            }
            score /= 1000;
            testEnd = CPU.GetCPUUptime();
            score = (ulong)(score * 0.4);
            scores.Add(score);
            LogScore("CPU load test (Random number generation)", score, myShell);
        }

        public void CPULoadTest()
        {
            testStart = CPU.GetCPUUptime();
            ulong score = 0;
            ulong number = 5000;
            while (CPU.GetCPUUptime() - testStart < RealTime.CPUSecond)
            {
                IsPrime(number);
                number++;
                score++;
            }
            score = (ulong)(score * 0.4);
            testEnd = CPU.GetCPUUptime();
            scores.Add(score);
            LogScore("CPU load test (Prime calculation)", score, myShell);
        }

        public void LogPart(string title, Watershell myShell)
        {
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Green));
            myShell.CWriteLine(">>> " + title + " <<<");
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
            Heap.Collect();
        }
        public void LogTime(string title, Watershell myShell)
        {
            double elapsedSeconds = (double)(testEnd - testStart) / (double)RealTime.CPUSecond;
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Cyan));

            myShell.CWrite($"[{elapsedSeconds.ToString("F3")} s] ");

            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
            myShell.CWriteLine($"{title} finished!");
            Heap.Collect();
        }

        public void LogScore(string title, ulong points, Watershell myShell)
        {
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Cyan));

            myShell.CWrite($"[{points} pts] ");

            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
            myShell.CWriteLine($"{title} finished!");
            Heap.Collect();
        }

    }
}
