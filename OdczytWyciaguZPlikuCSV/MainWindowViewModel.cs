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
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Windows.Controls;
using System.IO;
using System.Xml.Serialization;
using System.Windows;



namespace OdczytWyciaguZPlikuCSV
{  
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            if (!File.Exists("parsery")) StworzPustyPlikParserow();
        }

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
                NotifyPropertyChange("SciezkaPlikuCSV");
            }
        }

        private ObservableCollection<ParserCSV> listaParserow;
        public ObservableCollection<ParserCSV> ListaParserow
        {
            get
            {
                if (listaParserow == null) listaParserow = new ObservableCollection<ParserCSV>(OdczytajListeZapisanychParserow());
                return listaParserow;
            }
            set
            {
                listaParserow = value;
                NotifyPropertyChange("ListaParserow");
            }
        }

        private ParserCSV wybranyParserCSV;
        public ParserCSV WybranyParserCSV
        {
            get
            {
                if (wybranyParserCSV == null && ListaParserow.Any()) wybranyParserCSV = ListaParserow[0];
                return wybranyParserCSV;
            }
            set
            {
                wybranyParserCSV = value;
                NotifyPropertyChange("WybranyParserCSV");
            }
        }

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

        private ICommand dodajParserCommand;
        public ICommand DodajParserCommand
        {
            get
            {
                if (dodajParserCommand == null) dodajParserCommand = new RelayCommand(DodajParser_Execute);
                return dodajParserCommand;
            }
        }
        public void DodajParser_Execute(object parameter)
        {
            DodajEdytujParserViewModel dodajEdytujParserViewModel = new DodajEdytujParserViewModel(SciezkaPlikuCSV, OdswiezMainWindowViewModel);
            DodajEdytujParser dodajEdytujParserView = new DodajEdytujParser(dodajEdytujParserViewModel);
            dodajEdytujParserView.Show();
        }

        private ICommand edytujParserCommand;
        public ICommand EdytujParserCommand
        {
            get
            {
                if (edytujParserCommand == null) edytujParserCommand = new RelayCommand(EdytujParser_Execute);
                return edytujParserCommand;
            }
        }
        public void EdytujParser_Execute(object parameter)
        {
            DodajEdytujParserViewModel dodajEdytujParserViewModel = new DodajEdytujParserViewModel(SciezkaPlikuCSV, OdswiezMainWindowViewModel, WybranyParserCSV );
            DodajEdytujParser dodajEdytujParserView = new DodajEdytujParser(dodajEdytujParserViewModel);
            dodajEdytujParserView.Show();
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

        private void StworzPustyPlikParserow()
        {
            using (FileStream stream = new FileStream("parsery", FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<ParserCSV>));
                List<ParserCSV> listaParserow = null;
                xml.Serialize(stream, listaParserow);
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChange(string propName)
        {
            if (PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }

        public void OdswiezMainWindowViewModel(string _sciezkaPlikuCSV, List<ParserCSV> _listaParserow)
        {
            SciezkaPlikuCSV = _sciezkaPlikuCSV;
            ListaParserow = new ObservableCollection<ParserCSV>(_listaParserow);
        }
    }
}
