using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace Approval.Test.RequireApprovalFromParticipant
{
    [TestClass]
    public class ApprovalFromAParticipantCannotBeRequiredAfterClosed : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
            yield return Event.NewApprovalClosed(Id);
        }

        protected override Command When()
        {
            return Command.NewRequireApprovalFromParticipant(Id, ParticipantId);
        }

        [TestMethod]
        public void AnExceptionIsThrown()
        {
            Assert.IsInstanceOfType(this.Exception, typeof(ApprovalClosedException));
        }

        [TestMethod]
        public void NoEventIsRaised()
        {
            Assert.AreEqual(0, Events.Length);
        }
    }
}
