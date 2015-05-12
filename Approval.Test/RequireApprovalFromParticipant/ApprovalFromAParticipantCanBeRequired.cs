using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace Approval.Test.RequireApprovalFromParticipant
{
    [TestClass]
    public class ApprovalFromAParticipantCanBeRequired : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override Command When()
        {
            return Command.NewRequireApprovalFromParticipant(Id, ParticipantId);
        }
        
        [TestMethod]
        public void ExpectedVersionIsNull()
        {
            Assert.IsNull(this.ExpectedVersion);
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
        public void AParticipantApprovalRequiredEventIsRaised()
        {
            Assert.AreEqual(Events[0], Event.NewParticipantApprovalRequired(Id, ParticipantId));
        }
    }
}
