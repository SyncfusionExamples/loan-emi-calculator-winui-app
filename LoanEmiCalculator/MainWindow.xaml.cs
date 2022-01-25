using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoanEmiCalculator
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private double principal;

        public double Principal
        {
            get { return principal; }
            set { principal = value; }
        }

        private double rateOfInterest;

        public double RateOfInterest
        {
            get { return rateOfInterest; }
            set { rateOfInterest = value; }
        }

        private double noOfYears;

        public double NoOfYears
        {
            get { return noOfYears; }
            set { noOfYears = value; }
        }

        public ObservableCollection<Model> Data = new ObservableCollection<Model>() { new Model() { ChartLable = "Principal", LableValue = 320000 }, new Model() { ChartLable = "Interest", LableValue = 100000 } };
        public MainWindow()
        {
            this.InitializeComponent();
            doughnutSeries.ItemsSource = Data;
        }

        private void slider_ValueChanged(object sender, Syncfusion.UI.Xaml.Sliders.SliderValueChangedEventArgs e)
        {
            principal = loanAmountSlider.Value;
            rateOfInterest = rateOfInterestSlider.Value;
            noOfYears = numberOfYearsSlider.Value;

            calculate();
        }

        private void calculate()
        {
            double emi = (principal * (rateOfInterest / (12 * 100)) * Math.Pow((1 + (rateOfInterest / (12 * 100))), (noOfYears * 12))) / (Math.Pow((1 + (rateOfInterest / (12 * 100))), (noOfYears * 12)) - 1);
            double totalPayment = emi * noOfYears * 12;
            double totalInterest = totalPayment - principal;

            StringFormatConverter stringFormatConverter = new StringFormatConverter();
            emiTextBlock.Text = stringFormatConverter.Convert(emi, emi.GetType(), null, "").ToString();
            totalInterestTextBlock.Text = stringFormatConverter.Convert(totalInterest, totalInterest.GetType(), null, "").ToString();
            totalPaymentTextBlock.Text= stringFormatConverter.Convert(totalPayment, totalPayment.GetType(), null, "").ToString();
            principalTextBlock.Text= stringFormatConverter.Convert(principal, principal.GetType(), null, "").ToString();

            Data[0].LableValue = principal;
            Data[1].LableValue = totalInterest;
        }
    }

    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format("{0:C0}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Model: INotifyPropertyChanged
    {
        private string chartLable;

        public string ChartLable
        {
            get { return chartLable; }
            set { chartLable = value;
                OnPropertyChanged("ChartLable");
            }
        }

        private double lableValue;

        public double LableValue
        {
            get { return lableValue; }
            set { lableValue = value;
                OnPropertyChanged("LableValue");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
