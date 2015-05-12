using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.ParticipantApproves
{
    [TestClass]
    public class AParticipantCannotApproveTwice : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, ParticipantId);
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
            yield return Event.NewParticipantApproved(Id, ParticipantId);
        }

        protected override Command When()
        {
            return Command.NewParticipantApproves(Id, ParticipantId);
        }

        [TestMethod]
        public void AnExceptionIsThrown()
        {
            Assert.IsInstanceOfType(this.Exception, typeof(ParticipantAlreadyApprovedException));
        }

        [TestMethod]
        public void NoEventsAreRaised()
        {
            Assert.AreEqual(0, Events.Length);
        }
    }
}
