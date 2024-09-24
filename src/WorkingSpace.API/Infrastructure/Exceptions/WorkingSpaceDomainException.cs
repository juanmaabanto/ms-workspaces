using System;

namespace Sofisoft.Accounts.WorkingSpace.API.Infrastructure.Exceptions
{
    public class WorkingSpaceDomainException : Exception
    {
        private string errorId;

        public string ErrorId => errorId;

        public WorkingSpaceDomainException()
        { }

        public WorkingSpaceDomainException(string message)
            : base(message)
        { }

        public WorkingSpaceDomainException(string message, string errorId)
            : base(message)
        { 
            this.errorId = errorId;
        }
    }
}