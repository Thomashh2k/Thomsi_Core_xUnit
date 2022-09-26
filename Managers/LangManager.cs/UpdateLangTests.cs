using Headless.DB.Tables;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headless.Core.xUnit.Managers.LangManager.cs
{
    public class UpdateLangTests
    {
        private Core.Managers.LangManager LangManager { get; set; }
        private Mock<DB.HeadlessDbContext> MockedDbContext { get; set; }
        private Mock<DbSet<Lang>> MockLangDbSet { get; set; }
        public UpdateLangTests()
        {
            MockLangDbSet = new Mock<DbSet<Lang>>();
            MockedDbContext = new Mock<DB.HeadlessDbContext>();
            MockedDbContext.Setup(l => l.Languages).Returns(MockLangDbSet.Object);

            LangManager = new Core.Managers.LangManager(MockedDbContext.Object);

        }

        [Fact]
        public async void UpdateLang_returns_updatedLang()
        {
            Guid langId = Guid.NewGuid();
            var returnedFromFind = new Lang
            {
                Id = langId,
                LanguageIdentifier = "de-DE",
                LanguageName = "German"
            };

            var updatedLangPL = new Lang
            {
                Id = langId,
                LanguageIdentifier = "de-AT",
                LanguageName = "Austrian"
            };

            MockLangDbSet.Setup(l => l.Find(It.IsAny<Guid>())).Returns(returnedFromFind);

            Lang result = await LangManager.UpdateLang(langId, updatedLangPL);

            MockedDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            MockLangDbSet.Verify(m => m.Find(It.IsAny<Guid>()), Times.Once());
            MockLangDbSet.Verify(m => m.Update(It.IsAny<Lang>()), Times.Once());
            Assert.Equal(updatedLangPL, result);
        }
    }
}
