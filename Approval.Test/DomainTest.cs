using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Approval.Test
{
    public abstract class DomainTest
    {
        private Event[] events = new Event[] { };
        protected Event[] Events { get { return events; } }
        protected int? ExpectedVersion { get; private set; }
        protected System.Exception Exception { get; set; }

        protected virtual IEnumerable<Event> Given()
        {
            yield break;
        }

        protected abstract Command When();
        
        [TestInitialize]
        public void Init()
        {
            try
            {
                var tupe = Domain.Handle(Given(), When());
                events = tupe.Item1.ToArray();
                ExpectedVersion = tupe.Item2 != null ? (int?)tupe.Item2.Value : null;
            }
            catch (System.Exception e)
            {
                this.Exception = e;
            }
        }
    }
}