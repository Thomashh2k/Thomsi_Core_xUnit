using Headless.Core.Managers;
using Headless.Core.Payloads;
using Headless.DB.Tables;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headless.Core.xUnit.Managers.PageManager.cs
{
    public class CreatePageTests
    {
        private Core.Managers.PagesManager PagesManager { get; set; }
        private Mock<DB.HeadlessDbContext> MockedDbContext { get; set; }
        private Mock<DbSet<Page>> MockPageDbSet { get; set; }
        private Mock<DbSet<Lang>> MockLangDbSet { get; set; }
        public CreatePageTests()
        {
            MockPageDbSet = new Mock<DbSet<Page>>();
            MockLangDbSet = new Mock<DbSet<Lang>>();
            MockedDbContext = new Mock<DB.HeadlessDbContext>();
            MockedDbContext.Setup(l => l.Pages).Returns(MockPageDbSet.Object);
            MockedDbContext.Setup(l => l.Languages).Returns(MockLangDbSet.Object);
            PagesManager = new Core.Managers.PagesManager(MockedDbContext.Object);
        }

        [Fact]
        public async void CreatePage_returns_new_Page()
        {
            Guid langID = Guid.NewGuid();
            PagePL newPagePL = new PagePL
            {
                Title = "TestTitle",
                Body = "<p>Test</p>",
                Route = "/test-title",
                LangId = langID,
            };

            Lang returnedLang = new Lang
            {
                Id = langID,
                LanguageIdentifier = "de-DE",
                LanguageName = "German"
            };

            MockLangDbSet.Setup(l => l.Find(It.IsAny<Guid>())).Returns(returnedLang);

            Page result = await PagesManager.CreatePage(newPagePL);

            MockedDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsType<Page>(result);

        }
    }
}
