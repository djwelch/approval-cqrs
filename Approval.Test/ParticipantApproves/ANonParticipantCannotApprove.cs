using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.ParticipantApproves
{
    [TestClass]
    public class ANonParticipantCannotApprove : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        private readonly Guid ParticipantId = Guid.NewGuid();

        protected override IEnumerable<Event> Given()
        {
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
            yield return Event.NewParticipantApprovalRequired(Id, Guid.NewGuid());
        }

        protected override Command When()
        {
            return Command.NewParticipantApproves(Id, ParticipantId);
        }

        [TestMethod]
        public void ExceptionWasThrown()
        {
            Assert.IsInstanceOfType(this.Exception, typeof(ParticipantNotFoundException));
        }

    }
}
