using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.Win32;
using System.Data;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Windows.Controls;
using System.IO;
using System.Xml.Serialization;
using System.Windows;

namespace OdczytWyciaguZPlikuCSV
{
    public class DodajEdytujParserViewModel : INotifyPropertyChanged
    {

        #region Konstruktory
        
            public DodajEdytujParserViewModel() { }

            public DodajEdytujParserViewModel(string sciezkaPliku, Action<string, List<ParserCSV>> _OdswiezMainWindowViewModel)
            {
                SciezkaPlikuCSV = sciezkaPliku;

                OdswiezMainWindowViewModel = _OdswiezMainWindowViewModel;
            }

            public DodajEdytujParserViewModel(string sciezkaPliku, Action<string, List<ParserCSV>> _OdswiezMainWindowViewModel, ParserCSV parserCSVDoEdycji)
            {
                SciezkaPlikuCSV = sciezkaPliku;
                NazwaParsera = parserCSVDoEdycji.NazwaParsera;
                NazwaEdytowanegoParsera = parserCSVDoEdycji.NazwaParsera;

                OdswiezMainWindowViewModel = _OdswiezMainWindowViewModel;

                NazwaKontrahentaNumerKolumny = parserCSVDoEdycji.NazwaKontrahentaNumerKolumny;
                RachunekKontrahentaNumerKolumny = parserCSVDoEdycji.RachunekKontrahentaNumerKolumny;
                KwotaNumerKolumny = parserCSVDoEdycji.KwotaNumerKolumny.ToString();
                DataOperacjiNumerKolumny = parserCSVDoEdycji.DataOperacjiNumerKolumny;
                DataKsiegowaniaNumerKolumny = parserCSVDoEdycji.DataKsiegowaniaNumerKolumny;
                TytulemNumerKolumny = parserCSVDoEdycji.TytulemNumerKolumny;
                NumerWierszaZPierwszaOperacja = parserCSVDoEdycji.NumerWierszaZPierwszaOperacja.ToString();
            }
                
        #endregion

            Action<string, List<ParserCSV>> OdswiezMainWindowViewModel;

        #region VM fields properties

            private string sciezkaPlikuCSV;
            public string SciezkaPlikuCSV
            {
                get
                {
                    return sciezkaPlikuCSV;
                }
                set
                {
                    sciezkaPlikuCSV = value;
                    NotifyPropertyChange("sciezkaPlikuCSV");
                }
            }

            private string nazwaParsera;
            public string NazwaParsera
            {
                get
                {
                    return nazwaParsera;
                }
                set
                {
                    nazwaParsera = value;
                    NotifyPropertyChange("NazwaParsera");
                }
            }

            private string numerWierszaZPierwszaOperacja;
            public string NumerWierszaZPierwszaOperacja
            {
                get
                {
                    return numerWierszaZPierwszaOperacja;
                }
                set
                {
                    numerWierszaZPierwszaOperacja = value;
                    NotifyPropertyChange("NumerWierszaZPierwszaOperacja");
                }
            }

            private string symbolSeparatoraWierszy;
            public string SymbolSeparatoraWierszy
            {
                get
                {
                    return symbolSeparatoraWierszy;
                }
                set
                {
                    symbolSeparatoraWierszy = value;
                    NotifyPropertyChange("SymbolDelimitera");
                }
            }

            private string nazwaKontrahentaNumerKolumny;
            public string NazwaKontrahentaNumerKolumny
            {
                get
                {
                    return nazwaKontrahentaNumerKolumny;
                }
                set
                {
                    nazwaKontrahentaNumerKolumny = value;
                    NotifyPropertyChange("NazwaKontrahentaNumerKolumny");
                }
            }

            private string rachunekKontrahentaNumerKolumny;
            public string RachunekKontrahentaNumerKolumny
            {
                get
                {
                    return rachunekKontrahentaNumerKolumny;
                }
                set
                {
                    rachunekKontrahentaNumerKolumny = value;
                    NotifyPropertyChange("RachunekKontrahentaNumerKolumny");
                }
            }

            private string kwotaNumerKolumny;
            public string KwotaNumerKolumny
            {
                get
                {
                    return kwotaNumerKolumny;
                }
                set
                {
                    kwotaNumerKolumny = value;
                    NotifyPropertyChange("KwotaNumerKolumny");
                }
            }

            private string dataOperacjiNumerKolumny;
            public string DataOperacjiNumerKolumny
            {
                get
                {
                    return dataOperacjiNumerKolumny;
                }
                set
                {
                    dataOperacjiNumerKolumny = value;
                    NotifyPropertyChange("DataOperacjiNumerKolumny");
                }
            }

            private string dataKsiegowaniaNumerKolumny;
            public string DataKsiegowaniaNumerKolumny
            {
                get
                {
                    return dataKsiegowaniaNumerKolumny;
                }
                set
                {
                    dataKsiegowaniaNumerKolumny = value;
                    NotifyPropertyChange("DataKsiegowaniaNumerKolumny");
                }
            }

            private string tytulemNumerKolumny;
            public string TytulemNumerKolumny
            {
                get
                {
                    return tytulemNumerKolumny;
                }
                set
                {
                    tytulemNumerKolumny = value;
                    NotifyPropertyChange("TytulemNumerKolumny");
                }
            }

            private DataTable dataTable;
            public DataTable DataTable
            {
                get
                {
                    return dataTable;
                }
                set
                {
                    dataTable = value;
                }
            }

        #endregion

        #region field properties

            public string SciezkaPlikuCSVCache { get; set; }
            public List<string> WczytanyPlikDoPodgladu { get; set; } 
            public string NazwaEdytowanegoParsera { get; set; }
            
        #endregion

        #region Commands

            private ICommand wybierzPlikCommand;
            public ICommand WybierzPlikCommand
            {
                get
                {
                    if (wybierzPlikCommand == null) wybierzPlikCommand = new RelayCommand(WybierzPlik_Execute);
                    return wybierzPlikCommand;
                }
            }
            public void WybierzPlik_Execute(object parameter)
            {
                OpenFileDialog dialogWyboruPlikuDoOdczytu = new OpenFileDialog();
                dialogWyboruPlikuDoOdczytu.ShowDialog();
                SciezkaPlikuCSV = dialogWyboruPlikuDoOdczytu.FileName;
            }

            private ICommand podgladPlikuCommand;
            public ICommand PodgladPlikuCommand
            {
                get
                {
                    if (podgladPlikuCommand == null) podgladPlikuCommand = new RelayCommand(PodgladPlikuButton_Execute);
                    return podgladPlikuCommand;
                }
            }
            public void PodgladPlikuButton_Execute(object parameter)
            {            
                List<List<string>> logicznaTablicaSparsowanychDanych = UtworzLogicznaTabliceNaPodstawiePliku();
                StworzDataTableZTablicyLogicznej(logicznaTablicaSparsowanychDanych);    
            }

            private ICommand zapiszParserIZamknijOknoCommand;
            public ICommand ZapiszParserIZamknijOknoCommand
            {
                get
                {
                    if (zapiszParserIZamknijOknoCommand == null) zapiszParserIZamknijOknoCommand = new RelayCommand(ZapiszParserIZamknijOkno_Execute);
                    return zapiszParserIZamknijOknoCommand;
                }
            }
            public void ZapiszParserIZamknijOkno_Execute(Object window)
            {
                ParserCSV nowyParser = StworzParserZAtrybutowWViewModel();
                List<ParserCSV> listaParserow = OdczytajListeZapisanychParserow();
                if (!string.IsNullOrEmpty(NazwaEdytowanegoParsera)) listaParserow.Remove(listaParserow.Find(f => f.NazwaParsera==NazwaEdytowanegoParsera));
                listaParserow.Add(nowyParser);
                listaParserow = listaParserow.OrderBy(o => o.NazwaParsera).ToList();
                ZapiszListeParserowDoXml(listaParserow);
                OdswiezMainWindowViewModel(SciezkaPlikuCSV, listaParserow);
                (window as Window).Close();
            }

            private ICommand zamknijOknoCommand;
            public ICommand ZamknijOknoCommand
            {
                get
                {
                    if (zamknijOknoCommand == null) zamknijOknoCommand = new RelayCommand(ZamknijOknoCommand_Execute);
                    return zamknijOknoCommand;
                }
            }
            public void ZamknijOknoCommand_Execute(Object window)
            {
                (window as Window).Close();
            }

        #endregion


        public interface IWczytywaczWierszy
        {
            List<string> PodzielWiersz(string wiersz);
        }
        public class WczytywaczWierszyZSeparatorem : IWczytywaczWierszy
        {
            public string separator;
            public WczytywaczWierszyZSeparatorem(string _separator)
            {
                separator = _separator;
            }
            public List<string> PodzielWiersz(string wiersz)
            {
                return wiersz.Split(separator.ToCharArray()).ToList();
            }
        }
        public class WczytywaczWierszyBezSeparatora : IWczytywaczWierszy
        {
            public List<string> PodzielWiersz(string wiersz)
            {
                return (new List<string> { wiersz });
            }
        }
        public IWczytywaczWierszy PodajWczytywacz(string separator)
        {
            if (string.IsNullOrWhiteSpace(separator)) return new WczytywaczWierszyBezSeparatora();
            else return new WczytywaczWierszyZSeparatorem(separator);
        }




        private List<List<string>> UtworzLogicznaTabliceNaPodstawiePliku()
        {
            if (SciezkaPlikuCSVCache != SciezkaPlikuCSV)
            {
                WczytanyPlikDoPodgladu = WczytajPlik();
                SciezkaPlikuCSVCache = SciezkaPlikuCSV;
            }
            List<List<string>> logicznaTablicaNaPodstawiePliku = new List<List<string>>();
            IWczytywaczWierszy wczytywaczWierszy = PodajWczytywacz(SymbolSeparatoraWierszy);
            foreach (string wiersz in WczytanyPlikDoPodgladu)
            {
                logicznaTablicaNaPodstawiePliku.Add(wczytywaczWierszy.PodzielWiersz(wiersz));
            }

            return logicznaTablicaNaPodstawiePliku;

        }
        private List<string> WczytajPlik()
        {
            using (StreamReader strumienOdczytuWybranegoPliku = new StreamReader(SciezkaPlikuCSV, Encoding.GetEncoding("Windows-1250")))
            {
                List<string> listaWierszyWczytanegoPliku = new List<string>();
                string wiersz = null;

                while (!strumienOdczytuWybranegoPliku.EndOfStream)
                {
                    wiersz = strumienOdczytuWybranegoPliku.ReadLine();
                    listaWierszyWczytanegoPliku.Add(wiersz);
                }

                return listaWierszyWczytanegoPliku;
            }
        }
        private void StworzDataTableZTablicyLogicznej(List<List<string>> logicznaTablica)
        {
            dataTable = new DataTable();
            int liczbaKolumn = logicznaTablica.Select(s => s.Count).Max();
            DodajKolumnyDoDataTable(liczbaKolumn);
            DodajWierszeDoDataTable(logicznaTablica);
            NotifyPropertyChange("DataTable");
         
        }
        private void DodajKolumnyDoDataTable(int liczbaKolumn)
        {
            for (int i = 0; i < liczbaKolumn; i++)
            {
                DataTable.Columns.Add();
            }
        }
        private void DodajWierszeDoDataTable(List<List<string>> logicznaTablica)
        {
            for (int i = 0; i < logicznaTablica.Count(); i++)
            {
                DataTable.Rows.Add(logicznaTablica[i].ToArray());
            }
        }

        private ParserCSV StworzParserZAtrybutowWViewModel()
        {
            ParserCSV parser = new ParserCSV();
            
            parser.NazwaParsera = NazwaParsera;
            parser.NumerWierszaZPierwszaOperacja = int.Parse(NumerWierszaZPierwszaOperacja);
            parser.SymbolSeparatoraWierszy = SymbolSeparatoraWierszy;
            parser.NazwaKontrahentaNumerKolumny = NazwaKontrahentaNumerKolumny;
            parser.RachunekKontrahentaNumerKolumny = RachunekKontrahentaNumerKolumny;
            parser.KwotaNumerKolumny = double.Parse(KwotaNumerKolumny);
            parser.DataOperacjiNumerKolumny = DataOperacjiNumerKolumny;
            parser.DataKsiegowaniaNumerKolumny = DataKsiegowaniaNumerKolumny;
            parser.TytulemNumerKolumny = TytulemNumerKolumny;            

            return parser;
        }
        private List<ParserCSV> OdczytajListeZapisanychParserow()
        {
            using (FileStream stream = new FileStream("parsery", FileMode.Open))
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<ParserCSV>));
                List<ParserCSV> listaOdczytanychParserow = xml.Deserialize(stream) as List<ParserCSV>;
                return listaOdczytanychParserow;
            }
        }
        private void ZapiszListeParserowDoXml(List<ParserCSV> listaParserowDoZapisania)
        {
            using (FileStream stream = new FileStream("parsery", FileMode.Open))
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<ParserCSV>));
                xml.Serialize(stream, listaParserowDoZapisania);
            }
        }        

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChange(string propName)
        {
            if (PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }
    }

    public class OperacjaBankowaZCSV
    {
        public string RachunekWlasciciela { get; set; }
        public string RachunekPodmiotu { get; set; }
        public double Kwota { get; set; }
        public string DataOperacji{get; set;}
        public string DataKsiegowania { get; set; }
        public string Tytulem {get; set ;}
    }

    public class WyciagBankowyZCSV
    {
        public string DataOd { get; set; }
        public string DataDo { get; set; }
        public List<OperacjaBankowaZCSV> ListaOperacjiBankowychZCSV {get ; set ; }
    }

    public class ParserCSV
    {
        public string NazwaParsera { get; set; }
        public int NumerWierszaZPierwszaOperacja { get; set; }
        public string SymbolSeparatoraWierszy { get; set; }
        public string NazwaKontrahentaNumerKolumny{ get; set;}
        public string RachunekKontrahentaNumerKolumny { get; set; }
        public double KwotaNumerKolumny { get; set; }
        public string DataOperacjiNumerKolumny { get; set; }
        public string DataKsiegowaniaNumerKolumny { get; set; }
        public string TytulemNumerKolumny { get; set; }        
    }
}
