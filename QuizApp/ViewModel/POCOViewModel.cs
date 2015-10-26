using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.ViewModel
{
    public abstract class POCOViewModel<T> : ObservableObject
    {
        public T POCO { get; set; }

        public POCOViewModel(T poco)
        {
            POCO = poco;
        }
    }
}
