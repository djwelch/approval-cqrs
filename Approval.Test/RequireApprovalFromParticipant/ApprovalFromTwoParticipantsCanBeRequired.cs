using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace Approval.Test.RequireApprovalFromParticipant
{
    [TestClass]
    public class ApprovalFromTwoParticipantsCanBeRequired : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
        }

        protected override Command When()
        {
            return Command.NewRequireApprovalFromParticipant(Id, ParticipantId);
        }

        [TestMethod]
        public void NoExceptionWereThrown()
        {
            Assert.IsNull(this.Exception);
        }

        [TestMethod]
        public void OneEventIsRaised()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void AParticipantApprovalRequiredEventIsRaised()
        {
            Assert.AreEqual(Events[0], Event.NewParticipantApprovalRequired(Id, ParticipantId));
        }
    }
}
