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
    public class adminDashViewModel : INotifyPropertyChanged
    {
        private bool _isNotificationWithRedMarkVisible;
        private bool _isNotificationVisible;
        private int _userId;

        public bool IsNotificationWithRedMarkVisible
        {
            get { return _isNotificationWithRedMarkVisible; }
            set
            {
                _isNotificationWithRedMarkVisible = value;
                OnPropertyChanged();
            }
        }


        private static readonly List<string> ExcludedSubjects = new List<string>
        {
            "طلب تجديد المعطيات",
            "طلب تجديد كلمة المرور",
            "نسيت كلمة المرور"
        };



        private static readonly List<string> IncludedSubjects = new List<string>
        {
            "طلب تجديد المعطيات",
            "طلب تجديد كلمة المرور",
            "نسيت كلمة المرور"
        };


        //private static readonly List<string> IncludedSubjects = new List<string>
        //{
        //    "Validé",
        //    "Refusé",
        //    "Accepté"

        //};


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

        private string _role;


        public adminDashViewModel(int userId, string role)
        {
            _userId = userId;
            _role = role;
            LoadNotificationsCommand = new RelayCommand(_ => LoadNotifications());
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            // Load notifications from the database

            if (_role == "admin")
            {
                // Update the visibility of the buttons based on the notification state for admin
                List<Notification> notifications = DatabaseHelper.GetNotifications("admin", _userId);

                IsNotificationWithRedMarkVisible = notifications.Any(n => n.Etat == "Rédiger" && IncludedSubjects.Contains(n.Sujet));
                IsNotificationVisible = notifications.Any(n => n.Etat != "VU");
            }
            else
            {
                // Update the visibility of the buttons based on the notification state for user
                List<Notification> notifications = DatabaseHelper.GetNotifications("user", _userId);

                IsNotificationWithRedMarkVisible = notifications.Any(n => n.Etat == "Rédiger" && !ExcludedSubjects.Contains(n.Sujet));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
