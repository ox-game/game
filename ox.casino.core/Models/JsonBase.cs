using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX
{
    public class JsonBase<T>
    {
        public JsonBase(T data)
        {
            Data = data;
        }
        public int ReturnCode;
        public string Message;
        public T Data;
    }
}
