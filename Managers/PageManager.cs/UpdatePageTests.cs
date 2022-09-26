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
    public class UpdatePageTests
    {
        private Core.Managers.PagesManager PagesManager { get; set; }
        private Mock<DB.HeadlessDbContext> MockedDbContext { get; set; }
        private Mock<DbSet<Page>> MockPageDbSet { get; set; }
        private Mock<DbSet<Lang>> MockLangDbSet { get; set; }
        public UpdatePageTests()
        {
            MockPageDbSet = new Mock<DbSet<Page>>();

            MockLangDbSet = new Mock<DbSet<Lang>>();

            MockedDbContext = new Mock<DB.HeadlessDbContext>();
            MockedDbContext.Setup(dc => dc.Pages).Returns(MockPageDbSet.Object);
            MockedDbContext.Setup(dc => dc.Languages).Returns(MockLangDbSet.Object);

            PagesManager = new Core.Managers.PagesManager(MockedDbContext.Object);
        }

        [Fact]
        public async void UpdatePage_returns_updated_page()
        {
            Guid langID = Guid.NewGuid();
            Lang lang = new Lang
            {
                Id = langID,
                LanguageIdentifier = "de-DE",
                LanguageName = "German"
            };

            Guid pageId = Guid.NewGuid();
            Page pageReturnedbyFind = new Page
            {
                Id = pageId,
                Title = "TestTitle",
                Body = "<p>Test</p>",
                Route = "/test",
                LangId = langID
            };
            Page updatedPagePL = new Page
            {
                Id = pageId,
                Title = "TestTitle",
                Body = "<p>Updated Content</p>",
                Route = "/test",
                LangId = langID
            };
            MockPageDbSet.Setup(l => l.Find(It.IsAny<Guid>())).Returns(pageReturnedbyFind);

            var result = await PagesManager.UpdatePage(pageId, updatedPagePL);

            MockPageDbSet.Verify(m => m.Update(It.IsAny<Page>()), Times.Once());
            MockedDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            Assert.Equal(updatedPagePL.Body, updatedPagePL.Body);
        }
    }
}
