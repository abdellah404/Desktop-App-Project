using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class ClickCountModel : INotifyPropertyChanged
{
    private int _websiteAppCount;
    private int _fm6carteAppCount;
    private int _projectAppCount;
    private int _mailAppCount;
    private int _financeAppCount;
    private int _boAppCount;
    private int _archiveAppCount;
    private int _servicesAppCount;
    private int _medigesAppCount;
    // Add properties for other applications...

    public int WebsiteAppCount
    {
        get => _websiteAppCount;
        set
        {
            _websiteAppCount = value;
            OnPropertyChanged();
        }
    }

    public int Fm6carteAppCount
    {
        get => _fm6carteAppCount;
        set
        {
            _fm6carteAppCount = value;
            OnPropertyChanged();
        }
    }

    public int ProjectAppCount
    {
        get => _projectAppCount;
        set
        {
            _projectAppCount = value;
            OnPropertyChanged();
        }
    }



    public int MailAppCount
    {
        get => _mailAppCount;
        set
        {
            _mailAppCount = value;
            OnPropertyChanged();
        }
    }




    public int FinanceAppCount
    {
        get => _financeAppCount;
        set
        {
            _financeAppCount = value;
            OnPropertyChanged();
        }
    }


    public int BoAppCount
    {
        get => _boAppCount;
        set
        {
            _boAppCount = value;
            OnPropertyChanged();
        }
    }

    public int ArchiveAppCount
    {
        get => _archiveAppCount;
        set
        {
            _archiveAppCount = value;
            OnPropertyChanged();
        }
    }


    public int ServicesAppCount
    {
        get => _servicesAppCount;
        set
        {
            _servicesAppCount = value;
            OnPropertyChanged();
        }
    }


    public int MedigesAppCount
    {
        get => _medigesAppCount;
        set
        {
            _medigesAppCount = value;
            OnPropertyChanged();
        }
    }

    // Add properties for other applications...

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

