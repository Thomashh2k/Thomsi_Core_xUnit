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
    public class DeleteLangTests
    {
        private Core.Managers.LangManager LangManager { get; set; }
        private Mock<DB.HeadlessDbContext> MockedDbContext { get; set; }
        private Mock<DbSet<Lang>> mockLangDbSet { get; set; }
        public DeleteLangTests()
        {
            mockLangDbSet = new Mock<DbSet<Lang>>();
            MockedDbContext = new Mock<DB.HeadlessDbContext>();
            MockedDbContext.Setup(l => l.Languages).Returns(mockLangDbSet.Object);
            LangManager = new Core.Managers.LangManager(MockedDbContext.Object);
        }

        [Fact]
        public async void DeleteLang_Returns_true_if_lang_is_found()
        {
            var returnedFromFind = new Lang
            {
                Id = Guid.NewGuid(),
                LanguageIdentifier = "de-DE",
                LanguageName = "German"
            };
            mockLangDbSet.Setup(l => l.Find(It.IsAny<Guid>())).Returns(returnedFromFind);
            bool result = await LangManager.DeleteLang(returnedFromFind.Id);
            Assert.True(result);
        }
    }
}
