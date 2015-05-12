using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.ParticipantApproves
{
    [TestClass]
    public class AParticipantCanApprove : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, ParticipantId);
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
        }

        protected override Command When()
        {
            return Command.NewParticipantApproves(Id, ParticipantId);
        }

        [TestMethod]
        public void NoExceptionWereThrown()
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
