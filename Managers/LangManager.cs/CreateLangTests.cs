using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Headless.Core.Payloads;
using Headless.DB.Tables;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Headless.Core.xUnit.Managers.LangManager.cs
{
    public class CreateLangTests
    {
        private Core.Managers.LangManager LangManager { get; set; }
        private Mock<DB.HeadlessDbContext> MockedDbContext { get; set; }
        private Mock<DbSet<Lang>> MockLangDbSet { get; set; }
        public CreateLangTests()
        {
            MockLangDbSet = new Mock<DbSet<Lang>>();

            MockedDbContext = new Mock<DB.HeadlessDbContext>();
            MockedDbContext.Setup(l => l.Languages).Returns(MockLangDbSet.Object);

            LangManager = new Core.Managers.LangManager(MockedDbContext.Object);

        }

        [Fact]
        public async void CreatePage_Returns_Lang_Object()
        {
            LanuagePL mockPL = new LanuagePL
            {
                LanguageIdentifier = "de-DE",
                LanguageName = "German"
            };

            Lang result = await LangManager.CreateLang(mockPL);

            MockedDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            MockLangDbSet.Verify(m => m.Add(It.IsAny<Lang>()), Times.Once());
            MockLangDbSet.Verify(m => m.Add(It.IsAny<Lang>()), Times.Once());
            Assert.IsType<Lang>(result);
        }
    }
}
