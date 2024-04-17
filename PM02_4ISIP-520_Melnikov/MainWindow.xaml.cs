using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PM02_4ISIP_520_Melnikov
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static (int[][], int) MinElCostMeth(int[] supply, int[] demand, int[][] costs)
        {
            int[][] allocation = new int[supply.Length][];
            for (int i = 0; i < supply.Length; i++)
            {
                allocation[i] = new int[demand.Length];
            }

            int[] supplyCopy = supply.ToArray();
            int[] demandCopy = demand.ToArray();
            int totalCost = 0;

            while (true)
            {
                int minCost = int.MaxValue;
                int minRow = -1, minCol = -1;

                for (int row = 0; row < supply.Length; row++)
                {
                    for (int col = 0; col < demand.Length; col++)
                    {
                        if (supplyCopy[row] > 0 && demandCopy[col] > 0)
                        {
                            if (costs[row][col] < minCost)
                            {
                                minCost = costs[row][col];
                                minRow = row;
                                minCol = col;
                            }
                        }
                    }
                }

                if (minRow == -1 || minCol == -1)
                {
                    break;
                }

                int x = Math.Min(supplyCopy[minRow], demandCopy[minCol]);
                allocation[minRow][minCol] = x;
                supplyCopy[minRow] -= x;
                demandCopy[minCol] -= x;
                totalCost += x * minCost;
            }

            return (allocation, totalCost);
        }

        private void Display(int[][] allocation, int totalCost)
        {
            StringBuilder resultBuilder = new StringBuilder();

            for (int i = 0; i < allocation.Length; i++)
            {
                for (int j = 0; j < allocation[i].Length; j++)
                {
                    resultBuilder.Append(allocation[i][j]);
                    resultBuilder.Append("\t");
                }
                resultBuilder.AppendLine();
            }

            OutputBlock.Text = $"опорный план:\n {resultBuilder.ToString()} \n стоимость превозки:{totalCost}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] supplytxt = SupplyText.Text.Split(','); 
            string[] demandtxt = DemandText.Text.Split(',');
            string[] costs = CostsText.Text.Split('\n');

            int[] demand = Array.ConvertAll(demandtxt, int.Parse);
            int[] supply = Array.ConvertAll(supplytxt, int.Parse);
            int[][] costMatrix = new int[costs.Length][];
            for (int i = 0; i < costs.Length; i++)
            {
                costMatrix[i] = costs[i].Split(',').Select(int.Parse).ToArray();
            }

            var (allocation, totalCost) = MinElCostMeth(supply, demand, costMatrix);
            Display(allocation, totalCost);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SupplyText.Text = "";
            DemandText.Text = "";
            CostsText.Text = "";
            OutputBlock.Text = "";
        }
    }
}
