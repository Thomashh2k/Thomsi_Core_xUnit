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
    public class DeletePageTests
    {
        private Core.Managers.PagesManager PagesManager { get; set; }
        private Mock<DB.HeadlessDbContext> MockedDbContext { get; set; }
        private Mock<DbSet<Page>> MockPageDbSet { get; set; }
        public DeletePageTests()
        {
            MockPageDbSet = new Mock<DbSet<Page>>();
            MockedDbContext = new Mock<DB.HeadlessDbContext>();
            MockedDbContext.Setup(l => l.Pages).Returns(MockPageDbSet.Object);

            PagesManager = new Core.Managers.PagesManager(MockedDbContext.Object);
        }

        [Fact]
        public async void DeletePage_successfully_removes_Page()
        {
            Guid langID = Guid.NewGuid();
            Lang lang = new Lang
            {
                Id = langID,
                LanguageIdentifier = "de-DE",
                LanguageName = "German"
            };

            Guid pageID = Guid.NewGuid();
            Page deletedPage = new Page
            {
                Id = pageID,
                Title = "TestTitle",
                Route = "/test",
            };


            MockPageDbSet.Setup(p => p.Find(It.IsAny<Guid>())).Returns(deletedPage);

            var result = await PagesManager.DeletePage(pageID);

            MockedDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            MockPageDbSet.Verify(m => m.Remove(It.IsAny<Page>()), Times.Once());
            MockPageDbSet.Verify(m => m.Find(It.IsAny<Guid>()), Times.Once());
            Assert.True(result);
        }
    }
}
