using System;

namespace Tofunaut.Bootstrap.Interfaces
{
    public interface IUpdateService
    {
        public event Action Updated;
        public event Action UpdatedLate;
        public event Action UpdatedFixed;
    }
}