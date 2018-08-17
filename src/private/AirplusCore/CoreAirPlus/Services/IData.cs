using System;
using System.Collections.Generic;


namespace CoreAirPlus.Services
{
    public interface IData<T>
    {
        IEnumerable<T> GetAll();
        T GetData<T>(int Id);
        void AddData<T>(T inparam);
    }
}