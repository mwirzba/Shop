using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Validation.ChainsOfResponsibility
{
    public abstract class ValidatorBase<T>
    {
        protected ValidatorBase<T> Successor { get; private set; }
        protected Dictionary<string, string> ErrorsResult { get; set; }

        public ValidatorBase()
        {
            ErrorsResult = new Dictionary<string, string>();
        }

        public abstract Dictionary<string, string> HandleValidation(T model);

        public void SetSuccesor(ValidatorBase<T> successor)
        {
            Successor = successor;
        }
    }
}
