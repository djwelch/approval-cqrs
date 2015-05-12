using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.CloseApproval
{
    [TestClass]
    public class ClosingAnApprovedApprovalCompletes : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            var participantId = Guid.NewGuid();
            yield return Event.NewParticipantApprovalRequired(Id, participantId);
            yield return Event.NewParticipantApproved(Id, participantId);
        }

        protected override Command When()
        {
            return Command.NewCloseApproval(Id);
        }

        [TestMethod]
        public void NoExceptionWasThrown()
        {
            Assert.IsNotInstanceOfType(this.Exception, typeof(Exception));
        }

        [TestMethod]
        public void TwoEventsAreRaised()
        {
            Assert.AreEqual(2, Events.Length);
        }

        [TestMethod]
        public void AnApprovalClosedEventIsRaised()
        {
            Assert.AreEqual(Events[0], Event.NewApprovalClosed(Id));
        }

        [TestMethod]
        public void AnApprovalCompleteEventIsRaised()
        {
            Assert.AreEqual(Events[1], Event.NewApprovalComplete(Id));
        }
    }
}
