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
using System.Windows.Shapes;

namespace OdczytWyciaguZPlikuCSV
{
    /// <summary>
    /// Interaction logic for DodajEdytujParser.xaml
    /// </summary>
    public partial class DodajEdytujParser : Window
    {
        public DodajEdytujParser()
        {
            InitializeComponent();
            this.DataContext = new DodajEdytujParserViewModel();
        }

        public DodajEdytujParser(DodajEdytujParserViewModel dodajEdytujParserViewModel)
        {
            InitializeComponent();
            this.DataContext = dodajEdytujParserViewModel;
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
