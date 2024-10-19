using FM6_H.Helpers;
using FM6_H.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FM6_H.ViewModels
{
    public class dashViewModel : INotifyPropertyChanged
    {
        private bool _isNotificationWithRedMarkVisible;
        private bool _isNotificationVisible;
        private int _userId;

        private static readonly List<string> ExcludedSubjects = new List<string>
        {
            "طلب تجديد المعطيات",
            "طلب تجديد كلمة المرور",
            "نسيت كلمة المرور"
        };

        public bool IsNotificationWithRedMarkVisible
        {
            get { return _isNotificationWithRedMarkVisible; }
            set
            {
                _isNotificationWithRedMarkVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsNotificationVisible
        {
            get { return _isNotificationVisible; }
            set
            {
                _isNotificationVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadNotificationsCommand { get; set; }

        public dashViewModel(int userId)
        {
            _userId = userId;
            LoadNotificationsCommand = new RelayCommand(_ => LoadNotifications());
            LoadNotifications();
        }

        private void LoadNotifications()
        {

            // Update the visibility of the buttons based on the notification state for admin
            List<Notification> notifications = DatabaseHelper.GetNotifications("admin", _userId);

            IsNotificationVisible = notifications.Any(n => n.Etat != "VU");

            // Update the visibility of the buttons based on the notification state for user

            IsNotificationWithRedMarkVisible = notifications.Any(n => n.Etat == "Rédiger" && !ExcludedSubjects.Contains(n.Sujet));

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
