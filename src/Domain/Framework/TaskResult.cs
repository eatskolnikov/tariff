using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Framework
{
    public class TaskResult
    {
        public bool Success { get; set; }

        public string Message
        {
            get { return string.Join(", ", _messages); }
            set { _messages = new List<string>(value.Split(", ")); }
        }

        private List<string> _messages;
        public TaskResult()
        {
            _messages = new List<string>();
            Success = true;
        }

        public void AddErrorMessage(string message)
        {
            Success = false;
            _messages.Add(message);
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }
    }
    public class TaskResult<T> : TaskResult
    {
        public T Data { get; set; }
    }
}
