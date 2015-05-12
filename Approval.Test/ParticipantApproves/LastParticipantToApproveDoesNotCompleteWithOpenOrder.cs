using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.ParticipantApproves
{
    [TestClass]
    public class LastParticipantToApproveDoesNotCompleteWithOpenOrder : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            var participantId = Guid.NewGuid();
            yield return Event.NewParticipantApprovalRequired(Id, participantId);
            yield return Event.NewParticipantApproved(Id, participantId);
            yield return Event.NewParticipantApprovalRequired(Id, ParticipantId);
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
        public void OneEventIsRaised()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void AParticipantApprovedEventIsRaised()
        {
            Assert.AreEqual(Events[0], Event.NewParticipantApproved(Id, ParticipantId));
        }
    }
}
