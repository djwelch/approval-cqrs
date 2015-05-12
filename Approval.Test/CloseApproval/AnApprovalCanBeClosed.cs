using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.CloseApproval
{
    [TestClass]
    public class AnApprovalCanBeClosed : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
        }

        protected override Command When()
        {
            return Command.NewCloseApproval(Id);
        }

        [TestMethod]
        public void ExpectedVersionIs0()
        {
            Assert.AreEqual(0, this.ExpectedVersion);
        }

        [TestMethod]
        public void NoExceptionWasThrown()
        {
            Assert.IsNotInstanceOfType(this.Exception, typeof(Exception));
        }

        [TestMethod]
        public void OneEventIsRaised()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void AnApprovalClosedEventIsRaised()
        {
            Assert.AreEqual(Events[0], Event.NewApprovalClosed(Id));
        }
    }
}
