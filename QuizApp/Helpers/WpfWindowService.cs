﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;

namespace QuizApp.Helpers
{
    public class WpfWindowService : IWindowService
    {
        #region Fields
        List<Window> windows;
        #endregion

        #region Events
        public event EventHandler<CanOpenWindowEventArgs> OnCanOpenWindowChanged;
        #endregion

        public WpfWindowService()
        {
            windows = new List<Window>();

            Messenger.Default.Register<NotificationMessage>(this, OnReceivedMessage);
        }

        #region EventHandlers
        void OnReceivedMessage(NotificationMessage obj)
        {
            var onWindowChanged = OnCanOpenWindowChanged;
            onWindowChanged(this, new CanOpenWindowEventArgs(obj.Notification, true));
        }

        #endregion

        #region Methods
        public bool CanOpenWindow(string name)
        {
            var w = windows.Find(window => window.GetType().Name == name);
            if (w == null)
                return true;

            return w.Visibility != Visibility.Visible || !w.IsActive;
        }

        public void OpenWindow(string name)
        {
            var w = windows.Find(window => window.GetType().Name == name);

            if (CanOpenWindow(name))
            {
                if(w != null)
                    windows.Remove(w);

                var types = from a in AppDomain.CurrentDomain.GetAssemblies()
                            from t in a.GetTypes()
                            where t.Name == name
                            select t;

                w = (Window)Activator.CreateInstance(types.First());
                windows.Add(w);
                w.Show();
            }           
        }

        public void OpenWindow(string name, ViewModelBase viewModel)
        {
            var w = windows.Find(window => window.GetType().Name == name);

            if (CanOpenWindow(name))
            {
                if (w != null)
                    windows.Remove(w);

                var types = from a in AppDomain.CurrentDomain.GetAssemblies()
                            from t in a.GetTypes()
                            where t.Name == name
                            select t;

                w = (Window)Activator.CreateInstance(types.First());
                windows.Add(w);
                w.DataContext = viewModel;
                w.Show();
            }
        }

        public bool AskConfirmation(string promt, string owner)
        {
            var w = windows.FirstOrDefault(window => window.GetType().Name == owner);

            MessageBoxResult mbr = MessageBoxResult.None;

            if (w != null)
                mbr = MessageBox.Show(w, promt, "Confirm", MessageBoxButton.YesNo);
            else
                mbr = MessageBox.Show(promt, "Confirm", MessageBoxButton.YesNo);

            switch (mbr)
            {
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes:
                    return true;
            }

            return false;
        }

        public void CloseWindow(string name)
        {
            if(CanCloseWindow(name))
            {
                var w = windows.Find(window => window.GetType().Name == name);
                w.Close();
            }
        }

        public bool CanCloseWindow(string name)
        {
            var w = windows.Find(window => window.GetType().Name == name);
            if (w == null)
                return false;

            return w.Visibility == Visibility.Visible || w.IsActive;
        }


        #endregion
    }
}
