/* Critical Value Calculator
 * By expeditiousRubyist
 * 
 * A simple calculator for t-distribution and f-distribution critical values
 * May be further expanded for chi-square and z-distribution at a later point
 */

using System;
using System.Windows;
using System.Web.UI.DataVisualization.Charting;

namespace CVCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Chart chart; // Used for statistical methods
        public MainWindow()
        {
            InitializeComponent();
            chart = new Chart();
            LowerTailButton.IsChecked = true;
        }

        // Compute T-Test Critical Value
        private void TTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var alpha = Convert.ToDouble(AlphaBox.Text);
                var df = Convert.ToInt32(DFBox.Text);
                var stats = chart.DataManipulator.Statistics;
                double tvalue = -1.0;

                // Lower tail test
                if ((bool)LowerTailButton.IsChecked)
                {
                    bool negative = true;
                    if (alpha > 0.5d)
                    {
                        negative = false;
                        alpha = 1.0d - alpha;
                    }
                    alpha *= 2;
                    tvalue = stats.InverseTDistribution(alpha, df);
                    if (negative) { tvalue = -tvalue; } 
                }

                // Upper tail test
                else if ((bool)UpperTailButton.IsChecked)
                {
                    bool negative = false;
                    if (alpha > 0.5d)
                    {
                        negative = true;
                        alpha = 1.0d - alpha;
                    }
                    alpha *= 2;
                    tvalue = stats.InverseTDistribution(alpha, df);
                    if (negative) { tvalue = -tvalue; }
                }

                // 2 tail test
                else if ((bool)TwoTailButton.IsChecked)
                { 
                    tvalue = stats.InverseTDistribution(alpha, df);
                }
                AnswerBox.Text = tvalue.ToString();
            }
            catch (FormatException) { AnswerBox.Text = "Malformatted input"; }
            catch (OverflowException) { AnswerBox.Text = "Numeric overflow"; }
            catch (ArgumentOutOfRangeException) { AnswerBox.Text = "α or df out of range"; }
        }

        // Compute F-Test Critical Value
        private void FTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var alpha = Convert.ToDouble(AlphaBox.Text);
                var df = Convert.ToInt32(DFBox.Text);
                var df2 = Convert.ToInt32(DF2Box.Text);
                var stats = chart.DataManipulator.Statistics;
                var fvalue = stats.InverseFDistribution(alpha, df, df2);
                AnswerBox.Text = fvalue.ToString();
            }
            catch (FormatException) { AnswerBox.Text = "Malformatted input"; }
            catch (OverflowException) { AnswerBox.Text = "Numeric overflow"; }
            catch (ArgumentOutOfRangeException) { AnswerBox.Text = "α or df out of range"; }
        }
    }
}
