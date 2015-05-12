using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.CloseApproval
{
    [TestClass]
    public class AnApprovalCannotBeClosedTwice : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
            yield return Event.NewApprovalClosed(Id);
        }

        protected override Command When()
        {
            return Command.NewCloseApproval(Id);
        }

        [TestMethod]
        public void AnExceptionWasThrown()
        {
            Assert.IsInstanceOfType(this.Exception, typeof(ApprovalClosedException));
        }
    }
}
