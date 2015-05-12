using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.ParticipantApproves
{
    [TestClass]
    public class LastParticipantToApproveCompletesWithClosedOrder : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            var participantId = Guid.NewGuid();
            yield return Event.NewParticipantApprovalRequired(Id, participantId);
            yield return Event.NewParticipantApproved(Id, participantId);
            yield return Event.NewParticipantApprovalRequired(Id, ParticipantId);
            yield return Event.NewApprovalClosed(Id);
        }

        protected override Command When()
        {
            return Command.NewParticipantApproves(Id, ParticipantId);
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
        public void AParticipantApprovedEventIsRaised()
        {
            Assert.AreEqual(Events[0], Event.NewParticipantApproved(Id, ParticipantId));
        }

        [TestMethod]
        public void AnApprovalCompleteEventIsRaised()
        {
            Assert.AreEqual(Events[1], Event.NewApprovalComplete(Id));
        }
    }
}
