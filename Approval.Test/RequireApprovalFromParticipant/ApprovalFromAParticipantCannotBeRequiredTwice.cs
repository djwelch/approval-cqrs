using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace Approval.Test.RequireApprovalFromParticipant
{
    [TestClass]
    public class ApprovalFromAParticipantCannotBeRequiredTwice : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, ParticipantId);
        }

        protected override Command When()
        {
            return Command.NewRequireApprovalFromParticipant(Id, ParticipantId);
        }

        [TestMethod]
        public void AnExceptionIsThrown()
        {
            Assert.IsInstanceOfType(this.Exception, typeof(ParticipantAlreadyRequiresApprovalException));
        }

        [TestMethod]
        public void NoEventsAreRaised()
        {
            Assert.AreEqual(0, Events.Length);
        }
    }
}
