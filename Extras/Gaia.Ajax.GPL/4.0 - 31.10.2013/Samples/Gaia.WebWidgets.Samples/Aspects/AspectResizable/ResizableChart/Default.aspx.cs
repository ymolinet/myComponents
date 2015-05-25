namespace Gaia.WebWidgets.Samples.Aspects.AspectResizable.ResizableChart
{
    using System;
    using System.Web.UI;
    using Gaia.WebWidgets;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AnnualInterestRateBox.Aspects.Add(
                new AspectResizable(
                    InterestResized,
                    AspectResizable.ResizeModes.RightBorder,
                    1,
                    0,
                    150,
                    0));

            LoanTermBox.Aspects.Add(
                new AspectResizable(
                    TermResized,
                    AspectResizable.ResizeModes.RightBorder,
                    1,
                    0,
                    150,
                    0));

            UpdateComputations();
        }

        void InterestResized(object sender, EventArgs e)
        {
            AnnualInterestRate.Text = string.Concat(Convert.ToString((Convert.ToInt32(AnnualInterestRateBox.Width.Value) / 5) + 1), "%");
            UpdateComputations();
        }

        void TermResized(object sender, EventArgs e)
        {
            LoanTerm.Text = Convert.ToString(((Convert.ToInt32(LoanTermBox.Width.Value) / 5) + 1) * 12);
            UpdateComputations();
        }

        protected void amount_Changed(object sender, EventArgs e)
        {
            UpdateComputations();
        }

        protected void UpdateComputations()
        {
            if (string.IsNullOrEmpty(LoanAmount.Text))
                return;

            double loanAmount = double.Parse(LoanAmount.Text);

            if (loanAmount == 0)
                return;

            double annualInterestRate = double.Parse(AnnualInterestRate.Text.Remove(AnnualInterestRate.Text.Length - 1)) / 100;
            double monthlyInterestRate = annualInterestRate / 12;
            int numberOfPayments = int.Parse(LoanTerm.Text);
            double monthlyPayment = loanAmount * monthlyInterestRate / (1 - Math.Pow(1 + monthlyInterestRate, -numberOfPayments));

            MonthlyPayment.Text = String.Format("{0}", Math.Round(monthlyPayment));
            TotalPayment.Text = String.Format("{0}", Math.Round(monthlyPayment * numberOfPayments));

            // compute yearly balance
            int yearCount = numberOfPayments / 12;
            int actualYearCount = 0;

            double balance = loanAmount;
            double[] yearlyBalance = new double[yearCount];

            for (int paymentIndex = 0; paymentIndex < numberOfPayments; ++paymentIndex)
            {
                balance = Math.Round(balance * (1 + monthlyInterestRate) - monthlyPayment);

                if (balance < 0)
                    break;

                if ((paymentIndex + 1) % 12 == 0)
                {
                    yearlyBalance[actualYearCount] = balance;
                    ++actualYearCount;
                }
            }

            // create chart
            ChartPanel.Controls.Clear();

            if (actualYearCount > 1)
            {
                const int width = 12;
                const int height = 200;

                for (int yearIndex = actualYearCount - 1; yearIndex >= 0; --yearIndex)
                {
                    var barContainer = new Panel();
                    barContainer.Height = height;
                    barContainer.Style["border"] = "solid 1px Black";
                    barContainer.Style["background-color"] = "#5e5";
                    barContainer.Width = width;
                    barContainer.Style["position"] = "relative";
                    barContainer.Style["margin"] = "2px";
                    barContainer.Style["float"] = "left";

                    var bar = new Panel();
                    bar.Style["background-color"] = "#fff";
                    bar.Width = width;
                    bar.Style["position"] = "absolute";
                    bar.Style["top"] = "0";
                    bar.Width = width;
                    int barHeight = (int)Math.Round(yearlyBalance[yearIndex] / yearlyBalance[0] * height);
                    bar.Height = barHeight;

                    var lit = new LiteralControl();
                    lit.Text = "&nbsp;";
                    bar.Controls.Add(lit);

                    barContainer.Controls.Add(bar);
                    ChartPanel.Controls.Add(barContainer);
                }
            }
        }
    }
}